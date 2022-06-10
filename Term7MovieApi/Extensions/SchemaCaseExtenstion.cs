using System.Text.Json;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class SnakeCasePropertyNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name.ToSnakeCase();
    }
}

public static class StringExtensions
{
    public static string ToSnakeCase(this string str)
    {
        return string.Concat(str.Select((character, index) =>
                index > 0 && char.IsUpper(character)
                    ? "-" + character
                    : character.ToString()))
            .ToLower();
    }
}

public class SnakeCaseSchemaFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters != null)
        {
            foreach (var item in operation.Parameters)
            {
                if (char.IsUpper(item.Name[0]))
                {
                    item.Name = item.Name.ToSnakeCase();
                }
            }
        }

    }
}