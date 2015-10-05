using System;
namespace wxwinter.wf.WFDesigner
{
  public  interface ILoder
    {
      void LoadFlow<T>(T obj, IDesigner designer);
      T GetFlow<T>(IDesigner designer);
    }
}
