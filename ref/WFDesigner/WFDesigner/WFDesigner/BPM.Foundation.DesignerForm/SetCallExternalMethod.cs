using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Workflow.Activities;

namespace wxwinter.WFDesigner.DesignerForm
{
    public partial class SetCallExternalMethod : Form
    {
        CallExternalMethodActivity activity;
        public SetCallExternalMethod()
        {
            InitializeComponent();
        }

        public SetCallExternalMethod(CallExternalMethodActivity activity)
        {
            InitializeComponent();
            this.activity = activity;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            try
            {
                var v = openDllDialog.ShowDialog();
                if (v == DialogResult.OK)
                {
                    this.assemblyTextBox.Text = openDllDialog.FileName;

                    this.Text = openDllDialog.SafeFileName;
                }

                if (System.IO.File.Exists(this.assemblyTextBox.Text))
                {
                    System.Reflection.Assembly asb;
                    asb = System.Reflection.Assembly.LoadFrom(this.assemblyTextBox.Text);

                    foreach (Type classtypes in asb.GetTypes())
                    {

                        foreach (var attrib in classtypes.GetCustomAttributes(true))
                        {
                            if (attrib.GetType().ToString() == "System.Workflow.Activities.ExternalDataExchangeAttribute")
                            {
                                classListBox.Items.Add(classtypes);
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("文件格式不正确");
            }
        }

        private void classListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (classListBox.SelectedItem != null)
            {

                Type tp = classListBox.SelectedItem as Type ;

                methodListBox.Items.Clear();

                foreach (System.Reflection.MethodInfo p in tp.GetMethods())
                {
                   methodListBox.Items.Add(p);
                }
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (classListBox.SelectedItem == null)
            {
                return;                
            }
            if (methodListBox.SelectedItem == null)
            {
                return;
            }
            activity.InterfaceType = classListBox.SelectedItem as Type;
            System.Reflection.MethodInfo p = methodListBox.SelectedItem as System.Reflection.MethodInfo;
            activity.MethodName = p.Name;

            string path = System.Environment.CurrentDirectory +@"\";
            try
            {
                System.IO.File.Copy(this.assemblyTextBox.Text, path + this.Text, true);
            }
            catch
            {
            }


            this.Close();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
