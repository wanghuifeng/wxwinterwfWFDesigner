namespace wxwinter.WFDesigner.DesignerForm
{
    partial class SetCallExternalMethod
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
            this.browseButton = new System.Windows.Forms.Button();
            this.assemblyTextBox = new System.Windows.Forms.TextBox();
            this.openDllDialog = new System.Windows.Forms.OpenFileDialog();
            this.classListBox = new System.Windows.Forms.ListBox();
            this.methodListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.exitButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(399, 10);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 0;
            this.browseButton.Text = "浏览DLL";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // assemblyTextBox
            // 
            this.assemblyTextBox.Location = new System.Drawing.Point(12, 12);
            this.assemblyTextBox.Name = "assemblyTextBox";
            this.assemblyTextBox.Size = new System.Drawing.Size(381, 21);
            this.assemblyTextBox.TabIndex = 1;
            // 
            // openDllDialog
            // 
            this.openDllDialog.FileName = "openFileDialog1";
            // 
            // classListBox
            // 
            this.classListBox.FormattingEnabled = true;
            this.classListBox.ItemHeight = 12;
            this.classListBox.Location = new System.Drawing.Point(18, 60);
            this.classListBox.Name = "classListBox";
            this.classListBox.Size = new System.Drawing.Size(603, 100);
            this.classListBox.TabIndex = 2;
            this.classListBox.SelectedIndexChanged += new System.EventHandler(this.classListBox_SelectedIndexChanged);
            // 
            // methodListBox
            // 
            this.methodListBox.FormattingEnabled = true;
            this.methodListBox.ItemHeight = 12;
            this.methodListBox.Location = new System.Drawing.Point(18, 196);
            this.methodListBox.Name = "methodListBox";
            this.methodListBox.Size = new System.Drawing.Size(603, 100);
            this.methodListBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "DLL中的类列表";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 178);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "类中的方法列表";
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(546, 309);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 23);
            this.exitButton.TabIndex = 6;
            this.exitButton.Text = "取消";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(453, 309);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 7;
            this.okButton.Text = "确定";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // SetCallExternalMethod
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 344);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.methodListBox);
            this.Controls.Add(this.classListBox);
            this.Controls.Add(this.assemblyTextBox);
            this.Controls.Add(this.browseButton);
            this.Name = "SetCallExternalMethod";
            this.Text = "SetCallExternalMethod";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.TextBox assemblyTextBox;
        private System.Windows.Forms.OpenFileDialog openDllDialog;
        private System.Windows.Forms.ListBox classListBox;
        private System.Windows.Forms.ListBox methodListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button okButton;
    }
}