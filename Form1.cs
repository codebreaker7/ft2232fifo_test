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

namespace FtdiFifo
{
    public partial class Form1 : Form
    {
        private FTDI ftHandle = new FTDI();
        private FTDI.FT_DEVICE_INFO_NODE[] deviceInfos = new FTDI.FT_DEVICE_INFO_NODE[2];
        private string serialNumber = new string(' ', 32);
        private byte[] data = new byte[128];
        private byte[] recData = new byte[128];

        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 128; i++)
            {
                data[i] = (byte)(i % 256);
            }
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

        private void SendDataOverFifo()
        {
            FTDI.FT_STATUS status;
            uint txQueue = 0;
            uint rxQueue = 0;
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
        }

        private void MeasureSpeed()
        {
            byte[] masdata = new byte[4096]; // start array 
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
    }
}
