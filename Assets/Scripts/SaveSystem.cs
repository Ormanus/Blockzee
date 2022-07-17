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
    const int levelCount = 9;

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
                        unlocks = new int[levelCount] // one for each level
                    };
                }
                else
                {
                    _current = JsonUtility.FromJson<SaveData>(data);
                    if (_current.unlocks.Length != levelCount)
                    {
                        int[] newUnlocks = new int[levelCount];
                        for (int i = 0; i < newUnlocks.Length && i < _current.unlocks.Length; i++)
                        {
                            newUnlocks[i] = _current.unlocks[i];
                        }
                        _current.unlocks = newUnlocks;
                    }
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
