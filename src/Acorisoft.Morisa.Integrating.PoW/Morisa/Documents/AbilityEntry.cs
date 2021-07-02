namespace Acorisoft.Morisa.Documents
{
    /// <summary>
    /// <see cref="AbilityEntry"/> 表示一个词条
    /// </summary>
    public class AbilityEntry
    {
        internal const string  NameMoniker = "n";
        internal const string  DescriptionMoniker = "d";
        internal const string  CostMoniker = "c";
        internal const string  MotionMoniker = "m";
        internal const string  SubjectivityMoniker = "s";
            
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 获取或设置能力词条的能力描述。
        /// </summary>
        public string Description { get; set; }        
        
        /// <summary>
        /// 动作描述
        /// </summary>
        public string Motion { get; set; }
        
        /// <summary>
        /// 主观描述
        /// </summary>
        public string Subjectivity { get; set; }
    }
}