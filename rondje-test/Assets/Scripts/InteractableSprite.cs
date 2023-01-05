using System;
using System.Collections;
using UnityEngine;
using Tobii.Gaming;

public abstract class InteractableSprite : MonoBehaviour
{
    // Won't change
    private Renderer spriteRend;
    private Animator anim;
    private AudioManager aud;
    private UIManager uiManager;
    public GazeAware gazeAware;
    private int score;

    // Overridable in child classes
    public abstract float TimeToFade { get; }
    public abstract string AttachedAnimation { get; }
    public abstract string AttachedSound { get; }
    public abstract string AttachedTrigger { get; }
    public abstract bool InteractionReady { get; set; }

    // Start is called before the first frame update
    public void Start()
    {
        // Initializes gaze data provider with default settings.
        TobiiAPI.Start(null);
       
        spriteRend = GetComponent<Renderer>();
        anim = gameObject.GetComponent<Animator>();
        aud = FindObjectOfType<AudioManager>();
        gazeAware =  GameObject.Find(AttachedTrigger).GetComponent<GazeAware>();
        uiManager = FindObjectOfType<UIManager>();
        InteractionReady = true;

        // Set alpha to be 0 when the game starts
        spriteRend.material.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (gazeAware.HasGazeFocus)
        {
            StartInteraction();
        }
    }

    public void StartInteraction()
    {
        if (InteractionReady)
        {
            StartCoroutine(FadeTo(1.0f, TimeToFade));
            PlayAnimation();
            PlaySound();
            IncrementScore();
            InteractionReady = false;
        }
    }

    public void IncrementScore()
    {
        score += 1;
        uiManager.UpdateScore(score);
    }

    public void PlayAnimation()
    {
         anim.Play(AttachedAnimation, 0, 0f);
    }

    public void PlaySound()
    {
         aud.Play(AttachedSound);
    }

    /// <summary>
    /// Fades the alpha of the sprite to a given value over a given time period.
    /// </summary>
    /// <param name="aValue">Alpha value to fade to</param>
    /// <param name="aTime">Time over which the fade should occur</param>
    /// <returns></returns>
    public IEnumerator FadeTo(float aValue, float aTime)
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
