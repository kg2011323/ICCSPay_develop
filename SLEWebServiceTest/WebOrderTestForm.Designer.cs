namespace SLEWebServiceTest
{
    partial class WebOrderTestForm
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
            this.tbxCityCode = new System.Windows.Forms.TextBox();
            this.tbxDeviceId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "CityCode:";
            // 
            // tbxCityCode
            // 
            this.tbxCityCode.Location = new System.Drawing.Point(78, 10);
            this.tbxCityCode.Name = "tbxCityCode";
            this.tbxCityCode.Size = new System.Drawing.Size(100, 21);
            this.tbxCityCode.TabIndex = 1;
            this.tbxCityCode.Text = "020";
            // 
            // tbxDeviceId
            // 
            this.tbxDeviceId.Location = new System.Drawing.Point(264, 263);
            this.tbxDeviceId.Name = "tbxDeviceId";
            this.tbxDeviceId.Size = new System.Drawing.Size(100, 21);
            this.tbxDeviceId.TabIndex = 3;
            this.tbxDeviceId.Text = "webDI";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(199, 266);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "DeviceId:";
            // 
            // WebOrderTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 547);
            this.Controls.Add(this.tbxDeviceId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbxCityCode);
            this.Controls.Add(this.label1);
            this.Name = "WebOrderTestForm";
            this.Text = "WebOrderTestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxCityCode;
        private System.Windows.Forms.TextBox tbxDeviceId;
        private System.Windows.Forms.Label label2;
    }
}