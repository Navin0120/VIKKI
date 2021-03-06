using System;   
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Management;
using System.Speech.Synthesis;
using System.Threading;
using System.Speech.Recognition;
using System.IO;
using System.Diagnostics;

namespace comp_project
{
    public partial class Form1 : Form
    {
        enum RecycleFlags : uint
        {
            SHERB_NOCONFIRMATION = 0x00000001,
            SHERB_NOPROGRESSUI = 0x00000002,
            SHERB_NOSOUND = 0x00000004
        }
        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        static extern uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlags dwFlags);
        static SpeechSynthesizer synth = new SpeechSynthesizer();
        static Process n = new Process();
        static ManualResetEvent _completed = new ManualResetEvent(false);
        static SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine();
        static Choices Choice = new Choices(new Choices(System.IO.File.ReadAllLines(@"D:\Project\Final\Commands.txt")));
        static Choices Choices = new Choices(new Choices(System.IO.File.ReadAllLines(@"D:\Project\Final\Commands2.txt")));
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int WM_APPCOMMAND = 0x319;
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg,
            IntPtr wParam, IntPtr lParam);
        public Form1()
        {
            InitializeComponent();
            using (GraphicsPath GP = new GraphicsPath())
            {
                GP.AddEllipse(this.ClientRectangle);
                this.Region = new Region(GP);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = System.Drawing.Color.Black;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            (new SoundPlayer(@"D:\Project\Final\button-3.wav")).Play();
            Application.Exit();
        }
        const int WM_NCHITTEST = 0x0084;
        const int HTCAPTION = 2;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_NCHITTEST)
            {
                Point pt = this.PointToClient(new Point(m.LParam.ToInt32()));
                if (ClientRectangle.Contains(pt))
                {
                    m.Result = new IntPtr(HTCAPTION);
                    return;
                }
            }
            base.WndProc(ref m);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            (new SoundPlayer(@"D:\Project\Final\elastique.wav")).Play();
            synth.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen);
            synth.SetOutputToDefaultAudioDevice();
            _recognizer.SetInputToDefaultAudioDevice();
            Thread.Sleep(2000);
            synth.Speak("Welcome Sir! My Name Is VIKI Virtual Interface Kinetic Intelligence! How Can I Help?");
            try
            {
                _recognizer.LoadGrammar(new Grammar(new GrammarBuilder(Choice)));
                _recognizer.SpeechRecognized += _recognizer_SpeechRecognized;
                _recognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch
            {
                return;
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            (new SoundPlayer(@"D:\Project\Final\button-3.wav")).Play();
            this.WindowState = FormWindowState.Minimized;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            (new SoundPlayer(@"D:\Project\Final\button-3.wav")).Play();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            (new SoundPlayer(@"D:\Project\Final\button-3.wav")).Play();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            (new SoundPlayer(@"D:\Project\Final\button-3.wav")).Play();
            SelectQuery Sq = new SelectQuery("Win32_Battery");
            ManagementObjectSearcher objOSDetails = new ManagementObjectSearcher(Sq);
            ManagementObjectCollection osDetailsCollection = objOSDetails.Get();
            StringBuilder sb = new StringBuilder();
            foreach (ManagementObject mo in osDetailsCollection)
            {
                sb.AppendLine(string.Format("EstimatedChargeRemaining: {0}%", (ushort)mo["EstimatedChargeRemaining"]));
                sb.AppendLine(string.Format("Status : {0}", (string)mo["Status"]));
                sb.AppendLine(string.Format("Name : {0}", (string)mo["Name"]));
                sb.AppendLine(string.Format("Description: {0}", (string)mo["Description"]));
                sb.AppendLine(string.Format("DesignVoltage: {0}", (ulong)mo["DesignVoltage"]));
                sb.AppendLine(string.Format("DeviceID : {0}", (string)mo["DeviceID"]));
                sb.AppendLine(string.Format("InstallDate: {0}", Convert.ToDateTime(mo["InstallDate"]).ToString()));
            }
            MessageBox.Show(sb.ToString());
        }
        private void button6_Click(object sender, EventArgs e)
        {
            (new SoundPlayer(@"D:\Project\Final\button-3.wav")).Play();
            SendMessageW(this.Handle, WM_APPCOMMAND, this.Handle,
               (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            (new SoundPlayer(@"D:\Project\Final\button-3.wav")).Play();
            MessageBox.Show("1. My Computer\n2. Shutdown\n3. Restart\n4. Facebook\n5. Exit Window\n6. Hey Viki\n7. Goodbye\n8. Logical Drives\n9. Empty Recycle Bin\n10. Log Off\n11. What time is it\n12. Whats the time\n13. What time it is\n14. What day is it\n15. Whats the date\n16. Whats todays date\n17. Check mail\n18. cmd\n19. Notepad\n20. Whats your Name\n21. Whats My Name\n22. Wikipedia\n23. C\n24. D\n25. DVD Drive\n27. Exit Notepad");
        }
        static void _recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text.ToString();
            string path = @"D:\Project\Final\Commands2.txt";
            try
            {
                _recognizer.LoadGrammar(new Grammar(new GrammarBuilder(Choices)));
            }
            catch
            {
                File.WriteAllText(path, "Default");
            }
            switch (speech)
            {
                case "Hey Viki":
                case "Hi VIKI":
                    synth.Speak("Hello sir!");
                    break;

                case "Whats your Name":
                    synth.Speak("VIKI Virtual Interface Kinetic Intelligence!");
                    break;

                case "Whats My Name":
                    synth.Speak(System.Environment.UserName);
                    break;

                case "Goodbye":
                case "Goodbye VIKI":
                    synth.Speak("Until next time sir!");
                    System.Environment.Exit(0);
                    break;
                case "cmd":
                    synth.Speak("Opening cmd");
                    Process.Start("cmd.exe");
                    break;
                case "Notepad":
                    synth.Speak("Opening Notepad");
                    n = Process.Start(@"D:\Project\Final\Untitled.txt");
                    _recognizer.SpeechRecognized += Untitled;
                    _recognizer.SpeechRecognized -= _recognizer_SpeechRecognized;
                    break;
                case "My Computer":
                    synth.Speak("Opening My Computer");
                    Process.Start("explorer.exe", "::{20d04fe0-3aea-1069-a2d8-08002b30309d}");
                    break;

                case "Shutdown":
                    synth.Speak("Shutting Down");
                    //  Process.Start("shutdown", "-s");
                    break;

                case "Restart":
                    synth.Speak("Restarting");
                    //  Process.Start("shutdown","-r");
                    break;

                case "Log Off":
                    synth.Speak("Logging Off");
                    //   Process.Start("shutdown","-l");
                    break;

                case "Empty Recycle Bin":
                    synth.Speak("As you wish sir!");
                    uint result = SHEmptyRecycleBin(IntPtr.Zero, null, 0);
                    break;

                case "What time is it":
                case "Whats the time":
                case "What time it is":
                    DateTime now = DateTime.Now;
                    string time = now.GetDateTimeFormats('t')[0];
                    synth.Speak(time);
                    break;

                case "What day is it":
                    synth.Speak(DateTime.Today.ToString("dddd"));
                    break;

                case "Whats the date":
                case "Whats todays date":
                    synth.Speak(DateTime.Today.ToString("dd-MM-yyyy"));
                    break;

                case "Check mail":
                    synth.Speak("Checking your mail sir");
                    Process.Start(@"www.outlook.com");
                    break;

                case "Facebook":
                    synth.Speak("Opening Facebook");
                    Process.Start(@"www.facebook.com");
                    break;

                case "C":
                    synth.Speak("Opening Local Disk C");
                    Process.Start(@"C:\");
                    break;
                case "D":
                    synth.Speak("Opening Local Disk D");
                    Process.Start(@"D:\");
                    break;
                case "E":
                    synth.Speak("Opening Local Disk E");
                    Process.Start(@"E:\");
                    break;
                case "F":
                    synth.Speak("Opening Local Disk E");
                    Process.Start(@"E:\");
                    break;
                case "DVD Drive":
                    synth.Speak("Opening DVD Drive");
                    Process.Start(@"G:\");
                    break;
                case "Logical Drives":
                    string[] createText = Directory.GetLogicalDrives();
                    File.WriteAllLines(path, createText);
                    string[] readText2 = File.ReadAllLines(path, Encoding.UTF8);
                    foreach (string s in readText2)
                    {
                        Console.WriteLine(s);
                        synth.Speak(s);
                    }
                    break;

                case "Wikipedia":
                    synth.Speak("Opening Wikipedia");
                    Process.Start(@"www.wikipedia.com");
                    break;
                case "Exit Window":
                    foreach (Process myProc in Process.GetProcesses())
                    {
                        if (myProc.ProcessName == "explorer")
                            myProc.Kill();
                    }
                    break;
            }
        }
        static void Untitled(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text.ToString();
            switch (speech)
            {
                case "Exit Notepad":
                    n.Kill();
                    _recognizer.SpeechRecognized -= Untitled;
                    _recognizer.SpeechRecognized += _recognizer_SpeechRecognized;
                    break;
                default:
                    File.AppendAllText(@"D:\Project\Final\Untitled.txt", " ");
                    File.AppendAllText(@"D:\Project\Final\Untitled.txt", speech);
                    n.CloseMainWindow();
                    n = Process.Start(@"D:\Project\Final\Untitled.txt");
                    break;
            }
        }

    }
}
