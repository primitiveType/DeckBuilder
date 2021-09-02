using UnityEngine;

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