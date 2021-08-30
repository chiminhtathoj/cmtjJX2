using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cmtjJX2
{
    public partial class frmMoveTo : Form
    {
        public frmMoveTo()
        {
            InitializeComponent();
        }
        public AutoClientBS CurrentClient { set; get; }
        private bool update = true;
    }
}
