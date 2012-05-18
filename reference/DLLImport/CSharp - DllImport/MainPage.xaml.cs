using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.IO;
using Expression = System.Linq.Expressions.Expression;
namespace CSharp___DllImport
{
    using System.Runtime.InteropServices;

    public partial class MainPage : PhoneApplicationPage
    {
        void test()
        {
            return;
        }
        //[ASMMethod(Dll = "coredll", EntryPoint = "GetLastError")]
        //public static int GetLastError() { return 0; }

        public static MainPage This;

        public class HardwareDriver
        {
            public uint DriverPtr { get; private set; }

            public HardwareDriver(string driverPreffix)
            {
                var i = ASMNativeMethods.Create<Func<string, int>>(DLL.DllImportMango, "createfile")(driverPreffix);

                if (i == -1) throw new Exception("\"" + driverPreffix + "\" is not a valid driver preffix");

                DriverPtr = (uint)i;
            }

            public bool TalkToDriver(uint dwIoControlCode, [In] byte[] lpInBuf, int nInBufferSize, [Out] byte[] lpOutBuf, int nOutBufferSize, ref int pBytesReturned, IntPtr lpOverlapped)
            {
                return DllImportCaller.lib.DeviceIoControl7(DriverPtr, dwIoControlCode, lpInBuf, nInBufferSize, lpOutBuf, nOutBufferSize, ref pBytesReturned, lpOverlapped);
            }
        }
        public sealed class HardwareAccelerometer : HardwareDriver
        {
            public HardwareAccelerometer() : base("ACC1:") { }

            const uint ACCOnRot = 0x1014EE8;
            const uint ACCOffRot = 0x1014EE8;
            const uint ACCReadValues = 0x1014F10;

            [Obsolete("Not working", true)]
            public void Stop()
            {
                int bytesReturned = 0;
                var _in = new byte[1] {0};
                var _out = new byte[1];
                var res = this.TalkToDriver(ACCOffRot, _in, 1, _out, 1, ref bytesReturned, IntPtr.Zero);
            }
        }
        public sealed class HardwareCompass : HardwareDriver
        {
            public HardwareCompass() : base("CMP1:") { }
        }
        public sealed class HardwareProximeter : HardwareDriver
        {
            /// <summary>
            /// "FUS1:" ??? (Dif'fus'e sensor?) diffuse => light
            /// </summary>
            public HardwareProximeter() : base("FUS1:") { }

        }
        [ASMMethod(Dll = DLL.CoreDll, EntryPoint = "strcpy")]
        public delegate int strcpy([ASMRef] char destination, [ASMRef] char source);

        public static byte[] ReadBytesFromRAMOffset(int address, int intsToRead)
        {
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < intsToRead; i += 4)
            {
                var data = DllImportCaller.lib.ValueAtAddres(address + i);
                bytes.AddRange(BitConverter.GetBytes(data));
            }
            return bytes.ToArray();
        }

        [ASMMethod(Dll = DLL.DllImportMango, EntryPoint = "Callbacktest")]
        public delegate void Callbacktest([MarshalAs(UnmanagedType.FunctionPtr)] Action action);

        [ASMMethod(Dll = DLL.DllImportMango, EntryPoint = "Test_Return_TimeStruct")]
        public delegate int /*Time pointer*/ Test_Return_TimeStruct();


        [ASMMethod(Dll = DLL.CoreDll, EntryPoint = "FindFirstFileW")]
        public delegate int FindFirstFile(string lpFileName, [ASMRef] WIN32_FIND_DATA data);
        
      

        [StructLayout(LayoutKind.Sequential)]
        public struct SomeStruct
        {
            public Int32 X;
            public Int32 Y;
        }

        public struct Native<T>
        {
            public T NativeObjectWorker; //the object we work with
            
            int malloc;
            IntPtr mallocIntPtr;
            
            int structSize;

            public T Object
            {
                get
                {
                    return NativeObjectWorker;
                }
            }
            public int ObjectPointer
            {
                get
                {
                    return malloc;
                }
            }

            public Native(T nativeObject)
            {
                this.structSize = MarshalBypass.SizeOf(nativeObject);
                this.malloc = ASMNativeMethods.coredll_malloc(this.structSize);
                this.mallocIntPtr = (IntPtr)malloc;

                {
                    MarshalBypass.StructureToPtr(nativeObject, mallocIntPtr, false); //move it to byte native space

                    NativeObjectWorker = (T)MarshalBypass.PtrToStructure(mallocIntPtr, typeof(T));
                }
            }
            public Native(int ramAddress)
            {
                this.malloc = ramAddress;
                this.mallocIntPtr = (IntPtr)malloc;
                NativeObjectWorker = (T)MarshalBypass.PtrToStructure(mallocIntPtr, typeof(T));
                this.structSize = MarshalBypass.SizeOf(NativeObjectWorker);
            }

            public byte[] RAMData
            {
                get
                {
                    return ReadBytesFromRAMOffset(this.malloc, this.structSize);
                }
            }
            public void SetRAMData(byte[] bytes)
            {
                //var set = ASMNativeMethods.Create<Action<int, int>>(DLL.DllImportMango, "WriteByte");
                for (int i = 0; i < bytes.Length; i++)
                {
                    DllImportCaller.lib.WriteByte(malloc + (i * sizeof(byte)), bytes[i]);
                }
            }
            public string GetRAMDataString()
            {
                var ram = RAMData;
                return Encoding.UTF8.GetString(ram, 0, ram.Length);
            }

            public static Native<T> CreateFromPtr(int address)
            {
                var obj = (T)MarshalBypass.PtrToStructure((IntPtr)address, typeof(T));
                return new Native<T>(obj);
            }
            public static implicit operator Native<T>(T value)
            {
                return new Native<T>(value);
            }
            public static implicit operator Native<T>(ASMArgumentT<T> value)
            {
                return new Native<T>((T)value.Argument.Value);
            }
            public static implicit operator T(Native<T> _this)
            {
                return _this.Object;
            }
        }

        public static byte[] int2byte(int[] src)
        {
            int srcLength = src.Length;
            byte[] dst = new byte[srcLength << 2];

            for (int i = 0; i < srcLength; i++)
            {
                int x = src[i];
                int j = i << 2;
                dst[j++] = (byte)((x >> 0) & 0xff);
                dst[j++] = (byte)((x >> 8) & 0xff);
                dst[j++] = (byte)((x >> 16) & 0xff);
                dst[j++] = (byte)((x >> 24) & 0xff);
            }
            return dst;
        }
        public static int[] byte2int(byte[] src)
        {
            int dstLength = src.Length >> 2;
            int[] dst = new int[dstLength];

            for (int i = 0; i < dstLength; i++)
            {
                int j = i << 2;
                int x = 0;
                x += (src[j++] & 0xff) << 0;
                x += (src[j++] & 0xff) << 8;
                x += (src[j++] & 0xff) << 16;
                x += (src[j++] & 0xff) << 24;
                dst[i] = x;
            }
            return dst;
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        struct Time
        {
            int hours;
        }


        string[] ReadFunction(int ptr, int count)
        {
            var meth = Enumerable.Range(0, count).Select((_i) =>
            {
                var a = ptr;//DllImportCaller.lib.GetHelloWorldMSGBPtr();//.ASMExecute(ref b[0]);
                var bya = DllImportCaller.lib.ValueAtAddres(a + (sizeof(int) * _i));
                var lo = BitConverter.GetBytes(bya).Select(o => { var t = string.Format("{0:X}", o); if (t.Length == 1) return "0" + t; else return t; }).ToArray();
                return lo[3] + "" + lo[2] + "" + lo[1] + "" + lo[0];
            }).Select(op => ASMGenerator.resolveDecode(op)).ToArray();
            return meth;
        }

        void he()
        {

        }

        class EDBDatabase : IDisposable
        {
            public int guidAddr;
            public string db;

            public EDBDatabase(string edbFilePath)
            {
                db = edbFilePath;
                Open();
            }

            public void Open()
            {
                bool suc;
                guidAddr = DllImportCaller.lib.EDB_Mount(db, out suc);
                if (!suc)
                {
                    throw new ExternalException(((WinError)DllImportCaller.lib.GetLastError7()).ToString());
                }
            }

            private int CloseDatabaseEnumerator(int enumerator)
            {
                return DllImportCaller.lib.EDB_CloseHandle(enumerator);
            }
            public IEnumerable<int> GetDatabaseHandles()
            {
                var first = DllImportCaller.lib.EDB_FindFirstDB(guidAddr);
                {
                    int dbHandle;

                    while ((dbHandle = DllImportCaller.lib.EDB_FindNextDB(first, guidAddr)) != 0)
                    {
                        yield return dbHandle;
                    }
                }
                //CloseDatabaseEnumerator(first);
            }

            public void Dispose()
            {

            }

            public void Infotest()
            {
                var a = GetDatabaseHandles().ToArray();
                //foreach (var item in a)
                //{
                    //var res = DllImportCaller.lib.EDB_CeOidGetInfoEx(guidAddr, a);
                //}
            }
        }

        public static int ByteIndexOf(byte[] searched, byte[] find, int start)
        {
            // Do standard error checking here.

            // Did the values match?
            bool matched = false;

            // Cycle through each byte of the searched. Do not search past
            // searched.Length - find.Length bytes, since it's impossible
            // for the value to be found at that point.
            for (int index = start; index <= searched.Length - find.Length; ++index)
            {
                // Assume the values matched.
                matched = true;

                // Search in the values to be found.
                for (int subIndex = 0; subIndex < find.Length; ++subIndex)
                {
                    // Check the value in the searched array vs the value
                    // in the find array.
                    if (find[subIndex] != searched[index + subIndex])
                    {
                        // The values did not match.
                        matched = false;

                        // Break out of the loop.
                        break;
                    }
                }

                // If the values matched, return the index.
                if (matched)
                {
                    // Return the index.
                    return index;
                }
            }

            // None of the values matched, return -1.
            return -1;
        }

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] b = new byte[32768];
            int r;
            while ((r = input.Read(b, 0, b.Length)) > 0)
                output.Write(b, 0, r);
        }

        IEnumerable<int> findIndexes(byte[] input, byte[] find)
        {
            int start = 0;
            int findLen = find.Length;

            int index;
            while ((index = ByteIndexOf(input, find, start)) != -1)
            {
                int ret = index;
                {
                    start = index + findLen;
                }
                yield return ret;
            }
        }

        void theTests()
        {
            //var alarm = new Microsoft.Phone.Scheduler.Alarm("MyAlarm");
            //alarm.Content ="Hack";
           
            //var hack = new CSharp___DllImport.Schedule.HackedScheduledAction(
            {
                //var f = IsolatedStorageFile.GetUserStoreForApplication().OpenFile("pacman.edb", FileMode.Open);
                //var ms = new MemoryStream();
                //CopyStream(f, ms);
                //var bytes = ms.ToArray();

                //var find = new byte[] { 0x5F, 0x5F, 0x53, 0x79, 0x73, 0x4F, 0x62, 0x6A, 0x65, 0x63, 0x74, 0x73 }; //__SysObjects
                //var fi = findIndexes(bytes, find).Select(index => index + find.Length + 1 /* + 1 = '\0' */).ToArray();
                //var stringsSelect = fi.Select(index =>
                //{
                //    var ret = new List<char>();

                //    int i = 0;
                //    byte v;
                //    while ((v = bytes[index + i]) != 0)
                //    {
                //        var c = (char)v;
                //        ret.Add(c);
                //        i++;
                //    }
                //    return new string(ret.ToArray());
                //});


                //var strings = stringsSelect.ToArray();


                //using (var db = new EDBDatabase("\\Applications\\Data\\8DC5214E-88FA-4C2D-A379-2CD74FE24B72\\Data\\IsolatedStore\\pacman.edb"))
                //{

                //    //var dbs = db.GetDatabaseHandles().ToArray();
                //    //var errea = DllImportCaller.LastError();
                //    //var ina = DllImportCaller.lib.EDB_CeOpenDatabaseEx(db.guidAddr, dbs[0]);//.EDB_CeOidGetInfoEx(db.guidAddr, dbs[0]);
                //    //var erre = DllImportCaller.LastError();
                //}
            }
            var a = 10;
            //////bool suc;
            //////int guidAddr = DllImportCaller.lib.EDB_Mount(db, out suc);
            //////if (!suc)
            //////{
            //////    var erra = (WinError)DllImportCaller.lib.GetLastError7();
            //////}
            //////else
            //////{
            //////    var dbHandles = new List<int>();
            //////    var first = DllImportCaller.lib.EDB_FindFirstDB(guidAddr);
            //////    dbHandles.Add(first);

            //////    int dbHandle;
            //////    var erraA = (WinError)DllImportCaller.lib.GetLastError7();

            //////    while ((dbHandle = DllImportCaller.lib.EDB_FindNextDB(first, guidAddr)) != 0)
            //////    {
            //////        dbHandles.Add(dbHandle);
            //////        var erra = (WinError)DllImportCaller.lib.GetLastError7();
            //////    }
            //////    var sort = dbHandles.Distinct().ToArray();
            //////    var openSessions = sort.Select(d => DllImportCaller.lib.EDB_OpenDBSession(guidAddr, d)).ToArray();
            //////    //var sessionHandle = DllImportCaller.lib.EDB_OpenMounted(db, guidAddr);

            //////}


            //Action hello = he;
            //hello();
            ////
            //int re = DllImportCaller.lib.GetNativeDelegatePtr(hello);
            //var res = ReadFunction(re, 10);

            //var accel = new Microsoft.Devices.Sensors.Accelerometer();
            //accel.CurrentValueChanged += (o, e) => 
            //{
            //    System.Diagnostics.Debug.WriteLine(e.SensorReading.Acceleration);
            //};
            //accel.Start();
            ///



            //var bw = new System.ComponentModel.BackgroundWorker();
            //bw.DoWork += (o, e) => 
            //{
            //    System.Threading.Thread.Sleep(2 * 1000);
            //    var a = new HardwareAccelerometer();
            //    a.Stop();
            //};
            //bw.RunWorkerAsync();
            //object a = "Hello World!";
            //var addr = DllImportCaller.lib.AddressOfObject(ref a);

            //object a2 = 9100;
            //var addr2 = DllImportCaller.lib.AddressOfObject(ref a2);

            //var raw = ReadBytesFromRAMOffset(addr2 - 20, 40);
            //var data = UTF8Encoding.UTF8.GetString(raw, 0, raw.Length);//DllImportCaller.lib.ValueAtAddres(addr2);

            //var me = ASMNativeMethods.Create<strcpy>();
            //string dest = "          ";
            //string src = "hello";
            //var i = me(dest, src);
            //var ptr = HardwareAccelerometer.driverPtr;
            //ASMNativeMethods.Create<Func<string, int>>(DLL.DllImportMango, "createfile")("CMP:");

            //var s = "Hello".ToCharArray();
            //var s2 = "      ".ToCharArray();
            //Action a = () => 
            //{
            //    MessageBox.Show("Hello");
            //};


            //var bc = new char[] { 'h', 'e','j' };
            ////var addr = DllImportCaller.lib.AddressOfObject(ref bc);

            //var p = new SomeStruct();
            //p.X = 0;
            //p.Y = int.MaxValue;

            //var ptr = ASMNativeMethods.coredll_malloc(100);

            //MarshalBypass.StructureToPtr(p, (IntPtr)ptr, false);

            //var bbb = ReadBytesFromRAMOffset(ptr, MarshalBypass.SizeOf(p));
            //var ddd = Encoding.UTF8.GetString(bbb, 0, bbb.Length);

            //var poi = MarshalBypass.PtrToStructure((IntPtr)ptr, typeof(SomeStruct));
            {
                //Native<SomeStruct> i = new Native<SomeStruct>(new SomeStruct { X = 10, Y = 10 });

                //var dataA = i.GetRAMData();
                //i.SetRAMData(new int[] { 5, 5 });
                //System.Threading.Thread.Sleep(10);


                //var data = i.GetRAMData();
            }

            {
                //Native<float> a = 59651.6f;
                //var ram = a.GetRAMData();
                //WIN32_FIND_DATA data = new WIN32_FIND_DATA();
                //var f = ASMNativeMethods.Create<FindFirstFile>();
                //var a = f(@"\Applications\Data\8DC5214E-88FA-4C2D-A379-2CD74FE24B72\Data\", data); 
                //SystemTime time = new SystemTime();
                //ASMNativeMethods.coredll_GetSystemTime(time);

                //var timePointer = ASMNativeMethods.Create<Test_Return_TimeStruct>()();
                //var nat = Native<Time>.CreateFromPtr(timePointer);

                //var a = byte2int(Encoding.UTF8.GetBytes("Hello"));
                //nat.SetRAMData(new byte[] { 21, 0, 0, 0, 20, 0, 0, 0, 66, 0, 0, 0 });
            }
            //a();
            //ASMNativeMethods.Create<Callbacktest>()(a);


            var err = ASMNativeMethods.Create<GetLastError>()();
            //DllImportCaller.lib.GetLastError7();
            //var malloc = ASMNativeMethods.Create<malloc>(inv => c => inv.Invoke(c));

            //var status = Phone.OS.Memory.GetMemoryStatus();
            //int[] asa = new int[] { malloc(50000000) };
            //var status2 = Phone.OS.Memory.GetMemoryStatus();
            //var aaa = Type.GetType("System.Reflection.Emit.DynamicMethod, mscorlib, Version=3.7.0.0");

            //var callerMalloc = convertDelegate<malloc>(inv => c => inv.Invoke(c));
            //var data = callerMalloc(10);
            //var addr = callerMalloc.Invoke(10);
            //int mem = callerMalloc(10);
            //int a;

            //exp_test5_test();
            //exp_test6_test();
            //var bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

            //exp_test5_test(bytes);

            //exp_MessageBoxCE();

            //

            //var uptime = Phone.OS.Uptime;
            //for (int i = 0; i < 500; i++)
            //{
            //    ASMNativeMethods.coredll_GlobalMemoryStatus(status);
            //}
            //var uptime2 = Phone.OS.Uptime;
            //var uptimeRes = uptime2 - uptime;


            //var uptime3 = Phone.OS.Uptime;
            //for (int i = 0; i < 500; i++)
            //{
            //    status = Phone.OS.Memory.GetMemoryStatus();
            //}
            //var uptime4 = Phone.OS.Uptime;
            //var uptimeRes2 = uptime4 - uptime3;

            //MessageBox.Show(uptimeRes.TotalMilliseconds + " + " + uptimeRes2.TotalMilliseconds);


            //var i = ASMNativeMethods.coredll_GetSystemPowerStatusEx2(status, MarshalBypass.SizeOf(status), true);
            //var status = new Phone.OS.MEMORYSTATUS();
            //ASMNativeMethods.coredll_GlobalMemoryStatus(status);

            //var sdd = Type.GetType("System.IO.Ports.SerialPort, System, Version=3.7.0.0");
            //var consta = sdd.GetConstructor(new Type[] { typeof(string) });
            //var instance = consta.Invoke(new object[] { "COM1" });


            byte[] b = new byte[] 
                { 
                    /*0xe3a00000*///0x00, 0x00, 0xA0, 0xE3, //return 0 
                    /*0xe12fff1e*///0x1e, 0xff, 0x2f, 0xe1 //RET*/
                    //0x00, 0x00, 0xA0, 0xE3,
                    //0x1e, 0xff, 0x2f, 0xe1
                    0xc3, 0xc9
                };

            //var meth = Enumerable.Range(0, 50).Select((_i) =>
            //{
            //    var a = DllImportCaller.lib.GetHelloWorldMSGBPtr();//.ASMExecute(ref b[0]);
            //    var bya = DllImportCaller.lib.ValueAtAddres(a + (sizeof(int) * _i));
            //    var lo = BitConverter.GetBytes(bya).Select(o => { var t = string.Format("{0:X}", o); if (t.Length == 1) return "0" + t; else return t; }).ToArray();
            //    return lo[3] + "" + lo[2] + "" + lo[1] + "" + lo[0];
            //}).Select(op => ASMGenerator.resolveDecode(op)).ToArray();


            //var del = ASMGenerator.GenerateDelegateFor<GetLastError>();
            //object lastErr = del.Invoke(10);
            //foreach (var item in meth)
            //{
            //    System.Diagnostics.Debug.WriteLine(item);
            //}



            //System.Diagnostics.Debugger.Break();
            //test
            //object obj = new char[] { (char)54, (char)35, (char)54 };

            //var addr = DllImportCaller.lib.AddressOfObject(ref obj);
            //var hack = Enumerable.Range(-16, 16).Select(it => DllImportCaller.lib.ValueAtAddres(addr + it)).ToArray();

            //DllImportCaller.lib.ValueAtAddres(
            //                var code =
            //@"E1A0F00E
            //E52DE004
            //E3510000
            //4A000009
            //E5903000
            //E2432010
            //E5923008
            //E1510003
            //D5821004
            //D5903000
            //D3A02000
            //D0833081
            //D1C320B0
            //D49DF004
            //E59F0000".Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            //var byteCode = meth.Select((_i) => 
            //{ 

            //});
            //var shell = new byte[] 
            //{
            //    0x0E, 0xF0, 0xA0, 0xE1
            //    //0x04, 0xE0, 0x2D, 0xE5,
            //    //0x00, 0x00, 0x51, 0xE3,
            //    //0x09, 0x00, 0x00, 0x4A,
            //    //0x00, 0x30, 0x90, 0xE5,
            //    //0x10, 0x20, 0x43, 0xE2,
            //    //0x08, 0x30, 0x92, 0xE5,
            //    //0x03, 0x00, 0x51, 0xE1,
            //    //0x04, 0x10, 0x82, 0xD5,
            //};
            //var gea = ASMGenerator.GenerateFor((Action<int>)MainPage.SetLastError);
            //                ASMGenerator.Execute(new byte[] {
            //                    0x0C, 0x10, 0x9F, 0xE5,
            //0x00, 0x30, 0xA0, 0xE3,
            //0x00, 0x00, 0xA0, 0xE3,
            //0x01, 0x20, 0xA0, 0xE1,
            //0x0D, 0x0A, 0x00, 0xEA,
            //0xD0, 0xCD, 0x9D, 0x48,
            //0x00, 0x00, 0x9F, 0xE5 });

            /*
             * ___hello(); //call itself:
            0xFE, 0xFF, 0xFF, 0xEA
            0x00, 0x00, 0x9F, 0xE5
            0x0E, 0xF0, 0xA0, 0xE1
             * 
             * CloseClipboard(); :
            0xD6, 0x09, 0x00, 0xEA
            0x00, 0x00, 0x9F, 0xE5
            0x0E, 0xF0, 0xA0, 0xE1
             * 
             * GetFocus(); :
            0x7E, 0x0A, 0x00, 0xEA
            0x00, 0x00, 0x9F, 0xE5
            0x0E, 0xF0, 0xA0, 0xE1
             */

            //{
            //    var gen = new ASMGenerator();//DllImportCaller.lib.TestFunc2()
            //    gen.TestCall(ASMGenerator.PtrForFunc("coredll", "SetLastError"), new object[] { 10 });
            //    //gen.MOV(1, 25);
            //    //gen.MOV(0, 99);
            //    //gen.MOV_REG_TO_REG(0, 1);
            //    //gen.MOV(0, 88);
            //    //gen.MOV_REG_TO_REG(1, 0);
            //    //gen.Set_MOV_R0_EQ_1();
            //    //gen.Set_MOV_R0_EQ_1();
            //    //gen.Call(DllImportCaller.lib.GetHelloWorldMSGBPtr());
            //    //gen.Return();
            //    var s = gen.ToOpcodeStrings();

            //    var sh = gen.Execute();
            //}
            //{
            //    var gen = new ASMGenerator();//DllImportCaller.lib.TestFunc2()
            //    gen.TestCall(ASMGenerator.PtrForFunc("coredll", "GetLastError"), new object[0]);
            //    //gen.MOV(1, 25);
            //    //gen.MOV(0, 99);
            //    //gen.MOV_REG_TO_REG(0, 1);
            //    //gen.MOV(0, 88);
            //    //gen.MOV_REG_TO_REG(1, 0);
            //    //gen.Set_MOV_R0_EQ_1();
            //    //gen.Set_MOV_R0_EQ_1();
            //    //gen.Call(DllImportCaller.lib.GetHelloWorldMSGBPtr());
            //    //gen.Return();
            //    var s = gen.ToOpcodeStrings();

            //    var sh = gen.Execute();
            //}

            //var sh = new byte[]
            //{
            //    0x01, 0x00, 0xA0, 0xE3, // R0 = 1
            //    0x0E, 0xF0, 0xA0, 0xE1 //return
            //};
            //byte[] shell = ASMGenerator.OPcode_return;

            //var id = ManifestAppInfo.Info.ProductId;
            ////string BasePath = string.Format(@"\Applications\Install\{0}\Install\", "8DC5214E-88FA-4C2D-A379-2CD74FE24B72");
            //var res = Registrer.Register(@"\Applications\Install\8DC5214E-88FA-4C2D-A379-2CD74FE24B72\Install\DllImportMango.dll",
            //    "434B816A-3ADA-4386-8421-33B0E669F3F1");
            //var err = (Win32ErrorCode)res;

            //var a = new TestObject();
            //var caller = a as IFileSystemIO;
            //int resInt;

            //var ads = Phone.Search.SearchFor("umad?");//caller.VoidCall("PlatformInterop", "Stop");//.sayHello();
            ////var resCall = caller.VoidCall("PlatformInterop", "Vibrate");
            //return;




            /*
void (*func1)(void);
func1 = (void(__cdecl*)(void))0x15469548;
func1();

void (*func2)(void);
func2 = (void(__cdecl*)(void))0x46548264;
func2();

void (*func)(void);
func = (void(__cdecl*)(void))0xE52DE003;
func();
                  
      
0x04, 0xE0, 0x2D, 0xE5 //str         lr, [sp, #-4]!
0x28, 0x30, 0x9F, 0xE5 //ldr         r3, [pc, #0x28]	; -449978365 (0xe52de003)
0x0F, 0xE0, 0xA0, 0xE1 //mov         lr, pc
0x03, 0xF0, 0xA0, 0xE1 //mov         pc, r3
0x18, 0x30, 0x9F, 0xE5 //ldr         r3, [pc, #0x18]	; -449978365 (0xe52de003)
0x0F, 0xE0, 0xA0, 0xE1 //mov         lr, pc
0x03, 0xF0, 0xA0, 0xE1 //mov         pc, r3
0x08, 0x30, 0x9F, 0xE5 //ldr         r3, [pc, #8]	; -449978365 (0xe52de003)
0x0F, 0xE0, 0xA0, 0xE1 //mov         lr, pc
0x03, 0xF0, 0xA0, 0xE1 //mov         pc, r3
0x04, 0xF0, 0x9D, 0xE4 //ldr         pc, [sp], #4
0x03, 0xE0, 0x2D, 0xE5 //DCD         0xe52de003
0x64, 0x82, 0x54, 0x46 //DCD         0x46548264
0x48, 0x95, 0x46, 0x15 //DCD         0x15469548
0x00, 0x00, 0x9F, 0xE5 //???
0x0E, 0xF0, 0xA0, 0xE1 (return)
0x64, 0x50, 0x9D, 0x48
0x00, 0x00, 0x91, 0xE5
0x0E, 0xF0, 0xA0, 0xE1 (return)
0x01, 0x00, 0xA0, 0xE1
0x0E, 0xF0, 0xA0, 0xE1 (return)
0x00, 0x00, 0xA0, 0xE3
0x0E, 0xF0, 0xA0, 0xE1 (return)
0x08, 0x9E, 0x9D, 0x48
0x08, 0xCE, 0x9D, 0x48
0x0D, 0xC0, 0xA0, 0xE1
0x10, 0x58, 0x2D, 0xE9
0x10, 0xB0, 0x8D, 0xE2
0x01, 0x40, 0xA0, 0xE1
0xBC, 0x09, 0x00, 0xEB
0x00, 0x00, 0x50, 0xE3
0x0A, 0x00, 0x00, 0x0A
0x04, 0x10, 0xA0, 0xE1
0x84, 0x09, 0x00, 0xEB
0x00, 0x00, 0x50, 0xE3
0x04, 0x00, 0x00, 0x0A
0x0F, 0xE0, 0xA0, 0xE1
0x00, 0xF0, 0xA0, 0xE1
0x04, 0x00, 0x00, 0xEA
0x04, 0x00, 0xA0, 0xE3
0x02, 0x00, 0x00, 0xEA
0x03, 0x00, 0xA0, 0xE3
0x00, 0x00, 0x00, 0xEA
0x02, 0x00, 0xA0, 0xE3
0x10, 0xD0, 0x4B, 0xE2
0x10, 0xA8, 0x9D, 0xE8
0x08, 0x9E, 0x9D, 0x48
0x08, 0xCE, 0x9D, 0x48
0x00, 0x00, 0x9F, 0xE5
0x0E, 0xF0, 0xA0, 0xE1 (return)

             */

        }

        /*
         
        1 = blank (themed)
        2 = gray
        3 = blank (themed)
        4 = calender (themed)
        5 = [MS sandbox default] (themed)
        6 = wide blank (themed)
        7 = blank (themed)
        8 = gray
        9 = gray
        10 = people animation (themed) (Jaxbot hello there => Applications icon binding :P)
        11 = blank (themed)
        12 = "Three row tile"-animation (themed)
        13 = wide gray
        14 = "Picture hub animator" (themed)
        15 = 100% BLANK (black) - the perfect separator
        16 >> inf (black blanks)
         
         */

        void theTests2()
        {
            //var manager = typeof(TokenmanagerSingleton);
            //var inst = TokenmanagerSingleton.Instance;
            //var tiles = HackedShellTile.ActiveTiles.ToArray();
            // var secondaryTile = new Microsoft.Phone.Shell.StandardTileData{Title = "Hello"};
            // HackedShellTile.Create(new Uri("/?Id=1337", UriKind.Relative), secondaryTile);

            //Wide.tile.size = {w:358, h:173}
            //normal.tile.size= {w:173, h:173}
            //Func<int> rand = () => // prevent raging exception (tile url collision) "?id=" + rand()
            //{
            //    Random random = new Random();
            //    return random.Next(0, int.MaxValue);
            //};

            {
                var calendar = new HackedCalendarTile("appointmentTitle", "appointmentDescription", "appointmentLocation");
                var icon = new HackedIconTile("res://StartMenu!TokenIE.png", "@BrowsuiRes.dll,-12251");
                var wide = new HackedWideIconTile("res://StartMenu!TokenIE.png", "@BrowsuiRes.dll,-12251");
                var black_or_white = new HackedSeparatorTitle();
                var text = new HackedSimpleTextTile("My title", "Im subtitle");
                var row = new HackedTrippleRowTitle("res://StartMenu!TokenIE.png", "res://StartMenu!token.xboxlive.png", "res://StartMenu!TokenCamera.png");
                var people = new HackedPeopleHubTitle("People of XDA", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png");
                var anim = new HackedWidePictureAnimator(
                    @"file://\Application Data\Photos\Pictures_00.jpg", 
                    @"file://\Application Data\Photos\Pictures_01.jpg", 
                    @"file://\Application Data\Photos\Pictures_02.jpg", 
                    @"file://\Application Data\Photos\Pictures_03.jpg"
                    /*Up to 18 supported*/);
                var xbox = new HackedXboxTile("My title");
            }
        }

        public MainPage()
        {
            //http://www.codeproject.com/KB/cs/InterOp.aspx
            InitializeComponent();
            
            if (Phone.DEP.IsEmulator)
            {
                MessageBox.Show("This can NOT run under emulator, the code that is running is native ARMv4 C++. Use a real device, a Samsung Omnia 7 or instance. Demos will not be rendered.");
                return;
            }
            else
            {
                //theTests();
                //theTests2();
                //---- Uncomment if DllImport Tests / Standalone DllImport
                RenderDemos();

                //if (!System.Diagnostics.Debugger.IsAttached)
                //{
                //    return; //to be able to start even with corupted code / error casting code.
                //}
                //else
                //{
                //    Loaded += new RoutedEventHandler(MainPage_Loaded);
                //}
                //----
            }
        }

        static bool isOnly(int[] array, int val)
        {
            foreach (var item in array)
            {
                if (item != val)
                {
                    return false;
                }
            }
            return true;
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit, Size = 4)]
        public struct COLORREF
        {
            public COLORREF(byte r, byte g, byte b)
            {
                this.Value = 0;
                this.R = r;
                this.G = g;
                this.B = b;
            }

            public COLORREF(uint value)
            {
                this.R = 0;
                this.G = 0;
                this.B = 0;
                this.Value = value & 0x00FFFFFF;
            }

            [System.Runtime.InteropServices.FieldOffset(0)]
            public byte R;
            [System.Runtime.InteropServices.FieldOffset(1)]
            public byte G;
            [System.Runtime.InteropServices.FieldOffset(2)]
            public byte B;

            [System.Runtime.InteropServices.FieldOffset(0)]
            public uint Value;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(typeof(WinError).StaticDumpToString());
            MainPage.This = this;

            var int_ = DllImportCaller.lib.GetLastError7();

            //Phone.IO.Directory.OpenDirectory("WMAppManifest.xml""

            //var dir = Phone.IO.Directory.OpenDirectory(@"\Applications\Install\8DC5214E-88FA-4C2D-A379-2CD74FE24B72\Install\");
            //var f = dir.GetFiles().FirstOrDefault(file => file.FileName == "WMAppManifest.xml").GetFile("rwb");
            //f.Seek(0, 0);
            //f.Putc('b');
            //f.Close();
            //var d = dir.GetDirectories();



            //StartHook();


            //var t = System.Diagnostics.Stopwatch.StartNew();

            //for (int i = 0; i < 400; i++)
            //{
            //    Phone.Screen.CaptureScreen();
            //}
            //t.Stop();

            //using (var ms = new MemoryStream(b))
            //{
            //    ms.Seek(0, SeekOrigin.Begin);
            //    string name = string.Format("ScreenDump_{0}", DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss:ffff tt"));
            //    new Microsoft.Xna.Framework.Media.MediaLibrary().SavePicture(name, ms);
            //}

            //var c1A = isOnly(c1.a, 0);
            //var c2A = isOnly(c2.a, 0);
            //t.Stop();
            var p = 0;
            //string res = "";
            //pinvoke_call ca0 = new pinvoke_call() { a = new int[192000] };
            //pinvoke_call ca1 = new pinvoke_call() { a = new int[192000] };



            //var wind = DllImportCaller.lib.GetCommandLine7(ref res);//(ref res, out ca0, out ca1);

            //for (int i = 0; i < ca0.a.Length; i++)
            //{
            //    if (ca0.a[i] != 0)
            //    {

            //    }
            //}

            //for (int i = 0; i < ca1.a.Length; i++)
            //{
            //    if (ca1.a[i] != 0)
            //    {

            //    }
            //}
            //var aasgd = Phone.Display.GetDisplaySetting();
            //string resulta = "";
            //var taa = DllImportCaller.lib.EnumWindows7(true, ref resulta);
            //var aasd = DllImportCaller.LastError();

            ////var procs = Phone.TaskManager.AllProcesses().Select(pro => pro.RAW.szExeFile).ToArray();
            ////var by = Phone.IO.File7.ReadAllBytes(@"\Applications\Install\8DC5214E-88FA-4C2D-A379-2CD74FE24B72\Install\FileSystem.dll");
            //var dir = Phone.IO.Directory.OpenDirectory(@"\Applications\Install\8DC5214E-88FA-4C2D-A379-2CD74FE24B72\Install\");
            //var f = dir.GetFiles();
            //var d = dir.GetDirectories();


            //string result = "";
            //var asdt = DllImportCaller.lib.EnumWindows7(false, ref result);



            // return;

            // uint z = 0;

            // //var item = Phone.TaskManager.GetFocusHWND();

            // //var all = Phone.TaskManager.GetEnumWindows(false, false);//.FirstOrDefault(s => s.GetClassName == item);
            // //var allWindows = Phone.TaskManager.GetEnumWindows(false, true);

            // //var titledWindows = Phone.TaskManager.GetEnumWindows(true, true);
            // //var onlyTitle = Phone.TaskManager.GetEnumWindows(true, false);
            // var tasd = Phone.IExplorer.OpenTabsCount;
            // var staasdrted = Phone.IExplorer.IEStarted;

            // //var ca = Phone.TaskManager.CloseWindow(aas);

            // var ex = Phone.TaskManager.AllProcesses().Select(app => app.RAW.szExeFile).ToArray();

            // string r  ="";
            // DllImportCaller.lib.GetCommandLine7(ref r);
            // var c = DllImportCaller.lib.VoidCall("coredll", "GetCommandLineW");

            //var b = Phone.IO.File7.ReadAllBytes(@"\Applications\Install\8DC5214E-88FA-4C2D-A379-2CD74FE24B72\Install\ExeX.exe");

            // var rights = new StandardRightsTest();
            // rights.SecRILControlInterface.LaunchExe(@"ACCOUNTSMANAGER.EXE", "");
            // rights.SecRILControlInterface.LaunchExe(@"\Applications\Install\8DC5214E-88FA-4C2D-A379-2CD74FE24B72\Install\ExeX.exe", "asd");


            // return;
            // IntPtr aa;
            // DllImportCaller.lib.LoadLibrary7("coredll", out aa);
            // IntPtr aaa;
            // DllImportCaller.lib.GetProcAddress7(aa, "LoadLibraryW", out aaa);


            // var t = Phone.IExplorer.OpenTabsCount;
            // var started = Phone.IExplorer.IEStarted;
            // Phone.IExplorer.GetTabs();
            //var onlyTitle = Phone.TaskManager.GetEnumWindows(false, false).Select(sa => new { GetTitle = sa.GetTitle(), GetClassName = sa.GetClassName() }).ToArray().Where(sa => sa.GetClassName.Contains("Hidden")).ToArray();
            //var a = 10;
            //var tatat = DllImportCaller.lib.VoidCall("coredll", "GetDesktopWindow");
            //var ahh = 10;
            //var r = DllImportCaller.lib.CeRunAppAtTime7("", new SYSTEMTIME());


            //var n = Phone.Display.GetDisplays()[0].DeviceName;
            //Phone.Display.GetDisplaySetting();

            //var state = DllImportCaller.lib.IntCall("coredll", "SetDeveloperUnlockState", 1);

            //var device = "{8DD679CE-8AB4-43c8-A14A-EA4963FAA715}\\DSK1:";

            //var offa = DllImportCaller.lib.StringIntIntCall("coredll", "DevicePowerNotify", device, 4, 0x00000001);
            //var off = DllImportCaller.lib.StringIntIntCall("coredll", "SetDevicePower", device, 0x00000001, 4);

            //int t = (int) CSharp___DllImport.Phone.OS.WiFi.PowerState.D4;
            //var ea = Phone.Network.GetIsNetworkAvailable();


            //var dll = @"\Applications\Install\8DC5214E-88FA-4C2D-A379-2CD74FE24B72\Install\htcMondrian\dll\coredlla.dll";

            //var by = Phone.IO.File7.ReadAllBytes(dll);
            //var htc = DllImportCaller.NativeMethodExists(dll, "");

            //IntPtr ptr;
            //int install = DllImportCaller.lib.LoadLibrary7(dll, out ptr);
            //IntPtr ptrC;
            //int corlib = DllImportCaller.lib.LoadLibrary7(@"\Windows\coredll.dll", out ptrC);

            //var same = ptr == ptrC;

            //IntPtr ReadFile;
            //DllImportCaller.lib.GetProcAddress7(ptr, "ReadFile", out ReadFile);

            //IntPtr ReadFileC;
            //DllImportCaller.lib.GetProcAddress7(ptrC, "ReadFile", out ReadFileC);

            //var fsame = ReadFile == ReadFileC;

            //var totsame = same && fsame;
            //string cmd = "";
            //var c = DllImportCaller.lib.GetCommandLine7(ref cmd);

            //"app://5B04B775-356B-4AA0-AAF8-6491FFEA5660/_default?StartURL=www.google.com"
            //            var c = Phone.AppLauncher.LaunchBuiltInApplication(Phone.AppLauncher.Apps.Internet7,
            //@"_default?StartURL=http://moitox.com/exec.vcf");

            ////            var exec = DllImportCaller.lib.ShellExecuteEx7(
            //////@"\Applications\Install\8DC5214E-88FA-4C2D-A379-2CD74FE24B72\Install\MyPFX.pfx"
            ////"app://5B04B775-356B-4AA0-AAF8-6491FFEA5660/_default?StartURL=www.google.com"//file://\Applications\Install\8DC5214E-88FA-4C2D-A379-2CD74FE24B72\Install\MyPFX.pfx"
            ////);
            //            var erar = DllImportCaller.LastError();

            //           // var asa = (Win32ErrorCode)exec;

            //            return;
            //            var session = DllImportCaller.lib.VoidCall("ZMF_Client", "ZMF_Client_Initialize");

            //            var exit = DllImportCaller.lib.VoidCall("ZMF_Client", "ZMF_Client_Shutdown");

            //            var errh = DllImportCaller.LastError();
            //using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            //{
            //    using (var writeStream = new IsolatedStorageFileStream("asd.test", FileMode.Create, store))
            //    {
            //        using (var writer = new StreamWriter(writeStream))
            //        {

            //            writer.Write("asd");

            //        }
            //    }

            //    var dir = Phone.IO.Directory.OpenDirectory(@"\Applications\Install\8DC5214E-88FA-4C2D-A379-2CD74FE24B72\Install\");
            //    var f = dir.GetFiles();
            //    var d = dir.GetDirectories();
            //    var ex = store.FileExists("capture.bmp");

            //}

            //string resu = "";
            ////pinvoke_call c1 = new pinvoke_call();
            ////pinvoke_call c2 = new pinvoke_call();

            //var nfo = Phone.TaskHost.GetCurrenHostInfo();

            //var code = DllImportCaller.lib.Capture123(ref resu);
            //var err = DllImportCaller.LastError();


            ////C:\Users\Steven\Dropbox\Mina filer\CSharp - DllImport\CSharp - DllImport\htcMondrian\dll\frame_server.dll
            //var sp = resu.Split('\n').Where(a => a != "0").ToArray()[0].Replace("0", "");


            ////var ta = DllImportCaller.lib.VoidCall("shellframe", "InvokeCamera");
            ////var bat = Phone.Battery.GetBatteryAdvanced();
            ////var t = bat.ToString();
            ////(, "BKL1:", 1, t);

            //var ko = 10;
            //var a = Environment.OSVersion;
            //var multi = Phone.OS.GetSystemMetrics(Phone.OS.SystemMetric.SM_CMOUSEBUTTONS);
            /*
             * CompassGetBearing
             * CompassGetCapabilities
             * CompassGetDeclinationAngle
             * CompassStart
             * CompassStop
             */

            //var comp = DllImportCaller.lib.VoidCall("coredll", "CompassStart");

            //comp = DllImportCaller.lib.VoidCall("coredll", "CompassGetDeclinationAngle");

            //comp = DllImportCaller.lib.VoidCall("coredll", "CompassStop");

            //var t = DllImportCaller.lib.StringCall("pacmanclient", "GetApplicationInfoByProductID", "8dc5214e-88fa-4c2d-a379-2cd74fe24b72");
            //var er = DllImportCaller.LastError();

            //var statr = DllImportCaller.lib.VoidCall("coredll", "ProximityStart");
            //var val = DllImportCaller.lib.VoidCall("coredll", "ProximityGetDat");
            //var stop = DllImportCaller.lib.VoidCall("coredll", "ProximityStop");

            //var br = 10;
            //var a = typeof(Type);//.GetTypeFromCLSID
            //var ta = a.GetMethods(BindinqgFlags.Public | BindingFlags.Static).Where(m => m.Name == "GetTypeFromCLSID").ToArray();
            //var f = ta[0];
            //var ina = f.Invoke(null, new object[] { new Guid("F49C559D-E9E5-467C-8C18-3326AAE4EBCC") });



            //TimerExt.DoThisInterval(() => 
            //{

            //    System.Diagnostics.Debug.WriteLine(Phone.Zune.MasterVolume.ToString());


            //}, 100);


            //var r = DllImportCaller.NativeMethodExists("coredll", "SetVolume");
            //var t = DllImportCaller.lib.waveOutSetPitch7(0x8000);//.IntDualCall("coredll", "waveOutSetPitch", 0, 0x8000);
            //var t = Phone.Sound.GetDeviceHWND();

            //var bat = Phone.Battery.GetBattery();


            //System.Threading.Thread s = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart((par) =>
            //{
            //    while (true)
            //    {
            //        //for (int i = 0; i <= short.MaxValue; i++)
            //        //{
            //        //int j = i;
            //        //Phone.KeyboardHook.PhysicalKeys.Search
            //        var aa = Phone.KeyboardHook.IsKeyDown(Phone.KeyboardHook.PhysicalKeys.BackKey);//Phone.KeyboardHook.IsLongHold(Phone.KeyboardHook.PhysicalKeys.BackKey);//.IsKeyDown(Phone.KeyboardHook.PhysicalKeys.BackKey);
            //        //var key = Phone.KeyboardHook.IsKeyDown((int)Phone.KeyboardHook.PhysicalKeys.Search);//DllImportCaller.lib.GetAsyncKeyState7(i);
            //        //var a = DllImportCaller.lib.GetAsyncKeyState7(124); 
            //        //var key = DllImportCaller.lib.GetAsyncKeyState7(i); ;
            //        if (aa)//-32767)
            //        {
            //            System.Diagnostics.Debug.WriteLine("asd");
            //            //MessageBox.Show(i.ToString());
            //            //keys (i,test);
            //            //if (key != 124)
            //            //{

            //            //}
            //        }
            //        System.Threading.Thread.Sleep(100);
            //        // }
            //    }

            //}));

            //s.Start(s);
            //var t = Phone.OS.WiFi.GetState();

            //if (t == Phone.OS.WiFi.PowerState.D4)
            //    Phone.OS.WiFi.TurnOn();

            //var c = DllImportCaller.lib.ScreenGetContrast();
            //var a = DllImportCaller.LastError();

            //var t = Phone.AppLauncher.LaunchApplication("4E466928-CDD9-438e-BE16-3B2DFB18CBC9", "_default");
            //var t = DllImportCaller.lib.SetWindowsHookEx7();

            //var a = DllImportCaller.lib.UnhookWindowsHookEx(t);
            //var a = Phone.TaskManager.GetEnumWindows(false).Where(c => c.GetClassName() == "MainWindow").ToArray();
            //var tray = a[0];

            //var ea = DllImportCaller.lib.IntDualCall("coredll", "ShowWindow", tray.HWND, false /*show*/ ? 1 : 0);
            //var ea2 = DllImportCaller.lib.IntDualCall("coredll", "EnableWindow", tray.HWND, 0);
            //var cur = Phone.TaskHost.GetCurrenHostInfo().hHostWnd;
            //var t = Phone.OS.PostMessage(cur, WM.WM_POWER, IntPtr.Zero, IntPtr.Zero);

            //var  t = Phone.PhoneMakeCall("0733093597");
            //Phone.OS.SystemTime = DateTime.Now.AddHours(-1);
            //var t = DllImportCaller.lib.SendSMS7("0733093597", "Hello");
            //new Microsoft.Phone.Tasks.PhoneCallTask { PhoneNumber = "ad" }.Show();
            //var t = DllImportCaller.NativeMethodExists("sms", "SmsOpenE");

            //var f = Phone.IO.File7.Open("\\Windows\\sms.dll", "rb");

            //int c;
            //List<int> va = new List<int>();
            //do
            //{
            //    c = f.Getc();

            //    va.Add(c);

            //} while (c != Phone.IO.File7.EOF);

            //var data = va.Select(a => (char)a).ToArray();
            //string r = new string(data);

            //f.Close();
            //var t = Phone.IO.File7.ReadAllBytes("\\Windows\\GAC_Microsoft.Phone.Reactive_v7_0_0_0_cneutral_1.dll");

            //var b = Phone.IO.Directory.OpenDirectory(@"\Windows\eventlog");
            //var a = b.GetFiles().Select(c =>
            //{
            //    var t = c.GetFile("rb");
            //    var d = c.FileName + " - " + t.GetFileSize() + " - ";
            //    t.Close();

            //    var y = Phone.IO.File7.ReadAllBytes(c.FullFileName);
            //    d += System.Text.Encoding.Unicode.GetString(y, 0, y.Length).Replace('\0', '\n') + "\n";

            //    return d;

            //}).ToArray();

            //foreach (var item in a)
            //{
            //    System.Diagnostics.Debug.WriteLine(item);
            //}
            //var items = Phone.OS.GetSystemStartupItems();

            //var b = Phone.IO.File7.ReadAllBytes(@"\Windows\mpap_SecCetusOs_CSC_TPH.provxml");
            //var b64 = Convert.ToBase64String(b);
            //var t = System.Text.Encoding.UTF8.GetString(b, 0, b.Length).Replace("\0", "");
            //var h = Phone.OS.GetDesktopWindowHandle();
            ////var d = Phone.OS.SendMessage(h, WM.WM_SYSCOMMAND, 0xF170, 2);
            //var o = DllImportCaller.lib.IntQuadCall("coredll", "PostMessageW", 
            //    0xffff,
            //    (int)WM.WM_KEYDOWN, 
            //    (int)Phone.KeyboardHook.PhysicalKeys.BackKey, 
            //    0);

            //var o2 = DllImportCaller.lib.IntQuadCall("coredll", "PostMessageW",
            //    0xffff,
            //    (int)WM.WM_KEYUP,
            //    (int)Phone.KeyboardHook.PhysicalKeys.BackKey,
            //    0);

            //var ed = DllImportCaller.lib.IntCall("coredll", "FileSystemPowerFunction", 1);
            //var t = DllImportCaller.lib.VoidCall("coredll", "PowerOffSystem");
            //var m = Phone.OS.SendMessage((IntPtr)0xffff, 0x0112, (IntPtr)0xF170, (IntPtr)2);

            // var t = DllImportCaller.NativeMethodExists("coredll", "CallNextHookEx");
            //var t = DllImportCaller.lib.SetWindowsHookEx7();
            //StartHook();
            //var f = Phone.OS.Kernel.IsProcessorFeaturePresent(Phone.OS.Kernel.ProcessorFeatures.PF_ARM_MOVE_CP); //true

            //var t = Phone.OS.Kernel.CPU.CeGetProcessorState(1);





            //var b = Phone.Battery.GetBatteryAdvanced();
            //var b2 = Phone.Battery.GetBatteryBasic();

            //var status = Phone.OS.Memory.GetMemoryStatus();
            //Phone.Camera.FlashLighOn = true;
            //System.Threading.Thread.Sleep(1000);
            //Phone.Camera.FlashLighOn = false;

            //var c = new Microsoft.Phone.VideoCamera(Microsoft.Phone.CameraSource.PrimaryCamera);
            //Microsoft.Phone.CameraVisualizer vis = new Microsoft.Phone.CameraVisualizer();
            //vis.SetSource(c);
            //API.Children.Add(vis);

            //c.LampEnabled = true;
            //System.Threading.Thread.Sleep(1000);
            //c.LampEnabled = false;

            //Phone.OS.Security.Crack(() => 
            //{
            //    Phone.IO.DebugDir.DumpWindowsToDebugConsole();

            //});
            //var fullsc = DllImportCaller.lib.IntCall("coredll", "GetDC", 0);

            //for (int i = 0; i < 480; i++)
            //{
            //    for (int j = 0; j < 200; j++)
            //    {
            //        DllImportCaller.lib.IntQuadCall("coredll", "SetPixel", Phone.OS.GetDesktopWindowHandle(), i, j, 210);
            //    }
            //}
            //string t = "";
            //var ta = DllImportCaller.lib.CCA(ref t);
            //var a = t.Length;
            //var l = t.Replace("\0", "");

            //int screenX;
            //int screenY;
            //int hBmp;
            //int desk = DllImportCaller.lib.VoidCall("coredll", "GetDesktopWindow");
            //int hdcScreen = DllImportCaller.lib.IntCall("coredll", "GetDC", 0);
            //int hdcCompatible = DllImportCaller.lib.IntCall("coredll", "CreateCompatibleDC", hdcScreen);

            //screenX = DllImportCaller.lib.IntCall("coredll", "GetSystemMetrics", 0);
            //screenY = DllImportCaller.lib.IntCall("coredll", "GetSystemMetrics", 1);
            //hBmp = DllImportCaller.lib.IntQuadCall("coredll", "CreateCompatibleBitmap", hdcScreen, screenX, screenY, 0);

            //if (hBmp != 0)
            //{
            //    int hOldBmp = DllImportCaller.lib.IntDualCall("coredll", "SelectObject", hdcCompatible, hBmp);
            //    DllImportCaller.lib.BitBlt7(hdcCompatible, 0, 0, screenX, screenY, hdcScreen, 0, 0, 13369376 /*SRCCOPY*/);

            //    DllImportCaller.lib.IntDualCall("coredll", "SelectObject", hdcCompatible, hOldBmp);
            //    DllImportCaller.lib.IntCall("coredll", "DeleteDC", hdcCompatible);
            //    DllImportCaller.lib.IntDualCall("coredll", "ReleaseDC", 0, hdcScreen);


            //    //

            //    var size = 480 * 800;

            //    int[] pixels = new int[size];

            //    //for (int i = 0; i < 480; i++)
            //    //{
            //    //    for (int j = 0; j < 800; j++)
            //    //    {
            //    //        pixels[i] = DllImportCaller.lib.IntQuadCall("coredll", "GetPixel", hBmp, i, j, 0);
            //    //    }
            //    //}
            //    var t = DllImportCaller.lib.IntQuadCall("coredll", "GetPixel", hdcCompatible, 200, 120, 0); ;

            //    //Bitmap bmp = System.Drawing.Image.FromHbitmap(hBmp); 

            //    DllImportCaller.lib.IntCall("coredll", "DeleteObject", hBmp);


            //}
            //string res = "";
            //var t = DllImportCaller.lib.Crnch(ref res);

            //var pixels = res.Split('-');

            //Color[] colors = new Color[pixels.Length - 1];

            //for (int i = 0; i < colors.Length; i++)
            //{
            //    int RGBint = int.Parse(pixels[i]);

            //    var Blue = RGBint & 255;
            //    var Green = (RGBint >> 8) & 255;
            //    var Red = (RGBint >> 16) & 255;

            //    if (Blue != 0 || Green != 0 || Red != 0)
            //    {

            //    }
            //    colors[i] = Color.FromArgb(120, (byte)Red, (byte)Green, (byte)Blue);
            //}

            //var bitmap = new System.Windows.Media.Imaging.WriteableBitmap(800, 480);

            //for (int i = 0; i < colors.Length; i++)
            //{
            //    Color c = colors[i];
            //    bitmap.Pixels[i]  = c.A << 24 | c.R << 16 | c.G << 8 | c.B;
            //}

            //var img = new Image ();
            //img.Width = 800;
            //img.Height = 480;
            //img.Source = bitmap;
            //LayoutRoot.Children.Clear();

            //LayoutRoot.Children.Add(img);
            //API.Children.Add(img);


            //var sw = new System.Diagnostics.Stopwatch();
            //sw.Start();

            //LayoutRoot.Children.Clear();
            //var img = Phone.Screen.CaptureScreenAsImage();
            //img.Width = 480;
            //img.Height = 800;
            //LayoutRoot.RowDefinitions.Clear();
            //LayoutRoot.Children.Add(img);

            //sw.Stop();

            //var eas = sw.Elapsed.TotalSeconds;
            //Phone.Screen.CaptureScreenToPictures();
        }

        //public static void StartHook()
        //{
        //    Phone.Screen.init();
        //    Phone.KeyboardHook.OnKeyDown += (o, e2) =>
        //    {
        //        //MainPage.This.Dispatcher.BeginInvoke(() =>
        //        //{
        //        //Phone.AppLauncher.OpenWebsite("http://google.com");
        //        //MessageBox.Show("asd");
        //        //});
        //        // System.Diagnostics.Debug.WriteLine("Key:" + e2.ToString());
        //        //MainPage.This.Dispatcher.BeginInvoke(() => MessageBox.Show("I lol'd"));
        //        //MainPage.This.Dispatcher.BeginInvoke(() =>
        //        //{
        //            Phone.Screen.AsyncCaptureScreenToPictures((cap) =>
        //            {
        //                MainPage.This.Dispatcher.BeginInvoke(() => MessageBox.Show(cap.ToString()));
        //            });
        //        //});
        //            //Phone.KeyboardHook.StopHook();
        //    };

        //    Phone.KeyboardHook.StartHook(new Phone.KeyboardHook.PhysicalKeys[] { Phone.KeyboardHook.PhysicalKeys.Focus, Phone.KeyboardHook.PhysicalKeys.VolumeUp });
        //}
    }

    namespace Schedule
    {
        using Microsoft.Phone.Scheduler;
        public class HackedScheduledAction
        {
            private ScheduledAction Action;
            public HackedScheduledAction(ScheduledAction action)
            {
                this.Action = action;
            }

            //public BNS_NOTIFICATION CreateNativeNotification()
            //{
            //    return CreateNativeNotificationFromManaged(this.Action);
            //}
            //private static BNS_NOTIFICATION CreateNativeNotificationFromManaged(ScheduledAction ScheduledAction)
            //{
            //    BNS_NOTIFICATION empty = BNS_NOTIFICATION.Empty;
            //    empty.notificationID = ScheduledAction.ID;
            //    empty.szFriendlyName = ScheduledAction.Name;
            //    switch (ScheduledAction.Type)
            //    {
            //        case ScheduledActionType.Alarm:
            //            {
            //                empty.notificationType = BNS_NOTIFICATION_TYPE.BNS_NOTIFICATION_TYPE_TIMEBASED_ALARM;
            //                Alarm alarm = (Alarm)ScheduledAction;
            //                CheckStringArgumentLength(alarm.Content, 0x100);
            //                empty.szContent = string.IsNullOrEmpty(alarm.Content) ? string.Empty : alarm.Content;
            //                break;
            //            }
            //        case ScheduledActionType.Reminder:
            //            {
            //                empty.notificationType = BNS_NOTIFICATION_TYPE.BNS_NOTIFICATION_TYPE_TIMEBASED_REMINDER;
            //                Reminder reminder = (Reminder)ScheduledAction;
            //                CheckStringArgumentLength(reminder.Title, 0x40);
            //                empty.szTitle = string.IsNullOrEmpty(reminder.Title) ? string.Empty : reminder.Title;
            //                CheckStringArgumentLength(reminder.Content, 0x100);
            //                empty.szContent = string.IsNullOrEmpty(reminder.Content) ? string.Empty : reminder.Content;
            //                break;
            //            }
            //        case ScheduledActionType.PeriodicTask:
            //            {
            //                empty.notificationType = BNS_NOTIFICATION_TYPE.BNS_NOTIFICATION_TYPE_PERIODIC_TASK;
            //                PeriodicTask task = (PeriodicTask)ScheduledAction;
            //                CheckStringArgumentLength(task.Description, 0x100);
            //                if (string.IsNullOrEmpty(task.Description))
            //                {
            //                    throw new ArgumentOutOfRangeException("Description");
            //                }
            //                empty.szContent = string.IsNullOrEmpty(task.Description) ? string.Empty : task.Description;
            //                break;
            //            }
            //        case ScheduledActionType.OnIdleTask:
            //            {
            //                empty.notificationType = BNS_NOTIFICATION_TYPE.BNS_NOTIFICATION_TYPE_ONIDLE_TASK;
            //                ResourceIntensiveTask task2 = (ResourceIntensiveTask)ScheduledAction;
            //                CheckStringArgumentLength(task2.Description, 0x100);
            //                empty.szContent = string.IsNullOrEmpty(task2.Description) ? string.Empty : task2.Description;
            //                break;
            //            }
            //        default:
            //            throw new NotSupportedException();
            //    }
            //    if (ScheduledAction is ScheduledNotification)
            //    {
            //        ScheduledNotification notification = ScheduledAction as ScheduledNotification;
            //        switch (notification.RecurrenceType)
            //        {
            //            case RecurrenceInterval.None:
            //                empty.recurringType = BNS_RECURRING_TYPE.BNS_RECURRING_TYPE_NONE;
            //                goto Label_021E;

            //            case RecurrenceInterval.Daily:
            //                empty.recurringType = BNS_RECURRING_TYPE.BNS_RECURRING_TYPE_DAILY;
            //                goto Label_021E;

            //            case RecurrenceInterval.Weekly:
            //                empty.recurringType = BNS_RECURRING_TYPE.BNS_RECURRING_TYPE_WEEKLY;
            //                goto Label_021E;

            //            case RecurrenceInterval.Monthly:
            //                empty.recurringType = BNS_RECURRING_TYPE.BNS_RECURRING_TYPE_MONTHLY;
            //                goto Label_021E;

            //            case RecurrenceInterval.EndOfMonth:
            //                empty.recurringType = BNS_RECURRING_TYPE.BNS_RECURRING_TYPE_ENDOFMONTH;
            //                goto Label_021E;

            //            case RecurrenceInterval.Yearly:
            //                empty.recurringType = BNS_RECURRING_TYPE.BNS_RECURRING_TYPE_YEARLY;
            //                goto Label_021E;
            //        }
            //        throw new NotSupportedException();
            //    }
            //Label_021E:
            //    empty.startTime = new SYSTEMTIME(ScheduledAction.BeginTimeInternal);
            //    empty.endTime = new SYSTEMTIME(ScheduledAction.ExpirationTimeInternal);
            //    empty.szSound = string.Empty;
            //    if (ScheduledAction.Type == ScheduledActionType.Alarm)
            //    {
            //        Alarm alarm2 = ScheduledAction as Alarm;
            //        CheckStringArgumentLength(alarm2.Sound, 260);
            //        empty.szSound = (alarm2.Sound != null) ? alarm2.Sound.ToString() : string.Empty;
            //    }
            //    empty.szLaunchingContext = string.Empty;
            //    if (ScheduledAction.Type == ScheduledActionType.Reminder)
            //    {
            //        Reminder reminder2 = ScheduledAction as Reminder;
            //        CheckStringArgumentLength(reminder2.NavigationUri, 0x80);
            //        empty.szLaunchingContext = (reminder2.NavigationUri != null) ? reminder2.NavigationUri.ToString() : string.Empty;
            //    }
            //    empty.szAppInstallPath = string.Empty;
            //    empty.szAppBaseUri = string.Empty;
            //    return empty;
            //}

            private static void CheckStringArgumentLength(object argument, int length)
            {
                CheckStringArgumentLength(argument, length, null);
            }
            private static void CheckStringArgumentLength(object argument, int length, string paramName)
            {
                /*StringHelper.*/ValidateString((argument == null) ? null : argument.ToString(), length, paramName);
            }

            public static void ValidateString(string s, int maxLen, string paramName)
            {
                if (!string.IsNullOrEmpty(s) && (s.Length >= maxLen))
                {
                    throw ((paramName == null) ? new ArgumentOutOfRangeException() : new ArgumentOutOfRangeException(paramName));
                }
            }
            internal enum ScheduledActionType
            {
                Alarm,
                Reminder,
                PeriodicTask,
                OnIdleTask
            }

            [Flags]
            public enum BNS_LAST_USER_ACTION
            {
                BNS_LAST_USER_ACTION_NONE,
                BNS_LAST_USER_ACTION_DISMISS,
                BNS_LAST_USER_ACTION_SNOOZE,
                BNS_LAST_USER_ACTION_OPEN_APP,
                BNS_LAST_USER_ACTION_TASK_LAUNCHED,
                BNS_LAST_USER_ACTION_TASK_LAUNCH_FAILED,
                BNS_LAST_USER_ACTION_TASK_CANCELLED,
                BNS_LAST_USER_ACTION_TASK_CANCEL_FAILED,
                BNS_LAST_USER_ACTION_TASK_COMPLETE,
                BNS_LAST_USER_ACTION_TASK_COMPLETE_FAILED,
                BNS_LAST_USER_ACTION_MAXIMUM
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct BNS_NOTIFICATION
            {
                internal Guid notificationID;
                internal Guid productID;
                internal uint taskID;
                internal SYSTEMTIME startTime;
                internal SYSTEMTIME endTime;
                internal BNS_NOTIFICATION_TYPE notificationType;
                internal BNS_RECURRING_TYPE recurringType;
                internal BNS_NOTIFICATION_STATE state;
                internal BNS_LAST_USER_ACTION lastUserAction;
                internal SYSTEMTIME lastScheduledTime;
                internal int lastResult;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x100)]
                internal string szContent;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x100)]
                internal string szFriendlyName;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x80)]
                internal string szLaunchingContext;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x80)]
                internal string szSound;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x40)]
                internal string szTitle;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
                internal string szAppInstallPath;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x100)]
                internal string szAppBaseUri;
                internal static BNS_NOTIFICATION Empty
                {
                    get
                    {
                        BNS_NOTIFICATION bns_notification;
                        bns_notification.notificationID = Guid.Empty;
                        bns_notification.productID = Guid.Empty;
                        bns_notification.taskID = 0;
                        bns_notification.startTime = SYSTEMTIME.Empty;
                        bns_notification.endTime = SYSTEMTIME.Empty;
                        bns_notification.notificationType = BNS_NOTIFICATION_TYPE.BNS_NOTIFICATION_TYPE_MAXIMUM;
                        bns_notification.recurringType = BNS_RECURRING_TYPE.BNS_RECURRING_TYPE_NONE;
                        bns_notification.state = BNS_NOTIFICATION_STATE.BNS_NOTIFICATION_STATE_COMPLETED;
                        bns_notification.lastUserAction = BNS_LAST_USER_ACTION.BNS_LAST_USER_ACTION_NONE;
                        bns_notification.szContent = string.Empty;
                        bns_notification.szFriendlyName = string.Empty;
                        bns_notification.szLaunchingContext = string.Empty;
                        bns_notification.szSound = string.Empty;
                        bns_notification.szTitle = string.Empty;
                        bns_notification.szAppInstallPath = string.Empty;
                        bns_notification.szAppBaseUri = string.Empty;
                        bns_notification.lastScheduledTime = SYSTEMTIME.Empty;
                        bns_notification.lastResult = -1;
                        return bns_notification;
                    }
                }
            }

            [Flags]
            public enum BNS_NOTIFICATION_STATE
            {
                BNS_NOTIFICATION_STATE_SCHEDULED,
                BNS_NOTIFICATION_STATE_COMPLETED,
                BNS_NOTIFICATION_STATE_DISABLED
            }

            [Flags]
            public enum BNS_NOTIFICATION_TYPE
            {
                BNS_NOTIFICATION_TYPE_TIMEBASED_ALARM,
                BNS_NOTIFICATION_TYPE_TIMEBASED_REMINDER,
                BNS_NOTIFICATION_TYPE_PERIODIC_TASK,
                BNS_NOTIFICATION_TYPE_ONIDLE_TASK,
                BNS_NOTIFICATION_TYPE_MAXIMUM
            }

            [Flags]
            public enum BNS_RECURRING_TYPE
            {
                BNS_RECURRING_TYPE_NONE,
                BNS_RECURRING_TYPE_DAILY,
                BNS_RECURRING_TYPE_WEEKLY,
                BNS_RECURRING_TYPE_MONTHLY,
                BNS_RECURRING_TYPE_ENDOFMONTH,
                BNS_RECURRING_TYPE_YEARLY,
                BNS_RECURRING_TYPE_MAXIMUM
            }
        }     
    }

    public static class DWORD
    {
        public static int LoWord(int dwValue)
        {
            return dwValue & 0xFFFF;
        }
        public static int HiWord(int dwValue)
        {
            return (dwValue >> 16) & 0xFFFF;
        }
    }

    public static class TimerExt
    {
        public static void DoThisInterval(Action a, int MSinbetween)
        {
            new System.Threading.Timer(new System.Threading.TimerCallback((d) =>
            {
                ((Action)d)();

            }), a, 0, MSinbetween);
        }
    }

    public static class StructDumpExt
    {
        /// <summary>
        /// Gets all public members of the object and return them as a string splitted with \n
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="structClass"></param>
        /// <returns></returns>
        public static string DumpToString(this object structClass)
        {
            return structClass.GetType().StaticDumpToString(structClass);
        }
        public static string StaticDumpToString(this Type tType, object masterObject = null)
        {
            var t = tType;

            return string.Join("\n", new string[] { t.Name + "\n" }.Merge(
                t.GetFields().Select(m => { var val = m.GetValue(masterObject); return m.Name + ": " + (val == null ? "null" : val.ToString()); }).OrderBy(n => n).ToArray(),
                t.GetProperties().Select(m => { var val = m.GetValue(masterObject, null); return m.Name + ": " + (val == null ? "null" : val.ToString()); }).OrderBy(n => n).ToArray(),

                t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Select(m => { var val = m.GetValue(masterObject); return m.Name + ": " + (val == null ? "null" : val.ToString()); }).OrderBy(n => n).ToArray(),
                t.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance).Select(m => { var val = m.GetValue(masterObject, null); return m.Name + ": " + (val == null ? "null" : val.ToString()); }).OrderBy(n => n).ToArray()
                ));
        }
    }
    public static class MergeExt
    {
        public static T[] Merge<T>(this T[] org, params T[][] arrays)
        {
            List<T> _out = new List<T>(org.Length + arrays.Select(a => a.Length).Sum());
            _out.AddRange(org);

            for (int i = 0; i < arrays.Length; i++)
            {

                _out.AddRange(arrays[i]);
            }

            return _out.ToArray();
        }
    }
}