using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Forms;
using Microsoft.Win32;

namespace coursera_capture
{
    public partial class Form1 : Form
    {
        public static Size courseraSize = new Size(940, 585);
        public static String startupName = "CourseraUtils";
        KeyboardHook hook = new KeyboardHook();

        public Form1()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Visible = false;
            this.notifyIcon1.Visible = true;          
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;

            // register the event that is fired after the key press.
            hook.KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            // register the control + alt + F12 combination as hot key.
            //hook.RegisterHotKey(coursera_capture.ModifierKeys.Control | coursera_capture.ModifierKeys.Alt, Keys.F12);
            hook.RegisterHotKey(coursera_capture.ModifierKeys.Control, Keys.D1);
        }

        void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            captureCourseraScreen();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            captureCourseraScreen();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.notifyIcon1.Visible = false;
            Application.Exit();
        }

        private void captureCourseraMenuItem_Click(object sender, EventArgs e)
        {
            captureCourseraScreen();
        }

        public void captureCourseraScreen()
        {
            // Capture
            //작업 표시줄을 제외한 영역 크기   
            int w = Screen.PrimaryScreen.WorkingArea.Width;
            int h = Screen.PrimaryScreen.WorkingArea.Height;

            // Determine the size of the "virtual screen", which includes all monitors.
            int screenLeft = SystemInformation.VirtualScreen.Left;
            int screenTop = SystemInformation.VirtualScreen.Top;
            int screenWidth = SystemInformation.VirtualScreen.Width;
            int screenHeight = SystemInformation.VirtualScreen.Height;

            //Bitmap 객체 생성   
            Bitmap bmp = new Bitmap(Form1.courseraSize.Width, Form1.courseraSize.Height);

            //Graphics 객체 생성   
            Graphics g = Graphics.FromImage(bmp);

            //Graphics 객체의 CopyFromScreen()메서드로 bitmap 객체에 Screen을 캡처하여 저장   

            //g.CopyFromScreen(screenLeft, screenTop, 0, 0, bmp.Size);
            g.CopyFromScreen(screenLeft + 442, screenTop + 225, 0, 0, Form1.courseraSize);

            Clipboard.SetImage((Image)bmp);
        }

        private void addStartupToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                // 시작프로그램 등록하는 레지스트리
                string runKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                RegistryKey strUpKey = Registry.LocalMachine.OpenSubKey(runKey);
                if (strUpKey.GetValue(startupName) == null)
                {
                    strUpKey.Close();
                    strUpKey = Registry.LocalMachine.OpenSubKey(runKey, true);
                    // 시작프로그램 등록명과 exe경로를 레지스트리에 등록
                    strUpKey.SetValue(startupName, Application.ExecutablePath);
                }
                MessageBox.Show("Add Startup Success");
            }
            catch
            {
                MessageBox.Show("Add Startup Fail");
            }
        }

        private void removeStartupToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                string runKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                RegistryKey strUpKey = Registry.LocalMachine.OpenSubKey(runKey, true);
                // 레지스트리값 제거
                strUpKey.DeleteValue(startupName);
                MessageBox.Show("Remove Startup Success");
            }
            catch
            {
                MessageBox.Show("Remove Startup Fail");
            }
        }
    }
}
