using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ujicoba
{
    public partial class utama : Form
    {
        public utama(string namauser)
        {
            InitializeComponent();

            if (namauser == "Admin")
            {
                lOGINToolStripMenuItem.Enabled = false;
                dATAToolStripMenuItem.Enabled = false;
                bIAYATAMBAHANToolStripMenuItem.Enabled = false;
            }
            else if (namauser == "kasir")
            {
                lOGINToolStripMenuItem.Enabled = false;
            }
        }

        private void utama_Load(object sender, EventArgs e)
        {

        }

        private void pETUGASToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pELANGGANToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void lAYANANToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void uSERToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void oRDERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Order order = new Order();
            order.Show();
        }

        private void rEPORTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            report r = new report("kodeorder");
            r.ShowDialog();

        }
    }
}
