using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInit : MonoBehaviour
{
    public string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString("lastScene", sceneName);
        Debug.Log($"Last scene visited: {sceneName}");
    }
}
