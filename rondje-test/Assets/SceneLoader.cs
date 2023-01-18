using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Set as our CrossFade animator in the inspector.
    public Animator transition;

    public IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
    {
        transition.SetTrigger("Start");

        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);

        while (!loadingOperation.isDone)
        {
            yield return null;
        }
        
        if (loadingOperation.isDone)
        {
            transition.SetTrigger("End");
        }
    }
}
