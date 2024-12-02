using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MessageManager : MonoBehaviour
{
    public TextMeshPro messageText1, messageText2, messageText3, messageText4, messageText5, messageText6;
    private static MessageManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void ShowMessage(string message, float displayTime)
    {
        instance.StartCoroutine(instance.DisplayMessage(message, displayTime));
    }

    IEnumerator DisplayMessage(string message, float displayTime)
    {
        messageText1.text = message;
        messageText2.text = message;
        messageText3.text = message;
        messageText4.text = message;
        messageText5.text = message;
        messageText6.text = message;
        yield return new WaitForSeconds(displayTime);

        for (float alpha = 3; alpha >= 0; alpha -= Time.deltaTime / displayTime)
        {
            Color color = messageText1.color;
            color.a = alpha;
            messageText1.color = color;
            messageText2.color = color;
            messageText3.color = color;
            messageText4.color = color;
            messageText5.color = color;
            messageText6.color = color;
            yield return null;
        }

        messageText1.text = "";
        messageText2.text = "";
        messageText3.text = "";
        messageText4.text = "";
        messageText5.text = "";
        messageText6.text = "";
    }
}
