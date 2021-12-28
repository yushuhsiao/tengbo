using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using Tools.Protocol;
using Newtonsoft.Json;
using System.Globalization;

namespace shishicai
{
    public partial class frmServer : Form
    {
        static void Main(string[] args)
        {
            CultureInfo[] c = CultureInfo.GetCultures(CultureTypes.AllCultures);
            int[] lcid = new int[c.Length];
            for (int i = 0; i < c.Length; i++)
                lcid[i] = c[i].LCID;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmServer());
        }

        public frmServer()
        {
            InitializeComponent();
        }
    }
}