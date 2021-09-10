using AutoClient;
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

        private void frmMoveTo_Load(object sender, EventArgs e)
        {
            if (CurrentClient != null && update == true)
            {
                this.Text = CurrentClient.CurrentPlayer.EntityNameNoMark.ToString();
                txtX.Text = CurrentClient.toaDoX.ToString();
                txtY.Text = CurrentClient.toaDoY.ToString();
                txtMap.Text = CurrentClient.idMap.ToString();
            }
        }


        private void btnMove_Click(object sender, EventArgs e)
        {
            if (CurrentClient != null)
            {
            
                CurrentClient.MoveTo(Convert.ToInt32(txtX.Text), Convert.ToInt32(txtY.Text), Convert.ToInt32(txtMap.Text));
            }
        }

        private void frmMoveTo_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void tmrMoveTo_Tick(object sender, EventArgs e)
        {
            if (CurrentClient != null)
            {
                if (checkBox1.Checked)
                {
                    CurrentClient.toaDoX = Convert.ToInt32(txtX.Text);
                    CurrentClient.toaDoY = Convert.ToInt32(txtY.Text);
                    CurrentClient.idMap = Convert.ToInt32(txtMap.Text);
                    string entityNameUnicode = CurrentClient.CurrentPlayer.EntityNameUnicode;
                    string text = "//UserData//" + entityNameUnicode.Replace("*", ".") + ".ini";
                    WinAPI.Ghifile(text, "MoveTo", "ValueX", CurrentClient.toaDoX.ToString());
                    WinAPI.Ghifile(text, "MoveTo", "ValueY", CurrentClient.toaDoY.ToString());
                    WinAPI.Ghifile(text, "MoveTo", "ValueMap", CurrentClient.idMap.ToString());
                    WinAPI.Ghifile(text, "MoveTo", "CheckRun", "true");
                    CurrentClient.CheckRun = true;
                }
                else
                {
                    string entityNameUnicode = CurrentClient.CurrentPlayer.EntityNameUnicode;
                    string text = "//UserData//" + entityNameUnicode.Replace("*", ".") + ".ini";
                    WinAPI.Ghifile(text, "MoveTo", "CheckRun", "false");
                    CurrentClient.CheckRun = true;

                }

            }
        }
    }
}
