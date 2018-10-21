using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FTD2XX_NET;
using System.IO;
using System.Threading;
using System.Runtime.CompilerServices;

namespace FtdiFifo
{
    public partial class Form1 : Form
    {
        private FTDI ftHandle = new FTDI();
        private FTDI.FT_DEVICE_INFO_NODE[] deviceInfos = new FTDI.FT_DEVICE_INFO_NODE[2];
        private string serialNumber = new string(' ', 32);
        private byte[] data = new byte[4096];
        private byte[] recData = new byte[4096];

        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 4096; i++)
            {
                data[i] = (byte)(i % 256);
            }
            data[0] = 34;
            data[1] = 35;
        }

        private void IdentifyDevice()
        {
            FTDI.FT_STATUS status;
            uint devcount = 0;
            ftHandle.GetNumberOfDevices(ref devcount);
            if (devcount > 0)
            {
                status = ftHandle.GetDeviceList(deviceInfos);
                if (status == FTDI.FT_STATUS.FT_OK)
                {
                    for (int i = 0; i < devcount; i++)
                    {
                        deviveComboBox.Items.Add(deviceInfos[i].Description);
                    }
                }
                status = ftHandle.GetSerialNumber(out serialNumber);
            }
        }

        private void SetFifoMode()
        {
            FTDI.FT_STATUS status;
            //status = ftHandle.OpenBySerialNumber(serialNumber);
            status = ftHandle.OpenByIndex(0);
            if (status != FTDI.FT_STATUS.FT_OK)
            {
                MessageBox.Show("Cannot open device");
                return;
            }
            ftHandle.SetBitMode(0xff, 0x00);
            System.Threading.Thread.Sleep(10);
            status = ftHandle.SetBitMode(0xff, 0x40);
            if (status != FTDI.FT_STATUS.FT_OK)
            {
                MessageBox.Show("Cannot set status");
                return;
            }
            ftHandle.SetLatency(16);
            ftHandle.InTransferSize(0x10000);
            ftHandle.SetFlowControl(FTDI.FT_FLOW_CONTROL.FT_FLOW_RTS_CTS, 0, 0);
 
        }


        volatile uint rxQueue = 0;
        [MethodImpl(MethodImplOptions.NoOptimization|MethodImplOptions.NoInlining)]
        private void SendDataIntoFifo()
        {
            FTDI.FT_STATUS status;
            uint txQueue = 0;
            
            // check buffer
            status = ftHandle.GetTxBytesWaiting(ref txQueue);
            if (txQueue == 0)
            {
                // write data
                uint written = 0;
                ftHandle.Write(data, data.Length, ref written);
                if (written == 0)
                {
                    MessageBox.Show("No data have been written");
                    return;
                }
            }
            Thread.Sleep(20);
            status = ftHandle.GetRxBytesAvailable(ref rxQueue);
            if (rxQueue > 0 && status == FTDI.FT_STATUS.FT_OK)
            {
                // read data
                uint read = 0;
                ftHandle.Read(recData, (uint)rxQueue, ref read);
                if (read == 0)
                {
                    MessageBox.Show("No data have been read from FIFO");
                    return;
                }
            }
            //status = ftHandle.GetTxBytesWaiting(ref txQueue);
            //if (txQueue == 0)
            //{
            //    // write data
            //    uint written = 0;
            //    ftHandle.Write(data, data.Length, ref written);
            //    if (written == 0)
            //    {
            //        MessageBox.Show("No data have been written");
            //        return;
            //    }
            //}
        }

        private void SendDataOverFifo()
        {
            FTDI.FT_STATUS status;
            uint txQueue = 0;
            uint rxQueue = 0;
            // check buffer
            //status = ftHandle.GetTxBytesWaiting(ref txQueue);
            //if (txQueue == 0)
            //{
            //    // write data
            //    uint written = 0;
            //    ftHandle.Write(data, data.Length, ref written);
            //    if (written == 0)
            //    {
            //        MessageBox.Show("No data have been written");
            //        return;
            //    }
            //}
            //Thread.Sleep(10);
            // check read buffer
            status = ftHandle.GetRxBytesAvailable(ref rxQueue);
            if (rxQueue > 0)
            {
                // read data
                uint read = 0;
                ftHandle.Read(recData, (uint)rxQueue, ref read);
                if (read == 0)
                {
                    MessageBox.Show("No data have been read from FIFO");
                    return;
                }
            }
        }

        private void ReadPackageSequence()
        {
            FTDI.FT_STATUS status;
            uint rxQueue = 0;
            for (int i = 0; i < 256; i++)
            {
                status = ftHandle.GetRxBytesAvailable(ref rxQueue);
                if (rxQueue > 0 && status == FTDI.FT_STATUS.FT_OK)
                {
                    uint read = 0;
                    ftHandle.Read(recData, (uint)recData.Length, ref read);
                    // check data consistency
                    byte startIndex = (byte)i;
                    byte curValue = startIndex;
                    for (int j = 0; j < recData.Length; j++)
                    {
                        if (curValue != recData[j])
                        {
                            MessageBox.Show(string.Format("Incorrect value received in {0} package in {j} position", i, j));
                            return;
                        }
                        curValue++;
                    }
                }
            }
        }

        private void TestReliability()
        {
            Thread thread = new Thread((ThreadStart)delegate
            {
                FTDI.FT_STATUS status;
                uint txQueue = 0;
                uint rxQueue = 0;
                int iterCount = 0;
                for (iterCount = 0; iterCount < 1000000; iterCount++)
                {
                    // check buffer
                    status = ftHandle.GetTxBytesWaiting(ref txQueue);
                    if (txQueue == 0)
                    {
                        // write data
                        uint written = 0;
                        ftHandle.Write(data, data.Length, ref written);
                        if (written == 0)
                        {
                            MessageBox.Show("No data have been written");
                            return;
                        }
                    }
                    // check read buffer
                    status = ftHandle.GetRxBytesAvailable(ref rxQueue);
                    if (rxQueue > 0)
                    {
                        // read data
                        uint read = 0;
                        ftHandle.Read(recData, (uint)rxQueue, ref read);
                        if (read == 0)
                        {
                            MessageBox.Show("No data have been read from FIFO");
                            return;
                        }
                    }
                    //iterCountLabel.Text = iterCount.ToString();
                    iterCountLabel.Invoke((MethodInvoker)delegate
                    {
                        iterCountLabel.Text = iterCount.ToString();
                    });
                }
            });
            thread.Start();
        }

        private void MeasureSpeed()
        {
            byte[] masdata = new byte[4096]; // start array 
            masdata[0] = (byte)0x03;
            for (int i = 0; i < masdata.Length; i++)
            {
                masdata[i] = (byte)(i % 256);
            }
            byte[] bufdata = new byte[4096]; // array with intermediate data
            byte[] resmasdata = new byte[4096]; // array for receiving data
            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < 16; i++)
            {
                // write data
                uint written = 0;
                ftHandle.Write(masdata, masdata.Length, ref written);
                if (written != masdata.Length)
                {
                    uint startIndex = written;
                    uint copySectionSize = 4096 - written;
                    System.Array.Copy(masdata, startIndex, bufdata, 0, copySectionSize);
                    ftHandle.Write(bufdata, (int)copySectionSize, ref written);
                    while (copySectionSize != written)
                    {
                        startIndex = written;
                        copySectionSize = copySectionSize - written;
                        System.Array.Copy(bufdata, startIndex, bufdata, 0, copySectionSize);
                        ftHandle.Write(bufdata, (int)copySectionSize, ref written);
                    }
                }
                // read data
                FileStream resFile = File.Create("recdata.txt");
                StreamWriter writer = new StreamWriter(resFile);
                uint available = 0;
                uint read = 0;
                ftHandle.GetRxBytesAvailable(ref available);
                while (available != 0)
                {
                    ftHandle.Read(resmasdata, available, ref read);
                    for (int j = 0; j < read; j++)
                    {
                        writer.WriteLine(resmasdata[j]);
                    }
                    if (read != available)
                    {
                        // not all data have been read
                        ftHandle.GetRxBytesAvailable(ref available);
                    }
                    ftHandle.GetRxBytesAvailable(ref available);
                }
                writer.Flush();
                writer.Close();
                resFile.Close();
            }
            var elapsedMs = watch.ElapsedMilliseconds;
            speedLabel.Text = ((4096 * 16 * 2) / elapsedMs * 1000).ToString() + " bytes/s"; // bytes / (ms*1000)
        }

        private AutoResetEvent receivedDataEvent;
        private BackgroundWorker worker;

        private void TestReception() 
        {
            // set up asynchronous reception
            //receivedDataEvent = new AutoResetEvent(false);
            //FTDI.FT_STATUS status = ftHandle.SetEventNotification(FTDI.FT_EVENTS.FT_EVENT_RXCHAR, receivedDataEvent);
            //worker = new BackgroundWorker();
            //worker.DoWork += ReadData;
            //if (!worker.IsBusy)
            //{
            //    worker.RunWorkerAsync();
            //}
            //worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            Thread backThread = new Thread(() => { this.ReadData(); });
            backThread.Start();
        }

        private void ReadData()
        {
            uint available = 0;
            uint read = 0;
            received = 0;
            FTDI.FT_STATUS status;
            // send data
            uint written = 0;
            byte[] masdata = new byte[4096]; // start array 
            masdata[0] = (byte)0x03;
            for (int i = 0; i < masdata.Length; i++)
            {
                masdata[i] = (byte)(i % 256);
            }
            for (int i = 0; i < 1; i++)
            {
                ftHandle.Write(masdata, masdata.Length, ref written);
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();
            long[] timeValues = new long[1000000];
            int timeIndex = 0;

            while (received < totalRecBytes)
            {
                //receivedDataEvent.WaitOne();
                status = ftHandle.GetRxBytesAvailable(ref available);
                if (status != FTDI.FT_STATUS.FT_OK)
                {
                    break;
                }
                if (available > 0)
                {
                    status = ftHandle.Read(resmasdata, available, ref read);
                    // copy data to larger array
                    if (received + read < totalRecBytes)
                        Array.Copy(resmasdata, 0, bufMemory, (int)received, (int)read);
                    received += read;
                }
                timeValues[timeIndex++] = watch.ElapsedMilliseconds;
            }
            MessageBox.Show(String.Format("{0}", timeIndex));
            // write data to the file
            FileStream resFile = File.Create("rectest.txt");
            StreamWriter writer = new StreamWriter(resFile);
            for (int i = 0; i < totalRecBytes; i++)
            {
                writer.WriteLine("{0:X}", bufMemory[i]);
                //writer.WriteLine(bufMemory[i]);
            }
            writer.Flush();
            writer.Close();
            resFile.Close();
            // write time values to the file
            FileStream timeFile = File.Create("timetest.txt");
            writer = new StreamWriter(timeFile);
            for (int i = 0; i < timeValues.Length; i++)
            {
                writer.WriteLine("{0}", timeValues[i]);
            }
            writer.Flush();
            writer.Close();
            timeFile.Close();
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Received all data");
            if (e.Cancelled)
            {
                MessageBox.Show("Operation cancelled");
            }
        }

        uint totalRecBytes = 4096 * 2000; // 400 Kbytes
        byte[] resmasdata = new byte[4096];
        byte[] bufMemory = new byte[4096 * 2000];
        uint received = 0;
        private string dataFileName;

        private void ReadData(object sender, DoWorkEventArgs args)
        {
            
            uint available = 0;
            uint read = 0;
            received = 0;
            FTDI.FT_STATUS status;
            // send data
            uint written = 0;
            byte[] masdata = new byte[4096]; // start array 
            masdata[0] = (byte)0x03;
            for (int i = 0; i < masdata.Length; i++)
            {
                masdata[i] = (byte)(i % 256);
            }
            for (int i = 0; i < 1; i++)
            {
                ftHandle.Write(masdata, masdata.Length, ref written);
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();
            long[] timeValues = new long[1000];
            int timeIndex = 0;

            while (received < totalRecBytes)
            {
                //receivedDataEvent.WaitOne();
                status = ftHandle.GetRxBytesAvailable(ref available);
                if (status != FTDI.FT_STATUS.FT_OK)
                {
                    break;
                }
                if (available > 0)
                {
                    status = ftHandle.Read(resmasdata, available, ref read);
                    // copy data to larger array
                    Array.Copy(resmasdata, 0, bufMemory, (int)received, (int)read);
                    received += read;
                }
                args.Result = timeIndex;
                timeValues[timeIndex++] = watch.ElapsedMilliseconds;
            }
            MessageBox.Show(String.Format("{0}", timeIndex));
            // write data to the file
            FileStream resFile = File.Create("rectest.txt");
            StreamWriter writer = new StreamWriter(resFile);
            for (int i = 0; i < totalRecBytes; i++)
            {
                writer.WriteLine("{0:X}", bufMemory[i]);
                //writer.WriteLine(bufMemory[i]);
            }
            writer.Flush();
            writer.Close();
            resFile.Close();
            // write time values to the file
            FileStream timeFile = File.Create("timetest.txt");
            writer = new StreamWriter(timeFile);
            for (int i = 0; i < timeValues.Length; i++)
            {
                writer.WriteLine("{0}", timeValues[i]);
            }
            writer.Flush();
            writer.Close();
            timeFile.Close();
        }

        private void identifyButton_Click(object sender, EventArgs e)
        {
            IdentifyDevice();
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            SetFifoMode();
        }

        private void transmitButton_Click(object sender, EventArgs e)
        {
            SendDataOverFifo();
        }

        private void measureButton_Click(object sender, EventArgs e)
        {
            MeasureSpeed();
        }

        private void receiveContButton_Click(object sender, EventArgs e)
        {
            TestReception();
        }

        private void testReliabilityButton_Click(object sender, EventArgs e)
        {
            TestReliability();
        }

        private void readSequenceButton_Click(object sender, EventArgs e)
        {
            ReadPackageSequence();
        }

        private void sendDataButton_Click(object sender, EventArgs e)
        {
            SendDataIntoFifo();
        }

        private void selectFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text files | *.txt";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                fileTextBox.Text = dialog.FileName;
                dataFileName = dialog.FileName;
            }
        }

        private void sendFileDataButton_Click(object sender, EventArgs e)
        {
            string[] lines = File.ReadAllLines(dataFileName);
            byte[] dataArray = new byte[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                dataArray[i] = byte.Parse(lines[i]);
            }
            uint txQueue = 0;
            ftHandle.GetTxBytesWaiting(ref txQueue);
            if (txQueue == 0)
            {
                uint written = 0;
                ftHandle.Write(dataArray, dataArray.Length, ref written);
            }
            else
            {
                MessageBox.Show("Some data is waiting");
            }
            // sleep to make sure that data have been processed
            Thread.Sleep(50);
            uint rxQueue = 0;
            ftHandle.GetRxBytesAvailable(ref rxQueue);
            if (rxQueue > 0)
            {
                uint read = 0;
                ftHandle.Read(recData, rxQueue, ref read);
                FileStream timeFile = File.Create("ReceivedValuesFile.txt");
                StreamWriter writer = new StreamWriter(timeFile);
                for (int i = 0; i < read; i++)
                {
                    writer.WriteLine("{0}", recData[i]);
                }
                writer.Flush();
                writer.Close();
            }
        }
    }
}
