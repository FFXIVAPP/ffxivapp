#define _WIN32_WINNT 0x0400
#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <stdio.h>
#include "WinModXIV.h"
#include "dxhook.h"

typedef int (WINAPI *GetDeviceCaps_Type)(HDC, int);
typedef BOOL (WINAPI *GetCursorPos_Type)(LPPOINT);

int WINAPI extGetDeviceCaps(HDC, int);
BOOL WINAPI extGetCursorPos(LPPOINT);

DWORD dwFlags;
DWORD dwVersion;
BOOL bActive = 1;
HWND hWnd = 0;
DWORD dwWidth = 0;
DWORD dwHeight = 0;
GetDeviceCaps_Type pGetDeviceCaps;
GetCursorPos_Type pGetCursorPos;

WNDPROC pWindowProc = 0;

void OutTrace(const char *format, ...)
{
	va_list al;
	int i;
	void *a[8];
	static char path[MAX_PATH];
	FILE *fp;

	if(!(dwFlags & OUTTRACE))
	{
		//MessageBox(NULL, TEXT((LPCSTR)dwFlags), TEXT("caption"), 0);
		return;
	}

	GetCurrentDirectory(MAX_PATH, path);
	strcat(path, "\\WinModXIV_Trace.txt");
	fp = fopen(path, "a+");
	va_start(al, format);
	
	for(i = 0; i < 8;i ++)
	{
		a[i] = va_arg(al, void *);
	}

	fprintf(fp, format, a[0], a[1], a[2], a[3], a[4], a[5], a[6], a[7]);
	va_end(al);
	fclose(fp);
}

void *HookAPI(const char *module, const char *api, void *hookproc)
{
	int i;
	DWORD base;
	PIMAGE_NT_HEADERS pnth;
	PIMAGE_IMPORT_DESCRIPTOR pidesc;
	DWORD rva;
	PSTR impmodule;
	PIMAGE_THUNK_DATA ptaddr;
	PIMAGE_THUNK_DATA ptname;
	PIMAGE_IMPORT_BY_NAME piname;
	DWORD oldprotect;
	void *org;
	
	base = (DWORD)GetModuleHandle(0);
	if(!base) return 0;
	__try
	{
		pnth = PIMAGE_NT_HEADERS(PBYTE(base) + PIMAGE_DOS_HEADER(base)->e_lfanew);
		if(!pnth)
		{
			return 0;
		}
		rva = pnth->OptionalHeader.DataDirectory[IMAGE_DIRECTORY_ENTRY_IMPORT].VirtualAddress;
		if(!rva)
		{
			return 0;
		}
		pidesc = (PIMAGE_IMPORT_DESCRIPTOR)(base + rva);
		while(1)
		{
			if(!pidesc->FirstThunk)
			{
				return 0;
			}
			impmodule = (PSTR)(base + pidesc->Name);
			if(!lstrcmpi(module, impmodule))
			{
				break;
			}
			pidesc ++;
		}
		ptaddr = (PIMAGE_THUNK_DATA)(base + (DWORD)pidesc->FirstThunk);
		ptname = (PIMAGE_THUNK_DATA)(base + (DWORD)pidesc->OriginalFirstThunk);
		for(i = 0; (ptaddr + i)->u1.Function; i ++);
		while(1)
		{
			if(!ptaddr->u1.Function)
			{
				return 0;
			}
			if(!IMAGE_SNAP_BY_ORDINAL(ptname->u1.Ordinal))
			{
				piname = (PIMAGE_IMPORT_BY_NAME)(base + (DWORD)ptname->u1.AddressOfData);
				if(!lstrcmpi(api, (char *)piname->Name))
				{
					break;
				}
			}
			ptaddr ++;
			ptname ++;
		}
		org = (void *)ptaddr->u1.Function;
		if(org == hookproc)
		{
			return 0;
		}
		if(!VirtualProtect(&ptaddr->u1.Function, 4, PAGE_EXECUTE_READWRITE, &oldprotect))
		{
			return 0;
		}
		ptaddr->u1.Function = (DWORD)hookproc;
		VirtualProtect(&ptaddr->u1.Function, 4, oldprotect, &oldprotect);
	}
	__except(EXCEPTION_EXECUTE_HANDLER)
	{       
		OutTrace("%s:%s Hook Failed.\n", module, api);
		org = 0;
	}
	return org;
}

void *SetHook(void *target, void *hookproc)
{
	DWORD tmp, oldprot;
	
	tmp = *(DWORD *)target;
	if(tmp == (DWORD)hookproc)
	{
		return 0;
	}
	if(!VirtualProtect(target, 4, PAGE_READWRITE, &oldprot))
	{
		return 0;
	}
	*(DWORD *)target = (DWORD)hookproc;
	VirtualProtect(target, 4, oldprot, &oldprot);
	return (void *)tmp;
}

void AdjustWindowFrame(HWND hwnd, DWORD width, DWORD height)
{
	RECT rect;
	RECT offset;
	POINT p = {0, 0};
	static BOOL check = 0;

	OutTrace("AdjustWindowFrame\n");
	if(check)
	{
		return;
	}

	check = 1;
	rect.top = 0;
	rect.left = 0;
	rect.right = width - 1;
	rect.bottom = height - 1;
	AdjustWindowRect(&rect, WS_OVERLAPPEDWINDOW, FALSE);
	SetWindowLong(hwnd, GWL_STYLE, WS_OVERLAPPEDWINDOW);
	GetWindowRect(hwnd, &offset);
	SetWindowPos(hwnd, 0, offset.left, offset.top, rect.right - rect.left, rect.bottom - rect.top, 0);
	ShowWindow(hwnd, SW_SHOWNORMAL);
	pWindowProc = (WNDPROC)GetWindowLong(hwnd, GWL_WNDPROC);
	if(!((DWORD)pWindowProc & 0xff000000))
	{
		(WNDPROC)SetWindowLong(hwnd, GWL_WNDPROC, (LONG)extWindowProc);
	}
	ShowCursor(0);
	InvalidateRect(0, 0, TRUE);
}

LRESULT CALLBACK extWindowProc(HWND hwnd, UINT message, WPARAM wparam, LPARAM lparam)
{
	POINT p;
	RECT rect;

	switch(message)
	{
		case WM_ACTIVATE:
			bActive = (LOWORD(wparam) == WA_ACTIVE || LOWORD(wparam) == WA_CLICKACTIVE) ? 1 : 0;
		case WM_NCACTIVATE:
			if(message == WM_NCACTIVATE)
			{
				bActive = wparam;
			}
			SetWindowPos(hwnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
			if(dwFlags & UNNOTIFY)
			{
				DefWindowProc(hwnd, message, wparam, lparam);
				return false;
			}
			break;
		case WM_NCMOUSEMOVE:
			while(ShowCursor(1) < 0);
			break;
		case WM_MOUSEMOVE:
			p.x = LOWORD(lparam);
			p.y = HIWORD(lparam);
			GetClientRect(hwnd, &rect);
			if(p.x >= 0 && p.x < rect.right && p.y >= 0 && p.y < rect.bottom)
			{
				while(ShowCursor(0) >= 0);
			}
			else
			{
				while(ShowCursor(1) < 0);
			}
		case WM_MOUSEWHEEL:
		case WM_LBUTTONDOWN:
		case WM_LBUTTONUP:
		case WM_LBUTTONDBLCLK:
		case WM_RBUTTONDOWN:
		case WM_RBUTTONUP:
		case WM_RBUTTONDBLCLK:
		case WM_MBUTTONDOWN:
		case WM_MBUTTONUP:
		case WM_MBUTTONDBLCLK:
			if(dwFlags & MODIFYMOUSE)
			{
				p.x = LOWORD(lparam);
				p.y = HIWORD(lparam);
				ScreenToClient(hwnd, &p);
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
				lparam = MAKELPARAM(p.y, p.x);
			}
			break;
		case WM_SIZE:
			float ratio;
			char s[64];
			GetClientRect(hwnd, &rect);
			p.x = rect.right;
			p.y = rect.bottom;
			if(GetAsyncKeyState(VK_SHIFT) & 0x8000)
			{
				if(((float)p.x/dwWidth) > ((float)p.y/dwHeight))
				{
					ratio = (float)p.x / dwWidth;
					p.y = (int)((float)dwHeight * ratio);
				}
				else
				{
					ratio = (float)p.y / dwHeight;
					p.x = (int)((float)dwWidth * ratio);
				}
			}
			if(GetAsyncKeyState(VK_CONTROL) & 0x8000)
			{
				p.x = dwWidth;
				p.y = dwHeight;
			}
			p.x = p.x - rect.right;
			p.y = p.y - rect.bottom;
			sprintf(s, "%f %i %i", ratio, p.x, p.y);
			if(p.x || p.y)
			{
				GetWindowRect(hwnd, &rect);
				p.x = rect.right - rect.left + p.x;
				p.y = rect.bottom - rect.top + p.y;
				MoveWindow(hwnd, rect.left, rect.top, p.x, p.y, TRUE);
			}
			break;
	}
	if(pWindowProc)
	{
		return (*pWindowProc)(hwnd, message, wparam, lparam);
	}
	return DefWindowProc(hwnd, message, wparam, lparam);
}

int WINAPI extGetDeviceCaps(HDC hdc, int nindex)
{
	int res;
	
	res = (*GetDeviceCaps)(hdc, nindex);
	OutTrace("GetDeviceCaps: hdc = %x index = %x res = %x\n", hdc, nindex, res);

	switch(nindex)
	{
		case RASTERCAPS:
			if(dwFlags & EMULATEPAL)
			{
				res |= RC_PALETTE;
			}
			break;
		case BITSPIXEL:
		case COLORRES:
			if(dwFlags & EMULATEPAL)
			{
				res = 8;
			}
			break;
		case SIZEPALETTE:
			if(dwFlags & EMULATEPAL)
			{
				res = 256;
			}
			break;
		case NUMRESERVED:
			if(dwFlags & EMULATEPAL)
			{
				res = 0;
			}
			break;
	}
	return res;
}

BOOL WINAPI extGetCursorPos(LPPOINT lppoint)
{
	(*pGetCursorPos)(lppoint);
	OutTrace("GetCursorPos: x = %i y = %i\n", lppoint->x, lppoint->y);

	if(dwFlags & MODIFYMOUSE)
	{
		ScreenToClient(hWnd, lppoint);
		if(lppoint->x < 0)
		{
			lppoint->x = 0;
		}
		if(lppoint->x >= (int)dwWidth)
		{
			lppoint->x = dwWidth - 1;
		}
		if(lppoint->y < 0)
		{
			lppoint->y = 0;
		}
		if(lppoint->y >= (int)dwHeight)
		{
			lppoint->y = dwHeight - 1;
		}
	}
	return 1;
}


int HookDirectX(TARGETMAP *target)
{
	void *tmp;

	dwFlags = target->flags;
	
	if(dwFlags & EMULATEPAL)
	{
		tmp = HookAPI("gdi32.dll", "GetDeviceCaps", extGetDeviceCaps);
		if(tmp)
		{
			pGetDeviceCaps = (GetDeviceCaps_Type)tmp;
		}
	}
	
	if(dwFlags & MODIFYMOUSE)
	{
		tmp = HookAPI("user32.dll", "GetCursorPos", extGetCursorPos);
		if(tmp)
		{
			pGetCursorPos = (GetCursorPos_Type)tmp;
		}
	}

	if(dwFlags & HOOKDI)
	{
		HookDirectInput(target->dxversion);
		InitPosition(target->initx, target->inity, target->minx, target->miny, target->maxx, target->maxy);
	}
	HookDirectDraw(target->dxversion);
	HookDirect3D(target->dxversion);

	return 0;
}
