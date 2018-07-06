using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SLEWebServiceTest.View;
using SLEWebServiceTest.Util;
using log4net;
using System.Reflection;

namespace SLEWebServiceTest
{
    public partial class Form1 : Form
    {
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public Form1()
        {
            InitializeComponent();

            SetWebServiceURL();
        }

        private void btnWebOrderTest_Click(object sender, EventArgs e)
        {
            WebOrderTestForm instance = new WebOrderTestForm();
            instance.ShowDialog();
        }

        private void btnStaitonOrderTest_Click(object sender, EventArgs e)
        {
            StationOrderForm instance = new StationOrderForm();
            instance.ShowDialog();
        }

        private void tbxWebServiceURL_TextChanged(object sender, EventArgs e)
        {
            SetWebServiceURL();
        }

        private void SetWebServiceURL()
        {
            try
            {
                Constants.WebServiceURL = tbxWebServiceURL.Text.Trim();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }
        }
    }
}
