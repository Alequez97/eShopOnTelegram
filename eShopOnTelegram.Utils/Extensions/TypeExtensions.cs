namespace eShopOnTelegram.Utils.Extensions;
public static class TypeExtensions
{
    public static bool IsNullableType(this Type type)
    {
        return Nullable.GetUnderlyingType(type) != null;
    }
}
