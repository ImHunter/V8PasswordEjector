using System;
using System.Drawing;
using System.Windows.Forms;
using DevelPlatform.OneCEUtils.V8PasswordEjector;


namespace DevelPlatform.OneCEUtils.AccessGranted
{
    public partial class MainForm : Form
    {
        // Флаг "Это файловая база"
        private bool ThisFileBase = true;

        #region FormAndElements

        public MainForm()
        {
            InitializeComponent();

            SetVisiblePanel(ThisFileBase);

            richTextBoxWarning.SelectAll();
            richTextBoxWarning.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            comboBoxTypeDBMS.SelectedIndex = 0;
            SetVisibleDataBaseServerSetting();
        }

        private void MainForm_HelpButtonClicked(object sender, EventArgs e)
        {
            About frmAbout = new About();
            frmAbout.ShowDialog();
        }

        private void buttonChooseDatabaseFile_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
            textBoxDataBasePath.Text = openFileDialog.FileName;
        }

        private void toolStripFileBase_Click(object sender, EventArgs e)
        {
            SetVisiblePanel(true);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SetVisiblePanel(false);
        }

        private void radioButtonSimle_Click(object sender, EventArgs e)
        {
            radioButtonFull.Checked = false;
            SetVisibleDataBaseServerSetting();
        }

        private void radioButtonFull_Click(object sender, EventArgs e)
        {
            radioButtonSimle.Checked = false;
            SetVisibleDataBaseServerSetting();
            if (textBoxConnStr.Text == "")
            {
                textBoxConnStr.Text = GetConnectionString();
            }
        }

        private void btnResetUsers_Click(object sender, EventArgs e)
        {
            if (!CheckFillSettings())
            {
                return;
            }

            if (ThisFileBase)
            {
                try
                {
                    PasswordEjector.ResetFileBaseUsers(textBoxDataBasePath.Text);
                    MessageBox.Show("Операция успешно выполнена!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка:\n" + ex.Message);
                }
            }
            else
            {
                try
                {
                    PasswordEjector.ResetServerInfobaseUsers(
                        GetConnectionString(),
                        GetSelectedDBMS());
                    MessageBox.Show("Операция успешно выполнена!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка:\n" + ex.Message);
                }
            }
        }

        private void btnRecoveryPassword_Click(object sender, EventArgs e)
        {
            if (!CheckFillSettings())
            {
                return;
            }

            if (ThisFileBase)
            {
                try
                {
                    PasswordEjector.RecoveryFileBaseUsers(textBoxDataBasePath.Text);
                    MessageBox.Show("Операция успешно выполнена!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка:\n" + ex.Message);
                }
            }
            else
            {
                try
                {
                    PasswordEjector.RecoveryServerInfobaseUsers(
                        GetConnectionString(),
                        GetSelectedDBMS());
                    MessageBox.Show("Операция успешно выполнена!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка:\n" + ex.Message);
                }
            }
        }

        #endregion

        #region UserInterfaceControl

        // Устанавливаем видимость панелей файловой/серверной базы. TRUE - файловая база.
        //
        private void SetVisiblePanel(bool visThisFileBase)
        {
            ThisFileBase = visThisFileBase;
            panelFileBase.Visible = ThisFileBase;
            panelServerBase.Visible = !(ThisFileBase);
            if (ThisFileBase)
            {
                toolStripFileBase.BackColor = Color.FromName("ScrollBar");
                toolStripServerBase.BackColor = Color.FromName("Control");
            }
            else
            {
                toolStripServerBase.BackColor = Color.FromName("ScrollBar");
                toolStripFileBase.BackColor = Color.FromName("Control");
            }
        }

        // Устанавливаем видимость панелей настроек подключения к СУБД
        //
        private void SetVisibleDataBaseServerSetting()
        {
            panelDataBaseServerSimple.Visible = radioButtonSimle.Checked;
            panelConnStr.Visible = radioButtonFull.Checked;
        }

        // Проверка заполнения необходимых параметров
        //
        private bool CheckFillSettings()
        {
            bool Result = true;
            if (ThisFileBase)
            {
                if (textBoxDataBasePath.Text == "")
                {
                    MessageBox.Show("Не выбран файл базы данных!");
                    Result = false;
                }
            }
            else
            {
                if (radioButtonFull.Checked)
                {
                    if (textBoxConnStr.Text == "")
                    {
                        MessageBox.Show("Не заполнена строка подключения к СУБД!");
                        Result = false;
                    }
                }
                else
                {
                    if (textBoxDataBaseName.Text == "" || textBoxDataBaseServer.Text == ""
                        || textBoxDataBaseUser.Text == "" || textBoxDataBaseUserPassword.Text == "")
                    {
                        MessageBox.Show("Необходимо заполнить все параметры подключения к СУБД!");
                        Result = false;
                    }
                }
            }
            return Result;
        }

        #endregion

        #region Service

        private PasswordEjector.SupportedDBMS GetSelectedDBMS()
        {
            PasswordEjector.SupportedDBMS selectedDBMS;
            if (comboBoxTypeDBMS.SelectedIndex == 0)
                selectedDBMS = PasswordEjector.SupportedDBMS.MSSQL;
            else if (comboBoxTypeDBMS.SelectedIndex == 1)
                selectedDBMS = PasswordEjector.SupportedDBMS.PostgreSQL;
            else if (comboBoxTypeDBMS.SelectedIndex == 2)
                selectedDBMS = PasswordEjector.SupportedDBMS.IBMDB2;
            else if (comboBoxTypeDBMS.SelectedIndex == 3)
                selectedDBMS = PasswordEjector.SupportedDBMS.OracleDatabase;
            else
                selectedDBMS = PasswordEjector.SupportedDBMS.MSSQL;

            return selectedDBMS;
        }

        // Получает строку подключения для выбранных параметров SQL-сервера и базы
        public string GetConnectionString()
        {
            string connStr = "";

            // Если строка подключения введена вручную, то используем ее
            if (radioButtonFull.Checked && textBoxConnStr.Text != "")
            {
                connStr = textBoxConnStr.Text;
            }
            // Формируем строку подключения для MSSQL
            else if (comboBoxTypeDBMS.SelectedIndex == 0)
            {
                connStr = "Data Source=" + textBoxDataBaseServer.Text +
                                 ";Initial Catalog=" + textBoxDataBaseName.Text +
                                 ";User ID=" + textBoxDataBaseUser.Text +
                                 ";Password=" + textBoxDataBaseUserPassword.Text;
            }
            // Формируем строку подключения для PostgreSQL
            else if (comboBoxTypeDBMS.SelectedIndex == 1)
            {
                connStr = "Server=" + textBoxDataBaseServer.Text +
                          ";Port=5432;Database=" + textBoxDataBaseName.Text +
                          ";User Id=" + textBoxDataBaseUser.Text +
                          ";Password=" + textBoxDataBaseUserPassword.Text;
            }
            // Формируем строку подключения для IBMDB2
            else if (comboBoxTypeDBMS.SelectedIndex == 2)
            {
                connStr = "Provider=IBMDADB2;Database=" + textBoxDataBaseName.Text +
                          ";Hostname=" + textBoxDataBaseServer.Text +
                          ";Protocol=TCPIP;Port=50000;Uid=" + textBoxDataBaseUser.Text +
                          ";Pwd=" + textBoxDataBaseUserPassword.Text + ";";
            }
            // Формируем строку подключения для Oracle Database
            else if (comboBoxTypeDBMS.SelectedIndex == 3)
            {
                string server = "";
                string service = "";
                string buf = textBoxDataBaseServer.Text.Substring(2, textBoxDataBaseServer.Text.Length - 2);

                try
                {
                    int div = buf.IndexOf("/");
                    server = buf.Substring(0, div);
                    service = buf.Substring((div + 1), buf.Length - (div + 1));
                }
                catch
                {
                    server = "---";
                    service = "---";
                }

                connStr = "User Id=" + textBoxDataBaseUser.Text + ";Password=" + textBoxDataBaseUserPassword.Text +
                           ";Server=" + server + ";Unicode=True;Direct=True;Service Name=" + service + ";";
            }

            return connStr;
        }

        #endregion
    }
}
