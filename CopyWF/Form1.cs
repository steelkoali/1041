using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace CopyWF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitialCltr();
        }

        private void InitialCltr()
        {
            string filepath = @"C:\Users\Administrator\Documents\CopyAuto\Settings.ini";
            bool fgExist = File.Exists(filepath);

             if ( fgExist )
             {
                FileStream fsFile = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamReader srFile = new StreamReader(fsFile);
                string strText = srFile.ReadToEnd();

                Regex rgSourcePath1 = new Regex("(?<=(" + "SourcePath1=" + "))[.\\s\\S]*?(?=(\r|\n))", RegexOptions.Multiline | RegexOptions.Singleline);
                SourcePath1.Text = rgSourcePath1.Match(strText).Value;

                Console.WriteLine(SourcePath1.Text);

                Regex rgSourcePath2 = new Regex("(?<=(" + "SourcePath2=" + "))[.\\s\\S]*?(?=(\r|\n))", RegexOptions.Multiline | RegexOptions.Singleline);
                SourcePath2.Text = rgSourcePath2.Match(strText).Value;

                Console.WriteLine(SourcePath2.Text);

                Regex rgSourcePath3 = new Regex("(?<=(" + "SourcePath3=" + "))[.\\s\\S]*?(?=(\r|\n))", RegexOptions.Multiline | RegexOptions.Singleline);
                SourcePath3.Text = rgSourcePath3.Match(strText).Value;

                Console.WriteLine(SourcePath3.Text);

                Regex rgTargetPath1 = new Regex("(?<=(" + "TargetPath1=" + "))[.\\s\\S]*?(?=(\r|\n))", RegexOptions.Multiline | RegexOptions.Singleline);
                TargetPath1.Text = rgTargetPath1.Match(strText).Value;

                Console.WriteLine(TargetPath1.Text);

                Regex rgTargetPath2 = new Regex("(?<=(" + "TargetPath2=" + "))[.\\s\\S]*?(?=(\r|\n))", RegexOptions.Multiline | RegexOptions.Singleline);
                TargetPath2.Text = rgTargetPath2.Match(strText).Value;

                Console.WriteLine(TargetPath2.Text);

                Regex rgTargetPath3 = new Regex("(?<=(" + "TargetPath3=" + "))[.\\s\\S]*?(?=(\r|\n))", RegexOptions.Multiline | RegexOptions.Singleline);
                TargetPath3.Text = rgTargetPath3.Match(strText).Value;

                Console.WriteLine(TargetPath3.Text);

                Regex rgcheckDLL = new Regex("(?<=(" + "CheckDll=True" + "))[.\\s\\S]*?(?=(\r|\n))", RegexOptions.Multiline | RegexOptions.Singleline);
                checkDLL.Checked = rgcheckDLL.Match(strText).Success;

                Regex rgcheckEXE = new Regex("(?<=(" + "CheckExe=True" + "))[.\\s\\S]*?(?=(\r|\n))", RegexOptions.Multiline | RegexOptions.Singleline);
                checkEXE.Checked = rgcheckEXE.Match(strText).Success;

                Regex rgcheckALL = new Regex("(?<=(" + "CheckAll=True" + "))[.\\s\\S]*?(?=(\r|\n))", RegexOptions.Multiline | RegexOptions.Singleline);
                checkAll.Checked = rgcheckALL.Match(strText).Success;

                srFile.Close();
                fsFile.Close();
             }
             
        }
        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void SetFolderBowserDialog( FolderBrowserDialog fbdFoldeBrowerDialog, TextBox ttTextBox )
        {
            fbdFoldeBrowerDialog.Description = "请选择文件夹";
            DialogResult drFBD1Ret = fbdFoldeBrowerDialog.ShowDialog();

            if ((drFBD1Ret == DialogResult.OK))
            {
                ttTextBox.Text = @fbdFoldeBrowerDialog.SelectedPath;
                Console.WriteLine(ttTextBox.Text);
            }
            else if (drFBD1Ret == DialogResult.Yes)
            {
                ttTextBox.Text = @fbdFoldeBrowerDialog.SelectedPath;
            }
            else if ((drFBD1Ret == DialogResult.Cancel))
            {
            }
            else
            {
            }

            Console.WriteLine(ttTextBox.Text);
        }

        private void CopyDone( string strSourcePath, string strTargetPath )
        {
            try
            {
                // Determine whether the directory exists.
                bool fgSourcePathExist = Directory.Exists(@strSourcePath);

                if (false == fgSourcePathExist)
                {
                    label1.Text = "\""+@strSourcePath + "\" not exist!";
                    return;
                }

                bool fgTargetPathExist = Directory.Exists(@strTargetPath);

                if (true == fgTargetPathExist)
                {
                    // Delete the target to ensure it is not there.
                    Directory.Delete(@strTargetPath, true);
                    Console.WriteLine("Delet TargetPath1!");
                }

                Directory.CreateDirectory(@strTargetPath);

                {


                    //使用
                    bool fgCopyDll = checkDLL.Checked;
                    bool fgCopyExe = checkEXE.Checked;
                    bool fgCopyAll = checkAll.Checked;


                    if ( fgCopyAll )
                    {
                        copyDir("复制", @strSourcePath, @strTargetPath, ".*");
                    }
                    else
                    {
                        if (fgCopyDll)
                        {
                            copyDir("复制", @strSourcePath, @strTargetPath, ".dll");
                        }

                        if (fgCopyExe)
                        {
                            copyDir("复制", @strSourcePath, @strTargetPath, ".exe");
                        }
                    }


                }

            }

            catch (Exception error)
            {
                Console.WriteLine("The process failed: {0}", error.ToString());
            }
        }

        public void copyDir(string workWhat, string fromDir, string toDir, string extension)
        {
            string[] files = Directory.GetFiles(@fromDir);
            foreach (string file in files)
            {
                FileInfo fiE = new FileInfo(@file);
                //loading文本说明 赋值
                if ( ".*" == extension )
                {
                    File.Copy(@file, @toDir + "\\" + Path.GetFileName(@file), true);
                }
                else if (workWhat == "复制")
                {
                    if (fiE.Extension == extension)
                    {
                        File.Copy(@file, @toDir + "\\" + Path.GetFileName(@file), true);
                    }
                }
                else if (workWhat == "剪切")
                {
                    if (fiE.Extension == extension)
                    {
                        File.Move(@file, @toDir + "\\" + Path.GetFileName(@file));
                    }
                }
                else if (workWhat == "删除")
                {
                    if (fiE.Extension == extension)
                    {
                        File.Delete(@file);
                    }
                }
                else
                {

                }
            }
            //子目录处理
           /* string[] dirs = Directory.GetDirectories(@fromDir);
            foreach (string dir in dirs)
            {
                if (!Directory.Exists(@toDir + "\\" + Path.GetFileName(@dir)))
                {
                    Directory.CreateDirectory(@toDir + "\\" + Path.GetFileName(@dir));
                }
                copyDir(@dir, @toDir + "\\" + Path.GetFileName(@dir));
            }*/
        }

        //Copy1
        private void Copy1_Click(object sender, EventArgs e)
        {
            //  CopyDone(@SourcePath1.Text, @TargetPath1.Text);
                
        }

        private void SelSourcePath1_Click(object sender, EventArgs e)
        {
            SetFolderBowserDialog(folderBrowserDialog1, SourcePath1 );
        }

        private void SelTargetPath1_Click(object sender, EventArgs e)
        {
            SetFolderBowserDialog(folderBrowserDialog1, TargetPath1);
        }


        //Copy2
        private void Copy2_Click(object sender, EventArgs e)
        {
            CopyDone(@SourcePath2.Text, @TargetPath2.Text);
        }

        private void SelSourcePath2_Click(object sender, EventArgs e)
        {
            SetFolderBowserDialog(folderBrowserDialog1, SourcePath2);
            Console.WriteLine(SourcePath2.Text);
        }

        private void SelTargetPath2_Click(object sender, EventArgs e)
        {
            SetFolderBowserDialog(folderBrowserDialog1, TargetPath2);
        }        

        //Copy3
        private void Copy3_Click(object sender, EventArgs e)
        {
            CopyDone(@SourcePath3.Text, @TargetPath3.Text);
        }

        private void SelSourcePath3_Click(object sender, EventArgs e)
        {
            SetFolderBowserDialog(folderBrowserDialog1, SourcePath3);
        }

        private void SelTargetPath3_Click(object sender, EventArgs e)
        {
            SetFolderBowserDialog(folderBrowserDialog1, TargetPath3);
        }

        //Save
        private void Save_Click(object sender, EventArgs e)
        {
            string filepath = @"C:\Users\Administrator\Documents\CopyAuto\Settings.ini";
            string sPath = @"C:\Users\Administrator\Documents\CopyAuto";
            StreamWriter swSettings;
            FileStream fsFile;
            bool fgExist = File.Exists(filepath);

            if (!Directory.Exists(sPath))
            {
                Directory.CreateDirectory(sPath);
            }

            //如文件不存在
            if ( false == fgExist )
            {
                //创建文件读写流,类型为创建新文件
                fsFile = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                swSettings = new StreamWriter(fsFile);
            }
            //否则,如果文件存在
            else
            {
                //创建文件读写流, 类型为打开已有的文件
                fsFile = new FileStream(filepath, FileMode.Open, FileAccess.Write);
                swSettings = new StreamWriter(fsFile);
            }

            swSettings.WriteLine("TargetPath1="+TargetPath1.Text);//开始写入值
            swSettings.WriteLine("TargetPath2="+TargetPath2.Text);//开始写入值
            swSettings.WriteLine("TargetPath3="+TargetPath3.Text);//开始写入值
            swSettings.WriteLine("SourcePath1="+SourcePath1.Text);//开始写入值
            swSettings.WriteLine("SourcePath2="+SourcePath2.Text);//开始写入值
            swSettings.WriteLine("SourcePath3="+SourcePath3.Text);//开始写入值
            swSettings.WriteLine("CheckDll=" + checkDLL.Checked);
            swSettings.WriteLine("CheckExe=" + checkEXE.Checked);
            swSettings.WriteLine("CheckAll=" + checkAll.Checked);
            swSettings.Close();
            fsFile.Close();
        }

        //选择路径文件存储到另外一个路径,通过数据流保存.
        private void SelFileToSave()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult res = ofd.ShowDialog();
            if (res == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(ofd.FileName))
                {
                    //1.创建读入文件流对象
                    FileStream streamRead = new FileStream(ofd.FileName, FileMode.Open);
                    //2.创建1个字节数组，用于接收文件流对象读操作文件值
                    byte[] data = new byte[1024 * 1024];//1M
                    int length = 0;
                    SaveFileDialog sfd = new SaveFileDialog();
                    DialogResult sres = sfd.ShowDialog();
                    if (sres == DialogResult.OK)
                    {
                        if (!string.IsNullOrEmpty(ofd.FileName))
                        {
                            FileStream streamWrite = new FileStream(sfd.FileName, FileMode.Create);
                            do
                            {
                                //3.文件流读方法的参数1.data-文件流读出数据要存的地方，2. 0--从什么位置读，3. data.Length--1次读多少字节数据
                                //3.1 Read方法的返回值是一个int类型的，代表他真实读取 字节数据的长度，
                                length = streamRead.Read(data, 0, data.Length);//大文件读入时候，我们定义字节长度的可能会有限，如果文件超大，要接收文件流对象的Read()方法，会返回读入的实际长度
                                                                               //加密 和解密
                                for (int i = 0; i < length; i++)
                                {
                                    data[i] = (byte)(255 - data[i]);
                                }
                                streamWrite.Write(data, 0, length);
                            } while (length == data.Length); //如果实际写入长度等于我们设定的长度，有两种情况1.文件正好是我们设定的长度2.文件超大只上传了截取的一部分
                        }
                    }


                }
            }
        }
    }
}
