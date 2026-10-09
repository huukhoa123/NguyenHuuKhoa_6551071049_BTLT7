using System;
using System.Windows.Forms;

namespace Cau3_SunriseHomestay
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