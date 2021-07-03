using System;
using Acorisoft.Morisa.Internals;
using Acorisoft.Morisa.IO;

namespace Acorisoft.Morisa.Core
{
    public interface ICompose : IDisposable
    {
        /// <summary>
        /// 创建文件系统结构
        /// </summary>
        void BuildHierarchy();

        /// <summary>
        /// 
        /// </summary>
        string Directory { get; }

        /// <summary>
        /// 
        /// </summary>
        string VideoDirectory { get; }
    
        
        /// <summary>
        /// 
        /// </summary>
        string ImageDirectory { get; }
        
        /// <summary>
        /// 
        /// </summary>
        string AutoSaveDirectory  { get; }
        
        /// <summary>
        /// 
        /// </summary>
        string FileDirectory { get; }
        
        /// <summary>
        /// 0
        /// </summary>
        string BrushesDirectory  { get; }
        
        /// <summary>
        /// 
        /// </summary>
        string ThumbnailDirectory  { get; }
    }
}