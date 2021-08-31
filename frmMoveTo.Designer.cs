
namespace cmtjJX2
{
    partial class frmMoveTo
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
            this.txtX = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtY = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMap = new System.Windows.Forms.TextBox();
            this.btnMove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtX
            // 
            this.txtX.Location = new System.Drawing.Point(32, 25);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(45, 20);
            this.txtX.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(90, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Y";
            // 
            // txtY
            // 
            this.txtY.Location = new System.Drawing.Point(110, 25);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(45, 20);
            this.txtY.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(178, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Map";
            // 
            // txtMap
            // 
            this.txtMap.Location = new System.Drawing.Point(212, 25);
            this.txtMap.Name = "txtMap";
            this.txtMap.Size = new System.Drawing.Size(45, 20);
            this.txtMap.TabIndex = 4;
            // 
            // btnMove
            // 
            this.btnMove.Location = new System.Drawing.Point(110, 81);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(75, 23);
            this.btnMove.TabIndex = 6;
            this.btnMove.Text = "Move";
            this.btnMove.UseVisualStyleBackColor = true;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // frmMoveTo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 153);
            this.Controls.Add(this.btnMove);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMap);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtY);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtX);
            this.Name = "frmMoveTo";
            this.Text = "frmMoveTo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMoveTo_FormClosing);
            this.Load += new System.EventHandler(this.frmMoveTo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMap;
        private System.Windows.Forms.Button btnMove;
    }
}