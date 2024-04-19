namespace MPScannerImg
{
    partial class Form1
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
            this.cmbScanners = new System.Windows.Forms.ComboBox();
            this.txtSavePath = new System.Windows.Forms.TextBox();
            this.btnScanAndSave = new System.Windows.Forms.Button();
            this.rtbConsoleLog = new System.Windows.Forms.RichTextBox();
            this.txtWaitTime = new System.Windows.Forms.TextBox();
            this.txtNumberOfPages = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cmbScanners
            // 
            this.cmbScanners.FormattingEnabled = true;
            this.cmbScanners.Location = new System.Drawing.Point(60, 34);
            this.cmbScanners.Name = "cmbScanners";
            this.cmbScanners.Size = new System.Drawing.Size(345, 21);
            this.cmbScanners.TabIndex = 0;
            // 
            // txtSavePath
            // 
            this.txtSavePath.Location = new System.Drawing.Point(60, 86);
            this.txtSavePath.Name = "txtSavePath";
            this.txtSavePath.Size = new System.Drawing.Size(345, 20);
            this.txtSavePath.TabIndex = 1;
            // 
            // btnScanAndSave
            // 
            this.btnScanAndSave.Location = new System.Drawing.Point(60, 136);
            this.btnScanAndSave.Name = "btnScanAndSave";
            this.btnScanAndSave.Size = new System.Drawing.Size(345, 36);
            this.btnScanAndSave.TabIndex = 2;
            this.btnScanAndSave.Text = "SCAN AND SAVE PDF";
            this.btnScanAndSave.UseVisualStyleBackColor = true;
            this.btnScanAndSave.Click += new System.EventHandler(this.btnScanAndSave_Click);
            // 
            // rtbConsoleLog
            // 
            this.rtbConsoleLog.Location = new System.Drawing.Point(60, 209);
            this.rtbConsoleLog.Name = "rtbConsoleLog";
            this.rtbConsoleLog.ReadOnly = true;
            this.rtbConsoleLog.Size = new System.Drawing.Size(345, 205);
            this.rtbConsoleLog.TabIndex = 3;
            this.rtbConsoleLog.Text = "";
            this.rtbConsoleLog.Visible = false;
            // 
            // txtWaitTime
            // 
            this.txtWaitTime.Location = new System.Drawing.Point(490, 35);
            this.txtWaitTime.Name = "txtWaitTime";
            this.txtWaitTime.Size = new System.Drawing.Size(216, 20);
            this.txtWaitTime.TabIndex = 4;
            // 
            // txtNumberOfPages
            // 
            this.txtNumberOfPages.Location = new System.Drawing.Point(490, 79);
            this.txtNumberOfPages.Name = "txtNumberOfPages";
            this.txtNumberOfPages.Size = new System.Drawing.Size(216, 20);
            this.txtNumberOfPages.TabIndex = 5;
            this.txtNumberOfPages.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtNumberOfPages);
            this.Controls.Add(this.txtWaitTime);
            this.Controls.Add(this.rtbConsoleLog);
            this.Controls.Add(this.btnScanAndSave);
            this.Controls.Add(this.txtSavePath);
            this.Controls.Add(this.cmbScanners);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbScanners;
        private System.Windows.Forms.TextBox txtSavePath;
        private System.Windows.Forms.Button btnScanAndSave;
        private System.Windows.Forms.RichTextBox rtbConsoleLog;
        private System.Windows.Forms.TextBox txtWaitTime;
        private System.Windows.Forms.TextBox txtNumberOfPages;
    }
}

