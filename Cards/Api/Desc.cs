using System;
using System.ComponentModel;
using System.Linq;


public partial class Proxy 
{
   

    private class Desc : PropertyDescriptor
    {
        public Desc(Proxy proxy, ProxyProperty property)
            : base(property.Name, property.Attributes.ToArray())
        {
            Proxy = proxy;
            Property = property;
        }

        public Proxy Proxy { get; }
        public ProxyProperty Property { get; }

        public override Type ComponentType => Proxy.GetType();
        public override Type PropertyType => Property.PropertyType ?? typeof(object);
        public override bool IsReadOnly => Property.IsReadOnly;
        public override bool CanResetValue(object component) => Property.HasDefaultValue;
        public override object GetValue(object component) => Property.Value;
        public override void ResetValue(object component) { if (Property.HasDefaultValue) Property.Value = Property.DefaultValue; }
        public override void SetValue(object component, object value) => Property.Value = value;
        public override bool ShouldSerializeValue(object component) => Property.ShouldSerializeValue;
    }
}
