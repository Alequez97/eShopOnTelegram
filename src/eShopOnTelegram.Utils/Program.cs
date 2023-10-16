using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Utils.TypeScriptGenerator;

var typeScriptGenerator = new TypeScriptGenerator();

typeScriptGenerator
    .WithTypesContainingNamespace(typeof(GetRequest).Assembly, typeof(GetRequest).Namespace);

var typeScriptTypes = typeScriptGenerator.GenerateTypeScriptTypes();

File.WriteAllText("../../../../eShopOnTelegram.Admin/ClientApp/src/types/api.type.ts", typeScriptTypes);

Console.WriteLine("Generation finished!!!");
