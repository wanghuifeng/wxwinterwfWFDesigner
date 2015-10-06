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
    public class ToolItem
    {
        private string componentClassName;
        private Type componentClass;
        private string name = null;
        private string className = null;
        private Image glyph = null;

        public ToolItem(string componentClassName)
        {
            this.componentClassName = componentClassName;
        }

        public string ClassName
        {
            get
            {
                if (className == null)
                {
                    className = ComponentClass.FullName;
                }

                return className;
            }
        }

        public Type ComponentClass
        {
            get
            {
                if (componentClass == null)
                {
                    componentClass = Type.GetType(componentClassName);
                    if (componentClass == null)
                    {
                        int index = componentClassName.IndexOf(",");
                        if (index >= 0)
                            componentClassName = componentClassName.Substring(0, index);

                        foreach (AssemblyName referencedAssemblyName in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
                        {
                            Assembly assembly = Assembly.Load(referencedAssemblyName);
                            if (assembly != null)
                            {
                                componentClass = assembly.GetType(componentClassName);
                                if (componentClass != null)
                                    break;
                            }
                        }

                   
                        componentClass = typeof(SequentialWorkflowActivity).Assembly.GetType(componentClassName);
                    }
                }

                return componentClass;
            }
        }

        public string Name
        {
            get
            {
                if (name == null)
                {
                    if (ComponentClass != null)
                    {
                        switch (ComponentClass.Name)
                        {
           
                            case "StateInitializationActivity":
                                name = "状态-开始容器";
                                break;
                            case "EventDrivenActivity":
                                name = "等待外部触发触发容器";
                                break;
                            case "StateActivity":
                                name = "状态";
                                break;
                            case "SetStateActivity":
                                name = "状态-跳转";
                                break;
                            case "StateFinalizationActivity":
                                name = "状态-完成容器";
                                break;
                            case "DelayActivity":
                                name = "计时器";
                                break;
                            case "IfElseActivity":
                                name = "条件";
                                break;
                            case "ListenActivity":
                                name = "监听";
                                break;
                            case "ParallelActivity":
                                name = "并行";
                                break;
                            case "SequenceActivity":
                                name = "顺序";
                                break;
                            case "PolicyActivity":
                                name = "策略";
                                break;
                            case "WhileActivity":
                                name = "循环";
                                break;
                            case "CallExternalMethodActivity":
                                name = "调用外部方法";
                                break;
                            case "TerminateActivity":
                                name = "终止";
                                break;
                            case "SuspendActivity":
                                name = "暂停";
                                break;

                            case "HandleExternalEventActivity":
                                name = "等待外部触发";
                                break;

                                
                            default:
                                name = ComponentClass.Name;
                                break;

                        }
                       
                       
                    }
                    else
                    { name = "wxwinter"; }
                }

                return name;
            }
        }

        public virtual Image Glyph
        {
            get
            {
                if (glyph == null)
                {
                    Type t = ComponentClass;

                    if (t == null)
                        t = typeof(Component);

                    ToolboxBitmapAttribute attr = (ToolboxBitmapAttribute)TypeDescriptor.GetAttributes(t)[typeof(ToolboxBitmapAttribute)];

                    if (attr != null)
                        glyph = attr.GetImage(t, false);
                }
                return glyph;
            }
        }

        public override string ToString()
        {
            return componentClassName;
        }
    }
}
