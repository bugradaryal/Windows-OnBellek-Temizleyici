using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Cleaner
{
    public partial class Form1 : Form
    {
        DialogResult dialog;
        bool button = false;
        string pc_username = Environment.UserName;
        string system_disc = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System));
        public Form1()
        {
            InitializeComponent();
        }
        public void messagebox(string metin, string hata, string baslik)
        {
            var box = new Form() { Size = new Size(0, 0) };
            //Task.Delay(TimeSpan.FromSeconds(3)).ContinueWith((t) => box.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            if (hata == "")
                MessageBox.Show(box, metin, baslik, MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(box, metin + "\n Hata:" + hata, baslik,MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        enum RecycleFlags : uint
        {
            SHRB_NOCONFIRMATION = 0x00000001, // Don't ask confirmation
            SHRB_NOPROGRESSUI = 0x00000002, // Don't show any windows dialog
            SHRB_NOSOUND = 0x00000004 // Don't make sound, ninja mode enabled :v
        }
        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        static extern uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlags dwFlags);
        public void file_islem(DirectoryInfo konum)
        {
            try
            {
                foreach (FileInfo file in konum.GetFiles())
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception) { }
                }
                foreach (DirectoryInfo dir in konum.GetDirectories())
                {
                    try
                    {
                        dir.Delete(true);
                    }
                    catch (Exception) { }
                } 
            }
            catch (DirectoryNotFoundException hata) 
            {
                messagebox("Hata İle Karşılaşıldı!!", hata.ToString(), "Cleaner");
            }
        }
        private void button1_Click(object sender, EventArgs e) // hepsi
        {
            dialog = MessageBox.Show("Silme İşlemini Gerçekleştireceğinize Eminmisiniz?", "Cleaner", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (dialog == DialogResult.Yes)
            {
                button = true;
                label1.Text = "All...";
                label1.Visible = true;
                backgroundWorker1.RunWorkerAsync();
            }
        }
        private void button2_Click(object sender, EventArgs e) // temp
        {
            dialog = MessageBox.Show("Silme İşlemini Gerçekleştireceğinize Eminmisiniz?","Cleaner", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if(dialog == DialogResult.Yes)
            {
                label1.Text = "Temp...";
                label1.Visible = true;
                backgroundWorker2.RunWorkerAsync();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            dialog = MessageBox.Show("Silme İşlemini Gerçekleştireceğinize Eminmisiniz?", "Cleaner", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (dialog == DialogResult.Yes)
            {
                label1.Text = "%Temp%...";
                label1.Visible = true;
                backgroundWorker3.RunWorkerAsync();
            }
        } //%temp%
        private void button4_Click(object sender, EventArgs e)
        {
            string pc_username = Environment.UserName;
            dialog = MessageBox.Show("Silme İşlemini Gerçekleştireceğinize Eminmisiniz?", "Cleaner", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (dialog == DialogResult.Yes)
            {
                label1.Text = "Cache...";
                label1.Visible = true;
                backgroundWorker4.RunWorkerAsync();
            }
        }  //internet cache
        private void button5_Click(object sender, EventArgs e)
        {
            dialog = MessageBox.Show("Silme İşlemini Gerçekleştireceğinize Eminmisiniz?", "Cleaner", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (dialog == DialogResult.Yes)
            {
                label1.Text = "Trash...";
                label1.Visible = true;
                backgroundWorker5.RunWorkerAsync();
            }
        }  //çöp kutusu
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            label1.Text = "Starting...";
        }
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            file_islem(new DirectoryInfo(system_disc + @"Windows\Temp"));
        }
        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            file_islem(new DirectoryInfo(system_disc + @"Users\" + pc_username + @"\AppData\Local\Temp"));
        }
        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            file_islem(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache)));
        }
        private void backgroundWorker5_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                uint IsSuccess = SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlags.SHRB_NOCONFIRMATION);
            }
            catch (Exception hata) 
            {
                messagebox("Hata İle Karşılaşıldı!!", hata.ToString(), "Cleaner");
            }
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            backgroundWorker2.RunWorkerAsync();
        }
        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label1.Text = "Temp Cleared...";
            if (button == false)
            {
                messagebox("İşlem Tamamlandı...", "", "Cleaner");
                label1.Visible = false;
            }
            else
                backgroundWorker3.RunWorkerAsync();
        }
        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label1.Text = "%Temp% Cleared...";
            if (button == false)
            {
                messagebox("İşlem Tamamlandı...", "", "Cleaner");
                label1.Visible = false;
            }
            else
                backgroundWorker4.RunWorkerAsync();
        }
        private void backgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label1.Text = "Cache Cleared...";
            if (button == false)
            {
                messagebox("İşlem Tamamlandı...", "", "Cleaner");
                label1.Visible = false;
            }
            else
            {
                label1.Text = "All Cleared Except Trash...";
                backgroundWorker5.RunWorkerAsync();
            }
        }
        private void backgroundWorker5_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label1.Text = "Trash Cleared...";
            if (button == true)
                label1.Text = "All Cleared...";
            messagebox("İşlem Tamamlandı...", "", "Cleaner");
            label1.Visible = false;
            button = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Visible = false;
        }
    }
}

