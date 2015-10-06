using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Workflow.ComponentModel;
using wxwinter.WFDesigner.DesignerTools;

namespace wxwinter.WFDesigner.DesignerForm
{
    public partial class SeeBindingProperty : Form
    {
        public SeeBindingProperty(Activity activity)
        {
            InitializeComponent();

            List<WFBindPropertyData> bindPropertyList = null;
            bindPropertyList = new List<WFBindPropertyData>();
            WFBindingShow.writeBindPropertyList(activity, ref bindPropertyList);
            this.dgvBindProperty.DataSource = bindPropertyList;

        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
