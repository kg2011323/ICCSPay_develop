using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//using ICCSPayAPI;
using WxPayInterfaceLib;


namespace WxPayInterface
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("1001"))
            {

            }
            else if (textBox1.Text.Equals("1002"))
            {
               
            }
            //ICCSPayAPI.ICCSPayAPI api = new ICCSPayAPI.ICCSPayAPI();
            //api.
            bool res=false;
                 

            WxPayInterfaceAPI api = new WxPayInterfaceAPI();
            string result= api.WeixinPay_OrderQuery("", textBox3.Text, ref res);
            if (res)
            {
                MessageBox.Show("查询成功");
            }else
                MessageBox.Show("查询失败");


            MessageBox.Show(result);
        }
    }
}
