using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textNotification;

    public void DisplayNotification()
    {
        StartCoroutine(SetText());
    }

    private IEnumerator SetText()
    {
        if (textNotification.text == "")
        {
            textNotification.text = "Coming soon....!";
        }

        yield return new WaitForSeconds(1);

        textNotification.text = "";
    }


}
