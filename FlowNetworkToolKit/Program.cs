﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlowNetworkToolKit.Core.Utils.Logger;
using FlowNetworkToolKit.Forms;

namespace FlowNetworkToolKit
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Log.Init();
            Application.Run(new FMain());
        }
    }
}
