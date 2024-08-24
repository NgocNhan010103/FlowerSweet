using UnityEngine;
using UnityEngine.SceneManagement;

public class Callback : MonoBehaviour
{
    private void Start()
    {
        MenuManager.Instance.InsertMenuInList();
    }

    #region Reflection Methods
    public void CallMethods(string methodName)
    {
        var method = UIController.instance.GetType().GetMethod(methodName);
        method?.Invoke(UIController.instance, null);
    }

    #endregion

    public void OpenMenu(string menu)
    {
        MenuManager.Instance.OpenMenu(menu);
    }

    public void CloseMenu(Menu menu)
    {
        MenuManager.Instance.CloseMenu(menu);
    }

    public void QuitGame()
    {
        MenuManager.Instance.QuitGame();
    }


    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
