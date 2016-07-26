using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace medic {

    class ButtonRenderer : ToolStripProfessionalRenderer {

        private static Brush hoverColor = new SolidBrush(Color.FromArgb(22, 50, 89));

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e) {
            if (!e.Item.Selected || e.Item.Tag == null) {
                base.OnRenderButtonBackground(e);
            } else {
                Rectangle rectangle = new Rectangle(0, 0, e.Item.Size.Width - 1, e.Item.Size.Height - 1);
                e.Graphics.FillRectangle(hoverColor, rectangle);
            }
        }

    }

}
