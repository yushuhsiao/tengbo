using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace LogService
{
    partial class hg_test : UserControl
    {
        public hg_test()
        {
            InitializeComponent();
        }

        private void registration_Click(object sender, EventArgs e)
        {
            //var n = extAPI.hg.api.registration("tf_0000", null, extAPI.hg.LoginMode.娛樂, "tf_", "0000", BU.CurrencyCode.CNY, null, null, extAPI.hg.TestUser.測試, null);
        }

        private void accountbalance_Click(object sender, EventArgs e)
        {
            var n = extAPI.hg.api.accountbalance("tf_0000", extAPI.hg.LoginMode.娛樂);
        }

        private void blockuser_Click(object sender, EventArgs e)
        {

        }

        private void deposit_Click(object sender, EventArgs e)
        {

        }

        private void deposit_confirm_Click(object sender, EventArgs e)
        {

        }

        private void withdrawal_Click(object sender, EventArgs e)
        {

        }

        private void withdrawal_confirm_Click(object sender, EventArgs e)
        {

        }

        private void login_deposit_Click(object sender, EventArgs e)
        {

        }

        private void logout_Click(object sender, EventArgs e)
        {

        }

        private void session_Click(object sender, EventArgs e)
        {

        }

        private void winlimit_Click(object sender, EventArgs e)
        {

        }
    }
}
