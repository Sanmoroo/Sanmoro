using System.Collections;
using TMPro;
using Tobii.Gaming;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;

    // Presence tracking
    private static float userNotPresentTimer;
    private static float allowedAbsence;

    // Score
    [SerializeField]
    private TextMeshProUGUI scoreText;
    private static int score = 0;
    private static int maxScore = 6;
    private static int waitBeforeMenu; 

    void Awake()
    {
        // Initializes gaze data provider with default settings.
        TobiiAPI.Start(null);

        // Make sure there is an AudioManager object initialized.
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Change number to decide how long the user is allowed to be absent before being sent back to menu
        allowedAbsence = 5f;

        // Change number to decide how long the game should wait after the user finds all friends before being sent
        // back to menu.
        waitBeforeMenu = 5;

        scoreText.text = $"Vrienden gevonden: {score} / {maxScore}";
    }

    private void Update()
    {
        TrackUserPresence();
    }

    /// <summary>
    /// Determines whether user has been lost by the eye tracker for a sufficient amount of time to be returned to the menu.
    /// </summary>
    private void TrackUserPresence()
    {
        UserPresence userPresence = TobiiAPI.GetUserPresence();
        if (userPresence == UserPresence.NotPresent)
        {
            userNotPresentTimer += Time.deltaTime;

            if (userNotPresentTimer > allowedAbsence)
            {
                SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
            }

        }
        else if (userPresence == UserPresence.Present)
        {
            userNotPresentTimer = 0;
        }
    }

    public void UpdateScore()
    {
        score += 1;
        scoreText.text = $"Vrienden gevonden: {score} / {maxScore}";

        if (score >= maxScore)
        {
            StartCoroutine(SendBackToMenu());
        }
    }

    private IEnumerator SendBackToMenu()
    {
        yield return new WaitForSeconds(waitBeforeMenu);
        SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
    }
}
