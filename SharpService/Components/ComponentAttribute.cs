using System;

namespace SharpService.Components
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentAttribute : Attribute
    {
        public LifeStyle LifeStyle { get; private set; }

        public ComponentAttribute() : this(LifeStyle.Singleton) { }

        public ComponentAttribute(LifeStyle lifeStyle)
        {
            LifeStyle = lifeStyle;
        }
    }

    public enum LifeStyle
    {
        Transient,

        Singleton
    }
}
