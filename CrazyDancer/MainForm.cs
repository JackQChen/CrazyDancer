using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Imaging;
using AForge.Imaging.Filters;
using CrazyDancer.Properties;

namespace CrazyDancer
{
    public partial class MainForm : Form
    {
        [DllImport("User32.dll")]
        private extern static IntPtr GetDC(IntPtr hWnd);

        Hotkey hotKey;
        Rectangle rectKeys, rectBall;
        int currX, minX, maxX;
        Dictionary<int, Keys> dicKeys = new Dictionary<int, Keys>();
        Dictionary<Keys, Bitmap> dicTemplate = new Dictionary<Keys, Bitmap>();
        bool inputAvailable = false;

        Queue<byte> byteQueue = new Queue<byte>();
        AutoResetEvent resetEvent = new AutoResetEvent(false);

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_PnPEntity where Name like '%(COM%'"))
            {
                var hardInfos = searcher.Get();
                foreach (var hardInfo in hardInfos)
                {
                    if (hardInfo.Properties["Name"].Value != null)
                    {
                        string deviceName = hardInfo.Properties["Name"].Value.ToString();
                        if (deviceName.StartsWith("Arduino"))
                        {
                            serialPort1.PortName = Regex.Match(deviceName, "\\(.+?\\)").Value.Replace("(", "").Replace(")", "");
                            serialPort1.Open();
                        }
                    }
                }
            }
            hotKey = new Hotkey(this.Handle);
            dicKeys[hotKey.RegisterHotkey(Keys.End, Hotkey.KeyFlags.MOD_NONE)] = Keys.End;
            hotKey.OnHotkey += new HotkeyEventHandler(hotKey_OnHotkey);
            rectKeys = new Rectangle(327, 560, this.picKey1.Width, this.picKey1.Height);
            rectBall = new Rectangle(512, 536, this.picBall1.Width, this.picBall1.Height);
            var unit = GraphicsUnit.Pixel;
            dicTemplate[Keys.Left] = Resources.left.Clone(Resources.left.GetBounds(ref unit), PixelFormat.Format8bppIndexed);
            dicTemplate[Keys.Right] = Resources.right.Clone(Resources.right.GetBounds(ref unit), PixelFormat.Format8bppIndexed);
            dicTemplate[Keys.Up] = Resources.up.Clone(Resources.up.GetBounds(ref unit), PixelFormat.Format8bppIndexed);
            dicTemplate[Keys.Down] = Resources.down.Clone(Resources.down.GetBounds(ref unit), PixelFormat.Format8bppIndexed);
            //检测位置
            Task.Factory.StartNew(() =>
            {
                //始终检测位置
                while (true)
                {
                    var newX = GetBallX();
                    if (newX < 0)
                    {
                        minX = 999;
                        maxX = 0;
                        Thread.Sleep(1);
                        continue;
                    }
                    var percent = 0;
                    //消除异常数据
                    if (Math.Abs(newX - currX) > 5)
                    {
                        currX = newX;
                        continue;
                    }
                    if (newX > currX)
                    {
                        if (newX < minX)
                            minX = newX;
                        if (newX > maxX)
                            maxX = newX;
                        if (minX == maxX)
                            maxX = minX + 1;
                        percent = (currX - minX) * 100 / (maxX - minX);
                    }
                    currX = newX;
                    if (this.inputAvailable && this.btnDance.Enabled && percent > 80)
                    {
                        this.Invoke(new Action(() =>
                        {
                            this.btnDance.PerformClick();
                            this.btnDance.Enabled = false;
                        }));
                    }
                    if (this.btnDance.Enabled == false && percent > 60 && percent < 70)
                    {
                        this.Invoke(new Action(() =>
                        {
                            WriteSerialPortData(5);
                            this.btnDance.Enabled = true;
                        }));
                    }
                    Thread.Sleep(1);
                }
            });
            //发送串口数据
            Task.Factory.StartNew(() =>
            {
                var byteArray = new byte[1] { 0 };
                while (true)
                {
                    resetEvent.WaitOne();
                    while (byteQueue.Count > 0)
                    {
                        byteArray[0] = byteQueue.Dequeue();
                        this.serialPort1.Write(byteArray, 0, 1);
                        Thread.Sleep(35);
                        byteArray[0] = Convert.ToByte(byteArray[0] + 100);
                        this.serialPort1.Write(byteArray, 0, 1);
                        Thread.Sleep(35);
                    }
                }
            });
        }
        void WriteSerialPortData(byte data)
        {
            byteQueue.Enqueue(data);
            resetEvent.Set();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            serialPort1.Close();
            hotKey.UnregisterHotkeys();
        }

        void hotKey_OnHotkey(int hotKeyID)
        {
            if (dicKeys[hotKeyID] == Keys.End)
            {
                Image2Keys();
            }
        }

        private int GetBallX()
        {
            var bmp = new Bitmap(rectBall.Width, rectBall.Height);
            var g = Graphics.FromImage(bmp as Bitmap);
            g.CopyFromScreen(rectBall.Location, Point.Empty, rectBall.Size);
            this.picBall1.Image = bmp.Clone() as Bitmap;
            var bmpShow = bmp.Clone() as Bitmap;
            g = Graphics.FromImage(bmpShow);
            //HSLFiltering 使用IPLab进行选取
            new HSLFiltering(new AForge.IntRange(0, 18), new AForge.Range(0.2f, 1), new AForge.Range(0, 1)).ApplyInPlace(bmp);
            //灰度化
            bmp = Grayscale.CommonAlgorithms.BT709.Apply(bmp);
            //二值化
            new Threshold(60).ApplyInPlace(bmp);
            //BlobCounter
            var blobCounter = new BlobCounter()
            {
                FilterBlobs = true,
                MinWidth = 10,
                MinHeight = 10,
                MaxWidth = 20,
                MaxHeight = 20
            };
            blobCounter.ProcessImage(bmp);
            var blobs = blobCounter.GetObjectsInformation();
            foreach (var blob in blobs)
            {
                var p = blob.CenterOfGravity;
                var c = bmpShow.GetPixel(Convert.ToInt32(p.X - 4), Convert.ToInt32(p.Y));
                //是否可输入
                inputAvailable = c.R - c.G > 160;
                this.labIndicator.BackColor = inputAvailable ? Color.Green : Color.Red;
                g.FillRectangle(Brushes.White, p.X - 12, p.Y - 1, 24, 2);
                g.FillRectangle(Brushes.White, p.X - 1, p.Y - 12, 2, 24);
            }
            this.picBall2.Image = bmpShow;
            if (blobs.Length == 1)
                return Convert.ToInt32(blobs[0].CenterOfGravity.X);
            else
                return -1;
        }

        private List<Keys> Image2Keys()
        {
            var dicKeys = new Dictionary<Rectangle, Keys>();
            var bmp = new Bitmap(rectKeys.Width, rectKeys.Height);
            var g = Graphics.FromImage(bmp as Bitmap);
            g.CopyFromScreen(rectKeys.Location, Point.Empty, rectKeys.Size);
            this.picKey1.Image = bmp.Clone() as Bitmap;
            var bmpShow = bmp.Clone() as Bitmap;
            g = Graphics.FromImage(bmpShow);
            //灰度化
            bmp = Grayscale.CommonAlgorithms.BT709.Apply(bmp);
            //二值化
            new Threshold(200).ApplyInPlace(bmp);
            //保存模板以用于识别
            //bmp.Save("template.bmp");
            var matching = new ExhaustiveTemplateMatching(0.9f);
            foreach (var template in dicTemplate)
            {
                var rects = matching.ProcessImage(bmp, template.Value).Select(s => s.Rectangle).ToList();
                //移除相交矩形
                for (int i = 0; i < rects.Count; i++)
                    rects.RemoveAll(p => p != rects[i] && p.IntersectsWith(rects[i]));
                foreach (var rect in rects)
                {
                    g.FillRectangle(Brushes.Red, rect);
                    g.DrawString(template.Key.ToString(), this.Font, Brushes.Black, rect);
                    dicKeys[rect] = template.Key;
                }
            }
            this.picKey2.Image = bmpShow;
            //矩形按X轴排序
            return dicKeys.OrderBy(k => k.Key.X).Select(s => s.Value).ToList();
        }
        List<Keys> lastKeys = new List<Keys>();
        private void btnDance_Click(object sender, EventArgs e)
        {
            var keys = Image2Keys();
            var duplicateKeys = true;
            if (keys.Count != lastKeys.Count)
                duplicateKeys = false;
            else
            {
                for (int i = 0; i < keys.Count; i++)
                {
                    if (keys[i] != lastKeys[i])
                    {
                        duplicateKeys = false;
                        break;
                    }
                }
            }
            if (duplicateKeys)
                return;
            lastKeys = keys.ToList();
            this.txtLog.Clear();
            for (int i = 0; i < keys.Count; i++)
            {
                this.txtLog.AppendText(keys[i].ToString() + "\r\n");
                switch (keys[i])
                {
                    case Keys.Up: WriteSerialPortData(1); break;
                    case Keys.Down: WriteSerialPortData(2); break;
                    case Keys.Left: WriteSerialPortData(3); break;
                    case Keys.Right: WriteSerialPortData(4); break;
                }
            }
        }
    }
}

