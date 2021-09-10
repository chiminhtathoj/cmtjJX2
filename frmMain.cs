using AutoClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cmtjJX2
{
    public partial class frmMain : Form
    {

        private Dictionary<IntPtr, AutoClientBS> _clients = new Dictionary<IntPtr, AutoClientBS>();

        private List<IntPtr> _clientHwnds = new List<IntPtr>();

        private IntPtr currentSelectedChar;

        //biến 
        string log = string.Empty;

        #region tạo form mới
        private frmMoveTo frmmoveto = new frmMoveTo();
        #endregion
        #region Windows structure definitions

        /// <summary>
        /// The POINT structure defines the x- and y- coordinates of a point. 
        /// </summary>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/gdi/rectangl_0tiq.asp
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        private class POINT
        {
            /// <summary>
            /// Specifies the x-coordinate of the point. 
            /// </summary>
            public int x;
            /// <summary>
            /// Specifies the y-coordinate of the point. 
            /// </summary>
            public int y;
        }

        /// <summary>
        /// The MOUSEHOOKSTRUCT structure contains information about a mouse event passed to a WH_MOUSE hook procedure, MouseProc. 
        /// </summary>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookstructures/cwpstruct.asp
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        private class MouseHookStruct
        {
            /// <summary>
            /// Specifies a POINT structure that contains the x- and y-coordinates of the cursor, in screen coordinates. 
            /// </summary>
            public POINT pt;
            /// <summary>
            /// Handle to the window that will receive the mouse message corresponding to the mouse event. 
            /// </summary>
            public int hwnd;
            /// <summary>
            /// Specifies the hit-test value. For a list of hit-test values, see the description of the WM_NCHITTEST message. 
            /// </summary>
            public int wHitTestCode;
            /// <summary>
            /// Specifies extra information associated with the message. 
            /// </summary>
            public int dwExtraInfo;
        }

        /// <summary>
        /// The MSLLHOOKSTRUCT structure contains information about a low-level keyboard input event. 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private class MouseLLHookStruct
        {
            /// <summary>
            /// Specifies a POINT structure that contains the x- and y-coordinates of the cursor, in screen coordinates. 
            /// </summary>
            public POINT pt;
            /// <summary>
            /// If the message is WM_MOUSEWHEEL, the high-order word of this member is the wheel delta. 
            /// The low-order word is reserved. A positive value indicates that the wheel was rotated forward, 
            /// away from the user; a negative value indicates that the wheel was rotated backward, toward the user. 
            /// One wheel click is defined as WHEEL_DELTA, which is 120. 
            ///If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP,
            /// or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released, 
            /// and the low-order word is reserved. This value can be one or more of the following values. Otherwise, mouseData is not used. 
            ///XBUTTON1
            ///The first X button was pressed or released.
            ///XBUTTON2
            ///The second X button was pressed or released.
            /// </summary>
            public int mouseData;
            /// <summary>
            /// Specifies the event-injected flag. An application can use the following value to test the mouse flags. Value Purpose 
            ///LLMHF_INJECTED Test the event-injected flag.  
            ///0
            ///Specifies whether the event was injected. The value is 1 if the event was injected; otherwise, it is 0.
            ///1-15
            ///Reserved.
            /// </summary>
            public int flags;
            /// <summary>
            /// Specifies the time stamp for this message.
            /// </summary>
            public int time;
            /// <summary>
            /// Specifies extra information associated with the message. 
            /// </summary>
            public int dwExtraInfo;
        }


        /// <summary>
        /// The KBDLLHOOKSTRUCT structure contains information about a low-level keyboard input event. 
        /// </summary>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookstructures/cwpstruct.asp
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        private class KeyboardHookStruct
        {
            /// <summary>
            /// Specifies a virtual-key code. The code must be a value in the range 1 to 254. 
            /// </summary>
            public int vkCode;
            /// <summary>
            /// Specifies a hardware scan code for the key. 
            /// </summary>
            public int scanCode;
            /// <summary>
            /// Specifies the extended-key flag, event-injected flag, context code, and transition-state flag.
            /// </summary>
            public int flags;
            /// <summary>
            /// Specifies the time stamp for this message.
            /// </summary>
            public int time;
            /// <summary>
            /// Specifies extra information associated with the message. 
            /// </summary>
            public int dwExtraInfo;
        }
        #endregion
        #region Windows function imports
        /// <summary>
        /// The SetWindowsHookEx function installs an application-defined hook procedure into a hook chain. 
        /// You would install a hook procedure to monitor the system for certain types of events. These events 
        /// are associated either with a specific thread or with all threads in the same desktop as the calling thread. 
        /// </summary>
        /// <param name="idHook">
        /// [in] Specifies the type of hook procedure to be installed. This parameter can be one of the following values.
        /// </param>
        /// <param name="lpfn">
        /// [in] Pointer to the hook procedure. If the dwThreadId parameter is zero or specifies the identifier of a 
        /// thread created by a different process, the lpfn parameter must point to a hook procedure in a dynamic-link 
        /// library (DLL). Otherwise, lpfn can point to a hook procedure in the code associated with the current process.
        /// </param>
        /// <param name="hMod">
        /// [in] Handle to the DLL containing the hook procedure pointed to by the lpfn parameter. 
        /// The hMod parameter must be set to NULL if the dwThreadId parameter specifies a thread created by 
        /// the current process and if the hook procedure is within the code associated with the current process. 
        /// </param>
        /// <param name="dwThreadId">
        /// [in] Specifies the identifier of the thread with which the hook procedure is to be associated. 
        /// If this parameter is zero, the hook procedure is associated with all existing threads running in the 
        /// same desktop as the calling thread. 
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is the handle to the hook procedure.
        /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookfunctions/setwindowshookex.asp
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto,
           CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int SetWindowsHookEx(
            int idHook,
            HookProc lpfn,
            IntPtr hMod,
            int dwThreadId);

        /// <summary>
        /// The UnhookWindowsHookEx function removes a hook procedure installed in a hook chain by the SetWindowsHookEx function. 
        /// </summary>
        /// <param name="idHook">
        /// [in] Handle to the hook to be removed. This parameter is a hook handle obtained by a previous call to SetWindowsHookEx. 
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookfunctions/setwindowshookex.asp
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int UnhookWindowsHookEx(int idHook);

        /// <summary>
        /// The CallNextHookEx function passes the hook information to the next hook procedure in the current hook chain. 
        /// A hook procedure can call this function either before or after processing the hook information. 
        /// </summary>
        /// <param name="idHook">Ignored.</param>
        /// <param name="nCode">
        /// [in] Specifies the hook code passed to the current hook procedure. 
        /// The next hook procedure uses this code to determine how to process the hook information.
        /// </param>
        /// <param name="wParam">
        /// [in] Specifies the wParam value passed to the current hook procedure. 
        /// The meaning of this parameter depends on the type of hook associated with the current hook chain. 
        /// </param>
        /// <param name="lParam">
        /// [in] Specifies the lParam value passed to the current hook procedure. 
        /// The meaning of this parameter depends on the type of hook associated with the current hook chain. 
        /// </param>
        /// <returns>
        /// This value is returned by the next hook procedure in the chain. 
        /// The current hook procedure must also return this value. The meaning of the return value depends on the hook type. 
        /// For more information, see the descriptions of the individual hook procedures.
        /// </returns>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookfunctions/setwindowshookex.asp
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto,
             CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(
            int idHook,
            int nCode,
            int wParam,
            IntPtr lParam);

        /// <summary>
        /// The CallWndProc hook procedure is an application-defined or library-defined callback 
        /// function used with the SetWindowsHookEx function. The HOOKPROC type defines a pointer 
        /// to this callback function. CallWndProc is a placeholder for the application-defined 
        /// or library-defined function name.
        /// </summary>
        /// <param name="nCode">
        /// [in] Specifies whether the hook procedure must process the message. 
        /// If nCode is HC_ACTION, the hook procedure must process the message. 
        /// If nCode is less than zero, the hook procedure must pass the message to the 
        /// CallNextHookEx function without further processing and must return the 
        /// value returned by CallNextHookEx.
        /// </param>
        /// <param name="wParam">
        /// [in] Specifies whether the message was sent by the current thread. 
        /// If the message was sent by the current thread, it is nonzero; otherwise, it is zero. 
        /// </param>
        /// <param name="lParam">
        /// [in] Pointer to a CWPSTRUCT structure that contains details about the message. 
        /// </param>
        /// <returns>
        /// If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx. 
        /// If nCode is greater than or equal to zero, it is highly recommended that you call CallNextHookEx 
        /// and return the value it returns; otherwise, other applications that have installed WH_CALLWNDPROC 
        /// hooks will not receive hook notifications and may behave incorrectly as a result. If the hook 
        /// procedure does not call CallNextHookEx, the return value should be zero. 
        /// </returns>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookfunctions/callwndproc.asp
        /// </remarks>
        private delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        /// <summary>
        /// The ToAscii function translates the specified virtual-key code and keyboard 
        /// state to the corresponding character or characters. The function translates the code 
        /// using the input language and physical keyboard layout identified by the keyboard layout handle.
        /// </summary>
        /// <param name="uVirtKey">
        /// [in] Specifies the virtual-key code to be translated. 
        /// </param>
        /// <param name="uScanCode">
        /// [in] Specifies the hardware scan code of the key to be translated. 
        /// The high-order bit of this value is set if the key is up (not pressed). 
        /// </param>
        /// <param name="lpbKeyState">
        /// [in] Pointer to a 256-byte array that contains the current keyboard state. 
        /// Each element (byte) in the array contains the state of one key. 
        /// If the high-order bit of a byte is set, the key is down (pressed). 
        /// The low bit, if set, indicates that the key is toggled on. In this function, 
        /// only the toggle bit of the CAPS LOCK key is relevant. The toggle state 
        /// of the NUM LOCK and SCROLL LOCK keys is ignored.
        /// </param>
        /// <param name="lpwTransKey">
        /// [out] Pointer to the buffer that receives the translated character or characters. 
        /// </param>
        /// <param name="fuState">
        /// [in] Specifies whether a menu is active. This parameter must be 1 if a menu is active, or 0 otherwise. 
        /// </param>
        /// <returns>
        /// If the specified key is a dead key, the return value is negative. Otherwise, it is one of the following values. 
        /// Value Meaning 
        /// 0 The specified virtual key has no translation for the current state of the keyboard. 
        /// 1 One character was copied to the buffer. 
        /// 2 Two characters were copied to the buffer. This usually happens when a dead-key character 
        /// (accent or diacritic) stored in the keyboard layout cannot be composed with the specified 
        /// virtual key to form a single character. 
        /// </returns>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/userinput/keyboardinput/keyboardinputreference/keyboardinputfunctions/toascii.asp
        /// </remarks>
        [DllImport("user32")]
        private static extern int ToAscii(
            int uVirtKey,
            int uScanCode,
            byte[] lpbKeyState,
            byte[] lpwTransKey,
            int fuState);

        /// <summary>
        /// The GetKeyboardState function copies the status of the 256 virtual keys to the 
        /// specified buffer. 
        /// </summary>
        /// <param name="pbKeyState">
        /// [in] Pointer to a 256-byte array that contains keyboard key states. 
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError. 
        /// </returns>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/userinput/keyboardinput/keyboardinputreference/keyboardinputfunctions/toascii.asp
        /// </remarks>
        [DllImport("user32")]
        private static extern int GetKeyboardState(byte[] pbKeyState);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern short GetKeyState(int vKey);

        #endregion
        #region Windows constants

        /// <summary>
        /// Windows NT/2000/XP: Installs a hook procedure that monitors low-level keyboard  input events.
        /// </summary>
        private const int WH_KEYBOARD_LL = 13;
        /// <summary>
        /// The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem 
        /// key is pressed. A nonsystem key is a key that is pressed when the ALT key is not pressed.
        /// </summary>
        private const int WM_KEYDOWN = 0x100;
        /// <summary>
        /// The WM_KEYUP message is posted to the window with the keyboard focus when a nonsystem 
        /// key is released. A nonsystem key is a key that is pressed when the ALT key is not pressed, 
        /// or a keyboard key that is pressed when a window has the keyboard focus.
        /// </summary>
        private const int WM_KEYUP = 0x101;
        /// <summary>
        /// The WM_SYSKEYDOWN message is posted to the window with the keyboard focus when the user 
        /// presses the F10 key (which activates the menu bar) or holds down the ALT key and then 
        /// presses another key. It also occurs when no window currently has the keyboard focus; 
        /// in this case, the WM_SYSKEYDOWN message is sent to the active window. The window that 
        /// receives the message can distinguish between these two contexts by checking the context 
        /// code in the lParam parameter. 
        /// </summary>
        private const int WM_SYSKEYDOWN = 0x104;
        /// <summary>
        /// The WM_SYSKEYUP message is posted to the window with the keyboard focus when the user 
        /// releases a key that was pressed while the ALT key was held down. It also occurs when no 
        /// window currently has the keyboard focus; in this case, the WM_SYSKEYUP message is sent 
        /// to the active window. The window that receives the message can distinguish between 
        /// these two contexts by checking the context code in the lParam parameter. 
        /// </summary>
        private const int WM_SYSKEYUP = 0x105;

        private const byte VK_SHIFT = 0x10;
        private const byte VK_CONTROL = 0x11;
        private const byte VK_MENU = 0x12;
        private const byte VK_CAPITAL = 0x14;
        private const byte VK_NUMLOCK = 0x90;

        #endregion
        #region Chạy chương trình Hook Keyboard & Auto Thay do

        /// <summary>
        /// Stores the handle to the keyboard hook procedure.
        /// </summary>
        private int hKeyboardHook = 0;


        /// <summary>
        /// Declare KeyboardHookProcedure as HookProc type.
        /// </summary>
        private static HookProc KeyboardHookProcedure;


        /// <summary>
        /// Installs both mouse and keyboard hooks and starts rasing events
        /// </summary>
        /// <exception cref="Win32Exception">Any windows problem.</exception>
        public void Start()
        {
            this.Start(true);
        }

        /// <summary>
        /// Installs both or one of mouse and/or keyboard hooks and starts rasing events
        /// </summary>
        /// <param name="InstallKeyboardHook"><b>true</b> if keyboard events must be monitored</param>
        /// <exception cref="Win32Exception">Any windows problem.</exception>
        public void Start(bool InstallKeyboardHook)
        {
            // install Keyboard hook only if it is not installed and must be installed
            if (hKeyboardHook == 0 && InstallKeyboardHook)
            {
                // Create an instance of HookProc.
                KeyboardHookProcedure = new HookProc(KeyboardHookProc);
                //install hook
                hKeyboardHook = SetWindowsHookEx(
                    WH_KEYBOARD_LL,
                    KeyboardHookProcedure,
                    Marshal.GetHINSTANCE(
                    Assembly.GetExecutingAssembly().GetModules()[0]),
                    0);
                //If SetWindowsHookEx fails.
                if (hKeyboardHook == 0)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();
                    //do cleanup
                    Stop(true, false);
                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }

        /// <summary>
        /// Stops monitoring both mouse and keyboard events and rasing events.
        /// </summary>
        /// <exception cref="Win32Exception">Any windows problem.</exception>
        public void Stop()
        {
            this.Stop(true, true);
        }

        /// <summary>
        /// Stops monitoring both or one of mouse and/or keyboard events and rasing events.
        /// </summary>
        /// <param name="UninstallMouseHook"><b>true</b> if mouse hook must be uninstalled</param>
        /// <param name="UninstallKeyboardHook"><b>true</b> if keyboard hook must be uninstalled</param>
        /// <param name="ThrowExceptions"><b>true</b> if exceptions which occured during uninstalling must be thrown</param>
        /// <exception cref="Win32Exception">Any windows problem.</exception>
        public void Stop(bool UninstallKeyboardHook, bool ThrowExceptions)
        {
            //if keyboard hook set and must be uninstalled
            if (hKeyboardHook != 0 && UninstallKeyboardHook)
            {
                //uninstall hook
                int retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
                //reset invalid handle
                hKeyboardHook = 0;
                //if failed and exception must be thrown
                if (retKeyboard == 0 && ThrowExceptions)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();
                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }

        /// <summary>
        /// A callback function which will be called every time a keyboard activity detected.
        /// </summary>
        /// <param name="nCode">
        /// [in] Specifies whether the hook procedure must process the message. 
        /// If nCode is HC_ACTION, the hook procedure must process the message. 
        /// If nCode is less than zero, the hook procedure must pass the message to the 
        /// CallNextHookEx function without further processing and must return the 
        /// value returned by CallNextHookEx.
        /// </param>
        /// <param name="wParam">
        /// [in] Specifies whether the message was sent by the current thread. 
        /// If the message was sent by the current thread, it is nonzero; otherwise, it is zero. 
        /// </param>
        /// <param name="lParam">
        /// [in] Pointer to a CWPSTRUCT structure that contains details about the message. 
        /// </param>
        /// <returns>
        /// If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx. 
        /// If nCode is greater than or equal to zero, it is highly recommended that you call CallNextHookEx 
        /// and return the value it returns; otherwise, other applications that have installed WH_CALLWNDPROC 
        /// hooks will not receive hook notifications and may behave incorrectly as a result. If the hook 
        /// procedure does not call CallNextHookEx, the return value should be zero. 
        /// </returns>

        private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            //indicates if any of underlaing events set e.Handled flag
            bool handled = false;
            //it was ok and someone listens to events
            if ((nCode >= 0))
            {
                //read structure KeyboardHookStruct at lParam
                KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                //raise KeyDown
                if ((wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                    keyData |= ((GetKeyState(VK_SHIFT) & 0x80) == 0x80 ? Keys.Shift : Keys.None);
                    keyData |= ((GetKeyState(VK_CONTROL) & 0x80) == 0x80 ? Keys.Control : Keys.None);
                    keyData |= ((GetKeyState(VK_MENU) & 0x80) == 0x80 ? Keys.Menu : Keys.None);
                    Console.WriteLine(keyData);
                    KeyEventArgs e = new KeyEventArgs(keyData);

                    foreach (var autoClient in _clients.Values)
                    {
                        if (!autoClient.isInjected)
                            autoClient.Inject();

                        /////////////////////////////////////////Auto combo TLQ /////////////////////////////////////////////
                        if (e.KeyData.ToString() == autoClient.phimdung.ToString() && autoClient.ComboPKTLQ == true)
                        {
                            log = e.KeyData.ToString();

                            return (Int32)1;
                        }
                        //
                        else if (e.KeyData.ToString() == autoClient.phimvac.ToString() && autoClient.ComboPKTLQ == true)
                        {
                            log = e.KeyData.ToString();

                            return (Int32)1;
                        }
                        //
                        else if (e.KeyData.ToString() == autoClient.phimvacdt.ToString() && autoClient.ComboPKTLQ == true)
                        {

                            log = e.KeyData.ToString();

                            return (Int32)1;
                        }
                        //
                        else if (e.KeyData.ToString() == autoClient.phimvaccdt.ToString() && autoClient.ComboPKTLQ == true)
                        {

                            log = e.KeyData.ToString();

                            return (Int32)1;
                        }
                        //
                        else if (e.KeyData.ToString() == autoClient.phimdt1o.ToString() && autoClient.ComboPKTLQ == true)
                        {
                            log = e.KeyData.ToString();

                            return (Int32)1;
                        }


                        /////////////////////////////////////////Auto Thay đồ /////////////////////////////////////////////
                        if (e.KeyData.ToString() == autoClient.phimthaydo1.ToString() && autoClient.ckthaydobo1 == true && autoClient.ckthaydo == true)
                        {
                            for (int i = 0; i < autoClient.Listthaydo1.Count; i++)
                            {
                                if (autoClient.Listthaydo1[i] != "Null" && !autoClient.Listthaydo1[i].Contains("/5/0"))
                                {
                                    autoClient.FindAndUseItemID(autoClient.Listthaydo1[i]);
                                }
                                else if (autoClient.Listthaydo1[i] != "Null" && autoClient.Listthaydo1[i].Contains("/5/0"))
                                {
                                    autoClient.Thayngoc2(autoClient.Listthaydo1[i]);
                                }
                            }

                            return (Int32)1;
                        }

                        //
                        else if (e.KeyData.ToString() == autoClient.phimthaydo2.ToString() && autoClient.ckthaydobo2 == true && autoClient.ckthaydo == true)
                        {
                            for (int i = 0; i < autoClient.Listthaydo2.Count; i++)
                            {
                                if (autoClient.Listthaydo2[i] != "Null" && !autoClient.Listthaydo2[i].Contains("/5/0"))
                                {
                                    autoClient.FindAndUseItemID(autoClient.Listthaydo2[i]);
                                }
                                else if (autoClient.Listthaydo2[i] != "Null" && autoClient.Listthaydo2[i].Contains("/5/0"))
                                {
                                    autoClient.Thayngoc2(autoClient.Listthaydo2[i]);
                                }
                            }

                            return (Int32)1;

                        }

                        //
                        else if (e.KeyData.ToString() == autoClient.phimthaydo3.ToString() && autoClient.ckthaydobo3 == true && autoClient.ckthaydo == true)
                        {
                            for (int i = 0; i < autoClient.Listthaydo3.Count; i++)
                            {
                                if (autoClient.Listthaydo3[i] != "Null" && !autoClient.Listthaydo3[i].Contains("/5/0"))
                                {
                                    autoClient.FindAndUseItemID(autoClient.Listthaydo3[i]);
                                }
                                else if (autoClient.Listthaydo3[i] != "Null" && autoClient.Listthaydo3[i].Contains("/5/0"))
                                {
                                    autoClient.Thayngoc2(autoClient.Listthaydo3[i]);
                                }
                            }

                            return (Int32)1;
                        }

                        else if (e.KeyData.ToString() == autoClient.phimthaydo4.ToString() && autoClient.ckthaydobo4 == true && autoClient.ckthaydo == true)
                        {
                            for (int i = 0; i < autoClient.Listthaydo4.Count; i++)
                            {
                                if (autoClient.Listthaydo4[i] != "Null" && !autoClient.Listthaydo4[i].Contains("/5/0"))
                                {
                                    autoClient.FindAndUseItemID(autoClient.Listthaydo4[i]);
                                }
                                else if (autoClient.Listthaydo4[i] != "Null" && autoClient.Listthaydo4[i].Contains("/5/0"))
                                {
                                    autoClient.Thayngoc2(autoClient.Listthaydo4[i]);
                                }
                            }

                            return (Int32)1;
                        }
                    }

                }

            }

            //if event handled in application do not handoff to other listeners
            if (handled)
                return (Int32)1;
            else
                return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
        }
        #endregion
        public frmMain()
        {
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            //khi load form lên ở góc trên cùng bên phải
            base.Top = 0;
            base.Left = Screen.PrimaryScreen.WorkingArea.Width - base.Width;

            //Tạo thread mới chạy hàm MoveTo
            Thread thrdMoveTo = new Thread(new ThreadStart(moveTo));
            thrdMoveTo.Priority = ThreadPriority.Normal;
            thrdMoveTo.IsBackground = true;
            thrdMoveTo.Start();

        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop(); // dừng Hook Keyboard


            // Đóng Hook Game
            try
            {
                foreach (var autoClient in _clients.Values)
                {
                    if (autoClient.isInjected)
                        autoClient.DeInject();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Dong Form: " + ex.Message);
                return;
            }
        }
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Giải phóng bộ nhớ khi đóng form
            this.Dispose();
            GC.Collect();
        }
        // Sự kiện chọn Handle nhân vật
        public AutoClientBS CurrentClient
        {
            get
            {
                if (currentSelectedChar.ToInt32() == 0)
                    return null;
                if (!_clients.ContainsKey(currentSelectedChar))
                    return null;
                else
                    return _clients[currentSelectedChar];
            }
        }

        //Lưu lại các handle vào Client
        private bool SearchForGameWindows(IntPtr hwnd, IntPtr lParam)
        {
            StringBuilder title = new StringBuilder();
            WinAPI.GetWindowText(hwnd, title, title.Capacity);
            if (title.ToString().Contains("JX Online"))
            {
                _clientHwnds.Add(hwnd);
            }
            return true;
        }

        // cập nhật list view player real time

        private void TmrCheckChars_Tick(object sender, EventArgs e)
        {
            lsvPlayer.Invoke(new MethodInvoker(() =>
            {
                _clientHwnds.Clear();
                WinAPI.EnumWindows(SearchForGameWindows, new IntPtr(0));


                var keys = _clients.Keys.ToList();

                //////////////////////////////ListView//////////////////////
                // duyệt qua tất cả phần tử trong _clients, nếu mà trong _clientHwnds chứa key đó thì xóa
                // ngược lại thì xóa trong _clients
                // xóa trong listview
                for (int i = 0; i < keys.Count; i++)
                {
                    var clientKey = keys[i];

                    if (_clientHwnds.Contains(clientKey))
                    {
                        _clientHwnds.Remove(clientKey);
                    }

                    else
                    {
                        _clients.Remove(clientKey);

                        var add = clientKey.ToInt32().ToString();

                        foreach (ListViewItem item in lsvPlayer.Items)
                        {
                            if (item.ToString().Substring(0, add.Length) == add)
                            {
                                lsvPlayer.Items.Remove(item);
                            }

                        }
                    }
                }

                // Cập nhật Client

                foreach (var clientHwnd in _clientHwnds)
                {
                    var client = new AutoClientBS();
                    client.Attach(clientHwnd);

                    _clients.Add(clientHwnd, client);

                    ListViewItem row = new ListViewItem(clientHwnd.ToInt32().ToString(), 0);

                    row.SubItems.Add(client.CurrentPlayer.EntityNameUnicode);
                    row.SubItems.Add(client.CurrentPlayer.HP.ToString());
                    row.SubItems.Add(client.CurrentPlayer.EntityMapString);
                    row.SubItems.Add(client.CurrentPlayer.isOnline);

                    lsvPlayer.Items.Add(row);
                }

                ///////////////////Update ListView////////////////
                var _rowNeedToBeRemoved = new List<ListViewItem>();

                foreach (ListViewItem row in lsvPlayer.Items)
                {
                    var key = new IntPtr(Convert.ToInt32(row.Text));
                    if (_clients.ContainsKey(key))
                    {
                        var client = _clients[key];

                        row.SubItems[1].Text = client.CurrentPlayer.EntityNameUnicode;
                        row.SubItems[2].Text = client.CurrentPlayer.HP.ToString();
                        row.SubItems[3].Text = client.CurrentPlayer.EntityMapString;
                        row.SubItems[4].Text = client.CurrentPlayer.isOnline;
                    }
                    else
                    {
                        _rowNeedToBeRemoved.Add(row);
                    }

                }

                if (_rowNeedToBeRemoved.Count > 0)
                {
                    foreach (ListViewItem item in _rowNeedToBeRemoved)
                    {
                        lsvPlayer.Items.Remove(item);
                    }
                }

                ///////////////////Update Checkbox////////////////

                if (lsvPlayer.Items.Count < 1)
                {
                    cbFuncMoveTo.Checked = false;
                }

            }));
        }


        // Thay đổi chiều rộng Header cột Listview

        private void lsvPlayer_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.GreenYellow, e.Bounds);
            e.DrawText();
        }

        //các thủ tục làm trước khi đóng form




        // Click vào check box chọn acout trên listviews

        private void lsvPlayer_ItemCheck(object sender, ItemCheckEventArgs e)

        {

            lsvPlayer.Invoke(new MethodInvoker(() =>
            {

                lsvPlayer.Items[e.Index].Selected = true;
                CurrentClient.ischecked = (e.CurrentValue == CheckState.Unchecked) ? true : false;
            }));
        }


        // click vào list view player
        private void lsvPlayer_Click(object sender, EventArgs e)
        {
            lsvPlayer.Invoke(new MethodInvoker(() =>
            {
              if(CurrentClient != null)
                {
                    WinAPI.SetForegroundWindow((IntPtr)CurrentClient.WindowHwnd); //focus vao window
                    WinAPI.ShowWindow(CurrentClient.WindowHwnd.ToInt32(), 1);//param2 1 = show  //chi show khi an,k tu focus vao window
                }
            }));
        }


        //hàm Cập nhật thông tin account từ file lưu trong máy
        private void updateInfoAccount()
        {
            lsvPlayer.Invoke(new MethodInvoker(() =>
            {

                if (lsvPlayer.SelectedItems.Count == 0)
                {
                    currentSelectedChar = new IntPtr(0);
                    return;
                }

                var st = lsvPlayer.SelectedItems[0].Text;

                currentSelectedChar = new IntPtr(Convert.ToInt32(st));

                string entityNameUnicode = CurrentClient.CurrentPlayer.EntityNameUnicode;

                string text = "//UserData//" + entityNameUnicode.Replace("*", ".") + ".ini";

                // Checkbox

                CurrentClient.cbMoveTo = bool.Parse(WinAPI.Docfile(text, "MoveTo", "MoveTo.CheckActive", "false"));


                // load giá trị vào Project
                cbFuncMoveTo.Checked = CurrentClient.cbMoveTo;

                frmmoveto.CurrentClient = CurrentClient;

            }));
        }

        // cập nhật thông tin account từ file trong máy khi select trên list view
        private void lsvPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateInfoAccount();
        }

        // nhã chuột trên lsv
        private void lsvPlayer_MouseUp(object sender, MouseEventArgs e)
        {
            lsvPlayer.Invoke(new MethodInvoker(() =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right && CurrentClient != null)
                    WinAPI.ShowWindow(CurrentClient.WindowHwnd.ToInt32(), 0); //param2 0 = hide
            }));
        }



        //thủ tục click vào các button chức năng trong auto
        private void btnFuncMoveTo_Click(object sender, EventArgs e)
        {
            try
            {
                frmmoveto.Refresh();
                frmmoveto.CurrentClient = CurrentClient;
                frmmoveto.StartPosition = FormStartPosition.Manual;
                frmmoveto.Left = Convert.ToInt32(Left - (base.Width + 100));
                frmmoveto.Top = Convert.ToInt32(this.Top);
                frmmoveto.Show();
            }
            catch
            {
                frmmoveto = new frmMoveTo();
                frmmoveto.CurrentClient = CurrentClient;
                frmmoveto.StartPosition = FormStartPosition.Manual;
                frmmoveto.Left = Convert.ToInt32(Left - (base.Width + 100));
                frmmoveto.Top = Convert.ToInt32(this.Top);
                frmmoveto.Show();
            }
        }



        //thủ tục lưu các check box hoạt động

        #region checkbox hoạt động

        //checkbox move to
        private void cbFuncMoveTo_CheckedChanged(object sender, EventArgs e)
        {
            if (CurrentClient != null)
            {
                CurrentClient.cbMoveTo = cbFuncMoveTo.Checked;
                string entityNameUnicode = CurrentClient.CurrentPlayer.EntityNameUnicode;
                string text = "//UserData//" + entityNameUnicode.Replace("*", ".") + ".ini";
                WinAPI.Ghifile(text, "MoveTo", "MoveTo.CheckActive", CurrentClient.cbMoveTo.ToString());
            }
        }

        #endregion

        //thủ tục kiểm tra lựa chọn nhân vật
        #region kiểm tra lựa chọn nv

        private void cbFuncMoveTo_Click(object sender, EventArgs e)
        {
            if (CurrentClient == null && lsvPlayer.Items.Count >= 1)
            {
                cbFuncMoveTo.Checked = false;
                MessageBox.Show("Bạn chưa chọn nhân vật!", "Cảnh báo!");
            }
            else if (CurrentClient == null && lsvPlayer.Items.Count < 1)
            {
                cbFuncMoveTo.Checked = false;
                MessageBox.Show("Chưa đăng nhập nhân vật!", "Cảnh báo!");
            }
        }
        #endregion

        #region các hàm xử lý các chức năng chính auto
        private void moveTo()
        {
            while (true)
            {
                   
                Thread.Sleep(500);
                try
                {
                    if (_clients.Count < 1)
                        return;

                    var _clientsMoveTo = new Dictionary<IntPtr, AutoClientBS>(_clients);

                    foreach (var autoClient in _clientsMoveTo.Values)
                    {
                        if (autoClient.cbMoveTo == true && autoClient.ischecked == true && autoClient.CheckRun == true)
                        {
                            if (!autoClient.cbMoveTo || !autoClient.ischecked)
                                return;

                            if (!autoClient.isInjected)
                                autoClient.Inject();
                            if (autoClient.toaDoX != 0 && autoClient.toaDoY != 0 && autoClient.idMap != 0)
                            {
                                autoClient.MoveTo(autoClient.toaDoX, autoClient.toaDoY, autoClient.idMap);
                            }
                            //Thread.Sleep(1000);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Auto moveto: " + ex.Message);
                    return;
                }

            }
            #endregion

        }

        private void btnFuncReward3h_Click(object sender, EventArgs e)
        {
            CurrentClient.shortMove(200, 175, 180);
        }
    }
}