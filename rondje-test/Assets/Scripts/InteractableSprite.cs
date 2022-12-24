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
    public GazeAware _gazeAware;
    // TODO: Set this per sprite
    private DateTime prevTriggered;
    private bool interactionReady = true;

    // Overridable in child classes
    public abstract float TimeToFade { get; }
    public abstract string AttachedAnimation { get; }
    public abstract string AttachedSound { get; }
    public abstract string AttachedTrigger { get; }

    // Start is called before the first frame update
    public void Start()
    {
        // Initializes gaze data provider with default settings.
        TobiiAPI.Start(null);
       
        spriteRend = GetComponent<Renderer>();
        anim = gameObject.GetComponent<Animator>();
        aud = FindObjectOfType<AudioManager>();
        _gazeAware =  GameObject.Find(AttachedTrigger).GetComponent<GazeAware>();

        Debug.Log(GameObject.Find(AttachedTrigger));

        // Set alpha to be 0 when the game starts
        spriteRend.material.color = new Color(1, 1, 1, 0);

        // Make sure when the game initially starts we can trigger the fade.
        prevTriggered = DateTime.Now.AddSeconds(-10);
    }

    // Update is called once per frame
    void Update()
    {
        if (_gazeAware.HasGazeFocus)
        {
            StartInteraction();
        }
    }

    public void StartInteraction()
    {
        // If enough time has elapsed this check will pass
        if (!CanBeTriggered()) 
            return;

        StartCoroutine(FadeTo(1.0f, TimeToFade));
        PlayAnimation();
        PlaySound();
        interactionReady = false;

        StartCoroutine(ResetInteraction());
    }

    /// <summary>
    /// After a delay, resets the last time the interaction was triggered and fades out the sprite, then sets the interaction to be ready again.
    /// </summary>
    /// <returns></returns>
    public IEnumerator ResetInteraction()
    {
        if (!interactionReady)
        {
            yield return new WaitForSeconds(7);
            prevTriggered = DateTime.Now;
            StartCoroutine(FadeTo(0.0f, TimeToFade));
            // Set fadeReady back to true once the sprite has faded out so we can retrigger.
            interactionReady = true;
        }
    }

    public void PlayAnimation()
    {
        // Ensures the animation only plays once per fade
        if (interactionReady)
        {
            anim.Play(AttachedAnimation, 0, 0f);
        }
    }

    public void PlaySound()
    {
        if (interactionReady)
        {
            aud.Play(AttachedSound);
        }
    }

    /// <summary>
    /// Returns true if last fade happened more than 10 seconds ago, false if not
    /// </summary>
    /// <returns>boolean</returns>
    public bool CanBeTriggered()
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
