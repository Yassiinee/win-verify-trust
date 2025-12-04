using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinVerifyTrust
{
    public class ModernGroupBox : GroupBox
    {
        public ModernGroupBox()
        {
            Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            ForeColor = Color.FromArgb(52, 73, 94);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Parent.BackColor);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new(0, 10, Width - 1, Height - 11);
            using (Pen pen = new(Color.FromArgb(189, 195, 199), 2))
            {
                e.Graphics.DrawRectangle(pen, rect);
            }

            SizeF textSize = e.Graphics.MeasureString(Text, Font);
            Rectangle textRect = new(15, 0, (int)textSize.Width + 10, (int)textSize.Height);

            using (SolidBrush brush = new(Parent.BackColor))
            {
                e.Graphics.FillRectangle(brush, textRect);
            }

            using (SolidBrush brush = new(ForeColor))
            {
                e.Graphics.DrawString(Text, Font, brush, 20, 0);
            }
        }
    }
}
