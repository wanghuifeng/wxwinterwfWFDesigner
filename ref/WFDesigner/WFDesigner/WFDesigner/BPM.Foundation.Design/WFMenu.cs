using System;
using System.IO;
using System.Text;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Windows.Forms;
using System.Drawing;

namespace wxwinter.WFDesigner.Design
{
    //显示右键菜单
    public sealed class WFMenu : MenuCommandService
    {
        public WFMenu(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public override void ShowContextMenu(CommandID menuID, int x, int y)
        {
            if (menuID == WorkflowMenuCommands.SelectionMenu)
            {
                ContextMenu contextMenu = new ContextMenu();
 
                foreach (DesignerVerb verb in Verbs)
                {
                    MenuItem menuItem = new MenuItem(verb.Text, new EventHandler(OnMenuClicked));
                    menuItem.Tag = verb;

                   
                    if (menuItem.Text == "生成处理程序" ||
                        menuItem.Text == "绑定选定属性(&B)..." ||
                        menuItem.Text == "左移" ||
                        menuItem.Text == "右移" ||
                        menuItem.Text == "返回到状态组合视图" ||
                        menuItem.Text == "设置为初始状态" ||
                        menuItem.Text == "设置为已完成状态" ||
                        menuItem.Text == "添加 StateInitialization" ||
                         menuItem.Text == "升级可绑定属性" ||
                         menuItem.Text == "添加状态" ||
                         menuItem.Text == "添加 EventDriven" ||
                         menuItem.Text == "查看 EventDriven" ||
                        menuItem.Text == "查看取消处理程序" ||
                        menuItem.Text == "查看错误处理程序" ||
                        menuItem.Text == "前置" ||
                        menuItem.Text == "后置" ||
                         menuItem.Text == "查看 StateInitialization" ||
                        menuItem.Text == "查看 StateFinalization" ||
                        menuItem.Text == "添加 StateFinalization") 
                    {
                        
                    }
                    else
                    {
                          contextMenu.MenuItems.Add(menuItem);
                    }


                }

                MenuItem[] items = GetSelectionMenuItems();
                if (items.Length > 0)
                {
                    contextMenu.MenuItems.Add(new MenuItem("-"));
                    foreach (MenuItem item in items)
                    {
                        contextMenu.MenuItems.Add(item);
                    }
                }

                WorkflowView workflowView = GetService(typeof(WorkflowView)) as WorkflowView;
                if (workflowView != null)
                {
                    contextMenu.Show(workflowView, workflowView.PointToClient(new Point(x, y)));
                }
            }
        }

        private void OnMenuClicked(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

          
            if (menuItem != null && menuItem.Tag is MenuCommand)
            {
              
                MenuCommand command = menuItem.Tag as MenuCommand;

               
                command.Invoke();
            }
        }

        private MenuItem[] GetSelectionMenuItems()
        {
            List<MenuItem> menuItems = new List<MenuItem>();

            bool addMenuItems = true;
            ISelectionService selectionService = GetService(typeof(ISelectionService)) as ISelectionService;
            if (selectionService != null)
            {
                foreach (object obj in selectionService.GetSelectedComponents())
                {
                    if (!(obj is Activity))
                    {
                        addMenuItems = false;
                        break;
                    }
                }
            }

            if (addMenuItems)
            {
                Dictionary<CommandID, string> selectionCommands = new Dictionary<CommandID, string>();
                selectionCommands.Add(WorkflowMenuCommands.Cut, "剪切");
                selectionCommands.Add(WorkflowMenuCommands.Copy, "复制");
                selectionCommands.Add(WorkflowMenuCommands.Paste, "粘贴");
                selectionCommands.Add(WorkflowMenuCommands.Delete, "删除");



                foreach (CommandID id in selectionCommands.Keys)
                {
                    MenuCommand command = FindCommand(id);
                    if (command != null)
                    {
                        MenuItem menuItem = new MenuItem(selectionCommands[id], new EventHandler(OnMenuClicked));
                        menuItem.Tag = command;
                        menuItems.Add(menuItem);
                    }
                }
            }

            return menuItems.ToArray();
        }
    }
}
