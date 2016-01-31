using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace serwer1
{
    public partial class Form1 : Form
    {
        string ch;

        TcpListener listen = new TcpListener(IPAddress.Any, 1200);
        
        public Form1()
        {
            InitializeComponent();

            Thread watek1 = new Thread(funkcja_odbieraj);
            Thread watek2 = new Thread(watek_glowny);

            listen.Start();
            watek1.Start();
            watek1.Priority = ThreadPriority.Highest;
            watek2.Start();     
        }

        private delegate void wyswietl_del();

        private void funkcja_odbieraj()
        {
                while (true)
                {
                    try
                    {
                        TcpClient client = listen.AcceptTcpClient();
                        client.ReceiveTimeout = 1000;
                        NetworkStream stream = client.GetStream();
                        this.BeginInvoke(new wyswietl_del(funkcja2));

                        byte[] buffer = new byte[client.ReceiveBufferSize];
                        int data = stream.Read(buffer, 0, client.ReceiveBufferSize);
                        ch = Encoding.Unicode.GetString(buffer, 0, data);

                        stream.Close();
                        client.Close();
                    }

                    //Thread.Sleep(5000);

                    catch (SocketException se)
                    {
                        MessageBox.Show(se.Message);
                    }
            }
           
        }
        bool serwer1, serwer2, serwer3;
        private void watek_glowny()
        {
            while (true)
            {
                try
                {
                    if (checkBox1.Checked == true)
                    {
                        serwer1 = true;
                    }
                    else
                        serwer1 = false;
                    if (checkBox2.Checked == true)
                    {
                        serwer2 = true;
                    }
                    else
                        serwer2 = false;
                    if (checkBox3.Checked == true)
                    {
                        serwer3 = true;
                    }
                    else
                        serwer3 = false;
                    //this.Invoke(new wyswietl_del(funkcja3));
                }
                catch (SocketException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void funkcja3()
        {
            //textBox1.Text = serwer1.ToString();
        }

        private void funkcja2()
        {
            textBox2.Text = ch;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            textBox1.Text = trackBar1.Value.ToString();
        }

        // koncepcja wyszukiwania serwerów
        static bool a = false;
        private void button1_Click(object sender, EventArgs e)
        {
            // można dodać coś, co wykonuje się bistabilnie
            a ^= true;
            if(a == true)
            {
                MessageBox.Show(a.ToString());
            }
            else
            {
                MessageBox.Show(a.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Pierwsza zasrana zmiana z Gitem");
            //test666 BA
        }
    }
}