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

namespace klient
{
    public partial class Form1 : Form
    {
        string ch_1, ch_2, ch_3;
        int[] port_serwera = new int[3];
        string[] ip_serwera = new string[3];
        int[] czas_Tc = new int[3];
        bool serwer1, serwer2, serwer3;

        TcpListener listen1 = new TcpListener(IPAddress.Any, 1200);
        TcpListener listen2 = new TcpListener(IPAddress.Any, 1201);
        TcpListener listen3 = new TcpListener(IPAddress.Any, 1202);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //żeby od startu pokazywała się dobra wartość czasu nastawionego
            label8.Text = trackBar1.Value.ToString();
            label9.Text = trackBar2.Value.ToString();
            label11.Text = trackBar3.Value.ToString();


            Thread watek1 = new Thread(funkcja_odbieraj);
            Thread watek2 = new Thread(watek_glowny);

            listen1.Start();
            listen2.Start();
            listen3.Start();

            watek1.Start();
            watek1.Priority = ThreadPriority.Highest;
            watek2.Start(); 
        }

        private delegate void wyswietl_del();

        private void funkcja_odbieraj()
        {
                while (true)
                {
                    //port1 = Int32.Parse(textBox_port1.Text);
                    try
                    {
                    //                      1
                        TcpClient client_1 = listen1.AcceptTcpClient();
                        client_1.ReceiveTimeout = 1000;
                        NetworkStream stream_1 = client_1.GetStream();
                        this.BeginInvoke(new wyswietl_del(funkcja2));

                        byte[] buffer = new byte[client_1.ReceiveBufferSize];
                        int data_1 = stream_1.Read(buffer, 0, client_1.ReceiveBufferSize);
                        ch_1 = Encoding.Unicode.GetString(buffer, 0, data_1);
                    //
                    //                      2
                        TcpClient client_2 = listen2.AcceptTcpClient();
                        client_2.ReceiveTimeout = 1000;
                        NetworkStream stream_2 = client_2.GetStream();
                        this.BeginInvoke(new wyswietl_del(funkcja2));

                        byte[] buffer2 = new byte[client_2.ReceiveBufferSize];
                        int data_2 = stream_2.Read(buffer2, 0, client_2.ReceiveBufferSize);
                        ch_2 = Encoding.Unicode.GetString(buffer, 0, data_2);
                    //
                    //                      3
                        TcpClient client_3 = listen3.AcceptTcpClient(); 
                        client_2.ReceiveTimeout = 1000;
                        NetworkStream stream_3 = client_3.GetStream();
                        this.BeginInvoke(new wyswietl_del(funkcja2));

                        byte[] buffer_3 = new byte[client_3.ReceiveBufferSize];
                        int data_3 = stream_3.Read(buffer_3, 0, client_3.ReceiveBufferSize);
                        ch_3 = Encoding.Unicode.GetString(buffer_3, 0, data_3);
                    //
                    stream_1.Close();
                        client_1.Close();
                    }

                    catch (SocketException se)
                    {
                        MessageBox.Show(se.Message);
                    }
            }
        }

        private void watek_glowny()
        {
            while (true)
            {
                try
                {
                    if (checkBox1.Checked == true)
                    {
                        serwer1 = true;
                        //textBox_port1.Enabled = true;
                        textBox_port1.Invoke(new Action(delegate ()
                        {
                            textBox_port1.Enabled = true;
                        }));
                    }
                    else
                    {
                        serwer1 = false;
                        //textBox_port1.Enabled = false;
                        textBox_port1.Invoke(new Action(delegate ()
                        {
                            textBox_port1.Enabled = false;
                        }));
                    }
                    if (checkBox2.Checked == true)
                    {
                        serwer2 = true;
                        //textBox_port2.Enabled = true;
                        textBox_port2.Invoke(new Action(delegate ()
                        {
                            textBox_port2.Enabled = true;
                        }));
                    }
                    else
                    {
                        serwer2 = false;
                        //textBox_port2.Enabled = false;
                        textBox_port2.Invoke(new Action(delegate ()
                        {
                            textBox_port2.Enabled = false;
                        }));
                    }
                    if (checkBox3.Checked == true)
                    {
                        serwer3 = true;
                        //textBox_port3.Enabled = true;
                        textBox_port3.Invoke(new Action(delegate ()
                        {
                            textBox_port3.Enabled = true;
                        }));
                    }
                    else
                    {
                        serwer3 = false;
                        //textBox_port3.Enabled = false;
                        textBox_port3.Invoke(new Action(delegate ()
                        {
                            textBox_port3.Enabled = false;
                        }));
                    }
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
            textBox2.Text = "Porty w serwerach ustawić na sztywno: 1200, 1201 i 1202" + Environment.NewLine +
                            Environment.NewLine + ch_1 + Environment.NewLine + ch_2 + Environment.NewLine + ch_3;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            label8.Text = trackBar1.Value.ToString();
            //dodać wartość->timer-------------------------
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            label9.Text = trackBar2.Value.ToString();
            //dodać wartość->timer-------------------------
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            label11.Text = trackBar3.Value.ToString();
            //dodać wartość->timer-------------------------
        }

        // koncepcja wyszukiwania serwerów
        static bool START_STOP = false;
        private void button1_Click(object sender, EventArgs e)
        {
            if (START_STOP)
            {
                START_STOP = false;
                button1.Text = "START";
                //client.Close();   
            }
            else
            {
                START_STOP = true;
                button1.Text = "STOP";
                //różne ustawienia po wystartowaniu
               // ip_klienta = textBox7.Text;
               // port_klienta = Int32.Parse(textBox8.Text);
               // timer1.Interval = Int32.Parse(textBox9.Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Pierwsza zasrana zmiana z Gitem");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }



    }
}