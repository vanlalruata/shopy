using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shopy.Controls
{
    public class MaterialForm : Form, IMaterialControl
    {
        public const int WM_NCLBUTTONDOWN = 161;

        public const int HT_CAPTION = 2;

        public const int WM_MOUSEMOVE = 512;

        public const int WM_LBUTTONDOWN = 513;

        public const int WM_LBUTTONUP = 514;

        public const int WM_LBUTTONDBLCLK = 515;

        public const int WM_RBUTTONDOWN = 516;

        private const int HTBOTTOMLEFT = 16;

        private const int HTBOTTOMRIGHT = 17;

        private const int HTLEFT = 10;

        private const int HTRIGHT = 11;

        private const int HTBOTTOM = 15;

        private const int HTTOP = 12;

        private const int HTTOPLEFT = 13;

        private const int HTTOPRIGHT = 14;

        private const int BORDER_WIDTH = 7;

        private MaterialForm.ResizeDirection _resizeDir;

        private MaterialForm.ButtonState _buttonState = MaterialForm.ButtonState.None;

        private const int WMSZ_TOP = 3;

        private const int WMSZ_TOPLEFT = 4;

        private const int WMSZ_TOPRIGHT = 5;

        private const int WMSZ_LEFT = 1;

        private const int WMSZ_RIGHT = 2;

        private const int WMSZ_BOTTOM = 6;

        private const int WMSZ_BOTTOMLEFT = 7;

        private const int WMSZ_BOTTOMRIGHT = 8;

        private readonly Dictionary<int, int> _resizingLocationsToCmd = new Dictionary<int, int>()
        {
            { 12, 3 },
            { 13, 4 },
            { 14, 5 },
            { 10, 1 },
            { 11, 2 },
            { 15, 6 },
            { 16, 7 },
            { 17, 8 }
        };

        private const int STATUS_BAR_BUTTON_WIDTH = 24;

        private const int STATUS_BAR_HEIGHT = 24;

        private const int ACTION_BAR_HEIGHT = 40;

        private const uint TPM_LEFTALIGN = 0;

        private const uint TPM_RETURNCMD = 256;

        private const int WM_SYSCOMMAND = 274;

        private const int WS_MINIMIZEBOX = 131072;

        private const int WS_SYSMENU = 524288;

        private const int MONITOR_DEFAULTTONEAREST = 2;

        private readonly Cursor[] _resizeCursors = new Cursor[] { Cursors.SizeNESW, Cursors.SizeWE, Cursors.SizeNWSE, Cursors.SizeWE, Cursors.SizeNS };

        private Rectangle _minButtonBounds;

        private Rectangle _maxButtonBounds;

        private Rectangle _xButtonBounds;

        private Rectangle _actionBarBounds;

        private Rectangle _statusBarBounds;

        private bool _maximized;

        private Size _previousSize;

        private Point _previousLocation;

        private bool _headerMouseDown;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.Style = createParams.Style | 131072 | 524288;
                return createParams;
            }
        }

        [Browsable(false)]
        public int Depth
        {
            get;
            set;
        }

        public new FormBorderStyle FormBorderStyle
        {
            get
            {
                return base.FormBorderStyle;
            }
            set
            {
                base.FormBorderStyle = value;
            }
        }

        [Browsable(false)]
        public MouseState MouseState
        {
            get;
            set;
        }

        public bool Sizable
        {
            get;
            set;
        }

        [Browsable(false)]
        public MaterialSkinManager SkinManager
        {
            get
            {
                return MaterialSkinManager.Instance;
            }
        }

        public MaterialForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Sizable = true;
            this.DoubleBuffered = true;
            base.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
            Application.AddMessageFilter(new MouseMessageFilter());
            MouseMessageFilter.MouseMove += new MouseEventHandler(this.OnGlobalMouseMove);
        }

        [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
        public static extern bool GetMonitorInfo(HandleRef hmonitor, [In][Out] MaterialForm.MONITORINFOEX info);

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        private void MaximizeWindow(bool maximize)
        {
            if ((!base.MaximizeBox ? false : base.ControlBox))
            {
                this._maximized = maximize;
                if (!maximize)
                {
                    base.Size = this._previousSize;
                    base.Location = this._previousLocation;
                }
                else
                {
                    IntPtr intPtr = MaterialForm.MonitorFromWindow(base.Handle, 2);
                    MaterialForm.MONITORINFOEX mONITORINFOEX = new MaterialForm.MONITORINFOEX();
                    MaterialForm.GetMonitorInfo(new HandleRef(null, intPtr), mONITORINFOEX);
                    this._previousSize = base.Size;
                    this._previousLocation = base.Location;
                    base.Size = new Size(mONITORINFOEX.rcWork.Width(), mONITORINFOEX.rcWork.Height());
                    base.Location = new Point(mONITORINFOEX.rcWork.left, mONITORINFOEX.rcWork.top);
                }
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        protected void OnGlobalMouseMove(object sender, MouseEventArgs e)
        {
            if (!base.IsDisposed)
            {
                Point client = base.PointToClient(e.Location);
                MouseEventArgs mouseEventArg = new MouseEventArgs(MouseButtons.None, 0, client.X, client.Y, 0);
                this.OnMouseMove(mouseEventArg);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!base.DesignMode)
            {
                this.UpdateButtons(e, false);
                if ((e.Button != MouseButtons.Left ? false : !this._maximized))
                {
                    this.ResizeForm(this._resizeDir);
                }
                base.OnMouseDown(e);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (!base.DesignMode)
            {
                this._buttonState = MaterialForm.ButtonState.None;
                base.Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!base.DesignMode)
            {
                if (this.Sizable)
                {
                    bool childAtPoint = base.GetChildAtPoint(e.Location) != null;
                    if ((e.Location.X >= 7 || e.Location.Y <= base.Height - 7 || childAtPoint ? false : !this._maximized))
                    {
                        this._resizeDir = MaterialForm.ResizeDirection.BottomLeft;
                        this.Cursor = Cursors.SizeNESW;
                    }
                    else if ((e.Location.X >= 7 || childAtPoint ? false : !this._maximized))
                    {
                        this._resizeDir = MaterialForm.ResizeDirection.Left;
                        this.Cursor = Cursors.SizeWE;
                    }
                    else if ((e.Location.X <= base.Width - 7 || e.Location.Y <= base.Height - 7 || childAtPoint ? false : !this._maximized))
                    {
                        this._resizeDir = MaterialForm.ResizeDirection.BottomRight;
                        this.Cursor = Cursors.SizeNWSE;
                    }
                    else if ((e.Location.X <= base.Width - 7 || childAtPoint ? false : !this._maximized))
                    {
                        this._resizeDir = MaterialForm.ResizeDirection.Right;
                        this.Cursor = Cursors.SizeWE;
                    }
                    else if ((e.Location.Y <= base.Height - 7 || childAtPoint ? true : this._maximized))
                    {
                        this._resizeDir = MaterialForm.ResizeDirection.None;
                        if (this._resizeCursors.Contains<Cursor>(this.Cursor))
                        {
                            this.Cursor = Cursors.Default;
                        }
                    }
                    else
                    {
                        this._resizeDir = MaterialForm.ResizeDirection.Bottom;
                        this.Cursor = Cursors.SizeNS;
                    }
                }
                this.UpdateButtons(e, false);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (!base.DesignMode)
            {
                this.UpdateButtons(e, true);
                base.OnMouseUp(e);
                MaterialForm.ReleaseCapture();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.Clear(this.SkinManager.GetApplicationBackgroundColor());
            graphics.FillRectangle(this.SkinManager.ColorScheme.DarkPrimaryBrush, this._statusBarBounds);
            graphics.FillRectangle(this.SkinManager.ColorScheme.PrimaryBrush, this._actionBarBounds);
            using (Pen pen = new Pen(this.SkinManager.GetDividersColor(), 1f))
            {
                graphics.DrawLine(pen, new Point(0, this._actionBarBounds.Bottom), new Point(0, base.Height - 2));
                graphics.DrawLine(pen, new Point(base.Width - 1, this._actionBarBounds.Bottom), new Point(base.Width - 1, base.Height - 2));
                graphics.DrawLine(pen, new Point(0, base.Height - 1), new Point(base.Width - 1, base.Height - 1));
            }
            bool flag = (!base.MinimizeBox ? false : base.ControlBox);
            bool flag1 = (!base.MaximizeBox ? false : base.ControlBox);
            Brush flatButtonHoverBackgroundBrush = this.SkinManager.GetFlatButtonHoverBackgroundBrush();
            Brush flatButtonPressedBackgroundBrush = this.SkinManager.GetFlatButtonPressedBackgroundBrush();
            if (this._buttonState == MaterialForm.ButtonState.MinOver & flag)
            {
                graphics.FillRectangle(flatButtonHoverBackgroundBrush, (flag1 ? this._minButtonBounds : this._maxButtonBounds));
            }
            if (this._buttonState == MaterialForm.ButtonState.MinDown & flag)
            {
                graphics.FillRectangle(flatButtonPressedBackgroundBrush, (flag1 ? this._minButtonBounds : this._maxButtonBounds));
            }
            if (this._buttonState == MaterialForm.ButtonState.MaxOver & flag1)
            {
                graphics.FillRectangle(flatButtonHoverBackgroundBrush, this._maxButtonBounds);
            }
            if (this._buttonState == MaterialForm.ButtonState.MaxDown & flag1)
            {
                graphics.FillRectangle(flatButtonPressedBackgroundBrush, this._maxButtonBounds);
            }
            if ((this._buttonState != MaterialForm.ButtonState.XOver ? false : base.ControlBox))
            {
                graphics.FillRectangle(flatButtonHoverBackgroundBrush, this._xButtonBounds);
            }
            if ((this._buttonState != MaterialForm.ButtonState.XDown ? false : base.ControlBox))
            {
                graphics.FillRectangle(flatButtonPressedBackgroundBrush, this._xButtonBounds);
            }
            using (Pen pen1 = new Pen(this.SkinManager.ACTION_BAR_TEXT_SECONDARY, 2f))
            {
                if (flag)
                {
                    int num = (flag1 ? this._minButtonBounds.X : this._maxButtonBounds.X);
                    int num1 = (flag1 ? this._minButtonBounds.Y : this._maxButtonBounds.Y);
                    graphics.DrawLine(pen1, num + (int)((double)this._minButtonBounds.Width * 0.33), num1 + (int)((double)this._minButtonBounds.Height * 0.66), num + (int)((double)this._minButtonBounds.Width * 0.66), num1 + (int)((double)this._minButtonBounds.Height * 0.66));
                }
                if (flag1)
                {
                    graphics.DrawRectangle(pen1, this._maxButtonBounds.X + (int)((double)this._maxButtonBounds.Width * 0.33), this._maxButtonBounds.Y + (int)((double)this._maxButtonBounds.Height * 0.36), (int)((double)this._maxButtonBounds.Width * 0.39), (int)((double)this._maxButtonBounds.Height * 0.31));
                }
                if (base.ControlBox)
                {
                    graphics.DrawLine(pen1, this._xButtonBounds.X + (int)((double)this._xButtonBounds.Width * 0.33), this._xButtonBounds.Y + (int)((double)this._xButtonBounds.Height * 0.33), this._xButtonBounds.X + (int)((double)this._xButtonBounds.Width * 0.66), this._xButtonBounds.Y + (int)((double)this._xButtonBounds.Height * 0.66));
                    graphics.DrawLine(pen1, this._xButtonBounds.X + (int)((double)this._xButtonBounds.Width * 0.66), this._xButtonBounds.Y + (int)((double)this._xButtonBounds.Height * 0.33), this._xButtonBounds.X + (int)((double)this._xButtonBounds.Width * 0.33), this._xButtonBounds.Y + (int)((double)this._xButtonBounds.Height * 0.66));
                }
            }
            graphics.DrawString(this.Text, this.SkinManager.ROBOTO_MEDIUM_12, this.SkinManager.ColorScheme.TextBrush, new Rectangle(this.SkinManager.FORM_PADDING, 24, base.Width, 40), new StringFormat()
            {
                LineAlignment = StringAlignment.Center
            });
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this._minButtonBounds = new Rectangle(base.Width - this.SkinManager.FORM_PADDING / 2 - 72, 0, 24, 24);
            this._maxButtonBounds = new Rectangle(base.Width - this.SkinManager.FORM_PADDING / 2 - 48, 0, 24, 24);
            this._xButtonBounds = new Rectangle(base.Width - this.SkinManager.FORM_PADDING / 2 - 24, 0, 24, 24);
            this._statusBarBounds = new Rectangle(0, 0, base.Width, 24);
            this._actionBarBounds = new Rectangle(0, 24, base.Width, 40);
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern bool ReleaseCapture();

        private void ResizeForm(MaterialForm.ResizeDirection direction)
        {
            if (!base.DesignMode)
            {
                int num = -1;
                switch (direction)
                {
                    case MaterialForm.ResizeDirection.BottomLeft:
                        {
                            num = 16;
                            break;
                        }
                    case MaterialForm.ResizeDirection.Left:
                        {
                            num = 10;
                            break;
                        }
                    case MaterialForm.ResizeDirection.Right:
                        {
                            num = 11;
                            break;
                        }
                    case MaterialForm.ResizeDirection.BottomRight:
                        {
                            num = 17;
                            break;
                        }
                    case MaterialForm.ResizeDirection.Bottom:
                        {
                            num = 15;
                            break;
                        }
                }
                MaterialForm.ReleaseCapture();
                if (num != -1)
                {
                    MaterialForm.SendMessage(base.Handle, 161, num, 0);
                }
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

        private void UpdateButtons(MouseEventArgs e, bool up = false)
        {
            if (!base.DesignMode)
            {
                MaterialForm.ButtonState buttonState = this._buttonState;
                bool flag = (!base.MinimizeBox ? false : base.ControlBox);
                bool flag1 = (!base.MaximizeBox ? false : base.ControlBox);
                if ((e.Button != MouseButtons.Left ? false : !up))
                {
                    if ((!flag || flag1 ? false : this._maxButtonBounds.Contains(e.Location)))
                    {
                        this._buttonState = MaterialForm.ButtonState.MinDown;
                    }
                    else if ((!(flag & flag1) ? false : this._minButtonBounds.Contains(e.Location)))
                    {
                        this._buttonState = MaterialForm.ButtonState.MinDown;
                    }
                    else if ((!flag1 ? false : this._maxButtonBounds.Contains(e.Location)))
                    {
                        this._buttonState = MaterialForm.ButtonState.MaxDown;
                    }
                    else if ((!base.ControlBox ? true : !this._xButtonBounds.Contains(e.Location)))
                    {
                        this._buttonState = MaterialForm.ButtonState.None;
                    }
                    else
                    {
                        this._buttonState = MaterialForm.ButtonState.XDown;
                    }
                }
                else if ((!flag || flag1 ? false : this._maxButtonBounds.Contains(e.Location)))
                {
                    this._buttonState = MaterialForm.ButtonState.MinOver;
                    if (buttonState == MaterialForm.ButtonState.MinDown & up)
                    {
                        base.WindowState = FormWindowState.Minimized;
                    }
                }
                else if ((!(flag & flag1) ? false : this._minButtonBounds.Contains(e.Location)))
                {
                    this._buttonState = MaterialForm.ButtonState.MinOver;
                    if (buttonState == MaterialForm.ButtonState.MinDown & up)
                    {
                        base.WindowState = FormWindowState.Minimized;
                    }
                }
                else if ((!base.MaximizeBox || !base.ControlBox ? false : this._maxButtonBounds.Contains(e.Location)))
                {
                    this._buttonState = MaterialForm.ButtonState.MaxOver;
                    if (buttonState == MaterialForm.ButtonState.MaxDown & up)
                    {
                        this.MaximizeWindow(!this._maximized);
                    }
                }
                else if ((!base.ControlBox ? true : !this._xButtonBounds.Contains(e.Location)))
                {
                    this._buttonState = MaterialForm.ButtonState.None;
                }
                else
                {
                    this._buttonState = MaterialForm.ButtonState.XOver;
                    if (buttonState == MaterialForm.ButtonState.XDown & up)
                    {
                        base.Close();
                    }
                }
                if (buttonState != this._buttonState)
                {
                    base.Invalidate();
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            Point position;
            bool flag;
            bool flag1;
            Point point;
            Point point1;
            base.WndProc(ref m);
            if ((base.DesignMode ? false : !base.IsDisposed))
            {
                if (m.Msg != 515)
                {
                    if (m.Msg != 512 || !this._maximized || !this._statusBarBounds.Contains(base.PointToClient(Cursor.Position)) && !this._actionBarBounds.Contains(base.PointToClient(Cursor.Position)))
                    {
                        flag = false;
                    }
                    else
                    {
                        flag = (this._minButtonBounds.Contains(base.PointToClient(Cursor.Position)) || this._maxButtonBounds.Contains(base.PointToClient(Cursor.Position)) ? false : !this._xButtonBounds.Contains(base.PointToClient(Cursor.Position)));
                    }
                    if (!flag)
                    {
                        if (m.Msg != 513 || !this._statusBarBounds.Contains(base.PointToClient(Cursor.Position)) && !this._actionBarBounds.Contains(base.PointToClient(Cursor.Position)))
                        {
                            flag1 = false;
                        }
                        else
                        {
                            flag1 = (this._minButtonBounds.Contains(base.PointToClient(Cursor.Position)) || this._maxButtonBounds.Contains(base.PointToClient(Cursor.Position)) ? false : !this._xButtonBounds.Contains(base.PointToClient(Cursor.Position)));
                        }
                        if (flag1)
                        {
                            if (this._maximized)
                            {
                                this._headerMouseDown = true;
                            }
                            else
                            {
                                MaterialForm.ReleaseCapture();
                                MaterialForm.SendMessage(base.Handle, 161, 2, 0);
                            }
                        }
                        else if (m.Msg == 516)
                        {
                            Point client = base.PointToClient(Cursor.Position);
                            if ((!this._statusBarBounds.Contains(client) || this._minButtonBounds.Contains(client) || this._maxButtonBounds.Contains(client) ? false : !this._xButtonBounds.Contains(client)))
                            {
                                IntPtr systemMenu = MaterialForm.GetSystemMenu(base.Handle, false);
                                int x = Cursor.Position.X;
                                position = Cursor.Position;
                                int num = MaterialForm.TrackPopupMenuEx(systemMenu, 256, x, position.Y, base.Handle, IntPtr.Zero);
                                MaterialForm.SendMessage(base.Handle, 274, num, 0);
                            }
                        }
                        else if (m.Msg == 161)
                        {
                            if (this.Sizable)
                            {
                                byte item = 0;
                                if (this._resizingLocationsToCmd.ContainsKey((int)m.WParam))
                                {
                                    item = (byte)this._resizingLocationsToCmd[(int)m.WParam];
                                }
                                if (item != 0)
                                {
                                    MaterialForm.SendMessage(base.Handle, 274, 61440 | item, (int)m.LParam);
                                }
                            }
                        }
                        else if (m.Msg == 514)
                        {
                            this._headerMouseDown = false;
                        }
                    }
                    else if (this._headerMouseDown)
                    {
                        this._maximized = false;
                        this._headerMouseDown = false;
                        Point client1 = base.PointToClient(Cursor.Position);
                        if (client1.X >= base.Width / 2)
                        {
                            if (base.Width - client1.X < this._previousSize.Width / 2)
                            {
                                position = Cursor.Position;
                                int x1 = position.X - this._previousSize.Width + base.Width - client1.X;
                                position = Cursor.Position;
                                point = new Point(x1, position.Y - client1.Y);
                            }
                            else
                            {
                                position = Cursor.Position;
                                int num1 = position.X - this._previousSize.Width / 2;
                                position = Cursor.Position;
                                point = new Point(num1, position.Y - client1.Y);
                            }
                            base.Location = point;
                        }
                        else
                        {
                            if (client1.X < this._previousSize.Width / 2)
                            {
                                position = Cursor.Position;
                                int x2 = position.X - client1.X;
                                position = Cursor.Position;
                                point1 = new Point(x2, position.Y - client1.Y);
                            }
                            else
                            {
                                position = Cursor.Position;
                                int num2 = position.X - this._previousSize.Width / 2;
                                position = Cursor.Position;
                                point1 = new Point(num2, position.Y - client1.Y);
                            }
                            base.Location = point1;
                        }
                        base.Size = this._previousSize;
                        MaterialForm.ReleaseCapture();
                        MaterialForm.SendMessage(base.Handle, 161, 2, 0);
                    }
                }
                else
                {
                    this.MaximizeWindow(!this._maximized);
                }
            }
        }

        private enum ButtonState
        {
            XOver,
            MaxOver,
            MinOver,
            XDown,
            MaxDown,
            MinDown,
            None
        }

        public class MONITORINFOEX
        {
            public int cbSize = Marshal.SizeOf(typeof(MaterialForm.MONITORINFOEX));

            public MaterialForm.RECT rcMonitor = new MaterialForm.RECT();

            public MaterialForm.RECT rcWork = new MaterialForm.RECT();

            public int dwFlags = 0;

            public char[] szDevice = new char[32];

            public MONITORINFOEX()
            {
            }
        }

        public struct RECT
        {
            public int left;

            public int top;

            public int right;

            public int bottom;

            public int Height()
            {
                return this.bottom - this.top;
            }

            public int Width()
            {
                return this.right - this.left;
            }
        }

        private enum ResizeDirection
        {
            BottomLeft,
            Left,
            Right,
            BottomRight,
            Bottom,
            None
        }
    }

    public class MouseMessageFilter : IMessageFilter
    {
        private const int WM_MOUSEMOVE = 512;

        public MouseMessageFilter()
        {
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 512)
            {
                if (MouseMessageFilter.MouseMove != null)
                {
                    int x = Control.MousePosition.X;
                    int y = Control.MousePosition.Y;
                    MouseMessageFilter.MouseMove(null, new MouseEventArgs(MouseButtons.None, 0, x, y, 0));
                }
            }
            return false;
        }

        public static event MouseEventHandler MouseMove;
    }
}
