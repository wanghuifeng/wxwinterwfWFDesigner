using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EARTHLib;
using System.Runtime.InteropServices;

namespace GoogleEarthControl
{
    public partial class WinFormGEContainerControl : UserControl
    {

 
        private ApplicationGEClass googleEarth;
        private IntPtr mainWindowPtr = (IntPtr)(-1);

        public WinFormGEContainerControl()
        {
            InitializeComponent();

            
        }

        private void WinFormGEContainerControl_Load(object sender, EventArgs e)
        {

            mainWindowPtr = this.Handle;
            googleEarth = new ApplicationGEClass();
            Win32.GEHrender = (IntPtr)googleEarth.GetRenderHwnd();
            Win32.MoveWindow(Win32.GEHrender, 0, 0, (int)this.Width, (int)this.Height, true);
            Win32.SetParent(Win32.GEHrender, mainWindowPtr);
            Win32.SetWindowPos(googleEarth.GetMainHwnd(), Win32.HWND_BOTTOM,
                10, 10, 10, 10, Win32.SWP_HIDEWINDOW);


        }




        public void LoadXMLFile(String file)
        {
            //using (TextReader tr = new StreamReader(file))
            //{
            //    String s = tr.ReadToEnd().ToString();
            googleEarth.OpenKmlFile(file, 1);
            //}
        }


        public void StopGE()
        {
            try
            {
                Win32.SendMessage((IntPtr)googleEarth.GetMainHwnd(), Win32.WM_SYSCOMMAND,
                    (IntPtr)Win32.SC_CLOSE, (IntPtr)0);
            }
            catch (Exception)
            {
                //Ok P/Invoke close didn't work, so have no choice but to kill process
                Process[] p = Process.GetProcessesByName("googleearth");
                if (p.Length > 0)
                {
                    try
                    {
                        p[0].Kill();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("There was a problem shutting down googleearth");
                    }

                }
            }
            finally
            {
                try
                {
                    if (googleEarth != null)
                            Marshal.ReleaseComObject(googleEarth);
                }
                catch (ArgumentException argEx)
                {
                    Console.WriteLine("There was a problem shutting down googleearth");
                }

            }



        }





    }
}
