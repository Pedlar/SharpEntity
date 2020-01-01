using System;

namespace Entity.Component
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DescriptionAttribute : Attribute
    {        
        public string Description { get; private set; }

        public DescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}
