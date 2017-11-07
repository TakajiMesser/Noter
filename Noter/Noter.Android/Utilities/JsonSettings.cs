using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Noter.Droid.Utilities
{
    public static class JsonSettings
    {
        public static JsonSerializerSettings Settings
        {
            get
            {
                var contractResolver = new SetPropertiesContractResolver();

                return new JsonSerializerSettings
                {
                    ContractResolver = contractResolver
                };
            }
        }

        private class SetPropertiesContractResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var property = base.CreateProperty(member, memberSerialization);

                property.ShouldSerialize = _ =>
                {
                    var propertyInfo = member as PropertyInfo;
                    if (propertyInfo == null)
                    {
                        return false;
                    }

                    if (propertyInfo.SetMethod != null)
                    {
                        return true;
                    }

                    var getMethod = propertyInfo.GetMethod;
                    return Attribute.GetCustomAttribute(getMethod, typeof(CompilerGeneratedAttribute)) != null;
                };

                return property;
            }
        }
    }
}