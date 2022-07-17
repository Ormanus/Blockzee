using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    [System.Serializable]
    public class SaveData
    {
        public int version;
        public int[] unlocks;
    }

    const string key = "SaveData";

    static SaveData _current = null;
    static SaveData CurrentSave
    {
        get
        {
            if (_current == null)
            {
                string data = PlayerPrefs.GetString(key, "");
                if (data == null || data.Equals(""))
                {
                    _current = new SaveData()
                    {
                        version = 0,
                        unlocks = new int[5] // one for each level
                    };
                }
                else
                {
                    _current = JsonUtility.FromJson<SaveData>(data);
                }
            }
            Debug.Log($"{_current} ({_current?.version}, {_current?.unlocks.Length})");
            return _current;
        }
    }

    public static int GetLevelState(int index)
    {
        return CurrentSave.unlocks[index];
    }

    public static void SetLevelState(int index, int state)
    {
        Debug.Log($"index: {index}, state: {state}");
        CurrentSave.unlocks[index] = state;
        Save();
    }

    static void Save()
    {
        string data = JsonUtility.ToJson(CurrentSave);
        PlayerPrefs.SetString(key, data);
        PlayerPrefs.Save();
    }
}
