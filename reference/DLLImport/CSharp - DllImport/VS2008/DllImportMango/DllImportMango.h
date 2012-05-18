
#pragma warning( disable: 4049 )  /* more than 64k source lines */

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 5.03.0286 */
/* at Fri Nov 04 19:25:45 2011
 */
/* Compiler settings for .\DllImportMango.idl:
    Oicf (OptLev=i2), W1, Zp8, env=Win32 (32b run), ms_ext, c_ext
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
//@@MIDL_FILE_HEADING(  )


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 440
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __DllImportMango_h__
#define __DllImportMango_h__

/* Forward Declarations */ 

#ifndef __IMangoClass_FWD_DEFINED__
#define __IMangoClass_FWD_DEFINED__
typedef interface IMangoClass IMangoClass;
#endif 	/* __IMangoClass_FWD_DEFINED__ */


#ifndef __MangoClass_FWD_DEFINED__
#define __MangoClass_FWD_DEFINED__

#ifdef __cplusplus
typedef class MangoClass MangoClass;
#else
typedef struct MangoClass MangoClass;
#endif /* __cplusplus */

#endif 	/* __MangoClass_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 

void __RPC_FAR * __RPC_USER MIDL_user_allocate(size_t);
void __RPC_USER MIDL_user_free( void __RPC_FAR * ); 

#ifndef __IMangoClass_INTERFACE_DEFINED__
#define __IMangoClass_INTERFACE_DEFINED__

/* interface IMangoClass */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IMangoClass;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("A980817D-1DFC-4307-A069-A725E544F79C")
    IMangoClass : public IUnknown
    {
    public:
    };
    
#else 	/* C style interface */

    typedef struct IMangoClassVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *QueryInterface )( 
            IMangoClass __RPC_FAR * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void __RPC_FAR *__RPC_FAR *ppvObject);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *AddRef )( 
            IMangoClass __RPC_FAR * This);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *Release )( 
            IMangoClass __RPC_FAR * This);
        
        END_INTERFACE
    } IMangoClassVtbl;

    interface IMangoClass
    {
        CONST_VTBL struct IMangoClassVtbl __RPC_FAR *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IMangoClass_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IMangoClass_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IMangoClass_Release(This)	\
    (This)->lpVtbl -> Release(This)


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IMangoClass_INTERFACE_DEFINED__ */



#ifndef __DllImportMangoLib_LIBRARY_DEFINED__
#define __DllImportMangoLib_LIBRARY_DEFINED__

/* library DllImportMangoLib */
/* [helpstring][version][uuid] */ 




EXTERN_C const IID LIBID_DllImportMangoLib;

EXTERN_C const CLSID CLSID_MangoClass;

#ifdef __cplusplus

class DECLSPEC_UUID("434B816A-3ADA-4386-8421-33B0E669F3F1")
MangoClass;
#endif
#endif /* __DllImportMangoLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


