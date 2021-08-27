using UnityEngine;

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

public class PlayerPrefsData : IPersistentData
{
    public void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public int GetInt(string key)
    {
        return PlayerPrefs.GetInt(key);
    }

    public void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public float GetFloat(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }

    public void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public string GetString(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public void Save()
    {
        PlayerPrefs.Save();
    }
}