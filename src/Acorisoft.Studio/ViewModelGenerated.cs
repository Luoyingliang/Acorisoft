using Acorisoft.Platform.Windows.Services;
using DryIoc;

namespace Acorisoft.Studio
{
    /// <summary>
    /// ViewModelGenerator 在编译时自动生成的模块
    /// </summary>
    public partial class ViewModelGenerated
    {

        public static void Initialize()
        {
            //
            // 激活静态构造函数，完成视图模型注册
            Version++;
        }

        internal static int Version { get; private set; }
    }
      
}