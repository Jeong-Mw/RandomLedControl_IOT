using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Random r1 = new Random();

            RandomArray(r1, arrayLed);

            foreach(bool bo in arrayLed)
            {
                Console.WriteLine("{0,5}", bo);
            }
        }
    }
}
