using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing.Design;
using System.Drawing.Text;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Windows.Forms.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;


namespace wxwinter.WFDesigner.Design
{
    [ToolboxItem(false)]
    public class ToolBar : Panel, IToolboxService
    {
        private IServiceProvider provider;
        private Hashtable customCreators;
        private Type currentSelection;
        private ListBox listBox = new ListBox();


        public string ToolsFile
        {
            get;
            set;
        }


        public ToolBar(IServiceProvider provider, string toolsFile)
        {
            this.ToolsFile = toolsFile;
            this.provider = provider;

            Text = "¹¤¾ßÀ¸";
            Size = new System.Drawing.Size(224, 350);

            listBox.Dock = DockStyle.Fill;
            listBox.IntegralHeight = false;
            listBox.ItemHeight = 20;
            listBox.DrawMode = DrawMode.OwnerDrawFixed;
            listBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            listBox.BackColor = SystemColors.Window;
            listBox.ForeColor = SystemColors.ControlText;
            listBox.MouseMove += new MouseEventHandler(OnListBoxMouseMove);
            Controls.Add(listBox);

            listBox.DrawItem += new DrawItemEventHandler(this.OnDrawItem);
            listBox.SelectedIndexChanged += new EventHandler(this.OnListBoxClick);

            AddToolboxEntries(listBox);
        }

        public void AddCreator(ToolboxItemCreatorCallback creator, string format)
        {
            AddCreator(creator, format, null);
        }

        public void AddCreator(ToolboxItemCreatorCallback creator, string format, IDesignerHost host)
        {
            if (creator == null || format == null)
            {
                throw new ArgumentNullException(creator == null ? "creator" : "format");
            }

            if (customCreators == null)
            {
                customCreators = new Hashtable();
            }
            else
            {
                string key = format;

                if (host != null) key += ", " + host.GetHashCode().ToString();

                if (customCreators.ContainsKey(key))
                {
                    throw new Exception( format );
                }
            }

            customCreators[format] = creator;
        }

        public void AddLinkedToolboxItem(ToolboxItem toolboxItem, IDesignerHost host)
        {
        }

        public void AddLinkedToolboxItem(ToolboxItem toolboxItem, string category, IDesignerHost host)
        {
        }

        public virtual void AddToolboxItem(ToolboxItem toolboxItem)
        {
        }

        public virtual void AddToolboxItem(ToolboxItem toolboxItem, IDesignerHost host)
        {
        }

        public virtual void AddToolboxItem(ToolboxItem toolboxItem, string category)
        {
        }

        public virtual void AddToolboxItem(ToolboxItem toolboxItem, string category, IDesignerHost host)
        {
        }

        public CategoryNameCollection CategoryNames
        {
            get
            {
                return new CategoryNameCollection(new string[] { "Workflow" });
            }
        }

        public string SelectedCategory
        {
            get
            {
                return "Workflow";
            }
            set
            {
            }
        }

        private ToolboxItemCreatorCallback FindToolboxItemCreator(IDataObject dataObj, IDesignerHost host, out string foundFormat)
        {
            foundFormat = string.Empty;

            ToolboxItemCreatorCallback creator = null;
            if (customCreators != null)
            {
                IEnumerator keys = customCreators.Keys.GetEnumerator();
                while (keys.MoveNext())
                {
                    string key = (string)keys.Current;
                    string[] keyParts = key.Split(new char[] { ',' });
                    string format = keyParts[0];

                    if (dataObj.GetDataPresent(format))
                    {

                        if (keyParts.Length == 1 || (host != null && host.GetHashCode().ToString().Equals(keyParts[1])))
                        {
                            creator = (ToolboxItemCreatorCallback)customCreators[format];
                            foundFormat = format;
                            break;
                        }
                    }
                }
            }

            return creator;
        }

        public virtual ToolboxItem GetSelectedToolboxItem()
        {
            ToolboxItem toolClass = null;
            if (this.currentSelection != null)
            {
                try
                {
                    toolClass = ToolBar.GetToolboxItem(this.currentSelection);
                }
                catch (TypeLoadException)
                {
                }
            }

            return toolClass;
        }

        public virtual ToolboxItem GetSelectedToolboxItem(IDesignerHost host)
        {
            return GetSelectedToolboxItem();
        }

        public object SerializeToolboxItem(ToolboxItem toolboxItem)
        {
            DataObject dataObject = new DataObject();
            dataObject.SetData(typeof(ToolboxItem), toolboxItem);
            return dataObject;
        }

        public ToolboxItem DeserializeToolboxItem(object dataObject)
        {
            return DeserializeToolboxItem(dataObject, null);
        }

        public ToolboxItem DeserializeToolboxItem(object data, IDesignerHost host)
        {
            IDataObject dataObject = data as IDataObject;

            if (dataObject == null)
            {
                return null;
            }

            ToolboxItem t = (ToolboxItem)dataObject.GetData(typeof(ToolboxItem));

            if (t == null)
            {
                string format;
                ToolboxItemCreatorCallback creator = FindToolboxItemCreator(dataObject, host, out format);

                if (creator != null)
                {
                    return creator(dataObject, format);
                }
            }

            return t;
        }

        public ToolboxItemCollection GetToolboxItems()
        {
            return new ToolboxItemCollection(new ToolboxItem[0]);
        }

        public ToolboxItemCollection GetToolboxItems(IDesignerHost host)
        {
            return new ToolboxItemCollection(new ToolboxItem[0]);
        }

        public ToolboxItemCollection GetToolboxItems(string category)
        {
            return new ToolboxItemCollection(new ToolboxItem[0]);
        }

        public ToolboxItemCollection GetToolboxItems(string category, IDesignerHost host)
        {
            return new ToolboxItemCollection(new ToolboxItem[0]);
        }

        public bool IsSupported(object data, IDesignerHost host)
        {
            return true;
        }

        public bool IsSupported(object serializedObject, ICollection filterAttributes)
        {
            return true;
        }

        public bool IsToolboxItem(object dataObject)
        {
            return IsToolboxItem(dataObject, null);
        }

        public bool IsToolboxItem(object data, IDesignerHost host)
        {
            IDataObject dataObject = data as IDataObject;
            if (dataObject == null)
                return false;

            if (dataObject.GetDataPresent(typeof(ToolboxItem)))
            {
                return true;
            }
            else
            {
                string format;
                ToolboxItemCreatorCallback creator = FindToolboxItemCreator(dataObject, host, out format);
                if (creator != null)
                    return true;
            }

            return false;
        }

        public new void Refresh()
        {
        }

        public void RemoveCreator(string format)
        {
            RemoveCreator(format, null);
        }

        public void RemoveCreator(string format, IDesignerHost host)
        {
            if (format == null)
                throw new ArgumentNullException("format");

            if (customCreators != null)
            {
                string key = format;
                if (host != null)
                    key += ", " + host.GetHashCode().ToString();
                customCreators.Remove(key);
            }
        }

        public virtual void RemoveToolboxItem(ToolboxItem toolComponentClass)
        {
        }

        public virtual void RemoveToolboxItem(ToolboxItem componentClass, string category)
        {
        }

        public virtual bool SetCursor()
        {
            if (this.currentSelection != null)
            {
                Cursor.Current = Cursors.Cross;
                return true;
            }

            return false;
        }

        public virtual void SetSelectedToolboxItem(ToolboxItem selectedToolClass)
        {
            if (selectedToolClass == null)
            {
                listBox.SelectedIndex = 0;
                OnListBoxClick(null, EventArgs.Empty);
            }
        }

        public void AddType(Type t)
        {
            listBox.Items.Add(new ToolItem(t.AssemblyQualifiedName));
        }

        public Attribute[] GetEnabledAttributes()
        {
            return null;
        }

        public void SetEnabledAttributes(Attribute[] attrs)
        {
        }

        public void SelectedToolboxItemUsed()
        {
            SetSelectedToolboxItem(null);
        }


        internal static ToolboxItem GetToolboxItem(Type toolType)
        {
            if (toolType == null)
                throw new ArgumentNullException("toolType");

            ToolboxItem item = null;
            if ((toolType.IsPublic || toolType.IsNestedPublic) && typeof(IComponent).IsAssignableFrom(toolType) && !toolType.IsAbstract)
            {
                ToolboxItemAttribute toolboxItemAttribute = (ToolboxItemAttribute)TypeDescriptor.GetAttributes(toolType)[typeof(ToolboxItemAttribute)];
                if (toolboxItemAttribute != null && !toolboxItemAttribute.IsDefaultAttribute())
                {
                    Type itemType = toolboxItemAttribute.ToolboxItemType;
                    if (itemType != null)
                    {
     
                        ConstructorInfo ctor = itemType.GetConstructor(new Type[] { typeof(Type) });
                        if (ctor != null)
                        {
                            item = (ToolboxItem)ctor.Invoke(new object[] { toolType });
                        }
                        else
                        {
                            ctor = itemType.GetConstructor(new Type[0]);
                            if (ctor != null)
                            {
                                item = (ToolboxItem)ctor.Invoke(new object[0]);
                                item.Initialize(toolType);
                            }
                        }
                    }
                }
                else if (!toolboxItemAttribute.Equals(ToolboxItemAttribute.None))
                {
                    item = new ToolboxItem(toolType);
                }
            }
            else if (typeof(ToolboxItem).IsAssignableFrom(toolType))
            {

                try
                {
                    item = (ToolboxItem)Activator.CreateInstance(toolType, true);
                }
                catch
                {
                }
            }

            return item;
        }


        private void AddToolboxEntries(ListBox lb)
        {
            if (ToolsFile == "")
            { return; }
            System.IO.FileStream selfTools = new FileStream(ToolsFile, System.IO.FileMode.Open);

            int len = (int)selfTools.Length;
            byte[] bytes = new byte[len];
            selfTools.Read(bytes, 0, len);

            string entries = Encoding.Default.GetString(bytes);

            int start = 0, end = 0;
            string entry;
            while (end < entries.Length)
            {
                end = entries.IndexOf('\r', start);
                if (end == -1)
                { end = entries.Length; }

                entry = entries.Substring(start, (end - start));
                if (entry.Length != 0 && !entry.StartsWith(";"))
                { lb.Items.Add(new ToolItem(entry)); }


                start = end + 2;
            }
            selfTools.Close();
        }

        private void OnDrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            ListBox listBox = (ListBox)sender;
            object objItem = listBox.Items[e.Index];
            ToolItem item = null;
            Bitmap bitmap = null;
            string text = null;

            if (objItem is string)
            {
                bitmap = null;
                text = (string)objItem;
            }
            else
            {
                try
                {
                    item = (ToolItem)objItem;
                    bitmap = (Bitmap)item.Glyph;
                    text = item.Name;
                }
                catch
                {

                }
            }

            bool selected = false;
            bool disabled = false;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                selected = true;

            if ((int)(e.State & (DrawItemState.Disabled | DrawItemState.Grayed | DrawItemState.Inactive)) != 0)
                disabled = true;

            StringFormat format = new StringFormat();
            format.HotkeyPrefix = HotkeyPrefix.Show;

            int x = e.Bounds.X + 4;
            x += 20;

            if (selected)
            {
                Rectangle r = e.Bounds;
                r.Width--; r.Height--;
                g.DrawRectangle(SystemPens.ActiveCaption, r);
            }
            else
            {
                g.FillRectangle(SystemBrushes.Menu, e.Bounds);
                using (Brush border = new SolidBrush(Color.FromArgb(Math.Min(SystemColors.Menu.R + 15, 255), Math.Min(SystemColors.Menu.G + 15, 255), Math.Min(SystemColors.Menu.B + 15, 255))))
                    g.FillRectangle(border, new Rectangle(e.Bounds.X, e.Bounds.Y, 20, e.Bounds.Height));
            }

            if (bitmap != null)
                g.DrawImage(bitmap, e.Bounds.X + 2, e.Bounds.Y + 2, bitmap.Width, bitmap.Height);

            Brush textBrush = (disabled) ? new SolidBrush(Color.FromArgb(120, SystemColors.MenuText)) : SystemBrushes.FromSystemColor(SystemColors.MenuText);
            g.DrawString(text, Font, textBrush, x, e.Bounds.Y + 2, format);
            if (disabled)
                textBrush.Dispose();
            format.Dispose();
        }


        private void OnListBoxMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && this.listBox.SelectedItem != null)
            {
                ToolItem selectedItem = listBox.SelectedItem as ToolItem;

                if (selectedItem == null || selectedItem.ComponentClass == null)
                    return;

                ToolboxItem toolboxItem = ToolBar.GetToolboxItem(selectedItem.ComponentClass);
                IDataObject dataObject = this.SerializeToolboxItem(toolboxItem) as IDataObject;
                DragDropEffects effects = DoDragDrop(dataObject, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        private void OnListBoxClick(object sender, EventArgs eevent)
        {
            ToolItem toolboxItem = listBox.SelectedItem as ToolItem;
            if (toolboxItem != null)
            {
                this.currentSelection = toolboxItem.ComponentClass;
            }
            else if (this.currentSelection != null)
            {
                int index = this.listBox.Items.IndexOf(this.currentSelection);
                if (index >= 0)
                    this.listBox.SelectedIndex = index;
            }
        }
    }
	
}
