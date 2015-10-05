using System;
namespace wxwinter.wf.WFDesigner
{
  public  interface IDesigner
    {
        void AddActivity(ActivityControl activity);
        void AddActivityPath(string pathName, string startingPoint, string targetingPoint, string condition, string describe);
        System.Collections.Generic.List<ActivityControl> GetActivityControlList();
        System.Collections.Generic.List<ActivityPath> GetActivityPathList();
        System.Collections.Generic.List<string> NameList { get; }
        double PageHeight { get; set; }
        double PageSize { get; set; }
        double PageWidth { get; set; }
        void RemoveActivity(string activityName);
        void RemoveActivityPath(string pathName);
        void SetCurrentActivity(string activityName);
        void Clear(bool isCreate);

        FlowChartData 流程数据 { set; get; }
    }
}
