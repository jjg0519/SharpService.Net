using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheService.Common
{
    public static class SingletonProvider<T> where T : class, new()
    {
        private static readonly object LockHelper = new object();
        private static T _instance;
        private static Lazy<T> _lazyInstance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockHelper)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();
                        }
                    }
                }
                return _instance;
            }
        }

        public static Lazy<T> LazyInstance
        {
            get { return _lazyInstance ?? (_lazyInstance = new Lazy<T>(() => Instance)); }
        }
    }
}
