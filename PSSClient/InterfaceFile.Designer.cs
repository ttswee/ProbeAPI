namespace PSSClient
{
    partial class InterfaceFile
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
            this.label1 = new System.Windows.Forms.Label();
            this.cmbServer = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbSystem = new System.Windows.Forms.ComboBox();
            this.lblPath = new System.Windows.Forms.Label();
            this.btngetFile = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCaseNo = new System.Windows.Forms.TextBox();
            this.lblEndPoint = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server";
            // 
            // cmbServer
            // 
            this.cmbServer.FormattingEnabled = true;
            this.cmbServer.Location = new System.Drawing.Point(85, 38);
            this.cmbServer.Name = "cmbServer";
            this.cmbServer.Size = new System.Drawing.Size(135, 21);
            this.cmbServer.TabIndex = 1;
            this.cmbServer.SelectedIndexChanged += new System.EventHandler(this.cmbServer_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "System";
            // 
            // cmbSystem
            // 
            this.cmbSystem.FormattingEnabled = true;
            this.cmbSystem.Location = new System.Drawing.Point(85, 81);
            this.cmbSystem.Name = "cmbSystem";
            this.cmbSystem.Size = new System.Drawing.Size(135, 21);
            this.cmbSystem.TabIndex = 3;
            this.cmbSystem.SelectedIndexChanged += new System.EventHandler(this.cmbSystem_SelectedIndexChanged);
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(33, 124);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(32, 13);
            this.lblPath.TabIndex = 4;
            this.lblPath.Text = "Path ";
            // 
            // btngetFile
            // 
            this.btngetFile.Location = new System.Drawing.Point(36, 211);
            this.btngetFile.Name = "btngetFile";
            this.btngetFile.Size = new System.Drawing.Size(97, 31);
            this.btngetFile.TabIndex = 5;
            this.btngetFile.Text = "Retrieve";
            this.btngetFile.UseVisualStyleBackColor = true;
            this.btngetFile.Click += new System.EventHandler(this.btngetFile_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Case No.";
            // 
            // txtCaseNo
            // 
            this.txtCaseNo.Location = new System.Drawing.Point(85, 180);
            this.txtCaseNo.Name = "txtCaseNo";
            this.txtCaseNo.Size = new System.Drawing.Size(135, 20);
            this.txtCaseNo.TabIndex = 7;
            // 
            // lblEndPoint
            // 
            this.lblEndPoint.AutoSize = true;
            this.lblEndPoint.Location = new System.Drawing.Point(33, 154);
            this.lblEndPoint.Name = "lblEndPoint";
            this.lblEndPoint.Size = new System.Drawing.Size(53, 13);
            this.lblEndPoint.TabIndex = 8;
            this.lblEndPoint.Text = "End Point";
            // 
            // InterfaceFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1303, 589);
            this.Controls.Add(this.lblEndPoint);
            this.Controls.Add(this.txtCaseNo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btngetFile);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.cmbSystem);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbServer);
            this.Controls.Add(this.label1);
            this.Name = "InterfaceFile";
            this.Text = "Interface Files";
            this.Load += new System.EventHandler(this.InterfaceFile_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbServer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbSystem;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.Button btngetFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCaseNo;
        private System.Windows.Forms.Label lblEndPoint;
    }
}