using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace APMCalculator
{
    public class KeyboardHandler
    {
        private static int WH_KEYBOARD_LL = 13;
        private static int WM_KEYDOWN = 256;
        private static int WM_KEYUP = 257;
        
        private static Dictionary<int, bool> keyState = new Dictionary<int, bool>();
        private static List<KeyPressListener> listeners = new List<KeyPressListener>();

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookCallbackDelegate lpfn, IntPtr wParam, uint lParam);
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        public static void Listen()
        {
            string module = Process.GetCurrentProcess().MainModule.ModuleName;
            SetWindowsHookEx(WH_KEYBOARD_LL, HookCallback, GetModuleHandle(module), 0);
        }


        public delegate void KeyPressListener(KeyboardEvent evt);
        public static void OnKeyPress(KeyPressListener l)
        {
            if (listeners.Contains(l))
            {
                return;
            }

            listeners.Add(l);
        }
        
        // Handles keyboard events from the user
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            int keyCode = Marshal.ReadInt32(lParam); // which key was pressed, seems to be ASCII          

            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                HandleKeydown(keyCode);
            } 
            else if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)
            {
                HandleKeyup(keyCode);
            }

            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        private delegate IntPtr HookCallbackDelegate(int nCode, IntPtr wParam, IntPtr lParam);

        // Mark keys as being pressed
        // Handle logging if setting to pressed
        private static void HandleKeydown(int keyCode)
        {
            // Already pressed down? Don't need to do shit
            if (keyState.ContainsKey(keyCode) && keyState[keyCode] == true)
            {
                return;
            }

            keyState[keyCode] = true;
            KeyboardEvent ke = new KeyboardEvent(keyCode, DateTime.UtcNow);
            listeners.ForEach(l => l(ke));
        }

        // Mark keys as being unpressed
        private static void HandleKeyup(int keyCode)
        {
            keyState[keyCode] = false;
        }
    }
}
