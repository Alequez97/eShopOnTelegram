using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Utils.TypeScriptGenerator;

var typeScriptGenerator = new TypeScriptGenerator();

typeScriptGenerator
    .WithTypesContainingNamespace(typeof(GetRequest).Assembly, typeof(GetRequest).Namespace);

var typeScriptTypes = typeScriptGenerator.GenerateTypeScriptTypes();

File.WriteAllText("./api.type.ts", typeScriptTypes);

Console.WriteLine(typeScriptTypes);
