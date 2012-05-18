// MangoClass.cpp : Implementation of CMangoClass

#define __CSTRINGT_H__
#define EDB 1

#include <windows.h>
#include <sms.h>
#include <aygshell.h>
#include <list>
#include <phone.h>

#include "afxstr.h"
#include <afxstr.h>
#include <atlstr.h>


//#include <cstringt>

#include "stdafx.h"
#include "MangoClass.h"

#include <windows.h>

#include "stdafx.h"

#include "storemgr.h"
#include <sms.h>
#include <aygshell.h>
//#include "LPConv.h"
#include <Wingdi.h>
//#include "ScreenCapture.h"
//#include "Exports.h"
#include <list>
#include <phone.h>
#include <Tlhelp32.h>
//#import "mscorlib.tlb" no_namespace raw_interfaces_only
//#include "base64.h"
//#include "capture.h"
//#undef __CSTRINGT_H__
//#include "cstringt.h"

#pragma comment(lib, "sms.lib")
#pragma comment(lib, "ceshell.lib")
#pragma comment(lib, "aygshell.lib")
#pragma comment(lib, "phone.lib")
#pragma comment(lib, "Toolhelp.lib");

#include <atlstr.h>
#include <windbase.h>
//CString str(TEXT(""));
//using namespace ATL;

//#define CString ATL::CStringT<TCHAR, StrTraitMFC>
//CStringT< TCHAR, StrTraitMFC< TCHAR > > derpStr;
//ATLA::
//ATLA::CStringT<TCHAR, StrTraitMFC> ca;

//typedef CStringT<
//	TCHAR, 
//	StrTraitMFC
//> CString;

typedef UINT (CALLBACK* XNASIMPLEVOID) (void);

#define PP_NARG(...) PP_NARG_(__VA_ARGS__ , PP_RSEQ_N()) 
#define PP_NARG_(...) PP_ARG_N(__VA_ARGS__) 
#define PP_ARG_N(  _1, _2, _3, _4, _5, _6, _7, _8, _9,_10, _11,_12,_13,_14,_15,_16,_17,_18,_19,_20, _21,_22,_23,_24,_25,_26,_27,_28,_29,_30, _31,_32,_33,_34,_35,_36,_37,_38,_39,_40, _41,_42,_43,_44,_45,_46,_47,_48,_49,_50, _51,_52,_53,_54,_55,_56,_57,_58,_59,_60, _61,_62,_63,N,...) N 
#define PP_RSEQ_N() 63,62,61,60,59,58,57,56,55,54,53,52,51,50, 49,48,47,46,45,44,43,42,41,40,39,38,37,36,35,34,33,32,31,30,29,28,27,26,25,24,23,22,21,20,19,18,17,16,15,14,13,12,11,10,9,8,7,6,5,4,3,2,1,0 


template<typename CallBoxer>
HRESULT DLLCALL_VOID(LPCTSTR dll, LPCTSTR method)
{
	HINSTANCE xna = ::LoadLibrary(dll);

	if(xna != NULL)
	{
		FARPROC proc = ::GetProcAddress(xna, method);

		if(proc != NULL)
		{		
			CallBoxer call;

			try {
				call = (CallBoxer)proc;
				
				return (HRESULT)(((HRESULT)call()) ? 15 : 19);

				return ((HRESULT)11);
			}
			catch(...)
			{
				return ((HRESULT)4);
			}
			return((HRESULT)6);
		}
		else
		{	
			return((HRESULT)3);
		}
		::FreeLibrary(xna);

		return ((HRESULT)5);
	}
	else
	{
		return((HRESULT)2);
	}
	return ((HRESULT)1);
}


template<typename ReturnType, typename ArgType>//CallBoxer, ArgCaller>
HRESULT DLLCALL_PARAM(LPCTSTR dll, LPCTSTR method, ArgType args = NULL, BOOL hasArgs = false)
{
	HINSTANCE xna = ::LoadLibrary(dll);

	if(xna != NULL)
	{
		FARPROC proc = ::GetProcAddress(xna, method);

		if(proc != NULL)
		{		
			typedef ReturnType (CALLBACK* CallHandlerDynamic) (ArgType);

			CallHandlerDynamic call;

			try {
				call = (CallHandlerDynamic)proc;
				
				/*if(args == NULL)
				{
					return (HRESULT)(((HRESULT)call()) ? 41 : 35);
				}
				else
				{*/
					return ((HRESULT)call(args));
				//}
				return ((HRESULT)11);
			}
			catch(...)
			{
				return ((HRESULT)4);
			}
			return((HRESULT)6);
		}
		else
		{	
			return((HRESULT)3);
		}
		::FreeLibrary(xna);

		return ((HRESULT)5);
	}
	else
	{
		return((HRESULT)2);
	}
	return ((HRESULT)1);
}

template<typename ReturnType, typename ArgType>//CallBoxer, ArgCaller>
HRESULT DLLCALL_PARAM_DUAL(LPCTSTR dll, LPCTSTR method, ArgType args = NULL, ArgType args2 = NULL, BOOL hasArgs = false)
{
	HINSTANCE xna = ::LoadLibrary(dll);

	if(xna != NULL)
	{
		FARPROC proc = ::GetProcAddress(xna, method);

		if(proc != NULL)
		{		
			typedef ReturnType (CALLBACK* CallHandlerDynamic) (ArgType,ArgType);

			CallHandlerDynamic call;

			try {
				call = (CallHandlerDynamic)proc;
				
				/*if(args == NULL)
				{
					return (HRESULT)(((HRESULT)call()) ? 41 : 35);
				}
				else
				{*/
					return ((HRESULT)call(args, args2));
				//}
				return ((HRESULT)11);
			}
			catch(...)
			{
				return ((HRESULT)4);
			}
			return((HRESULT)6);
		}
		else
		{	
			return((HRESULT)3);
		}
		::FreeLibrary(xna);

		return ((HRESULT)5);
	}
	else
	{
		return((HRESULT)2);
	}
	return ((HRESULT)1);
}

template<typename ReturnType, typename ArgType>//CallBoxer, ArgCaller>
HRESULT DLLCALL_PARAM_VA(LPCTSTR dll, LPCTSTR method, int num_args, ...)
{
	HINSTANCE xna = ::LoadLibrary(dll);

	if(xna != NULL)
	{
		FARPROC proc = ::GetProcAddress(xna, method);

		if(proc != NULL)
		{		
			if( num_args > 0 )
			{
				typedef ReturnType (CALLBACK* CallHandlerDynamic) (void);//(ArgType);

				CallHandlerDynamic call;

				try {
					call = (CallHandlerDynamic)proc;
					//__VA_ARGS__

					/*if(args == NULL)
					{
						return (HRESULT)(((HRESULT)call()) ? 41 : 35);
					}
					else
					{*/
						return (HRESULT)(((HRESULT)call(  )) ? 94 : 27);
					//}
					return ((HRESULT)43);
				}
				catch(...)
				{
					return ((HRESULT)64);
				}
				return((HRESULT)69);
			}
			else
			{
				typedef ReturnType (CALLBACK* CallHandlerDynamic) (...);//(ArgType);

				CallHandlerDynamic call;

				try {
					call = (CallHandlerDynamic)proc;
					//__VA_ARGS__

					/*if(args == NULL)
					{
						return (HRESULT)(((HRESULT)call()) ? 41 : 35);
					}
					else
					{*/
						return (HRESULT)(((HRESULT)call( __VA_ARGS__ )) ? 15 : 19);
					//}
					return ((HRESULT)11);
				}
				catch(...)
				{
					return ((HRESULT)4);
				}
				return((HRESULT)6);
			}
		}
		else
		{	
			return((HRESULT)3);
		}
		::FreeLibrary(xna);

		return ((HRESULT)5);
	}
	else
	{
		return((HRESULT)2);
	}
	return ((HRESULT)1);
}





template<typename ReturnType>//CallBoxer, ArgCaller>
HRESULT DLLCALL_PARAM(LPCTSTR dll, LPCTSTR method)
{
	HINSTANCE xna = ::LoadLibrary(dll);

	if(xna != NULL)
	{
		FARPROC proc = ::GetProcAddress(xna, method);

		if(proc != NULL)
		{		
			typedef ReturnType (CALLBACK* CallHandlerDynamic) (void);

			CallHandlerDynamic call;

			try {
				call = (CallHandlerDynamic)proc;
				
				/*if(args == NULL)
				{
					return (HRESULT)(((HRESULT)call()) ? 41 : 35);
				}
				else
				{*/
					return ((HRESULT)call());
				//}
				return ((HRESULT)11);
			}
			catch(...)
			{
				return ((HRESULT)4);
			}
			return((HRESULT)6);
		}
		else
		{	
			return((HRESULT)3);
		}
		::FreeLibrary(xna);

		return ((HRESULT)5);
	}
	else
	{
		return((HRESULT)2);
	}
	return ((HRESULT)1);
}


template<typename ReturnType>
HRESULT DLLCALL_PARAM_OUT(LPCTSTR dll, LPCTSTR method, void * _outValue)
{
	HINSTANCE xna = ::LoadLibrary(dll);

	if(xna != NULL)
	{
		FARPROC proc = ::GetProcAddress(xna, method);

		if(proc != NULL)
		{		
			typedef ReturnType (CALLBACK* CallHandlerDynamic) (void*);

			CallHandlerDynamic call;

			try {
				call = (CallHandlerDynamic)proc;
				
				/*if(args == NULL)
				{
					return (HRESULT)(((HRESULT)call()) ? 41 : 35);
				}
				else
				{*/
					return (HRESULT)(((HRESULT)call(_outValue)) ? 15 : 19);
				//}
				return ((HRESULT)11);
			}
			catch(...)
			{
				return ((HRESULT)4);
			}
			return((HRESULT)6);
		}
		else
		{	
			return((HRESULT)3);
		}
		::FreeLibrary(xna);

		return ((HRESULT)5);
	}
	else
	{
		return((HRESULT)2);
	}
	return ((HRESULT)1);
}

template<typename ReturnType>
ReturnType LoadMethod(LPCTSTR dll, LPCTSTR method)
{
	HINSTANCE xna = ::LoadLibrary(dll);

	if(xna != NULL)
	{
		FARPROC proc = ::GetProcAddress(xna, method);

		if(proc != NULL)
		{		
			return (ReturnType)proc;
		}

		::FreeLibrary(xna);
	}
	return NULL;
}

//interface derp
//{
//	int hello();
//};
//class derpClass : public derp
//{
//public:
//	int hello()
//	{
//		return 1337;
//	}
//};
//STDMETHODIMP CMangoClass::sayHello()
//{
//	INT val = MessageBox(NULL, TEXT("Hello hannes :)"), TEXT("derp"), 0);
//	
//	return 1337;
//}
//
//
//
//#ifdef lol


//Microsoft.Phone.Perf.Interop.NativeMethods + RegQueryValueEx + coredll.dll

STDMETHODIMP CMangoClass::Zune_VoidCall(LPCTSTR method)
{
	return DLLCALL_PARAM<UINT>(TEXT("XnaFrameworkCore"), method);//DLLCALL_VOID<XNASIMPLEVOID>(TEXT("XnaFrameworkCore"), method);
}

STDMETHODIMP CMangoClass::Radio_Toggle(UINT powerMode)
{
	//ZuneNativeMethods + radio
	return DLLCALL_PARAM<UINT, UINT>(TEXT("zuneapi"), TEXT("MediaApi_EnableRadio"), powerMode, true);
}
STDMETHODIMP CMangoClass::LoadLibrary7(LPCTSTR lpFileName, UINT_PTR * address)
{
	HINSTANCE load = ::LoadLibrary(lpFileName);
	INT_PTR l = (INT_PTR)load;
	*address = (UINT_PTR)l;

	return l ? S_OK : S_FALSE;
}

STDMETHODIMP CMangoClass::GetProcAddress7(UINT_PTR address, LPCTSTR lpProcName, INT_PTR * procAddr)
{
	FARPROC c = ::GetProcAddress((HINSTANCE)address, lpProcName);
	*procAddr = (INT_PTR)c;

	return c ? S_OK : S_FALSE;
}


STDMETHODIMP CMangoClass::FreeLibrary7(UINT_PTR address)
{
	return ::FreeLibrary((HINSTANCE)address);
}


char* ConvertBSTRToLPSTR (BSTR bstrIn)
{
  LPSTR pszOut = NULL;
  if (bstrIn != NULL)
  {
	int nInputStrLen = SysStringLen (bstrIn);

	// Double NULL Termination
	int nOutputStrLen = WideCharToMultiByte(CP_ACP, 0, bstrIn, nInputStrLen, NULL, 0, 0, 0) + 2; 
	pszOut = new char [nOutputStrLen];

	if (pszOut)
	{
	  memset (pszOut, 0x00, sizeof (char)*nOutputStrLen);
	  WideCharToMultiByte (CP_ACP, 0, bstrIn, nInputStrLen, pszOut, nOutputStrLen, 0, 0);
	}
  }
  return pszOut;
}

LPCTSTR sText = _T("http://ilsken.net/ http://ilsken.net/ http://ilsken.net/ http://ilsken.net/\nhttp://ilsken.net/ http://ilsken.net/\nhttp://ilsken.net/");

STDMETHODIMP CMangoClass::Clipboard_SET(BSTR text)
{
	if (!OpenClipboard(NULL)){ return 1; }	//if failed to open clipboard, die
	EmptyClipboard();	//empty the clipboard

	HGLOBAL hMem = GlobalAlloc(GMEM_MOVEABLE, (lstrlen(sText)+1));	//allocate memory as big as the text-string
	LPTSTR sMem = (TCHAR*)GlobalLock(hMem);		//make memory-data space, lock the memory
	memcpy(sMem, sText, (lstrlen(sText)+1));	//copy text-data into memory-data
	GlobalUnlock(hMem);		//unlock the memory
	SetClipboardData(CF_TEXT, hMem);	//put the data (text) to the clipboard

	CloseClipboard();	//we don't want to put anymore data to it so..



    // UniformResourceLocator - CFSTR_SHELLURL
 //   const UINT cfShellURL = RegisterClipboardFormat(CFSTR_SHELLURL);
 //   HGLOBAL hmemURL = GlobalAlloc(GMEM_MOVEABLE | GMEM_ZEROINIT, 5 + 1);
 //   char *pszURL = (char *) GlobalLock(hmemURL);
 //   
	//*pszText = "Tello";
	////uri.ToCString(pszURL, uri.Length() + 1);
 //   GlobalUnlock(hmemURL);
 //   SetClipboardData(cfShellURL, hmemURL);

    // TODO
    // FileContents - CFSTR_FILECONTENTS
    // FileGroupDescriptor - CFSTR_FILEDESCRIPTORA
    // FileGroupDescriptorW - CFSTR_FILEDESCRIPTORW

    CloseClipboard();
	//////::MessageBox(NULL, CTtoW(text), CTtoW(text), 0); // -> Verify all data OK from C#
	////CString c = (CString)text;
	////
	////const char* output = (char *)(LPCTSTR)c;


	//////const char* output = cstr;//"Test";
	////const size_t len = ::strlen(output) + 1;
	////HGLOBAL hMem = GlobalAlloc(GMEM_MOVEABLE, len);
	////memcpy(GlobalLock(hMem), output, len);
	////GlobalUnlock(hMem);
	////OpenClipboard(0);
	////EmptyClipboard();
	////SetClipboardData(CF_TEXT, hMem);
	////CloseClipboard();

	////return S_OK;
	//CString strData(text);
	//

	//// test to see if we can open the clipboard first before
	//// wasting any cycles with the memory allocation

	//if (OpenClipboard(NULL))
	//{
	//	// Empty the Clipboard. This also has the effect
	//	// of allowing Windows to free the memory associated
	//	// with any data that is in the Clipboard

	//	EmptyClipboard();

	//	// Ok. We have the Clipboard locked and it's empty. 
	//	// Now let's allocate the global memory for our data.

	//	// Here I'm simply using the GlobalAlloc function to 
	//	// allocate a block of data equal to the text in the
	//	// "to clipboard" edit control plus one character for the
	//	// terminating null character required when sending
	//	// ANSI text to the Clipboard.

	//	HGLOBAL hClipboardData;
	//	hClipboardData = GlobalAlloc(GMEM_DDESHARE, 
	//		strData.GetLength()+1);

	//	// Calling GlobalLock returns to me a pointer to the 
	//	// data associated with the handle returned from 
	//	// GlobalAlloc

	//	char * pchData;
	//	pchData = (char*)GlobalLock(hClipboardData);

	//	// At this point, all I need to do is use the standard 
	//	// C/C++ strcpy function to copy the data from the local 
	//	// variable to the global memory.

	//	strcpy(pchData, ConvertBSTRToLPSTR( text ));

	//	// Once done, I unlock the memory - remember you 
	//	// don't call GlobalFree because Windows will free the 
	//	// memory automatically when EmptyClipboard is next 
	//	// called. 

	//	GlobalUnlock(hClipboardData);

	//	// Now, set the Clipboard data by specifying that 
	//	// ANSI text is being used and passing the handle to
	//	// the global memory.

	//	SetClipboardData(CF_OEMTEXT,hClipboardData);

	//	// Finally, when finished I simply close the Clipboard
	//	// which has the effect of unlocking it so that other
	//	// applications can examine or modify its contents.

	//	CloseClipboard();

	//	return 1;
	//}
	return 0;
}

STDMETHODIMP CMangoClass::Clipboard_GET(BSTR* name)
{
	return S_OK;
	//BOOL ok = 1;
	//char * buffer;
	//if(OpenClipboard(NULL))
	//{
	//	
	//	buffer = (char*)GetClipboardData(CF_TEXT);
	//	//do something with buffer here 
	//	//before it goes out of scope
	//	ok = 0;
	//}

	//CloseClipboard(); 

	//*c = *buffer;

	//return ok;
	//BSTR sra = ::SysAllocString(_T(*c));
	//CString cs(c);
	//::OLESTR(cs);

	//HANDLE clip;
 //   if (OpenClipboard(NULL)) 
	//{
 //       clip = GetClipboardData(CF_TEXT);

	//	*text = CString( (char*) clip );
	//	return S_OK;
	//}
	//return S_FALSE;
}

STDMETHODIMP CMangoClass::VoidCall(LPCTSTR dll, LPCTSTR method)
{
	return DLLCALL_PARAM<HRESULT>(dll, method);//DLLCALL_VOID<XNASIMPLEVOID>(TEXT("XnaFrameworkCore"), method);
}

STDMETHODIMP CMangoClass::MessageBoxRunningProc(BSTR* result)
{
	// Get the process list snapshot.
	HANDLE hProcessSnapShot = CreateToolhelp32Snapshot( TH32CS_SNAPALL, 0 );

	// Initialize the process entry structure.
	PROCESSENTRY32 ProcessEntry = { 0 };
	ProcessEntry.dwSize = sizeof( ProcessEntry );
	
	// Get the first process info.
	BOOL Return = FALSE;
	Return = Process32First( hProcessSnapShot, &ProcessEntry );

	// Getting process info failed.
	if( !Return )
	{
		return S_FALSE;
	}

	int x = 1;
	CString procList(TEXT(""));
	do
	{
	/*	CString sMessage = TEXT("");
		sMessage.Format(TEXT("%d"),x++);*/

		//procList.Append(ProcessEntry.szExeFile);

		procList.AppendFormat(TEXT("%d-%d-%d-%d-%d-%d-%d-%d-%d-%s\n"),
			ProcessEntry.dwSize, 
			ProcessEntry.cntUsage, 
			ProcessEntry.th32ProcessID, 
			ProcessEntry.th32DefaultHeapID,
			ProcessEntry.th32ModuleID,
			ProcessEntry.cntThreads,
			ProcessEntry.th32ParentProcessID, 
			ProcessEntry.pcPriClassBase, 
			ProcessEntry.dwFlags, 
			ProcessEntry.szExeFile); 
		
		//ProcessEntry.szExeFile, ProcessEntry.th32ProcessID);
		//procList.AppendFormat(TEXT(" Usage %d"), ProcessEntry.cntUsage);
		//procList.AppendFormat(TEXT(" Treads %d"), ProcessEntry.cntThreads);
		//procList.Append(TEXT("\r\n"));
		//::MessageBox(NULL, TEXT(""), TEXT(""), 0);
		//ProcessEntry.szExeFile
		//// print the process details.
		//cout << _T("Process EXE File:") << ProcessEntry.szExeFile
		//								<< endl;
		//cout << _T("Process ID:") << ProcessEntry.th32ProcessID
		//						  << endl;
		//cout << _T("Process References:") << ProcessEntry.cntUsage
		//								  << endl;
		//cout << _T("Process Thread Count:") << ProcessEntry.cntThreads
		//									<< endl;

		// check the PROCESSENTRY32 for other members.
	}
	while( Process32Next( hProcessSnapShot, &ProcessEntry ));

	


	// Close the handle
	CloseHandle( hProcessSnapShot );

	*result = (procList.AllocSysString());
	//AfxMessageBox(procList);

	return S_OK;
}

STDMETHODIMP CMangoClass::TerminateProcess7(UINT pid, UINT exitCode)
{
	return ::TerminateProcess((void*)pid, exitCode);
}

STDMETHODIMP CMangoClass::GetAsyncKeyState7(int key)
{
	SHORT s = ::GetAsyncKeyState(key);
	return s;
}

void GetBitmapBixel(HBITMAP hBitmap, int xPos, int yPos, CString* result)//, pinvoke_call * arraY, pinvoke_call * arraY2)
{
  HDC hDC = GetDC(NULL);

  if(!hDC)
	MessageBox(NULL, TEXT("hDC = NULL @ GetBitmapBixel"), TEXT(""), 0);


  HDC hMemDC = CreateCompatibleDC(hDC);

  if(!hMemDC)
	MessageBox(NULL, TEXT("hMemDC = NULL @ GetBitmapBixel"), TEXT(""), 0);

  HBITMAP hOld = (HBITMAP)SelectObject(hMemDC, hBitmap);

  

  //BITMAPINFO BitInfo = {0};
  //ZeroMemory(&BitInfo, sizeof(BITMAPINFO));

  COLORREF* cols = new COLORREF[480 * 800 + 8];
  int indexA = 0;
  for(int x = 0; x < 480; x++)
  {
	  for(int y = 0; y < 800; y++)
	  {
		  cols[indexA++] = GetPixel(hMemDC, x, y); 
	  }
  }
 
  CString c;
  c.AppendFormat(TEXT("%d"), cols);
  *result = c;


  SelectObject(hMemDC, hOld);
  DeleteDC(hMemDC);
  ReleaseDC(NULL, hDC);
}

HBITMAP CopyScreenToBitmap(LPRECT lpRect)
{
    HDC         hScrDC, hMemDC;         // screen DC and memory DC
    HBITMAP     hBitmap, hOldBitmap;    // handles to deice-dependent bitmaps
    int         nX, nY, nX2, nY2;       // coordinates of rectangle to grab
    int         nWidth, nHeight;        // DIB width and height
    int         xScrn, yScrn;           // screen resolution

    // check for an empty rectangle

    if (IsRectEmpty(lpRect))
      return NULL;

    // create a DC for the screen and create
    // a memory DC compatible to screen DC

    hScrDC = CreateDC(TEXT("DISPLAY"), NULL, NULL, NULL);

	if(hScrDC == NULL)
		MessageBox(NULL, TEXT("hScrDC = NULL @ CopyScreenToBitmap"), TEXT(""), 0);

    hMemDC = CreateCompatibleDC(hScrDC);

	if(hMemDC == NULL)
		MessageBox(NULL, TEXT("hMemDC = NULL @ CopyScreenToBitmap"), TEXT(""), 0);

    // get points of rectangle to grab

    nX = lpRect->left;
    nY = lpRect->top;
    nX2 = lpRect->right;
    nY2 = lpRect->bottom;

    // get screen resolution

    xScrn = GetDeviceCaps( (HDC) hScrDC, (int) HORZRES);
    yScrn = GetDeviceCaps( (HDC) hScrDC, (int) VERTRES);

    //make sure bitmap rectangle is visible

    if (nX < 0)
        nX = 0;
    if (nY < 0)
        nY = 0;
    if (nX2 > xScrn)
        nX2 = xScrn;
    if (nY2 > yScrn)
        nY2 = yScrn;

    nWidth = nX2 - nX;
    nHeight = nY2 - nY;

    // create a bitmap compatible with the screen DC
    hBitmap = CreateCompatibleBitmap(hScrDC, nWidth, nHeight);

	if(hMemDC == NULL)
		MessageBox(NULL, TEXT("hBitmap = NULL @ CopyScreenToBitmap"), TEXT(""), 0);

    // select new bitmap into memory DC
    hOldBitmap = (HBITMAP) SelectObject(hMemDC, hBitmap);
	
	if(!hOldBitmap)
		MessageBox(NULL, TEXT("hOldBitmap = NULL @ CopyScreenToBitmap"), TEXT(""), 0);


    // bitblt screen DC to memory DC
    BOOL bit = BitBlt(hMemDC, 0, 0, nWidth, nHeight, hScrDC, nX, nY, (DWORD) SRCCOPY);

	if(bit == 0)
		MessageBox(NULL, TEXT("bit = NULL @ CopyScreenToBitmap"), TEXT(""), 0);

    // select old bitmap back into memory DC and get handle to
    // bitmap of the screen

    hBitmap = (HBITMAP) SelectObject(hMemDC, hOldBitmap);

	if(!hBitmap)
		MessageBox(NULL, TEXT("hBitmap = NULL @ CopyScreenToBitmap"), TEXT(""), 0);


    // clean up

    if(DeleteDC(hScrDC) == 0)
		MessageBox(NULL, TEXT("DeleteDC 0 = NULL @ CopyScreenToBitmap"), TEXT(""), 0);
    if(DeleteDC(hMemDC) == 0)
		MessageBox(NULL, TEXT("DeleteDC 1 = NULL @ CopyScreenToBitmap"), TEXT(""), 0);
    // return handle to the bitmap

    return hBitmap;
}

STDMETHODIMP CMangoClass::Crnch(BSTR * result)//, pinvoke_call *  arraY, pinvoke_call *  arraY2)
{
   HDC hScrDC = CreateDC(TEXT("DISPLAY"), NULL, NULL, NULL);

   if(hScrDC == NULL)
		MessageBox(NULL, TEXT("hScrDC = NULL @ Crnch"), TEXT(""), 0);

   int xScrn = GetDeviceCaps(hScrDC, (int) HORZRES);
   int yScrn = GetDeviceCaps(hScrDC, (int) VERTRES);
   RECT rCaptureRect;

   BOOL rec = SetRect(&rCaptureRect, 0, 0 , xScrn, yScrn );
	if(rec == 0)
		MessageBox(NULL, TEXT("rec = NULL @ Crnch"), TEXT(""), 0);

   HBITMAP hBmp = CopyScreenToBitmap(&rCaptureRect);

   CString c = NULL;
 /*  pinvoke_call ca;
   ca.a[0] = 100;

   *arraY = ca;*/

   GetBitmapBixel(hBmp,0,0, &c);

 /*CString k;
   k.AppendFormat(TEXT("%d", c.GetLength()));*/
 //  
 //  //MessageBox(NULL, c.AllocSysString(), TEXT(""), 0);

   *result = c.AllocSysString();

   //DeleteDC(hBmp);
   return 0;
	/*ScreenRecorderWin32 rec(0);
	unsigned char* buffer = new unsigned char[480*800+8];
	ZeroMemory(buffer, 480*800+8);
	bool ok = rec.Capture(buffer, 480,800,NULL);
	return 0;*/
   
}
STDMETHODIMP CMangoClass::QueryPerformanceCounter7(unsigned __int64* result)
{
	//unsigned __int64 nCtr = 0, nFreq = 0, nCtrStop = 0;
	//if(!QueryPerformanceFrequency((LARGE_INTEGER *) &nFreq)) return 0;
	unsigned __int64 nCtrStop = 0;
	BOOL c = QueryPerformanceCounter((LARGE_INTEGER *) &nCtrStop);
	*result = nCtrStop;

	return c;
	//nCtrStop += nFreq;
}
//
//_declspec(naked) ULONGLONG GetCycleCount()
//{
//	int val;
//	__emit(0x0f);
//	__emit(0x31); // RDTSC
//	//__emit("MOV nTs1, EAX");
//	//_asm rdtsc;
//	//return val;
//}



//
STDMETHODIMP CMangoClass::SetWindowsHookExW(int key, int pre)
{
	return S_OK;
	//SetWindowsHookExW @ coredll.dll \ S000
}
//
STDMETHODIMP CMangoClass::ShellExecuteEx7(BSTR file, BSTR args)
{
	//ShExecInfo.cbSize = sizeof(SHELLEXECUTEINFO);
	//ShExecInfo.fMask = NULL;
	//ShExecInfo.hwnd = NULL;
	//ShExecInfo.lpVerb = NULL;
	//ShExecInfo.lpFile = pszParseName;
	//ShExecInfo.lpParameters = NULL;
	//ShExecInfo.lpDirectory = NULL;
	//ShExecInfo.nShow = SW_MAXIMIZE;
	//ShExecInfo.hInstApp = NULL;

	SHELLEXECUTEINFO lpExecInfo;
	memset(&lpExecInfo, 0, sizeof(SHELLEXECUTEINFO));

	lpExecInfo.cbSize = sizeof(SHELLEXECUTEINFO);
	lpExecInfo.lpFile = file;//_T("\\Windows\\Calc7.exe");
	lpExecInfo.lpParameters = _T("");
	lpExecInfo.lpDirectory = _T("");
	lpExecInfo.lpVerb = _T("open");
	lpExecInfo.nShow = SW_MINIMIZE;
	lpExecInfo.fMask = 0;
	lpExecInfo.hwnd = NULL;
	lpExecInfo.hInstApp = NULL;//AfxGetInstanceHandle();

	if(!ShellExecuteEx(&lpExecInfo))
	{
		DWORD err = GetLastError();

		return (HRESULT)err;
	}

	return S_OK;

	
}
//
STDMETHODIMP CMangoClass::CreateProcess7(BSTR file, BSTR args)
{
	STARTUPINFO siStartupInfo;
    PROCESS_INFORMATION piProcessInfo;

    memset(&siStartupInfo, 0, sizeof(siStartupInfo));
    memset(&piProcessInfo, 0, sizeof(piProcessInfo));

    siStartupInfo.cb = sizeof(siStartupInfo);

	BOOL ok = CreateProcess(file ,NULL, NULL, NULL, FALSE, NULL, NULL, NULL, &siStartupInfo, &piProcessInfo);
	if(!ok
		
		//CreateProcess(
		//file, 
		//args, 
		//0,
		//0, 
		//FALSE, 
		//0x04000000 /*CREATE_DEFAULT_ERROR_MODE*/,
		//0,
		//0,
		//&siStartupInfo,
		//&piProcessInfo)
		)
	{
		DWORD err = GetLastError();
		return (HRESULT)err;
	}

	return S_OK;
}
//	
//BOOL WINAPI GetProcessTimes(
//  __in   HANDLE hProcess,
//  __out  LPFILETIME lpCreationTime,
//  __out  LPFILETIME lpExitTime,
//  __out  LPFILETIME lpKernelTime,
//  __out  LPFILETIME lpUserTime
//);

//typedef struct _FILETIME {
//    DWORD dwLowDateTime;
//    DWORD dwHighDateTime;
//} FILETIME, *PFILETIME, *LPFILETIME;

typedef BOOL (WINAPI *GetProcessTimes)(
  __in   HANDLE hProcess,
  __out  LPFILETIME lpCreationTime,
  __out  LPFILETIME lpExitTime,
  __out  LPFILETIME lpKernelTime,
  __out  LPFILETIME lpUserTime
);

STDMETHODIMP CMangoClass::GetProcessTimes7(INT_PTR hProcess, 
											 int * ctime1, int * ctime2, 
											 int * etime1, int * etime2,
											 int * ktime1, int * ktime2,
											 int * utime1, int * utime2)
{
	HMODULE dll = ::LoadLibrary(TEXT("coredll"));

	GetProcessTimes times = (GetProcessTimes)GetProcAddress(dll, TEXT("GetProcessTimes"));

	FILETIME create;
	FILETIME eXit;
	FILETIME kernel;
	FILETIME user;

	BOOL result = times((HANDLE)hProcess, &create, &eXit, &kernel, &user);

	*ctime1 = create.dwLowDateTime;
	*ctime2 = create.dwHighDateTime;

	*etime1 = eXit.dwLowDateTime;
	*etime2 = eXit.dwHighDateTime;

	*ktime1 = kernel.dwLowDateTime;
	*ktime2 = kernel.dwHighDateTime;

	*utime1 = user.dwLowDateTime;
	*utime2 = user.dwHighDateTime;

	return result;
}

///*
//[DllImport("frame.dll", EntryPoint="SHPMNavigateToExternalPage", CharSet=CharSet.Unicode)]
//internal static extern int NavigateToExternalPage(
//
//ShellPageManagerHandle spmh, string pageUri, [MarshalAs(UnmanagedType.Bool)] bool fIsInvocation, byte[] pPageArgs, int cbPageArgs);
//*/
typedef BOOL (WINAPI *NavigateToExternalPage)(
  __in  INT_PTR safeHandle,
  __in  BSTR pageUri,
  __in  BOOL fIsInvocation,
  __in  unsigned char pPageArgs[1],
  __in  int cbPageArgs
);

STDMETHODIMP CMangoClass::NavigateToExternalPage7(INT_PTR safeHandle, BSTR pageUri, BOOL fIsInvocation, /*byte[] pPageArgs,*/ int cbPageArgs)
{
	//DllImportCaller.NativeMethodExists("frame", "SHPMNavigateToExternalPage") (OK)

	HMODULE dll = ::LoadLibrary(TEXT("frame"));

	NavigateToExternalPage page = (NavigateToExternalPage)GetProcAddress(dll, TEXT("SHPMNavigateToExternalPage"));

	unsigned char bytes[1];
	return page(safeHandle, pageUri, fIsInvocation, bytes, cbPageArgs);
}
//
STDMETHODIMP CMangoClass::MessageBox7(LPCTSTR lpText, LPCTSTR lpCaption, UINT uType, int * result)
{
	int val = MessageBox(NULL, lpText, lpCaption, uType);;
	*result = val;

	return val ? S_OK : S_FALSE;
}
////
STDMETHODIMP CMangoClass::ShutdownOS(UINT ewxCode)
{
	return ::ExitWindowsEx(ewxCode/*EWX_POWEROFF*/, 0);
}
//
typedef BOOL (WINAPI *CeCreateProcessExBox)(
  LPCWSTR lpApplicationName, 
  LPCWSTR lpCommandLine, 
  LPSECURITY_ATTRIBUTES lpProcessAttributes, 
  LPSECURITY_ATTRIBUTES lpThreadAttributes, 
  BOOL bInheritHandles, 
  DWORD dwCreationFlags, 
  LPVOID lpEnvironment, 
  LPWSTR lpCurrentDirectory, 
  LPSTARTUPINFO lpStartupInfo, 
  LPPROCESS_INFORMATION lpProcessInformation 
);

STDMETHODIMP CMangoClass::CeCreateProcessEx()
{
	HMODULE dll = ::LoadLibrary(TEXT("coredll"));

	STARTUPINFO siStartupInfo;
    PROCESS_INFORMATION piProcessInfo;

    memset(&siStartupInfo, 0, sizeof(siStartupInfo));
    memset(&piProcessInfo, 0, sizeof(piProcessInfo));

    siStartupInfo.cb = sizeof(siStartupInfo);

	CeCreateProcessExBox create = (CeCreateProcessExBox)GetProcAddress(dll, TEXT("CeCreateProcessEx"));

	return create(TEXT("calc7.exe"), TEXT(""), NULL, NULL, 0, 0, NULL, NULL, &siStartupInfo, &piProcessInfo);

}
//
STDMETHODIMP CMangoClass::GetLastError7()
{
	return ::GetLastError();
}
//
STDMETHODIMP CMangoClass::StringCall(LPCTSTR dll, LPCTSTR method, LPCTSTR value)
{
	return DLLCALL_PARAM<UINT, LPCTSTR>(dll, method, value, true);
}
//
typedef BOOL (WINAPI *NavigateToSafeURI)(
  __in   INT_PTR context,
  __in  LPWSTR location,
  __in  LPWSTR target,
  __in  UINT checkUserInitiatedAction
);

STDMETHODIMP CMangoClass::Tests()
{
	HMODULE dll = ::LoadLibrary(TEXT("agcore"));

	NavigateToSafeURI url = (NavigateToSafeURI)GetProcAddress(dll, TEXT("NavigateToSafeURI"));

	INT_PTR context = 1;
	UINT check = TRUE;
	return url(context, TEXT("http://google.com\0"), TEXT("_self\0"), check);
}
//
typedef BOOL (WINAPI *CreateDirectoryPath)(
  __in   LPCTSTR lpPathName,
  __in  LPSECURITY_ATTRIBUTES lpSecurityAttributes
);

STDMETHODIMP CMangoClass::CreateDirectoryPath7(LPCTSTR lpPathName)
{
	HMODULE dll = ::LoadLibrary(TEXT("shcore.dll"));

	CreateDirectoryPath path = (CreateDirectoryPath)GetProcAddress(dll, TEXT("CreateDirectoryPath"));

	LPSECURITY_ATTRIBUTES attr;

	return path(lpPathName, attr);

}
//
typedef void (WINAPI *NdisGetSystemUpTimeEx)(
   __out  PLARGE_INTEGER pSystemUpTime
);

STDMETHODIMP CMangoClass::NdisGetSystemUpTimeEx7(int part)
{
	LARGE_INTEGER save;
	

	HMODULE dll = ::LoadLibrary(TEXT("ndis"));

	NdisGetSystemUpTimeEx net = (NdisGetSystemUpTimeEx)GetProcAddress(dll, TEXT("NdisGetSystemUpTime"));


	net(&save);

	if(part == 0)
	{
		return save.HighPart;
	}
	else
	{
		return save.LowPart;
	}
}
//
//
//
FILE* handleToFILE(int handle)
{
	int sig = static_cast<long long>(handle);
	FILE* f = reinterpret_cast<FILE*>(sig);

	return f;
}

int FILEToHandle(FILE* f)
{
	long long sig1 = reinterpret_cast<long long> (f);
	int sig = static_cast<int>(sig1);
	
	return sig;
}

void* handleToVOID(int handle)
{
	int sig = static_cast<long long>(handle);
	void* f = reinterpret_cast<void*>(sig);

	return f;
}


int VOIDToHandle(void* f)
{
	long long sig1 = reinterpret_cast<long long> (f);
	int sig = static_cast<int>(sig1);
	
	return sig;
}
//
//
STDMETHODIMP CMangoClass::fopen7(BSTR filename, BSTR mode)
{
	char* fn = ConvertBSTRToLPSTR(filename);
	char* m = ConvertBSTRToLPSTR(mode);
	//const char * arg1 = ::OLE2T(filename);
	//const char * arg2 = ::OLE2T(mode);
	FILE* f = fopen(fn, m);
	
	return FILEToHandle(f);
}

STDMETHODIMP CMangoClass::fclose7(int handle)
{
	return fclose(handleToFILE(handle));
}
//
//
STDMETHODIMP CMangoClass::fseek7(int handle, long int offset, int origin) 
{
    return fseek(handleToFILE(handle), offset, origin);
}

STDMETHODIMP CMangoClass::fgetc7(int handle) 
{
	return fgetc(handleToFILE(handle));
}
//
//
STDMETHODIMP CMangoClass::fsize7(int handle) 
{
	DWORD size;
	if(!GetFileSize(handleToFILE(handle), &size))
	{
		return -1;
	}

	return size;
}
//
STDMETHODIMP CMangoClass::fputc7(int handle, char value) 
{
	return fputc(value, handleToFILE(handle));
}
//
STDMETHODIMP CMangoClass::ddirFiles7(BSTR fullFolderName, BSTR* result)
{
	return (HRESULT)3;
	WIN32_FIND_DATA FindFileData; 
	HANDLE hFind = INVALID_HANDLE_VALUE; 
	//char DirSpec[MAX_PATH]; // directory specification 
	//
	//WideCharToMultiByte(CP_ACP, NULL, fullFolderName, -1, DirSpec, MAX_PATH, NULL, NULL);

	//strncat(DirSpec, "\\*", 3);
	hFind = FindFirstFile(fullFolderName, &FindFileData);


	
	if(hFind == INVALID_HANDLE_VALUE)
	{
		return (HRESULT)3;
	}

	


	CString procList(TEXT(""));

	while(FindNextFile(hFind, &FindFileData) != 0)
	{
		procList.AppendFormat(TEXT("%s//%d//%d//%d//%d//%d//%d//%d//%d//%d//%d"),
			FindFileData.cFileName,
			FindFileData.dwFileAttributes,
			FindFileData.dwOID,
			FindFileData.ftCreationTime.dwHighDateTime,
			FindFileData.ftCreationTime.dwLowDateTime,
			FindFileData.ftLastAccessTime.dwHighDateTime,
			FindFileData.ftLastAccessTime.dwLowDateTime,
			FindFileData.ftLastWriteTime.dwHighDateTime,
			FindFileData.ftLastWriteTime.dwLowDateTime,
			FindFileData.nFileSizeHigh,
			FindFileData.nFileSizeLow);
	}

	FindClose(hFind);

	*result = (procList.AllocSysString());

	return (HRESULT)0;
}
//
STDMETHODIMP CMangoClass::dirFiles7(BSTR fullFolderName, BSTR* result)
{
	//return (HRESULT)3;
	WIN32_FIND_DATA FindFileData; 
	HANDLE hFind = INVALID_HANDLE_VALUE; 

	hFind = FindFirstFile(fullFolderName, &FindFileData);


	
	if(hFind == INVALID_HANDLE_VALUE)
	{
		return (HRESULT)-1;
	}
}
//
STDMETHODIMP CMangoClass::FindFirstFile7(BSTR dir, WIN32_FIND_DATA * result)
{
	return VOIDToHandle(FindFirstFile(dir, result));
}
STDMETHODIMP CMangoClass::FindNextFile7(int handle, WIN32_FIND_DATA * result)
{
	return FindNextFile((HANDLE)handleToVOID(handle), result);
}

STDMETHODIMP CMangoClass::FindClose7(int handle)
{
	return FindClose((HANDLE)handleToVOID(handle));
}

STDMETHODIMP CMangoClass::UintCall(LPCTSTR dll, LPCTSTR method, UINT value)
{
	return DLLCALL_PARAM<UINT, UINT>(dll, method, value, true);
}
//
STDMETHODIMP CMangoClass::WindowTextOfHWND(int hwnd /*GetForegroundWindow e.g.*/, BSTR * result, bool setOrGet)
{
	if(setOrGet) // set
	{
		return SetWindowText((HWND)hwnd, *result);
	}
	else // get
	{
		WCHAR title[MAX_PATH];
		int val = GetWindowText((HWND)hwnd, &title[0], GetWindowTextLength((HWND)hwnd) + 1);
		*result = CString(title).AllocSysString();
		return val;
	}
}

STDMETHODIMP CMangoClass::BoolCall(LPCTSTR dll, LPCTSTR method, bool value)
{
	return DLLCALL_PARAM<UINT, bool>(dll, method, value, true);
}
//
//typedef bool (CALLBACK *EnumWindowsCallback)(
//   __in int callptr,
//   __in int lParam
//);


CString wndows;
BOOL justWindows(HWND hWnd, LPARAM lParam)
{
	if(IsWindow(hWnd))
	{
		wndows.AppendFormat(TEXT("%d\n"), (int)hWnd);
	}
	return true;
}

//BOOL allHwnd(HWND hWnd, LPARAM lParam)
//{
//	wndows.AppendFormat(TEXT("%d\n"), (int)hWnd);
//	return true;
//}

	 /*
	  CWnd* wnd = CWnd::FromHandle(hWnd);
	  wnd->*/

STDMETHODIMP CMangoClass::EnumWindows7(bool onlyIsWindow, BSTR * result)
{
	//WNDENUMPROC
	wndows.Empty();

	WNDENUMPROC p = justWindows;
	
	/*if(onlyIsWindow)
		p = justWindows;
	else
		p = allHwnd;*/

	BOOL val = EnumWindows(p, 0);

	*result = wndows.AllocSysString();

	return val;
}
//
STDMETHODIMP CMangoClass::TextForHWNDEnumWindows(int hwnd, BSTR * result)
{
	CWindow c((HWND)hwnd);
	//CWnd* c = CWnd::FromHandle((HWND)hwnd);
	if(c == NULL)
	{
		return false;
	}
	else
	{
		CString str;
		c.GetWindowTextW(str);
		
		*result = str.AllocSysString();

		return true;
	}
}

STDMETHODIMP CMangoClass::GetClassNameForHWNDEnumWindows(int hwnd, BSTR * result)
{
	//char* szClass	= (char*)malloc(256);
	LPWSTR wClass	= (LPWSTR)malloc(256);

	int val = GetClassName((HWND)hwnd, wClass, 256);

	CString str(wClass);
	*result = str.AllocSysString();

	return true;
}
//
STDMETHODIMP CMangoClass::IntCall(LPCTSTR dll, LPCTSTR method, int value)
{
	return DLLCALL_PARAM<UINT, int>(dll, method, value, true);
}

STDMETHODIMP CMangoClass::IntDualCall(LPCTSTR dll, LPCTSTR method, int value, int value2)
{
	return DLLCALL_PARAM_DUAL<UINT, int>(dll, method, value, value2, true);
}
STDMETHODIMP CMangoClass::EnumDisplayDevices7(DWORD id, DISPLAY_DEVICE * deviceResult, DWORD flags)
{
	return EnumDisplayDevices(NULL, id, deviceResult,flags);
}
//
STDMETHODIMP CMangoClass::waveOutGetVolume7(DWORD * result)
{
	DWORD d;
	UINT val = waveOutGetVolume((HWAVEOUT)0, &d);
	*result = d;

	return val;
}
STDMETHODIMP CMangoClass::waveOutSetVolume7(DWORD value)
{
	return waveOutSetVolume(0, value);
}
//
STDMETHODIMP CMangoClass::waveOutSetPitch7(DWORD value)
{
	return waveOutSetPitch(0, value);
}

STDMETHODIMP CMangoClass::waveGetHWAVEOUT()
{
	WAVEFORMATEX m_WFX;
	HWAVEOUT g_hWaveOut;  
	UINT result = waveOutOpen(&g_hWaveOut, WAVE_MAPPER, &m_WFX, NULL, NULL, WAVE_ALLOWSYNC);

	HWAVEOUT__ v = *g_hWaveOut;

	return (HRESULT)&v;
}
//
typedef uint (WINAPI *KernelIoControlCall)(
   uint dwIoControlCode,
   INT_PTR lpInBuf,
   uint nInBufSize,
   INT_PTR lpOutBuf,
   uint nOutBufSize,
   uint * lpBytesReturned
);

STDMETHODIMP CMangoClass::KernelIoControl(
			uint dwIoControlCode,
            INT_PTR lpInBuf,
            uint nInBufSize,
            INT_PTR lpOutBuf,
            uint nOutBufSize,
            uint * lpBytesReturned /*C# ref*/)
{
	HMODULE dll = ::LoadLibrary(TEXT("coredll"));

	KernelIoControlCall kernel = (KernelIoControlCall)GetProcAddress(dll, TEXT("KernelIoControl"));

	return kernel(dwIoControlCode, lpInBuf, nInBufSize, lpOutBuf, nOutBufSize, lpBytesReturned);
}
//
STDMETHODIMP CMangoClass::GetSystemPowerStatusEx7(BSTR * result, bool fUpdate)
{
	SYSTEM_POWER_STATUS_EX sysPowerStatus = {0};
	BOOL bRet = GetSystemPowerStatusEx(&sysPowerStatus, fUpdate);

	CString c;

	c.AppendFormat(TEXT("%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d"), 
		sysPowerStatus.ACLineStatus, 
		sysPowerStatus.BackupBatteryFlag,
		sysPowerStatus.BackupBatteryFullLifeTime,
		sysPowerStatus.BackupBatteryLifePercent,
		sysPowerStatus.BackupBatteryLifeTime,
		sysPowerStatus.BatteryFlag,
		sysPowerStatus.BatteryFullLifeTime,
		sysPowerStatus.BatteryLifePercent,
		sysPowerStatus.BatteryLifeTime,
		
		sysPowerStatus.Reserved1,
		sysPowerStatus.Reserved2,
		sysPowerStatus.Reserved3);

	*result = c.AllocSysString();
	/*BOOL val = GetSystemPowerStatusEx(*holder, fUpdate);

	CString c;
	c.AppendFormat(TEXT("%d"), result->BatteryLifePercent);

	::MessageBox(NULL, c, c, 0);

	return val;*/
	return bRet;
}

typedef uint (WINAPI *StringIntInt)(
   BSTR str,
   int int1,
   int int2
);

//Used for wifi most
STDMETHODIMP CMangoClass::StringIntIntCall(LPCTSTR dll, LPCTSTR method, BSTR str, int int1, int int2)
{
	return LoadMethod<StringIntInt>(dll, method)(str, int1, int2);
}
//
typedef uint (WINAPI *StringIntIntOut)(
   BSTR str,
   int int1,
   int * int2
);

//Used for wifi most
STDMETHODIMP CMangoClass::StringIntIntOutCall(LPCTSTR dll, LPCTSTR method, BSTR str, int int1, int * int2)
{
	return LoadMethod<StringIntIntOut>(dll, method)(str, int1, int2);
}
//
#define QUERYESCSUPPORT 8 
#define CONTRASTCOMMAND 6149 
#define CONTRAST_CMD_GET 0 
#define CONTRAST_CMD_SET 1 
#define CONTRAST_CMD_INCREASE 2 
#define CONTRAST_CMD_DECREASE 3 
#define CONTRAST_CMD_DEFAULT 4 

struct ContrastCmdInputParm { 
	int command; 
	int parm; 
}; 

STDMETHODIMP CMangoClass::ScreenSetContrast(DWORD dwContrast /*0 to 255*/)
{
	ContrastCmdInputParm szContrast; 
	HDC hdcLCD = CreateDC(NULL, NULL, NULL, NULL); 
	szContrast.command = CONTRAST_CMD_SET; 
	szContrast.parm = dwContrast; 
	dwContrast = ExtEscape(
		hdcLCD, 
		CONTRASTCOMMAND, 
		sizeof(struct ContrastCmdInputParm), 
		(char *)&szContrast, 
		0, 
		NULL);

	return dwContrast;
}

STDMETHODIMP CMangoClass::ScreenGetContrast(/*returns 0 to 255*/)
{
	ContrastCmdInputParm szContrast; 
	HDC hdcLCD; 
	DWORD dwContrast = -1; 
	int i; 
	// Do we have support for get mode info 
	i = CONTRASTCOMMAND; 
	hdcLCD = CreateDC(NULL, NULL, NULL, NULL); 

	if (hdcLCD) 
	{ 
		if (ExtEscape(hdcLCD, QUERYESCSUPPORT, 4, (LPCSTR) &i, 0, 0))
		{ 
			szContrast.command = CONTRAST_CMD_GET; 
			szContrast.parm = 0; 
			dwContrast = ExtEscape(
				hdcLCD, 
				CONTRASTCOMMAND, 
				sizeof(struct ContrastCmdInputParm), 
			    (char *)&szContrast, 
				0,
				NULL); 

			return dwContrast;
		}
	}

	return (HRESULT)-1;
}
//

//
//#define MYMSG_TASKBARNOTIFY  (WM_USER + 100)
//
//#define WH_MIN      				(-1)
//#define WH_MSGFILTER    		(-1)
//#define WH_JOURNALRECORD  		0
//#define WH_JOURNALPLAYBACK  	1
//#define WH_KEYBOARD    			2
//#define WH_GETMESSAGE   		3
//#define WH_CALLWNDPROC   		4
//#define WH_CBT      				5
//#define WH_SYSMSGFILTER   		6
//#define WH_MOUSE     			7
//#define WH_HARDWARE    			8
//#define WH_DEBUG     			9
//#define WH_SHELL     			10
//#define WH_FOREGROUNDIDLE  	11
//#define WH_MAX      				11
//
//#define WH_KEYBOARD_LL   		20
//
//#define HC_ACTION           	0
//#define HC_GETNEXT          	1
//#define HC_SKIP             	2
//#define HC_NOREMOVE         	3
//#define HC_SYSMODALON       	4
//#define HC_SYSMODALOFF      	5
//
//#define HC_NOREM            	HC_NOREMOVE
//
//#define WH_CBT              	5
//#define GWL_HINSTANCE       	-6
//#define HCBT_ACTIVATE       	5
//
//// Used by WH_KEYBOARD_LL
//#define LLKHF_EXTENDED       	(KF_EXTENDED >> 8)
//#define LLKHF_INJECTED       	0x00000010
//#define LLKHF_ALTDOWN        	(KF_ALTDOWN >> 8)
//#define LLKHF_UP             	(KF_UP >> 8)
//#define LLMHF_INJECTED       	0x00000001
//


typedef HRESULT (WINAPI *IntQuad)(
   int int1, 
   int int2, 
   int int3, 
   int int4
);

STDMETHODIMP CMangoClass::IntQuadCall(LPCTSTR dll, LPCTSTR method, int int1, int int2, int int3, int int4)
{
	return LoadMethod<IntQuad>(dll, method)(int1, int2, int3, int4);
}
//
//
STDMETHODIMP CMangoClass::PhoneMakeCall7(BSTR number)
{
	PHONEMAKECALLINFO call;
    memset(&call, 0x0, sizeof(PHONEMAKECALLINFO));
    call.cbSize = sizeof(PHONEMAKECALLINFO);
    call.dwFlags = PMCF_DEFAULT;
    call.pszDestAddress = number;
    return PhoneMakeCall(&call);
}
//
STDMETHODIMP CMangoClass::SetSystemTime7(WORD wYear, WORD wMonth, WORD wDayOfWeek, WORD wDay, WORD wHour, WORD wMinute, WORD wSecond, WORD wMilliseconds)
{
	SYSTEMTIME time;
	time.wYear = wYear;
	time.wMonth = wMonth;
	time.wDayOfWeek = wDayOfWeek;
	time.wDay = wDay;
	time.wHour = wHour;
	time.wMinute = wMinute;
	time.wSecond = wSecond;
	time.wMilliseconds = wMilliseconds;

	BOOL t = SetSystemTime(&time);


	return t;
}

typedef uint (WINAPI *StringString)(
   BSTR str,
   BSTR str2
);

STDMETHODIMP CMangoClass::StringStringCall(LPCTSTR dll, LPCTSTR method, BSTR value, BSTR value2)
{
	return LoadMethod<StringString>(dll, method)(value, value2);
	 
}

typedef DWORD (WINAPI *GetSystemPowerStatusEx2Call)(
	PSYSTEM_POWER_STATUS_EX2 pSystemPowerStatusEx2,
	DWORD dwLen,
	BOOL fUpdate
);

STDMETHODIMP CMangoClass::GetSystemPowerStatusExAdv7(BSTR * result, bool fUpdate)
{
	SYSTEM_POWER_STATUS_EX2 sysPowerStatus = {0};
	GetSystemPowerStatusEx2Call call = LoadMethod<GetSystemPowerStatusEx2Call>(TEXT("coredll"), TEXT("GetSystemPowerStatusEx2"));

	DWORD res = call(&sysPowerStatus, (DWORD)sizeof(SYSTEM_POWER_STATUS_EX2), fUpdate);

	CString c;

	c.AppendFormat(TEXT("%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d"), 
		sysPowerStatus.ACLineStatus, 
		sysPowerStatus.BackupBatteryFlag,
		sysPowerStatus.BackupBatteryFullLifeTime,
		sysPowerStatus.BackupBatteryLifePercent,
		sysPowerStatus.BackupBatteryLifeTime,
		sysPowerStatus.BatteryFlag,
		sysPowerStatus.BatteryFullLifeTime,
		sysPowerStatus.BatteryLifePercent,
		sysPowerStatus.BatteryLifeTime,

		sysPowerStatus.BatteryVoltage,
		sysPowerStatus.BatteryCurrent,
		sysPowerStatus.BatteryAverageCurrent,
		sysPowerStatus.BatteryAverageInterval,
		sysPowerStatus.BatterymAHourConsumed,
		sysPowerStatus.BatteryTemperature,
		sysPowerStatus.BackupBatteryVoltage,
		sysPowerStatus.BatteryChemistry,

		sysPowerStatus.Reserved1,
		sysPowerStatus.Reserved2,
		sysPowerStatus.Reserved3);

	//c.AppendFormat(TEXT("%d"), res);

	//MessageBox(NULL, c.AllocSysString(), TEXT(""), 0);
	*result = c.AllocSysString();

	return res;
}

STDMETHODIMP CMangoClass::GlobalMemoryStatus7(BSTR * result)
{
	/*
	DWORD dwLength;
    DWORD dwMemoryLoad;
    DWORD dwTotalPhys;
    DWORD dwAvailPhys;
    DWORD dwTotalPageFile;
    DWORD dwAvailPageFile;
    DWORD dwTotalVirtual;
    DWORD dwAvailVirtual;
	*/
	MEMORYSTATUS status;
	GlobalMemoryStatus(&status);

	CString c;


	c.AppendFormat(TEXT("%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d"), 
		status.dwLength,
		status.dwMemoryLoad,
		status.dwTotalPhys,
		status.dwAvailPhys,
		status.dwTotalPageFile,
		status.dwAvailPageFile,
		status.dwTotalVirtual,
		status.dwAvailVirtual);


	*result = c.AllocSysString();

	return 1;
}


typedef BOOL (CALLBACK *CeRunAppAtTimeCall)(
TCHAR *pwszAppName, 
SYSTEMTIME *lpTime);


STDMETHODIMP CMangoClass::CeRunAppAtTime7(BSTR pwszAppName, SYSTEMTIME time)
{
	USES_CONVERSION;
	TCHAR* p = OLE2T(pwszAppName);
	return LoadMethod<CeRunAppAtTimeCall>(TEXT("coredll"),TEXT("CeRunAppAtTime"))(p,&time);	
}

STDMETHODIMP CMangoClass::EnumDisplaySettings7(BSTR lpszDeviceName, DWORD iModeNum, BSTR * lpDevMode)
{
	DEVMODE DeviceMode = { 0 };
	DeviceMode.dmSize = sizeof(DEVMODE);
	BOOL val = EnumDisplaySettings(NULL, ENUM_CURRENT_SETTINGS, &DeviceMode);

	CString c;

	c.AppendFormat(TEXT("%s\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%d\n%s\n%d\n%d\n%d\n%d\n%d\n%d\n%d"), 
		DeviceMode.dmDeviceName,
		DeviceMode.dmSpecVersion,
		DeviceMode.dmDriverVersion,
		DeviceMode.dmSize,
		DeviceMode.dmDriverExtra,
		DeviceMode.dmFields,


		DeviceMode.dmOrientation,
		DeviceMode.dmPaperSize,
		DeviceMode.dmPaperLength,
		DeviceMode.dmPaperWidth,
		DeviceMode.dmScale,
		DeviceMode.dmCopies,

		DeviceMode.dmDefaultSource,
		DeviceMode.dmPrintQuality,
		DeviceMode.dmColor,
		DeviceMode.dmDuplex,
		DeviceMode.dmYResolution,
		DeviceMode.dmTTOption,
		DeviceMode.dmCollate,

		DeviceMode.dmFormName,
		DeviceMode.dmLogPixels,
		DeviceMode.dmBitsPerPel,
		DeviceMode.dmPelsWidth,
		DeviceMode.dmPelsHeight,
		DeviceMode.dmDisplayFlags,
		DeviceMode.dmDisplayFrequency,
		DeviceMode.dmDisplayOrientation);


	*lpDevMode = c.AllocSysString();

	return val;
	//return EnumDisplaySettings(lpszDeviceName, iModeNum, lpDevMode);
}

STDMETHODIMP CMangoClass::GetCommandLine7(BSTR * result)
{
	LPWSTR *szArglist;
	int nArgs;
	int i;

	szArglist = CommandLineToArgvW(GetCommandLineW(), &nArgs);
	CString cl_parameter;
	for( i=0; i<nArgs; i++) 
	{
		cl_parameter.Format(TEXT("%d: %ws\n"), i, szArglist[i]);
		::MessageBox(NULL, cl_parameter, TEXT(""), 0);
	}

	LPWSTR cmd = ::GetCommandLine();
	CString c( GetCommandLine() );

	*result = c.AllocSysString();
	
	return 0;
}

STDMETHODIMP CMangoClass::PostMessage7(int hwnd, uint Msg, WPARAM wParam, WPARAM lParam)
{
	return PostMessage((HWND)handleToVOID(hwnd), Msg, wParam, lParam);
}

STDMETHODIMP CMangoClass::SendMessage7(int hwnd, uint Msg, WPARAM wParam, WPARAM lParam)
{
	return SendMessage((HWND)handleToVOID(hwnd), Msg, wParam, lParam);
}

STDMETHODIMP CMangoClass::ThisTaskHostInformation(LPCTSTR dll, LPCTSTR method, void * _outValue)
{
	return DLLCALL_PARAM_OUT<HRESULT>(dll, method, _outValue);//DLLCALL_VOID<XNASIMPLEVOID>(TEXT("XnaFrameworkCore"), method);
}

STDMETHODIMP CMangoClass::CaptureScreen(HBITMAP *pHandle, INT *pSize, VOID **ppvBits)
{
	int ret = 0;
	HDC hDC = NULL;
	HDC hMemDC = NULL;
	HBITMAP hCapturedBitmap = NULL;
	PVOID pBuffer = NULL;
	HGDIOBJ hOldObj = NULL;

	*pHandle = NULL;
	*pSize = 0;
	*ppvBits = NULL;

	hDC = GetDC(NULL);

	if(!FAILED(hDC))
	{
		int screenWidth = GetDeviceCaps(hDC, HORZRES);
		int screenHeight = GetDeviceCaps(hDC, VERTRES);

		BITMAPINFO dibInfo;
		ZeroMemory(&dibInfo, sizeof(BITMAPINFO));
		dibInfo.bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
		dibInfo.bmiHeader.biWidth = screenWidth;
		dibInfo.bmiHeader.biHeight = -screenHeight;
		dibInfo.bmiHeader.biPlanes = 1;
		dibInfo.bmiHeader.biBitCount = 24;
		dibInfo.bmiHeader.biCompression = BI_RGB;
		dibInfo.bmiHeader.biSizeImage = screenWidth * screenHeight * 3;

		hMemDC = CreateCompatibleDC(hDC);

		if(!FAILED(hMemDC))
		{
			hCapturedBitmap = CreateDIBSection(hMemDC, &dibInfo, DIB_RGB_COLORS, (VOID **)&pBuffer, NULL, 0);
			
			if(!FAILED(hCapturedBitmap))
			{
				hOldObj = SelectObject(hMemDC, hCapturedBitmap);
				
				if(!BitBlt(hMemDC, 0, 0, screenWidth, screenHeight, hDC, 0, 0, SRCCOPY))
					ret = -4;
				
				SelectObject(hMemDC, hOldObj);

				*pHandle = hCapturedBitmap;
				*pSize = dibInfo.bmiHeader.biSizeImage;
				*ppvBits = pBuffer;
			}
			else
				ret = -3;

			DeleteDC(hMemDC);
		}
		else
			ret = -2;

		ReleaseDC(NULL, hDC);
	}
	else
		ret = -1;

	return ret;
}

STDMETHODIMP CMangoClass::DeleteObject(HGDIOBJ hObject)
{
	return ::DeleteObject(hObject);
}

/*__declspec(naked)*/ STDMETHODIMP  CMangoClass::ASMExecute(BYTE * bytes)
{
	//
	//void (*shell)(); // Function pointer.
 //   shell = (void(*)()) (bytes);
	//shell();


	return ((int(*)())(bytes))();

	//__emit(0xe3a00000); // return 0
	//__emit(0xe1a0f00e);//0xe12fff1e); // ^
	
	__emit(0xE1A0F00E); //op return

	return 5; //wont get here

	//__emit(0xe3a00000);
	//__emit(0xe12fff1e);
	
	
	//__emit(0xe1a0f00e);
	/* 
	return NULL;

  00000	e3a00000	 mov         r0, #0    < set return value (0)
  00004	e1a0f00e	 mov         pc, lr	   < return 
	*/
}

#include <Winuser.h>

//void ___hello2() { MessageBox(NULL, TEXT("not used void"), TEXT("not used void"), 0); }

typedef void (WINAPI *CloseClipboardCall)();
void __TEST_CallMeWithINT(int* i)
{
	CString c;
	c.AppendFormat(TEXT("%d"), *i);
	MessageBox(NULL, c.AllocSysString(), TEXT("Int call"), 0);
}
__declspec(dllexport) void ___hello()
{
	void (*func1)(int,int,int,int,int,int,int,int);
	func1 = (void(__cdecl*)(int,int,int,int,int,int,int,int))0x15469548; //(int)(&(GetLastError));
	func1(0x56485598,0x13467954,0x31669741,0x51234879,0x16845965,0x16845965,0x16845965,0x16845965);

	//__emit(0xe52de004);
	//__emit(0xe59f0010);
	//__emit(0xe59f3008);
	//__emit(0xe1a0e00f);
	//__emit(0xe1a0f003);
	//__emit(0xe49df004);
	//__emit(0x061754f8);
	//__emit(0x061754ec);
	//__emit(0xe59f0000);
	//__emit(0xe1a0f00e);

	/*void (*func1)(void);
	func1 = (void(__cdecl*)(void))0x15469548;
	func1();

	void (*func2)(void);
	func2 = (void(__cdecl*)(void))0x46548264;
	func2();

	void (*func)(void);
	func = (void(__cdecl*)(void))0xE52DE003;
	func();
	return;*/
	/*HMODULE dll = ::LoadLibrary(TEXT("coredll"));

	CloseClipboardCall clip = (CloseClipboardCall)GetProcAddress(dll, TEXT("CloseClipboard"));
	clip();
	return;*/
	/*int val;
	MessageBox(0, TEXT("asd"), TEXT("asd"), 0);*/
}


STDMETHODIMP CMangoClass::GetHelloWorldMSGBPtr()
{
	return (int)(&(___hello));
}
STDMETHODIMP CMangoClass::ValueAtAddres(int * addr)
{
	int i = (*addr);
	return i;
}

STDMETHODIMP CMangoClass::AddressOfObject(void * object)
{
	return (int)object;
}

void __TEST_SayHello()
{
	MessageBox(NULL, TEXT("test"), TEXT("test"), 0);
}
STDMETHODIMP CMangoClass::TestFunc1()
{
	return (int)(&(__TEST_SayHello));
}


STDMETHODIMP CMangoClass::TestFunc2()
{
	return (int)(&(__TEST_CallMeWithINT));
}

STDMETHODIMP CMangoClass::DeviceIoControl7(HANDLE hDevice, DWORD dwIoControlCode, LPVOID lpInBuf, DWORD nInBufSize, __out_bcount_opt(nOutBufSize) LPVOID lpOutBuf, DWORD nOutBufSize, LPDWORD lpBytesReturned, LPOVERLAPPED lpOverlapped)
{
	//public static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, [In] byte[] inBuffer, int nInBufferSize, [Out] byte[] outBuffer, int nOutBufferSize, ref int pBytesReturned, IntPtr lpOverlapped);
	return DeviceIoControl(hDevice, dwIoControlCode, lpInBuf, nInBufSize, lpOutBuf, nOutBufSize, lpBytesReturned, lpOverlapped);
}

__declspec(dllexport) void EXP_test1()
{
	MessageBox(NULL, TEXT("Hello from \"EXP_test1\""), TEXT(""), 0);
}

__declspec(dllexport) void EXP_test2(int i)
{
	CString c;
	c.AppendFormat(TEXT("%d"), i);
	MessageBox(NULL, TEXT("Hello from \"EXP_test2\" (params [int i])"), c.AllocSysString(), 0);
}
__declspec(dllexport) void EXP_test3(int * assignToPtr)
{
	*assignToPtr = 1337;
	MessageBox(NULL, TEXT("Hello from \"EXP_test3\". I assigned \"int * assignToPtr\" = 1337"), TEXT(""), 0);
}
__declspec(dllexport) void EXP_test4(BSTR * text)
{
	CString c;
	c.AppendFormat(TEXT("%s"), text);
	MessageBox(NULL, TEXT("Hello from \"EXP_test4\". I say value of \"BSTR * text\""), c.AllocSysString(), 0);
}

LPCWSTR CSharpBase_Text = TEXT("The Text");
LPCWSTR CSharpBase_Capt = TEXT("The Caption");

void a(int a, int b, int c, int d, int e, int f, int g, int h, int i, int j, int k, int l, int m, int n)
{
	CString CC;
	CC.AppendFormat(TEXT("%s"), a, b, c, d, e, f, g, h, i, j, k, l, m, n);
	MessageBox(0, CC.AllocSysString(), CSharpBase_Capt, 0);
	//MessageBox(0, CSharpBase_Text, CSharpBase_Capt, 0);
}
void a(int a, int b, int c, int d, int e, int f)
{
	CString CC;
	CC.AppendFormat(TEXT("%s"), a, b, c, d, e, f);
	MessageBox(0, CC.AllocSysString(), CSharpBase_Capt, 0);
	//MessageBox(0, CSharpBase_Text, CSharpBase_Capt, 0);
}
void CSharpBase()
{
	a(1,2,3,4,5,6);

	//MessageBox(0, CSharpBase_Text, CSharpBase_Capt, 0);
}

struct Time {
    int hours;
};

__declspec(dllexport) void* EXP_test5()
{
	unsigned long lo = 4294967295;
	return &lo;
}

__declspec(dllexport) int add(int param1, int param2)
{
	return param1 + param2;
}
EXTERN_C __declspec(dllexport) int returnNumber()
{
	return 50;
}
EXTERN_C __declspec(dllexport) int createfile(BSTR * port)
{
   CString c;
   c.AppendFormat(TEXT("%s"), port);
   DCB dcb;
   HANDLE hCom;
   BOOL fSuccess;
   TCHAR *pcCommPort = TEXT("COM1"); //  Most systems have a COM1 port

   //  Open a handle to the specified com port.
   hCom = CreateFile( c.AllocSysString(),
                      GENERIC_READ | GENERIC_WRITE,
                      0,      //  must be opened with exclusive-access
                      NULL,   //  default security attributes
                      OPEN_EXISTING, //  must use OPEN_EXISTING
                      0,      //  not overlapped I/O
                      NULL ); //  hTemplate must be NULL for comm devices
   return (int)hCom;
}

typedef void (WINAPI *CSVoidCall)();

EXTERN_C __declspec(dllexport) void Callbacktest(CSVoidCall call)
{
	call();
}
STDMETHODIMP CMangoClass::WriteByte(int * address, BYTE value)
{
	*address = value;
	return 0;
}

typedef BOOL (*FN_CeMountDBVolEx)( PCEGUID pGuid, LPWSTR lpwszDBVol, CEVOLUMEOPTIONS* pOptions, DWORD dwFlags);

STDMETHODIMP CMangoClass::EDB_Mount(LPWSTR databaseFile, bool * sucCode)
{
	FN_CeMountDBVolEx CeMountDBVolEx = (FN_CeMountDBVolEx)GetProcAddress(LoadLibrary(L"coredll.dll"), L"CeMountDBVolEx");

	CEGUID _mvolguid;
	memset(&_mvolguid, 0, sizeof(CEGUID));

	bool ok = 0 != CeMountDBVolEx(&_mvolguid, databaseFile, NULL,OPEN_EXISTING | EDB_MOUNT_FLAG); 
	*sucCode = ok;

	return (HRESULT)&_mvolguid;
}

typedef HANDLE (*FN_CeOpenDatabaseInSession)( HANDLE hSession, PCEGUID pGuid, PCEOID poid, LPWSTR lpwszName, SORTORDERSPECEX* pSort, DWORD dwFlags, CENOTIFYREQUEST* pRequest);

STDMETHODIMP CMangoClass::EDB_OpenMounted(LPWSTR databaseFile, CEGUID * _mvolguid)
{
	return 0;
	//CeFindFirstDatabaseEx(_mvolguid, 0/* = allttpes*/);
	//::CeFindNextDatabaseEx(
	/*int m_ceoidCallLog = 0;
	::CeOpenDatabaseEx2(_mvolguid, */
	//FN_CeOpenDatabaseInSession CeOpenDatabaseInSession = (FN_CeOpenDatabaseInSession)GetProcAddress(LoadLibrary(L"coredll.dll"), L"CeOpenDatabaseInSession");
	//CEOID _dboid;
	//memset(&_dboid, 0, sizeof(CEOID));

	//HANDLE session = CeOpenDatabaseInSession(NULL, _mvolguid, 0 /*open by name >*/, databaseFile, 0, CEDB_AUTOINCREMENT, NULL);

	//return (HRESULT)session;
}
STDMETHODIMP CMangoClass::EDB_FindFirstDB(PCEGUID _mvolguid)
{
	HANDLE hand = CeFindFirstDatabaseEx(0, 0);//0/* = allttpes*/);
	return (HRESULT)hand;
}
STDMETHODIMP CMangoClass::EDB_FindNextDB(HANDLE findFirstHandle, PCEGUID _mvolguid)
{
	CEOID hand = CeFindNextDatabaseEx(findFirstHandle,_mvolguid); //0;//CeFindNextDatabaseEx(findFirstHandle, _mvolguid);
	return (HRESULT)hand;
}
STDMETHODIMP CMangoClass::EDB_OpenDBSession(PCEGUID _mvolguid, PCEOID findHand)
{
	FN_CeOpenDatabaseInSession CeOpenDatabaseInSession = (FN_CeOpenDatabaseInSession)GetProcAddress(LoadLibrary(L"coredll.dll"), L"CeOpenDatabaseInSession");

	HANDLE session = CeOpenDatabaseInSession(NULL, _mvolguid, findHand, NULL, 0, CEDB_AUTOINCREMENT, NULL);
	return (HRESULT)session;
}
STDMETHODIMP CMangoClass::EDB_CloseHandle(HANDLE findFirstHandle)
{
	return (HRESULT)CloseHandle(findFirstHandle);
}

STDMETHODIMP CMangoClass::EDB_CeOidGetInfoEx(PCEGUID _mvolguid, CEOID findHand)
{
	CEOIDINFOEX cedbinfo;
	memset(&cedbinfo, 0, sizeof(CEOIDINFOEX));
	cedbinfo.wVersion = 2;
	
	return (HRESULT)CeOidGetInfoEx2(_mvolguid, findHand, &cedbinfo);
	/*{
		CString str = cedbinfo.infDatabase.szDbaseName;
		MessageBox(NULL, str.AllocSysString(), TEXT(""), 0);
	}
	else
	{
		MessageBox(NULL, TEXT("FAIL"), TEXT("FAIL"), 0);
	}
	return 0;*/
	/*CEOIDINFOEX OIDInfo;
	if(CeOidGetInfoEx2(NULL, findHand, &OIDInfo))
	{
		MessageBox(NULL, TEXT("OK"), TEXT("OK"), 0);
	}
	else
	{
		MessageBox(NULL, TEXT("FAIL"), TEXT("FAIL"), 0);
	}
	return 0;*/
}
STDMETHODIMP CMangoClass::EDB_OpenDBFind(PCEGUID _mvolguid, CEOID findHand)
{
	HANDLE res = CeOpenDatabaseInSession(NULL, _mvolguid, &findHand, NULL, 0, CEDB_AUTOINCREMENT, NULL);
	return (HRESULT)res;
}

STDMETHODIMP CMangoClass::EDB_CeOpenDatabaseEx(PCEGUID _mvolguid, CEOID findHand)
{
	HANDLE hand = CeOpenDatabaseEx(_mvolguid, &findHand, NULL, NULL, CEDB_AUTOINCREMENT, NULL);
	return (HRESULT)hand;
}
//

//typedef void (*voidC)(void); 
//STDMETHODIMP CMangoClass::GetNativeDelegatePtr(voidC * d)
//{
//	return (HRESULT)d;
//	//d();
//	//return 0;
//	//return ((int(*)())(d))();
//}

EXTERN_C __declspec(dllexport) Time* Test_Return_TimeStruct()
{
	Time t = { 0 };
	t.hours = 50;

	return &t;
}
//
//#endif
// CMangoClass
