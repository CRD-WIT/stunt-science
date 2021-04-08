using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeartManager : MonoBehaviour
{
    public GameObject[] heart;
    public AudioSource bgm;
    public AudioSource Gameoversfx;
    int life;
    public GameObject gameOverBG, startBG;
    public bool losslife;
    
    // Start is called before the first frame update
    void Start()
    {
        life = 3;
        startbgentrance();
    }

    // Update is called once per frame
    void Update()
    {
        if (life == 2)
        {
            heart[0].SetActive(false);   
        }
         if (life == 1)
        {
            heart[1].SetActive(false);   
        }
        
        if (life == 0)
        {
            heart[2].SetActive(false);
            Time.timeScale = 0.4f;
            life = 3;
            
            StartCoroutine(actionreset());  
            StartCoroutine(gameover());
        }  
    }
    IEnumerator actionreset()
    {
        yield return new WaitForSeconds(3);
        
        heart[0].SetActive(true);
        heart[1].SetActive(true);
        heart[2].SetActive(true); 
        
    }
    IEnumerator gameover()
    {
        bgm.Stop();
        Gameoversfx.Play();
        StartCoroutine(endBGgone());
       yield return new WaitForSeconds(2);
        SceneManager.LoadScene("LevelOne");
        Time.timeScale = 1f;
    }
    public IEnumerator endBGgone()
    {
        gameOverBG.SetActive(true);
        yield return new WaitForSeconds(3);
        gameOverBG.SetActive(false);
    }
     
    
    public IEnumerator startBGgone()
    {
      startBG.SetActive(true);
      yield return new WaitForSeconds(1);  
      startBG.SetActive(false);

    }
    public void startbgentrance()
    {
        StartCoroutine(startBGgone());
    }
    public void losinglife()
    {
        if (losslife == false)
        {
            life -= 1;
            losslife = true;
        }
    }
}
