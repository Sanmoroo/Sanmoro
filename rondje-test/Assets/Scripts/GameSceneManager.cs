using System.Collections;
using TMPro;
using Tobii.Gaming;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;
    private SceneLoader sceneLoader;
    private AudioManager aud;

    // Presence tracking
    private float userNotPresentTimer;
    private float allowedAbsence;
    private bool hasTriggered;

    // Score
    [SerializeField]
    private TextMeshProUGUI scoreText;
    private int score = 0;
    private int maxScore = 6;
    private int waitBeforeMenu;

    public static GameSceneManager Instance()
    {
        return instance;
    }

    void Awake()
    {
        // Initializes gaze data provider with default settings.
        TobiiAPI.Start(null);

        // Make sure there is an GameSceneManager object initialized.
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
        // Initializes gaze data provider with default settings.
        TobiiAPI.Start(null);

        sceneLoader = FindObjectOfType<SceneLoader>();
        aud = FindObjectOfType<AudioManager>();

        // Change number to decide how long the user is allowed to be absent before being sent back to menu
        allowedAbsence = 10f;

        // Change number to decide how long the game should wait after the user finds all friends before being sent
        // back to menu.
        waitBeforeMenu = 23;

        scoreText.text = $" {score}/{maxScore}";
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

            if ((userNotPresentTimer > allowedAbsence) && !hasTriggered)
            {
                StartCoroutine(sceneLoader.LoadSceneAsync("Menu", LoadSceneMode.Single));
                hasTriggered = true;
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
        scoreText.text = $" {score}/{maxScore}";

        if (score >= maxScore)
        {
            StartCoroutine(SendBackToMenu());
        }
    }

    private IEnumerator SendBackToMenu()
    {
        yield return new WaitForSeconds(14);
        aud.Play("Outro");
        yield return new WaitForSeconds(waitBeforeMenu);
        StartCoroutine(sceneLoader.LoadSceneAsync("Menu", LoadSceneMode.Single));
    }
}
