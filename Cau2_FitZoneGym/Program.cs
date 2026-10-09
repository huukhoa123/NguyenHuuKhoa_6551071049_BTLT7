using System;
using System.Windows.Forms;

namespace Cau2_FitZoneGym
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