using System;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

namespace CSharp___DllImport
{
    public class ASMArgumentT<T>
    {
        public ASMArgumentT(T value)
        {
            var type = value.GetType();

            Argument = new ASMArgument
            {
                Value = value,
                Ref = type.IsClass
            };
        }
        public ASMArgumentT() { }

        public ASMArgument Argument;
        public static implicit operator ASMArgumentT<T>(T value)
        {
            return new ASMArgumentT<T>(value);
        }

        public static implicit operator T(ASMArgumentT<T> _this)
        {
            return (T)_this.Argument.Value;
        }
    }

    public class ASMArgument
    {
        public ASMArgument() { }
        public ASMArgument(object value, bool _ref)
        {
            var type = value.GetType();
            Value = value;
            Ref = type.IsClass;
        }
        public bool Ref; //As "int *" ptr to obj?
        public object Value;

        public int RefResultPtr;

        public bool WentRefForced { get; private set; }

        public Int32 GenerateCallInt(ParameterInfo parameterInfo)
        {
            var attr = parameterInfo.GetCustomAttributes(typeof(ASMRef), false);
            // var method = attr[0] as ASMMethodAttribute;

            if (attr.Length > 0)//Ref)
            {
                Ref = true;
                var ptr = GCHandleBypass.Alloc(Value, GCHandleType.Pinned).AddrOfPinnedObject().ToInt32();
                RefResultPtr = ptr;
                return ptr;
            }
            else
            {
                if (_isIntOrSmaller())
                {
                    return (int)(object)Value;
                }
                else
                {
                    WentRefForced = true;
                    //force ref due 32 bit OS (R0, R1, R2, etc = 32 bit int)
                    var ptr = GCHandleBypass.Alloc(Value, GCHandleType.Pinned).AddrOfPinnedObject().ToInt32();
                    RefResultPtr = ptr;
                    return ptr;
                }
            }
        }
        private bool _is(params Type[] typesToMatch)
        {
            var type = Value.GetType();

            foreach (var match in typesToMatch)
            {
                if (type != match) return false;
            }
            return true;
        }
        private bool _isIntOrSmaller()
        {
            return _is(typeof(sbyte), typeof(byte), typeof(char), typeof(short), typeof(ushort), typeof(int), typeof(uint));
        }
    }

    public class ASMGenerator
    {
        public static byte[] OPcode_return = new byte[] { 0x0E, 0xF0, 0xA0, 0xE1 }; //mov pc, lr
        public static byte[] OPcode_R0_eq_1 = new byte[] { 0x01, 0x00, 0xA0, 0xE3 }; //Mov R0,#1 
        public static byte[] OPcode_Call = new byte[] { 0x00, 0x00, 0x00, 0xeb }; //bl(eb000000) [ADDR]

        public static byte[] OPcode_MOV = new byte[] { /*[VALUE], [REG], */0xA0, 0xE3 }; //Mov R[REG],#[VALUE] 

        List<byte> asm;

        public void Clear()
        {
            asm.Clear();
        }

        public ASMGenerator()
        {
            asm = new List<byte>();
        }

        public ParameterInfo[] FunctionParamTypes;

        public static string resolveDecode(string opCode)
        {
            string val;
            switch (opCode.ToLower())
            {
                case "e59f0010": val = "ldr r0, [pc, #0x10]"; break;
                case "e59f3008": val = "ldr r3, [pc, #8]"; break;
                case "e1a0e00f": val = "mov lr, pc"; break;
                case "e1a0f003": val = "mov pc, r3"; break;
                case "e49df004": val = "ldr pc, [sp], #4"; break;
                case "e1a0f001": val = "mov pc, r1"; break;
                case "e59f0000": val = "ldr r0, [pc]"; break;
                case "e52de004": val = "str lr, [sp, #-4]!"; break;
                case "e1a0f00e": val = "mov pc, lr (return)"; break;
                case "ea000000": val = "b (next 4 byte addr of method)"; break;
                case "e59f1014": val = "ldr r1, [pc, #0x14]"; break;
                case "e59f300c": val = "ldr r3, [pc, #0xC]"; break;
                case "e5910000": val = "ldr r0, [r1]"; break;
                case "e1a00001": val = "mov r0, r1"; break;
                case "e59f100c": val = "ldr r1, [pc, #0xC]"; break;
                case "e3a03000": val = "mov r3, #0"; break;
                case "e59f3028": val = "ldr r3, [pc, #0x28]"; break;
                default:
                    {
                        if (opCode.Substring(0, 4) == "E59F")
                        {
                            var register = opCode.Substring(4, 2);
                            var value = opCode.Substring(6, 2);
                            val = "ldr r" + (byte.Parse(register) / 10) + ", [pc, #0x" + value + "]";
                        }
                        else
                        {
                            val = "DCD? (" + opCode + ")";
                        }
                    } break;

            }
            return val + " (" + opCode + ")";
        }

        public string ToOpcodeStrings()
        {
            var sb = new StringBuilder();
            var sh = asm.ToArray();
            for (int i = 0; i < sh.Length; )
            {
                var ver1 = (sh[i].ToString("X2") + "" + sh[i + 1].ToString("X2") + "" + sh[i + 2].ToString("X2") + "" + sh[i + 3].ToString("X2")).ToLower();
                var ver2 = (sh[i + 3].ToString("X2") + "" + sh[i + 2].ToString("X2") + "" + sh[i + 1].ToString("X2") + "" + sh[i + 0].ToString("X2")).ToLower();
                //System.Diagnostics.Debug.WriteLine(ver1 + " (" + ver2 + ")");
                sb.AppendFormat("{0} ({1}) ({2})\r\n", ver1, ver2, resolveDecode(ver2));
                i += 4;
            }
            return sb.ToString();
        }

        public int Execute()
        {
            var sh = asm.ToArray();
            //System.Diagnostics.Debug.WriteLine(ToOpcodeStrings());
            return DllImportCaller.lib.ASMExecute(ref sh[0]);
        }
        public static int Execute(byte[] shellcode)
        {
            return DllImportCaller.lib.ASMExecute(ref shellcode[0]);
        }

        public void Return()
        {
            asm.AddRange(OPcode_return);
        }
        public void Set_MOV_R0_EQ_1()
        {
            asm.AddRange(OPcode_R0_eq_1);
        }

        public void Call(int address)
        {
            //throw new Exception("Not sure if working");
            asm.AddRange(OPcode_Call);
            asm.AddRange(BitConverter.GetBytes(address));
            //bl = eb000000
        }

        private byte toReg(byte value)
        {
            return (byte)(value * 10);
        }

        public void MOV(byte register, byte value)
        {
            asm.Add(value);
            asm.Add(toReg(register));
            asm.AddRange(OPcode_MOV);
        }
        public void MOV_REG_TO_REG(byte src, byte dst)
        {
            asm.Add(toReg(dst));
            asm.Add(toReg(src));
            asm.AddRange(new byte[] { 0xA0, 0xE1 });
        }

        IEnumerable<byte> DCD_bytes(params object[] objects)
        {
            //int asd = 10;
            // var od = GCHandleBypass.Alloc(asd, GCHandleType.Pinned);
            // var ad = od.AddrOfPinnedObject();
            // var lk = asd;

            var addresses = objects.Reverse().Select(obj => BitConverter.GetBytes(GCHandleBypass.Alloc(obj, GCHandleType.Pinned).AddrOfPinnedObject().ToInt32())).ToArray();
            foreach (var bArray in addresses)
            {
                foreach (var b in bArray)
                {
                    yield return b;
                }
            }
        }

        byte[] LDR(byte offset, byte register)
        {
            return new byte[] { /*0x28*/offset, /*0x30*/register, 0x9F, 0xE5 };
        }

        /*
         04e02de5 //str         lr, [sp, #-4]!
         00109fe5 //r1, [pc]
         03089fe5
         0fe0a0e1 //mov         lr, pc
         03f0a0e1 //mov         pc, r3
         04f09de4 //ldr         pc, [sp], #4
         f8542a07 //DCD
         ec542a07 //DCD
         00009fe5 //ldr         r0, [pc]
         0ef0a0e1 //mov         pc, lr
         */


        public void TestCall_3_OR_LESS_ARGS(int funcAddress, params ASMArgumentT<object>[] obj)
        {
            var addr = funcAddress;

            int calc = 0;
            var objAddr = obj.Select(o =>
            {
                return o.Argument.GenerateCallInt(FunctionParamTypes[calc++]);
            }).ToArray();//obj.Select(o => GCHandleBypass.Alloc(o, GCHandleType.Pinned).AddrOfPinnedObject().ToInt32()).ToArray();
            //var msgIntPtr = GCHandleBypass.Alloc(msgInt, GCHandleType.Pinned).AddrOfPinnedObject().ToInt32();
            //var args = DCD_bytes(addr, msgIntPtr).ToArray();
            //            asm.AddRange(new byte[] 
            //            {
            //0x04, 0xE0, 0x2D, 0xE5,
            //0x08, 0x30, 0x9F, 0xE5,
            //0x0F, 0xE0, 0xA0, 0xE1,
            //0x03, 0xF0, 0xA0, 0xE1,
            //0x04, 0xF0, 0x9D, 0xE4,
            ///*0x48, 0x95, 0x46, 0x15 [insert 32 bit func ptr] > */ 
            //0x00, 0x00, 0x9F, 0xE5,
            //0x0E, 0xF0, 0xA0, 0xE1 /*(return)*/
            //            });
            asm.AddRange(new byte[] 
            {
0x04, 0xE0, 0x2D, 0xE5, //[SKIP]skip for edit //str         lr, [sp, #-4]!
//0x08, 0x30, 0x9F, 0xE5, //ldr r3 = #8

 });
            List<byte> asm2 = new List<byte>();
            //for (int i = objAddr.Length * 0x10 - 0x10, reg = objAddr.Length; i > -1; i -= 0x10, reg -=0x10)
            //{
            //    asm.AddRange(LDR((byte)i, (byte)(reg)));
            //}
            var asa = Enumerable.Range(0, objAddr.Length).Select(o =>
            {
                var ia = (int)(0xE59F0010 + (0x8 * o) + (o * 0x1000));
                return BitConverter.GetBytes(ia);
            }).Reverse().ToArray();

            foreach (var item in asa)
            {
                asm.AddRange(item);
            }

            var asm2R = asm2.ToArray();
            //asm.AddRange(LDR(0x10, 0));
            asm.AddRange(LDR(0x8, /*(byte)(objAddr.Length * 0x10 + 0x10)*/0x30)); // R0, R1, R2 filled OK, then => FAIL R3 used (caller ptr) (ASM "orr" needed?)

            asm.AddRange(new byte[] {
0x0F, 0xE0, 0xA0, 0xE1,//[SKIP]mov         lr, pc
0x03, 0xF0, 0xA0, 0xE1,//[SKIP]mov         pc, r3
0x04, 0xF0, 0x9D, 0xE4 //[SKIP]ldr         pc, [sp], #4
             });

            //var bits = BitConverter.GetBytes(addr);
            asm.AddRange(BitConverter.GetBytes(addr)); //place DCD args
            foreach (var item in objAddr)
            {
                asm.AddRange(BitConverter.GetBytes(item)); //place DCD args
            }
            //asm.AddRange(BitConverter.GetBytes(msgIntPtr)); //place DCD args
            asm.AddRange(new byte[] 
            {
 0x00, 0x00, 0x9F, 0xE5, //[SKIP]ldr         r0, [pc] 
 0x0E, 0xF0, 0xA0, 0xE1 /*(return)*/
            });
        }

        public void TestCall(int funcAddress, params ASMArgumentT<object>[] obj)
        {
            if (obj.Length < 4)
            {
                TestCall_3_OR_LESS_ARGS(funcAddress, obj);
                return;
            }
            else
            {
                throw new NotImplementedException("No more than 3 args are yet supprorted");
            }
            var addr = funcAddress;


            int calc = 0;
            var objAddr = obj.Select(o =>
            {
                return o.Argument.GenerateCallInt(FunctionParamTypes[calc++]);
            }).ToArray();

            //var objAddr = obj.Select(o => o.Argument.GenerateCallInt()).ToArray();//obj.Select(o => GCHandleBypass.Alloc(o, GCHandleType.Pinned).AddrOfPinnedObject().ToInt32()).ToArray();
            //var msgIntPtr = GCHandleBypass.Alloc(msgInt, GCHandleType.Pinned).AddrOfPinnedObject().ToInt32();
            //var args = DCD_bytes(addr, msgIntPtr).ToArray();
            //            asm.AddRange(new byte[] 
            //            {
            //0x04, 0xE0, 0x2D, 0xE5,
            //0x08, 0x30, 0x9F, 0xE5,
            //0x0F, 0xE0, 0xA0, 0xE1,
            //0x03, 0xF0, 0xA0, 0xE1,
            //0x04, 0xF0, 0x9D, 0xE4,
            ///*0x48, 0x95, 0x46, 0x15 [insert 32 bit func ptr] > */ 
            //0x00, 0x00, 0x9F, 0xE5,
            //0x0E, 0xF0, 0xA0, 0xE1 /*(return)*/
            //            });
            asm.AddRange(new byte[] 
            {
0x04, 0xE0, 0x2D, 0xE5, //[SKIP]skip for edit //str         lr, [sp, #-4]!
//0x08, 0x30, 0x9F, 0xE5, //ldr r3 = #8

 });
            List<byte> asm2 = new List<byte>();
            //for (int i = objAddr.Length * 0x10 - 0x10, reg = objAddr.Length; i > -1; i -= 0x10, reg -=0x10)
            //{
            //    asm.AddRange(LDR((byte)i, (byte)(reg)));
            //}
            var asa = Enumerable.Range(0, objAddr.Length).Select(o =>
            {
                var ia = (int)(0xE59F0010 + (0x8 * o) + (o * 0x1000));
                return BitConverter.GetBytes(ia);
            }).Reverse().ToArray();

            foreach (var item in asa)
            {
                asm.AddRange(item);
            }

            var asm2R = asm2.ToArray();
            //asm.AddRange(LDR(0x10, 0));
            asm.AddRange(LDR(0x8, /*(byte)(objAddr.Length * 0x10 + 0x10)*/0x30)); // R0, R1, R2 filled OK, then => FAIL R3 used (caller ptr) (ASM "orr" needed?)

            asm.AddRange(new byte[] {
0x0F, 0xE0, 0xA0, 0xE1,//[SKIP]mov         lr, pc
0x03, 0xF0, 0xA0, 0xE1,//[SKIP]mov         pc, r3
0x04, 0xF0, 0x9D, 0xE4 //[SKIP]ldr         pc, [sp], #4
             });

            //var bits = BitConverter.GetBytes(addr);
            asm.AddRange(BitConverter.GetBytes(addr)); //place DCD args
            foreach (var item in objAddr)
            {
                asm.AddRange(BitConverter.GetBytes(item)); //place DCD args
            }
            //asm.AddRange(BitConverter.GetBytes(msgIntPtr)); //place DCD args
            asm.AddRange(new byte[] 
            {
 0x00, 0x00, 0x9F, 0xE5, //[SKIP]ldr         r0, [pc] 
 0x0E, 0xF0, 0xA0, 0xE1 /*(return)*/
            });
        }


        public void TestCall(int funcAddress, object[] obj)
        {
            var addr = funcAddress;

            var objAddr = obj.Select(o => GCHandleBypass.Alloc(o, GCHandleType.Pinned).AddrOfPinnedObject().ToInt32()).ToArray();
            //var msgIntPtr = GCHandleBypass.Alloc(msgInt, GCHandleType.Pinned).AddrOfPinnedObject().ToInt32();
            //var args = DCD_bytes(addr, msgIntPtr).ToArray();
            //            asm.AddRange(new byte[] 
            //            {
            //0x04, 0xE0, 0x2D, 0xE5,
            //0x08, 0x30, 0x9F, 0xE5,
            //0x0F, 0xE0, 0xA0, 0xE1,
            //0x03, 0xF0, 0xA0, 0xE1,
            //0x04, 0xF0, 0x9D, 0xE4,
            ///*0x48, 0x95, 0x46, 0x15 [insert 32 bit func ptr] > */ 
            //0x00, 0x00, 0x9F, 0xE5,
            //0x0E, 0xF0, 0xA0, 0xE1 /*(return)*/
            //            });
            asm.AddRange(new byte[] 
            {
0x04, 0xE0, 0x2D, 0xE5, //[SKIP]skip for edit //str         lr, [sp, #-4]!
//0x08, 0x30, 0x9F, 0xE5, //ldr r3 = #8

 });
            List<byte> asm2 = new List<byte>();
            //for (int i = objAddr.Length * 0x10 - 0x10, reg = objAddr.Length; i > -1; i -= 0x10, reg -=0x10)
            //{
            //    asm.AddRange(LDR((byte)i, (byte)(reg)));
            //}
            var asa = Enumerable.Range(0, objAddr.Length).Select(o =>
            {
                var ia = (int)(0xE59F0010 + (0x8 * o) + (o * 0x1000));
                return BitConverter.GetBytes(ia);
            }).Reverse().ToArray();

            foreach (var item in asa)
            {
                asm.AddRange(item);
            }

            var asm2R = asm2.ToArray();
            //asm.AddRange(LDR(0x10, 0));
            asm.AddRange(LDR(0x8, /*(byte)(objAddr.Length * 0x10 + 0x10)*/0x30));

            asm.AddRange(new byte[] {
0x0F, 0xE0, 0xA0, 0xE1,//[SKIP]mov         lr, pc
0x03, 0xF0, 0xA0, 0xE1,//[SKIP]mov         pc, r3
0x04, 0xF0, 0x9D, 0xE4 //[SKIP]ldr         pc, [sp], #4
             });

            //var bits = BitConverter.GetBytes(addr);
            asm.AddRange(BitConverter.GetBytes(addr)); //place DCD args
            foreach (var item in objAddr)
            {
                asm.AddRange(BitConverter.GetBytes(item)); //place DCD args
            }
            //asm.AddRange(BitConverter.GetBytes(msgIntPtr)); //place DCD args
            asm.AddRange(new byte[] 
            {
 0x00, 0x00, 0x9F, 0xE5, //[SKIP]ldr         r0, [pc] 
 0x0E, 0xF0, 0xA0, 0xE1 /*(return)*/
            });
        }

        //        public void TestCall(int val)
        //        {
        //            var addr = DllImportCaller.lib.TestFunc2();
        //            int msgInt = val;
        //            var msgIntPtr = GCHandleBypass.Alloc(msgInt, GCHandleType.Pinned).AddrOfPinnedObject().ToInt32();
        //            //var args = DCD_bytes(addr, msgIntPtr).ToArray();
        //            //            asm.AddRange(new byte[] 
        //            //            {
        //            //0x04, 0xE0, 0x2D, 0xE5,
        //            //0x08, 0x30, 0x9F, 0xE5,
        //            //0x0F, 0xE0, 0xA0, 0xE1,
        //            //0x03, 0xF0, 0xA0, 0xE1,
        //            //0x04, 0xF0, 0x9D, 0xE4,
        //            ///*0x48, 0x95, 0x46, 0x15 [insert 32 bit func ptr] > */ 
        //            //0x00, 0x00, 0x9F, 0xE5,
        //            //0x0E, 0xF0, 0xA0, 0xE1 /*(return)*/
        //            //            });
        //            asm.AddRange(new byte[] 
        //            {
        //0x04, 0xE0, 0x2D, 0xE5, //[SKIP]skip for edit //str         lr, [sp, #-4]!
        ////0x08, 0x30, 0x9F, 0xE5, //ldr r3 = #8

        // });
        //            asm.AddRange(LDR(0x10, 0));
        //            asm.AddRange(LDR(0x8, 0x30));

        //            asm.AddRange(new byte[] {
        //0x0F, 0xE0, 0xA0, 0xE1,//[SKIP]mov         lr, pc
        //0x03, 0xF0, 0xA0, 0xE1,//[SKIP]mov         pc, r3
        //0x04, 0xF0, 0x9D, 0xE4 //[SKIP]ldr         pc, [sp], #4
        //             });

        //            //var bits = BitConverter.GetBytes(addr);
        //            asm.AddRange(BitConverter.GetBytes(addr)); //place DCD args
        //            asm.AddRange(BitConverter.GetBytes(msgIntPtr)); //place DCD args
        //            asm.AddRange(new byte[] 
        //            {
        // 0x00, 0x00, 0x9F, 0xE5, //[SKIP]ldr         r0, [pc] 
        // 0x0E, 0xF0, 0xA0, 0xE1 /*(return)*/
        //            });
        //        }


        //Works below
        //        public void TestCall()
        //        {
        //            var addr = DllImportCaller.lib.TestFunc1();
        //            //            asm.AddRange(new byte[] 
        //            //            {
        //            //0x04, 0xE0, 0x2D, 0xE5,
        //            //0x08, 0x30, 0x9F, 0xE5,
        //            //0x0F, 0xE0, 0xA0, 0xE1,
        //            //0x03, 0xF0, 0xA0, 0xE1,
        //            //0x04, 0xF0, 0x9D, 0xE4,
        //            ///*0x48, 0x95, 0x46, 0x15 [insert 32 bit func ptr] > */ 
        //            //0x00, 0x00, 0x9F, 0xE5,
        //            //0x0E, 0xF0, 0xA0, 0xE1 /*(return)*/
        //            //            });
        //            asm.AddRange(new byte[] 
        //            {
        //0x04, 0xE0, 0x2D, 0xE5,
        //0x08, 0x30, 0x9F, 0xE5,
        //0x0F, 0xE0, 0xA0, 0xE1,
        //0x03, 0xF0, 0xA0, 0xE1,
        //0x04, 0xF0, 0x9D, 0xE4});
        //            var bits = BitConverter.GetBytes(addr);
        //            asm.AddRange(bits);
        //            asm.AddRange(new byte[] 
        //            {
        // 0x00, 0x00, 0x9F, 0xE5,
        // 0x0E, 0xF0, 0xA0, 0xE1 /*(return)*/
        //            });
        //        }

        [System.Diagnostics.DebuggerNonUserCode, System.Diagnostics.DebuggerHidden]
        public static int PtrForFunc(string dll, string method)
        {
            IntPtr dllLook;
            if ((dllLook = DllImportCaller.lookupDll(dll)) != IntPtr.Zero)
            {
                IntPtr methodLookup;
                if ((methodLookup = DllImportCaller.lookupMethod(dllLook, method)) != IntPtr.Zero)
                {
                    return methodLookup.ToInt32();
                    //EXECUTE / OK
                }
                else
                {
                    DllImportCaller.lib.FreeLibrary7(dllLook);
                    throw new Exception("Could not find Method (Dll found)");
                }
            }
            else
            {
                throw new Exception("Could not find dll (" + dll + ")");
            }

            throw new InvalidOperationException();
        }

        public class ASMInvoker<T>
        {
            public ASMInvoker(ParameterInfo[] functionParamTypes)
            {
                this.gen = new ASMGenerator();
                this.gen.FunctionParamTypes = this.FunctionParamTypes = functionParamTypes;
            }
            public int FuncPtr;

            ParameterInfo[] FunctionParamTypes;
            ASMGenerator gen;

            public int Invoke(params ASMArgumentT<object>[] objects)
            {
                gen.Clear();
                gen.TestCall(FuncPtr, objects);
                return gen.Execute();
            }
        }
    }

    public class ASMMethodAttribute : Attribute
    {
        public string Dll { get; set; }
        public string EntryPoint { get; set; }
    }
    /// <summary>
    /// Makes a parameter "Type *"
    /// </summary>
    public class ASMRef : Attribute { }

    public static class DLL
    {
        public const string DllImportMango = "DllImportMango";
        public const string CoreDll = "coredll";
    }

    [ASMMethod(Dll = DLL.CoreDll, EntryPoint = "malloc")]
    public delegate int malloc(int size);

    [ASMMethod(Dll = DLL.DllImportMango, EntryPoint = "?add@@YAHHH@Z")]
    public delegate int add(int param1, int param2);

    [ASMMethod(Dll = DLL.CoreDll, EntryPoint = "SetLastError")]
    public delegate void SetLastError(int dwErrCode);

    [ASMMethod(Dll = DLL.CoreDll, EntryPoint = "GetLastError")]
    public delegate int GetLastError();

    [ASMMethod(Dll = DLL.DllImportMango, EntryPoint = "?EXP_test2@@YAXH@Z")]
    public delegate void EXP_test2([ASMRef] int i);

    [ASMMethod(Dll = DLL.CoreDll, EntryPoint = "GlobalMemoryStatus")]
    public delegate void GlobalMemoryStatus([ASMRef] CSharp___DllImport.Phone.OS.MEMORYSTATUS status);

    [ASMMethod(Dll = "PlatformInterop.dll", EntryPoint = "Vibrate")]
    public delegate void Vibrate();

    [ASMMethod(Dll = "PlatformInterop.dll", EntryPoint = "Stop")]
    public delegate void Stop();

    [ASMMethod(Dll = DLL.CoreDll, EntryPoint = "GetSystemPowerStatusEx2")]
    public delegate int GetSystemPowerStatusEx2([ASMRef] CSharp___DllImport.Phone.Battery.SYSTEM_POWER_STATUS_EX2 status, int dwLen, bool fUpdate);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="time">SystemTime *</param>
    [ASMMethod(Dll = DLL.CoreDll, EntryPoint = "GetSystemTime")]
    public delegate void /*Time pointer*/ GetSystemTime([ASMRef] SystemTime time);

    public static class ASMNativeMethods
    {

        private delegate int InvokeExpressionDelegate(params ASMArgumentT<object>[] objects);

        [System.Diagnostics.DebuggerNonUserCode, System.Diagnostics.DebuggerHidden]
        public static T Create<T>(string dll = null, string entryPoint = null)
        {
            var param = typeof(T).GetMethod("Invoke").GetParameters();
            var paramTypes = param.Select(t => t.ParameterType).ToArray();

            int func;
            if (dll == null && entryPoint == null)
            {
                var attr = typeof(T).GetCustomAttributes(typeof(ASMMethodAttribute), false);
                var method = attr[0] as ASMMethodAttribute;

                func = ASMGenerator.PtrForFunc(method.Dll, method.EntryPoint);
            }
            else
            {
                func = ASMGenerator.PtrForFunc(dll, entryPoint);
            }

            var inv = new CSharp___DllImport.ASMGenerator.ASMInvoker<T>(param) { FuncPtr = func };

            var c = (InvokeExpressionDelegate)inv.Invoke;


            var exp = param.Select(info => Expression.Parameter(info.ParameterType, info.Name)).ToArray();

            return Expression.Lambda<T>(
                Expression.Call(
                    Expression.Constant(inv),
                    c.Method,
                    System.Linq.Expressions.NewArrayExpression.NewArrayInit(
                        typeof(ASMArgumentT<object>),
                        exp.Select(arg => Expression.New(typeof(ASMArgumentT<object>).GetConstructors()[0],
                        Expression.Convert(arg, typeof(object)))).ToArray())
                    ), exp)
                .Compile();
        }

        public static malloc coredll_malloc = Create<malloc>();
        public static add dllImport_add = Create<add>();
        public static GetLastError coredll_GetLastError = Create<GetLastError>();
        public static SetLastError coredll_SetLastError = Create<SetLastError>();
        public static EXP_test2 dllImport_EXP_test2 = Create<EXP_test2>();
        public static GlobalMemoryStatus coredll_GlobalMemoryStatus = Create<GlobalMemoryStatus>();

        public static Vibrate dllImport_Vibrate = Create<Vibrate>();
        public static Stop dllImport_Stop = Create<Stop>();

        public static GetSystemPowerStatusEx2 coredll_GetSystemPowerStatusEx2 = Create<GetSystemPowerStatusEx2>();
        public static GetSystemTime coredll_GetSystemTime = Create<GetSystemTime>();
    }

    [StructLayout(LayoutKind.Explicit, Size = 16, CharSet = CharSet.Auto)]
    public class SystemTime
    {
        [FieldOffset(0)]
        public ushort wYear;
        [FieldOffset(2)]
        public ushort wMonth;
        [FieldOffset(4)]
        public ushort wDayOfWeek;
        [FieldOffset(6)]
        public ushort wDay;
        [FieldOffset(8)]
        public ushort wHour;
        [FieldOffset(10)]
        public ushort wMinute;
        [FieldOffset(12)]
        public ushort wSecond;
        [FieldOffset(14)]
        public ushort wMilliseconds;
    }

    //[StructLayout(LayoutKind.Sequential)]
    //public struct FILETIME
    //{
    //    public uint DateTimeLow;
    //    public uint DateTimeHigh;
    //}

    //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    //public struct WIN32_FIND_DATA
    //{
    //    public uint dwFileAttributes;
    //    public FILETIME ftCreationTime;
    //    public FILETIME ftLastAccessTime;
    //    public FILETIME ftLastWriteTime;
    //    public uint nFileSizeHigh;
    //    public uint nFileSizeLow;
    //    public uint dwReserved0;
    //    public uint dwReserved1;
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    //    public string cFileName;
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
    //    public string cAlternateFileName;
    //}

}
