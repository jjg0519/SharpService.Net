using System;
using Autofac;

namespace SharpService.Components
{
    public class AutofacObjectContainer : IObjectContainer
    {
        private IContainer _container;

        private ContainerBuilder _containerBuilder;

        public AutofacObjectContainer()
        {
            _containerBuilder = new ContainerBuilder();
        }

        public AutofacObjectContainer(ContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder;
        }

        public IContainer Container
        {
            get
            {
                return _container;
            }
        }

        public void Build()
        {
            _container = _containerBuilder.Build();
        }

        public void RegisterType(Type implementationType, string serviceName = null, LifeStyle life = LifeStyle.Singleton)
        {         
            var registrationBuilder = _containerBuilder.RegisterType(implementationType);
            if (serviceName != null)
            {
                registrationBuilder.Named(serviceName, implementationType);
            }
            if (life == LifeStyle.Singleton)
            {
                registrationBuilder.SingleInstance();
            }         
        }

        public void RegisterType(Type serviceType, Type implementationType, string serviceName = null, LifeStyle life = LifeStyle.Singleton)
        {
            var registrationBuilder = _containerBuilder.RegisterType(implementationType).As(serviceType);
            if (serviceName != null)
            {
                registrationBuilder.Named(serviceName, serviceType);
            }
            if (life == LifeStyle.Singleton)
            {
                registrationBuilder.SingleInstance();
            }
        }

        public void Register<TService, TImplementer>(string serviceName = null, LifeStyle life = LifeStyle.Singleton)
            where TService : class
            where TImplementer : class, TService
        {
            var registrationBuilder = _containerBuilder.RegisterType<TImplementer>().As<TService>();
            if (serviceName != null)
            {
                registrationBuilder.Named<TService>(serviceName);
            }
            if (life == LifeStyle.Singleton)
            {
                registrationBuilder.SingleInstance();
            }
        }

        public void RegisterInstance<TService, TImplementer>(TImplementer instance, string serviceName = null)
            where TService : class
            where TImplementer : class, TService
        {
            var registrationBuilder = _containerBuilder.RegisterInstance(instance).As<TService>().SingleInstance();
            if (serviceName != null)
            {
                registrationBuilder.Named<TService>(serviceName);
            }
        }

        public TService Resolve<TService>() where TService : class
        {
            return _container.Resolve<TService>();
        }

        public object Resolve(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        public bool TryResolve<TService>(out TService instance) where TService : class
        {
            return _container.TryResolve(out instance);
        }

        public bool TryResolve(Type serviceType, out object instance)
        {
            return _container.TryResolve(serviceType, out instance);
        }

        public TService ResolveNamed<TService>(string serviceName) where TService : class
        {
            return _container.ResolveNamed<TService>(serviceName);
        }

        public object ResolveNamed(string serviceName, Type serviceType)
        {
            return _container.ResolveNamed(serviceName, serviceType);
        }

        public bool TryResolveNamed(string serviceName, Type serviceType, out object instance)
        {
            return _container.TryResolveNamed(serviceName, serviceType, out instance);
        }
    }
}

