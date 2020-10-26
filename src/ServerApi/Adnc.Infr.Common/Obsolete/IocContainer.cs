/*
 如果想从容器中获取对象，我们都是通过构造方法获取对象，但有些条件不允许不能通过构造方法获取对象
，我们必须单独从容器中单独创建获取找个对象，这样我们就不行把找个容器静态保存起来供全局使用
*/
//using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Adnc.Common.Obsolete
{
    //public static class ServiceCollectionExtensions
    //{
    //    public static T GetServiceFromCollection<T>(this IServiceCollection services)
    //    {
    //        var con = services.LastOrDefault(p => p.ServiceType == typeof(T)).ImplementationFactory;
    //        return (T)con.Invoke(null);
    //    }
    //}
    [Obsolete("设计不合理，用不上，先保留代码")]
    public interface IIocContainer
    {
        /// <summary>
        /// 构建一个实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        //T Resolve<T>() where T : class;
        /// <summary>
        /// 构建类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object Resolve(Type type);
    }

    public sealed class IocContainer : IIocContainer
    {
        private IServiceProvider _serviceProvider;
        public IocContainer(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 构建实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        //public T Resolve<T>() where T : class
        //{
        //    return _serviceProvider.GetService<T>();
        //}
        /// <summary>
        /// 构建类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return _serviceProvider.GetService(type);
        }
    }

    public sealed class ContainerContext
    {
        private static IIocContainer _container;

        /// <summary>
        /// 把服务容器存入一个静态属性中，主要是为了方便不通过构造函数的方式，获取服务获取其他类库中使用
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]    //多线程同时只能访问一次 
        public static IIocContainer Initialize(IIocContainer container)
        {
            if (_container == null)
                _container = container;
            return _container;
        }

        /// <summary>
        /// 当前引擎
        /// </summary>
        public static IIocContainer Current
        {
            get
            {
                return _container;
            }
        }
    }
}
