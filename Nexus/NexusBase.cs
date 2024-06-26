﻿using System.Reflection;
using Microsoft.VisualBasic.CompilerServices;

namespace Nexus;

public class NexusBase : INexus
{
    public readonly string UUID;
    
    public NexusBase()
    {
        UUID = System.Guid.NewGuid().ToString();
    }

    public NexusBase(String uuid)
    {
        UUID = uuid;
    }
    
    public void SetInstanceProperty<TPropertyType>(string propertyName, TPropertyType value)
    {
        PropertyInfo? propertyInfo = GetType().GetProperty( 
            propertyName, 
            BindingFlags.Instance|BindingFlags.Public,
            null, 
            value.GetType(),
            [],
            null
        );
        propertyInfo?.SetValue(this, value, null);
    }
    
    public object? GetPropValue(string propName)
    {
        // Use reflection to get the property
        PropertyInfo? propInfo = GetType().GetProperty(propName);
    
        if (propInfo == null)
        {
            throw new ArgumentException($"Property '{propName}' not found on object of type '{GetType().FullName}'.");
        }

        // Get the value of the property
        return propInfo.GetValue(this);
    }

    public Dictionary<string, Type> GetInputs()
    {
        return GetAllOfType(typeof(NexusInput<>));
    }
    
    public Dictionary<string, Type> GetOutputs()
    {
        return GetAllOfType(typeof(NexusOutput<>));
    }

    public List<NexusStatic> GetAllStatics()
    {
        List<NexusStatic> resultList = new List<NexusStatic>();
        FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            if (field.FieldType == typeof(NexusStatic))
            {
                NexusStatic instance = (NexusStatic)field.GetValue(this);
                resultList.Add(instance);
            }
        }
        return resultList;
    }

    public List<NexusDisplay> GetAllDisplays()
    {
        List<NexusDisplay> resultList = new List<NexusDisplay>();
        FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            if (field.FieldType == typeof(NexusDisplay))
            {
                NexusDisplay instance = (NexusDisplay)field.GetValue(this);
                resultList.Add(instance);
            }
        }
        return resultList;
    }

    public string GetUUID()
    {
        return UUID;
    }

    private Dictionary<string, Type> GetAllOfType(Type type)
    {
        Dictionary<string, Type> nodeInputProperties = new Dictionary<string, Type>();
        PropertyInfo[] propertyInfos = GetType().GetProperties();
        foreach (var prop in propertyInfos)
        {
            // Check if property type is NodeInput<T>
            if (prop.PropertyType.IsGenericType &&
                prop.PropertyType.GetGenericTypeDefinition() == type)
            {
                // Get the generic argument type T
                var genericType = prop.PropertyType.GetGenericArguments().First();
                // Add to dictionary with property name and T
                nodeInputProperties.Add(prop.Name, genericType);
            }
        }
        return nodeInputProperties;
    }
}