using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;
using System.Diagnostics;
using System.IO;

namespace NEMCU
{
    using Microsoft.Win32;
    using System.Threading;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement;

    public partial class Form1 : MaterialForm
    {
        int copy = 0;
        bool isrewrite = true;

        public Form1()
        {
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            bool isrewrite = true;
            string installlocation = Getinstalllocation();
            string downloadpath = Getdownloadpath();
            string app = installlocation + "\\" + "WPFLauncher.exe";
            string nemcupath = downloadpath + "\\" + "nemcu";
            string nemcuresourcepackspath = nemcupath + "\\" + "resourcepacks\\";
            string mcresourcepackspath = downloadpath + "\\Game\\.minecraft\\resourcepacks\\";
            string mcoptions = downloadpath + "\\Game\\.minecraft\\options.txt";
            string nemcumcoptions = nemcupath + "\\options.txt";
            string mcresourcepackorganizer = downloadpath + "\\Game\\.minecraft\\config\\resourcepackorganizer.cfg";
            string nemcuresourcepackorganizer = nemcupath + "\\resourcepackorganizer.cfg";
            string mcshaderpackspath = downloadpath + "\\Game\\.minecraft\\shaderpacks\\";
            string nemcushaderpackspath = nemcupath + "\\" + "shaderpacks\\";
            materialTextBox21.Text = installlocation;
            materialTextBox22.Text = downloadpath;
            materialTextBox23.Text = nemcuresourcepackspath;
            materialTextBox24.Text = mcresourcepackspath;
            materialTextBox25.Text = mcoptions;
            materialTextBox26.Text = nemcumcoptions;
            materialTextBox27.Text = mcresourcepackorganizer;
            materialTextBox28.Text = nemcuresourcepackorganizer;
            materialTextBox29.Text = nemcupath;
            materialTextBox210.Text = mcshaderpackspath;
            materialTextBox211.Text = nemcushaderpackspath;
            Process.Start(app);
            if (Directory.Exists(nemcuresourcepackspath))
            {
                Directory.Delete(nemcuresourcepackspath,true);
            }
            if (Directory.Exists(nemcushaderpackspath))
            {
                Directory.Delete(nemcushaderpackspath,true);
            }
            System.IO.File.Copy(mcoptions, nemcumcoptions, isrewrite);
            CopyFolder(mcresourcepackspath, nemcuresourcepackspath);
            CopyFolder(mcshaderpackspath, nemcushaderpackspath);
            System.IO.File.Copy(mcresourcepackorganizer, nemcuresourcepackorganizer, isrewrite);
            if (!File.Exists(materialTextBox29.Text + "\\nemcusettings.txt"))
            {
                StreamWriter sw = new StreamWriter(materialTextBox29.Text + "\\nemcusettings.txt");
                sw.WriteLine(textBox1.Text);
                sw.Close();
            }
            textBox2.Text = string.Empty;
            StreamReader sr = new StreamReader(materialTextBox29.Text + "\\nemcusettings.txt");
            textBox2.Text = sr.ReadToEnd();
            sr.Close();
            if(textBox2.Lines[0] == "nightvision=0")
            {
                materialCheckbox1.Checked = false;
            }
            else
            {
                materialCheckbox1.Checked = true;
            }
            if (textBox2.Lines[1] == "autohide=1")
            {
                materialCheckbox2.Checked = true;
            }
            else
            {
                materialCheckbox2.Checked = false;
            }
            if (textBox2.Lines[2] == "foreverhide=0")
            {
                materialCheckbox3.Checked = false;
            }
            else
            {
                materialCheckbox3.Checked = true;
                this.Hide();
            }
            label1.Text = "提示:1.本程序需启动过一次游戏才能正常使用\r\n        2.本程序会保留您使用的材质包及光影，无需再次复制和启用\r\n        3.打开永久隐藏后会在下次启动生效，如需取消请删除\r\n        " + materialTextBox29.Text + "\\nemcusettings.txt";
            timer1.Enabled = true;
        }
        public static string Getinstalllocation()
        {
            //Set path
            string installEntry = @"SOFTWARE\Netease\MCLauncher\";
            string version = string.Empty;
            //Get path
            RegistryKey key = Registry.CurrentUser.OpenSubKey(installEntry);
            if (key != null)
            {
                object oVersion = key.GetValue("InstallLocation");
                if (null != oVersion)
                {
                    version = oVersion.ToString();
                }
            }
            else
            {
                version = "未从注册表中找到网易盒子路径";
            }
            return version;
        }
        public static string Getdownloadpath()
        {
            //Set path
            string installEntry = @"SOFTWARE\Netease\MCLauncher\";
            string version = string.Empty;
            //Get path
            RegistryKey key = Registry.CurrentUser.OpenSubKey(installEntry);
            if (key != null)
            {
                object oVersion = key.GetValue("DownloadPath");
                if (null != oVersion)
                {
                    version = oVersion.ToString();
                }
            }
            else
            {
                version = "未从注册表中找到游戏目录";
            }
            return version;
        }

        public static void CopyFolder(string from, string to)
        {
            if (!Directory.Exists(to))
                Directory.CreateDirectory(to);

            // 子文件夹
            foreach (string sub in Directory.GetDirectories(from))
                CopyFolder(sub + "\\", to + Path.GetFileName(sub) + "\\");

            // 文件
            foreach (string file in Directory.GetFiles(from))

                File.Copy(file, to + Path.GetFileName(file), true);
        }
        public static void KillProcess(string processName)
        {

            foreach (Process p in Process.GetProcesses())
            {
                if (p.ProcessName.Contains(processName))
                {
                    try
                    {
                        p.Kill();
                        p.WaitForExit();
                    }
                    catch (Win32Exception e)
                    {

                    }
                    catch (InvalidOperationException e)
                    {

                    }
                }

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer1.Enabled == true)
            {

            }
            this.Text = "NEMCU" + " | " + materialLabel3.Text + materialLabel4.Text + " | " + materialLabel5.Text + materialLabel6.Text;
            if (System.Diagnostics.Process.GetProcessesByName("WPFLauncher").ToList().Count > 0)
            {
                materialLabel4.Text = "已启动";
                if (System.Diagnostics.Process.GetProcessesByName("WPFLauncher").ToList().Count > 0)
                {
                    materialLabel4.Text = "已启动";
                    if (System.Diagnostics.Process.GetProcessesByName("javaw").ToList().Count > 0)
                    {
                        materialLabel6.Text = "已启动";
                        if (copy == 0)
                        {
                            CopyFolder(materialTextBox23.Text, materialTextBox24.Text);
                            CopyFolder(materialTextBox211.Text, materialTextBox210.Text);
                            System.IO.File.Copy(materialTextBox26.Text, materialTextBox25.Text, isrewrite);
                            System.IO.File.Copy(materialTextBox28.Text, materialTextBox27.Text, isrewrite);
                            copy = 1;
                        }
                        if(materialCheckbox2.Checked)
                        {
                            this.Hide();
                        }
                    }
                    else
                    {
                        materialLabel6.Text = "未启动";
                        if (textBox2.Lines[2] == "foreverhide=1")
                        {
                            this.Hide();
                        }
                        else
                        {
                            if (materialCheckbox2.Checked)
                            {
                                this.Show();
                            }
                        }

                        if (copy == 1)
                        {
                            copy = 0;
                            CopyFolder(materialTextBox24.Text, materialTextBox23.Text);
                            CopyFolder(materialTextBox210.Text, materialTextBox211.Text);
                            System.IO.File.Copy(materialTextBox25.Text, materialTextBox26.Text, isrewrite);
                            System.IO.File.Copy(materialTextBox27.Text, materialTextBox28.Text, isrewrite);
                        }
                    }
                }
                else
                {
                    materialLabel4.Text = "未启动";

                }


            }
        }

        private void materialCheckbox1_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckbox1.Checked)
            {
                string con = "";
                FileStream fs = new FileStream(materialTextBox26.Text, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                con = sr.ReadToEnd();
                con = con.Replace("gamma:1.0", "gamma:200.0");
                sr.Close();
                fs.Close();
                FileStream fs2 = new FileStream(materialTextBox26.Text, FileMode.Open, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs2);
                sw.WriteLine(con);
                sw.Close();
                fs2.Close();
                string ls1 = "";
                FileStream fsnv = new FileStream(materialTextBox29.Text + "\\nemcusettings.txt", FileMode.Open, FileAccess.Read);
                StreamReader srnv = new StreamReader(fsnv);
                ls1 = srnv.ReadToEnd();
                ls1 = ls1.Replace("nightvision=0", "nightvision=1");
                srnv.Close();
                fsnv.Close();
                FileStream fsnv2 = new FileStream(materialTextBox29.Text + "\\nemcusettings.txt", FileMode.Open, FileAccess.Write);
                StreamWriter swnv = new StreamWriter(fsnv2);
                swnv.WriteLine(ls1);
                swnv.Close();
                fsnv2.Close();
            }
            if (!materialCheckbox1.Checked)
            {
                string con = "";
                FileStream fs = new FileStream(materialTextBox26.Text, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                con = sr.ReadToEnd();
                con = con.Replace("gamma:200.0", "gamma:1.0");
                sr.Close();
                fs.Close();
                FileStream fs2 = new FileStream(materialTextBox26.Text, FileMode.Open, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs2);
                sw.WriteLine(con);
                sw.Close();
                fs2.Close();
                string ls1 = "";
                FileStream fsnv = new FileStream(materialTextBox29.Text + "\\nemcusettings.txt", FileMode.Open, FileAccess.Read);
                StreamReader srnv = new StreamReader(fsnv);
                ls1 = srnv.ReadToEnd();
                ls1 = ls1.Replace("nightvision=1", "nightvision=0");
                srnv.Close();
                fsnv.Close();
                FileStream fsnv2 = new FileStream(materialTextBox29.Text + "\\nemcusettings.txt", FileMode.Open, FileAccess.Write);
                StreamWriter swnv = new StreamWriter(fsnv2);
                swnv.WriteLine(ls1);
                swnv.Close();
                fsnv2.Close();
            }
        }

        private void materialCheckbox2_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckbox2.Checked)
            {
                string ls1 = "";
                FileStream fsnv = new FileStream(materialTextBox29.Text + "\\nemcusettings.txt", FileMode.Open, FileAccess.Read);
                StreamReader srnv = new StreamReader(fsnv);
                ls1 = srnv.ReadToEnd();
                ls1 = ls1.Replace("autohide=0", "autohide=1");
                srnv.Close();
                fsnv.Close();
                FileStream fsnv2 = new FileStream(materialTextBox29.Text + "\\nemcusettings.txt", FileMode.Open, FileAccess.Write);
                StreamWriter swnv = new StreamWriter(fsnv2);
                swnv.WriteLine(ls1);
                swnv.Close();
                fsnv2.Close();
            }
            if (!materialCheckbox2.Checked)
            {
                string ls1 = "";
                FileStream fsnv = new FileStream(materialTextBox29.Text + "\\nemcusettings.txt", FileMode.Open, FileAccess.Read);
                StreamReader srnv = new StreamReader(fsnv);
                ls1 = srnv.ReadToEnd();
                ls1 = ls1.Replace("autohide=1", "autohide=0");
                srnv.Close();
                fsnv.Close();
                FileStream fsnv2 = new FileStream(materialTextBox29.Text + "\\nemcusettings.txt", FileMode.Open, FileAccess.Write);
                StreamWriter swnv = new StreamWriter(fsnv2);
                swnv.WriteLine(ls1);
                swnv.Close();
                fsnv2.Close();
            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            KillProcess("WPFLauncher");
            Process.Start(materialTextBox21.Text + "//" + "WPFLauncher.exe");
        }

        private void materialCheckbox3_CheckedChanged(object sender, EventArgs e)
        {
                if (materialCheckbox3.Checked)
                {
                string ls1 = "";
                FileStream fsnv = new FileStream(materialTextBox29.Text + "\\nemcusettings.txt", FileMode.Open, FileAccess.Read);
                StreamReader srnv = new StreamReader(fsnv);
                ls1 = srnv.ReadToEnd();
                ls1 = ls1.Replace("foreverhide=0", "foreverhide=1");
                srnv.Close();
                fsnv.Close();
                FileStream fsnv2 = new FileStream(materialTextBox29.Text + "\\nemcusettings.txt", FileMode.Open, FileAccess.Write);
                StreamWriter swnv = new StreamWriter(fsnv2);
                swnv.WriteLine(ls1);
                swnv.Close();
                fsnv2.Close();
                }
                if (!materialCheckbox3.Checked)
                {
                string ls1 = "";
                FileStream fsnv = new FileStream(materialTextBox29.Text + "\\nemcusettings.txt", FileMode.Open, FileAccess.Read);
                StreamReader srnv = new StreamReader(fsnv);
                ls1 = srnv.ReadToEnd();
                ls1 = ls1.Replace("foreverhide=1", "foreverhide=0");
                srnv.Close();
                fsnv.Close();
                FileStream fsnv2 = new FileStream(materialTextBox29.Text + "\\nemcusettings.txt", FileMode.Open, FileAccess.Write);
                StreamWriter swnv = new StreamWriter(fsnv2);
                swnv.WriteLine(ls1);
                swnv.Close();
                fsnv2.Close();
            }
            
        }
    }
}