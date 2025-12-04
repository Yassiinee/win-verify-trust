using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinVerifyTrust.Custom_Controls
{
    public class ModernButton : Button
    {
        public ModernButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            Cursor = Cursors.Hand;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.FlatAppearance.BorderSize = 2;
            this.FlatAppearance.BorderColor = Color.White;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.FlatAppearance.BorderSize = 0;
        }
    }
}
