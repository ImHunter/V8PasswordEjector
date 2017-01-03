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

        private void linkLabel3_Click(object sender, EventArgs e)
        {
            Process.Start("http://develplatform.ru/OneC/Stream/OneCBlog/2012/12/07/Вход-без-приглашения");
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
            Process.Start("http://develplatform.ru/OneC/Stream/OneCBlog/2013/05/11/Сброс-учетных-записей-Пишем-универсальную-программу-на-NET-Framework");
        }
    }
}
