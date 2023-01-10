using Tobii.Gaming;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private GazeAware gazeAware;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        TobiiAPI.Start(null);
        gazeAware = GameObject.Find("PlayButtonTrigger").GetComponent<GazeAware>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gazeAware.HasGazeFocus)
        {
            SceneManager.LoadSceneAsync("Zjacky_Art_Scene", LoadSceneMode.Additive);
        }
    }

    private void OnMouseEnter()
    {
        Debug.Log("hello");
        SceneManager.LoadSceneAsync("Zjacky_Art_Scene", LoadSceneMode.Additive);
    }
}
