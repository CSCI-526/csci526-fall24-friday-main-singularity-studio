using System.Collections;
using UnityEngine;

public class RotationIndicator : MonoBehaviour
{
    public Color flashColor = Color.green;
    public float totalFlashDuration = 2.0f;
    public int flashCount = 4;
    private SpriteRenderer[] spriteRenderers;
    private Color[] originalColors;
    private bool isFlashing = false;

    void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        originalColors = new Color[spriteRenderers.Length];

        // store the original color for the current object and its direct children
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            originalColors[i] = spriteRenderers[i].color;
        }

        // add to event
        SceneRotation sceneRotation = FindObjectOfType<SceneRotation>();
        sceneRotation.OnFlashBeforeRotation += StartFlashing;
    }

    void StartFlashing()
    {
        if (!isFlashing)
        {
            StartCoroutine(FlashRoutine());
        }
    }

    private IEnumerator FlashRoutine()
    {
        isFlashing = true;
        float flashDuration = totalFlashDuration / flashCount;  // Duration of each flash cycle
        float elapsedTime = 0f;

        // flash flashcount amount of time
        for (int i = 0; i < flashCount; i++)
        {
            float flashStartTime = Time.time;

            while (elapsedTime < flashDuration)
            {
                float lerpTime = Mathf.PingPong((Time.time - flashStartTime) * 2f, 1);
                Color color = Color.Lerp(originalColors[0], flashColor, lerpTime);

                // flash color to the parent and its direct children (1 level deep)
                for (int j = 0; j < spriteRenderers.Length; j++)
                {
                    spriteRenderers[j].color = Color.Lerp(originalColors[j], flashColor, lerpTime);
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0f;
        }
        
        // revert after changing
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = originalColors[i];
        }

        isFlashing = false;
    }
}
