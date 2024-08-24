using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;



[System.Serializable]
public class UserDeepData
{
    public int Coins;
    public int Diamonds;
    public int CoinsInCache;
    public int ScoreInCache;
    public int LevelInCache;
}

[System.Serializable]
public class PetalDefind
{
    public int Id;
    public string Name;
    public Color PetalColor;
    public int IdSlot;
}

[System.Serializable]
public class PetalSlotsDefind
{
    public int Id;
    public string Name;
    public State State;
}


[System.Serializable]
public class FlowerData
{
    public int Id;
    public string Name;
    public int SlotId;
    public int PetalNumber;
    public int Score;
    public int Coins;
    public List<PetalDefind> ListPetalDefind;
}

public class UserData
{
    public static UserDeepData data;
    public static void SaveData()
    {
        PlayerPrefs.SetString(GameData.PP_USER_DATA, JsonConvert.SerializeObject(data));
    }
}
public class GameData
{
    public const string PP_USER_DATA = "UserDataPrefab";
}