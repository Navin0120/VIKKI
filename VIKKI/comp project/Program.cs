using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Threading;
using System.Speech.Recognition;
using System.IO;
using System.Diagnostics;

namespace comp_project
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
