namespace SLEWebServiceTest
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
            this.btnWebOrderTest = new System.Windows.Forms.Button();
            this.btnStaitonOrderTest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxWebServiceURL = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnWebOrderTest
            // 
            this.btnWebOrderTest.Location = new System.Drawing.Point(204, 104);
            this.btnWebOrderTest.Name = "btnWebOrderTest";
            this.btnWebOrderTest.Size = new System.Drawing.Size(75, 23);
            this.btnWebOrderTest.TabIndex = 0;
            this.btnWebOrderTest.Text = "网络订单测试";
            this.btnWebOrderTest.UseVisualStyleBackColor = true;
            this.btnWebOrderTest.Click += new System.EventHandler(this.btnWebOrderTest_Click);
            // 
            // btnStaitonOrderTest
            // 
            this.btnStaitonOrderTest.Location = new System.Drawing.Point(611, 31);
            this.btnStaitonOrderTest.Name = "btnStaitonOrderTest";
            this.btnStaitonOrderTest.Size = new System.Drawing.Size(161, 216);
            this.btnStaitonOrderTest.TabIndex = 1;
            this.btnStaitonOrderTest.Text = "车站测试";
            this.btnStaitonOrderTest.UseVisualStyleBackColor = true;
            this.btnStaitonOrderTest.Click += new System.EventHandler(this.btnStaitonOrderTest_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Web Service URL:";
            // 
            // tbxWebServiceURL
            // 
            this.tbxWebServiceURL.Location = new System.Drawing.Point(111, 4);
            this.tbxWebServiceURL.Name = "tbxWebServiceURL";
            this.tbxWebServiceURL.Size = new System.Drawing.Size(661, 21);
            this.tbxWebServiceURL.TabIndex = 3;
            this.tbxWebServiceURL.Text = "http://58.63.71.37:9090/SLEWebService1/SLEWebService.asmx";
            this.tbxWebServiceURL.TextChanged += new System.EventHandler(this.tbxWebServiceURL_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 262);
            this.Controls.Add(this.tbxWebServiceURL);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStaitonOrderTest);
            this.Controls.Add(this.btnWebOrderTest);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnWebOrderTest;
        private System.Windows.Forms.Button btnStaitonOrderTest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxWebServiceURL;
    }
}

