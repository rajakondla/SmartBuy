using System;

namespace SmartBuy.Common.Utilities
{
    public sealed class DefaultObject<T>
    {
        private DefaultObject()
        {

        }

        private static Lazy<T> defaultObj = new Lazy<T>(() => (T)Activator.CreateInstance(typeof(T)));
        public static T GetObject => defaultObj.Value;
    }
}
