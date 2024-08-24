using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitDatas : MonoBehaviour
{
    private void Awake()
    {
        CreateData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log($"Data reseted");
        }
    }



    public void CreateData()
    {
        if (!PlayerPrefs.HasKey(GameData.PP_USER_DATA))
        {
            UserData.data = new UserDeepData();
            UserData.data.Coins = 0;
            UserData.data.Diamonds = 0;
            UserData.SaveData();
        }
        else
        {
            var stringJson = PlayerPrefs.GetString(GameData.PP_USER_DATA);
            Debug.Log(" UserData json : " + stringJson);
            UserData.data = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDeepData>(stringJson);
        }
    }
}
