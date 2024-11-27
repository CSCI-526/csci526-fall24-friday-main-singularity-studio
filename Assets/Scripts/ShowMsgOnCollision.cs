using UnityEngine;

public class ShowMsgOnCollision : MonoBehaviour
{
    public string message = "Message";
    public float displayTime = 1.0f;

    // void OnCollisionEnter2D(Collision2D other)
    void OnTriggerEnter2D(Collider2D other)
    {
        // if (other.gameObject.CompareTag("Player"))
        if (other.gameObject.name == "Player")
        {
            print("collides with player");
            MessageManager.ShowMessage(message, displayTime);
        }
    }
}
