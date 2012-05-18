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

namespace CSharp___DllImport
{
    public static partial class Phone
    {
        public static partial class OS
        {
            public static partial class Kernel
            {
                // Control Code flags
                private const uint FILE_DEVICE_UNKNOWN = 0x00000022;
                private const uint FILE_DEVICE_HAL = 0x00000101;
                private const uint FILE_DEVICE_CONSOLE = 0x00000102;
                private const uint FILE_DEVICE_PSL = 0x00000103;
                private const uint METHOD_BUFFERED = 0;
                private const uint METHOD_IN_DIRECT = 1;
                private const uint METHOD_OUT_DIRECT = 2;
                private const uint METHOD_NEITHER = 3;
                private const uint FILE_ANY_ACCESS = 0;
                private const uint FILE_READ_ACCESS = 0x0001;
                private const uint FILE_WRITE_ACCESS = 0x0002;

                private static uint CTL_CODE(uint DeviceType, uint Function, uint Method, uint Access)
                {
                    return ((DeviceType << 16) | (Access << 14) | (Function << 2) | Method);
                }

                /// <summary>
                /// the method needed to be called to actually reset the device.
                /// </summary>
                public static void ResetDevice()
                {
                    throw new Exception("I realy DONT recommend calling this, in documentation is says restarts. but im not sure if it does a Hard Reset. I myself wont try and see what it does!!!!");
                    
                    uint bytesReturned = 0;
                    uint IOCTL_HAL_REBOOT = CTL_CODE(FILE_DEVICE_HAL, 15, METHOD_BUFFERED, FILE_ANY_ACCESS);
                    DllImportCaller.lib.KernelIoControl(IOCTL_HAL_REBOOT, IntPtr.Zero, 0, IntPtr.Zero, 0, ref bytesReturned);
                }

                /* Processor features */
                /*        
                    #define PF_FLOATING_POINT_PRECISION_ERRATA  0
                    #define PF_FLOATING_POINT_EMULATED          1
                    #define PF_COMPARE_EXCHANGE_DOUBLE          2
                    #define PF_MMX_INSTRUCTIONS_AVAILABLE       3
                    #define PF_PPC_MOVEMEM_64BIT_OK             4
                    #define PF_ALPHA_BYTE_INSTRUCTIONS          5
                    #define PF_XMMI_INSTRUCTIONS_AVAILABLE      6
                    #define PF_3DNOW_INSTRUCTIONS_AVAILABLE     7
                    #define PF_RDTSC_INSTRUCTION_AVAILABLE      8
                    #define PF_PAE_ENABLED                      9
                    #define PF_XMMI64_INSTRUCTIONS_AVAILABLE   10
                    #define PF_SSE_DAZ_MODE_AVAILABLE          11
                    #define PF_NX_ENABLED                      12
                    #define PF_SSE3_INSTRUCTIONS_AVAILABLE     13
                    #define PF_COMPARE_EXCHANGE128             14
                    #define PF_COMPARE64_EXCHANGE128           15
                    #define PF_CHANNELS_ENABLED                16
                    #define PF_XSAVE_ENABLED                   17
                 */

                public enum ProcessorFeatures : uint
                {
                    // ARM
                    PF_ARM_V4 = 0x80000001,
                    PF_ARM_V5 = 0x80000002,
                    PF_ARM_V6 = 0x80000003,
                    PF_ARM_V7 = 0x80000004,
                    PF_ARM_THUMB = 0x80000005,
                    PF_ARM_JAZELLE = 0x80000006,
                    PF_ARM_DSP = 0x80000007,
                    PF_ARM_MOVE_CP = 0x80000008,
                    PF_ARM_VFP10 = 0x80000009,
                    PF_ARM_MPU = 0x8000000A,
                    PF_ARM_WRITE_BUFFER = 0x8000000B,
                    PF_ARM_MBX = 0x8000000C,
                    PF_ARM_L2CACHE = 0x8000000D,
                    PF_ARM_PHYSICALLY_TAGGED_CACHE = 0x8000000E,
                    PF_ARM_VFP_SINGLE_PRECISION = 0x8000000F,
                    PF_ARM_VFP_DOUBLE_PRECISION = 0x80000010,
                    PF_ARM_ITCM = 0x80000011,
                    PF_ARM_DTCM = 0x80000012,
                    PF_ARM_UNIFIED_CACHE = 0x80000013,
                    PF_ARM_WRITE_BACK_CACHE = 0x80000014,
                    PF_ARM_CACHE_CAN_BE_LOCKED_DOWN = 0x80000015,
                    PF_ARM_L2CACHE_MEMORY_MAPPED = 0x80000016,
                    PF_ARM_L2CACHE_COPROC = 0x80000017,


                    // Specific OEM extentions                         
                    PF_ARM_INTEL_XSCALE = 0x80010001,
                    PF_ARM_INTEL_PMU = 0x80010002,
                    PF_ARM_INTEL_WMMX = 0x80010003,

                    // MIPS (Non ARM processors)

                    //PF_MIPS_MIPSII = 0x81000001, // MIPSII  instruction set
                    //PF_MIPS_MIPSIII = 0x81000002, // MIPSIII instruction set
                    //PF_MIPS_MIPSIV = 0x81000003, // MIPSIV  instruction set
                    //PF_MIPS_SMART_ASE = 0x81000004, // MIPS smart card arch. specific ext.
                    //PF_MIPS_MIPS16 = 0x81000005, // MIPS16  instruction set
                    //PF_MIPS_MIPS32 = 0x81000006, // MIPS32  instruction set
                    //PF_MIPS_MIPS64 = 0x81000007, // MIPS64  instruction set
                    //PF_MIPS_FPU = 0x81000008, // FPU support
                    //PF_MIPS_CPU_4KEX = 0x81000009, // "R4K" exception model
                    //PF_MIPS_CPU_4KTLB = 0x8100000A, // "R4K" TLB handler
                    //PF_MIPS_CPU_32FPR = 0x8100000B, // 32 dbl. prec. FP registers
                    //PF_MIPS_CPU_COUNTER = 0x8100000C, // Cycle count/compare
                    //PF_MIPS_CPU_WATCH = 0x8100000D, // watchpoint registers
                    //PF_MIPS_CPU_DIVEC = 0x8100000E, // dedicated interrupt vector
                    //PF_MIPS_CPU_VCE = 0x8100000F, // virt. coherence conflict possible
                    //PF_MIPS_CPU_CACHE_CDEX = 0x81000010, // Create_Dirty_Exclusive CACHE op
                    //PF_MIPS_CPU_MCHECK = 0x81000011, // Machine check exception
                    //PF_MIPS_CPU_EJTAG = 0x81000012, // EJTAG exception
                    //PF_MIPS_PERF_COUNTER = 0x81000013, // perf counter
                    //PF_MIPS_ARCH_2 = 0x81000014, // arch. release 2

                    // SHx (Non ARM processors)

                    //PF_SHX_SH3 = 0x82000001,
                    //PF_SHX_SH4 = 0x82000002,
                    //PF_SHX_SH5 = 0x82000003,
                    //PF_SHX_DSP = 0x82000004,
                    //PF_SHX_FPU = 0x82000005,

                    //Desktop x86 (Non ARM processors)

                    //PF_FLOATING_POINT_PRECISION_ERRATA = 0,
                    //PF_FLOATING_POINT_EMULATED = 1,
                    //PF_COMPARE_EXCHANGE_DOUBLE = 2,
                    //PF_MMX_INSTRUCTIONS_AVAILABLE = 3,
                    //PF_PPC_MOVEMEM_64BIT_OK = 4,
                    //PF_ALPHA_BYTE_INSTRUCTIONS = 5,
                    //PF_XMMI_INSTRUCTIONS_AVAILABLE = 6,
                    //PF_3DNOW_INSTRUCTIONS_AVAILABLE = 7,
                    //PF_RDTSC_INSTRUCTION_AVAILABLE = 8,
                    //PF_PAE_ENABLED = 9,
                    //PF_XMMI64_INSTRUCTIONS_AVAILABLE = 10,
                    //PF_SSE_DAZ_MODE_AVAILABLE = 11,
                    //PF_NX_ENABLED = 12,
                    //PF_SSE3_INSTRUCTIONS_AVAILABLE = 13,
                    //PF_COMPARE_EXCHANGE128 = 14,
                    //PF_COMPARE64_EXCHANGE128 = 15,
                    //PF_CHANNELS_ENABLED = 16,
                    //PF_XSAVE_ENABLED = 17
                }

                public static bool IsProcessorFeaturePresent(ProcessorFeatures feature)
                {
                    return DllImportCaller.lib.IntCall("coredll", "IsProcessorFeaturePresent", (int)feature) != 0;
                }

                public static partial class CPU
                {
                    public enum CE_PROCESSOR_STATE
                    {
                        /// <summary>
                        /// Call GetLastError for error code
                        /// </summary>
                        CS_ERROR_OCCURED = 0,
                        /// <summary>
                        /// The processor is in the middle of a power cycle change.
                        /// </summary>
                        CE_PROCESSOR_STATE_IN_TRANSITION = 1,
                        /// <summary>
                        /// The processor is turned off.
                        /// </summary>
                        CE_PROCESSOR_STATE_POWERED_OFF = 2,
                        /// <summary>
                        /// The processor is running normally.
                        /// </summary>
                        CE_PROCESSOR_STATE_POWERED_ON = 3
                    }

                    //[Obsolete("Wrong states are reurned, need some more network research", true)] //done.
                    /// <summary>
                    /// The windows phones only have 1 cpu at the moment so "1" goes in
                    /// </summary>
                    /// <param name="_1basedIndex"></param>
                    /// <returns></returns>
                    public static CE_PROCESSOR_STATE CeGetProcessorState(int _1basedIndex)
                    {
                        return (CE_PROCESSOR_STATE)DllImportCaller.lib.IntCall("coredll", "CeGetProcessorState", _1basedIndex);
                    }
                }
            }
        }
    }
}
