using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Windows.Automation;

namespace WebPageGrabber
{
    public partial class MainForm : Form
    {
        Thread GrabberThread;
        TcpModule _tcpmodule = new TcpModule();
        string get_url;
        public string information_for_send;
        public MainForm()
        {
            InitializeComponent();
            _tcpmodule.Receive += new TcpModule.ReceiveEventHandler(_tcpmodule_Receive);
            _tcpmodule.Disconnected += new TcpModule.DisconnectedEventHandler(_tcpmodule_Disconnected);
            _tcpmodule.Connected += new TcpModule.ConnectedEventHandler(_tcpmodule_Connected);
            _tcpmodule.Accept += new TcpModule.AcceptEventHandler(_tcpmodule_Accept);

            _tcpmodule.Parent = this;


            listBox1.HorizontalScrollbar = true;
        }
        private void collection_information()
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public void ReEnableGrabButton()
        {
            btGrab.Enabled = true;
        }
        private void btAbort_Click(object sender, EventArgs e)
        {

            try
            {
                GrabberThread.Abort();
                MessageBox.Show("Task aborted!");
                ReEnableGrabButton();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error aborting " + Environment.NewLine + "Details: " + ex.Message);
            }


        }
        private void btGrab_Click(object sender, EventArgs e)
        {

            try
            {
                SettingsObject settings = GetSettings();
                if (settings != null)
                {
                    //The settings are valid and we are good to go! 
                    btGrab.Enabled = false;


                    //Create a new webGrabber object to start our work! :D
                    WebPageGrabber Grabber = new WebPageGrabber(settings);
                    GrabberThread = new Thread(() => Grabber.StartGrab());
                    GrabberThread.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception Occured!" + Environment.NewLine + Environment.NewLine + "Details: " + Environment.NewLine + ex.Message);
            }
        }
        private SettingsObject GetSettings()
        {
            //////////////////////////////////////////
            string DestinationFolder = Utilities.GetFolderSelection();
            //////////////////////////////////////////

            //Create new Settings Object:
            SettingsObject Settings = new SettingsObject(this);
            Settings.DestinationFolder = DestinationFolder;
            Settings.Depth = Convert.ToInt16(tbDepth.Text);
            //////////////////////////////////////////
            Settings.URL = "https://habr.com/ru/sandbox/113454/";
            /////////////////////////////////////////
            return Settings;
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbDepth.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                tbDepth.Text.Remove(tbDepth.Text.Length - 1);
            }
        }
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void Form1_FormClosing(object sender, FormClosedEventArgs e)
        {
            _tcpmodule.CloseSocket();
        }
        void _tcpmodule_Accept(object sender)
        {
            ShowReceiveMessage("Клиент подключился!");
        }

        void _tcpmodule_Connected(object sender, string result)
        {
            ShowReceiveMessage(result);
        }

        void _tcpmodule_Disconnected(object sender, string result)
        {
            ShowReceiveMessage(result);
        }

        void _tcpmodule_Receive(object sender, ReceiveEventArgs e)
        {

            if (e.sendInfo.message != null)
            {
                ShowReceiveMessage("Письмо: " + e.sendInfo.message);
            }

            if (e.sendInfo.filesize > 0)
            {
                ShowReceiveMessage("Файл: " + e.sendInfo.filename);
            }

        }

        private void buttonStartServer_Click(object sender, EventArgs e)
        {
            _tcpmodule.StartServer();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            _tcpmodule.ConnectClient(textBoxIPserver.Text);
        }

        private void buttonSendData_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null || textBox1.Text == "" || textBox_login.Text == null || textBox_login.Text == "" || textBox_password.Text == null || textBox_password.Text == "")
            {
                MessageBox.Show("Заполните все формы");
            }
            else
            {
                information_for_send = get_url + '▼' + textBox_login.Text + '▼' + textBox_password.Text + '▼' + tbDepth.Text;
                Thread t = new Thread(_tcpmodule.SendData);
                t.Start();
            }

        }

        private void buttonAddFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _tcpmodule.SendFileName = dlg.FileName;
                labelFileName.Text = dlg.SafeFileName;
            }
        }

        delegate void UpdateReceiveDisplayDelegate(string message);
        public void ShowReceiveMessage(string message)
        {
            if (listBox1.InvokeRequired == true)
            {
                UpdateReceiveDisplayDelegate rdd = new UpdateReceiveDisplayDelegate(ShowReceiveMessage);

                // Данный метод вызывается в дочернем потоке,
                // ищет основной поток и выполняет делегат указанный в качестве параметра 
                // в главном потоке, безопасно обновляя интерфейс формы.
                Invoke(rdd, new object[] { message });
            }
            else
            {
                // Если не требуется вызывать метод Invoke, обратимся напрямую к элементу формы.
                listBox1.Items.Add((listBox1.Items.Count + 1).ToString() + ". " + message);
            }
        }

        private void ChangeBackColor(object sender, EventArgs e)
        {

        }
        delegate void BackColorFormDelegate(Color color);
        public void ChangeBackColor(Color color)
        {
            if (this.InvokeRequired == true)
            {
                BackColorFormDelegate bcf = new BackColorFormDelegate(ChangeBackColor);

                // Данный метод вызывается в дочернем потоке,
                // ищет основной поток и выполняет делегат указанный в качестве параметра 
                // в главном потоке, безопасно обновляя интерфейс формы.
                Invoke(bcf, new object[] { color });
            }
            else
            {
                this.BackColor = color;
            }
        }




        private string GetURLWEBPAGE()
        {
            var root = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, "Chrome_WidgetWin_1"));
            var textP = root.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
            var vpi = textP.GetCurrentPropertyValue(ValuePatternIdentifiers.ValueProperty).ToString();
            return vpi;
        }
        private void button_get_URL_Click(object sender, EventArgs e)
        {
            get_url = GetURLWEBPAGE();
            textBox1.Text = get_url;
        }
    }
}
