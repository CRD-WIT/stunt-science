using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// using Firebase;

public class SceneNavigator : MonoBehaviour
{

    public GameObject loadingScreen;
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
    public void start()
    {
        // Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        // {
        //     var dependencyStatus = task.Result;
        //     if (dependencyStatus == Firebase.DependencyStatus.Available)
        //     {
        //         // Create and hold a reference to your FirebaseApp,
        //         // where app is a Firebase.FirebaseApp property of your application class.
        //         FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;

        //         // Set a flag here to indicate whether Firebase is ready to use by your app.
        //     }
        //     else
        //     {
        //         UnityEngine.Debug.LogError(System.String.Format(
        //           "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        //         // Firebase Unity SDK is not safe to use here.
        //     }
        // });

    }
    public void GotoSceneByName(string name)
    {
        // Always set timescale to default value.
        Time.timeScale = 1;
        string activeName = PlayerPrefs.GetString("lastScene");
        PlayerPrefs.SetString("lastScene", activeName);
        StartCoroutine(LoadSceneAsync(name));


    }

    public void BackToPreviousScene()
    {
        string lastName = PlayerPrefs.GetString("lastScene");
        Debug.Log(lastName);
        SceneManager.LoadScene(lastName);
    }
}
