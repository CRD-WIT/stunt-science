using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManagerV1 : MonoBehaviour
{
    public HeartManager theHeart;
    int mystar, scorestar, coin;
    public GameObject star1, star2, star3, good, great, perfect, starscreen;
    public Button taptocon, replay;
    bool scoreready = true;
    public AudioSource starfx, victorysfx, bgm, coinsfx;
    public Text mycoin;
    public bool coinAvail = true;
    void Start()
    {
        theHeart = FindObjectOfType<HeartManager>();
        coin = PlayerPrefs.GetInt("mycoins");
        mystar = PlayerPrefs.GetInt("VstarE");
    }

    // Update is called once per frame
    void Update()
    {
        scorestar = theHeart.life;
        mycoin.text = coin.ToString("F0");
    }
    public void finalstar()
    {
        StartCoroutine(scoringstar());
        if (scorestar > mystar)
        {  
            PlayerPrefs.SetInt("VstarE", scorestar);
        }
    }
    IEnumerator scoringstar()
    {
        if (scoreready)
        {
            scoreready = false;
            //yield return new WaitForSeconds(1);
            starscreen.SetActive(true);
            victorysfx.Play();
            bgm.Stop();
            if (scorestar > 0)
            {
                star1.SetActive(true);
                starfx.Play(0);
                good.SetActive(true);
                yield return new WaitForSeconds(0.6f);
                for (int i = 0; i < 10; i++)
                {
                    yield return new WaitForEndOfFrame();
                    coin++;
                }
            }
            yield return new WaitForSeconds(1);
            if (scorestar > 1)
            {
                starfx.Play(0);
                good.SetActive(false);
                great.SetActive(true);
                star2.SetActive(true);
                yield return new WaitForSeconds(0.6f);
                for (int i = 0; i < 10; i++)
                {
                    yield return new WaitForEndOfFrame();
                    coin++;
                }
            }
            yield return new WaitForSeconds(1);
            if (scorestar > 2)
            {
                starfx.Play(0);
                great.SetActive(false);
                perfect.SetActive(true);
                star3.SetActive(true);
                yield return new WaitForSeconds(0.6f);
                for (int i = 0; i < 10; i++)
                {
                    yield return new WaitForEndOfFrame();
                    coin++;
                }
            }
            yield return new WaitForSeconds(1);
            taptocon.gameObject.SetActive(true);
            replay.gameObject.SetActive(true);
            PlayerPrefs.SetInt("mycoins", coin);
        }
    }
    public void gotolevel()
    {
        SceneManager.LoadScene("LevelSelection");
    }
    public void replaylevel()
    {
        SceneManager.LoadScene("LevelOne");
    }
    public void addingoOneCoin()
    {
        if (coinAvail)
        {
            StartCoroutine(addCoin());
            coinAvail = false;
        }
    }
    IEnumerator addCoin()
    {
        yield return new WaitForSeconds(1);
        coin += theHeart.life * 10;
        PlayerPrefs.SetInt("mycoins", coin);
        coinsfx.Play();
    }
}
