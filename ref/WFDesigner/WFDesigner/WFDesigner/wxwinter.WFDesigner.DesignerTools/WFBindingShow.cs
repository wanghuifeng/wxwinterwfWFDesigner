using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;
using System.Reflection;
using System.Web;
using System.IO;
using System.Xml;
using System.Workflow.ComponentModel.Serialization;

namespace wxwinter.WFDesigner.DesignerTools
{
    public static class WFBindingShow
    {
        public static void writeBindPropertyList(object activity, ref List<WFBindPropertyData> bindPropertyDataList)
        {
            if (activity is DependencyObject)
            {

                DependencyObject dObject = activity as DependencyObject;
                List<DependencyProperty> dpList = getDependencyPropertyList(dObject);

                foreach (DependencyProperty dp in dpList)
                {
                    if (dObject.IsBindingSet(dp))
                    {

                        ActivityBind activityBind = dObject.GetBinding(dp);
                        string bindName = activityBind.Name;
                        string bindPath = activityBind.Path;
                        string parentName = "";
                        string name = ((Activity)dObject).Name;
                        if (((Activity)dObject).Parent != null)
                        {
                            parentName = ((Activity)dObject).Parent.Name;
                        }
   
                        WFBindPropertyData bindProperty = new WFBindPropertyData { source = name, sourceProperty = dp.Name, target = bindName, targetProperty = bindPath };

                        bindPropertyDataList.Add(bindProperty);

                    }
                }
            }



            if (activity is System.Workflow.ComponentModel.CompositeActivity)
            {
                System.Workflow.ComponentModel.CompositeActivity wxd;
                wxd = (System.Workflow.ComponentModel.CompositeActivity)activity;


                foreach (object temp in wxd.Activities)
                {
                    writeBindPropertyList(temp, ref  bindPropertyDataList);
                }
            }

        }

        public static string getStateActivityName(Activity activity)
        {
            Activity temp = activity;
            while (temp.Parent !=null)
            {
                temp = temp.Parent;
            }
            return temp.Name;
        }

        public static List<DependencyProperty> getDependencyPropertyList(DependencyObject dObject)
        {
            if (dObject == null)
            {
                return null;
            }
            List<DependencyProperty> listResult = new List<DependencyProperty>();
            List<FieldInfo> fieldInfoList = dObject.GetType().GetFields().ToList();
            foreach (FieldInfo field in fieldInfoList)
            {
                if (field.GetValue(dObject) is DependencyProperty)
                {
                    DependencyProperty dp = field.GetValue(dObject) as DependencyProperty;
                    if (dp != null)
                    {
                        listResult.Add(dp);
                    }
                }
            }
            return listResult;
        }

        public static Activity getActivityByXoml(string xoml)
        {
            xoml = HttpUtility.HtmlDecode(xoml);
            System.Text.UTF8Encoding ut8 = new UTF8Encoding();
            byte[] bs = ut8.GetBytes(xoml);

            System.IO.MemoryStream ms = new MemoryStream(bs);

            XmlReader xmlReader = XmlReader.Create(ms);
            WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
            Activity activity = (Activity)serializer.Deserialize(xmlReader);
            return activity;

        }
    }


    public class WFBindPropertyData
    {


        public string source
        {
            get;
            set;
        }

        public string sourceProperty
        {
            get;
            set;
        }
        public string bindingString
        {
            get { return "§→"; }
        }

        public string targetProperty
        {
            get;
            set;
        }

        public string target
        {
            get;
            set;
        }


    }
}
