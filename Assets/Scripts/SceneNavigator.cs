using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    // Start is called before the first frame update
    public string sceneDestination;
    public bool navigateOnStart;
    public GameObject loadingScreen;
    public float navigationDelay;
    void Start()
    {
        if (navigateOnStart && sceneDestination != "")
        {
            GotoScene(sceneDestination);
        }
    }

    public void GotoScene(string destination)
    {
        StartCoroutine(LoadSceneDelay(destination));
    }

    IEnumerator LoadSceneDelay(string destination)
    {
        yield return new WaitForSeconds(navigationDelay);
        StartCoroutine(LoadSceneAsync(destination));
    }


    IEnumerator LoadSceneAsync(string levelName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(levelName);

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / .9f);
            Debug.Log(op.progress);
            if (loadingScreen != null)
            {
                loadingScreen.SetActive(true);
            }
            yield return null;

        }
    }

}
