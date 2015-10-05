using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;




namespace wxwinter.wf.WFLib
{
 
    public class 状态机模板 : StateMachineWorkflowActivity, ITemplate
    {

     

        string _模板编号="";

     
        public string 模板编号
        {
            get
            {
                if (string.IsNullOrEmpty(_模板编号))
                {
                    return this.Name;
                }
                else
                {
                    return _模板编号;
                }
        
            }
            set
            {
               _模板编号=value;
            }

        }
  
        public static DependencyProperty 模板名称Property = DependencyProperty.Register("模板名称", typeof(string), typeof(状态机模板), new PropertyMetadata(""));
        public string 模板名称
        {
            get
            {
                return ((string)(base.GetValue(状态机模板.模板名称Property)));
            }
            set
            {
                base.SetValue(状态机模板.模板名称Property, value);
            }
        }
   
        public static DependencyProperty 模板说明Property = DependencyProperty.Register("模板说明", typeof(string), typeof(状态机模板), new PropertyMetadata(""));
        public string 模板说明
        {
            get
            {
                return ((string)(base.GetValue(状态机模板.模板说明Property)));
            }
            set
            {
                base.SetValue(状态机模板.模板说明Property, value);
            }
        }

        public static DependencyProperty 扩展数据Property = DependencyProperty.Register("扩展数据", typeof(string), typeof(状态机模板), new PropertyMetadata(""));
        public string 扩展数据
        {
            get
            {
                return ((string)(base.GetValue(状态机模板.扩展数据Property)));
            }
            set
            {
                base.SetValue(状态机模板.扩展数据Property, value);
            }
        }
   
         public string 模板类型
        {
            get
            {
                return "状态机";
            }
          
        }


        private string _模板版本 = "1.0.0.0";

       public string 模板版本
        {
            get
            {
                return _模板版本;
            }
            set
            {
                _模板版本 = value;
            }
        }
  


        public static DependencyProperty 启动窗体Property = DependencyProperty.Register("启动窗体", typeof(string), typeof(状态机模板), new PropertyMetadata(""));

        public string 启动窗体
        {
            get
            {
                return ((string)(base.GetValue(状态机模板.启动窗体Property)));
            }
            set
            {
                base.SetValue(状态机模板.启动窗体Property, value);
            }
        }
 
        public static DependencyProperty 数据表单列表Property = DependencyProperty.Register("数据表单列表", typeof(string), typeof(状态机模板), new PropertyMetadata(""));

        public string 数据表单列表
        {
            get
            {
                return ((string)(base.GetValue(状态机模板.数据表单列表Property)));
            }
            set
            {
                base.SetValue(状态机模板.数据表单列表Property, value);
            }
        }
 
        public static DependencyProperty 启动时填写的表单Property = DependencyProperty.Register("启动时填写的表单", typeof(string), typeof(状态机模板), new PropertyMetadata(""));

        public string 启动时填写的表单
        {
            get
            {
                return ((string)(base.GetValue(状态机模板.启动时填写的表单Property)));
            }
            set
            {
                base.SetValue(状态机模板.启动时填写的表单Property, value);
            }
        }


      

        public static DependencyProperty 业务类型Property = DependencyProperty.Register("业务类型", typeof(string), typeof(状态机模板), new PropertyMetadata(""));
        public string 业务类型
        {
            get
            {
                return ((string)(base.GetValue(状态机模板.业务类型Property)));
            }
            set
            {
                base.SetValue(状态机模板.业务类型Property, value);
            }
        }
      
        public static DependencyProperty 业务编号Property = DependencyProperty.Register("业务编号", typeof(string), typeof(状态机模板), new PropertyMetadata(""));

          public string 业务编号
        {
            get
            {
                return ((string)(base.GetValue(状态机模板.业务编号Property)));
            }
            set
            {
                base.SetValue(状态机模板.业务编号Property, value);
            }
        }
      
        public static DependencyProperty 主业务编号Property = DependencyProperty.Register("主业务编号", typeof(string), typeof(状态机模板), new PropertyMetadata(""));

        public string 主业务编号
        {
            get
            {
                return ((string)(base.GetValue(状态机模板.主业务编号Property)));
            }
            set
            {
                base.SetValue(状态机模板.主业务编号Property, value);
            }
        }
      
        public static DependencyProperty 业务名称Property = DependencyProperty.Register("业务名称", typeof(string), typeof(状态机模板), new PropertyMetadata(""));

       
        public string 业务名称
        {
            get
            {
                return ((string)(base.GetValue(状态机模板.业务名称Property)));
            }
            set
            {
                base.SetValue(状态机模板.业务名称Property, value);
            }
        }
     
        public static DependencyProperty 业务描述Property = DependencyProperty.Register("业务描述", typeof(string), typeof(状态机模板), new PropertyMetadata(""));

     
        public string 业务描述
        {
            get
            {
                return ((string)(base.GetValue(状态机模板.业务描述Property)));
            }
            set
            {
                base.SetValue(状态机模板.业务描述Property, value);
            }
        }
      
        public static DependencyProperty 业务紧急度Property = DependencyProperty.Register("业务紧急度", typeof(string), typeof(状态机模板), new PropertyMetadata(""));

        public string 业务紧急度
        {
            get
            {
                return ((string)(base.GetValue(状态机模板.业务紧急度Property)));
            }
            set
            {
                base.SetValue(状态机模板.业务紧急度Property, value);
            }
        }
     
        public static DependencyProperty 业务保密度Property = DependencyProperty.Register("业务保密度", typeof(string), typeof(状态机模板), new PropertyMetadata(""));

        public string 业务保密度
        {
            get
            {
                return ((string)(base.GetValue(状态机模板.业务保密度Property)));
            }
            set
            {
                base.SetValue(状态机模板.业务保密度Property, value);
            }
        }
     
        public static DependencyProperty 启动人员Property = DependencyProperty.Register("启动人员", typeof(string), typeof(状态机模板), new PropertyMetadata(""));

      
        public string 启动人员
        {
            get
            {
                return ((string)(base.GetValue(状态机模板.启动人员Property)));
            }
            set
            {
                base.SetValue(状态机模板.启动人员Property, value);
            }
        }

      
        public static DependencyProperty 启动部门Property = DependencyProperty.Register("启动部门", typeof(string), typeof(状态机模板), new PropertyMetadata(""));

        public string 启动部门
        {
            get
            {
                return ((string)(base.GetValue(状态机模板.启动部门Property)));
            }
            set
            {
                base.SetValue(状态机模板.启动部门Property, value);
            }
        }
     
        public static DependencyProperty 启动职能Property = DependencyProperty.Register("启动职能", typeof(string), typeof(状态机模板), new PropertyMetadata(""));

      
        public string 启动职能
        {
            get
            {
                return ((string)(base.GetValue(状态机模板.启动职能Property)));
            }
            set
            {
                base.SetValue(状态机模板.启动职能Property, value);
            }
        }
      
        public static DependencyProperty 启动时间Property = DependencyProperty.Register("启动时间", typeof(DateTime), typeof(状态机模板), new PropertyMetadata(new DateTime(1900, 1, 1)));

      
        public DateTime 启动时间
        {
            get
            {
                return ((DateTime)(base.GetValue(状态机模板.启动时间Property)));
            }
            set
            {
                base.SetValue(状态机模板.启动时间Property, value);
            }
        }
 
 
        public static DependencyProperty 流程回归组Property = DependencyProperty.Register("流程回归组", typeof(string), typeof(状态机模板), new PropertyMetadata(""));

        
        public string 流程回归组
        {
            get
            {
                return ((string)(base.GetValue(状态机模板.流程回归组Property)));
            }
            set
            {
                base.SetValue(状态机模板.流程回归组Property, value);
            }
        }
      
        public static DependencyProperty 回归主流程Property = DependencyProperty.Register("回归主流程", typeof(bool), typeof(状态机模板));

     
        public bool 回归主流程
        {
            get
            {
                return ((bool)(base.GetValue(状态机模板.回归主流程Property)));
            }
            set
            {
                base.SetValue(状态机模板.回归主流程Property, value);
            }
        }
    
        public static DependencyProperty 主流程编号Property = DependencyProperty.Register("主流程编号", typeof(Guid), typeof(状态机模板), new PropertyMetadata(Guid.Empty));

      
        public Guid 主流程编号
        {
            get
            {
                return ((Guid)(base.GetValue(状态机模板.主流程编号Property)));
            }
            set
            {
                base.SetValue(状态机模板.主流程编号Property, value);
            }
        }
 

         Guid _流程编号 = Guid.Empty;

        public Guid 流程编号
        {
            get
            {
                return _流程编号;
            }
            set
            {
                _流程编号 = value;
            }

        }
      

        public static DependencyProperty 状态编号Property = DependencyProperty.Register("状态编号", typeof(string), typeof(状态机模板), new PropertyMetadata(""));


        public string 状态编号
        {
            get
            {
                return ((string)(base.GetValue(状态机模板.状态编号Property)));
            }
            set
            {
                base.SetValue(状态机模板.状态编号Property, value);
            }
        }


        public static DependencyProperty 当前节点提交人员Property = DependencyProperty.Register("当前节点提交人员", typeof(string), typeof(状态机模板), new PropertyMetadata(""));
        public string 当前节点提交人员
        {
            get
            {
                return ((string)(base.GetValue(状态机模板.当前节点提交人员Property)));
            }
            set
            {
                base.SetValue(状态机模板.当前节点提交人员Property, value);
            }
        }

        public static DependencyProperty 当前节点提交结果Property = DependencyProperty.Register("当前节点提交结果", typeof(string), typeof(状态机模板), new PropertyMetadata(""));

        public string 当前节点提交结果
        {
            get
            {
                return ((string)(base.GetValue(状态机模板.当前节点提交结果Property)));
            }
            set
            {
                base.SetValue(状态机模板.当前节点提交结果Property, value);
            }
        }
  
       


      
        public static DependencyProperty 测试编号Property = DependencyProperty.Register("测试编号", typeof(int), typeof(状态机模板), new PropertyMetadata(0));

 
        public int 测试编号
        {
            get
            {
                return ((int)(base.GetValue(状态机模板.测试编号Property)));
            }
            set
            {
                base.SetValue(状态机模板.测试编号Property, value);
            }
        }
   
        public static DependencyProperty 状态跟踪器Property = DependencyProperty.Register("状态跟踪器", typeof(int), typeof(状态机模板), new PropertyMetadata(0));


        public int 状态跟踪器
        {
            get
            {
                return ((int)(base.GetValue(状态机模板.状态跟踪器Property)));
            }
            set
            {
                base.SetValue(状态机模板.状态跟踪器Property, value);
            }
        }


        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {


            this.状态跟踪器 = 1;

         
            this.流程编号 = this.WorkflowInstanceId;

      
            return base.Execute(executionContext);
        }




    }
    
  
}
