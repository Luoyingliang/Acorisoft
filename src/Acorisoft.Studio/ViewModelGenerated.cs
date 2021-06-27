using DryIoc;

namespace Acorisoft.Studio
{
    /// <summary>
    /// ViewModelGenerator 在编译时自动生成的模块
    /// </summary>
    public partial class ViewModelGenerated
    {
        /// <summary>
        /// 调用该方法可以实现注册程序中所有的视图模型，并且实现视图模型与视图之间的关联。
        /// </summary>
        /// <param name="container">Ioc容器</param>
        public static partial void RegisterViewModelsAndViews(IContainer container);
    }
      
}