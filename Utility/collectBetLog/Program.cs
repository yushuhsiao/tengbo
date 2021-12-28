using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace collecReportLog
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //byte[] xx = Convert.FromBase64String("vbClc0usdo45vn4zhKNmRWjUsM1sWKeP+GMa2CsgK12KmGQiRBtwBZRFuNK/vTAqSExZ8dPEiHksGjpMgvIX8CRsLMTMhe8v7rRHtdBVrIyZEG4Q2mipZd2aZYa6SwvJpH3VtIMwQGlq57NwVwA0671g8GNyp5y1mp2Pcn7wnJtQdaQHsB9GQWaWGGM0eCOkNJdAsKgef3k1RhHPDHe/zKfoSGebArwU3WKeDAYE/7hqaZfRaoEDxCiaPdWAnWcGmtyOJWV0rKo0Xyte9TE8rgTmcg55FkcJyrprzlLrNhAYxKxy+ZyoSx7K7HtdOSRtoKGL8OIZYARCPZeiN0IZxw==");

            //System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider(2048);
            //string k1 = rsa.ToXmlString(true);
            //string k2 = rsa.ToXmlString(false);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //frmLogin frmLogin = new frmLogin();
            //if (frmLogin.ShowDialog() == DialogResult.OK)
            Application.Run(new collectReportLog());
        }
    }
}
