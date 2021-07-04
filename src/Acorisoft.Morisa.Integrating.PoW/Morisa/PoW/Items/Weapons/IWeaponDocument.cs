namespace Acorisoft.Morisa.PoW.Items.Weapons
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWeaponDocument : IValuableItemDocument
    {
        /// <summary>
        /// 获取或设置武器的分类
        /// </summary>
        Category Category { get; set; }
    }
}