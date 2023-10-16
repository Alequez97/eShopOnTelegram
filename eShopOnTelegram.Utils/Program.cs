using eShopOnTelegram.Domain.Requests.Products;
using eShopOnTelegram.Utils.TypeScriptGenerator;

var typeScriptGenerator = new TypeScriptGenerator();

typeScriptGenerator
    .WithTypesFromNamespace(typeof(CreateProductRequest).Namespace);

var typeScriptTypes = typeScriptGenerator.GenerateTypeScriptTypes();

Console.WriteLine(typeScriptTypes);
