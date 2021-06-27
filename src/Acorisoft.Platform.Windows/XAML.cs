using System;

namespace Acorisoft.Platform
{
    /// <summary>
    /// <see cref="Xaml"/> 类型为 <see cref="Windows Presentation Foundation"/> 框架提供更便利的操作方法。
    /// </summary>
    public static class Xaml
    {
        //--------------------------------------------------------------------------------------------------------------
        //
        // Avoid Boxing Methods
        //
        //--------------------------------------------------------------------------------------------------------------
        
        #region Avoid Boxing

        private static readonly object True = true;
        private static readonly object False = false;

        /// <summary>
        /// 获取一个已装箱的布尔实例。
        /// </summary>
        /// <param name="condition">要返回的布尔值。</param>
        /// <returns>返回一个已装箱的布尔实例。</returns>
        public static object Box(bool condition) => condition ? True : False;

        #endregion
        
        //--------------------------------------------------------------------------------------------------------------
        //
        // Visual Tree Operation Methods
        //
        //--------------------------------------------------------------------------------------------------------------

        #region Visual Tree Operation Methods


        #endregion
    }
}
