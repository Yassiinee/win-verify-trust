using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinVerifyTrust.Custom_Controls
{
    public class GradientPanel : Panel
    {
        public GradientPanel()
        {
            // Enable proper custom painting and prevent flicker
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.ResizeRedraw |
                          ControlStyles.UserPaint, true);

            this.DoubleBuffered = true;
        }

        // Designer-friendly Color property 1
        [Category("Appearance")]
        [Description("First gradient color.")]
        [DefaultValue(typeof(Color), "Blue")]
        [Browsable(true)]
        public Color GradientColor1
        {
            get;
            set
            {
                if (field == value) return;
                field = value;
                Invalidate();
            }
        } = Color.Blue;

        // Designer-friendly Color property 2
        [Category("Appearance")]
        [Description("Second gradient color.")]
        [DefaultValue(typeof(Color), "Cyan")]
        [Browsable(true)]
        public Color GradientColor2
        {
            get;
            set
            {
                if (field == value) return;
                field = value;
                Invalidate();
            }
        } = Color.Cyan;

        // These helpers tell the designer when to serialize the property
        public bool ShouldSerializeGradientColor1()
        {
            return GradientColor1 != Color.Blue;
        }

        public void ResetGradientColor1()
        {
            GradientColor1 = Color.Blue;
        }

        public bool ShouldSerializeGradientColor2()
        {
            return GradientColor2 != Color.Cyan;
        }

        public void ResetGradientColor2()
        {
            GradientColor2 = Color.Cyan;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Draw gradient background manually
            using LinearGradientBrush brush = new(
                this.ClientRectangle,
                GradientColor1,
                GradientColor2,
                LinearGradientMode.Horizontal);
            e.Graphics.FillRectangle(brush, this.ClientRectangle);
        }

        // Keep default OnPaint behaviour (draw children, etc.)
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
    }
}
