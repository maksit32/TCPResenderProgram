using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Client
{
    public partial class Form1 : Form
    {
        //ip
        private IPAddress localAddr = IPAddress.Parse("192.168.0.13");
        //port
        private int port = 4572;
        //server
        private TcpClient server = new TcpClient();

        public Form1()
        {
            InitializeComponent();
            //��������� ������� � �������
            server.ConnectAsync(localAddr, port);
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            //���������� �� �����
            Send_ReceivePrBar.Visible = false;
            //�������� ������� ����� �� ������ ��������.
            ListenFilesAsync();
        }

        private async Task ListenFilesAsync()
        {
            while (true)
            {
                //UI ���������
                //��������
                byte[] buffer = await BufferedReadBlock(server.GetStream());
                string name = Encoding.UTF8.GetString(buffer);


                //������ ���� ������!
                byte[] sizeBuffer = await BufferedReadBlock(server.GetStream());
                string size = Encoding.UTF8.GetString(sizeBuffer);

                //��������� �������� � ������
                this.listOnServer.Items.Add(name);

                var res = MessageBox.Show($"��� ��� ��������� ����.{Environment.NewLine}������ ������� ���?", "��������!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                //������� �������
                if (res == DialogResult.Yes)
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    //���� ���������??
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        Send_ReceivePrBar.Visible = true;
                        //���������� ���� �����
                        if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                        {
                            //������� ���� (���� + ���������� ���)
                            using (Stream write = File.Create(fbd.SelectedPath + "\\" + name))
                            {
                                //����� ���������� ����
                                int countReceived = 0;
                                int sizeBuf = 1024 * 1024;
                                byte[] buffer2 = new byte[sizeBuf];
                                Send_ReceivePrBar.Maximum = Convert.ToInt32(size);
                                while (countReceived < Convert.ToInt32(size))
                                {
                                    //��������� �������� � ������
                                    buffer2 = await BufferedReadBlock(server.GetStream());
                                    //����� ���������� �� ����� ������
                                    await write.WriteAsync(buffer2, 0, buffer2.Length);
                                    countReceived += buffer2.Length;

                                    //����� ������ ������, ��������� � UI
                                    Send_ReceivePrBar.Invoke((Action)(() => { Send_ReceivePrBar.Value = countReceived; }));
                                }
                                //����������!
                                Send_ReceivePrBar.Visible = false;
                                MessageBox.Show("�����!");
                            }
                        }
                    }
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.server.Dispose();
        }
        //������� ����������
        private async Task<byte[]> BufferedReadBlock(Stream stream)
        {
            byte[] prefixBuf = await BufferedRead(stream, sizeof(int));
            int length = BitConverter.ToInt32(prefixBuf);
            byte[] message = await BufferedRead(stream, length);
            return message;
        }
        private async Task<byte[]> BufferedRead(Stream stream, int length)
        {
            byte[] buffer = new byte[length];
            for (int progress = 0; progress < length;)
            {
                int chunk = await stream.ReadAsync(buffer, progress, length - progress);
                progress += chunk;
            }
            return buffer;
        }

        private async void ChoiceFile_Click(object sender, EventArgs e)
        {
            //�������� ���� ��� ��������
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //�������� pb ���������
                Send_ReceivePrBar.Visible = true;
                byte[] NameBuffer = Encoding.UTF8.GetBytes(Path.GetFileName(ofd.FileName));
                //���
                await server.GetStream().WriteAsync(BitConverter.GetBytes(NameBuffer.Length));
                await server.GetStream().WriteAsync(NameBuffer);



                long FileSize = new FileInfo(ofd.FileName).Length;
                byte[] SizeBuffer = Encoding.UTF8.GetBytes(Convert.ToString(FileSize));
                //������
                await server.GetStream().WriteAsync(BitConverter.GetBytes(SizeBuffer.Length));
                await server.GetStream().WriteAsync(SizeBuffer);
                long sizeSend = 0;
                using (Stream ins = File.OpenRead(ofd.FileName))
                {


                    int block = 1024 * 1024;
                    byte[] buffer = new byte[block];
                    int count;

                    Send_ReceivePrBar.Maximum = Convert.ToInt32(FileSize);
                    while (sizeSend < FileSize)
                    {
                        count = await ins.ReadAsync(buffer, 0, block);
                        await server.GetStream().WriteAsync(BitConverter.GetBytes(count));
                        await server.GetStream().WriteAsync(buffer, 0, count);
                        sizeSend += count;
                        //��������� � UI
                        Send_ReceivePrBar.Invoke(() => { Send_ReceivePrBar.Value = Convert.ToInt32(sizeSend); });

                    }
                    //������� ���������
                    Send_ReceivePrBar.Visible = false;
                }
            }
        }
    }
}