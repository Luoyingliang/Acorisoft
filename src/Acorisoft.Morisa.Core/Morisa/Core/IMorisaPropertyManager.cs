using System;
using System.Threading.Tasks;

namespace Acorisoft.Morisa.Core
{
    public interface IMorisaPropertyManager
    {
        void SetProperty(object property);
        Task SetPropertyAsync(object property);
        T GetProperty<T>();
        Task<T> GetPropertyAsync<T>();
        
        /// <summary>
        /// 获取属性流。
        /// </summary>
        public IObservable<MorisaComposeProperty> PropertyStream { get; }
    }
}