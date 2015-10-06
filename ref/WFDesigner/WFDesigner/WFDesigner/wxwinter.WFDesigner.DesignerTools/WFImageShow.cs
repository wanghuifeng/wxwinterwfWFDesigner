using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

namespace wxwinter.WFDesigner.DesignerTools
{
    public class WFImageShow
    {
         List<实体> 实体 = new List<实体>();

        List<关系> 关系 = new List<关系>();

        public WFImageShow(string xoml, string xmllayout)
        {


            System.Text.UTF8Encoding ut8 = new UTF8Encoding();
            byte[] bs = ut8.GetBytes(xoml);

            System.IO.MemoryStream ms = new System.IO.MemoryStream(bs);

            XmlReader xmlReader = XmlReader.Create(ms);
            System.Workflow.ComponentModel.Serialization.WorkflowMarkupSerializer serializer = new System.Workflow.ComponentModel.Serialization.WorkflowMarkupSerializer();
            System.Workflow.ComponentModel.Activity activity = (System.Workflow.ComponentModel.Activity)serializer.Deserialize(xmlReader);


            ShowState(activity);
            ShowTargetState(activity);
            ShowLocation(xmllayout);
            ShowHard(activity);
        }

        void ShowState(object activity)
        {
            if (activity is System.Workflow.ComponentModel.CompositeActivity)
            {
                System.Workflow.ComponentModel.CompositeActivity Workflow = activity as System.Workflow.ComponentModel.CompositeActivity;
                foreach (var v in Workflow.EnabledActivities)
                {
                    实体.Add(new 实体() { 类型 = "节", 说明 = v.Description, 名称 = v.Name, X = 0, Y = 0 });
                }
            }
        }

        void ShowTargetState(object activity)
        {
            if (activity is System.Workflow.ComponentModel.CompositeActivity)
            {
                System.Workflow.ComponentModel.CompositeActivity wxd = activity as System.Workflow.ComponentModel.CompositeActivity;

                foreach (object temp in wxd.Activities)
                {
                    ShowTargetState(temp);
                }
            }
            else
            {
                if (activity is System.Workflow.Activities.SetStateActivity)
                {
                    System.Workflow.ComponentModel.Activity at = activity as System.Workflow.ComponentModel.Activity;
                    string s = "";
                    while (true)
                    {
                        at = at.Parent;
                        if (at is System.Workflow.Activities.StateActivity)
                        {
                            s = at.Name;
                            break;
                        }
                    }

                    System.Workflow.Activities.SetStateActivity wxd = activity as System.Workflow.Activities.SetStateActivity;

                    关系.Add(new 关系() { 起点 = s, 目标 = wxd.TargetStateName, 说明 = wxd.Description });
              

                }
            }
        }

        void ShowLocation(string xmllayout)
        {

            System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding();

            byte[] a = utf8.GetBytes(xmllayout);

            System.IO.MemoryStream m1 = new System.IO.MemoryStream(a);

            XmlReader r = XmlReader.Create(m1);



            System.Xml.XmlDocument DOC = new System.Xml.XmlDocument();
            DOC.Load(r);

            System.Xml.XmlNodeList s;

            s = DOC.GetElementsByTagName("StateDesigner", "http://schemas.microsoft.com/winfx/2006/xaml/workflow");


            foreach (System.Xml.XmlNode temp in s)
            {
                System.Xml.XmlElement em = temp as System.Xml.XmlElement;
                string 名称 = em.GetAttribute("Name");
                string Location = em.GetAttribute("Location");
                string[] xy = Location.Split(',');

                var v = 实体.Single(p => p.名称 == 名称);
                v.X = int.Parse(xy[0]);
                v.Y = int.Parse(xy[1]);

            }

        }

        void ShowHard(object activity)
        {
            if (activity is System.Workflow.Activities.StateMachineWorkflowActivity)
            {
                System.Workflow.Activities.StateMachineWorkflowActivity sa = activity as System.Workflow.Activities.StateMachineWorkflowActivity;

                var v1 = 实体.Single(p => p.名称 == sa.InitialStateName);
                v1.类型 = "头";

                var v2 = 实体.Single(p => p.名称 == sa.CompletedStateName);
                v2.类型 = "尾";
            }
        }

        void point(System.Drawing.Bitmap b, int x, int y, System.Drawing.Color c)
        {
            b.SetPixel(x, y, c);

        }

        public System.Drawing.Bitmap Draw(string fileName, string state)
        {
            int w = 实体.Max(p => p.X) + 220;
            int h = 实体.Max(p => p.Y) + 100;

            System.Drawing.Bitmap t = new System.Drawing.Bitmap(w, h);
            System.Drawing.Font f = new System.Drawing.Font("宋体", 12);
            System.Drawing.Font f2 = new System.Drawing.Font("宋体", 10);
            System.Drawing.Graphics pic;
            pic = System.Drawing.Graphics.FromImage(t);
            //=
            System.Random rd = new Random();

            //=
            foreach (var rs in 关系)
            {
                var b = 实体.Single(p => p.名称 == rs.起点);
                var e = 实体.Single(p => p.名称 == rs.目标);
                //  pic.DrawLine(System.Drawing.Pens.Red, b.X + 100, b.Y + 40, e.X + 100, e.Y - 10);

                //===========================

                Rectangle 矩形 = new Rectangle();
                矩形.X = b.X + 180 - 3;
                矩形.Y = b.Y + 70 - 3;
                矩形.Height = 6;
                矩形.Width = 6;



                pic.DrawEllipse(System.Drawing.Pens.Blue, 矩形);


                //===============================

                System.Drawing.Point[] 点s = new System.Drawing.Point[] { new Point(b.X + 180, b.Y + 70), 
                                                                         new Point(b.X + 180, b.Y + 100), 
                                                                         new Point(e.X + 100, e.Y - 100), 
                                                                         new Point(e.X + 100, e.Y - 10) };

                Color 颜色 = new Color();


                颜色 = Color.FromArgb(rd.Next(100, 255), rd.Next(0, 180), rd.Next(0, 180));
                // 颜色 = Color.Black;
                System.Drawing.Pen 笔 = new Pen(颜色);
                笔.Width = 1;	//笔的粗细

                pic.DrawBeziers(笔, 点s);

                // pic.DrawLine(笔, new Point(b.X + 180, b.Y + 70), new Point(e.X + 100, e.Y - 10));



                //====================

                //V
                pic.DrawLine(System.Drawing.Pens.Blue, e.X + 100, e.Y, e.X + 100 + 5, e.Y - 10);
                pic.DrawLine(System.Drawing.Pens.Blue, e.X + 100 - 5, e.Y - 10, e.X + 100 + 5, e.Y - 10);
                pic.DrawLine(System.Drawing.Pens.Blue, e.X + 100, e.Y, e.X + 100 - 5, e.Y - 10);

                //tab
                int t1 = rd.Next(130, 270);
                int t2 = rd.Next(30, 150);
                pic.DrawString(rs.说明, f, new SolidBrush(颜色), new System.Drawing.PointF((e.X + b.X + t1) / 2, (e.Y + b.Y + t2) / 2));

                //pic.DrawString(rs.说明, f, new SolidBrush(颜色), new System.Drawing.PointF((e.X + b.X + 200) / 2, (e.Y + b.Y + 70) / 2));
            }
            //=
            foreach (var v in 实体)
            {


                switch (v.类型)
                {
                    case "节":
                        // pic.FillRectangle(System.Drawing.Brushes.White, v.X, v.Y, 200, 80);
                        pic.DrawRectangle(System.Drawing.Pens.Blue, v.X, v.Y, 200, 80);
                        break;
                    case "头":
                        pic.FillRectangle(System.Drawing.Brushes.YellowGreen, v.X, v.Y, 200, 80);
                        pic.DrawRectangle(System.Drawing.Pens.Blue, v.X, v.Y, 200, 80);
                        break;
                    case "尾":
                        pic.FillRectangle(System.Drawing.Brushes.Silver, v.X, v.Y, 200, 80);
                        pic.DrawRectangle(System.Drawing.Pens.Black, v.X, v.Y, 200, 80);
                        break;
                }
                pic.DrawString(v.名称, f, System.Drawing.Brushes.Blue, new System.Drawing.PointF(v.X + 2, v.Y + 2));

                string s = v.说明;

                int i = 0;
                while (true)
                {
                    if (s.Length > i * 15 + 15)
                    {
                        string ps = s.Substring(i * 15, 15);
                        pic.DrawString(ps, f2, System.Drawing.Brushes.Black, new System.Drawing.PointF(v.X + 2, v.Y + 25 + i * 20));
                        i = i + 1;
                    }
                    else
                    {
                        string ps = s.Substring(i * 15, s.Length - i * 15);
                        pic.DrawString(ps, f2, System.Drawing.Brushes.Black, new System.Drawing.PointF(v.X + 2, v.Y + 25 + i * 20));
                        i = i + 1;
                        break;
                    }

                }

            }

            if (state != "")
            {
                if (实体.Count(p => p.名称 == state) == 1)
                {
                    var at = 实体.Single(p => p.名称 == state);

                    pic.FillEllipse(System.Drawing.Brushes.Red, at.X - 8, at.Y - 8, 16, 16);

                }
            }


            if (fileName != "")
            {
                t.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
            }
            return t;
        }



    }

    class 实体
    {
        public string 类型
        { set; get; }
        public string 名称
        { set; get; }
        public string 说明
        { set; get; }
        public int X
        { set; get; }
        public int Y
        { set; get; }
    }

    class 关系
    {
        public string 起点
        { set; get; }
        public string 目标
        { set; get; }
        public string 说明
        { set; get; }

    }
}
