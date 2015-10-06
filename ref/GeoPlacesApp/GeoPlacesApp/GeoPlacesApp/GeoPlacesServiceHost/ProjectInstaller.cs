﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess.Design;
using System.ServiceProcess;
using System.Windows.Forms;


namespace GeoPlacesServiceHost
{
    /// <summary>
    /// A custom Windows service installer
    /// </summary>
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        #region Ctor
        public ProjectInstaller()
        {
            InitializeComponent();
        }
        #endregion

        #region Public Methods
        // Prompt the user for service installation account values.
        public static bool GetServiceAccount(ref ServiceProcessInstaller svcInst)
        {
            bool accountSet = false;
            ServiceInstallerDialog svcDialog = new ServiceInstallerDialog();

            // Query the user for the service account type.
            do
            {
                svcDialog.TopMost = true;
                svcDialog.StartPosition = FormStartPosition.CenterScreen;
                svcDialog.ShowDialog();

                if (svcDialog.Result == ServiceInstallerDialogResult.OK)
                {
                    // Do a very simple validation on the user
                    // input.  Check to see whether the user name
                    // or password is blank.

                    if ((svcDialog.Username.Length > 0) &&
                        (svcDialog.Password.Length > 0))
                    {
                        // Use the account and password.
                        accountSet = true;

                        svcInst.Account = ServiceAccount.User;
                        svcInst.Username = svcDialog.Username;
                        svcInst.Password = svcDialog.Password;
                    }
                }
                else if (svcDialog.Result == ServiceInstallerDialogResult.UseSystem)
                {
                    svcInst.Account = ServiceAccount.LocalSystem;
                    svcInst.Username = null;
                    svcInst.Password = null;
                    accountSet = true;
                }

                if (!accountSet)
                {
                    // Display a message box.  Tell the user to
                    // enter a valid user and password, or cancel
                    // out to leave the service account alone.
                    DialogResult result;
                    result = MessageBox.Show(
                        "Invalid user name or password for service installation." +
                        "  Press Cancel to leave the service account unchanged.",
                        "Change Service Account",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Hand);

                    if (result == DialogResult.Cancel)
                    {
                        // Break out of loop.
                        break;
                    }
                }
            } while (!accountSet);

            return accountSet;
        }
        #endregion
    }
}
