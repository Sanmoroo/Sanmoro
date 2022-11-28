using System;
using System.Collections;
using UnityEngine;

public class Select : MonoBehaviour
{
    private DateTime prevTriggered;
    private Renderer spriteRend;
    private Boolean fadeReady;

    // Start is called before the first frame update
    void Start()
    {
        spriteRend = GetComponent<Renderer>();

        // Set alpha to be 0 when the game starts
        spriteRend.material.color = new Color(1, 1, 1, 0);

        // Make sure when the game initially starts we can trigger the fade.
        prevTriggered = DateTime.Now.AddSeconds(-10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        if (!CanBeTriggered()) 
            return;
        
        StartCoroutine(FadeTo(1.0f, 1.0f));
        fadeReady = false;
    }

    private IEnumerator OnMouseExit()
    {
        if (!fadeReady)
        {
            yield return new WaitForSeconds(7);
            prevTriggered = DateTime.Now;
            StartCoroutine(FadeTo(0.0f, 1.0f));

            // Set fadeReady back to true once the sprite has faded out so we can retrigger.
            fadeReady = true;
        }
    }

    /// <summary>
    /// Returns true if last fade happened more than 10 seconds ago, false if not
    /// </summary>
    /// <returns>boolean</returns>
    private bool CanBeTriggered()
    {
        TimeSpan timeDifference = DateTime.Now - prevTriggered;
        if (timeDifference.Seconds > 10)
            return true;

        return false;
    }

    /// <summary>
    /// Fades the alpha of the sprite to a given value over a given time period.
    /// </summary>
    /// <param name="aValue">Alpha value to fade to</param>
    /// <param name="aTime">Time over which the fade should occur</param>
    /// <returns></returns>
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
