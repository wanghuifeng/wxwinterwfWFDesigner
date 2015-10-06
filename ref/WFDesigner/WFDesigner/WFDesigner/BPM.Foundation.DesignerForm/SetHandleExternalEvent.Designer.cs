namespace wxwinter.WFDesigner.DesignerForm
{
    partial class SetHandleExternalEvent
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.okButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.eventListBox = new System.Windows.Forms.ListBox();
            this.classListBox = new System.Windows.Forms.ListBox();
            this.assemblyTextBox = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.openDllDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(484, 309);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 15;
            this.okButton.Text = "确定";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(577, 309);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 23);
            this.exitButton.TabIndex = 14;
            this.exitButton.Text = "取消";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 173);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "类中的方法列表";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "DLL中的类列表";
            // 
            // eventListBox
            // 
            this.eventListBox.FormattingEnabled = true;
            this.eventListBox.ItemHeight = 12;
            this.eventListBox.Location = new System.Drawing.Point(12, 191);
            this.eventListBox.Name = "eventListBox";
            this.eventListBox.Size = new System.Drawing.Size(640, 100);
            this.eventListBox.TabIndex = 11;
            // 
            // classListBox
            // 
            this.classListBox.FormattingEnabled = true;
            this.classListBox.ItemHeight = 12;
            this.classListBox.Location = new System.Drawing.Point(12, 55);
            this.classListBox.Name = "classListBox";
            this.classListBox.Size = new System.Drawing.Size(640, 100);
            this.classListBox.TabIndex = 10;
            this.classListBox.SelectedIndexChanged += new System.EventHandler(this.classListBox_SelectedIndexChanged);
            // 
            // assemblyTextBox
            // 
            this.assemblyTextBox.Location = new System.Drawing.Point(12, 7);
            this.assemblyTextBox.Name = "assemblyTextBox";
            this.assemblyTextBox.Size = new System.Drawing.Size(375, 21);
            this.assemblyTextBox.TabIndex = 9;
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(393, 5);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 8;
            this.browseButton.Text = "浏览DLL";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // openDllDialog
            // 
            this.openDllDialog.FileName = "openFileDialog1";
            // 
            // SetHandleExternalEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 344);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.eventListBox);
            this.Controls.Add(this.classListBox);
            this.Controls.Add(this.assemblyTextBox);
            this.Controls.Add(this.browseButton);
            this.Name = "SetHandleExternalEvent";
            this.Text = "SetHandleExternalEvent";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox eventListBox;
        private System.Windows.Forms.ListBox classListBox;
        private System.Windows.Forms.TextBox assemblyTextBox;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.OpenFileDialog openDllDialog;
    }
}