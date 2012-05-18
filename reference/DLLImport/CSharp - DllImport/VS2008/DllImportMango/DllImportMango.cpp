// DllImportMango.cpp : Implementation of DLL Exports.


#include "stdafx.h"

#ifdef POCKETPC2003_UI_MODEL
#include "resourceppc.h"
#endif 

#include "DllImportMango.h"


class CDllImportMangoModule : public CAtlDllModuleT< CDllImportMangoModule >
{
public :
	DECLARE_LIBID(LIBID_DllImportMangoLib)
#ifndef _CE_DCOM
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_DLLIMPORTMANGO, "{46F0B1F6-B398-4CEC-821A-1F304E5D9E57}")
#endif
};

CDllImportMangoModule _AtlModule;


// DLL Entry Point
extern "C" BOOL WINAPI DllMain(HANDLE hInstance, DWORD dwReason, LPVOID lpReserved)
{
	hInstance;
    return _AtlModule.DllMain(dwReason, lpReserved); 
}


// Used to determine whether the DLL can be unloaded by OLE
STDAPI DllCanUnloadNow(void)
{
    return _AtlModule.DllCanUnloadNow();
}


// Returns a class factory to create an object of the requested type
STDAPI DllGetClassObject(REFCLSID rclsid, REFIID riid, LPVOID* ppv)
{
    return _AtlModule.DllGetClassObject(rclsid, riid, ppv);
}


// DllRegisterServer - Adds entries to the system registry
STDAPI DllRegisterServer(void)
{
    // registers object, typelib and all interfaces in typelib
    HRESULT hr = _AtlModule.DllRegisterServer();
	return hr;
}


// DllUnregisterServer - Removes entries from the system registry
STDAPI DllUnregisterServer(void)
{
	HRESULT hr = _AtlModule.DllUnregisterServer(FALSE);
	return hr;
}

