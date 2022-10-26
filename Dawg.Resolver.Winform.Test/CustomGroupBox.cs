using System;
using System.Windows.Forms;

namespace Dawg.Solver.Winform
{
    public class CustomGroupBox : GroupBox
    {
        public event EventHandler CustomEvent;
        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                base.OnPaint(e);
            }
            catch (InvalidOperationException )
            {
                if (CustomEvent != null)
                {
                    CustomEvent(this, e);
                }
            }
        }
    }
    public class SuspendDrawingUpdate : IDisposable
    {
        private const int WM_SETREDRAW = 0x000B;
        private readonly Control _control;
        private readonly NativeWindow _window;
        private bool invokeRequired { get; }


        public SuspendDrawingUpdate(Control control)
        {
            _control = control;
            invokeRequired = _control.InvokeRequired && _control.Handle != IntPtr.Zero;
            if (!invokeRequired) return;
            var msgSuspendUpdate = Message.Create(_control.Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);

            _window = NativeWindow.FromHandle(_control.Handle);
            _window.DefWndProc(ref msgSuspendUpdate);
        }

        public void Dispose()
        {
            if (!invokeRequired) return;
            var wparam = new IntPtr(1);  // Create a C "true" boolean as an IntPtr
            var msgResumeUpdate = Message.Create(_control.Handle, WM_SETREDRAW, wparam, IntPtr.Zero);

            _window.DefWndProc(ref msgResumeUpdate);

            _control.Invalidate();
        }
    }
}
