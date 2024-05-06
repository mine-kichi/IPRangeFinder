using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace IPRangeFinder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string startIp = $"{textBox1.Text}.{textBox2.Text}.{textBox3.Text}.{textBox4.Text}";
            string endIp = $"{textBox8.Text}.{textBox7.Text}.{textBox6.Text}.{textBox5.Text}";
            await ScanIpRangeAsync(startIp, endIp);

/*            Ping ping = new Ping();
            PingReply reply;

            listBox1.Items.Clear();
            for (string ip = startIp; ip != endIp; ip = NextIp(ip))
            {
                reply = ping.Send(ip);
                if (reply.Status == IPStatus.Success)
                {
                    listBox1.Items.Add(ip + " :Reachable" );
                }
                else
                {
                    listBox1.Items.Add(ip + " :UnReachable");
                }
            }*/
        }
        public string NextIp(string currentIp)
        {
            IPAddress ip = IPAddress.Parse(currentIp);
            byte[] bytes = ip.GetAddressBytes();

            // バイト配列を逆順にして、32ビット整数として扱いやすくする
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            // 32ビット整数に変換
            uint ipAsUint = BitConverter.ToUInt32(bytes, 0);

            // IPアドレスを1増やす
            ipAsUint++;

            // uintからbyte配列に戻す
            byte[] incrementedBytes = BitConverter.GetBytes(ipAsUint);

            // エンディアンが小さい場合は再度逆転する
            if (BitConverter.IsLittleEndian)
                Array.Reverse(incrementedBytes);

            // 新しいIPアドレスオブジェクトを生成して返す
            return new IPAddress(incrementedBytes).ToString();
        }
        public async Task ScanIpRangeAsync(string startIp, string endIp)
        {
            var startBytes = IPAddress.Parse(startIp).GetAddressBytes();
            var endBytes = IPAddress.Parse(endIp).GetAddressBytes();
            uint start = BitConverter.ToUInt32(startBytes.Reverse().ToArray(), 0);
            uint end = BitConverter.ToUInt32(endBytes.Reverse().ToArray(), 0);

            listBox1.Items.Clear();
            List<Task> tasks = new List<Task>();

            for (uint ip = start; ip <= end; ip++)
            {
                tasks.Add(PingIpAsync(new IPAddress(BitConverter.GetBytes(ip).Reverse().ToArray())));
            }

            await Task.WhenAll(tasks);
        }

        private async Task PingIpAsync(IPAddress ip)
        {
            using (var ping = new Ping())
            {
                var reply = await ping.SendPingAsync(ip);
                if (reply.Status == IPStatus.Success)
                {
                    listBox1.Items.Add(ip + " :Reachable");
                }
                else
                {
//                    listBox1.Items.Add(ip + " :UnReachable");
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }
    }
}
