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
        public bool[] arrayLed = new bool[4];
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
        static int RandomNumber(int min, int max)
        {
            Random random = new Random(); 
            
            return random.Next(min, max);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine(RandomNumber(0, 2));

            for (int i = 0; i < 4; i++)
            {
                if (RandomNumber(0, 2)==0)
                {
                    arrayLed[i] = true;
                }
                else
                {
                    arrayLed[i] = false;
                }
                Console.WriteLine(arrayLed[i]);
            }
        }
    }
}
