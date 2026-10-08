using System;

namespace SUIF.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class UQueryAttribute : Attribute
    {
        public string Query { get; }

        public UQueryAttribute(string query)
        {
            Query = query;
        }
    }
}
