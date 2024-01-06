using System.Reflection;
using System.Text;

using eShopOnTelegram.Utils.Extensions;

namespace eShopOnTelegram.Utils.TypeScriptGenerator;
public class TypeScriptGenerator
{
	private List<Type> _types = new();
	private Dictionary<string, string> _namesReplacements = new();
	private StringBuilder _typeScriptDefinition = new();

	private readonly StringComparison _stringComparison;

	public TypeScriptGenerator(StringComparison stringComparison)
	{
		_stringComparison = stringComparison;
	}

	public TypeScriptGenerator WithTypeNameReplacement(string csharpTypeNamePart, string typeScriptTypeNamePart)
	{
		_namesReplacements.Add(csharpTypeNamePart, typeScriptTypeNamePart);
		return this;
	}

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
			string typeScriptTypeName = csharpType.Name;
			foreach (var (cSharpTypeReplacement, typeScriptTypeReplacement) in _namesReplacements)
			{
				typeScriptTypeName = typeScriptTypeName.Replace(cSharpTypeReplacement, typeScriptTypeReplacement, _stringComparison);
			}

			if (csharpType.IsEnum)
			{
				AddTypeScriptEnum(csharpType, typeScriptTypeName);
			}
			else if (csharpType.IsInterface)
			{
				AddTypeScriptInterface(csharpType, typeScriptTypeName);
			}
			else if (csharpType.IsClass)
			{
				AddTypeScriptType(csharpType, typeScriptTypeName);
			}
			else
			{
				throw new ArgumentException("Unsupported C# type");
			}
		}

		return _typeScriptDefinition.ToString();
	}

	public TypeScriptGenerator Clear()
	{
		_types = new();
		_typeScriptDefinition = new();
		_namesReplacements = new();
		return this;
	}

	private void AddTypeScriptEnum(Type csharpEnumType, string typeScriptTypeName)
	{
		_typeScriptDefinition.Append($"export enum {typeScriptTypeName} {{\n");

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

	private void AddTypeScriptInterface(Type csharpInterface, string typeScriptTypeName)
	{
		_typeScriptDefinition.Append($"export interface {typeScriptTypeName} {{\n");

		var interfaceProperties = csharpInterface.GetProperties();
		foreach (var property in interfaceProperties)
		{
			string propertyName = property.Name.ToCamelCase();
			Type propertyType = property.PropertyType;

			string tsPropertyTypeName = MapCSharpTypeToTypeScriptType(propertyType);
			foreach (var (cSharpTypeReplacement, typeScriptTypeReplacement) in _namesReplacements)
			{
				tsPropertyTypeName = tsPropertyTypeName.Replace(cSharpTypeReplacement, typeScriptTypeReplacement, _stringComparison);
			}

			if (property.IsNullable())
			{
				_typeScriptDefinition.Append($"  {propertyName}?: {tsPropertyTypeName};\n");
			}
			else
			{
				_typeScriptDefinition.Append($"  {propertyName}: {tsPropertyTypeName};\n");
			}
		}

		_typeScriptDefinition.AppendLine("}");
		_typeScriptDefinition.AppendLine();
	}

	private void AddTypeScriptType(Type csharpClassType, string typeScriptTypeName)
	{
		_typeScriptDefinition.Append($"export interface {typeScriptTypeName} {{\n");

		var properties = csharpClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

		foreach (var property in properties)
		{
			string propertyName = property.Name.ToCamelCase();

			Type propertyType = property.PropertyType;
			string tsPropertyTypeName = MapCSharpTypeToTypeScriptType(propertyType);
			foreach (var (cSharpTypeReplacement, typeScriptTypeReplacement) in _namesReplacements)
			{
				tsPropertyTypeName = tsPropertyTypeName.Replace(cSharpTypeReplacement, typeScriptTypeReplacement, _stringComparison);
			}

			if (property.IsNullable())
			{
				_typeScriptDefinition.Append($"  {propertyName}?: {tsPropertyTypeName};\n");
			}
			else
			{
				_typeScriptDefinition.Append($"  {propertyName}: {tsPropertyTypeName};\n");
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
			if (genericType == typeof(List<>) || genericType == typeof(IList<>) || genericType == typeof(IEnumerable<>))
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
			Type t when t.Name == "DateTime" => "Date",
			Type t when t.IsEnum => t.Name,
			Type t when t.IsArray => $"{MapCSharpTypeToTypeScriptType(t.GetElementType())}[]",
			Type t when t.IsInterface => t.Name,
			Type t when t.IsClass => t.Name,
			_ => throw new ArgumentException($"Unsupported C# type: {csharpType.Name}")
		};
	}
}
