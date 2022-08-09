using shopy.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shopy.Controls
{
    class Material
    {
    }

    public enum Accent
    {
        LightBlue700 = 37354,
        LightBlue400 = 45311,
        Cyan700 = 47316,
        Teal700 = 49061,
        Green700 = 51283,
        Cyan400 = 58879,
        Green400 = 58998,
        Cyan200 = 1638399,
        Teal400 = 1960374,
        Blue700 = 2712319,
        Blue400 = 2718207,
        Indigo700 = 3166206,
        Indigo400 = 4020990,
        LightBlue200 = 4244735,
        Blue200 = 4492031,
        Indigo200 = 5467646,
        DeepPurple700 = 6422762,
        LightGreen700 = 6610199,
        Teal200 = 6619098,
        DeepPurple400 = 6627327,
        Green200 = 6942894,
        LightGreen400 = 7798531,
        DeepPurple200 = 8146431,
        LightBlue100 = 8444159,
        Blue100 = 8565247,
        Cyan100 = 8716287,
        Indigo100 = 9215743,
        Teal100 = 11010027,
        Purple700 = 11141375,
        Lime700 = 11463168,
        LightGreen200 = 11730777,
        DeepPurple100 = 11766015,
        Green100 = 12187338,
        Pink700 = 12915042,
        Lime400 = 13041408,
        LightGreen100 = 13434768,
        Red700 = 13959168,
        Purple400 = 13959417,
        DeepOrange700 = 14494720,
        Purple200 = 14696699,
        Purple100 = 15368444,
        Lime200 = 15662913,
        Lime100 = 16056193,
        Pink400 = 16056407,
        Red400 = 16717636,
        DeepOrange400 = 16727296,
        Pink200 = 16728193,
        Red200 = 16732754,
        Orange700 = 16739584,
        DeepOrange200 = 16739904,
        Pink100 = 16744619,
        Red100 = 16747136,
        Orange400 = 16748800,
        DeepOrange100 = 16752256,
        Amber700 = 16755456,
        Orange200 = 16755520,
        Amber400 = 16761856,
        Orange100 = 16765312,
        Yellow700 = 16766464,
        Amber200 = 16766784,
        Amber100 = 16770431,
        Yellow400 = 16771584,
        Yellow200 = 16776960,
        Yellow100 = 16777101
    }

    public static class ColorExtension
    {
        public static int PercentageToColorComponent(this int percentage)
        {
            return (int)((double)percentage / 100 * 255);
        }

        public static Color RemoveAlpha(this Color color)
        {
            Color color1 = Color.FromArgb((int)color.R, (int)color.G, (int)color.B);
            return color1;
        }

        public static Color ToColor(this int argb)
        {
            Color color = Color.FromArgb((argb & 16711680) >> 16, (argb & 65280) >> 8, argb & 255);
            return color;
        }
    }

    public class ColorScheme
    {
        public readonly Color PrimaryColor;

        public readonly Color DarkPrimaryColor;

        public readonly Color LightPrimaryColor;

        public readonly Color AccentColor;

        public readonly Color TextColor;

        public readonly Pen PrimaryPen;

        public readonly Pen DarkPrimaryPen;

        public readonly Pen LightPrimaryPen;

        public readonly Pen AccentPen;

        public readonly Pen TextPen;

        public readonly Brush PrimaryBrush;

        public readonly Brush DarkPrimaryBrush;

        public readonly Brush LightPrimaryBrush;

        public readonly Brush AccentBrush;

        public readonly Brush TextBrush;

        public ColorScheme(Primary primary, Primary darkPrimary, Primary lightPrimary, Accent accent, TextShade textShade)
        {
            this.PrimaryColor = ((int)primary).ToColor();
            this.DarkPrimaryColor = ((int)darkPrimary).ToColor();
            this.LightPrimaryColor = ((int)lightPrimary).ToColor();
            this.AccentColor = ((int)accent).ToColor();
            this.TextColor = ((int)textShade).ToColor();
            this.PrimaryPen = new Pen(this.PrimaryColor);
            this.DarkPrimaryPen = new Pen(this.DarkPrimaryColor);
            this.LightPrimaryPen = new Pen(this.LightPrimaryColor);
            this.AccentPen = new Pen(this.AccentColor);
            this.TextPen = new Pen(this.TextColor);
            this.PrimaryBrush = new SolidBrush(this.PrimaryColor);
            this.DarkPrimaryBrush = new SolidBrush(this.DarkPrimaryColor);
            this.LightPrimaryBrush = new SolidBrush(this.LightPrimaryColor);
            this.AccentBrush = new SolidBrush(this.AccentColor);
            this.TextBrush = new SolidBrush(this.TextColor);
        }
    }

    internal static class DrawHelper
    {
        public static Color BlendColor(Color backgroundColor, Color frontColor, double blend)
        {
            double num = blend / 255;
            double num1 = 1 - num;
            int r = (int)((double)backgroundColor.R * num1 + (double)frontColor.R * num);
            int g = (int)((double)backgroundColor.G * num1 + (double)frontColor.G * num);
            int b = (int)((double)backgroundColor.B * num1 + (double)frontColor.B * num);
            return Color.FromArgb(r, g, b);
        }

        public static Color BlendColor(Color backgroundColor, Color frontColor)
        {
            return DrawHelper.BlendColor(backgroundColor, frontColor, (double)frontColor.A);
        }

        public static GraphicsPath CreateRoundRect(float x, float y, float width, float height, float radius)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddLine(x + radius, y, x + width - radius * 2f, y);
            graphicsPath.AddArc(x + width - radius * 2f, y, radius * 2f, radius * 2f, 270f, 90f);
            graphicsPath.AddLine(x + width, y + radius, x + width, y + height - radius * 2f);
            graphicsPath.AddArc(x + width - radius * 2f, y + height - radius * 2f, radius * 2f, radius * 2f, 0f, 90f);
            graphicsPath.AddLine(x + width - radius * 2f, y + height, x + radius, y + height);
            graphicsPath.AddArc(x, y + height - radius * 2f, radius * 2f, radius * 2f, 90f, 90f);
            graphicsPath.AddLine(x, y + height - radius * 2f, x, y + radius);
            graphicsPath.AddArc(x, y, radius * 2f, radius * 2f, 180f, 90f);
            graphicsPath.CloseFigure();
            return graphicsPath;
        }

        public static GraphicsPath CreateRoundRect(Rectangle rect, float radius)
        {
            GraphicsPath graphicsPath = DrawHelper.CreateRoundRect((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height, radius);
            return graphicsPath;
        }
    }

    public interface IMaterialControl
    {
        int Depth
        {
            get;
            set;
        }

        MouseState MouseState
        {
            get;
            set;
        }

        MaterialSkinManager SkinManager
        {
            get;
        }
    }

    public class MaterialSkinManager
    {
        private static MaterialSkinManager _instance;

        private readonly List<MaterialForm> _formsToManage = new List<MaterialForm>();

        private MaterialSkinManager.Themes _theme;

        private ColorScheme _colorScheme;

        private readonly static Color PRIMARY_TEXT_BLACK;

        private readonly static Brush PRIMARY_TEXT_BLACK_BRUSH;

        public static Color SECONDARY_TEXT_BLACK;

        public static Brush SECONDARY_TEXT_BLACK_BRUSH;

        private readonly static Color DISABLED_OR_HINT_TEXT_BLACK;

        private readonly static Brush DISABLED_OR_HINT_TEXT_BLACK_BRUSH;

        private readonly static Color DIVIDERS_BLACK;

        private readonly static Brush DIVIDERS_BLACK_BRUSH;

        private readonly static Color PRIMARY_TEXT_WHITE;

        private readonly static Brush PRIMARY_TEXT_WHITE_BRUSH;

        public static Color SECONDARY_TEXT_WHITE;

        public static Brush SECONDARY_TEXT_WHITE_BRUSH;

        private readonly static Color DISABLED_OR_HINT_TEXT_WHITE;

        private readonly static Brush DISABLED_OR_HINT_TEXT_WHITE_BRUSH;

        private readonly static Color DIVIDERS_WHITE;

        private readonly static Brush DIVIDERS_WHITE_BRUSH;

        private readonly static Color CHECKBOX_OFF_LIGHT;

        private readonly static Brush CHECKBOX_OFF_LIGHT_BRUSH;

        private readonly static Color CHECKBOX_OFF_DISABLED_LIGHT;

        private readonly static Brush CHECKBOX_OFF_DISABLED_LIGHT_BRUSH;

        private readonly static Color CHECKBOX_OFF_DARK;

        private readonly static Brush CHECKBOX_OFF_DARK_BRUSH;

        private readonly static Color CHECKBOX_OFF_DISABLED_DARK;

        private readonly static Brush CHECKBOX_OFF_DISABLED_DARK_BRUSH;

        private readonly static Color RAISED_BUTTON_BACKGROUND;

        private readonly static Brush RAISED_BUTTON_BACKGROUND_BRUSH;

        private readonly static Color RAISED_BUTTON_TEXT_LIGHT;

        private readonly static Brush RAISED_BUTTON_TEXT_LIGHT_BRUSH;

        private readonly static Color RAISED_BUTTON_TEXT_DARK;

        private readonly static Brush RAISED_BUTTON_TEXT_DARK_BRUSH;

        private readonly static Color FLAT_BUTTON_BACKGROUND_HOVER_LIGHT;

        private readonly static Brush FLAT_BUTTON_BACKGROUND_HOVER_LIGHT_BRUSH;

        private readonly static Color FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT;

        private readonly static Brush FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT_BRUSH;

        private readonly static Color FLAT_BUTTON_DISABLEDTEXT_LIGHT;

        private readonly static Brush FLAT_BUTTON_DISABLEDTEXT_LIGHT_BRUSH;

        private readonly static Color FLAT_BUTTON_BACKGROUND_HOVER_DARK;

        private readonly static Brush FLAT_BUTTON_BACKGROUND_HOVER_DARK_BRUSH;

        private readonly static Color FLAT_BUTTON_BACKGROUND_PRESSED_DARK;

        private readonly static Brush FLAT_BUTTON_BACKGROUND_PRESSED_DARK_BRUSH;

        private readonly static Color FLAT_BUTTON_DISABLEDTEXT_DARK;

        private readonly static Brush FLAT_BUTTON_DISABLEDTEXT_DARK_BRUSH;

        private readonly static Color CMS_BACKGROUND_LIGHT_HOVER;

        private readonly static Brush CMS_BACKGROUND_HOVER_LIGHT_BRUSH;

        private readonly static Color CMS_BACKGROUND_DARK_HOVER;

        private readonly static Brush CMS_BACKGROUND_HOVER_DARK_BRUSH;

        private readonly static Color BACKGROUND_LIGHT;

        private static Brush BACKGROUND_LIGHT_BRUSH;

        private readonly static Color BACKGROUND_DARK;

        private static Brush BACKGROUND_DARK_BRUSH;

        public readonly Color ACTION_BAR_TEXT = Color.FromArgb(255, 255, 255, 255);

        public readonly Brush ACTION_BAR_TEXT_BRUSH = new SolidBrush(Color.FromArgb(255, 255, 255, 255));

        public readonly Color ACTION_BAR_TEXT_SECONDARY = Color.FromArgb(153, 255, 255, 255);

        public readonly Brush ACTION_BAR_TEXT_SECONDARY_BRUSH = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

        public Font ROBOTO_MEDIUM_12;

        public Font ROBOTO_REGULAR_11;

        public Font ROBOTO_MEDIUM_11;

        public Font ROBOTO_MEDIUM_10;

        public int FORM_PADDING = 14;

        private readonly PrivateFontCollection privateFontCollection = new PrivateFontCollection();

        public ColorScheme ColorScheme
        {
            get
            {
                return this._colorScheme;
            }
            set
            {
                this._colorScheme = value;
                this.UpdateBackgrounds();
            }
        }

        public static MaterialSkinManager Instance
        {
            get
            {
                MaterialSkinManager materialSkinManager = MaterialSkinManager._instance;
                if (materialSkinManager == null)
                {
                    materialSkinManager = new MaterialSkinManager();
                    MaterialSkinManager._instance = materialSkinManager;
                }
                return materialSkinManager;
            }
        }

        public MaterialSkinManager.Themes Theme
        {
            get
            {
                return this._theme;
            }
            set
            {
                this._theme = value;
                this.UpdateBackgrounds();
            }
        }

        static MaterialSkinManager()
        {
            MaterialSkinManager.PRIMARY_TEXT_BLACK = Color.FromArgb(222, 0, 0, 0);
            MaterialSkinManager.PRIMARY_TEXT_BLACK_BRUSH = new SolidBrush(MaterialSkinManager.PRIMARY_TEXT_BLACK);
            MaterialSkinManager.SECONDARY_TEXT_BLACK = Color.FromArgb(138, 0, 0, 0);
            MaterialSkinManager.SECONDARY_TEXT_BLACK_BRUSH = new SolidBrush(MaterialSkinManager.SECONDARY_TEXT_BLACK);
            MaterialSkinManager.DISABLED_OR_HINT_TEXT_BLACK = Color.FromArgb(66, 0, 0, 0);
            MaterialSkinManager.DISABLED_OR_HINT_TEXT_BLACK_BRUSH = new SolidBrush(MaterialSkinManager.DISABLED_OR_HINT_TEXT_BLACK);
            MaterialSkinManager.DIVIDERS_BLACK = Color.FromArgb(31, 0, 0, 0);
            MaterialSkinManager.DIVIDERS_BLACK_BRUSH = new SolidBrush(MaterialSkinManager.DIVIDERS_BLACK);
            MaterialSkinManager.PRIMARY_TEXT_WHITE = Color.FromArgb(255, 255, 255, 255);
            MaterialSkinManager.PRIMARY_TEXT_WHITE_BRUSH = new SolidBrush(MaterialSkinManager.PRIMARY_TEXT_WHITE);
            MaterialSkinManager.SECONDARY_TEXT_WHITE = Color.FromArgb(179, 255, 255, 255);
            MaterialSkinManager.SECONDARY_TEXT_WHITE_BRUSH = new SolidBrush(MaterialSkinManager.SECONDARY_TEXT_WHITE);
            MaterialSkinManager.DISABLED_OR_HINT_TEXT_WHITE = Color.FromArgb(77, 255, 255, 255);
            MaterialSkinManager.DISABLED_OR_HINT_TEXT_WHITE_BRUSH = new SolidBrush(MaterialSkinManager.DISABLED_OR_HINT_TEXT_WHITE);
            MaterialSkinManager.DIVIDERS_WHITE = Color.FromArgb(31, 255, 255, 255);
            MaterialSkinManager.DIVIDERS_WHITE_BRUSH = new SolidBrush(MaterialSkinManager.DIVIDERS_WHITE);
            MaterialSkinManager.CHECKBOX_OFF_LIGHT = Color.FromArgb(138, 0, 0, 0);
            MaterialSkinManager.CHECKBOX_OFF_LIGHT_BRUSH = new SolidBrush(MaterialSkinManager.CHECKBOX_OFF_LIGHT);
            MaterialSkinManager.CHECKBOX_OFF_DISABLED_LIGHT = Color.FromArgb(66, 0, 0, 0);
            MaterialSkinManager.CHECKBOX_OFF_DISABLED_LIGHT_BRUSH = new SolidBrush(MaterialSkinManager.CHECKBOX_OFF_DISABLED_LIGHT);
            MaterialSkinManager.CHECKBOX_OFF_DARK = Color.FromArgb(179, 255, 255, 255);
            MaterialSkinManager.CHECKBOX_OFF_DARK_BRUSH = new SolidBrush(MaterialSkinManager.CHECKBOX_OFF_DARK);
            MaterialSkinManager.CHECKBOX_OFF_DISABLED_DARK = Color.FromArgb(77, 255, 255, 255);
            MaterialSkinManager.CHECKBOX_OFF_DISABLED_DARK_BRUSH = new SolidBrush(MaterialSkinManager.CHECKBOX_OFF_DISABLED_DARK);
            MaterialSkinManager.RAISED_BUTTON_BACKGROUND = Color.FromArgb(255, 255, 255, 255);
            MaterialSkinManager.RAISED_BUTTON_BACKGROUND_BRUSH = new SolidBrush(MaterialSkinManager.RAISED_BUTTON_BACKGROUND);
            MaterialSkinManager.RAISED_BUTTON_TEXT_LIGHT = MaterialSkinManager.PRIMARY_TEXT_WHITE;
            MaterialSkinManager.RAISED_BUTTON_TEXT_LIGHT_BRUSH = new SolidBrush(MaterialSkinManager.RAISED_BUTTON_TEXT_LIGHT);
            MaterialSkinManager.RAISED_BUTTON_TEXT_DARK = MaterialSkinManager.PRIMARY_TEXT_BLACK;
            MaterialSkinManager.RAISED_BUTTON_TEXT_DARK_BRUSH = new SolidBrush(MaterialSkinManager.RAISED_BUTTON_TEXT_DARK);
            MaterialSkinManager.FLAT_BUTTON_BACKGROUND_HOVER_LIGHT = Color.FromArgb(20.PercentageToColorComponent(), 10066329.ToColor());
            MaterialSkinManager.FLAT_BUTTON_BACKGROUND_HOVER_LIGHT_BRUSH = new SolidBrush(MaterialSkinManager.FLAT_BUTTON_BACKGROUND_HOVER_LIGHT);
            MaterialSkinManager.FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT = Color.FromArgb(40.PercentageToColorComponent(), 10066329.ToColor());
            MaterialSkinManager.FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT_BRUSH = new SolidBrush(MaterialSkinManager.FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT);
            MaterialSkinManager.FLAT_BUTTON_DISABLEDTEXT_LIGHT = Color.FromArgb(26.PercentageToColorComponent(), 0.ToColor());
            MaterialSkinManager.FLAT_BUTTON_DISABLEDTEXT_LIGHT_BRUSH = new SolidBrush(MaterialSkinManager.FLAT_BUTTON_DISABLEDTEXT_LIGHT);
            MaterialSkinManager.FLAT_BUTTON_BACKGROUND_HOVER_DARK = Color.FromArgb(15.PercentageToColorComponent(), 13421772.ToColor());
            MaterialSkinManager.FLAT_BUTTON_BACKGROUND_HOVER_DARK_BRUSH = new SolidBrush(MaterialSkinManager.FLAT_BUTTON_BACKGROUND_HOVER_DARK);
            MaterialSkinManager.FLAT_BUTTON_BACKGROUND_PRESSED_DARK = Color.FromArgb(25.PercentageToColorComponent(), 13421772.ToColor());
            MaterialSkinManager.FLAT_BUTTON_BACKGROUND_PRESSED_DARK_BRUSH = new SolidBrush(MaterialSkinManager.FLAT_BUTTON_BACKGROUND_PRESSED_DARK);
            MaterialSkinManager.FLAT_BUTTON_DISABLEDTEXT_DARK = Color.FromArgb(30.PercentageToColorComponent(), 16777215.ToColor());
            MaterialSkinManager.FLAT_BUTTON_DISABLEDTEXT_DARK_BRUSH = new SolidBrush(MaterialSkinManager.FLAT_BUTTON_DISABLEDTEXT_DARK);
            MaterialSkinManager.CMS_BACKGROUND_LIGHT_HOVER = Color.FromArgb(255, 238, 238, 238);
            MaterialSkinManager.CMS_BACKGROUND_HOVER_LIGHT_BRUSH = new SolidBrush(MaterialSkinManager.CMS_BACKGROUND_LIGHT_HOVER);
            MaterialSkinManager.CMS_BACKGROUND_DARK_HOVER = Color.FromArgb(38, 204, 204, 204);
            MaterialSkinManager.CMS_BACKGROUND_HOVER_DARK_BRUSH = new SolidBrush(MaterialSkinManager.CMS_BACKGROUND_DARK_HOVER);
            MaterialSkinManager.BACKGROUND_LIGHT = Color.FromArgb(255, 255, 255, 255);
            MaterialSkinManager.BACKGROUND_LIGHT_BRUSH = new SolidBrush(MaterialSkinManager.BACKGROUND_LIGHT);
            MaterialSkinManager.BACKGROUND_DARK = Color.FromArgb(255, 51, 51, 51);
            MaterialSkinManager.BACKGROUND_DARK_BRUSH = new SolidBrush(MaterialSkinManager.BACKGROUND_DARK);
        }

        private MaterialSkinManager()
        {
            this.ROBOTO_MEDIUM_12 = new Font(this.LoadFont(Resources.Roboto_Medium), 12f);
            this.ROBOTO_MEDIUM_10 = new Font(this.LoadFont(Resources.Roboto_Medium), 10f);
            this.ROBOTO_REGULAR_11 = new Font(this.LoadFont(Resources.Roboto_Regular), 11f);
            this.ROBOTO_MEDIUM_11 = new Font(this.LoadFont(Resources.Roboto_Medium), 11f);
            this.Theme = MaterialSkinManager.Themes.LIGHT;
            this.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        [DllImport("gdi32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pvd, [In] ref uint pcFonts);

        public void AddFormToManage(MaterialForm materialForm)
        {
            this._formsToManage.Add(materialForm);
            this.UpdateBackgrounds();
        }

        public Color GetApplicationBackgroundColor()
        {
            return (this.Theme == MaterialSkinManager.Themes.LIGHT ? MaterialSkinManager.BACKGROUND_LIGHT : MaterialSkinManager.BACKGROUND_DARK);
        }

        public Brush GetCheckboxOffBrush()
        {
            return (this.Theme == MaterialSkinManager.Themes.LIGHT ? MaterialSkinManager.CHECKBOX_OFF_LIGHT_BRUSH : MaterialSkinManager.CHECKBOX_OFF_DARK_BRUSH);
        }

        public Color GetCheckboxOffColor()
        {
            return (this.Theme == MaterialSkinManager.Themes.LIGHT ? MaterialSkinManager.CHECKBOX_OFF_LIGHT : MaterialSkinManager.CHECKBOX_OFF_DARK);
        }

        public Brush GetCheckBoxOffDisabledBrush()
        {
            return (this.Theme == MaterialSkinManager.Themes.LIGHT ? MaterialSkinManager.CHECKBOX_OFF_DISABLED_LIGHT_BRUSH : MaterialSkinManager.CHECKBOX_OFF_DISABLED_DARK_BRUSH);
        }

        public Color GetCheckBoxOffDisabledColor()
        {
            return (this.Theme == MaterialSkinManager.Themes.LIGHT ? MaterialSkinManager.CHECKBOX_OFF_DISABLED_LIGHT : MaterialSkinManager.CHECKBOX_OFF_DISABLED_DARK);
        }

        public Brush GetCmsSelectedItemBrush()
        {
            return (this.Theme == MaterialSkinManager.Themes.LIGHT ? MaterialSkinManager.CMS_BACKGROUND_HOVER_LIGHT_BRUSH : MaterialSkinManager.CMS_BACKGROUND_HOVER_DARK_BRUSH);
        }

        public Brush GetDisabledOrHintBrush()
        {
            return (this.Theme == MaterialSkinManager.Themes.LIGHT ? MaterialSkinManager.DISABLED_OR_HINT_TEXT_BLACK_BRUSH : MaterialSkinManager.DISABLED_OR_HINT_TEXT_WHITE_BRUSH);
        }

        public Color GetDisabledOrHintColor()
        {
            return (this.Theme == MaterialSkinManager.Themes.LIGHT ? MaterialSkinManager.DISABLED_OR_HINT_TEXT_BLACK : MaterialSkinManager.DISABLED_OR_HINT_TEXT_WHITE);
        }

        public Brush GetDividersBrush()
        {
            return (this.Theme == MaterialSkinManager.Themes.LIGHT ? MaterialSkinManager.DIVIDERS_BLACK_BRUSH : MaterialSkinManager.DIVIDERS_WHITE_BRUSH);
        }

        public Color GetDividersColor()
        {
            return (this.Theme == MaterialSkinManager.Themes.LIGHT ? MaterialSkinManager.DIVIDERS_BLACK : MaterialSkinManager.DIVIDERS_WHITE);
        }

        public Brush GetFlatButtonDisabledTextBrush()
        {
            return (this.Theme == MaterialSkinManager.Themes.LIGHT ? MaterialSkinManager.FLAT_BUTTON_DISABLEDTEXT_LIGHT_BRUSH : MaterialSkinManager.FLAT_BUTTON_DISABLEDTEXT_DARK_BRUSH);
        }

        public Brush GetFlatButtonHoverBackgroundBrush()
        {
            return (this.Theme == MaterialSkinManager.Themes.LIGHT ? MaterialSkinManager.FLAT_BUTTON_BACKGROUND_HOVER_LIGHT_BRUSH : MaterialSkinManager.FLAT_BUTTON_BACKGROUND_HOVER_DARK_BRUSH);
        }

        public Color GetFlatButtonHoverBackgroundColor()
        {
            return (this.Theme == MaterialSkinManager.Themes.LIGHT ? MaterialSkinManager.FLAT_BUTTON_BACKGROUND_HOVER_LIGHT : MaterialSkinManager.FLAT_BUTTON_BACKGROUND_HOVER_DARK);
        }

        public Brush GetFlatButtonPressedBackgroundBrush()
        {
            return (this.Theme == MaterialSkinManager.Themes.LIGHT ? MaterialSkinManager.FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT_BRUSH : MaterialSkinManager.FLAT_BUTTON_BACKGROUND_PRESSED_DARK_BRUSH);
        }

        public Color GetFlatButtonPressedBackgroundColor()
        {
            return (this.Theme == MaterialSkinManager.Themes.LIGHT ? MaterialSkinManager.FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT : MaterialSkinManager.FLAT_BUTTON_BACKGROUND_PRESSED_DARK);
        }

        public Brush GetPrimaryTextBrush()
        {
            return (this.Theme == MaterialSkinManager.Themes.LIGHT ? MaterialSkinManager.PRIMARY_TEXT_BLACK_BRUSH : MaterialSkinManager.PRIMARY_TEXT_WHITE_BRUSH);
        }

        public Color GetPrimaryTextColor()
        {
            return (this.Theme == MaterialSkinManager.Themes.LIGHT ? MaterialSkinManager.PRIMARY_TEXT_BLACK : MaterialSkinManager.PRIMARY_TEXT_WHITE);
        }

        public Brush GetRaisedButtonBackgroundBrush()
        {
            return MaterialSkinManager.RAISED_BUTTON_BACKGROUND_BRUSH;
        }

        public Brush GetRaisedButtonTextBrush(bool primary)
        {
            return (primary ? MaterialSkinManager.RAISED_BUTTON_TEXT_LIGHT_BRUSH : MaterialSkinManager.RAISED_BUTTON_TEXT_DARK_BRUSH);
        }

        public Brush GetSecondaryTextBrush()
        {
            return (this.Theme == MaterialSkinManager.Themes.LIGHT ? MaterialSkinManager.SECONDARY_TEXT_BLACK_BRUSH : MaterialSkinManager.SECONDARY_TEXT_WHITE_BRUSH);
        }

        public Color GetSecondaryTextColor()
        {
            return (this.Theme == MaterialSkinManager.Themes.LIGHT ? MaterialSkinManager.SECONDARY_TEXT_BLACK : MaterialSkinManager.SECONDARY_TEXT_WHITE);
        }

        private FontFamily LoadFont(byte[] fontResource)
        {
            int length = (int)fontResource.Length;
            IntPtr intPtr = Marshal.AllocCoTaskMem(length);
            Marshal.Copy(fontResource, 0, intPtr, length);
            uint num = 0;
            MaterialSkinManager.AddFontMemResourceEx(intPtr, (uint)fontResource.Length, IntPtr.Zero, ref num);
            this.privateFontCollection.AddMemoryFont(intPtr, length);
            return this.privateFontCollection.Families.Last<FontFamily>();
        }

        public void RemoveFormToManage(MaterialForm materialForm)
        {
            this._formsToManage.Remove(materialForm);
        }

        private void UpdateBackgrounds()
        {
            Color applicationBackgroundColor = this.GetApplicationBackgroundColor();
            foreach (MaterialForm materialForm in this._formsToManage)
            {
                materialForm.BackColor = applicationBackgroundColor;
                this.UpdateControl(materialForm, applicationBackgroundColor);
            }
        }

        private void UpdateControl(Control controlToUpdate, Color newBackColor)
        {
            if (controlToUpdate != null)
            {
                if (controlToUpdate.ContextMenuStrip != null)
                {
                    this.UpdateToolStrip(controlToUpdate.ContextMenuStrip, newBackColor);
                }
                MaterialTabControl materialTabControl = controlToUpdate as MaterialTabControl;
                if (materialTabControl != null)
                {
                    foreach (TabPage tabPage in materialTabControl.TabPages)
                    {
                        tabPage.BackColor = newBackColor;
                    }
                }
                if (controlToUpdate is MaterialDivider)
                {
                    controlToUpdate.BackColor = this.GetDividersColor();
                }
                if (controlToUpdate is MaterialListView)
                {
                    controlToUpdate.BackColor = newBackColor;
                }
                foreach (Control control in controlToUpdate.Controls)
                {
                    this.UpdateControl(control, newBackColor);
                }
                controlToUpdate.Invalidate();
            }
        }

        private void UpdateToolStrip(ToolStrip toolStrip, Color newBackColor)
        {
            if (toolStrip != null)
            {
                toolStrip.BackColor = newBackColor;
                foreach (ToolStripItem item in toolStrip.Items)
                {
                    item.BackColor = newBackColor;
                    if ((!(item is MaterialToolStripMenuItem) ? false : (item as MaterialToolStripMenuItem).HasDropDown))
                    {
                        this.UpdateToolStrip((item as MaterialToolStripMenuItem).DropDown, newBackColor);
                    }
                }
            }
        }

        public enum Themes : byte
        {
            LIGHT,
            DARK
        }
    }

    public enum MouseState
    {
        HOVER,
        DOWN,
        OUT
    }

    public enum Primary
    {
        Teal900 = 19776,
        Cyan900 = 24676,
        Teal800 = 26972,
        Teal700 = 31083,
        Cyan800 = 33679,
        Teal600 = 35195,
        Teal500 = 38536,
        Cyan700 = 38823,
        Cyan600 = 44225,
        Cyan500 = 48340,
        LightBlue900 = 87963,
        LightBlue800 = 161725,
        LightBlue700 = 166097,
        LightBlue600 = 236517,
        LightBlue500 = 240116,
        Blue900 = 870305,
        Blue800 = 1402304,
        Blue700 = 1668818,
        Indigo900 = 1713022,
        Green900 = 1793568,
        Blue600 = 2001125,
        Grey900 = 2171169,
        Blue500 = 2201331,
        BlueGrey900 = 2503224,
        Teal400 = 2533018,
        Cyan400 = 2541274,
        Indigo800 = 2635155,
        LightBlue400 = 2733814,
        Green800 = 3046706,
        Indigo700 = 3162015,
        DeepPurple900 = 3218322,
        LightGreen900 = 3369246,
        BlueGrey800 = 3622735,
        Green700 = 3706428,
        Indigo600 = 3754411,
        Brown900 = 4073251,
        Indigo500 = 4149685,
        Grey800 = 4342338,
        Blue400 = 4367861,
        Green600 = 4431943,
        DeepPurple800 = 4532128,
        BlueGrey700 = 4545124,
        Purple900 = 4854924,
        Green500 = 5025616,
        Teal300 = 5093036,
        Cyan300 = 5099745,
        Brown800 = 5125166,
        LightBlue300 = 5227511,
        DeepPurple700 = 5320104,
        BlueGrey600 = 5533306,
        LightGreen800 = 5606191,
        Indigo400 = 6056896,
        Brown700 = 6111287,
        DeepPurple600 = 6174129,
        BlueGrey500 = 6323595,
        Grey700 = 6381921,
        Blue300 = 6600182,
        Green400 = 6732650,
        DeepPurple500 = 6765239,
        LightGreen700 = 6856504,
        Purple800 = 6953882,
        Brown600 = 7162945,
        Grey600 = 7697781,
        BlueGrey400 = 7901340,
        Brown500 = 7951688,
        Indigo300 = 7964363,
        Purple700 = 8069026,
        LightGreen600 = 8172354,
        DeepPurple400 = 8280002,
        Teal200 = 8440772,
        Cyan200 = 8445674,
        Green300 = 8505220,
        LightBlue200 = 8508666,
        Lime900 = 8550167,
        Pink900 = 8916559,
        LightGreen500 = 9159498,
        Brown400 = 9268835,
        Purple600 = 9315498,
        BlueGrey300 = 9479342,
        Blue200 = 9489145,
        DeepPurple300 = 9795021,
        Purple500 = 10233776,
        LightGreen400 = 10275941,
        Lime800 = 10394916,
        Grey500 = 10395294,
        Indigo200 = 10463450,
        Brown300 = 10586239,
        Green200 = 10868391,
        Purple400 = 11225020,
        Pink800 = 11342935,
        LightGreen300 = 11457921,
        Lime700 = 11514923,
        BlueGrey200 = 11583173,
        Teal100 = 11722715,
        Cyan100 = 11725810,
        DeepPurple200 = 11771355,
        LightBlue100 = 11789820,
        Red900 = 12000284,
        Purple300 = 12216520,
        Blue100 = 12312315,
        Brown200 = 12364452,
        Grey400 = 12434877,
        DeepOrange900 = 12531212,
        Lime600 = 12634675,
        Pink700 = 12720219,
        Indigo100 = 12962537,
        LightGreen200 = 12968357,
        Red800 = 12986408,
        Green100 = 13166281,
        Lime500 = 13491257,
        Purple200 = 13538264,
        BlueGrey100 = 13621468,
        DeepPurple100 = 13747433,
        Red700 = 13840175,
        Lime400 = 13951319,
        Brown100 = 14142664,
        Pink600 = 14162784,
        DeepOrange800 = 14172949,
        Lime300 = 14477173,
        LightGreen100 = 14478792,
        Grey300 = 14737632,
        Teal50 = 14742257,
        Cyan50 = 14743546,
        Purple100 = 14794471,
        LightBlue50 = 14808574,
        Blue50 = 14938877,
        Red600 = 15022389,
        Red300 = 15037299,
        DeepOrange700 = 15092249,
        Orange900 = 15094016,
        Lime200 = 15134364,
        Indigo50 = 15264502,
        Green50 = 15267305,
        Pink500 = 15277667,
        Pink400 = 15483002,
        BlueGrey50 = 15527921,
        DeepPurple50 = 15591414,
        Grey200 = 15658734,
        Red400 = 15684432,
        Orange800 = 15690752,
        Red200 = 15702682,
        Brown50 = 15723497,
        Pink300 = 15753874,
        Lime100 = 15791299,
        LightGreen50 = 15857897,
        Purple50 = 15984117,
        Red500 = 16007990,
        DeepOrange600 = 16011550,
        Pink200 = 16027569,
        Orange700 = 16088064,
        Yellow900 = 16088855,
        Grey100 = 16119285,
        Pink100 = 16301008,
        Yellow800 = 16361509,
        Lime50 = 16382951,
        Grey50 = 16448250,
        Orange600 = 16485376,
        Yellow700 = 16498733,
        DeepOrange50 = 16509415,
        Pink50 = 16573676,
        Yellow600 = 16635957,
        DeepOrange500 = 16733986,
        Amber900 = 16740096,
        DeepOrange400 = 16740419,
        DeepOrange300 = 16747109,
        Amber800 = 16748288,
        Orange500 = 16750592,
        Amber700 = 16752640,
        Orange400 = 16754470,
        DeepOrange200 = 16755601,
        Amber600 = 16757504,
        Orange300 = 16758605,
        Amber500 = 16761095,
        Amber400 = 16763432,
        Orange200 = 16764032,
        DeepOrange100 = 16764092,
        Red100 = 16764370,
        Amber300 = 16766287,
        Amber200 = 16769154,
        Orange100 = 16769202,
        Yellow500 = 16771899,
        Red50 = 16772078,
        Amber100 = 16772275,
        Yellow400 = 16772696,
        Yellow300 = 16773494,
        Orange50 = 16774112,
        Yellow200 = 16774557,
        Amber50 = 16775393,
        Yellow100 = 16775620,
        Yellow50 = 16776679
    }

    public enum TextShade
    {
        BLACK = 2171169,
        WHITE = 16777215
    }

    public static class AnimationCustomQuadratic
    {
        public static double CalculateProgress(double progress)
        {
            double num = 0.6;
            double num1 = 1 - Math.Cos((Math.Max(progress, num) - num) * 3.14159265358979 / (2 - 2 * num));
            return num1;
        }
    }

    internal enum AnimationDirection
    {
        In,
        Out,
        InOutIn,
        InOutOut,
        InOutRepeatingIn,
        InOutRepeatingOut
    }
        
    internal static class AnimationEaseInOut
    {
        public static double PI;

        public static double PI_HALF;

        static AnimationEaseInOut()
        {
            AnimationEaseInOut.PI = 3.14159265358979;
            AnimationEaseInOut.PI_HALF = 1.5707963267949;
        }

        public static double CalculateProgress(double progress)
        {
            return AnimationEaseInOut.EaseInOut(progress);
        }

        private static double EaseInOut(double s)
        {
            double num = s - Math.Sin(s * 2 * AnimationEaseInOut.PI) / (2 * AnimationEaseInOut.PI);
            return num;
        }
    }

    public static class AnimationEaseOut
    {
        public static double CalculateProgress(double progress)
        {
            return -1 * progress * (progress - 2);
        }
    }

    internal static class AnimationLinear
    {
        public static double CalculateProgress(double progress)
        {
            return progress;
        }
    }

    internal class AnimationManager
    {
        private readonly List<double> _animationProgresses;

        private readonly List<Point> _animationSources;

        private readonly List<AnimationDirection> _animationDirections;

        private readonly List<object[]> _animationDatas;

        private const double MIN_VALUE = 0;

        private const double MAX_VALUE = 1;

        private readonly Timer _animationTimer = new Timer()
        {
            Interval = 5,
            Enabled = false
        };

        public AnimationType AnimationType
        {
            get;
            set;
        }

        public double Increment
        {
            get;
            set;
        }

        public bool InterruptAnimation
        {
            get;
            set;
        }

        public double SecondaryIncrement
        {
            get;
            set;
        }

        public bool Singular
        {
            get;
            set;
        }

        public AnimationManager(bool singular = true)
        {
            this._animationProgresses = new List<double>();
            this._animationSources = new List<Point>();
            this._animationDirections = new List<AnimationDirection>();
            this._animationDatas = new List<object[]>();
            this.Increment = 0.03;
            this.SecondaryIncrement = 0.03;
            this.AnimationType = AnimationType.Linear;
            this.InterruptAnimation = true;
            this.Singular = singular;
            if (this.Singular)
            {
                this._animationProgresses.Add(0);
                this._animationSources.Add(new Point(0, 0));
                this._animationDirections.Add(AnimationDirection.In);
            }
            this._animationTimer.Tick += new EventHandler(this.AnimationTimerOnTick);
        }

        private void AnimationTimerOnTick(object sender, EventArgs eventArgs)
        {
            bool flag;
            for (int i = 0; i < this._animationProgresses.Count; i++)
            {
                this.UpdateProgress(i);
                if (!this.Singular)
                {
                    if ((this._animationDirections[i] != AnimationDirection.InOutIn ? false : this._animationProgresses[i] == 1))
                    {
                        this._animationDirections[i] = AnimationDirection.InOutOut;
                    }
                    else if ((this._animationDirections[i] != AnimationDirection.InOutRepeatingIn ? false : this._animationProgresses[i] == 0))
                    {
                        this._animationDirections[i] = AnimationDirection.InOutRepeatingOut;
                    }
                    else if ((this._animationDirections[i] != AnimationDirection.InOutRepeatingOut ? true : this._animationProgresses[i] != 0))
                    {
                        if ((this._animationDirections[i] != AnimationDirection.In || this._animationProgresses[i] != 1) && (this._animationDirections[i] != AnimationDirection.Out || this._animationProgresses[i] != 0))
                        {
                            flag = (this._animationDirections[i] != AnimationDirection.InOutOut ? false : this._animationProgresses[i] == 0);
                        }
                        else
                        {
                            flag = true;
                        }
                        if (flag)
                        {
                            this._animationProgresses.RemoveAt(i);
                            this._animationSources.RemoveAt(i);
                            this._animationDirections.RemoveAt(i);
                            this._animationDatas.RemoveAt(i);
                        }
                    }
                    else
                    {
                        this._animationDirections[i] = AnimationDirection.InOutRepeatingIn;
                    }
                }
                else if ((this._animationDirections[i] != AnimationDirection.InOutIn ? false : this._animationProgresses[i] == 1))
                {
                    this._animationDirections[i] = AnimationDirection.InOutOut;
                }
                else if ((this._animationDirections[i] != AnimationDirection.InOutRepeatingIn ? false : this._animationProgresses[i] == 1))
                {
                    this._animationDirections[i] = AnimationDirection.InOutRepeatingOut;
                }
                else if ((this._animationDirections[i] != AnimationDirection.InOutRepeatingOut ? false : this._animationProgresses[i] == 0))
                {
                    this._animationDirections[i] = AnimationDirection.InOutRepeatingIn;
                }
            }
            AnimationManager.AnimationProgress animationProgress = this.OnAnimationProgress;
            if (animationProgress != null)
            {
                animationProgress(this);
            }
            else
            {
            }
        }

        private void DecrementProgress(int index)
        {
            List<double> item = this._animationProgresses;
            int num = index;
            item[num] = item[num] - (this._animationDirections[index] == AnimationDirection.InOutOut || this._animationDirections[index] == AnimationDirection.InOutRepeatingOut ? this.SecondaryIncrement : this.Increment);
            if (this._animationProgresses[index] < 0)
            {
                this._animationProgresses[index] = 0;
                int num1 = 0;
                while (num1 < this.GetAnimationCount())
                {
                    if (this._animationDirections[num1] == AnimationDirection.InOutIn)
                    {
                        return;
                    }
                    else if (this._animationDirections[num1] == AnimationDirection.InOutRepeatingIn)
                    {
                        return;
                    }
                    else if (this._animationDirections[num1] == AnimationDirection.InOutRepeatingOut)
                    {
                        return;
                    }
                    else if ((this._animationDirections[num1] != AnimationDirection.InOutOut ? false : this._animationProgresses[num1] != 0))
                    {
                        return;
                    }
                    else if ((this._animationDirections[num1] != AnimationDirection.Out ? true : this._animationProgresses[num1] == 0))
                    {
                        num1++;
                    }
                    else
                    {
                        return;
                    }
                }
                this._animationTimer.Stop();
                AnimationManager.AnimationFinished animationFinished = this.OnAnimationFinished;
                if (animationFinished != null)
                {
                    animationFinished(this);
                }
                else
                {
                }
            }
        }

        public int GetAnimationCount()
        {
            return this._animationProgresses.Count;
        }

        public object[] GetData()
        {
            if (!this.Singular)
            {
                throw new Exception("Animation is not set to Singular.");
            }
            if (this._animationDatas.Count == 0)
            {
                throw new Exception("Invalid animation");
            }
            return this._animationDatas[0];
        }

        public object[] GetData(int index)
        {
            if (index >= this._animationDatas.Count)
            {
                throw new IndexOutOfRangeException("Invalid animation index");
            }
            return this._animationDatas[index];
        }

        public AnimationDirection GetDirection()
        {
            if (!this.Singular)
            {
                throw new Exception("Animation is not set to Singular.");
            }
            if (this._animationDirections.Count == 0)
            {
                throw new Exception("Invalid animation");
            }
            return this._animationDirections[0];
        }

        public AnimationDirection GetDirection(int index)
        {
            if (index >= this._animationDirections.Count)
            {
                throw new IndexOutOfRangeException("Invalid animation index");
            }
            return this._animationDirections[index];
        }

        public double GetProgress()
        {
            if (!this.Singular)
            {
                throw new Exception("Animation is not set to Singular.");
            }
            if (this._animationProgresses.Count == 0)
            {
                throw new Exception("Invalid animation");
            }
            return this.GetProgress(0);
        }

        public double GetProgress(int index)
        {
            double num;
            if (index >= this.GetAnimationCount())
            {
                throw new IndexOutOfRangeException("Invalid animation index");
            }
            switch (this.AnimationType)
            {
                case AnimationType.Linear:
                    {
                        num = AnimationLinear.CalculateProgress(this._animationProgresses[index]);
                        break;
                    }
                case AnimationType.EaseInOut:
                    {
                        num = AnimationEaseInOut.CalculateProgress(this._animationProgresses[index]);
                        break;
                    }
                case AnimationType.EaseOut:
                    {
                        num = AnimationEaseOut.CalculateProgress(this._animationProgresses[index]);
                        break;
                    }
                case AnimationType.CustomQuadratic:
                    {
                        num = AnimationCustomQuadratic.CalculateProgress(this._animationProgresses[index]);
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException("The given AnimationType is not implemented");
                    }
            }
            return num;
        }

        public Point GetSource(int index)
        {
            if (index >= this.GetAnimationCount())
            {
                throw new IndexOutOfRangeException("Invalid animation index");
            }
            return this._animationSources[index];
        }

        public Point GetSource()
        {
            if (!this.Singular)
            {
                throw new Exception("Animation is not set to Singular.");
            }
            if (this._animationSources.Count == 0)
            {
                throw new Exception("Invalid animation");
            }
            return this._animationSources[0];
        }

        private void IncrementProgress(int index)
        {
            List<double> item = this._animationProgresses;
            int num = index;
            item[num] = item[num] + this.Increment;
            if (this._animationProgresses[index] > 1)
            {
                this._animationProgresses[index] = 1;
                int num1 = 0;
                while (num1 < this.GetAnimationCount())
                {
                    if (this._animationDirections[num1] == AnimationDirection.InOutIn)
                    {
                        return;
                    }
                    else if (this._animationDirections[num1] == AnimationDirection.InOutRepeatingIn)
                    {
                        return;
                    }
                    else if (this._animationDirections[num1] == AnimationDirection.InOutRepeatingOut)
                    {
                        return;
                    }
                    else if ((this._animationDirections[num1] != AnimationDirection.InOutOut ? false : this._animationProgresses[num1] != 1))
                    {
                        return;
                    }
                    else if ((this._animationDirections[num1] != AnimationDirection.In ? true : this._animationProgresses[num1] == 1))
                    {
                        num1++;
                    }
                    else
                    {
                        return;
                    }
                }
                this._animationTimer.Stop();
                AnimationManager.AnimationFinished animationFinished = this.OnAnimationFinished;
                if (animationFinished != null)
                {
                    animationFinished(this);
                }
                else
                {
                }
            }
        }

        public bool IsAnimating()
        {
            return this._animationTimer.Enabled;
        }

        public void SetData(object[] data)
        {
            if (!this.Singular)
            {
                throw new Exception("Animation is not set to Singular.");
            }
            if (this._animationDatas.Count == 0)
            {
                throw new Exception("Invalid animation");
            }
            this._animationDatas[0] = data;
        }

        public void SetDirection(AnimationDirection direction)
        {
            if (!this.Singular)
            {
                throw new Exception("Animation is not set to Singular.");
            }
            if (this._animationProgresses.Count == 0)
            {
                throw new Exception("Invalid animation");
            }
            this._animationDirections[0] = direction;
        }

        public void SetProgress(double progress)
        {
            if (!this.Singular)
            {
                throw new Exception("Animation is not set to Singular.");
            }
            if (this._animationProgresses.Count == 0)
            {
                throw new Exception("Invalid animation");
            }
            this._animationProgresses[0] = progress;
        }

        public void StartNewAnimation(AnimationDirection animationDirection, object[] data = null)
        {
            this.StartNewAnimation(animationDirection, new Point(0, 0), data);
        }

        public void StartNewAnimation(AnimationDirection animationDirection, Point animationSource, object[] data = null)
        {
            if ((!this.IsAnimating() ? true : this.InterruptAnimation))
            {
                if ((!this.Singular ? true : this._animationDirections.Count <= 0))
                {
                    this._animationDirections.Add(animationDirection);
                }
                else
                {
                    this._animationDirections[0] = animationDirection;
                }
                if ((!this.Singular ? true : this._animationSources.Count <= 0))
                {
                    this._animationSources.Add(animationSource);
                }
                else
                {
                    this._animationSources[0] = animationSource;
                }
                if ((!this.Singular ? true : this._animationProgresses.Count <= 0))
                {
                    switch (this._animationDirections[this._animationDirections.Count - 1])
                    {
                        case AnimationDirection.In:
                        case AnimationDirection.InOutIn:
                        case AnimationDirection.InOutRepeatingIn:
                            {
                                this._animationProgresses.Add(0);
                                break;
                            }
                        case AnimationDirection.Out:
                        case AnimationDirection.InOutOut:
                        case AnimationDirection.InOutRepeatingOut:
                            {
                                this._animationProgresses.Add(1);
                                break;
                            }
                        default:
                            {
                                throw new Exception("Invalid AnimationDirection");
                            }
                    }
                }
                if ((!this.Singular ? true : this._animationDatas.Count <= 0))
                {
                    this._animationDatas.Add(data ?? new object[0]);
                }
                else
                {
                    this._animationDatas[0] = data ?? new object[0];
                }
            }
            this._animationTimer.Start();
        }

        public void UpdateProgress(int index)
        {
            switch (this._animationDirections[index])
            {
                case AnimationDirection.In:
                case AnimationDirection.InOutIn:
                case AnimationDirection.InOutRepeatingIn:
                    {
                        this.IncrementProgress(index);
                        break;
                    }
                case AnimationDirection.Out:
                case AnimationDirection.InOutOut:
                case AnimationDirection.InOutRepeatingOut:
                    {
                        this.DecrementProgress(index);
                        break;
                    }
                default:
                    {
                        throw new Exception("No AnimationDirection has been set");
                    }
            }
        }

        public event AnimationManager.AnimationFinished OnAnimationFinished;

        public event AnimationManager.AnimationProgress OnAnimationProgress;

        public delegate void AnimationFinished(object sender);

        public delegate void AnimationProgress(object sender);
    }
    
    internal enum AnimationType
    {
        Linear,
        EaseInOut,
        EaseOut,
        CustomQuadratic
    }



}
