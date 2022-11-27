using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    private Renderer spriteRend;

    // Start is called before the first frame update
    void Start()
    {
        spriteRend = GetComponent<Renderer>();
        // Set colour to be 0 when the game starts
        spriteRend.material.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        StartCoroutine(FadeTo(1.0f, 1.0f));
    }

    private IEnumerator OnMouseExit()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(FadeTo(0.0f, 1.0f));
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = spriteRend.material.color.a;

        // For every smol unit of time
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            // Increments/ decrements the alpha value and sets it to be the new colour of the sprite
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            spriteRend.material.color = newColor;

            // Wait for the next frame and continue execution from this line
            yield return null;
        }
    }
}
