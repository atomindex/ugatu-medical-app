using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace medic {

    public static class FormUtils {

        private static void updateTabIndex(Control parent, ref int tabIndex) {
            for (int i = parent.Controls.Count-1; i >= 0; i--) {
                parent.Controls[i].TabIndex = tabIndex;
                tabIndex++;
                updateTabIndex(parent.Controls[i], ref tabIndex);
            }
        }

        public static int UpdateTabIndex(Control parent, int startIndex = 0) {
            int tabIndex = startIndex;
            updateTabIndex(parent, ref tabIndex);
            return tabIndex;
        }

    }

}
