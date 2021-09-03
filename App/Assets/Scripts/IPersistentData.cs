public interface IPersistentData
{
    void SetInt(string key, int value);
    int GetInt(string key);
    void SetFloat(string key, float value);
    float GetFloat(string key);
    void SetString(string key, string value);
    string GetString(string key);
    void Save();
}