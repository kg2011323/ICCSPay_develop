using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WxPayInterfaceLib;
using AlipayInterfaceLib;

namespace WxPayInterface
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //WxPayInterfaceAPI api = new WxPayInterfaceAPI();
            //bool incode=false;
            //string res= api.WeixinPay_DownloadBill(textBox4.Text, "ALL",ref  incode);
            //MessageBox.Show(res);

            AlipayInterfaceAPI api = new AlipayInterfaceAPI();
            bool incode = false;
            string res = api.Alipay_DownloadBill(textBox2.Text,textBox3.Text, ref  incode);
            MessageBox.Show(res);

        }
    }
}
