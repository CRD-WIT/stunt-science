using UnityEngine;
using UnityEngine.SceneManagement;


public class BackToMainMenu : MonoBehaviour
{
    public GameObject blurPanel, dimension;
    bool pauseFlag = true, dimensionFlag;
    SimulationManager SM = new SimulationManager();
    public void BackToMenu()
    {
        SceneManager.LoadScene("GameMenu");
    }
    public void PauseGame()
    {
        if(pauseFlag){
            Time.timeScale = 0;
            blurPanel.SetActive(true);
            if(dimension.activeSelf){
                dimensionFlag = true;
                dimension.SetActive(false);
            }
            pauseFlag =false;
        }
        else{
            Time.timeScale = 1;
            blurPanel.SetActive(false);
            if(dimensionFlag){
                dimension.SetActive(true);
            }
            pauseFlag =true;
        }
    }
    public void Restart()
    {
        blurPanel.SetActive(false);
    }
}
