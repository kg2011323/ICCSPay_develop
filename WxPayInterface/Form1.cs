using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//using ICCSPayAPI;
using WxPayInterfaceLib;

namespace WxPayInterface
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtauth_code.Text))
            {
                MessageBox.Show("请输入授权码！");
                return;
            }
            if (string.IsNullOrEmpty(txtbody.Text))
            {
                MessageBox.Show("请输入商品描述！");
                return;
            }
            if (string.IsNullOrEmpty(txtfee.Text))
            {
                MessageBox.Show("请输入商品总金额！");
                return;
            }
            //调用刷卡支付,如果内部出现异常则在页面上显示异常原因

          
           
                bool incode=false;
                string res = "";
                //ICCSPayAPI.ICCSPayAPI ipa = new ICCSPayAPI.ICCSPayAPI();
                //res = ipa.ICCSPay_Natvie(textBox1.Text, textBox3.Text, txtbody.Text, textBox3.Text, txtauth_code.Text, txtfee.Text, ref incode);


                //AlipayInterfaceLib.AlipayInterfaceAPI ap = new AlipayInterfaceLib.AlipayInterfaceAPI();
                //res = ap.Alipay_Natvie(textBox3.Text, txtbody.Text, textBox3.Text, txtauth_code.Text, txtfee.Text, ref incode);






                WxPayInterfaceAPI ap = new WxPayInterfaceAPI();
                res = ap.WeixinPay_Natvie(textBox3.Text, txtbody.Text, textBox3.Text, txtauth_code.Text, txtfee.Text, ref incode);

                if (incode == true)
                {
                    MessageBox.Show("支付成功！");
                    MessageBox.Show(res);
                }
                else
                {
                    MessageBox.Show("支付失败！");
                    MessageBox.Show(res);
                }

         

        }


      

    
    }
}
