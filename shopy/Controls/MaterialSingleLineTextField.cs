using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shopy.Controls
{
    public class MaterialSingleLineTextField : Control, IMaterialControl
    {
        private readonly MaterialSingleLineTextField.BaseTextBox _baseTextBox;

        private readonly AnimationManager _animationManager;

        [Browsable(false)]
        public int Depth
        {
            get;
            set;
        }

        public string Hint
        {
            get
            {
                return this._baseTextBox.Hint;
            }
            set
            {
                this._baseTextBox.Hint = value;
            }
        }

        public int MaxLength
        {
            get
            {
                return this._baseTextBox.MaxLength;
            }
            set
            {
                this._baseTextBox.MaxLength = value;
            }
        }

        [Browsable(false)]
        public MouseState MouseState
        {
            get;
            set;
        }

        public char PasswordChar
        {
            get
            {
                return this._baseTextBox.PasswordChar;
            }
            set
            {
                this._baseTextBox.PasswordChar = value;
            }
        }

        public string SelectedText
        {
            get
            {
                return this._baseTextBox.SelectedText;
            }
            set
            {
                this._baseTextBox.SelectedText = value;
            }
        }

        public int SelectionLength
        {
            get
            {
                return this._baseTextBox.SelectionLength;
            }
            set
            {
                this._baseTextBox.SelectionLength = value;
            }
        }

        public int SelectionStart
        {
            get
            {
                return this._baseTextBox.SelectionStart;
            }
            set
            {
                this._baseTextBox.SelectionStart = value;
            }
        }

        [Browsable(false)]
        public MaterialSkinManager SkinManager
        {
            get
            {
                return MaterialSkinManager.Instance;
            }
        }

        public new object Tag
        {
            get
            {
                return this._baseTextBox.Tag;
            }
            set
            {
                this._baseTextBox.Tag = value;
            }
        }

        public override string Text
        {
            get
            {
                return this._baseTextBox.Text;
            }
            set
            {
                this._baseTextBox.Text = value;
            }
        }

        public int TextLength
        {
            get
            {
                return this._baseTextBox.TextLength;
            }
        }

        public bool UseSystemPasswordChar
        {
            get
            {
                return this._baseTextBox.UseSystemPasswordChar;
            }
            set
            {
                this._baseTextBox.UseSystemPasswordChar = value;
            }
        }

        public MaterialSingleLineTextField()
        {
            base.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            this._animationManager = new AnimationManager(true)
            {
                Increment = 0.06,
                AnimationType = AnimationType.EaseInOut,
                InterruptAnimation = false
            };
            this._animationManager.OnAnimationProgress += new AnimationManager.AnimationProgress((object sender) => base.Invalidate());
            this._baseTextBox = new MaterialSingleLineTextField.BaseTextBox()
            {
                BorderStyle = BorderStyle.None,
                Font = this.SkinManager.ROBOTO_REGULAR_11,
                ForeColor = this.SkinManager.GetPrimaryTextColor(),
                Location = new Point(0, 0),
                Width = base.Width,
                Height = base.Height - 5
            };
            if ((base.Controls.Contains(this._baseTextBox) ? false : !base.DesignMode))
            {
                base.Controls.Add(this._baseTextBox);
            }
            this._baseTextBox.GotFocus += new EventHandler((object sender, EventArgs args) => this._animationManager.StartNewAnimation(AnimationDirection.In, null));
            this._baseTextBox.LostFocus += new EventHandler((object sender, EventArgs args) => this._animationManager.StartNewAnimation(AnimationDirection.Out, null));
            base.BackColorChanged += new EventHandler((object sender, EventArgs args) => {
                this._baseTextBox.BackColor = this.BackColor;
                this._baseTextBox.ForeColor = this.SkinManager.GetPrimaryTextColor();
            });
            this._baseTextBox.TabStop = true;
            base.TabStop = false;
        }

        public void Clear()
        {
            this._baseTextBox.Clear();
        }

        public new void Focus()
        {
            this._baseTextBox.Focus();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this._baseTextBox.BackColor = base.Parent.BackColor;
            this._baseTextBox.ForeColor = this.SkinManager.GetPrimaryTextColor();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics graphics = pevent.Graphics;
            graphics.Clear(base.Parent.BackColor);
            int bottom = this._baseTextBox.Bottom + 3;
            if (this._animationManager.IsAnimating())
            {
                int width = (int)((double)this._baseTextBox.Width * this._animationManager.GetProgress());
                int num = width / 2;
                Point location = this._baseTextBox.Location;
                int x = location.X + this._baseTextBox.Width / 2;
                Brush dividersBrush = this.SkinManager.GetDividersBrush();
                location = this._baseTextBox.Location;
                graphics.FillRectangle(dividersBrush, location.X, bottom, this._baseTextBox.Width, 1);
                graphics.FillRectangle(this.SkinManager.ColorScheme.PrimaryBrush, x - num, bottom, width, 2);
            }
            else
            {
                graphics.FillRectangle((this._baseTextBox.Focused ? this.SkinManager.ColorScheme.PrimaryBrush : this.SkinManager.GetDividersBrush()), this._baseTextBox.Location.X, bottom, this._baseTextBox.Width, (this._baseTextBox.Focused ? 2 : 1));
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this._baseTextBox.Location = new Point(0, 0);
            this._baseTextBox.Width = base.Width;
            base.Height = this._baseTextBox.Height + 5;
        }

        public void SelectAll()
        {
            this._baseTextBox.SelectAll();
        }

        public event EventHandler AcceptsTabChanged
        {
            add
            {
                this._baseTextBox.AcceptsTabChanged += value;
            }
            remove
            {
                this._baseTextBox.AcceptsTabChanged -= value;
            }
        }

        public event EventHandler AutoSizeChanged
        {
            add
            {
                this._baseTextBox.AutoSizeChanged += value;
            }
            remove
            {
                this._baseTextBox.AutoSizeChanged -= value;
            }
        }

        public event EventHandler BackgroundImageChanged
        {
            add
            {
                this._baseTextBox.BackgroundImageChanged += value;
            }
            remove
            {
                this._baseTextBox.BackgroundImageChanged -= value;
            }
        }

        public event EventHandler BackgroundImageLayoutChanged
        {
            add
            {
                this._baseTextBox.BackgroundImageLayoutChanged += value;
            }
            remove
            {
                this._baseTextBox.BackgroundImageLayoutChanged -= value;
            }
        }

        public event EventHandler BindingContextChanged
        {
            add
            {
                this._baseTextBox.BindingContextChanged += value;
            }
            remove
            {
                this._baseTextBox.BindingContextChanged -= value;
            }
        }

        public event EventHandler BorderStyleChanged
        {
            add
            {
                this._baseTextBox.BorderStyleChanged += value;
            }
            remove
            {
                this._baseTextBox.BorderStyleChanged -= value;
            }
        }

        public event EventHandler CausesValidationChanged
        {
            add
            {
                this._baseTextBox.CausesValidationChanged += value;
            }
            remove
            {
                this._baseTextBox.CausesValidationChanged -= value;
            }
        }

        public event UICuesEventHandler ChangeUICues
        {
            add
            {
                this._baseTextBox.ChangeUICues += value;
            }
            remove
            {
                this._baseTextBox.ChangeUICues -= value;
            }
        }

        public event EventHandler Click
        {
            add
            {
                this._baseTextBox.Click += value;
            }
            remove
            {
                this._baseTextBox.Click -= value;
            }
        }

        public event EventHandler ClientSizeChanged
        {
            add
            {
                this._baseTextBox.ClientSizeChanged += value;
            }
            remove
            {
                this._baseTextBox.ClientSizeChanged -= value;
            }
        }

        public event EventHandler ContextMenuChanged
        {
            add
            {
                this._baseTextBox.ContextMenuChanged += value;
            }
            remove
            {
                this._baseTextBox.ContextMenuChanged -= value;
            }
        }

        public event EventHandler ContextMenuStripChanged
        {
            add
            {
                this._baseTextBox.ContextMenuStripChanged += value;
            }
            remove
            {
                this._baseTextBox.ContextMenuStripChanged -= value;
            }
        }

        public event ControlEventHandler ControlAdded
        {
            add
            {
                this._baseTextBox.ControlAdded += value;
            }
            remove
            {
                this._baseTextBox.ControlAdded -= value;
            }
        }

        public event ControlEventHandler ControlRemoved
        {
            add
            {
                this._baseTextBox.ControlRemoved += value;
            }
            remove
            {
                this._baseTextBox.ControlRemoved -= value;
            }
        }

        public event EventHandler CursorChanged
        {
            add
            {
                this._baseTextBox.CursorChanged += value;
            }
            remove
            {
                this._baseTextBox.CursorChanged -= value;
            }
        }

        public event EventHandler Disposed
        {
            add
            {
                this._baseTextBox.Disposed += value;
            }
            remove
            {
                this._baseTextBox.Disposed -= value;
            }
        }

        public event EventHandler DockChanged
        {
            add
            {
                this._baseTextBox.DockChanged += value;
            }
            remove
            {
                this._baseTextBox.DockChanged -= value;
            }
        }

        public event EventHandler DoubleClick
        {
            add
            {
                this._baseTextBox.DoubleClick += value;
            }
            remove
            {
                this._baseTextBox.DoubleClick -= value;
            }
        }

        public event DragEventHandler DragDrop
        {
            add
            {
                this._baseTextBox.DragDrop += value;
            }
            remove
            {
                this._baseTextBox.DragDrop -= value;
            }
        }

        public event DragEventHandler DragEnter
        {
            add
            {
                this._baseTextBox.DragEnter += value;
            }
            remove
            {
                this._baseTextBox.DragEnter -= value;
            }
        }

        public event EventHandler DragLeave
        {
            add
            {
                this._baseTextBox.DragLeave += value;
            }
            remove
            {
                this._baseTextBox.DragLeave -= value;
            }
        }

        public event DragEventHandler DragOver
        {
            add
            {
                this._baseTextBox.DragOver += value;
            }
            remove
            {
                this._baseTextBox.DragOver -= value;
            }
        }

        public event EventHandler EnabledChanged
        {
            add
            {
                this._baseTextBox.EnabledChanged += value;
            }
            remove
            {
                this._baseTextBox.EnabledChanged -= value;
            }
        }

        public event EventHandler Enter
        {
            add
            {
                this._baseTextBox.Enter += value;
            }
            remove
            {
                this._baseTextBox.Enter -= value;
            }
        }

        public event EventHandler FontChanged
        {
            add
            {
                this._baseTextBox.FontChanged += value;
            }
            remove
            {
                this._baseTextBox.FontChanged -= value;
            }
        }

        public event EventHandler ForeColorChanged
        {
            add
            {
                this._baseTextBox.ForeColorChanged += value;
            }
            remove
            {
                this._baseTextBox.ForeColorChanged -= value;
            }
        }

        public event GiveFeedbackEventHandler GiveFeedback
        {
            add
            {
                this._baseTextBox.GiveFeedback += value;
            }
            remove
            {
                this._baseTextBox.GiveFeedback -= value;
            }
        }

        public event EventHandler GotFocus
        {
            add
            {
                this._baseTextBox.GotFocus += value;
            }
            remove
            {
                this._baseTextBox.GotFocus -= value;
            }
        }

        public event EventHandler HandleCreated
        {
            add
            {
                this._baseTextBox.HandleCreated += value;
            }
            remove
            {
                this._baseTextBox.HandleCreated -= value;
            }
        }

        public event EventHandler HandleDestroyed
        {
            add
            {
                this._baseTextBox.HandleDestroyed += value;
            }
            remove
            {
                this._baseTextBox.HandleDestroyed -= value;
            }
        }

        public event HelpEventHandler HelpRequested
        {
            add
            {
                this._baseTextBox.HelpRequested += value;
            }
            remove
            {
                this._baseTextBox.HelpRequested -= value;
            }
        }

        public event EventHandler HideSelectionChanged
        {
            add
            {
                this._baseTextBox.HideSelectionChanged += value;
            }
            remove
            {
                this._baseTextBox.HideSelectionChanged -= value;
            }
        }

        public event EventHandler ImeModeChanged
        {
            add
            {
                this._baseTextBox.ImeModeChanged += value;
            }
            remove
            {
                this._baseTextBox.ImeModeChanged -= value;
            }
        }

        public event InvalidateEventHandler Invalidated
        {
            add
            {
                this._baseTextBox.Invalidated += value;
            }
            remove
            {
                this._baseTextBox.Invalidated -= value;
            }
        }

        public event KeyEventHandler KeyDown
        {
            add
            {
                this._baseTextBox.KeyDown += value;
            }
            remove
            {
                this._baseTextBox.KeyDown -= value;
            }
        }

        public event KeyPressEventHandler KeyPress
        {
            add
            {
                this._baseTextBox.KeyPress += value;
            }
            remove
            {
                this._baseTextBox.KeyPress -= value;
            }
        }

        public event KeyEventHandler KeyUp
        {
            add
            {
                this._baseTextBox.KeyUp += value;
            }
            remove
            {
                this._baseTextBox.KeyUp -= value;
            }
        }

        public event LayoutEventHandler Layout
        {
            add
            {
                this._baseTextBox.Layout += value;
            }
            remove
            {
                this._baseTextBox.Layout -= value;
            }
        }

        public event EventHandler Leave
        {
            add
            {
                this._baseTextBox.Leave += value;
            }
            remove
            {
                this._baseTextBox.Leave -= value;
            }
        }

        public event EventHandler LocationChanged
        {
            add
            {
                this._baseTextBox.LocationChanged += value;
            }
            remove
            {
                this._baseTextBox.LocationChanged -= value;
            }
        }

        public event EventHandler LostFocus
        {
            add
            {
                this._baseTextBox.LostFocus += value;
            }
            remove
            {
                this._baseTextBox.LostFocus -= value;
            }
        }

        public event EventHandler MarginChanged
        {
            add
            {
                this._baseTextBox.MarginChanged += value;
            }
            remove
            {
                this._baseTextBox.MarginChanged -= value;
            }
        }

        public event EventHandler ModifiedChanged
        {
            add
            {
                this._baseTextBox.ModifiedChanged += value;
            }
            remove
            {
                this._baseTextBox.ModifiedChanged -= value;
            }
        }

        public event EventHandler MouseCaptureChanged
        {
            add
            {
                this._baseTextBox.MouseCaptureChanged += value;
            }
            remove
            {
                this._baseTextBox.MouseCaptureChanged -= value;
            }
        }

        public event MouseEventHandler MouseClick
        {
            add
            {
                this._baseTextBox.MouseClick += value;
            }
            remove
            {
                this._baseTextBox.MouseClick -= value;
            }
        }

        public event MouseEventHandler MouseDoubleClick
        {
            add
            {
                this._baseTextBox.MouseDoubleClick += value;
            }
            remove
            {
                this._baseTextBox.MouseDoubleClick -= value;
            }
        }

        public event MouseEventHandler MouseDown
        {
            add
            {
                this._baseTextBox.MouseDown += value;
            }
            remove
            {
                this._baseTextBox.MouseDown -= value;
            }
        }

        public event EventHandler MouseEnter
        {
            add
            {
                this._baseTextBox.MouseEnter += value;
            }
            remove
            {
                this._baseTextBox.MouseEnter -= value;
            }
        }

        public event EventHandler MouseHover
        {
            add
            {
                this._baseTextBox.MouseHover += value;
            }
            remove
            {
                this._baseTextBox.MouseHover -= value;
            }
        }

        public event EventHandler MouseLeave
        {
            add
            {
                this._baseTextBox.MouseLeave += value;
            }
            remove
            {
                this._baseTextBox.MouseLeave -= value;
            }
        }

        public event MouseEventHandler MouseMove
        {
            add
            {
                this._baseTextBox.MouseMove += value;
            }
            remove
            {
                this._baseTextBox.MouseMove -= value;
            }
        }

        public event MouseEventHandler MouseUp
        {
            add
            {
                this._baseTextBox.MouseUp += value;
            }
            remove
            {
                this._baseTextBox.MouseUp -= value;
            }
        }

        public event MouseEventHandler MouseWheel
        {
            add
            {
                this._baseTextBox.MouseWheel += value;
            }
            remove
            {
                this._baseTextBox.MouseWheel -= value;
            }
        }

        public event EventHandler Move
        {
            add
            {
                this._baseTextBox.Move += value;
            }
            remove
            {
                this._baseTextBox.Move -= value;
            }
        }

        public event EventHandler MultilineChanged
        {
            add
            {
                this._baseTextBox.MultilineChanged += value;
            }
            remove
            {
                this._baseTextBox.MultilineChanged -= value;
            }
        }

        public event EventHandler PaddingChanged
        {
            add
            {
                this._baseTextBox.PaddingChanged += value;
            }
            remove
            {
                this._baseTextBox.PaddingChanged -= value;
            }
        }

        public event PaintEventHandler Paint
        {
            add
            {
                this._baseTextBox.Paint += value;
            }
            remove
            {
                this._baseTextBox.Paint -= value;
            }
        }

        public event EventHandler ParentChanged
        {
            add
            {
                this._baseTextBox.ParentChanged += value;
            }
            remove
            {
                this._baseTextBox.ParentChanged -= value;
            }
        }

        public event PreviewKeyDownEventHandler PreviewKeyDown
        {
            add
            {
                this._baseTextBox.PreviewKeyDown += value;
            }
            remove
            {
                this._baseTextBox.PreviewKeyDown -= value;
            }
        }

        public event QueryAccessibilityHelpEventHandler QueryAccessibilityHelp
        {
            add
            {
                this._baseTextBox.QueryAccessibilityHelp += value;
            }
            remove
            {
                this._baseTextBox.QueryAccessibilityHelp -= value;
            }
        }

        public event QueryContinueDragEventHandler QueryContinueDrag
        {
            add
            {
                this._baseTextBox.QueryContinueDrag += value;
            }
            remove
            {
                this._baseTextBox.QueryContinueDrag -= value;
            }
        }

        public event EventHandler ReadOnlyChanged
        {
            add
            {
                this._baseTextBox.ReadOnlyChanged += value;
            }
            remove
            {
                this._baseTextBox.ReadOnlyChanged -= value;
            }
        }

        public event EventHandler RegionChanged
        {
            add
            {
                this._baseTextBox.RegionChanged += value;
            }
            remove
            {
                this._baseTextBox.RegionChanged -= value;
            }
        }

        public event EventHandler Resize
        {
            add
            {
                this._baseTextBox.Resize += value;
            }
            remove
            {
                this._baseTextBox.Resize -= value;
            }
        }

        public event EventHandler RightToLeftChanged
        {
            add
            {
                this._baseTextBox.RightToLeftChanged += value;
            }
            remove
            {
                this._baseTextBox.RightToLeftChanged -= value;
            }
        }

        public event EventHandler SizeChanged
        {
            add
            {
                this._baseTextBox.SizeChanged += value;
            }
            remove
            {
                this._baseTextBox.SizeChanged -= value;
            }
        }

        public event EventHandler StyleChanged
        {
            add
            {
                this._baseTextBox.StyleChanged += value;
            }
            remove
            {
                this._baseTextBox.StyleChanged -= value;
            }
        }

        public event EventHandler SystemColorsChanged
        {
            add
            {
                this._baseTextBox.SystemColorsChanged += value;
            }
            remove
            {
                this._baseTextBox.SystemColorsChanged -= value;
            }
        }

        public event EventHandler TabIndexChanged
        {
            add
            {
                this._baseTextBox.TabIndexChanged += value;
            }
            remove
            {
                this._baseTextBox.TabIndexChanged -= value;
            }
        }

        public event EventHandler TabStopChanged
        {
            add
            {
                this._baseTextBox.TabStopChanged += value;
            }
            remove
            {
                this._baseTextBox.TabStopChanged -= value;
            }
        }

        public event EventHandler TextAlignChanged
        {
            add
            {
                this._baseTextBox.TextAlignChanged += value;
            }
            remove
            {
                this._baseTextBox.TextAlignChanged -= value;
            }
        }

        public event EventHandler TextChanged
        {
            add
            {
                this._baseTextBox.TextChanged += value;
            }
            remove
            {
                this._baseTextBox.TextChanged -= value;
            }
        }

        public event EventHandler Validated
        {
            add
            {
                this._baseTextBox.Validated += value;
            }
            remove
            {
                this._baseTextBox.Validated -= value;
            }
        }

        public event CancelEventHandler Validating
        {
            add
            {
                this._baseTextBox.Validating += value;
            }
            remove
            {
                this._baseTextBox.Validating -= value;
            }
        }

        public event EventHandler VisibleChanged
        {
            add
            {
                this._baseTextBox.VisibleChanged += value;
            }
            remove
            {
                this._baseTextBox.VisibleChanged -= value;
            }
        }

        private class BaseTextBox : TextBox
        {
            private const int EM_SETCUEBANNER = 5377;

            private const char EmptyChar = '\0';

            private const char VisualStylePasswordChar = '\u25CF';

            private const char NonVisualStylePasswordChar = '*';

            private string hint = string.Empty;

            private char _passwordChar = '\0';

            private char _useSystemPasswordChar = '\0';

            public string Hint
            {
                get
                {
                    return this.hint;
                }
                set
                {
                    this.hint = value;
                    MaterialSingleLineTextField.BaseTextBox.SendMessage(base.Handle, 5377, (int)IntPtr.Zero, this.Hint);
                }
            }

            public new char PasswordChar
            {
                get
                {
                    return this._passwordChar;
                }
                set
                {
                    this._passwordChar = value;
                    this.SetBasePasswordChar();
                }
            }

            public new bool UseSystemPasswordChar
            {
                get
                {
                    return this._useSystemPasswordChar > '\0';
                }
                set
                {
                    if (!value)
                    {
                        this._useSystemPasswordChar = '\0';
                    }
                    else
                    {
                        this._useSystemPasswordChar = (Application.RenderWithVisualStyles ? '\u25CF' : '*');
                    }
                    this.SetBasePasswordChar();
                }
            }

            public BaseTextBox()
            {
                MaterialContextMenuStrip textBoxContextMenuStrip = new MaterialSingleLineTextField.TextBoxContextMenuStrip();
                textBoxContextMenuStrip.Opening += new CancelEventHandler(this.ContextMenuStripOnOpening);
                textBoxContextMenuStrip.OnItemClickStart += new MaterialContextMenuStrip.ItemClickStart(this.ContextMenuStripOnItemClickStart);
                this.ContextMenuStrip = textBoxContextMenuStrip;
            }

            private void ContextMenuStripOnItemClickStart(object sender, ToolStripItemClickedEventArgs toolStripItemClickedEventArgs)
            {
                string text = toolStripItemClickedEventArgs.ClickedItem.Text;
                if (text == "Undo")
                {
                    base.Undo();
                }
                else if (text == "Cut")
                {
                    base.Cut();
                }
                else if (text == "Copy")
                {
                    base.Copy();
                }
                else if (text == "Paste")
                {
                    base.Paste();
                }
                else if (text == "Delete")
                {
                    this.SelectedText = string.Empty;
                }
                else if (text == "Select All")
                {
                    this.SelectAll();
                }
            }

            private void ContextMenuStripOnOpening(object sender, CancelEventArgs cancelEventArgs)
            {
                MaterialSingleLineTextField.TextBoxContextMenuStrip canUndo = sender as MaterialSingleLineTextField.TextBoxContextMenuStrip;
                if (canUndo != null)
                {
                    canUndo.Undo.Enabled = base.CanUndo;
                    canUndo.Cut.Enabled = !string.IsNullOrEmpty(this.SelectedText);
                    canUndo.Copy.Enabled = !string.IsNullOrEmpty(this.SelectedText);
                    canUndo.Paste.Enabled = Clipboard.ContainsText();
                    canUndo.Delete.Enabled = !string.IsNullOrEmpty(this.SelectedText);
                    canUndo.SelectAll.Enabled = !string.IsNullOrEmpty(this.Text);
                }
            }

            public new void Focus()
            {
                base.BeginInvoke(new MethodInvoker(() => base.Focus()));
            }

            public new void SelectAll()
            {
                base.BeginInvoke(new MethodInvoker(() => {
                    base.Focus();
                    base.SelectAll();
                }));
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

            private void SetBasePasswordChar()
            {
                base.PasswordChar = (this.UseSystemPasswordChar ? this._useSystemPasswordChar : this._passwordChar);
            }
        }

        private class TextBoxContextMenuStrip : MaterialContextMenuStrip
        {
            public readonly ToolStripItem Undo = new MaterialToolStripMenuItem()
            {
                Text = "Undo"
            };

            public readonly ToolStripItem Seperator1 = new ToolStripSeparator();

            public readonly ToolStripItem Cut = new MaterialToolStripMenuItem()
            {
                Text = "Cut"
            };

            public readonly ToolStripItem Copy = new MaterialToolStripMenuItem()
            {
                Text = "Copy"
            };

            public readonly ToolStripItem Paste = new MaterialToolStripMenuItem()
            {
                Text = "Paste"
            };

            public readonly ToolStripItem Delete = new MaterialToolStripMenuItem()
            {
                Text = "Delete"
            };

            public readonly ToolStripItem Seperator2 = new ToolStripSeparator();

            public readonly ToolStripItem SelectAll = new MaterialToolStripMenuItem()
            {
                Text = "Select All"
            };

            public TextBoxContextMenuStrip()
            {
                this.Items.AddRange(new ToolStripItem[] { this.Undo, this.Seperator1, this.Cut, this.Copy, this.Paste, this.Delete, this.Seperator2, this.SelectAll });
            }
        }
    }
}
