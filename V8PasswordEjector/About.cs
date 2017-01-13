using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace DevelPlatform.OneCEUtils.AccessGranted
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://develplatform.ru");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("mailto:ypermitin@yandex.ru");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }

        private void btnSendFeedback_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.develplatform.ru/About-us/Contact-us?messageSubject=%D0%A1%D0%B1%D1%80%D0%BE%D1%81%20%D1%83%D1%87%D0%B5%D1%82%D0%BD%D1%8B%D1%85%20%D0%B7%D0%B0%D0%BF%D0%B8%D1%81%D0%B5%D0%B9%201C:%D0%9F%D1%80%D0%B5%D0%B4%D0%BF%D1%80%D0%B8%D1%8F%D1%82%D0%B8%D1%8F%20(%D0%BF%D1%80%D0%B8%D0%BB%D0%BE%D0%B6%D0%B5%D0%BD%D0%B8%D0%B5)");
        }

        private void linkLblGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/YPermitin/V8PasswordEjector");
        }
    }
}
