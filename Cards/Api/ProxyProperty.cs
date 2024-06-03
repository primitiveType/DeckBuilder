using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

public class ProxyProperty
{
    public ProxyProperty(string name, object value)
    {
        if (name == null)
            throw new ArgumentNullException(nameof(value));

        Name = name;
        Value = value;
        Attributes = new List<Attribute>();
    }

    public ProxyProperty(object instance, PropertyDescriptor descriptor)
    {
        if (descriptor == null)
            throw new ArgumentNullException(nameof(descriptor));

        Name = descriptor.Name;
        Value = descriptor.GetValue(instance);
        var copy = Activator.CreateInstance(instance.GetType());
        HasDefaultValue = true;

        DefaultValue = descriptor.GetValue(copy);
        

        IsReadOnly = instance.GetType().GetProperty(descriptor.Name).SetMethod == null;//(descriptor.Attributes.OfType<ReadOnlyAttribute>().FirstOrDefault()?.IsReadOnly).GetValueOrDefault();
        //ShouldSerializeValue = descriptor.ShouldSerializeValue(instance);
        Attributes = descriptor.Attributes.Cast<Attribute>().ToList();
        PropertyType = descriptor.PropertyType;
    }

    public string Name { get; }
    public object Value { get; set; }
    public object DefaultValue { get; set; }
    public bool HasDefaultValue { get; set; }
    public bool IsReadOnly { get; set; }
    public bool ShouldSerializeValue => false;// Value != DefaultValue;
    public Type PropertyType { get; set; }
    public IList<Attribute> Attributes { get; }
}