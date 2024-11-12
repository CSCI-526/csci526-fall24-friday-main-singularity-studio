using UnityEngine;
using UnityEngine.UI;

public class ButtonPulse : MonoBehaviour
{
    public float pulseSpeed = 2.0f;
    public float minScale = 0.9f;
    public float maxScale = 1.1f;
    private bool isPulsing = false;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isPulsing)
        {
            float scale = Mathf.PingPong(Time.time * pulseSpeed, maxScale - minScale) + minScale;
            transform.localScale = new Vector3(scale, scale, 1);
        }
    }

    public void StartPulse()
    {
        isPulsing = true;
        gameObject.SetActive(true);
    }

    public void StopPulse()
    {
        isPulsing = false;
        gameObject.SetActive(false); 
        transform.localScale = Vector3.one;
    }
}

