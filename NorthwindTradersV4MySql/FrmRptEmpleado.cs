using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmRptEmpleado : Form
    {
        public FrmRptEmpleado()
        {
            InitializeComponent();
        }

        private void FrmRptEmpleado_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }
    }
}
