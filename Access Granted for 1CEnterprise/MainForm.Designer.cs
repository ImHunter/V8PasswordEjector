namespace DevelPlatform.OneCEUtils.AccessGranted
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.panelFileBase = new System.Windows.Forms.Panel();
            this.textBoxDataBasePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.toolStripMainPanel = new System.Windows.Forms.ToolStrip();
            this.toolStripFileBase = new System.Windows.Forms.ToolStripButton();
            this.toolStripServerBase = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonHelp = new System.Windows.Forms.ToolStripButton();
            this.panelServerBase = new System.Windows.Forms.Panel();
            this.labelTypeDBMS = new System.Windows.Forms.Label();
            this.comboBoxTypeDBMS = new System.Windows.Forms.ComboBox();
            this.radioButtonFull = new System.Windows.Forms.RadioButton();
            this.radioButtonSimle = new System.Windows.Forms.RadioButton();
            this.panelDataBaseServerSimple = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.labelDataBaseUser = new System.Windows.Forms.Label();
            this.labelDataBaseName = new System.Windows.Forms.Label();
            this.labelataBaseServer = new System.Windows.Forms.Label();
            this.textBoxDataBaseUserPassword = new System.Windows.Forms.TextBox();
            this.textBoxDataBaseUser = new System.Windows.Forms.TextBox();
            this.textBoxDataBaseName = new System.Windows.Forms.TextBox();
            this.textBoxDataBaseServer = new System.Windows.Forms.TextBox();
            this.panelConnStr = new System.Windows.Forms.Panel();
            this.labelConnStr = new System.Windows.Forms.Label();
            this.textBoxConnStr = new System.Windows.Forms.TextBox();
            this.panelFileBase.SuspendLayout();
            this.toolStripMainPanel.SuspendLayout();
            this.panelServerBase.SuspendLayout();
            this.panelDataBaseServerSimple.SuspendLayout();
            this.panelConnStr.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Файловая информационная база (*.1CD)|*.1CD";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(216, 191);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(166, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Сбросить пользователей";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 191);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(170, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "Восстановить пользователей";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // panelFileBase
            // 
            this.panelFileBase.Controls.Add(this.textBoxDataBasePath);
            this.panelFileBase.Controls.Add(this.label1);
            this.panelFileBase.Controls.Add(this.button1);
            this.panelFileBase.Location = new System.Drawing.Point(13, 28);
            this.panelFileBase.Name = "panelFileBase";
            this.panelFileBase.Size = new System.Drawing.Size(369, 157);
            this.panelFileBase.TabIndex = 7;
            // 
            // textBox1
            // 
            this.textBoxDataBasePath.Location = new System.Drawing.Point(6, 26);
            this.textBoxDataBasePath.Name = "textBox1";
            this.textBoxDataBasePath.ReadOnly = true;
            this.textBoxDataBasePath.Size = new System.Drawing.Size(327, 20);
            this.textBoxDataBasePath.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Путь к файлу базы данных (\'*.1CD\'):";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(339, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(27, 20);
            this.button1.TabIndex = 3;
            this.button1.Text = "....";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonChooseDatabaseFile_Click);
            // 
            // toolStripMainPanel
            // 
            this.toolStripMainPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripFileBase,
            this.toolStripServerBase,
            this.toolStripButtonHelp});
            this.toolStripMainPanel.Location = new System.Drawing.Point(0, 0);
            this.toolStripMainPanel.Name = "toolStripMainPanel";
            this.toolStripMainPanel.Size = new System.Drawing.Size(394, 25);
            this.toolStripMainPanel.TabIndex = 8;
            this.toolStripMainPanel.Text = "Главная панель";
            // 
            // toolStripFileBase
            // 
            this.toolStripFileBase.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripFileBase.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripFileBase.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline);
            this.toolStripFileBase.Image = ((System.Drawing.Image)(resources.GetObject("toolStripFileBase.Image")));
            this.toolStripFileBase.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripFileBase.Name = "toolStripFileBase";
            this.toolStripFileBase.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripFileBase.Size = new System.Drawing.Size(92, 22);
            this.toolStripFileBase.Text = "Файловая база";
            this.toolStripFileBase.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.toolStripFileBase.Click += new System.EventHandler(this.toolStripFileBase_Click);
            // 
            // toolStripServerBase
            // 
            this.toolStripServerBase.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.toolStripServerBase.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripServerBase.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline);
            this.toolStripServerBase.Image = ((System.Drawing.Image)(resources.GetObject("toolStripServerBase.Image")));
            this.toolStripServerBase.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripServerBase.Name = "toolStripServerBase";
            this.toolStripServerBase.Size = new System.Drawing.Size(97, 22);
            this.toolStripServerBase.Text = "Серверная база";
            this.toolStripServerBase.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButtonHelp
            // 
            this.toolStripButtonHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonHelp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripButtonHelp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonHelp.Image")));
            this.toolStripButtonHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHelp.Name = "toolStripButtonHelp";
            this.toolStripButtonHelp.Size = new System.Drawing.Size(87, 22);
            this.toolStripButtonHelp.Text = "О программе";
            this.toolStripButtonHelp.Click += new System.EventHandler(this.MainForm_HelpButtonClicked);
            // 
            // panelServerBase
            // 
            this.panelServerBase.Controls.Add(this.labelTypeDBMS);
            this.panelServerBase.Controls.Add(this.comboBoxTypeDBMS);
            this.panelServerBase.Controls.Add(this.radioButtonFull);
            this.panelServerBase.Controls.Add(this.radioButtonSimle);
            this.panelServerBase.Controls.Add(this.panelDataBaseServerSimple);
            this.panelServerBase.Location = new System.Drawing.Point(13, 28);
            this.panelServerBase.Name = "panelServerBase";
            this.panelServerBase.Size = new System.Drawing.Size(369, 157);
            this.panelServerBase.TabIndex = 10;
            // 
            // labelTypeDBMS
            // 
            this.labelTypeDBMS.AutoSize = true;
            this.labelTypeDBMS.Location = new System.Drawing.Point(4, 31);
            this.labelTypeDBMS.Name = "labelTypeDBMS";
            this.labelTypeDBMS.Size = new System.Drawing.Size(63, 13);
            this.labelTypeDBMS.TabIndex = 15;
            this.labelTypeDBMS.Text = "Тип СУБД:";
            // 
            // comboBoxTypeDBMS
            // 
            this.comboBoxTypeDBMS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTypeDBMS.FormattingEnabled = true;
            this.comboBoxTypeDBMS.Items.AddRange(new object[] {
            "Microsoft SQL Server",
            "PostgreSQL",
            "IBM DB2",
            "Oracle Database"});
            this.comboBoxTypeDBMS.Location = new System.Drawing.Point(203, 25);
            this.comboBoxTypeDBMS.Name = "comboBoxTypeDBMS";
            this.comboBoxTypeDBMS.Size = new System.Drawing.Size(161, 21);
            this.comboBoxTypeDBMS.TabIndex = 14;
            // 
            // radioButtonFull
            // 
            this.radioButtonFull.AutoSize = true;
            this.radioButtonFull.Location = new System.Drawing.Point(234, 4);
            this.radioButtonFull.Name = "radioButtonFull";
            this.radioButtonFull.Size = new System.Drawing.Size(131, 17);
            this.radioButtonFull.TabIndex = 2;
            this.radioButtonFull.TabStop = true;
            this.radioButtonFull.Text = "Строка подключения";
            this.radioButtonFull.UseVisualStyleBackColor = true;
            this.radioButtonFull.Click += new System.EventHandler(this.radioButtonFull_Click);
            // 
            // radioButtonSimle
            // 
            this.radioButtonSimle.AutoSize = true;
            this.radioButtonSimle.Checked = true;
            this.radioButtonSimle.Location = new System.Drawing.Point(4, 4);
            this.radioButtonSimle.Name = "radioButtonSimle";
            this.radioButtonSimle.Size = new System.Drawing.Size(148, 17);
            this.radioButtonSimle.TabIndex = 1;
            this.radioButtonSimle.TabStop = true;
            this.radioButtonSimle.Text = "Упрощенные настройки";
            this.radioButtonSimle.UseVisualStyleBackColor = true;
            this.radioButtonSimle.Click += new System.EventHandler(this.radioButtonSimle_Click);
            // 
            // panelDataBaseServerSimple
            // 
            this.panelDataBaseServerSimple.Controls.Add(this.label5);
            this.panelDataBaseServerSimple.Controls.Add(this.labelDataBaseUser);
            this.panelDataBaseServerSimple.Controls.Add(this.labelDataBaseName);
            this.panelDataBaseServerSimple.Controls.Add(this.labelataBaseServer);
            this.panelDataBaseServerSimple.Controls.Add(this.textBoxDataBaseUserPassword);
            this.panelDataBaseServerSimple.Controls.Add(this.textBoxDataBaseUser);
            this.panelDataBaseServerSimple.Controls.Add(this.textBoxDataBaseName);
            this.panelDataBaseServerSimple.Controls.Add(this.textBoxDataBaseServer);
            this.panelDataBaseServerSimple.Location = new System.Drawing.Point(0, 48);
            this.panelDataBaseServerSimple.Name = "panelDataBaseServerSimple";
            this.panelDataBaseServerSimple.Size = new System.Drawing.Size(370, 109);
            this.panelDataBaseServerSimple.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(191, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Пароль пользователя базы данных:";
            // 
            // labelDataBaseUser
            // 
            this.labelDataBaseUser.AutoSize = true;
            this.labelDataBaseUser.Location = new System.Drawing.Point(7, 63);
            this.labelDataBaseUser.Name = "labelDataBaseUser";
            this.labelDataBaseUser.Size = new System.Drawing.Size(152, 13);
            this.labelDataBaseUser.TabIndex = 20;
            this.labelDataBaseUser.Text = "Пользователь базы данных:";
            // 
            // labelDataBaseName
            // 
            this.labelDataBaseName.AutoSize = true;
            this.labelDataBaseName.Location = new System.Drawing.Point(7, 36);
            this.labelDataBaseName.Name = "labelDataBaseName";
            this.labelDataBaseName.Size = new System.Drawing.Size(101, 13);
            this.labelDataBaseName.TabIndex = 19;
            this.labelDataBaseName.Text = "Имя базы данных:";
            // 
            // labelataBaseServer
            // 
            this.labelataBaseServer.AutoSize = true;
            this.labelataBaseServer.Location = new System.Drawing.Point(7, 9);
            this.labelataBaseServer.Name = "labelataBaseServer";
            this.labelataBaseServer.Size = new System.Drawing.Size(108, 13);
            this.labelataBaseServer.TabIndex = 18;
            this.labelataBaseServer.Text = "Сервер баз данных:";
            // 
            // textBoxDataBaseUserPassword
            // 
            this.textBoxDataBaseUserPassword.Location = new System.Drawing.Point(202, 83);
            this.textBoxDataBaseUserPassword.Name = "textBoxDataBaseUserPassword";
            this.textBoxDataBaseUserPassword.PasswordChar = '*';
            this.textBoxDataBaseUserPassword.Size = new System.Drawing.Size(161, 20);
            this.textBoxDataBaseUserPassword.TabIndex = 17;
            // 
            // textBoxDataBaseUser
            // 
            this.textBoxDataBaseUser.Location = new System.Drawing.Point(203, 57);
            this.textBoxDataBaseUser.Name = "textBoxDataBaseUser";
            this.textBoxDataBaseUser.Size = new System.Drawing.Size(161, 20);
            this.textBoxDataBaseUser.TabIndex = 16;
            // 
            // textBoxDataBaseName
            // 
            this.textBoxDataBaseName.Location = new System.Drawing.Point(203, 30);
            this.textBoxDataBaseName.Name = "textBoxDataBaseName";
            this.textBoxDataBaseName.Size = new System.Drawing.Size(161, 20);
            this.textBoxDataBaseName.TabIndex = 15;
            // 
            // textBoxDataBaseServer
            // 
            this.textBoxDataBaseServer.Location = new System.Drawing.Point(203, 3);
            this.textBoxDataBaseServer.Name = "textBoxDataBaseServer";
            this.textBoxDataBaseServer.Size = new System.Drawing.Size(161, 20);
            this.textBoxDataBaseServer.TabIndex = 14;
            // 
            // panelConnStr
            // 
            this.panelConnStr.Controls.Add(this.labelConnStr);
            this.panelConnStr.Controls.Add(this.textBoxConnStr);
            this.panelConnStr.Location = new System.Drawing.Point(12, 76);
            this.panelConnStr.Name = "panelConnStr";
            this.panelConnStr.Size = new System.Drawing.Size(370, 108);
            this.panelConnStr.TabIndex = 21;
            // 
            // labelConnStr
            // 
            this.labelConnStr.AutoSize = true;
            this.labelConnStr.Location = new System.Drawing.Point(5, 3);
            this.labelConnStr.Name = "labelConnStr";
            this.labelConnStr.Size = new System.Drawing.Size(116, 13);
            this.labelConnStr.TabIndex = 3;
            this.labelConnStr.Text = "Строка подключения:";
            // 
            // textBoxConnStr
            // 
            this.textBoxConnStr.Location = new System.Drawing.Point(2, 19);
            this.textBoxConnStr.Multiline = true;
            this.textBoxConnStr.Name = "textBoxConnStr";
            this.textBoxConnStr.Size = new System.Drawing.Size(366, 87);
            this.textBoxConnStr.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 226);
            this.Controls.Add(this.panelConnStr);
            this.Controls.Add(this.panelServerBase);
            this.Controls.Add(this.toolStripMainPanel);
            this.Controls.Add(this.panelFileBase);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Сброс учетных записей [1С:Предприятие 8.x] ";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.MainForm_HelpButtonClicked);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panelFileBase.ResumeLayout(false);
            this.panelFileBase.PerformLayout();
            this.toolStripMainPanel.ResumeLayout(false);
            this.toolStripMainPanel.PerformLayout();
            this.panelServerBase.ResumeLayout(false);
            this.panelServerBase.PerformLayout();
            this.panelDataBaseServerSimple.ResumeLayout(false);
            this.panelDataBaseServerSimple.PerformLayout();
            this.panelConnStr.ResumeLayout(false);
            this.panelConnStr.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Panel panelFileBase;
        private System.Windows.Forms.TextBox textBoxDataBasePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStrip toolStripMainPanel;
        private System.Windows.Forms.ToolStripButton toolStripFileBase;
        private System.Windows.Forms.ToolStripButton toolStripServerBase;
        private System.Windows.Forms.Panel panelServerBase;
        private System.Windows.Forms.Panel panelDataBaseServerSimple;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelDataBaseUser;
        private System.Windows.Forms.Label labelDataBaseName;
        private System.Windows.Forms.Label labelataBaseServer;
        private System.Windows.Forms.TextBox textBoxDataBaseUserPassword;
        private System.Windows.Forms.TextBox textBoxDataBaseUser;
        private System.Windows.Forms.TextBox textBoxDataBaseName;
        private System.Windows.Forms.TextBox textBoxDataBaseServer;
        private System.Windows.Forms.RadioButton radioButtonFull;
        private System.Windows.Forms.RadioButton radioButtonSimle;
        private System.Windows.Forms.Label labelTypeDBMS;
        private System.Windows.Forms.ComboBox comboBoxTypeDBMS;
        private System.Windows.Forms.Panel panelConnStr;
        private System.Windows.Forms.Label labelConnStr;
        private System.Windows.Forms.TextBox textBoxConnStr;
        private System.Windows.Forms.ToolStripButton toolStripButtonHelp;
    }
}

