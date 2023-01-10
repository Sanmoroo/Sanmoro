using Tobii.Gaming;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private GazeAware gazeAware;

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
            LoadGameScene();
        }
    }

    private void OnMouseEnter()
    {
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        SceneManager.LoadSceneAsync("Zjacky_Art_Scene", LoadSceneMode.Single);
    }
}
