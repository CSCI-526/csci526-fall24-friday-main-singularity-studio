using UnityEngine;

public class ShowMsgOnCollision : MonoBehaviour
{
    public string message = "Oops, watch your health bar!";
    public float displayTime = 1.0f;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            MessageManager.ShowMessage(message, displayTime);
        }
    }
}
