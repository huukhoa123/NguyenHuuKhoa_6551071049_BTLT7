using System;
using System.Windows.Forms;

namespace Cau4_AnKhangClinic
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FormMain());
        }
    }
}