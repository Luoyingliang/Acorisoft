using System;
using System.IO;

namespace Acorisoft
{
    public static class ThrowHelper
    {
        public static void ThrowArgumentNull(string paramName)
        {
            throw new ArgumentNullException($"{paramName}:参数为空");
        }

        public static void ThrowArgumentError()
        {
            throw new ArgumentException("参数存在异常");
        }

        public static void ThrowInvalidOperation()
        {
            throw new InvalidOperationException("无效的操作");
        }
        
        public static void ThrowFileNotFound(string paramName)
        {
            throw new FileNotFoundException($"无法找到文件：{paramName}");
        }
    }
}