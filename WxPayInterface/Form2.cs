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
using ICCSPayAPI;

namespace WxPayInterface
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool res = false;


            //WxPayInterfaceAPI api = new WxPayInterfaceAPI();
            //string result = api.WeixinPay_refund("",textBox3.Text,txtfee.Text,textBox2.Text,    ref res);
            //AlipayInterfaceAPI api = new AlipayInterfaceAPI();

            ICCSPayAPI.ICCSPayAPI api = new ICCSPayAPI.ICCSPayAPI();
            Random r = new Random();
            string no = DateTime.Now.ToString("yyyyMMddHHmmss") + r.Next(1000, 9999).ToString();
            //string result = api.Alipay_Refund(textBox3.Text, no, textBox2.Text, ref res);


            string result=api.ICCSPay_Refund(textBox1.Text, textBox3.Text, no, txtfee.Text, textBox2.Text, ref  res);





            if (res)
            {
                MessageBox.Show("退款成功");
            }
            else
                MessageBox.Show("退款失败");


            MessageBox.Show(result);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool res = false;


            WxPayInterfaceAPI api = new WxPayInterfaceAPI();
            string result = api.WeixinPay_RefundQuery(textBox4.Text, ref res);
            if (res)
            {
                MessageBox.Show("退款查询成功" );
            }
            else
                MessageBox.Show("退款查询失败");


            MessageBox.Show(result);
        }
    }
}
