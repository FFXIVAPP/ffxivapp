#include <windows.h>
#include <ddraw.h>
#include "WinModXIV.h"

typedef HRESULT (WINAPI *DirectDrawCreate_Type)(GUID *, LPDIRECTDRAW *, IUnknown *);
typedef HRESULT (WINAPI *DirectDrawCreateEx_Type)(GUID *, LPDIRECTDRAW *, REFIID, IUnknown *);
typedef HRESULT (WINAPI *QueryInterface_Type)(void *, REFIID, LPVOID *);
typedef HRESULT (WINAPI *CreateSurface_Type)(LPDIRECTDRAW, DDSURFACEDESC2 *, LPDIRECTDRAWSURFACE *, void *);
typedef HRESULT (WINAPI *SetCooperativeLevel_Type)(void *, HWND, DWORD);
typedef HRESULT (WINAPI *GetAttachedSurface_Type)(void *, DDSCAPS *, LPDIRECTDRAWSURFACE *);
typedef HRESULT (WINAPI *SetDisplayMode_Type)(LPDIRECTDRAW, DWORD, DWORD, DWORD, DWORD, DWORD);
typedef HRESULT (WINAPI *SetDisplayMode1_Type)(LPDIRECTDRAW, DWORD, DWORD, DWORD);
typedef HRESULT (WINAPI *CreatePalette_Type)(LPDIRECTDRAW, DWORD, LPPALETTEENTRY, LPDIRECTDRAWPALETTE *, IUnknown *);
typedef HRESULT (WINAPI *Blt_Type)(LPDIRECTDRAWSURFACE, LPRECT, LPDIRECTDRAWSURFACE, LPRECT, DWORD, LPDDBLTFX);
typedef HRESULT (WINAPI *SetPalette_Type)(LPDIRECTDRAWSURFACE, LPDIRECTDRAWPALETTE);
typedef HRESULT (WINAPI *GetPalette_Type)(LPDIRECTDRAWSURFACE, LPDIRECTDRAWPALETTE *);
typedef HRESULT (WINAPI *SetEntries_Type)(LPDIRECTDRAWPALETTE, DWORD, DWORD, DWORD, LPPALETTEENTRY);
typedef HRESULT (WINAPI *GetDisplayMode_Type)(LPDIRECTDRAW, LPDDSURFACEDESC);
typedef HRESULT (WINAPI *WaitForVerticalBlank_Type)(LPDIRECTDRAW, DWORD, HANDLE);
typedef HRESULT (WINAPI *SetClipper_Type)(LPDIRECTDRAWSURFACE, LPDIRECTDRAWCLIPPER);

HRESULT WINAPI extDirectDrawCreate(GUID FAR *, LPDIRECTDRAW FAR *, IUnknown FAR *);
HRESULT WINAPI extDirectDrawCreateEx(GUID FAR *, LPDIRECTDRAW FAR *, REFIID, IUnknown FAR *);
HRESULT WINAPI extQueryInterfaceD(void *, REFIID, LPVOID *);
HRESULT WINAPI extQueryInterfaceS(void *, REFIID, LPVOID *);
HRESULT WINAPI extSetDisplayMode(LPDIRECTDRAW, DWORD, DWORD, DWORD, DWORD, DWORD);
HRESULT WINAPI extSetDisplayMode1(LPDIRECTDRAW, DWORD, DWORD, DWORD);
HRESULT WINAPI extCreateSurface(LPDIRECTDRAW, DDSURFACEDESC2 *, LPDIRECTDRAWSURFACE *, void *);
HRESULT WINAPI extSetCooperativeLevel(void *, HWND, DWORD);
HRESULT WINAPI extGetAttachedSurface(void *, DDSCAPS *, LPDIRECTDRAWSURFACE *);
HRESULT WINAPI extFlip(LPDIRECTDRAWSURFACE, LPDIRECTDRAWSURFACE, DWORD);
HRESULT WINAPI extBltFast(LPDIRECTDRAWSURFACE, DWORD, DWORD, LPDIRECTDRAWSURFACE, LPRECT, DWORD);
HRESULT WINAPI extCreatePalette(LPDIRECTDRAW, DWORD, LPPALETTEENTRY, LPDIRECTDRAWPALETTE *, IUnknown *);
HRESULT WINAPI extSetEntries(LPDIRECTDRAWPALETTE, DWORD, DWORD, DWORD, LPPALETTEENTRY);
HRESULT WINAPI extBlt(LPDIRECTDRAWSURFACE, LPRECT, LPDIRECTDRAWSURFACE, LPRECT, DWORD, LPDDBLTFX);
HRESULT WINAPI extGetPalette(LPDIRECTDRAWSURFACE, LPDIRECTDRAWPALETTE *);
HRESULT WINAPI extSetPalette(LPDIRECTDRAWSURFACE, LPDIRECTDRAWPALETTE);
HRESULT WINAPI extGetDisplayMode(LPDIRECTDRAW, LPDDSURFACEDESC);
HRESULT WINAPI extWaitForVerticalBlank(LPDIRECTDRAW, DWORD, HANDLE);
HRESULT WINAPI extSetClipper(LPDIRECTDRAWSURFACE, LPDIRECTDRAWCLIPPER);

extern HWND hWnd;
extern DWORD dwWidth;
extern DWORD dwHeight;
DWORD dwDDVersion;
extern DWORD dwFlags;
extern BOOL bActive;

DirectDrawCreate_Type pDirectDrawCreate = 0;
DirectDrawCreateEx_Type pDirectDrawCreateEx = 0;
QueryInterface_Type pQueryInterfaceD;
QueryInterface_Type pQueryInterfaceS;
CreateSurface_Type pCreateSurface;
CreateSurface_Type pCreateSurfaceOld;
SetCooperativeLevel_Type pSetCooperativeLevel;
GetAttachedSurface_Type pGetAttachedSurface;
SetDisplayMode_Type pSetDisplayMode;
SetDisplayMode1_Type pSetDisplayMode1;
CreatePalette_Type pCreatePalette;
Blt_Type pBlt;
GetPalette_Type pGetPalette;
SetPalette_Type pSetPalette;
SetEntries_Type pSetEntries;
GetDisplayMode_Type pGetDisplayMode;
WaitForVerticalBlank_Type pWaitForVerticalBlank;
SetClipper_Type pSetClipper;

LPDIRECTDRAWSURFACE lpDDSBack, lpDDSPrim, lpDDSEmu_Prim, lpDDSEmu_Back;
LPDIRECTDRAWCLIPPER lpDDC;
LPDIRECTDRAWPALETTE lpDDP = 0;
LPDIRECTDRAW lpDD;
DWORD PaletteEntries[256];

int HookDirectDraw(int version)
{
	HINSTANCE hinst;
	void *tmp;
	const GUID dd7 = {0x15e65ec0,0x3b9c,0x11d2,0xb9,0x2f,0x00,0x60,0x97,0x97,0xea,0x5b};
	LPDIRECTDRAW lpdd;

	switch(version)
	{
		case 0:
			tmp = HookAPI("ddraw.dll", "DirectDrawCreate", extDirectDrawCreate);
			if(tmp)
			{
				pDirectDrawCreate = (DirectDrawCreate_Type)tmp;
			}
			tmp = HookAPI("ddraw.dll", "DirectDrawCreateEx", extDirectDrawCreateEx);
			if(tmp)
			{
				pDirectDrawCreateEx = (DirectDrawCreateEx_Type)tmp;
			}
			break;
		case 1:
		case 2:
		case 3:
		case 5:
		case 6:
			hinst = LoadLibrary("ddraw.dll");
			pDirectDrawCreate = (DirectDrawCreate_Type)GetProcAddress(hinst, "DirectDrawCreate");
			if(pDirectDrawCreate)
			{
				if(!extDirectDrawCreate(0, &lpdd, 0))
				{
					lpdd->Release();
				}
			}
			break;
		case 7:
			hinst = LoadLibrary("ddraw.dll");
			pDirectDrawCreateEx = (DirectDrawCreateEx_Type)GetProcAddress(hinst, "DirectDrawCreateEx");
			if(pDirectDrawCreateEx)
			{
				if(!extDirectDrawCreateEx(0, &lpdd, dd7, 0))
				{
					lpdd->Release();
				}
			}
			break;
	}
	if(pDirectDrawCreate || pDirectDrawCreateEx)
	{
		return 1;
	}
	return 0;
}

HRESULT WINAPI extDirectDrawCreate(GUID FAR *lpguid, LPDIRECTDRAW FAR *lplpdd, IUnknown FAR *pu)
{
	HRESULT res;
	void *tmp;

	dwDDVersion = 1;
	res = (*pDirectDrawCreate)(lpguid, lplpdd, pu);
	if(res)
	{
		return res;
	}
	tmp = SetHook((void *)(**(DWORD **)lplpdd), extQueryInterfaceD);
	if(tmp)
	{
		pQueryInterfaceD = (QueryInterface_Type)tmp;
	}
	tmp = SetHook((void *)(**(DWORD **)lplpdd + 84), extSetDisplayMode1);
	if(tmp)
	{
		pSetDisplayMode1 = (SetDisplayMode1_Type)tmp;
	}
	tmp = SetHook((void *)(**(DWORD **)lplpdd + 24), extCreateSurface);
	if(tmp)
	{
		pCreateSurface = (CreateSurface_Type)tmp;
	}
	tmp = SetHook((void *)(**(DWORD **)lplpdd + 80), extSetCooperativeLevel);
	if(tmp)
	{
		pSetCooperativeLevel = (SetCooperativeLevel_Type)tmp;
	}
	tmp = SetHook((void *)(**(DWORD **)lplpdd + 20), extCreatePalette);
	if(tmp)
	{
		pCreatePalette = (CreatePalette_Type)tmp;
	}
	tmp = SetHook((void *)(**(DWORD **)lplpdd + 88), extWaitForVerticalBlank);
	if(tmp)
	{
		pWaitForVerticalBlank = (WaitForVerticalBlank_Type)tmp;
	}
	tmp = SetHook((void *)(**(DWORD **)lplpdd + 48), extGetDisplayMode);
	if(tmp)
	{
		pGetDisplayMode = (GetDisplayMode_Type)tmp;
	}

	OutTrace("DirectDrawCreate: pCreateSurface = %x\n", pCreateSurface);
	return 0;
}

HRESULT WINAPI extDirectDrawCreateEx(GUID FAR *lpguid, LPDIRECTDRAW FAR *lplpdd, REFIID iid, IUnknown FAR *pu)
{
	HRESULT res;
	void *tmp;

	dwDDVersion = 7;
	res = (*pDirectDrawCreateEx)(lpguid, lplpdd, iid, pu);
	if(res)
	{
		return res;
	}
	tmp = SetHook((void *)(**(DWORD **)lplpdd + 84), extSetDisplayMode);
	if(tmp)
	{
		pSetDisplayMode = (SetDisplayMode_Type)tmp;
	}
	tmp = SetHook((void *)(**(DWORD **)lplpdd + 24), extCreateSurface);
	if(tmp)
	{
		pCreateSurface = (CreateSurface_Type)tmp;
	}
	tmp = SetHook((void *)(**(DWORD **)lplpdd + 80), extSetCooperativeLevel);
	if(tmp)
	{
		pSetCooperativeLevel = (SetCooperativeLevel_Type)tmp;
	}
	tmp = SetHook((void *)(**(DWORD **)lplpdd + 20), extCreatePalette);
	if(tmp)
	{
		pCreatePalette = (CreatePalette_Type)tmp;
	}
	tmp = SetHook((void *)(**(DWORD **)lplpdd + 48), extGetDisplayMode);
	if(tmp)
	{
		pGetDisplayMode = (GetDisplayMode_Type)tmp;
	}
	tmp = SetHook((void *)(**(DWORD **)lplpdd + 88), extWaitForVerticalBlank);
	if(tmp)
	{
		pWaitForVerticalBlank = (WaitForVerticalBlank_Type)tmp;
	}

	OutTrace("DirectDrawCreateEx: REFIID = %x pCreateSurface = %x\n", iid.Data1, pCreateSurface);
	return 0;
}

HRESULT WINAPI extQueryInterfaceD(void *lpdd, REFIID riid, LPVOID *obp)
{
	HRESULT res;
	void *tmp;

	res = (*pQueryInterfaceD)(lpdd, riid, obp);

	if(res)
	{
		return res;
	}

	switch(riid.Data1)
	{
		case 0x9c59509a:		//DirectDraw4
			dwDDVersion = 4;
			break;
		case 0xB3A6F3E0:		//DirectDraw2
			if(dwDDVersion != 4)
			{
				dwDDVersion = 2;
			}
			tmp = SetHook((void *)(**(DWORD **)obp + 84), extSetDisplayMode);
			if(tmp)
			{
				pSetDisplayMode = (SetDisplayMode_Type)tmp;
			}
			tmp = SetHook((void *)(**(DWORD **)obp + 24), extCreateSurface);
			if(tmp)
			{
				pCreateSurfaceOld = pCreateSurface;
				pCreateSurface = (CreateSurface_Type)tmp;
			}
			tmp = SetHook((void *)(**(DWORD **)obp + 80), extSetCooperativeLevel);
			if(tmp)
			{
				pSetCooperativeLevel = (SetCooperativeLevel_Type)tmp;
			}
			tmp = SetHook((void *)(**(DWORD **)obp + 20), extCreatePalette);
			if(tmp)
			{
				pCreatePalette = (CreatePalette_Type)tmp;
			}
			tmp = SetHook((void *)(**(DWORD **)obp + 48), extGetDisplayMode);
			if(tmp)
			{
				pGetDisplayMode = (GetDisplayMode_Type)tmp;
			}
			tmp = SetHook((void *)(**(DWORD **)obp + 88), extWaitForVerticalBlank);
			if(tmp)
			{
				pWaitForVerticalBlank = (WaitForVerticalBlank_Type)tmp;
			}
			break;
	}
	
	OutTrace("lpDD->QueryInterface: lpDD = %x REFIID = %x pCreateSurface = %x\n", lpdd, riid.Data1, pCreateSurface);
	return 0;
}

HRESULT WINAPI extQueryInterfaceS(void *lpdds, REFIID riid, LPVOID *obp)
{
	HRESULT res;
	void *tmp;

	res = (*pQueryInterfaceS)(lpdds, riid, obp);
	if(res)
	{
		return res;
	}
	switch(riid.Data1)
	{
		case 0xDA044E00:		//DDSurface3
			dwDDVersion = 3;
			if(lpdds == lpDDSPrim)
			{
				lpDDSPrim = (LPDIRECTDRAWSURFACE)*obp;
			}
			tmp = SetHook((void *)(**(DWORD **)obp + 48), extGetAttachedSurface);
			if(tmp)
			{
				pGetAttachedSurface = (GetAttachedSurface_Type)tmp;
			}
			SetHook((void *)(**(DWORD **)obp + 44), extFlip);
			tmp = SetHook((void *)(**(DWORD **)obp + 20), extBlt);
			if(tmp)
			{
				pBlt = (Blt_Type)tmp;
			}
			SetHook((void *)(**(DWORD **)obp + 28), extBltFast);
			tmp = SetHook((void *)(**(DWORD **)obp + 80), extGetPalette);
			if(tmp)
			{
				pGetPalette = (GetPalette_Type)tmp;
			}
			tmp = SetHook((void *)(**(DWORD **)obp + 124), extSetPalette);
			if(tmp)
			{
				pSetPalette = (SetPalette_Type)tmp;
			}
			tmp = SetHook((void *)(*(DWORD **)obp + 112), extSetClipper);
			if(tmp)
			{
				pSetClipper = (SetClipper_Type)tmp;
			}
			break;
	}
	
	OutTrace("lpDDS->QueryInterface: lpdds = %x REFIID = %x\n", lpdds, riid.Data1);
	return 0;
}

HRESULT WINAPI extSetDisplayMode(LPDIRECTDRAW lpdd, DWORD dwwidth, DWORD dwheight, DWORD dwbpp, DWORD dwrefreshrate, DWORD dwflags)
{
	DDSURFACEDESC2 ddsd;
	HRESULT res = 0;

	OutTrace("SetDisplayMode: dwWidth = %i dwHeight = %i dwBPP = %i dwRefresh = %i dwFlags = %x\n", dwwidth, dwheight, dwbpp, dwrefreshrate, dwflags);

	dwWidth = dwwidth;
	dwHeight = dwheight;
	if(hWnd)
	{
		AdjustWindowFrame(hWnd, dwwidth, dwheight);
	}

	if(dwFlags & EMULATEPAL)
	{
		dwbpp = 32;
	}

	ZeroMemory(&ddsd, sizeof(ddsd));
	ddsd.dwSize = (dwDDVersion < 4) ? sizeof(DDSURFACEDESC) : sizeof(DDSURFACEDESC2);
	ddsd.dwFlags = DDSD_WIDTH | DDSD_HEIGHT | DDSD_PIXELFORMAT | DDSD_REFRESHRATE;
	ddsd.ddpfPixelFormat.dwSize = sizeof(DDPIXELFORMAT);
	ddsd.ddpfPixelFormat.dwFlags = DDPF_RGB;

	(*pGetDisplayMode)(lpdd, (LPDDSURFACEDESC)&ddsd);
	if(ddsd.ddpfPixelFormat.dwRGBBitCount != dwbpp)
	{
		res = (*pSetDisplayMode)(lpdd, ddsd.dwWidth, ddsd.dwHeight, dwbpp, ddsd.dwRefreshRate, 0);
	}
	return 0;
}

HRESULT WINAPI extSetDisplayMode1(LPDIRECTDRAW lpdd, DWORD dwwidth, DWORD dwheight, DWORD dwbpp)
{
	DDSURFACEDESC2 ddsd;
	HRESULT res = 0;

	OutTrace("SetDisplayMode1: dwWidth = %i dwHeight = %i dwBPP = %i\n", dwwidth, dwheight, dwbpp);

	dwWidth = dwwidth;
	dwHeight = dwheight;
	if(hWnd)
	{
		AdjustWindowFrame(hWnd, dwwidth, dwheight);
	}

	if(dwFlags & EMULATEPAL)
	{
		dwbpp = 32;
	}
	ZeroMemory(&ddsd, sizeof(ddsd));
	ddsd.dwSize = (dwDDVersion < 4) ? sizeof(DDSURFACEDESC) : sizeof(DDSURFACEDESC2);
	ddsd.dwFlags = DDSD_WIDTH | DDSD_HEIGHT | DDSD_PIXELFORMAT;
	ddsd.ddpfPixelFormat.dwSize = sizeof(DDPIXELFORMAT);
	ddsd.ddpfPixelFormat.dwFlags = DDPF_RGB;
	(*pGetDisplayMode)(lpdd, (LPDDSURFACEDESC)&ddsd);
	if(ddsd.ddpfPixelFormat.dwRGBBitCount != dwbpp)
	{
		res = (*pSetDisplayMode1)(lpdd, ddsd.dwWidth, ddsd.dwHeight, dwbpp);
	}
	return res;
}

HRESULT WINAPI extGetDisplayMode(LPDIRECTDRAW lpdd, LPDDSURFACEDESC lpddsd)
{
	OutTrace("GetDisplayMode\n");

	(*pGetDisplayMode)(lpdd, lpddsd);
	lpddsd->dwWidth = dwWidth;
	lpddsd->dwHeight = dwHeight;
	if(dwFlags & EMULATEPAL)
	{
		lpddsd->ddpfPixelFormat.dwFlags |= DDPF_PALETTEINDEXED8;
		lpddsd->ddpfPixelFormat.dwRGBBitCount = 8;
		lpddsd->ddsCaps.dwCaps |= DDSCAPS_PALETTE;
	}
	return 0;
}

HRESULT WINAPI extSetCooperativeLevel(void *lpdd, HWND hwnd, DWORD dwflags)
{
	hWnd = hwnd;
	OutTrace("SetCooperativeLevel: dwFlags = %x\n", dwflags);

	if(dwWidth)
	{
		AdjustWindowFrame(hwnd, dwWidth, dwHeight);
	}
	return (*pSetCooperativeLevel)(lpdd, hwnd, DDSCL_NORMAL);
}

HRESULT WINAPI extCreateSurface(LPDIRECTDRAW lpdd, DDSURFACEDESC2 *lpddsd, LPDIRECTDRAWSURFACE *lplpdds, void *pu)
{
	void *tmp;
	HRESULT res;
	DDSURFACEDESC2 ddsd;
	lpDD = lpdd;
	OutTrace("CreateSurface: lpDD = %x Flags = %x Width = %i Height = %i Caps = %x", lpdd, lpddsd->dwFlags, lpddsd->dwWidth, lpddsd->dwHeight, lpddsd->ddsCaps.dwCaps);

	memcpy(&ddsd, lpddsd, lpddsd->dwSize);

	if(ddsd.dwFlags & DDSD_CAPS && ddsd.ddsCaps.dwCaps & DDSCAPS_PRIMARYSURFACE)
	{
		if(dwFlags & EMULATEPAL)
		{
			ddsd.dwFlags = DDSD_CAPS | DDSD_WIDTH | DDSD_HEIGHT | DDSD_PIXELFORMAT;
			ddsd.ddsCaps.dwCaps = DDSCAPS_SYSTEMMEMORY | DDSCAPS_OFFSCREENPLAIN;
			ddsd.dwWidth = dwWidth;
			ddsd.dwHeight = dwHeight;
			ddsd.ddpfPixelFormat.dwSize = sizeof(DDPIXELFORMAT);
			ddsd.ddpfPixelFormat.dwFlags = DDPF_RGB | DDPF_PALETTEINDEXED8;
			ddsd.ddpfPixelFormat.dwRGBBitCount = 8;
			(*pCreateSurface)(lpdd, &ddsd, lplpdds, 0);
			lpDDSPrim = *lplpdds;
			(*pCreateSurface)(lpdd, &ddsd, &lpDDSBack, 0);
			ddsd.dwFlags = DDSD_CAPS;
			ddsd.ddsCaps.dwCaps = DDSCAPS_PRIMARYSURFACE;
			(*pCreateSurface)(lpdd, &ddsd, &lpDDSEmu_Prim, 0);
			lpdd->CreateClipper(0, &lpDDC, NULL);
			lpDDC->SetHWnd(0, hWnd);
			tmp = SetHook((void *)(*(DWORD *)lpDDSPrim + 112), extSetClipper);
			if(tmp)
			{
				pSetClipper = (SetClipper_Type)tmp;
			}
			(*pSetClipper)(lpDDSEmu_Prim, lpDDC);
			ddsd.dwFlags = DDSD_CAPS | DDSD_WIDTH | DDSD_HEIGHT;
			ddsd.ddsCaps.dwCaps = DDSCAPS_OFFSCREENPLAIN;
			ddsd.dwWidth = dwWidth;
			ddsd.dwHeight = dwHeight;
			(*pCreateSurface)(lpdd, &ddsd, &lpDDSEmu_Back, 0);
		}
		else
		{
			ddsd.dwFlags &= ~(DDSD_WIDTH | DDSD_HEIGHT | DDSD_BACKBUFFERCOUNT | DDSD_REFRESHRATE | DDSD_PIXELFORMAT);
			ddsd.ddsCaps.dwCaps &= ~(DDSCAPS_FLIP | DDSCAPS_COMPLEX);
			res = (*pCreateSurface)(lpdd, &ddsd, lplpdds, pu);
			if(res)
			{
				return res;
			}
			lpDDSPrim = *lplpdds;
			lpdd->CreateClipper(0, &lpDDC, NULL);
			lpDDC->SetHWnd(0, hWnd);
			tmp = SetHook((void *)(*(DWORD *)lpDDSPrim + 112), extSetClipper);
			if(tmp)
			{
				pSetClipper = (SetClipper_Type)tmp;
			}
			(*pSetClipper)(lpDDSPrim, lpDDC);

			ddsd.dwFlags = DDSD_WIDTH | DDSD_HEIGHT | DDSD_CAPS;
			ddsd.dwWidth = dwWidth;
			ddsd.dwHeight = dwHeight;
			ddsd.ddsCaps.dwCaps &= ~DDSCAPS_PRIMARYSURFACE;
			ddsd.ddsCaps.dwCaps |= DDSCAPS_OFFSCREENPLAIN;

			(*pCreateSurface)(lpdd, &ddsd, &lpDDSBack, 0);
		}
		tmp = SetHook((void *)(*(DWORD *)lpDDSPrim), extQueryInterfaceS);
		if(tmp)
		{
			pQueryInterfaceS = (QueryInterface_Type)tmp;
		}
		tmp = SetHook((void *)(*(DWORD *)lpDDSPrim + 48), extGetAttachedSurface);
		if(tmp)
		{
			pGetAttachedSurface = (GetAttachedSurface_Type)tmp;
		}
		SetHook((void *)(*(DWORD *)lpDDSPrim + 44), extFlip);
		tmp = SetHook((void *)(*(DWORD *)lpDDSPrim + 20), extBlt);
		if(tmp)
		{
			pBlt = (Blt_Type)tmp;
		}
		SetHook((void *)(*(DWORD *)lpDDSPrim + 28), extBltFast);
		tmp = SetHook((void *)(*(DWORD *)lpDDSPrim + 80), extGetPalette);
		if(tmp)
		{
			pGetPalette = (GetPalette_Type)tmp;
		}
		tmp = SetHook((void *)(*(DWORD *)lpDDSPrim + 124), extSetPalette);
		if(tmp)
		{
			pSetPalette = (SetPalette_Type)tmp;
		}
		return 0;
	}
	if(dwFlags & EMULATEPAL && !(ddsd.dwFlags & DDSD_PIXELFORMAT))
	{
		ddsd.ddsCaps.dwCaps &= ~DDSCAPS_VIDEOMEMORY;
		ddsd.ddsCaps.dwCaps |= DDSCAPS_SYSTEMMEMORY;
		ddsd.dwFlags |= DDSD_PIXELFORMAT;
		ddsd.ddpfPixelFormat.dwSize = sizeof(DDPIXELFORMAT);
		ddsd.ddpfPixelFormat.dwFlags = DDPF_RGB | DDPF_PALETTEINDEXED8;
		ddsd.ddpfPixelFormat.dwRGBBitCount = 8;
		res = (*pCreateSurface)(lpdd, &ddsd, lplpdds, pu);
		OutTrace(" 256EmuSub Return %x\n", res);
		return res;
	}

	res = (*pCreateSurface)(lpdd, lpddsd, lplpdds, 0);
	if(res)
	{
		res = (*pCreateSurfaceOld)(lpdd, lpddsd, lplpdds, 0);
	}
	OutTrace(" Default Return %x\n", res);
	return res;
}

HRESULT WINAPI extGetAttachedSurface(void *lpdds, LPDDSCAPS lpddsc, LPDIRECTDRAWSURFACE *lplpddas)
{
	if(lpdds == lpDDSPrim)
	{
		*lplpddas = lpDDSBack;
		OutTrace("GetAttachedSurface: BackBuffer\n");
		return 0;
	}
	else
	{
		OutTrace("GetAttachedSurface: Other\n");
		return (*pGetAttachedSurface)(lpdds, lpddsc, lplpddas);
	}
}

HRESULT WINAPI extFlip(LPDIRECTDRAWSURFACE lpdds, LPDIRECTDRAWSURFACE lpddst, DWORD dwflags)
{
	OutTrace("Flip: target = %x src = %x\n", lpdds, lpddst);

	if((dwflags & DDFLIP_WAIT) || (dwFlags & SAVELOAD))
	{
		lpDD->WaitForVerticalBlank(DDWAITVB_BLOCKEND , 0);
	}
	if(lpddst)
	{
		return lpdds->Blt(0, lpddst, 0, DDBLT_WAIT, 0);
	}
	else
	{
		return lpdds->Blt(0, lpDDSBack, 0, DDBLT_WAIT, 0);
	}
}

HRESULT WINAPI extBlt(LPDIRECTDRAWSURFACE lpdds, LPRECT lpdestrect, LPDIRECTDRAWSURFACE lpddssrc, LPRECT lpsrcrect, DWORD dwflags, LPDDBLTFX lpddbltfx)
{
	DDSURFACEDESC2 ddsd;
	BYTE *src;
	DWORD *dest;
	long srcpitch, destpitch;
	DWORD x, y, w, h;
	RECT rect, screen;
	POINT p = {0, 0};

	OutTrace("Blt: dest = %x src = %x dwFlags = %x\n", lpdds, lpddssrc, dwflags);

	if(lpdds == lpDDSPrim)
	{
		if(lpdestrect)
		{
			screen = *lpdestrect;
		}
		if(!lpdestrect || (screen.right == dwWidth && screen.bottom == dwHeight))
		{
			GetClientRect(hWnd, &screen);
		}
		ClientToScreen(hWnd, &p);
		OffsetRect(&screen ,p.x, p.y);

		if(dwFlags & EMULATEPAL)
		{
			if(!lpddssrc)
			{
				return 0;
			}
			(*pBlt)(lpdds, lpdestrect, lpddssrc, lpsrcrect, dwflags, lpddbltfx);
			if(lpdestrect)
			{
				rect = *lpdestrect;
			}
			else
			{
				rect.left = 0;
				rect.top = 0;
				rect.right = dwWidth;
				rect.bottom = dwHeight;
			}
			w = rect.right - rect.left;
			h = rect.bottom - rect.top;
	
			ddsd.dwSize = (dwDDVersion < 4) ? sizeof(DDSURFACEDESC) : sizeof(DDSURFACEDESC2);
			ddsd.dwFlags = DDSD_LPSURFACE | DDSD_PITCH;
			if(lpDDSEmu_Back->Lock(0, (DDSURFACEDESC *)&ddsd, DDLOCK_SURFACEMEMORYPTR | DDLOCK_WRITEONLY, 0))
			{
				return 0;
			}
			ddsd.lPitch >>= 2;
			dest = (DWORD *)ddsd.lpSurface;
			dest += rect.top*ddsd.lPitch;
			dest += rect.left;
			destpitch = ddsd.lPitch - w;
			if(lpdds->Lock(0, (DDSURFACEDESC *)&ddsd, DDLOCK_SURFACEMEMORYPTR | DDLOCK_READONLY, 0))
			{
				lpDDSEmu_Back->Unlock(0);
				return 0;
			}
			src = (BYTE *)ddsd.lpSurface;
			src += rect.top*ddsd.lPitch;
			src += rect.left;
			srcpitch = ddsd.lPitch - w;

			for(y = 0; y < h; y ++)
			{
				for(x = 0; x < w; x ++)
				{
					*(dest ++) = PaletteEntries[*(src ++)];
				}
				dest += destpitch;
				src += srcpitch;
			}
	
			lpdds->Unlock(0);
			lpDDSEmu_Back->Unlock(0);
			p.x = 0; p.y = 0;
			ClientToScreen(hWnd, &p);
			OffsetRect(&rect, p.x, p.y);
			(*pBlt)(lpDDSEmu_Prim, &screen, lpDDSEmu_Back, lpdestrect, DDBLT_WAIT, 0);
			return 0;
		}
		else
		{
			return (*pBlt)(lpDDSPrim, &screen, lpddssrc, lpsrcrect, dwflags, lpddbltfx);
		}
	}
	else
	{
		return (*pBlt)(lpdds, lpdestrect, lpddssrc, lpsrcrect, dwflags, lpddbltfx);
	}
	return 0;
}

HRESULT WINAPI extBltFast(LPDIRECTDRAWSURFACE lpdds, DWORD dwx, DWORD dwy, LPDIRECTDRAWSURFACE lpddssrc, LPRECT lpsrcrect, DWORD dwtrans)
{
	RECT r;
	POINT p = {0, 0};
	DWORD flags = 0;
	DDSURFACEDESC2 ddsd;

	OutTrace("BltFast: dest = %x src = %x dwTrans = %x\n", lpdds, lpddssrc, dwtrans);

	if(dwtrans & DDBLTFAST_WAIT)
	{
		flags |= DDBLT_WAIT;
	}
	if(dwtrans & DDBLTFAST_DESTCOLORKEY)
	{
		flags |= DDBLT_KEYDEST;
	}
	if(dwtrans & DDBLTFAST_SRCCOLORKEY)
	{
		flags |= DDBLT_KEYSRC;
	}

	r.left = dwx;
	r.top = dwy;
	if(lpsrcrect)
	{
		r.right = r.left + lpsrcrect->right - lpsrcrect->left;
		r.bottom = r.top + lpsrcrect->bottom - lpsrcrect->top;
	}
	else
	{
		ddsd.dwSize = (dwDDVersion < 4) ? sizeof(DDSURFACEDESC) : sizeof(DDSURFACEDESC2);
		ddsd.dwFlags = DDSD_WIDTH | DDSD_HEIGHT;
		lpddssrc->GetSurfaceDesc((LPDDSURFACEDESC)&ddsd);
		r.right = r.left + ddsd.dwWidth;
		r.bottom = r.top + ddsd.dwHeight;
	}
	return lpdds->Blt(&r, lpddssrc, lpsrcrect, flags, 0);
}

HRESULT WINAPI extCreatePalette(LPDIRECTDRAW lpdd, DWORD dwflags, LPPALETTEENTRY lpddpa, LPDIRECTDRAWPALETTE *lplpddp, IUnknown *pu)
{
	HRESULT res;
	void *tmp;
	int i;

	OutTrace("CreatePalette: dwFlags = %x\n", dwflags);

	if(!(dwFlags & EMULATEPAL))
	{
		res = (*pCreatePalette)(lpdd, dwflags, lpddpa, lplpddp, pu);
		OutTrace("res:%x\n", res);
		return res;
	}
	res = (*pCreatePalette)(lpdd, dwflags & ~DDPCAPS_PRIMARYSURFACE, lpddpa, lplpddp, pu);
	if(res)
	{
		return res;
	}
	tmp = SetHook((void *)(**(DWORD **)lplpddp + 24), extSetEntries);
	if(tmp)
	{
		pSetEntries = (SetEntries_Type)tmp;
	}
	if(dwflags & DDPCAPS_PRIMARYSURFACE)
	{
		for(i = 0; i < 256; i ++)
		{
			PaletteEntries[i] = (((DWORD)lpddpa[i].peRed) << 16) + (((DWORD)lpddpa[i].peGreen) << 8) + ((DWORD)lpddpa[i].peBlue);
		}
		lpDDP = *lplpddp;
	}
	return 0;
}

HRESULT WINAPI extWaitForVerticalBlank(LPDIRECTDRAW lpdd, DWORD dwflags, HANDLE hevent)
{
	static DWORD time = 0;
	static BOOL step = 0;
	DWORD tmp;
	if(!(dwFlags & SAVELOAD))
	{
		return (*pWaitForVerticalBlank)(lpdd, dwflags, hevent);
	}
	tmp = GetTickCount();
	if((time - tmp) > 32)
	{
		time = tmp;
	}
	Sleep(time - tmp);
	if(step) 
	{
		time += 16;
	}
	else
	{
		time += 17;
	}
	step ^= 1;
	return 0;
}

HRESULT WINAPI extGetPalette(LPDIRECTDRAWSURFACE lpdds, LPDIRECTDRAWPALETTE *lplpddp)
{
	OutTrace("GetPalette\n");

	if(!(dwFlags & EMULATEPAL) || lpdds != lpDDSPrim)
	{
		return (*pGetPalette)(lpdds, lplpddp);
	}

	if(!lpDDP)
	{
		return DDERR_NOPALETTEATTACHED;
	}
	*lplpddp  = lpDDP;
	return 0;
}

HRESULT WINAPI extSetPalette(LPDIRECTDRAWSURFACE lpdds, LPDIRECTDRAWPALETTE lpddp)
{
	int i;
	//HRESULT res;
	PALETTEENTRY *tmp;

	OutTrace("SetPalette\n");

	if(!(dwFlags & EMULATEPAL) || lpdds != lpDDSPrim)
	{
		return  (*pSetPalette)(lpdds, lpddp);
	}

	lpDDP = lpddp;
	(*pSetPalette)(lpDDSBack, lpddp);
	if(lpddp)
	{
		tmp = (LPPALETTEENTRY)PaletteEntries;
		lpddp->GetEntries(0,0, 256, tmp);
		for(i = 0; i < 256; i ++)
		{
			PaletteEntries[i] = (((DWORD)tmp[i].peRed) << 16) + (((DWORD)tmp[i].peGreen) << 8) + ((DWORD)tmp[i].peBlue);
		}
	}
	return 0;
}

HRESULT WINAPI extSetEntries(LPDIRECTDRAWPALETTE lpddp, DWORD dwflags, DWORD dwstart, DWORD dwcount, LPPALETTEENTRY lpentries)
{
	int i;
	HRESULT res;

	OutTrace("SetEntries\n");

	res = (*pSetEntries)(lpddp, dwflags, dwstart, dwcount, lpentries);
	if(!(dwFlags & EMULATEPAL) || lpDDP != lpddp)
	{
		return res;
	}

	for(i = 0; i < (int)dwcount; i ++)
	{
		PaletteEntries[i + dwstart] = (((DWORD)lpentries[i].peRed) << 16) + (((DWORD)lpentries[i].peGreen) << 8) + ((DWORD)lpentries[i].peBlue);
	}
	return 0;
}

HRESULT WINAPI extSetClipper(LPDIRECTDRAWSURFACE lpdds, LPDIRECTDRAWCLIPPER lpddc)
{
	OutTrace("SetClipper\n");
	if(lpdds == lpDDSPrim && (dwFlags & EMULATEPAL))
	{
		return (*pSetClipper)(lpDDSEmu_Prim, lpddc);
	}
	return (*pSetClipper)(lpdds, lpddc);
}