using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;

namespace CSharp___DllImport
{
    public delegate void CopyDelegate(IntPtr source, byte[] destination, int startIndex, int length);
    public delegate object PtrToStructureDelegate(IntPtr ptr, Type structureType);
    public delegate int SizeOfDelegate(object structure);
    public delegate string PtrToStringUniDelegate(IntPtr ptr);
    public delegate void StructureToPtrDelegate(object structure, IntPtr ptr, bool fDeleteOld);


    /// <summary>
    /// Wrapper of the leaked securitysafe marshal, called by relection call
    /// </summary>
    public class MarshalBypass
    {
        private static System.Reflection.MethodInfo[] methods;

        static void init()
        {
            var asm = System.Reflection.Assembly.Load("Microsoft.Phone.InteropServices, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e");

            Type comBridgeType = asm.GetType("Microsoft.Phone.InteropServices.Marshal");

            methods = comBridgeType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        }

        private static T make<T>(string name, int index = 0) where T : class
        {
            if (methods == null) init();

            var searchParams = typeof(T).GetMethod("Invoke").GetParameters().Select(t => t.ParameterType).ToArray();

            var names = methods.Where(meth => meth.Name == name);
            var arr = names.Where(meth =>
            {
                var param = meth.GetParameters();
                var filter = param.Select(t => t.ParameterType).ToArray();
                var eq = filter.SequenceEqual(searchParams);
                
                return eq;
            }).ToArray();
            var info = arr[index];

            var act = Delegate.CreateDelegate(typeof(T), info);
            return act as T;
        }

        public static PtrToStructureDelegate PtrToStructure = MarshalBypass.make<PtrToStructureDelegate>("PtrToStructure");
        public static CopyDelegate Copy = MarshalBypass.make<CopyDelegate>("Copy");
        public static SizeOfDelegate SizeOf = MarshalBypass.make<SizeOfDelegate>("SizeOf");
        public static PtrToStringUniDelegate PtrToStringUni = MarshalBypass.make<PtrToStringUniDelegate>("PtrToStringUni");
        public static StructureToPtrDelegate StructureToPtr = MarshalBypass.make<StructureToPtrDelegate>("StructureToPtr");

        public static T PtrToStructureT<T>(IntPtr ptr)
        {
            return (T)PtrToStructure(ptr, typeof(T));
        }

        public static IntPtr AllocMemory(int size)
        {
            return IntPtr.Zero;
        }
    }
    public class GCHandleBypass
    {
        object Handle;
        private GCHandleBypass(object handle)
        {
            Handle = handle;
        }

         private static System.Reflection.MethodInfo cached_alloc;
         private static System.Reflection.MethodInfo cached_addrOfPinned;
         public static void initCopy()
         {
             var asm = System.Reflection.Assembly.Load("Microsoft.Phone.InteropServices, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e");
             
             Type comBridgeType = null;

             comBridgeType = asm.GetType("Microsoft.Phone.InteropServices.GCHandle");
             {
                 var dynMethod = comBridgeType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                 cached_alloc = dynMethod[0];
             }
             {
                 var dynMethod = comBridgeType.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                 cached_addrOfPinned = dynMethod[3];
             }
         }
         public static GCHandleBypass Alloc(object obj, System.Runtime.InteropServices.GCHandleType type)
         {
             if (cached_alloc == null)
                 initCopy();

             return new GCHandleBypass(cached_alloc.Invoke(null, new object[] { obj, type }));
         }
         public IntPtr AddrOfPinnedObject()
         {
             var obj = cached_addrOfPinned.Invoke(Handle, new object[0]);
             return (IntPtr)obj;
         }
    }
}
