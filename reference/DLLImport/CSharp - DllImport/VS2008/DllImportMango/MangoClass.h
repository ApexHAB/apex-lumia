// MangoClass.h : Declaration of the CMangoClass

#pragma once
#ifdef STANDARDSHELL_UI_MODEL
#include "resource.h"
#endif
#ifdef POCKETPC2003_UI_MODEL
#include "resourceppc.h"
#endif
#ifdef SMARTPHONE2003_UI_MODEL
#include "resourcesp.h"
#endif
#ifdef AYGSHELL_UI_MODEL
#include "resourceayg.h"
#endif

#include "DllImportMango.h"

typedef void (*voidC)(void); 


// CMangoClass

class ATL_NO_VTABLE CMangoClass :
	public CComObjectRootEx<CComMultiThreadModel>,
	public CComCoClass<CMangoClass, &CLSID_MangoClass>,
	public IMangoClass//public IDispatchImpl<IMangoClass, &IID_IMangoClass, &LIBID_DllImportMangoLib, /*wMajor =*/ 1, /*wMinor =*/ 0>
{
public:
	CMangoClass()
	{
	}

#ifndef _CE_DCOM
DECLARE_REGISTRY_RESOURCEID(IDR_MANGOCLASS)
#endif


BEGIN_COM_MAP(CMangoClass)
	COM_INTERFACE_ENTRY(IMangoClass)
	//COM_INTERFACE_ENTRY(IDispatch)
END_COM_MAP()

	//STDMETHOD(sayHello)();
	//void* sayHello();
	STDMETHOD(Zune_VoidCall)(LPCTSTR method);

	STDMETHOD(Radio_Toggle)(UINT powerMode);
	
	STDMETHOD(LoadLibrary7)(LPCTSTR lpFileName, UINT_PTR * hFind);

	STDMETHOD(GetProcAddress7)(UINT_PTR address, LPCTSTR lpProcName, INT_PTR * procAddr);
	
	STDMETHOD(FreeLibrary7)(UINT_PTR address);

	STDMETHOD(Clipboard_SET)(BSTR text);

	STDMETHOD(Clipboard_GET)(BSTR* pBstrClipboardText);

	STDMETHOD(VoidCall)(LPCTSTR dll, LPCTSTR method);

	STDMETHOD(MessageBoxRunningProc)(BSTR* result);

	STDMETHOD(TerminateProcess7)(UINT pid, UINT exitCode);

	STDMETHOD(GetAsyncKeyState7)(int key);

	STDMETHOD(Crnch)(BSTR * result);

	STDMETHOD(QueryPerformanceCounter7)(unsigned __int64* result);

	STDMETHOD(SetWindowsHookExW)(int key, int pre);

	STDMETHOD(ShellExecuteEx7)(BSTR file, BSTR args);

	STDMETHOD(CreateProcess7)(BSTR file, BSTR args);

	STDMETHOD(GetProcessTimes7)(INT_PTR hProcess, 
											 int * ctime1, int * ctime2, 
											 int * etime1, int * etime2,
											 int * ktime1, int * ktime2,
											 int * utime1, int * utime2);

	STDMETHOD(NavigateToExternalPage7)(INT_PTR safeHandle, BSTR pageUri, BOOL fIsInvocation, int cbPageArgs);

	STDMETHOD(MessageBox7)(LPCTSTR lpText, LPCTSTR lpCaption, UINT uType, int * result);

	STDMETHOD(ShutdownOS)(UINT ewxCode);

	STDMETHOD(CeCreateProcessEx)();

	STDMETHOD(GetLastError7)();

	STDMETHOD(StringCall)(LPCTSTR dll, LPCTSTR method, LPCTSTR value);

	STDMETHOD(Tests)();

	STDMETHOD(CreateDirectoryPath7)(LPCTSTR lpPathName);

	STDMETHOD(NdisGetSystemUpTimeEx7)(int part);

	STDMETHOD(fopen7)(BSTR filename, BSTR mode);

	STDMETHOD(fclose7)(int handle);

	STDMETHOD(fseek7)(int handle, long int offset, int origin);

	STDMETHOD(fgetc7)(int handle);

	STDMETHOD(fsize7)(int handle);

	STDMETHOD(fputc7)(int handle, char value);

	STDMETHOD(ddirFiles7)(BSTR fullFolderName, BSTR * result);

	STDMETHOD(dirFiles7)(BSTR fullFolderName, BSTR * result);

	STDMETHOD(FindFirstFile7)(BSTR dir, WIN32_FIND_DATA * result);

	STDMETHOD(FindNextFile7)(int handle, WIN32_FIND_DATA * result);

	STDMETHOD(FindClose7)(int handle);

	STDMETHOD(UintCall)(LPCTSTR dll, LPCTSTR method, UINT value);

	STDMETHOD(WindowTextOfHWND)(int hwnd /*GetForegroundWindow e.g.*/, BSTR * result, bool setOrGet);

	STDMETHOD(BoolCall)(LPCTSTR dll, LPCTSTR method, bool value);

	STDMETHOD(EnumWindows7)(bool onlyIsWindow, BSTR * result);

	STDMETHOD(TextForHWNDEnumWindows)(int hwnd, BSTR * result);

	STDMETHOD(GetClassNameForHWNDEnumWindows)(int hwnd, BSTR * result);

	STDMETHOD(IntCall)(LPCTSTR dll, LPCTSTR method, int value);

	STDMETHOD(IntDualCall)(LPCTSTR dll, LPCTSTR method, int value, int value2);

	STDMETHOD(EnumDisplayDevices7)(DWORD id, DISPLAY_DEVICE * deviceResult, DWORD flags);

	STDMETHOD(waveOutGetVolume7)(DWORD * result);

	STDMETHOD(waveOutSetVolume7)(DWORD value);

	STDMETHOD(waveOutSetPitch7)(DWORD value);

	STDMETHOD(waveGetHWAVEOUT)();

	STDMETHOD(KernelIoControl)(
			uint dwIoControlCode,
            INT_PTR lpInBuf,
            uint nInBufSize,
            INT_PTR lpOutBuf,
            uint nOutBufSize,
            uint * lpBytesReturned /*C# ref*/);

	STDMETHOD(GetSystemPowerStatusEx7)(BSTR * result, bool fUpdate);

	STDMETHOD(StringIntIntCall)(LPCTSTR dll, LPCTSTR method, BSTR str, int int1, int int2);

	STDMETHOD(StringIntIntOutCall)(LPCTSTR dll, LPCTSTR method, BSTR str, int int1, int * int2);

	STDMETHOD(ScreenSetContrast)(DWORD dwContrast /*0 to 255*/);

	STDMETHOD(ScreenGetContrast)(/*returns 0 to 255*/);

	STDMETHOD(IntQuadCall)(LPCTSTR dll, LPCTSTR method, int int1, int int2, int int3, int int4);

	STDMETHOD(PhoneMakeCall7)(BSTR number);

	STDMETHOD(SetSystemTime7)(WORD wYear, WORD wMonth, WORD wDayOfWeek, WORD wDay, WORD wHour, WORD wMinute, WORD wSecond, WORD wMilliseconds);

	STDMETHOD(StringStringCall)(LPCTSTR dll, LPCTSTR method, BSTR value, BSTR value2);
	
	STDMETHOD(GetSystemPowerStatusExAdv7)(BSTR * result, bool fUpdate);

	STDMETHOD(GlobalMemoryStatus7)(BSTR * result);

	STDMETHOD(CeRunAppAtTime7)(BSTR pwszAppName, SYSTEMTIME time);

	STDMETHOD(EnumDisplaySettings7)(BSTR lpszDeviceName, DWORD iModeNum, BSTR * lpDevMode);

	STDMETHOD(GetCommandLine7)(BSTR * result);

	STDMETHOD(PostMessage7)(int hwnd, uint Msg, WPARAM wParam, WPARAM lParam);

	STDMETHOD(SendMessage7)(int hwnd, uint Msg, WPARAM wParam, WPARAM lParam);

	STDMETHOD(ThisTaskHostInformation)(LPCTSTR dll, LPCTSTR method, void * _outValue);

	STDMETHOD(CaptureScreen)(HBITMAP *pHandle, INT *pSize, VOID **ppvBits);
	STDMETHOD(DeleteObject)(HGDIOBJ);

	STDMETHOD(ASMExecute)(BYTE * bytes);

	STDMETHOD(GetHelloWorldMSGBPtr)();

	STDMETHOD(ValueAtAddres)(int * addr);

	STDMETHOD(AddressOfObject)(void * object);

	STDMETHOD(TestFunc1)();

	STDMETHOD(TestFunc2)();

	STDMETHOD(DeviceIoControl7)(HANDLE hDevice, DWORD dwIoControlCode, LPVOID lpInBuf, DWORD nInBufSize, __out_bcount_opt(nOutBufSize) LPVOID lpOutBuf, DWORD nOutBufSize, LPDWORD lpBytesReturned, LPOVERLAPPED lpOverlapped);

	STDMETHOD(WriteByte)(int * address, BYTE value);

	STDMETHOD(EDB_Mount)(LPWSTR databaseFile, bool * sucCode);

	STDMETHOD(EDB_OpenMounted)(LPWSTR databaseFile, CEGUID * _mvolguid);

	STDMETHOD(EDB_FindFirstDB)(CEGUID * _mvolguid);

	STDMETHOD(EDB_FindNextDB)(void* findFirstHandle, CEGUID * _mvolguid);

	STDMETHOD(EDB_OpenDBSession)(PCEGUID _mvolguid, PCEOID findHand);

	STDMETHOD(EDB_CloseHandle)(HANDLE findFirstHandle);

	STDMETHOD(EDB_CeOidGetInfoEx)(PCEGUID _mvolguid, CEOID findHand);

	STDMETHOD(EDB_OpenDBFind)(PCEGUID _mvolguid, CEOID findHand);

	STDMETHOD(EDB_CeOpenDatabaseEx)(PCEGUID _mvolguid, CEOID findHand);

	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}

	void FinalRelease()
	{
	}

public:

};

OBJECT_ENTRY_AUTO(__uuidof(MangoClass), CMangoClass)
