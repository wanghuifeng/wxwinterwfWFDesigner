using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;


using System.Collections.Generic;

using System.IO;
using System.Xml;

using System.Data;
using System.Text;

namespace wxwinter.wf.WFLib
{
  
    public class 等待提交 : System.Workflow.Activities.HandleExternalEventActivity, IWaitSubmitTemplate
	{
	  public 等待提交()
		{
			InitializeComponent();
		}

    
      ITemplate Root()
        {
            Activity o = this.Parent;

            while (o.Parent != null)
            {
                o = o.Parent;
            }
            ITemplate tp = o as ITemplate;

            if (tp != null)
            {
                return tp;
            }
            else
            {
                throw new System.Exception("GetRoot");
            }

        }


 
      [System.Diagnostics.DebuggerNonUserCode]
	   private void InitializeComponent()
		{


    
           this.EventName = "submitEvent";
            
           this.InterfaceType = typeof(wxwinter.wf.WFLib.IExternalEvent);

           this.Invoked += new EventHandler<ExternalDataEventArgs>(EndNotify);
            
  		}

  
       protected override void OnActivityExecutionContextLoad(IServiceProvider provider)
       {

          
        
           base.OnActivityExecutionContextLoad(provider);

            if (Root().状态跟踪器 == 1)
           {

               this.状态实例编号 = Guid.NewGuid();

                          List<string> ls = provider.GetService(typeof(List<string>)) as List<string>;
                   if (ls != null)
                   {
                       ls.Add("--------------------------等待提交-----------------------------------");

                       ls.Add("状态编号:" + this.状态编号.ToString());
                       ls.Add("状态名称:" + this.状态名称.ToString());
                       ls.Add("状态说明:" + this.状态说明.ToString());
                       ls.Add("办理人员:" + this.办理人员.ToString());
                       ls.Add("接件部门:" + this.接件部门.ToString());
                       ls.Add("接件职能:" + this.接件职能.ToString());
                       ls.Add("办理查看业务表单:" + this.办理查看业务表单.ToString());
                       ls.Add("办理添写业务表单:" + this.办理添写业务表单.ToString());
                      
                       ls.Add("办理提交选项:" + this.办理提交选项.ToString());
                      
                   }

                Root().状态跟踪器 = 0;
           }

       }

 
       void EndNotify(object sender, ExternalDataEventArgs e)
       {
           SubmitEventArgs input = e as SubmitEventArgs;


        
           this.提交结果 = input.提交结果;
           this.提交人员 = input.提交人员;
           this.提交部门 = input.提交部门;
           this.提交职能 = input.提交职能;
           this.提交日期 = System.DateTime.Now;
           this.提交方式 = input.提交方式;
           this.提交说明 = input.提交说明;
           this.触发器类型 = input.触发器类型;

          
           this.下一状态办理人员 = input.下一状态办理人员;

    
           Root().当前节点提交结果 = input.提交结果;
           Root().当前节点提交人员 = input.提交人员;
           Root().状态编号 = input.状态实例编号.ToString();
           Root().状态跟踪器 = 1;
 
          
       }


       protected override void OnClosed(IServiceProvider provider)
       {

               List<string> ls = provider.GetService(typeof(List<string>)) as List<string>;

               ls.Add("--------------------------提交-----------------------------------");
               ls.Add("流程编号:" + Root().流程编号.ToString());
               ls.Add("状态实例编号:" + this.状态实例编号);
               ls.Add("状态名称:" + this.状态名称);
               ls.Add("提交部门:" + this.提交部门);
               ls.Add("提交职能:" + this.提交职能);
               ls.Add("提交人员:" + this.提交人员);
               ls.Add("提交结果:" + this.提交结果);
               ls.Add("");

           base.OnClosed(provider);
       }




                public string 状态编号
                {
                    get
                    {
                        return this.Name;
                    }

                }
 
                public static DependencyProperty 状态名称Property = DependencyProperty.Register("状态名称", typeof(string), typeof(等待提交), new PropertyMetadata(""));
                public string 状态名称
                {
                    get
                    {
                        return ((string)(base.GetValue(等待提交.状态名称Property)));
                    }
                    set
                    {
                        base.SetValue(等待提交.状态名称Property, value);
                    }
                }
   
                public static DependencyProperty 状态说明Property = DependencyProperty.Register("状态说明", typeof(string), typeof(等待提交), new PropertyMetadata(""));
                public string 状态说明
                {
                    get
                    {
                        return ((string)(base.GetValue(等待提交.状态说明Property)));
                    }
                    set
                    {
                        base.SetValue(等待提交.状态说明Property, value);
                    }
                }

                private string _处理方式 = "";
                public string 处理方式
                {
                    get
                    {
                        return _处理方式;
                    }
                    set
                    {
                        _处理方式 = value;
                    }
                }
 
   

                public static DependencyProperty 启动窗体Property = DependencyProperty.Register("启动窗体", typeof(string), typeof(等待提交), new PropertyMetadata(""));
                public string 启动窗体
                {
                    get
                    {
                        return ((string)(base.GetValue(等待提交.启动窗体Property)));
                    }
                    set
                    {
                        base.SetValue(等待提交.启动窗体Property, value);
                    }
                }
            

  
                public static DependencyProperty 办理人员Property = DependencyProperty.Register("办理人员", typeof(string), typeof(等待提交), new PropertyMetadata(""));
                public string 办理人员
                {
                    get
                    {
                        return ((string)(base.GetValue(等待提交.办理人员Property)));
                    }
                    set
                    {
                        base.SetValue(等待提交.办理人员Property, value);
                    }
                }
   
                public static DependencyProperty 办理时限Property = DependencyProperty.Register("办理时限", typeof(int), typeof(等待提交), new PropertyMetadata(0));
                public int 办理时限
                {
                    get
                    {
                        return ((int)(base.GetValue(等待提交.办理时限Property)));
                    }
                    set
                    {
                        base.SetValue(等待提交.办理时限Property, value);
                    }
                }

                private string _办理提交选项 = "提交";
                public string 办理提交选项
                {
                    get
                    {
                        return _办理提交选项;
                    }
                    set
                    {
                        _办理提交选项 = value;
                    }
                }
    
                public static DependencyProperty 办理查看业务表单Property = DependencyProperty.Register("办理查看业务表单", typeof(string), typeof(等待提交), new PropertyMetadata(""));
                public string 办理查看业务表单
                {
                    get
                    {
                        return ((string)(base.GetValue(等待提交.办理查看业务表单Property)));
                    }
                    set
                    {
                        base.SetValue(等待提交.办理查看业务表单Property, value);
                    }
                }
   
                public static DependencyProperty 办理添写业务表单Property = DependencyProperty.Register("办理添写业务表单", typeof(string), typeof(等待提交), new PropertyMetadata(""));
                public string 办理添写业务表单
                {
                    get
                    {
                        return ((string)(base.GetValue(等待提交.办理添写业务表单Property)));
                    }
                    set
                    {
                        base.SetValue(等待提交.办理添写业务表单Property, value);
                    }
                }
          
                public static DependencyProperty 接件部门Property = DependencyProperty.Register("接件部门", typeof(string), typeof(等待提交), new PropertyMetadata(""));
                public string 接件部门
                {
                    get
                    {
                        return ((string)(base.GetValue(等待提交.接件部门Property)));
                    }
                    set
                    {
                        base.SetValue(等待提交.接件部门Property, value);
                    }
                }
    
                public static DependencyProperty 接件职能Property = DependencyProperty.Register("接件职能", typeof(string), typeof(等待提交), new PropertyMetadata(""));
                public string 接件职能
                {
                    get
                    {
                        return ((string)(base.GetValue(等待提交.接件职能Property)));
                    }
                    set
                    {
                        base.SetValue(等待提交.接件职能Property, value);
                    }
                }
        
                public static DependencyProperty 提交次数Property = DependencyProperty.Register("提交次数", typeof(int), typeof(等待提交), new PropertyMetadata(0));
                public int 提交次数
                {
                    get
                    {
                        return ((int)(base.GetValue(等待提交.提交次数Property)));
                    }
                    set
                    {
                        base.SetValue(等待提交.提交次数Property, value);
                    }
                }

  
                public static DependencyProperty 提交结果Property = DependencyProperty.Register("提交结果", typeof(string), typeof(等待提交), new PropertyMetadata(""));
                public string 提交结果
                {
                    get
                    {
                        return ((string)(base.GetValue(等待提交.提交结果Property)));
                    }
                    set
                    {
                        base.SetValue(等待提交.提交结果Property, value);
                    }
                }

                public static DependencyProperty 提交人员Property = DependencyProperty.Register("提交人员", typeof(string), typeof(等待提交), new PropertyMetadata(""));
                public string 提交人员
                {
                    get
                    {
                        return ((string)(base.GetValue(等待提交.提交人员Property)));
                    }
                    set
                    {
                        base.SetValue(等待提交.提交人员Property, value);
                    }
                }
                public static DependencyProperty 提交部门Property = DependencyProperty.Register("提交部门", typeof(string), typeof(等待提交), new PropertyMetadata(""));
                public string 提交部门
                {
                    get
                    {
                        return ((string)(base.GetValue(等待提交.提交部门Property)));
                    }
                    set
                    {
                        base.SetValue(等待提交.提交部门Property, value);
                    }
                }
                public static DependencyProperty 提交职能Property = DependencyProperty.Register("提交职能", typeof(string), typeof(等待提交), new PropertyMetadata(""));
                public string 提交职能
                {
                    get
                    {
                        return ((string)(base.GetValue(等待提交.提交职能Property)));
                    }
                    set
                    {
                        base.SetValue(等待提交.提交职能Property, value);
                    }
                }
                public static DependencyProperty 提交日期Property = DependencyProperty.Register("提交日期", typeof(DateTime), typeof(等待提交), new PropertyMetadata(new DateTime(1900, 1, 1)));
                public DateTime 提交日期
                {
                    get
                    {
                        return ((DateTime)(base.GetValue(等待提交.提交日期Property)));
                    }
                    set
                    {
                        base.SetValue(等待提交.提交日期Property, value);
                    }
                }

                private string _提交方式 = "";
                public string 提交方式
                {
                    get
                    {
                        return _提交方式;
                    }
                    set
                    {
                        _提交方式 = value;
                    }
                }

                public static DependencyProperty 提交说明Property = DependencyProperty.Register("提交说明", typeof(string), typeof(等待提交), new PropertyMetadata(""));
                public string 提交说明
                {
                    get
                    {
                        return ((string)(base.GetValue(等待提交.提交说明Property)));
                    }
                    set
                    {
                        base.SetValue(等待提交.提交说明Property, value);
                    }
                }

                public static DependencyProperty 触发器类型Property = DependencyProperty.Register("触发器类型", typeof(string), typeof(等待提交), new PropertyMetadata(""));
                public string 触发器类型
                {
                    get
                    {
                        return ((string)(base.GetValue(等待提交.触发器类型Property)));
                    }
                    set
                    {
                        base.SetValue(等待提交.触发器类型Property, value);
                    }
                }

                public static DependencyProperty 下一状态办理人员Property = DependencyProperty.Register("下一状态办理人员", typeof(string), typeof(等待提交), new PropertyMetadata(""));
                public string 下一状态办理人员
                {
                    get
                    {
                        return ((string)(base.GetValue(等待提交.下一状态办理人员Property)));
                    }
                    set
                    {
                        base.SetValue(等待提交.下一状态办理人员Property, value);
                    }
                }

                public static DependencyProperty 状态实例编号Property = DependencyProperty.Register("状态实例编号", typeof(Guid), typeof(等待提交), new PropertyMetadata(Guid.Empty));

                public Guid 状态实例编号
                {
                    get
                    {
                        return ((Guid)(base.GetValue(等待提交.状态实例编号Property)));
                    }
                    set
                    {
                        base.SetValue(等待提交.状态实例编号Property, value);
                    }
                }
  


                public static DependencyProperty 状态跟踪器Property = DependencyProperty.Register("状态跟踪器", typeof(int), typeof(等待提交), new PropertyMetadata(0));

                public int 状态跟踪器
                {
                    get
                    {
                        return ((int)(base.GetValue(等待提交.状态跟踪器Property)));
                    }
                    set
                    {
                        base.SetValue(等待提交.状态跟踪器Property, value);
                    }
                }

       
	}


}
