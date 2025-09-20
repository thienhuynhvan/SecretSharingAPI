public static class ApiKeyStore
{
    private static readonly Dictionary<string, string> _keys = new(); // key → email

    public static string GenerateKey(string email)
    {
        var key = Guid.NewGuid().ToString("N"); //remove '-'
        _keys[key] = email;
        return key;
    }

    public static bool ValidateKey(string key) => _keys.ContainsKey(key);

    public static string GetUserByKey(string key) => _keys[key];
}