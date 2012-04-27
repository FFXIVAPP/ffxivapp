#define MAXTARGETS			256
#define UNNOTIFY			0x0001
#define EMULATEPAL			0x0002
#define CAPTURECURSOR		0x0004
#define HIDECURSOR			0x0008
#define HOOKDI				0x0010
#define MODIFYMOUSE			0x0020
#define OUTTRACE			0x0040
#define SAVELOAD			0x0080

typedef struct TARGETMAP
{
	char path[MAX_PATH];
	int dxversion;
	int flags;
	int initx;
	int inity;
	int minx;
	int miny;
	int maxx;
	int maxy;
}

TARGETMAP;

int SetTarget(TARGETMAP *);
int StartHook(void);
int EndHook(void);
char* GetDllVersion();
int HookDirectX(TARGETMAP *);

void *SetHook(void *, void *);
void OutTrace(const char *, ...);
void *HookAPI(const char *, const char *, void *);
void AdjustWindowFrame(HWND, DWORD, DWORD);
LRESULT CALLBACK extWindowProc(HWND, UINT, WPARAM, LPARAM);