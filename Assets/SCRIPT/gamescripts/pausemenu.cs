using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class pausemenu : MonoBehaviour
{
   public AudioSource buttonsfx1;
    public AudioSource buttonsfx2;
    public void Update()
    {
       
    }
   

    public GameObject pausemenuscreen;
    
    public void PauseGame()
    {
        Time.timeScale = 0f;
        pausemenuscreen.SetActive(true);
        buttonsfx1.Play();

    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausemenuscreen.SetActive(false);
        buttonsfx1.Play();
    }
    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
        buttonsfx1.Play();
    }
    public void shop()
   
   {
       Time.timeScale = 1f;
       PlayerPrefs.SetInt("lastscene", 2);
       SceneManager.LoadScene("ShopScene");
       buttonsfx2.Play();
   }

}
