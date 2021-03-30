using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public actionCount theAct;
    int mystar;

    public GameObject star1;
    public GameObject star2;
    public GameObject star3;
    public GameObject good;
    public GameObject great;
    public GameObject perfect;
    public GameObject starscreen;
    public Button taptocon;
    public Button replay;
    int scorestar;
    bool scoreready = true;
    public AudioSource starfx;

    public AudioSource victorysfx;
    public AudioSource bgm;

    public AudioSource coinsfx;
    public Text mycoin;
    int coin;
    public bool coinAvail = true;
    // Start is called before the first frame update
    void Start()
    {
        theAct = FindObjectOfType<actionCount>();
        coin = PlayerPrefs.GetInt("mycoins");
        mystar = PlayerPrefs.GetInt("VstarE");
    }

    // Update is called once per frame
    void Update()
    {
        scorestar = theAct.actionAttempt;
        mycoin.text = coin.ToString("F0");

        
        
    }
    public void finalstar()
    {
        StartCoroutine(scoringstar());
        if(scorestar > mystar)
        {
           PlayerPrefs.SetInt("VstarE", scorestar);
        }
    }
    IEnumerator scoringstar()
    {
        if (scoreready)
        {
            scoreready = false;
             yield return new WaitForSeconds(3);
             starscreen.SetActive(true);
             victorysfx.Play();
             bgm.Stop();
             if(scorestar > 0 )
            {
                star1.SetActive(true);
                starfx.Play(0);
                good.SetActive(true);
            }
            yield return new WaitForSeconds(1);
            if(scorestar > 1 )
            {
                starfx.Play(0);
                good.SetActive(false);
                great.SetActive(true);
                star2.SetActive(true);
            }
            yield return new WaitForSeconds(1);
            if(scorestar > 2)
            {
                starfx.Play(0);
                great.SetActive(false);
                perfect.SetActive(true);
                star3.SetActive(true);
               
            }
            yield return new WaitForSeconds(1);
            taptocon.gameObject.SetActive(true);
            replay.gameObject.SetActive(true);

        }

        
    }
    public void gotolevel()
    {
        SceneManager.LoadScene("GameLevel");
    }
    public void replaylevel()
    {
        SceneManager.LoadScene("LevelOne");
    }
    
    public void addingoOneCoin()
    {
        if(coinAvail)
        {
            StartCoroutine(addCoin());
            coinAvail = false;
        }
    }
    IEnumerator addCoin()
    {
        yield return new WaitForSeconds(1);
        coin += 1;
        PlayerPrefs.SetInt("mycoins", coin);
        coinsfx.Play();
        

    }

}