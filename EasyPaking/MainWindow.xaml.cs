using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ParkingSystem;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EasyPaking
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// <remarks>serialPort State</remarks>
        /// </summary>
        private bool commState = false;
        /// <summary>
        /// <remarks>Ethernet State</remarks>
        /// </summary>
        private bool ethNetOpenState = false;
        /// <summary>
        /// <remarks>SerialPort Instantiation</remarks>
        /// </summary>
        public static ParkingSystem.ParkingSerialPort serialPort = new ParkingSerialPort();
        /// <summary>
        /// <remarks>TCP Instantiation</remarks>
        /// </summary>
        public static ParkingSystem.ParkingRemoteTCP wapper = new ParkingRemoteTCP();
        /// <summary>
        /// <remarks>tcp Listener</remarks>
        /// </summary>
        private static TcpListener tcpListener = null;
        /// <summary>
        /// <remarks>IP Address</remarks>
        /// </summary>
        private static IPAddress localIP;
        /// <summary>
        /// <remarks>TCP portNum</remarks>
        /// </summary>
        private static UInt16 portNum;
        /// <summary>
        /// <remarks>Tcp Client</remarks>
        /// </summary>
        private static TcpClient client = new TcpClient();
        /// <summary>
        /// <remarks>Tcp Thread</remarks>
        /// </summary>
        private Thread m_serverThread;
        public MainWindow()
        {
            InitializeComponent();
        }
        public void MainWindow_Loaded(object sender, RoutedEventArgs e) {
            CommInit();
            EthCommInit();
            ParkingOriginalPacket.EvProcessReceivedPacket += sp_ProcessReceivedPacket;
        }
        
        #region SerialPort Operation
        /// <summary>
        /// SerialPort Initialize
        /// </summary>
        private void CommInit()
        {
            foreach (string name in serialPort.portNames)
            {
                if (!CommPort_comboBox.Items.Contains(name))
                {
                    CommPort_comboBox.Items.Add(name);
                }
                CommPort_comboBox.Text = name;
            }

            CommBaud_comboBox.Text = "115200";
        }
        /// <summary>
        /// Set SerialPort Open
        /// </summary>
        private void SetOpen()
        {
            OpenClosePort_Button.Content = "Close  Comm";
            CommStatus_label.Foreground = Brushes.Lime;
            commState = true;
        }
        /// <summary>
        /// Set SerialPort Close
        /// </summary>
        private void SetClose()
        {
            OpenClosePort_Button.Content = "Open  Comm";
            CommStatus_label.Foreground = Brushes.DarkGray;
            commState = false;
        }
        /// <summary>
        /// Open Close SerialPort Oper
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void OpenClosePort_Button_Click(object sender, EventArgs e)
        {
            try
            {
                if (OpenClosePort_Button.Content.ToString() == "Open  Comm")
                {
                    serialPort = new ParkingSerialPort(CommPort_comboBox.Text, Convert.ToInt32(CommBaud_comboBox.Text));
                    serialPort.Open();
                    SetOpen();
                }
                else if (OpenClosePort_Button.Content.ToString() == "Close  Comm")
                {
                    if (!serialPort.IsOpen)
                    {
                        SetClose();
                        return;
                    }
                    serialPort.Close();
                    SetClose();

                }


            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                SetClose();
                return;
            }
        }
        #endregion


        #region Device Operation
        /// <summary>
        /// Get device name
        /// </summary>
        /// <param name="devname">devicetype</param>
        /// <returns>devicename</returns>
        public string GetDevName(byte byteName)
        {
            string devname = "";
            switch (byteName)
            {
                case 0x01:
                    devname = "WDC-4003";
                    break;
                case 0x02:
                    devname = "WDC-4005";
                    break;
                case 0x03:
                    devname = "WDC-4008";
                    break;
                case 0x04:
                    devname = "WDC-4007";
                    break;
                case 0x05:
                    devname = "WPSD-340S3";
                    break;
                case 0x06:
                    devname = "WPSD-340S5";
                    break;
                case 0x07:
                    devname = "WPSD-340S8";
                    break;
                case 0x08:
                    devname = "WPSD-340S7";
                    break;
                case 0x09:
                    devname = "WPSD-340E3";
                    break;
                case 0x0A:
                    devname = "WPSD-340E5";
                    break;
                case 0x0B:
                    devname = "WPSD-340E8";
                    break;
                case 0x0C:
                    devname = "WPSD-340E7";
                    break;

                default:
                    devname = "WDC-400x";
                    break;
            }
            return devname;
        }
        #endregion

        #region Ethernet Operation
        /// <summary>
        /// Ethernet Initialize
        /// </summary>
        private void EthCommInit()
        {
            string addresses = GetLocalAddresses();
            severIPcomboBox.Items.Clear();
            if (addresses.Length > 0)
            {

                severIPcomboBox.Items.Add(addresses);

                severIPcomboBox.Text = (string)severIPcomboBox.Items[0];
            }
        }
        /// <summary>
        /// Get Local IP Address
        /// </summary>
        public string GetLocalAddresses()
        {
            // Get hostname
            string strHostName = Dns.GetHostName();
            System.Net.IPAddress addr;
            addr = new System.Net.IPAddress(Dns.GetHostByName(Dns.GetHostName()).AddressList[0].Address);
            return addr.ToString();
        }
        /// <summary>
        /// Set Ethernet Close
        /// </summary>
        private void eth_Setclose()
        {
            connectstatelabel.Text = "Waiting  Connect...";
            ethNetOpenState = false;
            listenstatelabel.Foreground = Brushes.DarkGray;
            startListenbutton.Content = "Start  Listen";
            deviceIPstatelabel.Text = "";

        }
        /// <summary>
        /// startListenbutton_Click Oper
        /// </summary>
        /// /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void startListenbutton_Click(object sender, RoutedEventArgs e)
        {
            deviceIPstatelabel.Text = "";
            try
            {
                if (severPorttextBox.Text != "")
                {
                    if (startListenbutton.Content.ToString() == "Start  Listen")
                    {
                        portNum = UInt16.Parse(severPorttextBox.Text);
                        if (portNum > 0 && portNum < 65535)
                        {
                            TCP_StartListen();
                            ethNetOpenState = true;
                            startListenbutton.Content = "Stop  Listen";
                            connectstatelabel.Text = "Listening...";
                        }
                        else
                        {
                            MessageBox.Show("Port Range:1~65535!");
                            return;
                        }

                    }
                    else
                    {
                        try
                        {
                            if (connectstatelabel.Text == "Listening...")
                            {
                                tcpListener.Stop();
                            }
                            else
                            {
                                tcpListener.Stop();
                                m_serverThread.Abort();
                                m_serverThread = null; 

                                client.Close();
                            }
                            eth_Setclose();
                        }
                        catch (SocketException ex)
                        {
                            MessageBox.Show("TCP Server Listen Error!" + ex.Message);
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Please input Port Adress!");
                }
            }
            catch (ThreadAbortException exl)
            {
                MessageBox.Show("TCP Server Button Error:" + exl.ToString());
            }
            catch (SocketException se)           
            {
                MessageBox.Show("TCP Server Listen Error:" + se.Message);
            }
            catch (Exception ex)
            {
                tcpListener.Stop();
                client.Close();
                MessageBox.Show("TCP Server Button Error:" + ex.ToString());
            }
        }
        /// <summary>
        /// Start TCP Listening
        /// </summary>
        public void TCP_StartListen()
        {
            try
            {
                localIP = IPAddress.Parse(this.severIPcomboBox.Text);
                tcpListener = new TcpListener(localIP, portNum);
                tcpListener.Start();
                m_serverThread = new Thread(new ThreadStart(ReceiveAccept));
                m_serverThread.Start();
                m_serverThread.IsBackground = true;

            }
            catch (SocketException ex)
            {
                tcpListener.Stop();
                eth_Setclose();
                MessageBox.Show("TCP Server Listen Error:" + ex.Message);
            }
            catch (Exception err)
            {
                MessageBox.Show("TCP Server Listen Error:" + err.Message);
            }
        }
        /// <summary>
        /// TCP Client Accept
        /// </summary>
        private void ReceiveAccept()
        {
            while (true)
            {
                try
                {
                    client = tcpListener.AcceptTcpClient();
                    this.Dispatcher.Invoke((Action)(()=>{
                        this.listenstatelabel.Foreground = Brushes.Lime;
                        this.connectstatelabel.Text = "connect ok";
                        this.deviceIPstatelabel.Text = client.Client.RemoteEndPoint.ToString();
                        ethNetOpenState = true;
                    }));
                    wapper = new ParkingRemoteTCP(client);
                }
                catch (Exception ex)
                {
                    eth_Setclose();
                    //MessageBox.Show(ex.Message, "?", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show("275 строка ошибка");
                }
            }
        }
        #endregion

        #region Received Process
        /// <summary>
        /// <remarks>Process Received Packet</remarks>
        /// </summary>
        /// <param name="pk">Received Packet</param>
        private async void sp_ProcessReceivedPacket(baseReceivedPacket pk)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var producer = new ProducerBuilder<Null, string>(config).Build();
            try
            {
                byte revType = Convert.ToByte(pk.type_ver >> 8);
                string wpsdid = "";
                string WDCid = "";
                string RSSI = "";
                byte carState = 0;
                string voltage = "";
                string hardVer = "";
                string softVer = "";
                string deviceName = "";
                string hbPeriod = "";
                this.Dispatcher.Invoke((Action)(async () =>
                {
                    #region Senser Heart Beat
                    if (pk is SensorHBeat)
                    {
                        SensorHBeat hb = (SensorHBeat)pk;
                        reshow(hb.recRawData, true);
                        wpsdid = (hb.WPSD_ID).ToString("X2").PadLeft(8, '0');
                        WDCid = (hb.WDC_ID).ToString("X2").PadLeft(8, '0');
                        softVer = "v" + int.Parse(hb.APP_VER.ToString("X2").Substring(0, 1)).ToString() + "." + int.Parse(hb.APP_VER.ToString("X2").Substring(1, 1)).ToString().PadLeft(2, '0');
                        hardVer = ((int)(hb.HARD_VER) + 10).ToString();
                        hardVer = "v" + hardVer.Substring(0, 1) + "." + hardVer.Substring(1, 1);
                        voltage = (Math.Round((decimal)hb.VOLT / 10, 2)).ToString() + "V"; RSSI = ((Int16)hb.RSSI - 30).ToString();
                        hbPeriod = hb.HB_PERIOD.ToString();
                        deviceName = GetDevName(hb.DEV_TYPE);
                        carState = hb.CAR_STATE;
                        var parkingSensor = new ParkingSensor(wpsdid);
                        if (carState == 0x01)
                        {
                            richTextBox1.AppendText("wpsd id:" + wpsdid + "\nsoft Ver:" + softVer + "\nhard Ver:" + hardVer + "\nvoltage:" + voltage + "\nRSSI:" + RSSI + "\ncar State:Have Car\n");
                            lotColor.Foreground = Brushes.Red;
                            parkingSensor.setIsOcuppied(true);
                        }
                        else
                        {
                            lotColor.Foreground = Brushes.Lime;
                            richTextBox1.AppendText("wpsd id:" + wpsdid + "\nsoft Ver:" + softVer + "\nhard Ver:" + hardVer + "\nvoltage:" + voltage + "\nRSSI:" + RSSI + "\ncar State:No Car\n");
                            parkingSensor.setIsOcuppied(false);
                        }

                        var jsonSerializerSettings = new JsonSerializerSettings
                        {
                            ContractResolver = new DefaultContractResolver 
                            {
                                NamingStrategy = new CamelCaseNamingStrategy()
                            }
                        };
                        var serializedObject = JsonConvert.SerializeObject(parkingSensor, jsonSerializerSettings);
                        var message = new Message<Null, string> { Value = serializedObject };
                        var deliveryResult = await producer.ProduceAsync("parking-sensor-topic", message);
                        Console.WriteLine($"Delivered '{deliveryResult.Value}' to '{deliveryResult.TopicPartitionOffset}'");
                    }
                    #endregion
                    #region Senser Detect
                    else if (pk is SensorDetect)
                    {
                        SensorDetect decbeat = (SensorDetect)pk;
                        reshow(decbeat.recRawData, true);
                        wpsdid = (decbeat.WPSD_ID).ToString("X2").PadLeft(8, '0');
                        WDCid = (decbeat.WDC_ID).ToString("X2").PadLeft(8, '0');
                        hardVer = ((int)(decbeat.HARD_VER) + 10).ToString();
                        hardVer = "v" + hardVer.Substring(0, 1) + "." + hardVer.Substring(1, 1);
                        deviceName = GetDevName(decbeat.DEV_TYPE);
                        carState = decbeat.CAR_STATE;
                        var parkingSensor = new ParkingSensor(wpsdid);
                        if (carState == 0x01)
                        {
                            richTextBox1.AppendText("wpsd id:" + wpsdid + "\nhard Ver:" + hardVer + "\ncar State:Have Car\n");
                            lotColor.Foreground = Brushes.Red;
                            parkingSensor.setIsOcuppied(true);
                        }
                        else
                        {
                            richTextBox1.AppendText("wpsd id:" + wpsdid + "\nhard Ver:" + hardVer + "\ncar State:No Car\n");
                            lotColor.Foreground = Brushes.Lime;
                            parkingSensor.setIsOcuppied(false);
                        }
                        var jsonSerializerSettings = new JsonSerializerSettings
                        {
                            ContractResolver = new DefaultContractResolver
                            {
                                NamingStrategy = new CamelCaseNamingStrategy()
                            }
                        };
                        var serializedObject = JsonConvert.SerializeObject(parkingSensor, jsonSerializerSettings);
                        var message = new Message<Null, string> { Value = serializedObject };
                        var deliveryResult = await producer.ProduceAsync("parking-sensor-topic", message);

                        Console.WriteLine($"Delivered '{deliveryResult.Value}' to '{deliveryResult.TopicPartitionOffset}'");
                    }
                    #endregion
                }));
            }
            catch (ProduceException<Null, string> e) 
            { 
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static async Task SendParkingSensorData(ParkingSensor parkingSensor)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var producer = new ProducerBuilder<Null, string>(config).Build();
            try
            {
                var serializedObject = JsonConvert.SerializeObject(parkingSensor);
                var message = new Message<Null, string> { Value = serializedObject };
                var deliveryResult = await producer.ProduceAsync("parking-sensor-topic", message);
                Console.WriteLine($"Delivered '{deliveryResult.Value}' to '{deliveryResult.TopicPartitionOffset}'");
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// <remarks>Received Data Show</remarks>
        /// </summary>
        /// <param name="text">Received data</param>
        public void reshow(byte[] text, bool source)
        {
            string restr = "";
            if (text != null)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    restr += text[i].ToString("X2");
                    restr += " ";
                }
            }
            if (source)
            {
                richTextBox1.AppendText(System.DateTime.Now.ToString() + "[Received]:  " + restr + "\n");
            }
            else { richTextBox1.AppendText(System.DateTime.Now.ToString() + "[Send]:  " + restr + "\n"); }
        }

        #endregion
        private void exitbutton_Click(object sender, RoutedEventArgs e) {
            try
            {

                System.Environment.Exit(System.Environment.ExitCode);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }/*
        private void Form1_FormClosing(object sender, RoutedEventArgs e)
        {
            try
            {

                System.Environment.Exit(System.Environment.ExitCode);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }*/
    }
}
