using System;
using System.Threading.Tasks;

namespace Acorisoft.Morisa.Core
{
    /// <summary>
    /// <see cref="IMorisaObjectManager"/>
    /// </summary>
    public interface IMorisaObjectManager
    {
        /// <summary>
        /// 获取一个对象。
        /// </summary>
        /// <typeparam name="T">将要获取的对象实例类型。</typeparam>
        /// <returns>返回该类型的对象实例。如果对象不存在则返回 default 。</returns>
        T GetObject<T>() where T : notnull;
        
        /// <summary>
        /// 获取一个对象。
        /// </summary>
        /// <param name="type">要获取的对象类型。</param>
        /// <returns>返回该类型的对象实例。如果对象不存在则返回 default 。</returns>
        object GetObject(Type type);
        
        /// <summary>
        /// 在异步操作中完成获取一个对象的方法。
        /// </summary>
        /// <typeparam name="T">将要获取的对象实例类型。</typeparam>
        /// <returns>返回可等待获取对象操作的任务实例。</returns>
        Task<T> GetObjectAsync<T>() where T : notnull;
        
        /// <summary>
        /// 在异步操作中完成获取一个对象。
        /// </summary>
        /// <param name="type">要获取的对象类型。</param>
        /// <returns>返回可等待获取对象操作的任务实例。</returns>
        Task<object> GetObjectAsync(Type type);
        
        
        /// <summary>
        /// 设置一个对象。
        /// </summary>
        /// <typeparam name="T">要设置的对象类型。</typeparam>
        /// <returns>返回设置的该对象。</returns>
        T SetObject<T>();

        /// <summary>
        /// 设置一个对象。
        /// </summary>
        /// <param name="instance">要设置的类型实例。</param>
        /// <typeparam name="T">要设置的对象类型。</typeparam>
        /// <returns>返回设置的该对象。</returns>
        T SetObject<T>(T instance);

        /// <summary>
        /// 设置一个对象。
        /// </summary>
        /// <param name="factory">返回要设置实例的工厂方法。</param>
        /// <typeparam name="T">要设置的对象类型。</typeparam>
        /// <returns>返回设置的该对象。</returns>
        T SetObject<T>(Func<T> factory);
        
        /// <summary>
        /// 在异步操作中完成设置对象。
        /// </summary>
        /// <typeparam name="T">要设置的对象类型。</typeparam>
        /// <returns>返回可等待设置对象操作的任务实例。</returns>
        Task<T> SetObjectAsync<T>();
        
        /// <summary>
        /// 在异步操作中完成设置对象。
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>返回可等待设置对象操作的任务实例。</returns>
        Task<T> SetObjectAsync<T>(T instance);
        
        /// <summary>
        /// 在异步操作中完成设置对象。
        /// </summary>
        /// <param name="factory">返回要设置实例的工厂方法。</param>
        /// <typeparam name="T">要设置的对象类型。</typeparam>
        /// <returns>返回可等待设置对象操作的任务实例。</returns>
        Task<T> SetObjectAsync<T>(Func<T> factory);
    }
}