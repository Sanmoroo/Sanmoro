using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Set as our CrossFade animator in the inspector.
    public Animator transition;

    public IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        loadingOperation.allowSceneActivation = false;

        while (loadingOperation.progress < 0.9f)
        {
            yield return new WaitForEndOfFrame();
        }

        transition.SetTrigger("Start");
        loadingOperation.allowSceneActivation = true;
    }
}
