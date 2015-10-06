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
    //��ʾ�Ҽ��˵�
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

                   
                    if (menuItem.Text == "���ɴ������" ||
                        menuItem.Text == "��ѡ������(&B)..." ||
                        menuItem.Text == "����" ||
                        menuItem.Text == "����" ||
                        menuItem.Text == "���ص�״̬�����ͼ" ||
                        menuItem.Text == "����Ϊ��ʼ״̬" ||
                        menuItem.Text == "����Ϊ�����״̬" ||
                        menuItem.Text == "��� StateInitialization" ||
                         menuItem.Text == "�����ɰ�����" ||
                         menuItem.Text == "���״̬" ||
                         menuItem.Text == "��� EventDriven" ||
                         menuItem.Text == "�鿴 EventDriven" ||
                        menuItem.Text == "�鿴ȡ���������" ||
                        menuItem.Text == "�鿴���������" ||
                        menuItem.Text == "ǰ��" ||
                        menuItem.Text == "����" ||
                         menuItem.Text == "�鿴 StateInitialization" ||
                        menuItem.Text == "�鿴 StateFinalization" ||
                        menuItem.Text == "��� StateFinalization") 
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
                selectionCommands.Add(WorkflowMenuCommands.Cut, "����");
                selectionCommands.Add(WorkflowMenuCommands.Copy, "����");
                selectionCommands.Add(WorkflowMenuCommands.Paste, "ճ��");
                selectionCommands.Add(WorkflowMenuCommands.Delete, "ɾ��");



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
