using eShopOnTelegram.Domain.Requests.ProductAttributes;
using eShopOnTelegram.Utils.TypeScriptGenerator;

var typeScriptGenerator = new TypeScriptGenerator();

typeScriptGenerator
    .WithType(typeof(CreateProductAttributeRequest));

var typeScriptTypes = typeScriptGenerator.GenerateTypeScriptTypes();

Console.WriteLine(typeScriptTypes);
