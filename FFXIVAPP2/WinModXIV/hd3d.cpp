#include <windows.h>
#include <d3d9.h>
#include "WinModXIV.h"

typedef void* (WINAPI *Direct3DCreate8_Type)(UINT);
typedef void* (WINAPI *Direct3DCreate9_Type)(UINT);
typedef HRESULT (WINAPI *CreateDevice_Type)(void *, UINT, D3DDEVTYPE, HWND, DWORD, void *, void **);

void* WINAPI extDirect3DCreate8(UINT);
void* WINAPI extDirect3DCreate9(UINT);
HRESULT WINAPI extCreateDevice(void *, UINT, D3DDEVTYPE, HWND, DWORD, D3DPRESENT_PARAMETERS *, void **);

extern HWND hWnd;
extern DWORD dwWidth;
extern DWORD dwHeight;
extern DWORD dwFlags;
extern BOOL bActive;

Direct3DCreate8_Type pDirect3DCreate8 = 0;
Direct3DCreate9_Type pDirect3DCreate9 = 0;
CreateDevice_Type pCreateDevice;
DWORD dwD3DVersion;

int HookDirect3D(int version)
{
	HINSTANCE hinst;
	void *tmp;
	LPDIRECT3D9 lpd3d;

	switch(version)
	{
		case 0:
			tmp = HookAPI("d3d8.dll", "Direct3DCreate8", extDirect3DCreate8);
			if(tmp)
			{
				pDirect3DCreate8 = (Direct3DCreate8_Type)tmp;
			}
			tmp = HookAPI("d3d9.dll", "Direct3DCreate9", extDirect3DCreate9);
			if(tmp)
			{
				pDirect3DCreate9 = (Direct3DCreate9_Type)tmp;
			}
			break;
		case 8:
			hinst = LoadLibrary("d3d8.dll");
			pDirect3DCreate8 = (Direct3DCreate8_Type)GetProcAddress(hinst, "Direct3DCreate8");
			if(pDirect3DCreate8)
			{
				lpd3d = (LPDIRECT3D9)extDirect3DCreate8(220);
				if(lpd3d)
				{
					lpd3d->Release();
				}
			}
			break;
		case 9:
			hinst = LoadLibrary("d3d9.dll");
			pDirect3DCreate9 = (Direct3DCreate9_Type)GetProcAddress(hinst, "Direct3DCreate9");
			if(pDirect3DCreate9)
			{
				lpd3d = (LPDIRECT3D9)extDirect3DCreate9(31);
				if(lpd3d)
				{
					lpd3d->Release();
				}
			}
		break;
	}
	if(pDirect3DCreate8 || pDirect3DCreate9)
	{
		return 1;
	}
	return 0;
}

void* WINAPI extDirect3DCreate8(UINT sdkversion)
{
	void *lpd3d;
	void *tmp;

	dwD3DVersion = 8;
	lpd3d = (*pDirect3DCreate8)(sdkversion);
	if(!lpd3d)
	{
		return 0;
	}
	tmp = SetHook((void *)(*(DWORD *)lpd3d + 60), extCreateDevice);
	if(tmp)
	{
		pCreateDevice = (CreateDevice_Type)tmp;
	}

	OutTrace("Direct3DCreate8: SDKVERSION = %x pCreateDevice = %x\n", sdkversion, pCreateDevice);
	return lpd3d;
}

void* WINAPI extDirect3DCreate9(UINT sdkversion)
{
	void *lpd3d;
	void *tmp;

	dwD3DVersion = 9;
	lpd3d = (*pDirect3DCreate9)(sdkversion);
	if(!lpd3d)
	{
		return 0;
	}
	tmp = SetHook((void *)(*(DWORD *)lpd3d + 64), extCreateDevice);
	if(tmp)
	{
		pCreateDevice = (CreateDevice_Type)tmp;
	}

	OutTrace("Direct3DCreate9: SDKVERSION = %x pCreateDevice = %x\n", sdkversion, pCreateDevice);
	return lpd3d;
}

HRESULT WINAPI extCreateDevice(void *lpd3d, UINT adapter, D3DDEVTYPE devicetype, HWND hfocuswindow, DWORD behaviorflags, D3DPRESENT_PARAMETERS *ppresentparam, void **ppd3dd)
{
	HRESULT res;
	DWORD param[64], *tmp;
	D3DDISPLAYMODE mode;

	if(dwD3DVersion == 9)
	{
		memcpy(param, ppresentparam, 56);
		OutTrace("D3D9::CreateDevice\n");
	}
	else
	{
		memcpy(param, ppresentparam, 52);
		OutTrace("D3D8::CreateDevice\n");
	}

	hWnd = hfocuswindow;
	dwWidth = param[0];
	dwHeight = param[1];
	AdjustWindowFrame(hWnd, dwWidth, dwHeight);

	tmp = param;
	OutTrace("  Adapter = %i\n", adapter);
	OutTrace("  DeviceType = %i\n", devicetype);
	OutTrace("  hFocusWindow = 0x%x\n", hfocuswindow);
	OutTrace("  BehaviorFlags = 0x%x\n", behaviorflags);
	OutTrace("    BackBufferWidth = %i\n", *(tmp ++));
	OutTrace("    BackBufferHeight = %i\n", *(tmp ++));
	OutTrace("    BackBufferFormat = %i\n", *(tmp ++));
	OutTrace("    BackBufferCount = %i\n", *(tmp ++));
	OutTrace("    MultiSampleType = %i\n", *(tmp ++));
	if(dwD3DVersion == 9) OutTrace("    MultiSampleQuality = %i\n", *(tmp ++));
	OutTrace("    SwapEffect = 0x%x\n", *(tmp ++));
	OutTrace("    hDeviceWindow = 0x%x\n", *(tmp ++));
	OutTrace("    Windowed = %i\n", *(tmp ++));
	OutTrace("    EnableAutoDepthStencil = %i\n", *(tmp ++));
	OutTrace("    AutoDepthStencilFormat = %i\n", *(tmp ++));
	OutTrace("    Flags = 0x%x\n", *(tmp ++));
	OutTrace("    FullScreen_RefreshRateInHz = %i\n", *(tmp ++));
	OutTrace("    PresentationInterval = 0x%x\n", *(tmp ++));

	((LPDIRECT3D9)lpd3d)->GetAdapterDisplayMode(0, &mode);
	param[2] = mode.Format;

	//  0 UINT                BackBufferWidth;
	//  1 UINT                BackBufferHeight;
	//  2 D3DFORMAT           BackBufferFormat;
	//  3 UINT                BackBufferCount;
	//  4 D3DMULTISAMPLE_TYPE MultiSampleType;
	//  5 DWORD               MultiSampleQuality;
	//  6 D3DSWAPEFFECT       SwapEffect;
	//  7 HWND                hDeviceWindow;
	//  8 BOOL                Windowed;
	//  9 BOOL                EnableAutoDepthStencil;
	// 10 D3DFORMAT           AutoDepthStencilFormat;
	// 11 DWORD               Flags;
	// 12 UINT                FullScreen_RefreshRateInHz;
	// 13 UINT                PresentationInterval;

	if(dwD3DVersion == 9)
	{
		param[7] = 0;			//hDeviceWindow
		param[8] = 1;			//Windowed
		param[12] = 0;			//FullScreen_RefreshRateInHz;
		param[13] = D3DPRESENT_INTERVAL_DEFAULT;	//PresentationInterval
	}
	else
	{
		param[6] = 0;			//hDeviceWindow
		param[7] = 1;			//Windowed
		param[11] = 0;			//FullScreen_RefreshRateInHz;
		param[12] = D3DPRESENT_INTERVAL_DEFAULT;	//PresentationInterval
	}

	res = (*pCreateDevice)(lpd3d, 0, devicetype, hfocuswindow, behaviorflags, param, ppd3dd);
	if(res)
	{
		OutTrace("FAILED! %x\n", res);
		return res;
	}
	OutTrace("SUCCESS!\n");
	return 0;
}