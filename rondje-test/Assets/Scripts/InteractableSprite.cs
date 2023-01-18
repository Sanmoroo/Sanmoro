using System;
using System.Collections;
using UnityEngine;
using Tobii.Gaming;

public abstract class InteractableSprite : MonoBehaviour
{
    // Won't change
    private SpriteRenderer spriteRend;
    private Animator anim;
    private AudioManager aud;
    private GazeAware gazeAware;
    private GameObject introBox;
    private static DateTime lastTriggered;
    private static DateTime introFinished;

    // Overridable in child classes
    public abstract float TimeToFade { get; }
    public abstract string AttachedAnimation { get; }
    public abstract string AttachedSound { get; }
    public abstract string AttachedTrigger { get; }
    public abstract bool UntriggeredInteraction { get; set; }

    // Start is called before the first frame update
    public void Start()
    {
        // Get neccessary objects.
        spriteRend = GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        aud = FindObjectOfType<AudioManager>();
        gazeAware =  GameObject.Find(AttachedTrigger).GetComponent<GazeAware>();
        introBox = GameObject.Find("Intro");

        // Each sprite starts with their interaction untriggered
        UntriggeredInteraction = true;

        //TODO: Set this to something specific
        lastTriggered = DateTime.Now;

        // Change number in add seconds to increase the delay between the game starting and interactions being possible
        introFinished = DateTime.Now.AddSeconds(5);

        // Set alpha of sprites to be 0 when the game starts
        spriteRend.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (gazeAware.HasGazeFocus)
        {
            StartInteraction();
        }

        if (IntroFinished())
        {
            introBox.SetActive(false);
        }
    }

    private void OnMouseEnter()
    {
        StartInteraction();
    }

    /// <summary>
    /// Performs all the necessary actions for a player interaction.
    /// </summary>
    public void StartInteraction()
    {
        if (UntriggeredInteraction && CooldownExpired() && IntroFinished())
        {
            StartCoroutine(FadeTo(1.0f, TimeToFade));
            PlayAnimation();
            PlaySound();
            IncrementScore();
            UntriggeredInteraction = false;
            lastTriggered = DateTime.Now;
        }
    }

    public bool IntroFinished()
    {
        if (DateTime.Now > introFinished)
            return true;

        return false;
    }

    /// <summary>
    /// Change number to change to time it takes for the interactible strite to be ready again 
    /// Returns true if last fade happened more than the specified seconds ago, false if not
    /// </summary>
    public bool CooldownExpired()
    {
        TimeSpan timeDifference = DateTime.Now - lastTriggered;
        if (timeDifference.TotalSeconds > 1)
            return true;

        return false;
    }

    public void IncrementScore()
    {
        GameSceneManager.Instance().UpdateScore();
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
            spriteRend.color = newColor;

            // Wait for the next frame and continue execution from this line
            yield return null;
        }
    }
}
