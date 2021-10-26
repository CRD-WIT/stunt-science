using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeartManager : MonoBehaviour
{
    //public GameObject[] hearts;
    public AudioSource bgm;


    public AudioSource Gameoversfx;
    public int life;
    public GameObject gameOverBG, startBG;
    public bool losslife;
    public GameObject heartItem;

    // Start is called before the first frame update
    void Start()
    {
        //startbgentrance();
        life = PlayerPrefs.GetInt("Life");
        PlayerPrefs.SetInt("Life", life);
    }
    public void DestroyHearts()
    {
        foreach (Transform item in transform)
        {
            GameObject.Destroy(item.gameObject);
        }
    }
    public void losinglife()
    {
        if (losslife == false)
        {
            life -= 1;
            losslife = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.childCount < life)
        {
            for (int i = 0; i < 1; i++)
            {
                var heart = Instantiate(heartItem, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                heart.transform.parent = transform;
                heart.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
        }
        else
        {
            int diff = transform.childCount - life;
            for (int i = 0; i < diff; i++)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }

            if (life == 0)
            {

                Time.timeScale = 0.4f;
                Debug.Log("Gameover triggered!");
                StartCoroutine(gameover());
            }
        }

    }
    // public void actionreset()
    // {
    //     // TODO: Get data from playerprefs
    //     life = 3;
    //     PlayerPrefs.SetInt("Life", life);
    // }
    IEnumerator gameover()
    {
        bgm.Stop();
        Gameoversfx.Play();
        Debug.Log($"Game over triggered!");
        yield return new WaitForSeconds(1);
        StartCoroutine(endBGgone());
        //yield return new WaitForSeconds(2);
        // SceneManager.LoadScene("LevelOne");
        Time.timeScale = 1f;
    }
    public IEnumerator endBGgone()
    {
        gameOverBG.SetActive(true);
        yield return new WaitForSeconds(3f);
        //gameOverBG.SetActive(false);
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
    public void ReduceLife()
    {
        if (losslife == false)
        {
            life = PlayerPrefs.GetInt("Life");
            life -= 1;
            PlayerPrefs.SetInt("Life", life);
            losslife = true;
        }
    }
    public void reloadScene()
    {
        life = 3;
        PlayerPrefs.SetInt("Life", life);
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void LoadLevelsMenu()
    {
        SceneManager.LoadScene("LevelSelectV2");
    }
    
   

}