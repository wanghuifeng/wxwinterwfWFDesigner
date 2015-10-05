using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wxwinter.wf.WFDesigner
{
 
    public partial class DesignerControl : UserControl, IDesigner
    {
        public DesignerControl()
        {
            InitializeComponent();

        }

        Random random = new Random();

        int link_n = 0;
        
        int activity_n = 0;

        public FlowChartData 流程数据
        { set; get; }
        
        List<ActivityControl> ActivityControlList = new List<ActivityControl>();

        List<ActivityPath> ActivityPathList = new List<ActivityPath>();

        void pt_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Path p = sender as Path;
            var v = System.Windows.MessageBox.Show("删除所选连线?", "", MessageBoxButton.YesNo);
            if (v == MessageBoxResult.Yes)
            {
                RemoveActivityPath(p.Name);
            }
        }

        void pt_MouseLeave(object sender, MouseEventArgs e)
        {
            Path p = sender as Path;
            p.Stroke = System.Windows.Media.Brushes.Black;
            p.StrokeThickness = 1;
        }

        void pt_MouseEnter(object sender, MouseEventArgs e)
        {
            Path p = sender as Path;
            p.StrokeThickness = 2;
            p.Stroke = System.Windows.Media.Brushes.Red;
        }

        void activity_连接事件(object sender, EventArgs e)
        {
            ActivityControl act = sender as ActivityControl;
            LinkEventArgs c = e as LinkEventArgs;

            AddActivityPath("", c.起点, act.Name, c.路由条件, "");
        }

        void activity_刷新事件(object sender, EventArgs e)
        {
            RefurbishActivityPath();
        }

        void DrawActivityPath(ActivityControl startingActivity, ActivityControl targetingActivity, Path pathObject, ActivityPath activityPath)
        {

            int i = startingActivity.分支集合.FindIndex(p => p == activityPath.路由);
            int l = 0;
            for (int n = 0; n <= i; n++)
            {
                l = startingActivity.分支集合[n].Length + l;
            }
            //double x1 = Canvas.GetLeft(起点) + 起点.Width - 15;

            double x1=0;
            double y1=0;

            switch (activityPath.路由)
            {

                case "开始状态":
                    x1 = Canvas.GetLeft(startingActivity) + startingActivity.Width / 2;
                    y1 = Canvas.GetTop(startingActivity) + startingActivity.Height;
                    break;
   
                case "回归":
                    x1 = Canvas.GetLeft(startingActivity) + startingActivity.Width / 2;
                    y1 = Canvas.GetTop(startingActivity) + startingActivity.Height;
                    break;
                default:
                    x1 = Canvas.GetLeft(startingActivity) + 6 + (l * 11) + i * 5;
                    y1 = Canvas.GetTop(startingActivity) + startingActivity.Height - 10;
                    break;
            }





            double x4 = Canvas.GetLeft(targetingActivity) + 20;
            double y4 = Canvas.GetTop(targetingActivity);

            double x2 = x1;
            double y2 = y1 + 100;

            double x3 = x4;
            double y3 = y4 - 100;

            PathFigure 形状子部分 = new PathFigure();
            PathSegmentCollection 串线 = new PathSegmentCollection();
            PathFigureCollection 形状集合 = new PathFigureCollection();
            PathGeometry 形状 = new PathGeometry();
            形状子部分.StartPoint = new Point(x1, y1);
            形状子部分.Segments = 串线;
            形状集合.Add(形状子部分);
            形状.Figures = 形状集合;



            pathObject.Data = 形状;
            pathObject.Stroke = System.Windows.Media.Brushes.Black;
            pathObject.StrokeThickness = 1;



            //BezierSegment b = new BezierSegment();

            //b.Point1 = new Point(x2, y2);
            //b.Point2 = new Point(x3, y3);
            //b.Point3 = new Point(x4, y4);

            LineSegment b = new LineSegment();
            b.Point = new Point(x4, y4);

            串线.Add(b);
            串线.Add(new LineSegment() { Point = new Point() { Y = y4 - 10, X = x4 + 5 } });
            串线.Add(new LineSegment() { Point = new Point() { Y = y4 - 10, X = x4 + 4 } });
            串线.Add(new LineSegment() { Point = new Point() { Y = y4, X = x4 } });
            串线.Add(new LineSegment() { Point = new Point() { Y = y4 - 10, X = x4 - 5 } });
            串线.Add(new LineSegment() { Point = new Point() { Y = y4 - 10, X = x4 - 6 } });






            Canvas.SetLeft(activityPath.标签, x1 + (x4 - x1) / 2);
            Canvas.SetTop(activityPath.标签, y4 - (y4 - y1) / 2);


        }

        void RefurbishActivityPath()
        {

            foreach (var v in ActivityPathList)
            {
                if (ActivityControlList.Count(p => p.Name == v.起点) == 1 && ActivityControlList.Count(p => p.Name == v.目标) == 1)
                {
                    var s = ActivityControlList.Single(p => p.Name == v.起点);
                    var t = ActivityControlList.Single(p => p.Name == v.目标);
                    DrawActivityPath(s, t, v.连线, v);
                }





            }
        }

        public double PageSize
        {
            set
            {
                scaleTransform.ScaleX = value;
                scaleTransform.ScaleY = value;
            }
            get
            {
                return scaleTransform.ScaleX;
            }
        }

        public double PageHeight
        {
            set
            {
                this.myDesigner.Height = value;
            }
            get
            {
                return this.myDesigner.Height;
            }
        }

        public double PageWidth
        {
            set
            {
                this.myDesigner.Width = value;
            }
            get
            {
                return this.myDesigner.Width;
            }
        }

        public List<string> NameList
        { 
            get
            {
                var v = myDesigner.Children.Cast<FrameworkElement>().Select(p => p.Name).ToList();
                return v;
            }
        }

        public void AddActivity(ActivityControl activity)
        {
            if (string.IsNullOrEmpty(activity.Name))
            {
                activity_n = activity_n + 1;
                activity.Name = "activity" + activity_n.ToString();

            }

            if (NameList.Exists(p => p == activity.Name))
            {
                return;
            }

 
            this.myDesigner.Children.Add(activity);
            ActivityControlList.Add(activity);
            activity.刷新事件 += new EventHandler(activity_刷新事件);
            activity.连接事件 += new EventHandler(activity_连接事件);
            activity.设计器 = this;
            activity.显示分支选项();
            RefurbishActivityPath();
        }

        public void RemoveActivity(string activityName)
        {
            var v = ActivityControlList.Single(p => p.Name == activityName);
            this.myDesigner.Children.Remove(v);
            ActivityControlList.Remove(v);


           var pt=  ActivityPathList.Where (p => p.起点 == activityName || p.目标 == activityName).Select(p=>p.Name).ToList();
           foreach (var tp in pt)
           {
               RemoveActivityPath(tp);
           }
           
            RefurbishActivityPath();

        }

        public void AddActivityPath(string pathName, string startingPoint, string targetingPoint,string condition, string describe)
        {
            var v = ActivityPathList.Count(p => p.起点 == startingPoint && p.路由 == condition);

            if (v != 0)
            {
                MessageBox.Show("每个[起点]的[路由条件]只能指向一个[目标],如果要重新指定,请删除原指向");
                return;
            }

            if (startingPoint == targetingPoint)
            {
                return;
            }

            if (string.IsNullOrEmpty(pathName))
            {
                link_n = link_n + 1;
                pathName = "link" + link_n.ToString();
            }

            if (NameList.Exists(p => p == pathName))
            {
                return;
            }

            Path pt = new Path();
            pt.MouseEnter += new MouseEventHandler(pt_MouseEnter);
            pt.MouseLeave += new MouseEventHandler(pt_MouseLeave);
            pt.MouseRightButtonUp += new MouseButtonEventHandler(pt_MouseRightButtonUp);
            pt.Name = pathName;
            ActivityPath ptr = new ActivityPath();
            ptr.连线 = pt;
            ptr.说明 = describe;
            ptr.目标 = targetingPoint;
            ptr.起点 = startingPoint;
            ptr.路由 = condition;
            ptr.标签 = new TextBlock() { Text = condition, Name = "tb_" + pathName };
            myDesigner.Children.Add(ptr.标签);
            this.myDesigner.Children.Add(pt);
            ActivityPathList.Add(ptr);



            RefurbishActivityPath();
        }

        public void RemoveActivityPath(string pathName)
        {
            var prt = ActivityPathList.Single(p => p.连线.Name == pathName);
            this.myDesigner.Children.Remove(prt.连线);
            this.myDesigner.Children.Remove(prt.标签);
            ActivityPathList.Remove(prt);

            if (prt.路由 == "开始状态")
            {
                var b = this.ActivityControlList.Single(p => p.Name == "开始状态");
                b.分支集合.Clear();
            }

            RefurbishActivityPath();
        }

        public void SetCurrentActivity(string activityName)
        {
            ActivityControlList.Single(p => p.Name == activityName).设为活动();
        }

        public List<ActivityPath> GetActivityPathList()
        {
            return ActivityPathList;
        }

        public List<ActivityControl> GetActivityControlList()
        {
            return ActivityControlList;
        }


        public void Clear(bool isCreate)
        {
            ActivityControlList.Clear();
            ActivityPathList.Clear();
            this.myDesigner.Children.Clear();
            this.流程数据 = new FlowChartData() { 模板版本 = "1.0.0.0", 数据表单列表 = "标准空文档", 启动时填写的表单 = "标准空文档.文档" };
            if (isCreate)
            {
                AddActivity(new BeginControl() { Name = "开始状态", 类型 = "头" });
                AddActivity(new EndControl() { Name = "归档状态", 类型 = "尾" });
            }
        }
    }




   
}
