using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SLEWebServiceTest.View
{
    public class TestBaseForm : Form
    {
        public TextBox tbxBaseDeviceId = new TextBox();

        public string reqSysDate
        {
            get
            {
                return DateTime.Now.ToString("yyyyMMddHHmmss");
            }
        }

        public string operationCode = String.Empty;

        public const string  cityCode = "020";

        public string deviceId
        {
            get { return tbxBaseDeviceId.Text.Trim(); }
        }

        public string channelType;
    }
}
