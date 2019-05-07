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

          


        }
        private void btGrab_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
     
        private void Form1_FormClosing(object sender, FormClosedEventArgs e)
        {

        }
        void _tcpmodule_Accept(object sender)
        {

        }

        void _tcpmodule_Connected(object sender, string result)
        {

        }

        void _tcpmodule_Disconnected(object sender, string result)
        {

        }

        void _tcpmodule_Receive(object sender, ReceiveEventArgs e)
        {


            
        }

        private void buttonStartServer_Click(object sender, EventArgs e)
        {

        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {

        }

        private void buttonSendData_Click(object sender, EventArgs e)
        {


        }

        private void buttonAddFile_Click(object sender, EventArgs e)
        {

        }

        delegate void UpdateReceiveDisplayDelegate(string message);
        public void ShowReceiveMessage(string message)
        {

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
       
        private void button_get_URL_Click(object sender, EventArgs e)
        {

        }
    }
}
