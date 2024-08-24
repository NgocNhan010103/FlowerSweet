using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public AudioClip soundClickBTN;
    [SerializeField] List<Menu> menus;
    [SerializeField] List<Others> others;

    void Awake()
    {
        Instance = this;
    }

    public void InsertMenuInList()
    {
         menus?.Clear();
        foreach(var chill in GameObject.FindGameObjectsWithTag("Menu"))
        {
            menus.Add(chill.GetComponent<Menu>());
        }
        others?.Clear();
        foreach (var chill in GameObject.FindGameObjectsWithTag("Other"))
        {
            others.Add(chill.GetComponent<Others>());
        }
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (menus[i].menuName == menuName)
            {
                menus[i].Open();
               
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        for (int j = 0; j < others.Count; j++)
        {
            others[j].Close();
        }
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();

        SoundFXManager.Instance.PlaySoundFXClip(soundClickBTN, 3f);
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
        for (int j = 0; j < others.Count; j++)
        {
            others[j].Open();
        }
        SoundFXManager.Instance.PlaySoundFXClip(soundClickBTN, 3f);
    }



    public void QuitGame()
    {
        Application.Quit();
    }
}
