using Dwell_Clicker;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

public class ClickHandler
{
    [DllImport("user32.dll", SetLastError = true)]
    static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

    [StructLayout(LayoutKind.Sequential)]
    struct INPUT
    {
        public uint type;
        public MOUSEKEYBDHARDWAREINPUT Data;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct MOUSEKEYBDHARDWAREINPUT
    {
        [FieldOffset(0)] public MOUSEINPUT Mouse;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public int mouseData;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    const int INPUT_MOUSE = 0;
    const int MOUSEEVENTF_LEFTDOWN = 0x0002;
    const int MOUSEEVENTF_LEFTUP = 0x0004;
    const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
    const int MOUSEEVENTF_RIGHTUP = 0x0010;
    const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
    const int MOUSEEVENTF_MIDDLEUP = 0x0040;

    public event EventHandler ClickPerformed;
    private bool _dragging = false;

    public void PerformClick(Dwell_Clicker.ClickState buttonState)
    {
        if (buttonState == Dwell_Clicker.ClickState.LeftClick || buttonState == Dwell_Clicker.ClickState.RightClick)
        {
            PerformMouseDown(buttonState);
            PerformMouseUp(buttonState);
            ClickPerformed?.Invoke(this, EventArgs.Empty);
        }
        else if (buttonState == Dwell_Clicker.ClickState.DoubleClick)
        {
            PerformDoubleClick();
        }
        else if (buttonState == ClickState.Drag)
        {
            PerformDrag();
        }
        else if (buttonState == ClickState.MiddleClick)
        {
            SendMouseEvent(MOUSEEVENTF_MIDDLEDOWN);
            SendMouseEvent(MOUSEEVENTF_MIDDLEUP);
        }
    }

    private void PerformMouseDown(Dwell_Clicker.ClickState buttonState)
    {
        uint buttonDownFlag = buttonState == Dwell_Clicker.ClickState.LeftClick ? (uint)MOUSEEVENTF_LEFTDOWN : (uint)MOUSEEVENTF_RIGHTDOWN;
        SendMouseEvent(buttonDownFlag);
    }

    private void PerformMouseUp(Dwell_Clicker.ClickState buttonState)
    {
        uint buttonUpFlag = buttonState == Dwell_Clicker.ClickState.LeftClick ? (uint)MOUSEEVENTF_LEFTUP : (uint)MOUSEEVENTF_RIGHTUP;
        SendMouseEvent(buttonUpFlag);
    }

    private void SendMouseEvent(uint buttonFlag)
    {
        INPUT input = new INPUT
        {
            type = INPUT_MOUSE,
            Data = new MOUSEKEYBDHARDWAREINPUT
            {
                Mouse = new MOUSEINPUT
                {
                    dx = 0,
                    dy = 0,
                    mouseData = 0,
                    dwFlags = buttonFlag,
                    time = 0,
                    dwExtraInfo = IntPtr.Zero
                }
            }
        };
        SendInput(1, ref input, Marshal.SizeOf(typeof(INPUT)));
    }

    public bool IsDragging()
    {
        return _dragging;
    }

    private void PerformDoubleClick()
    {
        PerformMouseDown(ClickState.LeftClick);
        PerformMouseUp(ClickState.LeftClick);
        Thread.Sleep(50); // Small delay between clicks for better compatibility
        PerformMouseDown(ClickState.LeftClick);
        PerformMouseUp(ClickState.LeftClick);
        ClickPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void PerformDrag()
    {
        if (_dragging)
        {
            PerformMouseUp(ClickState.LeftClick);
            _dragging = false;
        }
        else
        {
            PerformMouseDown(ClickState.LeftClick);
            _dragging = true;
        }
        ClickPerformed?.Invoke(this, EventArgs.Empty);
    }

    private bool IsCursorOverButton()
    {
        Form1 mainForm = (Form1)Application.OpenForms["Form1"];
        return mainForm.IsCursorOverClickMethodButton();
    }
}
