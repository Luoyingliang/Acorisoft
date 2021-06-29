namespace Acorisoft.Morisa
{
    /// <summary>
    /// <see cref="IMorisaCompose"/> 表示一个创作。
    /// </summary>
    public interface IMorisaCompose
    {
        /// <summary>
        /// 构建创作集目录结构
        /// </summary>
        void BuildHierarchy();
    }
}