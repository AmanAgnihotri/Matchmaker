namespace Matchmaker;

public interface IDb
{
  Task<string?> GetString(string key);

  Task<bool> SetString(string key, string value, TimeSpan? duration = null);

  Task<bool> DeleteString(string key);
}
