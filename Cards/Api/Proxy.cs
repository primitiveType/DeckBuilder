using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

public partial class Proxy : ICustomTypeDescriptor
{
    public Proxy(object instance)
    {
        if (instance == null)
            throw new ArgumentNullException(nameof(instance));

        Instance = instance;
        Properties = TypeDescriptor.GetProperties(instance).OfType<PropertyDescriptor>().Select(d => new ProxyProperty(instance, d)).ToDictionary(p => p.Name);
    }
    public void SetProperty(PropertyDescriptor propertyDescriptor, object value)
    {
        Instance.GetType().GetProperty(propertyDescriptor.Name).SetValue(Instance, value, null);
    }

    public object Instance { get; }
    public IDictionary<string, ProxyProperty> Properties { get; }

    public AttributeCollection GetAttributes() => TypeDescriptor.GetAttributes(Instance);
    public string GetClassName() => TypeDescriptor.GetClassName(Instance);
    public string GetComponentName() => TypeDescriptor.GetComponentName(Instance);
    public TypeConverter GetConverter() => TypeDescriptor.GetConverter(Instance);
    public EventDescriptor GetDefaultEvent() => TypeDescriptor.GetDefaultEvent(Instance);
    public object GetEditor(Type editorBaseType) => TypeDescriptor.GetEditor(Instance, editorBaseType);
    public EventDescriptorCollection GetEvents() => TypeDescriptor.GetEvents(Instance);
    public EventDescriptorCollection GetEvents(Attribute[] attributes) => TypeDescriptor.GetEvents(Instance, attributes);
    public PropertyDescriptor GetDefaultProperty() => TypeDescriptor.GetDefaultProperty(Instance);
    public PropertyDescriptorCollection GetProperties() => new PropertyDescriptorCollection(Properties.Values.Select(p => new Desc(this, p)).ToArray());
    public PropertyDescriptorCollection GetProperties(Attribute[] attributes) => GetProperties();
    public object GetPropertyOwner(PropertyDescriptor pd) => Instance;
}
