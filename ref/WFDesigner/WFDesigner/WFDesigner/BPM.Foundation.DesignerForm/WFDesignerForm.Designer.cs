namespace wxwinter.WFDesigner.DesignerForm
{
    partial class WFDesignerForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.workFlowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.新建空工作流ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.打开Xoml文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存Xoml文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.加载功能类ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置CallExternalMethod绑定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置HandleExternalEvent绑定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看绑定信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.面板 = new System.Windows.Forms.Panel();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.生成业务流程图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.workFlowToolStripMenuItem,
            this.加载功能类ToolStripMenuItem,
            this.工具ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1064, 25);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // workFlowToolStripMenuItem
            // 
            this.workFlowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.新建空工作流ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.打开Xoml文件ToolStripMenuItem,
            this.保存Xoml文件ToolStripMenuItem,
            this.toolStripMenuItem2});
            this.workFlowToolStripMenuItem.Name = "workFlowToolStripMenuItem";
            this.workFlowToolStripMenuItem.Size = new System.Drawing.Size(56, 21);
            this.workFlowToolStripMenuItem.Text = "工作流";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItem3.Text = "新建顺序工作流";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // 新建空工作流ToolStripMenuItem
            // 
            this.新建空工作流ToolStripMenuItem.Name = "新建空工作流ToolStripMenuItem";
            this.新建空工作流ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.新建空工作流ToolStripMenuItem.Text = "新建状态机作流";
            this.新建空工作流ToolStripMenuItem.Click += new System.EventHandler(this.新建空工作流ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(157, 6);
            // 
            // 打开Xoml文件ToolStripMenuItem
            // 
            this.打开Xoml文件ToolStripMenuItem.Name = "打开Xoml文件ToolStripMenuItem";
            this.打开Xoml文件ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.打开Xoml文件ToolStripMenuItem.Text = "打开Xoml文件";
            this.打开Xoml文件ToolStripMenuItem.Click += new System.EventHandler(this.打开Xoml文件ToolStripMenuItem_Click);
            // 
            // 保存Xoml文件ToolStripMenuItem
            // 
            this.保存Xoml文件ToolStripMenuItem.Name = "保存Xoml文件ToolStripMenuItem";
            this.保存Xoml文件ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.保存Xoml文件ToolStripMenuItem.Text = "保存Xoml文件";
            this.保存Xoml文件ToolStripMenuItem.Click += new System.EventHandler(this.保存Xoml文件ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(157, 6);
            // 
            // 加载功能类ToolStripMenuItem
            // 
            this.加载功能类ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置CallExternalMethod绑定ToolStripMenuItem,
            this.设置HandleExternalEvent绑定ToolStripMenuItem});
            this.加载功能类ToolStripMenuItem.Name = "加载功能类ToolStripMenuItem";
            this.加载功能类ToolStripMenuItem.Size = new System.Drawing.Size(80, 21);
            this.加载功能类ToolStripMenuItem.Text = "加载功能类";
            // 
            // 设置CallExternalMethod绑定ToolStripMenuItem
            // 
            this.设置CallExternalMethod绑定ToolStripMenuItem.Name = "设置CallExternalMethod绑定ToolStripMenuItem";
            this.设置CallExternalMethod绑定ToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.设置CallExternalMethod绑定ToolStripMenuItem.Text = "设置CallExternalMethod绑定";
            this.设置CallExternalMethod绑定ToolStripMenuItem.Click += new System.EventHandler(this.设置CallExternalMethod绑定ToolStripMenuItem_Click);
            // 
            // 设置HandleExternalEvent绑定ToolStripMenuItem
            // 
            this.设置HandleExternalEvent绑定ToolStripMenuItem.Name = "设置HandleExternalEvent绑定ToolStripMenuItem";
            this.设置HandleExternalEvent绑定ToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.设置HandleExternalEvent绑定ToolStripMenuItem.Text = "设置HandleExternalEvent绑定";
            this.设置HandleExternalEvent绑定ToolStripMenuItem.Click += new System.EventHandler(this.设置HandleExternalEvent绑定ToolStripMenuItem_Click);
            // 
            // 工具ToolStripMenuItem
            // 
            this.工具ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查看绑定信息ToolStripMenuItem,
            this.生成业务流程图ToolStripMenuItem});
            this.工具ToolStripMenuItem.Name = "工具ToolStripMenuItem";
            this.工具ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.工具ToolStripMenuItem.Text = "工具";
            // 
            // 查看绑定信息ToolStripMenuItem
            // 
            this.查看绑定信息ToolStripMenuItem.Name = "查看绑定信息ToolStripMenuItem";
            this.查看绑定信息ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.查看绑定信息ToolStripMenuItem.Text = "查看绑定信息";
            this.查看绑定信息ToolStripMenuItem.Click += new System.EventHandler(this.查看绑定信息ToolStripMenuItem_Click);
            // 
            // 面板
            // 
            this.面板.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.面板.Dock = System.Windows.Forms.DockStyle.Fill;
            this.面板.Location = new System.Drawing.Point(0, 0);
            this.面板.Name = "面板";
            this.面板.Size = new System.Drawing.Size(1064, 429);
            this.面板.TabIndex = 1;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "png files (*.png)|";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.面板);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1064, 429);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 25);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(1064, 454);
            this.toolStripContainer1.TabIndex = 6;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // 生成业务流程图ToolStripMenuItem
            // 
            this.生成业务流程图ToolStripMenuItem.Name = "生成业务流程图ToolStripMenuItem";
            this.生成业务流程图ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.生成业务流程图ToolStripMenuItem.Text = "生成业务流程图";
            this.生成业务流程图ToolStripMenuItem.Click += new System.EventHandler(this.生成业务流程图ToolStripMenuItem_Click);
            // 
            // WFDesignerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 479);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "WFDesignerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "工作流设计器";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.工作流设计器_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem workFlowToolStripMenuItem;
        private System.Windows.Forms.Panel 面板;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStripMenuItem 新建空工作流ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 打开Xoml文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存Xoml文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem 加载功能类ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置CallExternalMethod绑定ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置HandleExternalEvent绑定ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 工具ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看绑定信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 生成业务流程图ToolStripMenuItem;


    }
}