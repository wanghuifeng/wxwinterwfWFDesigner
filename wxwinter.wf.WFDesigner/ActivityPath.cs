using System.Windows.Shapes;
using System.Windows.Controls;
public class ActivityPath
{
    public string Name
    {
        get { return 连线.Name; }
    }

    public string 起点
    { set; get; }
    
    public string 目标
    { set; get; }
  
    public string 说明
    { set; get; }

    public string 路由
    { set; get; }

    public Path   连线
    { set; get; }

    public TextBlock 标签
    { set; get; }


}