using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    private int score = 0;
    private int maxScore = 6;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = $"Vrienden gevonden: {score} / {maxScore}";
    }

    public void UpdateScore()
    {
        score += 1;
        scoreText.text = $"Vrienden gevonden: {score} / {maxScore}";
    }
}
