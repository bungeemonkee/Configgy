using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Configgy;

public class NullableReferenceChecker
{
    private static readonly Type _dictionaryType = typeof(IDictionary<,>);
    private static readonly Type _enumerableType = typeof(IEnumerable<>);
    
    public bool RequiresNullableReferenceCheck(IConfigProperty property)
    {   
        // There really should be a Type.IsValueType property
        // If there was we could just return !property.ValueType.IsValueType
        // But this (property.ValueType.IsClass || property.ValueType.IsInterface) is functionally equivalent
        // So since it is a value type then it doesn't need this check
        // And we can only check values where we have nullability information
        return (property.ValueType.IsClass || property.ValueType.IsInterface)
               && property.NullabilityInfo is not { ReadState: NullabilityState.Unknown };
    }

    public bool SatisfiesNullableReferenceRules(IConfigProperty property, object? value)
    {
        return SatisfiesNullableReferenceRules(property.NullabilityInfo, value);
    }

    private bool SatisfiesNullableReferenceRules(NullabilityInfo nullabilityInfo, object? value)
    {
        if (nullabilityInfo.ReadState == NullabilityState.Unknown)
        {
            // If there is no nullability info fall back on the default C# behavior
            // ..which is that nulls are allowed
            
            // ASSUMPTION: If the nullability is not known for this property then
            // the nullability of any generic type arguments will not be known either

            return true;
        }

        if (value == null)
        {
            switch (nullabilityInfo.ReadState)
            {
                case NullabilityState.NotNull:
                    // Obviously this is not valid
                    return false;
                case NullabilityState.Nullable:
                    // The value is null, but that's allowed
                    return true;

                    // NOTE: This condition must remain within the `if (value == null)` condition
                    // Because even though it is allowed to be null it may be an array or other
                    // complex object type with elements or members that can not be null.
                    // Eg. object[]? - The array can be null but if it isn't then
                    // every member of the array must be checked.
            }
        }

        // Handle arrays
        // Handle this case first because it is quick to detect and can't overlap with the more complex cases below
        if (nullabilityInfo.Type.IsArray)
        {
            // The value is an array and each element needs to be checked, so check every value in the array
            foreach (var element in (object?[])value!)
            {
                if (!SatisfiesNullableReferenceRules(nullabilityInfo.ElementType!, element))
                {
                    return false;
                }
            }

            // Couldn't find any nullability violations so it should be good
            return true;
        }
        
        // Handle complex objects
        // NOTE: It is important to handle properties of complex objects before
        // dictionaries or enumerables because it is possible for someone
        // to implement a dictionary or enumerable that also has other properties
        foreach (var memberInfo in nullabilityInfo.Type.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy))
        {
            NullabilityInfo memberNullability;
            object? memberValue;
            switch (memberInfo)
            {
                case FieldInfo fieldInfo:
                    memberNullability = NullabilityInfoGetter.Get(fieldInfo);
                    memberValue = fieldInfo.GetValue(value);
                    break;
                case PropertyInfo propertyInfo:
                    memberNullability = NullabilityInfoGetter.Get(propertyInfo);
                    memberValue = propertyInfo.GetValue(value);
                    break;
                default:
                    // Non-supported member types are skipped
                    continue;
            }

            // We now have the nullability information and the value, determine if they align
            if (!SatisfiesNullableReferenceRules(memberNullability, memberValue))
            {
                // Nullability violation - we don't need to keep checking
                return false;
            }
        }

        // TODO: Handle dictionaries
        // It is important to handle dictionaries before other enumerables because
        // dictionaries are also enumerable and we don't want to duplicate those checks
        if (IsGenericType(nullabilityInfo.Type, _dictionaryType))
        {
            // Check the keys
            
            // Check the values
            
        }
        else
        {
            // TODO: Handle other enumerables
            if (nullabilityInfo.Type.IsAssignableTo(_enumerableType))
            {
                // TODO
            }
        }

        // Couldn't find any nullability violations so it should be good
        return true;
    }

    private static bool IsGenericType(Type type, Type genericType)
    {
        var interfaces = type.FindInterfaces((x, y) => x.IsConstructedGenericType, null);
        foreach (var interfaceType in interfaces)
        {
            if (interfaceType.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }
        }

        return false;
    }
}