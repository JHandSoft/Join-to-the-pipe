using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance = null;
    readonly List<string[]> data = new List<string[]>();
    string path;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        path = Application.persistentDataPath + "/SaveFile.jmr";
        string[] fileText;
        try
        {
            fileText = File.ReadAllLines(path);
            foreach (string l in fileText)
            {
                string line = EncodeString(l);
                string[] keywordAndValue = line.Split('>');
                if (keywordAndValue.Length == 2)
                    data.Add(keywordAndValue);
            }
        }
        catch { }
    }

    string EncodeString(string str)
    {
        string newString = "";
        foreach (char c in str)
            newString += Convert.ToChar(c ^ 200);
        return newString;
    }

    public bool GetBool(string value, bool defaultValue)
    {
        if (GetValue(value) == null)
            return defaultValue;
        try { return Convert.ToBoolean(GetValue(value)); }
        catch { return defaultValue; }
    }

    public int GetInt(string value, int defaultValue)
    {
        if (GetValue(value) == null)
            return defaultValue;
        try { return Convert.ToInt32(GetValue(value)); }
        catch { return defaultValue; }
    }

    public float GetFloat(string value, float defaultValue)
    {
        if (GetValue(value) == null)
            return defaultValue;
        try { return Convert.ToSingle(GetValue(value)); }
        catch { return defaultValue; }
    }

    string GetValue(string keyword)
    {
        foreach (string[] s in data)
        {
            if (s[0] == keyword)
                return s[1];
        }
        return null;
    }

    public void SetValue(string keyword, object value)
    {
        foreach (string[] s in data)
        {
            if (s[0] == keyword)
            {
                s[1] = value.ToString();
                return;
            }
        }
        data.Add(new string[] { keyword, value.ToString() });
    }

    public void SaveFile()
    {
        List<string> l = new List<string>();
        foreach (string[] s in data)
            l.Add(EncodeString(s[0] + ">" + s[1]));
        File.WriteAllLines(path, l);
    }
}