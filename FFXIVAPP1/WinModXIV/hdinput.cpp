#include <windows.h>
#include <dinput.h>
#include "WinModXIV.h"

typedef HRESULT (WINAPI *QueryInterface_Type)(void *, REFIID, LPVOID *);
typedef HRESULT (WINAPI *DirectInputCreate_Type)(HINSTANCE, DWORD, LPDIRECTINPUT *, LPUNKNOWN);
typedef HRESULT (WINAPI *DirectInputCreateEx_Type)(HINSTANCE, DWORD, REFIID, LPVOID *, LPUNKNOWN);
typedef HRESULT (WINAPI *DICreateDevice_Type)(LPDIRECTINPUT, REFGUID, LPDIRECTINPUTDEVICE *, LPUNKNOWN);
typedef HRESULT (WINAPI *DICreateDeviceEx_Type)(LPDIRECTINPUT, REFGUID, REFIID, LPVOID *, LPUNKNOWN);
typedef HRESULT (WINAPI *GetDeviceData_Type)(LPDIRECTINPUTDEVICE, DWORD, LPVOID, LPDWORD, DWORD);
typedef HRESULT (WINAPI *GetDeviceState_Type)(LPDIRECTINPUTDEVICE, DWORD, LPDIMOUSESTATE);
typedef HRESULT (WINAPI *DISetCooperativeLevel_Type)(LPDIRECTINPUTDEVICE, HWND, DWORD);
typedef HRESULT (WINAPI *SetDataFormat_Type)(LPDIRECTINPUTDEVICE, LPCDIDATAFORMAT);

HRESULT WINAPI extDirectInputCreate(HINSTANCE, DWORD, LPDIRECTINPUT *, LPUNKNOWN);
HRESULT WINAPI extDirectInputCreateEx(HINSTANCE, DWORD, REFIID, LPVOID *, LPUNKNOWN);
HRESULT WINAPI extDirectInput8Create(HINSTANCE, DWORD, REFIID, LPVOID *, LPUNKNOWN);
HRESULT WINAPI extDICreateDevice(LPDIRECTINPUT, REFGUID, LPDIRECTINPUTDEVICE *, LPUNKNOWN);
HRESULT WINAPI extDICreateDeviceEx(LPDIRECTINPUT, REFGUID, REFIID, LPVOID *, LPUNKNOWN);
HRESULT WINAPI extGetDeviceData(LPDIRECTINPUTDEVICE, DWORD, LPVOID, LPDWORD, DWORD);
HRESULT WINAPI extGetDeviceState(LPDIRECTINPUTDEVICE, DWORD, LPDIMOUSESTATE);
HRESULT WINAPI extDISetCooperativeLevel(LPDIRECTINPUTDEVICE, HWND, DWORD);
HRESULT WINAPI extSetDataFormat(LPDIRECTINPUTDEVICE, LPCDIDATAFORMAT);
HRESULT WINAPI extQueryInterfaceI(void *, REFIID, LPVOID *);
void GetMousePosition(int *, int *);
void InitPosition(int, int, int, int, int, int);
	
extern DWORD dwFlags;
extern BOOL bActive;
extern HWND hWnd;
extern DWORD dwWidth;
extern DWORD dwHeight;

DirectInputCreate_Type pDirectInputCreate = 0;
DirectInputCreateEx_Type pDirectInputCreateEx = 0;
DICreateDevice_Type pDICreateDevice = 0;
DICreateDeviceEx_Type pDICreateDeviceEx = 0;
GetDeviceData_Type pGetDeviceData;
GetDeviceState_Type pGetDeviceState;
DISetCooperativeLevel_Type pDISetCooperativeLevel;
SetDataFormat_Type pSetDataFormat;
QueryInterface_Type pQueryInterfaceI;

BOOL bDInputAbs = 0;

int iCursorX;
int iCursorY;
int iCursorXBuf;
int iCursorYBuf;
int iCurMinX;
int iCurMinY;
int iCurMaxX;
int iCurMaxY;

int HookDirectInput(int version)
{
	HINSTANCE hinst;
	void *tmp;
	LPDIRECTINPUT lpdi;
	const GUID di7 = {0x9A4CB684,0x236D,0x11D3,0x8E,0x9D,0x00,0xC0,0x4F,0x68,0x44,0xAE};
	const GUID di8 = {0xBF798030,0x483A,0x4DA2,0xAA,0x99,0x5D,0x64,0xED,0x36,0x97,0x00};

	tmp = HookAPI("dinput.dll", "DirectInputCreateA", extDirectInputCreate);
	if(tmp)
	{
		pDirectInputCreate = (DirectInputCreate_Type)tmp;
	}
	tmp = HookAPI("dinput.dll", "DirectInputCreateW", extDirectInputCreate);
	if(tmp)
	{
		pDirectInputCreate = (DirectInputCreate_Type)tmp;
	}
	tmp = HookAPI("dinput.dll", "DirectInputCreateEx", extDirectInputCreateEx);
	if(tmp)
	{
		pDirectInputCreateEx = (DirectInputCreateEx_Type)tmp;
	}
	tmp = HookAPI("dinput8.dll", "DirectInput8Create", extDirectInput8Create);
	if(tmp)
	{
		pDirectInputCreateEx = (DirectInputCreateEx_Type)tmp;
	}
	if(!pDirectInputCreate && !pDirectInputCreateEx)
	{
		if(version < 8)
		{
			hinst = LoadLibrary("dinput.dll");
			pDirectInputCreate = (DirectInputCreate_Type)GetProcAddress(hinst, "DirectInputCreateA");
			if(pDirectInputCreate)
			{
				if(!extDirectInputCreate(GetModuleHandle(0), DIRECTINPUT_VERSION, &lpdi, 0))
				{
					lpdi->Release();
				}
				pDirectInputCreateEx = (DirectInputCreateEx_Type)GetProcAddress(hinst, "DirectInputCreateEx");
				if(pDirectInputCreateEx)
				{
					if(!extDirectInputCreateEx(GetModuleHandle(0), DIRECTINPUT_VERSION, di7, (void **)&lpdi, 0))
					{
						lpdi->Release();
					}
				}
			}
		}
		else
		{
			hinst = LoadLibrary("dinput8.dll");
			pDirectInputCreateEx = (DirectInputCreateEx_Type)GetProcAddress(hinst, "DirectInput8Create");
			if(pDirectInputCreateEx)
			{
				if(!extDirectInputCreateEx(GetModuleHandle(0), DIRECTINPUT_VERSION, di8, (void **)&lpdi, 0))
				{
					lpdi->Release();
				}
			}
		}
	}
	if(pDirectInputCreate || pDirectInputCreateEx)
	{
		return 1;
	}
	return 0;
}

HRESULT WINAPI extDirectInputCreate(HINSTANCE hinst, DWORD dwversion, LPDIRECTINPUT *lplpdi, LPUNKNOWN pu)
{
	HRESULT res;
	void *tmp;

	OutTrace("DirectInputCreate: dwVersion = %x\n", dwversion);

	res = (*pDirectInputCreate)(hinst, dwversion, lplpdi, pu);
	if(res)
	{
		return res;
	}
	tmp = SetHook((void *)(**(DWORD **)lplpdi + 12), extDICreateDevice);
	if(tmp)
	{
		pDICreateDevice = (DICreateDevice_Type)tmp;
	}
	tmp = SetHook((void *)(**(DWORD **)lplpdi), extQueryInterfaceI);
	if(tmp)
	{
		pQueryInterfaceI = (QueryInterface_Type)tmp;
	}
	return 0;
}

HRESULT WINAPI extDirectInputCreateEx(HINSTANCE hinst, DWORD dwversion, REFIID riidltf, LPVOID *ppvout, LPUNKNOWN pu)
{
	HRESULT res;
	void *tmp;

	OutTrace("DirectInputCreateEx: dwVersion = %x REFIID = %x\n", dwversion, riidltf.Data1);

	res = (*pDirectInputCreateEx)(hinst, dwversion, riidltf, ppvout, pu);
	if(res)
	{
		return res;
	}
	tmp = SetHook((void *)(**(DWORD **)ppvout + 12), extDICreateDevice);
	if(tmp)
	{
		pDICreateDevice = (DICreateDevice_Type)tmp;
	}
	tmp = SetHook((void *)(**(DWORD **)ppvout + 36), extDICreateDeviceEx);
	if(tmp)
	{
		pDICreateDeviceEx = (DICreateDeviceEx_Type)tmp;
	}
	return 0;
}

HRESULT WINAPI extQueryInterfaceI(void * lpdi, REFIID riid, LPVOID *obp)
{
	HRESULT res;
	void *tmp;

	OutTrace("lpDI->QueryInterface: REFIID = %x\n", riid.Data1);

	res = (*pQueryInterfaceI)(lpdi, riid, obp);
	if(res)
	{
		return res;
	}

	switch(riid.Data1)
	{
		case 0x5944E662:		//DirectInput2A
		case 0x5944E663:		//DirectInput2W
			tmp = SetHook((void *)(**(DWORD **)obp + 12), extDICreateDevice);
			if(tmp)
			{
				pDICreateDevice = (DICreateDevice_Type)tmp;
			}
			break;
	}
	return 0;
}

HRESULT WINAPI extDirectInput8Create(HINSTANCE hinst, DWORD dwversion, REFIID riidltf, LPVOID *ppvout, LPUNKNOWN pu)
{
	HRESULT res;
	void *tmp;

	OutTrace("DirectInput8Create: dwVersion = %x REFIID = %x\n", dwversion, riidltf.Data1);

	res = (*pDirectInputCreateEx)(hinst, dwversion, riidltf, ppvout, pu);
	if(res)
	{
		return res;
	}
	tmp = SetHook((void *)(**(DWORD **)ppvout + 12), extDICreateDevice);
	if(tmp)
	{
		pDICreateDevice = (DICreateDevice_Type)tmp;
	}
	return 0;
}

HRESULT WINAPI extDICreateDevice(LPDIRECTINPUT lpdi, REFGUID rguid, LPDIRECTINPUTDEVICE *lplpdid, LPUNKNOWN pu)
{
	HRESULT res;
	void *tmp;

	OutTrace("lpDI->CreateDevice: REFGUID = %x\n", rguid.Data1);

	res = (*pDICreateDevice)(lpdi, rguid, lplpdid, pu);
	if(res)
	{
		return res;
	}
	tmp = SetHook((void *)(**(DWORD **)lplpdid + 36), extGetDeviceState);
	if(tmp)
	{
		pGetDeviceState = (GetDeviceState_Type)tmp;
	}
	tmp = SetHook((void *)(**(DWORD **)lplpdid + 40), extGetDeviceData);
	if(tmp)
	{
		pGetDeviceData = (GetDeviceData_Type)tmp;
	}
	tmp = SetHook((void *)(**(DWORD **)lplpdid + 44), extSetDataFormat);
	if(tmp)
	{
		pSetDataFormat = (SetDataFormat_Type)tmp;
	}
	tmp = SetHook((void *)(**(DWORD **)lplpdid + 52), extDISetCooperativeLevel);
	if(tmp)
	{
		pDISetCooperativeLevel = (DISetCooperativeLevel_Type)tmp;
	}
	return 0;
}

HRESULT WINAPI extDICreateDeviceEx(LPDIRECTINPUT lpdi, REFGUID rguid, REFIID riid, LPVOID *pvout, LPUNKNOWN pu)
{
	HRESULT res;
	void *tmp;

	OutTrace("lpDI->CreateDeviceEx: GUID = %x REFIID = %x\n", rguid.Data1, riid.Data1);

	res = (*pDICreateDeviceEx)(lpdi, rguid, riid, pvout, pu);
	if(res)
	{
		return res;
	}
	tmp = SetHook((void *)(**(DWORD **)pvout + 36), extGetDeviceState);
	if(tmp)
	{
		pGetDeviceState = (GetDeviceState_Type)tmp;
	}
	tmp = SetHook((void *)(**(DWORD **)pvout + 40), extGetDeviceData);
	if(tmp)
	{
		pGetDeviceData = (GetDeviceData_Type)tmp;
	}
	tmp = SetHook((void *)(**(DWORD **)pvout + 44), extSetDataFormat);
	if(tmp)
	{
		pSetDataFormat = (SetDataFormat_Type)tmp;
	}
	tmp = SetHook((void *)(**(DWORD **)pvout + 52), extDISetCooperativeLevel);
	if(tmp)
	{
		pDISetCooperativeLevel = (DISetCooperativeLevel_Type)tmp;
	}
	return 0;
}

HRESULT WINAPI extGetDeviceData(LPDIRECTINPUTDEVICE lpdid, DWORD cbdata, LPVOID rgdod, LPDWORD pdwinout, DWORD dwflags)
{
	HRESULT res;
	BYTE *tmp;
	int i;
	POINT p;

	OutTrace("GetDeviceData cbdata:%i\n", cbdata);

	res = (*pGetDeviceData)(lpdid, cbdata, rgdod, pdwinout, dwflags);
	if(res)
	{
		return res;
	}

	if(!bActive)
	{
		*pdwinout = 0;
	}
	GetMousePosition((int *)&p.x, (int *)&p.y);
	if(cbdata == 20 || cbdata == 24)
	{
		tmp = (BYTE *)rgdod;
		for(i = 0; i < (int)*pdwinout; i ++)
		{
			if(((LPDIDEVICEOBJECTDATA)tmp)->dwOfs == DIMOFS_X)
			{
				((LPDIDEVICEOBJECTDATA)tmp)->dwData = p.x;
				if(!bDInputAbs)
				{
					if(p.x < iCurMinX)
					{
						p.x = iCurMinX;
					}
					if(p.x > iCurMaxX)
					{
						p.x = iCurMaxX;
					}
					((LPDIDEVICEOBJECTDATA)tmp)->dwData = p.x - iCursorXBuf;
					iCursorXBuf = p.x;
				}
			}
			if(((LPDIDEVICEOBJECTDATA)tmp)->dwOfs == DIMOFS_Y)
			{
				((LPDIDEVICEOBJECTDATA)tmp)->dwData = p.y;
				if(!bDInputAbs)
				{
					if(p.y < iCurMinY) 
					{
						p.y = iCurMinY;
					}
					if(p.y > iCurMaxY)
					{
						p.y = iCurMaxY;
					}
					((LPDIDEVICEOBJECTDATA)tmp)->dwData = p.y - iCursorYBuf;
					iCursorYBuf = p.y;
				}
			}
			tmp += cbdata;
		}
	}
	return 0;
}

HRESULT WINAPI extGetDeviceState(LPDIRECTINPUTDEVICE lpdid, DWORD cbdata, LPDIMOUSESTATE lpvdata)
{
	HRESULT res;
	POINT p = {0, 0};

	OutTrace("GetDeviceState cbData:%i %i\n", cbdata, bActive);

	res = (*pGetDeviceState)(lpdid, cbdata, lpvdata);
	if(res)
	{
		return res;
	}
	if(cbdata == sizeof(DIMOUSESTATE) || cbdata == sizeof(DIMOUSESTATE2))
	{
		GetMousePosition((int *)&p.x, (int *)&p.y);
		lpvdata->lX = p.x;
		lpvdata->lY = p.y;
		if(!bDInputAbs)
		{
			if(p.x < iCurMinX) 
			{
				p.x = iCurMinX;
			}
			if(p.x > iCurMaxX) 
			{
				p.x = iCurMaxX;
			}
			if(p.y < iCurMinY)
			{
				p.y = iCurMinY;
			}
			if(p.y > iCurMaxY) 
			{
				p.y = iCurMaxY;
			}
			lpvdata->lX = p.x - iCursorX;
			lpvdata->lY = p.y - iCursorY;
			iCursorX = p.x;
			iCursorY = p.y;
		}
		if(!bActive)
		{
			lpvdata->lZ = 0;
			*(DWORD *)lpvdata->rgbButtons = 0;
		}
	}
	
	if(cbdata == 256 && !bActive)
	{
		ZeroMemory(lpvdata, 256);
	}
	return 0;
}
	
HRESULT WINAPI extSetDataFormat(LPDIRECTINPUTDEVICE lpdid, LPCDIDATAFORMAT lpdf)
{
	OutTrace("SetDataFormat: flags = 0x%x\n", lpdf->dwFlags);

	if(lpdf->dwFlags & DIDF_ABSAXIS) 
	{
		bDInputAbs = 1;
	}
	if(lpdf->dwFlags & DIDF_RELAXIS)
	{
		bDInputAbs = 0;
	}
	return (*pSetDataFormat)(lpdid, lpdf);
}

HRESULT WINAPI extDISetCooperativeLevel(LPDIRECTINPUTDEVICE lpdid, HWND hwnd, DWORD dwflags)
{
	OutTrace("lpDI->SetCooperativeLevel\n");

	dwflags = DISCL_NONEXCLUSIVE | DISCL_BACKGROUND;
	return (*pDISetCooperativeLevel)(lpdid, hwnd, dwflags);
}

void GetMousePosition(int *x, int *y)
{
	POINT p;
	RECT r;

	GetCursorPos(&p);
	ScreenToClient(hWnd, &p);
	GetClientRect(hWnd, &r);

	p.x *= dwWidth;
	p.x /= r.right;
	p.y *= dwHeight;
	p.y /= r.bottom;

	if(p.x < 0)
	{
		p.x = 0;
	}
	if(p.x >= (int)dwWidth)
	{
		p.x = dwWidth - 1;
	}
	if(p.y < 0)
	{
		p.y = 0;
	}
	if(p.y >= (int)dwHeight)
	{
		p.y = dwHeight - 1;
	}

	*x = p.x;
	*y = p.y;
}

void InitPosition(int x, int y, int minx, int miny, int maxx, int maxy)
{
	iCursorX = x;
	iCursorY = y;
	iCursorXBuf = x;
	iCursorYBuf = y;
	iCurMinX = minx;
	iCurMinY = miny;
	iCurMaxX = maxx;
	iCurMaxY = maxy;
}

