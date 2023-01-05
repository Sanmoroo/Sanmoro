using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Vrienden gevonden: " + 0;
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Vrienden gevonden: " + score.ToString();
    }
}
