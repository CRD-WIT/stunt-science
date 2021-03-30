using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Utilities : MonoBehaviour
{
    public GameObject pauseDialog;
    public Animator gameControls;
    public Animator levelOverview;
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator PauseRoutine()
    {
        // Wait for dialog to complete animation
        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 0;
    }

    IEnumerator ResumeRoutine()
    {
        Time.timeScale = 1;
        // Wait for dialog to complete animation
        yield return new WaitForSeconds(0.1f);
        pauseDialog.SetActive(false);

    }

    public void PauseLevel()
    {
        StartCoroutine(PauseRoutine());
        if (pauseDialog != null)
        {
            pauseDialog.SetActive(true);
        }
        if (gameControls != null)
        {
            gameControls.SetBool("PlayIntro", false);
        }
    }


    public void ResumeLevel()
    {
        StartCoroutine(ResumeRoutine());
    }

    public void PreviewLevel()
    {
        levelOverview.SetBool("PlayIntro", true);
    }
}
