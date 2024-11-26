using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppearance : MonoBehaviour
{
    private float lastZRotation; 
    public bool isSurprise = false;
    private bool isTouchingPlatform;
    private SpriteRenderer spriteRenderer;
    private Sprite notRollingSprite;
    public Sprite supriseSprite;
    public Sprite rollingSprite;

    // Start is called before the first frame update
    void Start()
    {
        lastZRotation = transform.rotation.eulerAngles.z;
        spriteRenderer = GetComponent<SpriteRenderer>();
        notRollingSprite = spriteRenderer.sprite;
    }

    private void Update()
    {
        float currentZRotation = transform.rotation.eulerAngles.z;
        if(isSurprise){
            spriteRenderer.sprite = supriseSprite;
        }
        else if (isTouchingPlatform) {
            spriteRenderer.sprite = rollingSprite;
        }else{
            transform.rotation = Quaternion.Euler(0, 0, 0);
            spriteRenderer.sprite = notRollingSprite;
        }
        lastZRotation = currentZRotation;
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Square" || collision.gameObject.name == "Platform Trap Art")
        {
            isTouchingPlatform = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Square" || collision.gameObject.name == "Platform Trap Art")
        {
            isTouchingPlatform = false;
        }
    }

}
