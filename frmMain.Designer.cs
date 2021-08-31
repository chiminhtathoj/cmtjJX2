
namespace cmtjJX2
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lsvPlayer = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TmrCheckChars = new System.Windows.Forms.Timer(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnFuncMoveTo = new System.Windows.Forms.Button();
            this.cbFuncMoveTo = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lsvPlayer
            // 
            this.lsvPlayer.CheckBoxes = true;
            this.lsvPlayer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.lsvPlayer.FullRowSelect = true;
            this.lsvPlayer.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lsvPlayer.HideSelection = false;
            this.lsvPlayer.Location = new System.Drawing.Point(6, 67);
            this.lsvPlayer.Name = "lsvPlayer";
            this.lsvPlayer.Size = new System.Drawing.Size(339, 325);
            this.lsvPlayer.TabIndex = 0;
            this.lsvPlayer.UseCompatibleStateImageBehavior = false;
            this.lsvPlayer.View = System.Windows.Forms.View.Details;
            this.lsvPlayer.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.lsvPlayer_DrawColumnHeader);
            this.lsvPlayer.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lsvPlayer_ItemCheck);
            this.lsvPlayer.SelectedIndexChanged += new System.EventHandler(this.lsvPlayer_SelectedIndexChanged);
            this.lsvPlayer.Click += new System.EventHandler(this.lsvPlayer_Click);
            this.lsvPlayer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lsvPlayer_MouseUp);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "  ";
            this.columnHeader1.Width = 20;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 77;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "HP";
            this.columnHeader3.Width = 67;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Map";
            this.columnHeader4.Width = 133;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "On ?";
            this.columnHeader5.Width = 36;
            // 
            // TmrCheckChars
            // 
            this.TmrCheckChars.Enabled = true;
            this.TmrCheckChars.Interval = 500;
            this.TmrCheckChars.Tick += new System.EventHandler(this.TmrCheckChars_Tick);
            // 
            // btnFuncMoveTo
            // 
            this.btnFuncMoveTo.Location = new System.Drawing.Point(26, 398);
            this.btnFuncMoveTo.Name = "btnFuncMoveTo";
            this.btnFuncMoveTo.Size = new System.Drawing.Size(75, 23);
            this.btnFuncMoveTo.TabIndex = 1;
            this.btnFuncMoveTo.Text = "MoveTo";
            this.btnFuncMoveTo.UseVisualStyleBackColor = true;
            this.btnFuncMoveTo.Click += new System.EventHandler(this.btnFuncMoveTo_Click);
            // 
            // cbFuncMoveTo
            // 
            this.cbFuncMoveTo.AutoSize = true;
            this.cbFuncMoveTo.Location = new System.Drawing.Point(6, 403);
            this.cbFuncMoveTo.Name = "cbFuncMoveTo";
            this.cbFuncMoveTo.Size = new System.Drawing.Size(15, 14);
            this.cbFuncMoveTo.TabIndex = 2;
            this.cbFuncMoveTo.UseVisualStyleBackColor = true;
            this.cbFuncMoveTo.CheckedChanged += new System.EventHandler(this.cbFuncMoveTo_CheckedChanged);
            this.cbFuncMoveTo.Click += new System.EventHandler(this.cbFuncMoveTo_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 687);
            this.Controls.Add(this.cbFuncMoveTo);
            this.Controls.Add(this.btnFuncMoveTo);
            this.Controls.Add(this.lsvPlayer);
            this.Name = "frmMain";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lsvPlayer;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Timer TmrCheckChars;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnFuncMoveTo;
        private System.Windows.Forms.CheckBox cbFuncMoveTo;
    }
}

