using System;

namespace Noter.Shared.DataAccessLayer
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IdentifierAttribute : Attribute
    {
        public IdentifierAttribute() { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ForeignKeyAttribute : Attribute
    {
        private Type _type;

        public Type Type { get { return _type; } }

        public ForeignKeyAttribute(Type type)
        {
            _type = type;
        }
    }
}