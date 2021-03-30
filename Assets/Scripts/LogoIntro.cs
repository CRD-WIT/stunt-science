using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LogoIntro : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EndLogoIntro());
    }

    IEnumerator EndLogoIntro()
    {
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("GameTitle");
    }
}
