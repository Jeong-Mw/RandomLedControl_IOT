using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        #region 변수

        private Socket m_Client; //Receive
        private Socket cbSocket; //Send
        public const int bufferSize = 1024;
        private byte[] buffer = new byte[bufferSize];
        delegate void DsetRichText(string data);

        #endregion


        public bool swich;
        public bool[] arrayLed = new bool[3];
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (swich)
            {
                label1.Text = "ON";
                swich = false;
            }
            else
            {
                label1.Text = "OFF";
                swich = true;
            }

        }
        public void RandomArray(Random d1, bool[] result)
        {
            byte[] b1 = new byte[3];
            d1.NextBytes(b1);

            for (int i = 0; i <3; i++)
            {
                if (b1[i]%2==0)
                {
                    result[i] = true;
                }
                else
                {
                    result[i] = false;
                }
            }
            Console.WriteLine();

            arrayLed = result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Random r1 = new Random();

            RandomArray(r1, arrayLed);

            foreach(bool bo in arrayLed)
            {
                Console.WriteLine("{0,5}", bo);
            }
            /*for (int i = 0; i < 3; i++)
            {
                
            }*/
            if (arrayLed[0])
                label3.BackColor = Color.FromArgb(255, 210, 210);
            else
                label3.BackColor = Color.FromArgb(5, 10, 0);

            if (arrayLed[1])
                label4.BackColor = Color.FromArgb(255, 210, 210);
            else
                label4.BackColor = Color.FromArgb(5, 10, 0);

            if (arrayLed[2])
                label5.BackColor = Color.FromArgb(255, 210, 210);
            else
                label5.BackColor = Color.FromArgb(5, 10, 0);


            string result = "";

            foreach (bool led in arrayLed)
            {
                result += led.ToString();
            }
            byte[] message = Encoding.UTF8.GetBytes(result.ToString());
            string string1 = result.ToString();
            //서버와는 다른 방법으로 string을 전달해 봤다 그냥 공부하는겹 해봤다..
            m_Client.BeginSend(message, 0, message.Length, SocketFlags.None,
                new AsyncCallback(SendData), string1);
        }

        private void SendData(IAsyncResult iar)
        {
            string strSendData = (string)iar.AsyncState;
            SetShowMsg(strSendData + "\n");
        }


        private void button3_Click(object sender, EventArgs e)
        {
            //connect
            string strIP = text_ip.Text.ToString();
            string strPORT = text_port.Text.ToString();
            int iPORT = Convert.ToInt32(strPORT.ToString());

            SetLogMsg(strIP.ToString() + ":" + strPORT.ToString() + " 접속중...\n");
            m_Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IPE생성
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(strIP), iPORT);
            m_Client.BeginConnect(iep, new AsyncCallback(Connected), m_Client);
            button1.Enabled = false;
            button2.Enabled = true;
        }

        private void SetLogMsg(string strLogMsg)
        {
            if (richTextBox1.InvokeRequired)
            {
                DsetRichText _call = new DsetRichText(SetLogMsg);
                this.Invoke(_call, strLogMsg);
            }
            else
            {
                richTextBox1.AppendText("[" + DateTime.Now.ToString() + "] " + strLogMsg);
                richTextBox1.ScrollToCaret();
            }
        }


        private void Connected(IAsyncResult iar)
        {
            cbSocket = (Socket)iar.AsyncState;
            try
            {
                cbSocket.EndConnect(iar);
                string temp = cbSocket.RemoteEndPoint.ToString() + "에 연결 완료\n";
                SetLogMsg(temp);
                cbSocket.BeginReceive(buffer, 0, bufferSize, SocketFlags.None,
                    new AsyncCallback(ReceiveData), cbSocket);
            }
            catch (SocketException)
            {
                SetLogMsg("연결 실패\n");
            }
        }


        private void ReceiveData(IAsyncResult iar)
        {
            cbSocket = (Socket)iar.AsyncState;
            try
            {
                int recv = cbSocket.EndReceive(iar);
                if (recv == 0) return;
                string stringData = Encoding.UTF8.GetString(buffer, 0, recv);
                SetShowMsg(stringData + "\n");
                //수정중
                cbSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None,
                    new AsyncCallback(ReceiveData), cbSocket);

            }
            catch (Exception ex)
            {
                cbSocket.Close();
                SetShowMsg("강제종료" + "\n");
            }
        }

        

        private void SetShowMsg(string strShowMsg)
        {
            if (richTextBox2.InvokeRequired)
            {
                DsetRichText _call = new DsetRichText(SetShowMsg);
                this.Invoke(_call, strShowMsg);
            }
            else
            {
                richTextBox2.AppendText("[" + DateTime.Now.ToString() + "] " + strShowMsg);
                richTextBox2.ScrollToCaret();
            }
        }
    }
}
