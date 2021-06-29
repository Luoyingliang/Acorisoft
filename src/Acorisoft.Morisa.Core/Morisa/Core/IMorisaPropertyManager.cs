using System;
using System.Threading.Tasks;

namespace Acorisoft.Morisa.Core
{
    public interface IMorisaPropertyManager
    {
        /// <summary>
        /// 获取属性流。
        /// </summary>
        public IObservable<MorisaComposeProperty> PropertyStream { get; }
    }
}