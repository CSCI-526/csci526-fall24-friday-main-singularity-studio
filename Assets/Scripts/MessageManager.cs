using UnityEngine;
using TMPro;
using System.Collections;

public class MessageManager : MonoBehaviour
{
    public TextMeshPro messageText;
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
        messageText.text = "<mark=#FFFF00AA>" + message + "</mark>";
        yield return new WaitForSeconds(displayTime);

        for (float alpha = 1; alpha >= 0; alpha -= Time.deltaTime / displayTime)
        {
            Color color = messageText.color;
            color.a = alpha;
            messageText.color = color;
            yield return null;
        }

        messageText.text = "";
    }
}
