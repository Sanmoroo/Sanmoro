using System;
using Tobii.Gaming;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private GazeAware gazeAware;
    private static DateTime? gazeInitiallyTriggered = null;
    private static DateTime? timeincrementAllowed = null;
    private static float gazeAwareTimer;
    private static float timeToTriggerGame;
    private static float timeBeforeTimerReset;

    // Start is called before the first frame update
    void Start()
    {
        TobiiAPI.Start(null);
        gazeAware = GameObject.Find("PlayButtonTrigger").GetComponent<GazeAware>();

        // Change this number to decide how long the menu should be gazed at before the game triggers.
        timeToTriggerGame = 4f;

        // Change this number to decide how long timer should stack before being reset.
        timeBeforeTimerReset = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if (gazeAware.HasGazeFocus)
        {
            // Set intial values for the timestamp that the gaze was first detected and the length of time we're going to be
            // incrementing the gazeAwareTimer after that
            if (!gazeInitiallyTriggered.HasValue)
            {
                gazeInitiallyTriggered = DateTime.Now;
                timeincrementAllowed = gazeInitiallyTriggered.Value.AddSeconds(timeBeforeTimerReset);
            }

            // If we haven't gone over the allowance (time since first triggering gaze detection), increment the timer.
            if (!(DateTime.Now > timeincrementAllowed))
            {
                gazeAwareTimer += Time.deltaTime;

            } else
            // We've gone over the allowance, reset all of our timer variables.
            {
                gazeInitiallyTriggered = null;
                timeincrementAllowed = null;
                gazeAwareTimer = 0f;
            }

            // The user has looked at the menu for long enough, trigger scene change
            if (gazeAwareTimer >= timeToTriggerGame)
            {
                SceneManager.LoadSceneAsync("Zjacky_Art_Scene", LoadSceneMode.Single);
            }
        }
    }

    private void OnMouseEnter()
    {
        SceneManager.LoadSceneAsync("Zjacky_Art_Scene", LoadSceneMode.Single);
    }
}
