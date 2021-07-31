using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsHandler : MonoBehaviour
{
    public static bool GetBool(string key)
    {
        if(GetInt(key) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static int GetInt(string key)
    {
        return PlayerPrefs.GetInt(key);
    }
    public static string GetString(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public static float GetFloat(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }
    public static void SetBool(string key, bool state)
    {
        int val = 0;
        if (state)
        {
            val = 1;
        }
        SetInt(key, val);
    }
    public static void SetInt(string key, int val)
    {
        PlayerPrefs.SetInt(key, val);
        PlayerPrefs.Save();
    }
    public static void SetString(string key, string val)
    {
        PlayerPrefs.SetString(key, val);
        PlayerPrefs.Save();
    }
    public static void SetFloat(string key, float val)
    {
        PlayerPrefs.SetFloat(key, val);
        PlayerPrefs.Save();
    }
    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }
}
