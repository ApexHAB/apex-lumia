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

namespace CSharp___DllImport.ASM_JIT
{
    public class Tests
    {
        void exp_test1_test()
        {
            var EXP_test1 = ASMGenerator.PtrForFunc("DllImportMango" /*_this_ homebrew dll*/, "?EXP_test1@@YAXXZ");

            var gen = new ASMGenerator();
            gen.TestCall(EXP_test1, new object[0]);
            var ops = gen.ToOpcodeStrings();
            gen.Execute();
        }

        void exp_test2_test()
        {
            var EXP_test1 = ASMGenerator.PtrForFunc("DllImportMango" /*_this_ homebrew dll*/, "?EXP_test2@@YAXH@Z");

            var gen = new ASMGenerator();
            gen.TestCall(EXP_test1, 10);
            var ops = gen.ToOpcodeStrings();
            gen.Execute();
        }

        int exp_test3_test()
        {
            var EXP_test1 = ASMGenerator.PtrForFunc("DllImportMango" /*_this_ homebrew dll*/, "?EXP_test3@@YAXPAH@Z");

            var gen = new ASMGenerator();
            var arg1 = new ASMArgument(10, true);
            gen.TestCall(EXP_test1, arg1);
            var ops = gen.ToOpcodeStrings();
            gen.Execute();
            var i = DllImportCaller.lib.ValueAtAddres(arg1.RefResultPtr);
            return i;
        }

        void exp_test4_test()
        {
            var EXP_test1 = ASMGenerator.PtrForFunc("DllImportMango" /*_this_ homebrew dll*/, "?EXP_test4@@YAXPAPA_W@Z");

            var gen = new ASMGenerator();
            gen.TestCall(EXP_test1, "XDA!");
            var ops = gen.ToOpcodeStrings();
            gen.Execute();
            //var i = DllImportCaller.lib.ValueAtAddres(arg1.RefResultPtr);

        }

        void exp_MessageBoxCE()
        {
            var EXP_test1 = ASMGenerator.PtrForFunc("coredll" /*_this_ homebrew dll*/, "MessageBoxW");

            var gen = new ASMGenerator();
            gen.TestCall(EXP_test1, 0 /*(hWnd)*/, "The Text", "The Caption", 0 /*(uType)*/);
            var ops = gen.ToOpcodeStrings();
            gen.Execute();
            //var i = DllImportCaller.lib.ValueAtAddres(arg1.RefResultPtr);

        }
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        struct Time
        {
            int hours;
        }

        void exp_test5_test()
        {
            var EXP_test1 = ASMGenerator.PtrForFunc("DllImportMango", "?EXP_test5@@YAPAXXZ");

            var gen = new ASMGenerator();
            //ASMArgument arg1 = "Tests.txt";
            //var arg3 = new ASMArgument<int> { Value = bytesToZero.Length };
            gen.TestCall(EXP_test1);
            var ops = gen.ToOpcodeStrings();
            int ret = gen.Execute(); //void* of Time


            var t = MarshalBypass.PtrToStructureT<long>((IntPtr)ret);
            var s = MarshalBypass.SizeOf(t);

            var a = DllImportCaller.lib.ValueAtAddres(ret);
            //MarshalBypass.PtrToStructure(ret
            //var i = DllImportCaller.lib.ValueAtAddres(arg1.RefResultPtr);

        }

        void exp_test6_test()
        {
            var EXP_test1 = ASMGenerator.PtrForFunc("coredll", "malloc");

            var gen = new ASMGenerator();
            //ASMArgument arg1 = "Tests.txt";
            //var arg3 = new ASMArgument<int> { Value = bytesToZero.Length };
            gen.TestCall(EXP_test1, 60 /*byte*/);
            var ops = gen.ToOpcodeStrings();
            int ret = gen.Execute(); //void* of Time

            //MarshalBypass.StructureToPtr(

            //var t = MarshalBypass.PtrToStructureT<long>((IntPtr)ret);
            //var s = MarshalBypass.SizeOf(t);

            //var a = DllImportCaller.lib.ValueAtAddres(ret);
            //MarshalBypass.PtrToStructure(ret
            //var i = DllImportCaller.lib.ValueAtAddres(arg1.RefResultPtr);

        }
    }
}
