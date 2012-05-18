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
using System.Collections;
using System.Linq;

namespace CSharp___DllImport
{
    public static partial class Phone
    {
        public static class KeyboardHook
        {
            private class LongKeyData
            {
                public DateTime AtTime;
                public PhysicalKeys Key;
            }

            private static System.Collections.Generic.Stack<LongKeyData> longHolder;

            static KeyboardHook()
            {
                longHolder = new System.Collections.Generic.Stack<LongKeyData>(10);
            }

            public static bool IsKeyDown(PhysicalKeys key)
            {
                return DllImportCaller.lib.GetAsyncKeyState7((int)key) != 0;
            }

            public class PhysicalKeysEventArgs : EventArgs
            {
                public PhysicalKeys Key;
            }
            public delegate void HardwareKeyHookEvent(object sender, PhysicalKeys e);
            public static event HardwareKeyHookEvent OnKeyDown;

#pragma warning disable
            private static bool isRunninghook;
            private static new System.Threading.Timer hookThread;
#pragma warning restore
            private static int[] m_listenFor;

            public static void StartHook(PhysicalKeys[] listenFor)
            {
                if (listenFor == null || listenFor.Length == 0)
                {
                    throw new ArgumentException("listenFor; null or 0 length");
                }

                m_listenFor = listenFor.Select(enu => (int)enu).ToArray();

                if (!isRunninghook)
                {
                    isRunninghook = true;

                    hookThread = new System.Threading.Timer((callback) =>
                    {
                        while (true)
                        {
                            for (int i = 0; i < m_listenFor.Length; i++)
                            {
                                if (DllImportCaller.lib.GetAsyncKeyState7(m_listenFor[i]) != 0) //OPTIMIZE, SO FEW CALLS POSSIBLE; Phone.KeyboardHook.IsKeyDown(m_listenFor[i]))
                                {
                                    if (OnKeyDown != null)
                                    {
                                        OnKeyDown(null, (PhysicalKeys)m_listenFor[i]);
                                    }
                                }
                            }

                            System.Threading.Thread.Sleep(500);
                        }

                    }, null, 0, System.Threading.Timeout.Infinite);
                }
            }
            public static void StopHook()
            {
                if (isRunninghook)
                {
                    isRunninghook = false;

                    hookThread.Dispose();

                    hookThread = null;
                }
            }

            [Obsolete("Prototype, not working, still needs some calculations", true)]
            public static bool IsLongHold(PhysicalKeys key)
            {
                if (IsKeyDown(key))
                {
                    longHolder.Push(new LongKeyData { AtTime = DateTime.Now, Key = key });
                }

                var c = longHolder.Count;

                if (c < 10) return false;

                var enumerator = longHolder.GetEnumerator();
                long tick = 0;
                int count = 0;
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.Key != key)
                    {
                        return false;
                    }

                    count++;
                    tick += enumerator.Current.AtTime.Ticks / 1000;
                }

                var v = tick / count;



                var d = (DateTime.Now.Ticks) - v;

                var dt = new DateTime(d);

                return true;//
            }

            public enum PhysicalKeys : int { BackKey = 27, Home = 91, VolumeUp = 117, VolumeDown = 118, Search = 124, Lock = 129, FocusDown = 130, Focus = 131 }

           

            public static KBDI_KEYBOARD GetKeyboardStatus()
            {
                return new KBDI_KEYBOARD(DllImportCaller.lib.VoidCall("coredll", "GetKeyboardStatus"));
            }

            public class KBDI_KEYBOARD
            {
                byte KBDI_KEYBOARD_PRESENT = 0x0001;
                byte KBDI_KEYBOARD_ENABLED = 0x0002;
                byte KBDI_KEYBOARD_ENTER_ESC = 0x0004;
                byte KBDI_KEYBOARD_ALPHA_NUM = 0x0008;

                public int FullMask { get; private set; }

                public KBDI_KEYBOARD(int fullMask)
                {
                    this.FullMask = fullMask;
                }

                /// <summary>
                /// Indicates whether or not the system has keyboard hardware.
                /// </summary>
                public bool KeyboardPresent
                {
                    get
                    {
                        return Is(KBDI_KEYBOARD_PRESENT);
                    }
                }
                /// <summary>
                /// Indicates whether or not the keyboard hardware is enabled.
                /// This bit is assumed to be set but can be modified with EnableHardwareKeyboard.
                /// </summary>
                public bool KeyboardEnabled
                {
                    get
                    {
                        return Is(KBDI_KEYBOARD_ENABLED);
                    }
                }
                /// <summary>
                /// Indicates whether or not the keyboard hardware has ENTER and ESC keys.
                /// </summary>
                public bool KeyboardEnterEsc
                {
                    get
                    {
                        return Is(KBDI_KEYBOARD_ENTER_ESC);
                    }
                }
                /// <summary>
                /// Indicates whether or not the keyboard hardware has alphanumeric keys.
                /// </summary>
                public bool KeyboardAlphaNum
                {
                    get
                    {
                        return Is(KBDI_KEYBOARD_ALPHA_NUM);
                    }
                }
                private bool Is(byte mask)
                {
                    return (FullMask & mask) == mask;
                }
            }

            /// <summary>
            /// The EnableHardwareKeyboard function is useful for applications in which a user can write on the touch screen. When the keyboard is disabled, a user can rest a hand on it without causing spurious keyboard input. Use the disable mode carefully. If an application hangs while the keyboard is disabled, the keyboard is not available to other applications.
            /// </summary>
            /// <param name="bEnable">[in] Boolean value that specifies whether to enable or disable the keyboard. Set it to TRUE to enable the keyboard or FALSE to disable it.</param>
            /// <returns></returns>
            public static bool EnableHardwareKeyboard(bool bEnable)
            {
                return DllImportCaller.lib.IntCall("coredll", "EnableHardwareKeyboard", bEnable ? 1 : 0) == 1;
            }

            /// <summary>
            /// Not sure if those are used in WP7
            /// </summary>
            /// <param name="nVirtKey"></param>
            /// <returns></returns>
            public static short GetKeyState(WM nVirtKey)
            {
                int k = (int)nVirtKey;
                return (short)DllImportCaller.lib.IntCall("coredll", "GetKeyState", k);
            }

            /// <summary>
            /// Virtual Keys, Standard Set
            /// </summary>
            public enum VK : int
            {

                VK_LBUTTON = 0x01,
                VK_RBUTTON = 0x02,
                VK_CANCEL = 0x03,
                VK_MBUTTON = 0x04,

                VK_XBUTTON1 = 0x05,
                VK_XBUTTON2 = 0x06,

                // * 0x07 : unassigned

                VK_BACK = 0x08,
                VK_TAB = 0x09,

                //* 0x0A - 0x0B : reserved

                VK_CLEAR = 0x0C,
                VK_RETURN = 0x0D,

                VK_SHIFT = 0x10,
                VK_CONTROL = 0x11,
                VK_MENU = 0x12,
                VK_PAUSE = 0x13,
                VK_CAPITAL = 0x14,

                VK_KANA = 0x15,
                VK_HANGUL = 0x15,
                VK_JUNJA = 0x17,
                VK_FINAL = 0x18,
                VK_HANJA = 0x19,
                VK_KANJI = 0x19,

                VK_ESCAPE = 0x1B,

                VK_CONVERT = 0x1C,
                VK_NONCONVERT = 0x1D,
                VK_ACCEPT = 0x1E,
                VK_MODECHANGE = 0x1F,

                VK_SPACE = 0x20,
                VK_PRIOR = 0x21,
                VK_NEXT = 0x22,
                VK_END = 0x23,
                VK_HOME = 0x24,
                VK_LEFT = 0x25,
                VK_UP = 0x26,
                VK_RIGHT = 0x27,
                VK_DOWN = 0x28,
                VK_SELECT = 0x29,
                VK_PRINT = 0x2A,
                VK_EXECUTE = 0x2B,
                VK_SNAPSHOT = 0x2C,
                VK_INSERT = 0x2D,
                VK_DELETE = 0x2E,
                VK_HELP = 0x2F,

                //* VK_0 - VK_9 are the same as ASCII '0' - '9' (0x30 - 0x39)
                //* 0x40 : unassigned
                //* VK_A - VK_Z are the same as ASCII 'A' - 'Z' (0x41 - 0x5A)

                VK_0 = 0x30,
                VK_1 = 0x31,
                VK_2 = 0x32,
                VK_3 = 0x33,
                VK_4 = 0x34,
                VK_5 = 0x35,
                VK_6 = 0x36,
                VK_7 = 0x37,
                VK_8 = 0x38,
                VK_9 = 0x39,
                VK_A = 0x41,
                VK_B = 0x42,
                VK_C = 0x43,
                VK_D = 0x44,
                VK_E = 0x45,
                VK_F = 0x46,
                VK_G = 0x47,
                VK_H = 0x48,
                VK_I = 0x49,
                VK_J = 0x4A,
                VK_K = 0x4B,
                VK_L = 0x4C,
                VK_M = 0x4D,
                VK_N = 0x4E,
                VK_O = 0x4F,
                VK_P = 0x50,
                VK_Q = 0x51,
                VK_R = 0x52,
                VK_S = 0x53,
                VK_T = 0x54,
                VK_U = 0x55,
                VK_V = 0x56,
                VK_W = 0x57,
                VK_X = 0x58,
                VK_Y = 0x59,
                VK_Z = 0x5A,

                VK_LWIN = 0x5B,
                VK_RWIN = 0x5C,
                VK_APPS = 0x5D,

                //* 0x5E : reserved

                VK_SLEEP = 0x5F,

                VK_NUMPAD0 = 0x60,
                VK_NUMPAD1 = 0x61,
                VK_NUMPAD2 = 0x62,
                VK_NUMPAD3 = 0x63,
                VK_NUMPAD4 = 0x64,
                VK_NUMPAD5 = 0x65,
                VK_NUMPAD6 = 0x66,
                VK_NUMPAD7 = 0x67,
                VK_NUMPAD8 = 0x68,
                VK_NUMPAD9 = 0x69,
                VK_MULTIPLY = 0x6A,
                VK_ADD = 0x6B,
                VK_SEPARATOR = 0x6C,
                VK_SUBTRACT = 0x6D,
                VK_DECIMAL = 0x6E,
                VK_DIVIDE = 0x6F,
                VK_F1 = 0x70,
                VK_F2 = 0x71,
                VK_F3 = 0x72,
                VK_F4 = 0x73,
                VK_F5 = 0x74,
                VK_F6 = 0x75,
                VK_F7 = 0x76,
                VK_F8 = 0x77,
                VK_F9 = 0x78,
                VK_F10 = 0x79,
                VK_F11 = 0x7A,
                VK_F12 = 0x7B,
                VK_F13 = 0x7C,
                VK_F14 = 0x7D,
                VK_F15 = 0x7E,
                VK_F16 = 0x7F,
                VK_F17 = 0x80,
                VK_F18 = 0x81,
                VK_F19 = 0x82,
                VK_F20 = 0x83,
                VK_F21 = 0x84,
                VK_F22 = 0x85,
                VK_F23 = 0x86,
                VK_F24 = 0x87,

                // * 0x88 - 0x8F : unassigned

                VK_NUMLOCK = 0x90,
                VK_SCROLL = 0x91,

                VK_LSHIFT = 0xA0,
                VK_RSHIFT = 0xA1,

                VK_LCONTROL = 0xA2,
                VK_RCONTROL = 0xA3,





                VK_LMENU = 0xA4,
                VK_RMENU = 0xA5,

                VK_EXTEND_BSLASH = 0xE2,
                VK_OEM_102 = 0xE2,

                VK_PROCESSKEY = 0xE5,

                VK_ATTN = 0xF6,
                VK_CRSEL = 0xF7,
                VK_EXSEL = 0xF8,
                VK_EREOF = 0xF9,
                VK_PLAY = 0xFA,
                VK_ZOOM = 0xFB,
                VK_NONAME = 0xFC,
                VK_PA1 = 0xFD,
                VK_OEM_CLEAR = 0xFE,


                VK_SEMICOLON = 0xBA,
                VK_EQUAL = 0xBB,
                VK_COMMA = 0xBC,
                VK_HYPHEN = 0xBD,
                VK_PERIOD = 0xBE,
                VK_SLASH = 0xBF,
                VK_BACKQUOTE = 0xC0,

                VK_BROWSER_BACK = 0xA6,
                VK_BROWSER_FORWARD = 0xA7,
                VK_BROWSER_REFRESH = 0xA8,
                VK_BROWSER_STOP = 0xA9,
                VK_BROWSER_SEARCH = 0xAA,
                VK_BROWSER_FAVORITES = 0xAB,
                VK_BROWSER_HOME = 0xAC,
                VK_VOLUME_MUTE = 0xAD,
                VK_VOLUME_DOWN = 0xAE,
                VK_VOLUME_UP = 0xAF,
                VK_MEDIA_NEXT_TRACK = 0xB0,
                VK_MEDIA_PREV_TRACK = 0xB1,
                VK_MEDIA_STOP = 0xB2,
                VK_MEDIA_PLAY_PAUSE = 0xB3,
                VK_LAUNCH_MAIL = 0xB4,
                VK_LAUNCH_MEDIA_SELECT = 0xB5,
                VK_LAUNCH_APP1 = 0xB6,
                VK_LAUNCH_APP2 = 0xB7,

                VK_LBRACKET = 0xDB,
                VK_BACKSLASH = 0xDC,
                VK_RBRACKET = 0xDD,
                VK_APOSTROPHE = 0xDE,
                VK_OFF = 0xDF,



                VK_DBE_ALPHANUMERIC = 0x0f0,
                VK_DBE_KATAKANA = 0x0f1,
                VK_DBE_HIRAGANA = 0x0f2,
                VK_DBE_SBCSCHAR = 0x0f3,
                VK_DBE_DBCSCHAR = 0x0f4,
                VK_DBE_ROMAN = 0x0f5,
                VK_DBE_NOROMAN = 0x0f6,
                VK_DBE_ENTERWORDREGISTERMODE = 0x0f7,
                VK_DBE_ENTERIMECONFIGMODE = 0x0f8,
                VK_DBE_FLUSHSTRING = 0x0f9,
                VK_DBE_CODEINPUT = 0x0fa,
                VK_DBE_NOCODEINPUT = 0x0fb,
                VK_DBE_DETERMINESTRING = 0x0fc,
                VK_DBE_ENTERDLGCONVERSIONMODE = 0x0fd
            }
        }
    }
}
