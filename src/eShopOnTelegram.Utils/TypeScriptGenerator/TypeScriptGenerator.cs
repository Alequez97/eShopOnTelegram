using System.Reflection;
using System.Text;

using eShopOnTelegram.Utils.Extensions;

namespace eShopOnTelegram.Utils.TypeScriptGenerator;
public class TypeScriptGenerator
{
    private readonly List<Type> _types = new();
    private readonly StringBuilder _typeScriptDefinition = new();

    public TypeScriptGenerator WithType(Type type)
    {
        _types.Add(type);
        return this;
    }

    public TypeScriptGenerator WithTypeAndItsDerivedTypes(Type baseType)
    {
        _types.Add(baseType);

        var derivedTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => baseType.IsAssignableFrom(type) && type != baseType);

        foreach (var derivedType in derivedTypes)
        {
            _types.Add(derivedType);
        }

        return this;
    }

    public TypeScriptGenerator WithTypesFromNamespace(string namespaceName)
    {
        var typesInNamespace = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.Namespace == namespaceName);

        foreach (var type in typesInNamespace)
        {
            WithType(type);
        }

        return this;
    }

    public TypeScriptGenerator WithTypesContainingNamespace(Assembly assembly, string namespaceName)
    {
        var typesInNamespace = assembly.GetTypes()
            .Where(type => type.Namespace != null && type.Namespace.Contains(namespaceName));

        foreach (var type in typesInNamespace)
        {
            WithType(type);
        }

        return this;
    }

    public string GenerateTypeScriptTypes()
    {
        foreach (Type csharpType in _types)
        {
            if (csharpType.IsEnum)
            {
                AddTypeScriptEnum(csharpType);
            }
            else if (csharpType.IsInterface)
            {
                AddTypeScriptInterface(csharpType);
            }
            else if (csharpType.IsClass)
            {
                AddTypeScriptType(csharpType);
            }
            else
            {
                throw new ArgumentException("Unsupported C# type");
            }
        }

        return _typeScriptDefinition.ToString();
    }

    public void Clear()
    {
        _types.Clear();
    }

    private void AddTypeScriptEnum(Type csharpEnumType)
    {
        _typeScriptDefinition.Append($"export enum {csharpEnumType.Name} {{\n");

        var enumValues = Enum.GetValues(csharpEnumType);
        for (int i = 0; i < enumValues.Length; i++)
        {
            string enumName = Enum.GetName(csharpEnumType, enumValues.GetValue(i));
            int enumValue = (int)enumValues.GetValue(i);
            _typeScriptDefinition.Append($"  {enumName} = {enumValue},\n");
        }

        _typeScriptDefinition.AppendLine("}");
        _typeScriptDefinition.AppendLine();
    }

    private void AddTypeScriptInterface(Type csharpInterface)
    {
        _typeScriptDefinition.Append($"export interface {csharpInterface.Name} {{\n");

        var interfaceProperties = csharpInterface.GetProperties();
        foreach (var property in interfaceProperties)
        {
            string propertyName = property.Name.ToCamelCase();
            Type propertyType = property.PropertyType;

            string tsType = MapCSharpTypeToTypeScriptType(propertyType);

            if (property.IsNullable())
            {
                _typeScriptDefinition.Append($"  {propertyName}?: {tsType};\n");
            }
            else
            {
                _typeScriptDefinition.Append($"  {propertyName}: {tsType};\n");
            }
        }

        _typeScriptDefinition.AppendLine("}");
        _typeScriptDefinition.AppendLine();
    }

    private void AddTypeScriptType(Type csharpClassType)
    {
        _typeScriptDefinition.Append($"export type {csharpClassType.Name} = {{\n");

        var properties = csharpClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            string propertyName = property.Name.ToCamelCase();
            Type propertyType = property.PropertyType;
            string tsType = MapCSharpTypeToTypeScriptType(propertyType);
            
            if (property.IsNullable())
            {
                _typeScriptDefinition.Append($"  {propertyName}?: {tsType};\n");
            }
            else
            {
                _typeScriptDefinition.Append($"  {propertyName}: {tsType};\n");
            }
        }

        _typeScriptDefinition.AppendLine("}");
        _typeScriptDefinition.AppendLine();
    }

    private static string MapCSharpTypeToTypeScriptType(Type csharpType)
    {
        // Nullable value types
        if (Nullable.GetUnderlyingType(csharpType) != null)
        {
            var underlyingType = Nullable.GetUnderlyingType(csharpType);
            return MapCSharpTypeToTypeScriptType(underlyingType);
        }

        // TODO: Support nullable reference types

        if (csharpType.IsGenericType)
        {
            var genericType = csharpType.GetGenericTypeDefinition();
            if (genericType == typeof(List<>) || genericType == typeof(IList<>))
            {
                var genericArgumentType = csharpType.GetGenericArguments()[0];
                var elementTypeScriptType = MapCSharpTypeToTypeScriptType(genericArgumentType);
                return elementTypeScriptType + "[]";
            }
        }

        return csharpType switch
        {
            Type t when t == typeof(bool) => "boolean",
            Type t when t == typeof(int) => "number",
            Type t when t == typeof(long) => "number",
            Type t when t == typeof(double) => "number",
            Type t when t == typeof(float) => "number",
            Type t when t == typeof(decimal) => "number",
            Type t when t == typeof(short) => "number",
            Type t when t == typeof(byte) => "number",
            Type t when t == typeof(sbyte) => "number",
            Type t when t == typeof(uint) => "number",
            Type t when t == typeof(ulong) => "number",
            Type t when t == typeof(ushort) => "number",
            Type t when t == typeof(string) => "string",
            Type t when t == typeof(object) => "any",
            Type t when t.IsEnum => t.Name,
            Type t when t.IsArray => $"{MapCSharpTypeToTypeScriptType(t.GetElementType())}[]", 
            Type t when t.IsInterface => t.Name,
            Type t when t.IsClass => t.Name,
            _ => throw new ArgumentException($"Unsupported C# type: {csharpType.Name}")
        };
    }
}