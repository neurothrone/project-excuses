namespace Excuses.Persistence.Shared.Utils;

public static class ExcuseMessages
{
    public const string DbReadFailed = "Database read failed.";
    public const string DbUpdateFailed = "Database update failed.";
    public const string ExcuseNotFound = "Excuse not found.";
    public const string NoExcuses = "No excuses available.";

    public static string NoExcusesForCategory(string category) =>
        $"No excuses found for category: {category}.";
}