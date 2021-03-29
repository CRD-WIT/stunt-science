using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Director : MonoBehaviour
{
    public GameObject LevelName;
    public Animator CameraAnimator;
    // public Animator ControlsAnimator;
    public GameObject StuntScript;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Level1IntroRoutine());
    }

    IEnumerator Level1IntroRoutine()
    {
        ShowLevelName();
        yield return new WaitForSeconds(1.0f);
        HideLevelName();
        yield return new WaitForSeconds(1.0f);
        ShowStuntScript();
        PlayControlsAnimator();
    }

    void ShowStuntScript()
    {
        StuntScript.SetActive(true);
    }

    void ShowLevelName()
    {
        LevelName.SetActive(true);
    }

    void HideLevelName()
    {
        LevelName.SetActive(false);
    }

    void PlayLevelOverview()
    {
        CameraAnimator.SetBool("PlayIntro", true);
    }

    void PlayControlsAnimator(){
         // ControlsAnimator.SetBool("PlayIntro", true);
    }
}
