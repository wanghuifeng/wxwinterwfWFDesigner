using System;
using System.Windows.Controls;
using System.Collections.Generic;
namespace wxwinter.wf.WFDesigner
{
    public class ActivityControl : UserControl, IActivity
    {
        public IDesigner 设计器
        { set; get; }
  
        public virtual  string 标题
        {
            set;
           
            get;
            
        }
       
        public virtual string 说明
        {
            set;

            get;
  
        }
        
        public Double X坐标
        {
            set { Canvas.SetLeft(this, value); }
            get { return Canvas.GetLeft(this); }
        }
       
        public Double Y坐标
        {
            set { Canvas.SetTop(this, value); }
            get { return Canvas.GetTop(this); }
        }

        public string 类型
        { set; get; }

        public object 结点数据
        { set; get; }

        List<string> _分支集合 = new List<string>();

        public List<string> 分支集合
        {
            get { return _分支集合; }
            set { _分支集合 = value; }
        }

        public void 删除()
        {

            设计器.RemoveActivity(this.Name);

        }

        public virtual  void 设为活动()
        {
            
        }

        public event EventHandler 刷新事件 = null;

        public event EventHandler 连接事件 = null;

        protected void On刷新事件()
        {
            if (刷新事件 != null)
            {
                刷新事件(this, EventArgs.Empty);
            }
        }

        protected void On连接事件(string s)
        {
            if (连接事件 != null)
            {
                string[] os = s.Split(',');
                连接事件(this, new LinkEventArgs() { 起点 = os[0] , 路由条件=os[1]});
            }
        }

        public virtual  void 显示分支选项()
        {
        }
    }

    public class LinkEventArgs : EventArgs
    {
        public string 起点
        {
            set;
            get;
        }

        public string 路由条件
        { set; get; }
    }
}
