// ReSharper disable InconsistentNaming
namespace Acorisoft.Morisa
{
    
    public enum ResourcePermission : int
    {
        #region Version1
        
        V1_None,
        V1_FullControl,
        V1_ReadOnly,
        
        #endregion
        
        Denied,
        ReadOnly,
        FullControl,
    }
}