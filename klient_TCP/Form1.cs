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
using System.Windows.Forms.DataVisualization.Charting;

namespace klient
{
    public partial class Form1 : Form
    {
        string ch_1, ch_2, ch_3;
        int[] port_serwera = new int[3];
        string[] ip_serwera = new string[3];
        int[] czas_Tc = new int[3];
        bool serwer1, serwer2, serwer3;
        string[] odebrane_dane1 = new string[30]; //[dane(x, y, host, ip, czas)]
        string[] odebrane_dane2 = new string[30]; //[dane(x, y, host, ip, czas)]
        string[] odebrane_dane3 = new string[30]; //[dane(x, y, host, ip, czas)]
        int[,,] xy = new int[3,2,30];   //[serwer],[0=x;1=y],[dane_historyczne]

        TcpListener listen1;
        TcpListener listen2;
        TcpListener listen3;

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

            Thread watek2 = new Thread(watek_glowny);
            watek2.Start();
        }

        private delegate void wyswietl_del();

        public void funkcja_odbieraj_serwer1()
        {
            //port1 = Int32.Parse(textBox_port1.Text);
            
            try
            {
                //                      1
                TcpClient client_1 = listen1.AcceptTcpClient();
                //client_1.ReceiveTimeout = 100;
                NetworkStream stream_1 = client_1.GetStream();

                byte[] buffer = new byte[client_1.ReceiveBufferSize];
                int data_1 = stream_1.Read(buffer, 0, client_1.ReceiveBufferSize);
                ch_1 = Encoding.Unicode.GetString(buffer, 0, data_1);
                this.BeginInvoke(new wyswietl_del(wyswietl_s1));
                //

                   // client_1.Close();
                   // stream_1.Close();      
            }

            catch (SocketException se)
            {
                //MessageBox.Show(se.Message);
            }
            catch (System.IO.IOException se)
            {
               // MessageBox.Show("serwer1" + se.Message);
            }
        }

        private void funkcja_odbieraj_serwer2()
        {
            try
            {
                //                      2
                TcpClient client_2 = listen2.AcceptTcpClient();
                //client_2.ReceiveTimeout = 1000;
                NetworkStream stream_2 = client_2.GetStream();
               

                byte[] buffer_2 = new byte[client_2.ReceiveBufferSize];
                int data_2 = stream_2.Read(buffer_2, 0, client_2.ReceiveBufferSize);
                ch_2 = Encoding.Unicode.GetString(buffer_2, 0, data_2);
                this.BeginInvoke(new wyswietl_del(wyswietl_s2));

                //
                stream_2.Close();
                client_2.Close();
            }

            catch (SocketException se)
            {
               // MessageBox.Show(se.Message);
            }
            catch(System.IO.IOException se)
            {
               // MessageBox.Show("serwer2"+se.Message);
            }
        }

        private void funkcja_odbieraj_serwer3()
        {
            try
            {
                //                      3
                TcpClient client_3 = listen3.AcceptTcpClient();
                //client_3.ReceiveTimeout = 100;
                NetworkStream stream_3 = client_3.GetStream();
                

                byte[] buffer_3 = new byte[client_3.ReceiveBufferSize];
                int data_3 = stream_3.Read(buffer_3, 0, client_3.ReceiveBufferSize);
                ch_3 = Encoding.Unicode.GetString(buffer_3, 0, data_3);
                this.BeginInvoke(new wyswietl_del(wyswietl_s3));
                
                //
                stream_3.Close();
                client_3.Close();
            }

            catch (SocketException se)
            {
               // MessageBox.Show(se.Message);
            }
            catch (System.IO.IOException se)
            {
               // MessageBox.Show("serwer3" + se.Message);
            }
        }


        private void watek_glowny()
        {
            
        }

        void wyswietl_s1()
        {
            textBox1.Text = ch_1;
        }

        void wyswietl_s2()
        {
            textBox2.Text = ch_2;
        }

        void wyswietl_s3()
        {
            textBox6.Text = ch_3;
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
        public void button1_Click(object sender, EventArgs e)
        {
            
            if (START_STOP)
            {
                START_STOP = false;
                button1.Text = "START";
                //zamykanie niepotrzebnych funkcji
                //client.Close();   
                timer1.Enabled = false;
                timer2.Enabled = false;
                timer3.Enabled = false;
                timer4.Enabled = false;
               
                //Thread.Sleep(5000);
                //stream_1.Close();
                //client_1.Close();
                listen1.Stop();
                listen2.Stop();
                listen3.Stop(); 
            }
            else
            {
                START_STOP = true;
                button1.Text = "STOP";
                port_serwera[0] = Int32.Parse(textBox_port1.Text);
                port_serwera[1] = Int32.Parse(textBox_port2.Text);
                port_serwera[2] = Int32.Parse(textBox_port3.Text);
                listen1 = new TcpListener(IPAddress.Any, port_serwera[0]);
                listen2 = new TcpListener(IPAddress.Any, port_serwera[1]);
                listen3 = new TcpListener(IPAddress.Any, port_serwera[2]);

                //różne ustawienia po wystartowaniu

               // ip_klienta = textBox7.Text;
               // port_klienta = Int32.Parse(textBox8.Text);
               // timer1.Interval = Int32.Parse(textBox9.Text);
                listen1.Start();
                listen2.Start();
                listen3.Start();
                timer1.Interval = trackBar1.Value;
                timer2.Interval = trackBar2.Value;
                timer3.Interval = trackBar3.Value;
                timer1.Enabled = serwer1;
                timer2.Enabled = serwer2;
                timer3.Enabled = serwer3;
                timer4.Enabled = true;  
            }
        }



        private void label2_Click(object sender, EventArgs e)
        {

        }

        public void timer1_Tick(object sender, EventArgs e)
        {
            Thread watek1 = new Thread(funkcja_odbieraj_serwer1);
            watek1.Start(); //serwer1
            // watek1.Priority = ThreadPriority.Highest;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Thread watek3 = new Thread(funkcja_odbieraj_serwer2);
            watek3.Start(); //serwer2
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            Thread watek4 = new Thread(funkcja_odbieraj_serwer3);
            watek4.Start(); //serwer3
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
                serwer1 = checkBox1.Checked;
                textBox_port1.Enabled = checkBox1.Checked;
                textBox3.Enabled = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

                serwer2 = checkBox2.Checked;
                textBox_port2.Enabled = checkBox2.Checked;
                textBox4.Enabled = checkBox2.Checked;

             /*   textBox_port2.Invoke(new Action(delegate()
                {
                    textBox_port2.Enabled = false;
                }));*/        
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
                serwer3 = checkBox3.Checked;
                textBox_port3.Enabled = checkBox3.Checked;
                textBox5.Enabled = checkBox3.Checked;
        }

        private void przed_zamknieciem_Formsa(object sender, FormClosingEventArgs e)
        {
            //żeby program zamykał się bez błędów
            timer1.Enabled = false;
            timer2.Enabled = false;
            timer3.Enabled = false;
            listen1.Stop();
            listen2.Stop();
            listen3.Stop(); 
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            //dane serwer1
            if (!(ch_1 == null))
            {
                ch_1.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                odebrane_dane1 = ch_1.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }

            //dane serwer2
            if (!(ch_2 == null))
            {
                ch_2.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                odebrane_dane2 = ch_2.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }

            //dane serwer3
            if (!(ch_3 == null))
            {
                ch_3.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                odebrane_dane3 = ch_3.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }

            
            for (int i = 29; i > 0; i-- )
            {
                xy[0, 0, i] = xy[0, 0, i-1];    //serwer1 x
                xy[0, 1, i] = xy[0, 1, i-1];    //serwer1 y

                xy[1, 0, i] = xy[1, 0, i - 1];   //serwer2 x
                xy[1, 1, i] = xy[1, 1, i - 1];  //serwer2 y

                xy[2, 0, i] = xy[2, 0, i - 1];   //serwer3 x
                xy[2, 1, i] = xy[2, 1, i - 1];  //serwer3 y
            }
            if (!(odebrane_dane1[0] == null))
            {
                xy[0, 0, 0] = Int32.Parse(odebrane_dane1[0]);   //serwer1 x0
                xy[0, 1, 0] = Int32.Parse(odebrane_dane1[1]);   //serwer1 y0
            }
            if (!(odebrane_dane2[0] == null))
            {
                xy[1, 0, 0] = Int32.Parse(odebrane_dane2[0]);   //serwer2 x0
                xy[1, 1, 0] = Int32.Parse(odebrane_dane2[1]);   //serwer2 y0
            }
            if (!(odebrane_dane3[0] == null))
            {
                xy[2, 0, 0] = Int32.Parse(odebrane_dane3[0]);   //serwer3 x0
                xy[2, 1, 0] = Int32.Parse(odebrane_dane3[1]);   //serwer3 y0
            }

            chart1.Series["serwer1_chart"].Points.Clear();
            chart1.Series["serwer2_chart"].Points.Clear();
            chart1.Series["serwer3_chart"].Points.Clear();
           
                for (int i = 0; i < 30; i++)
                {
                    chart1.Series["serwer1_chart"].Points.AddXY
                                    (xy[0, 0, i], -xy[0, 1, i]);
                    chart1.Series["serwer2_chart"].Points.AddXY
                                    (xy[1, 0, i], -xy[1, 1, i]);
                    chart1.Series["serwer3_chart"].Points.AddXY
                                    (xy[2, 0, i], -xy[2, 1, i]);
                }

                chart1.Series["serwer1_chart"].ChartType =
                                    SeriesChartType.Spline;
            chart1.Series["serwer1_chart"].Color = Color.Red;

            chart1.Series["serwer2_chart"].ChartType =
                                SeriesChartType.Spline;
            chart1.Series["serwer2_chart"].Color = Color.Green;

            chart1.Series["serwer3_chart"].ChartType =
                                SeriesChartType.Spline;
            chart1.Series["serwer3_chart"].Color = Color.Blue;

            chart1.ChartAreas[0].AxisY.Minimum = -(1080);
            chart1.ChartAreas[0].AxisY.Maximum = 0;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = 1920;


        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }





    }
}