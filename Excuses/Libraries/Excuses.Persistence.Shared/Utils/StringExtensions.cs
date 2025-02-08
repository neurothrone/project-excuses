namespace Excuses.Persistence.Shared.Utils;

public static class StringExtensions
{
    public static string Capitalize(this string input)
    {
        return string.IsNullOrEmpty(input)
            ? input
            : char.ToUpper(input[0]) + input[1..].ToLower();
    }
}