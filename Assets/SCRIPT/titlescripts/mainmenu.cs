using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class mainmenu : MonoBehaviour
{
   
   
    
       int startready;
    public Button gamestart;
    public GameObject proftext;
    public GameObject createtext;
    int account;
    public AudioSource button1;
    public GameObject transitE;
    public GameObject soundsettings;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(entrancetransit());
    }

    // Update is called once per frame
    public void Update()
    {
        account = PlayerPrefs.GetInt ("account");
         startready = PlayerPrefs.GetInt ("startbutton");
        if(startready == 0)
        
            gamestart.gameObject.SetActive(false);
            else
            gamestart.gameObject.SetActive(true);
        if (account == 1)
        {
            proftext.SetActive(true);
            createtext.SetActive(false);
        }
         if (account == 0)
        {
            proftext.SetActive(false);
            createtext.SetActive(true);
        }   
        
    }
    public void StartGame()
    {
        SceneManager.LoadScene("GameLevel");
        button1.Play();

    }
    public void gotoProfile()
    {
        SceneManager.LoadScene("ProfileScene");
        button1.Play();
        
       


    }
    public void settings()
    {
        button1.Play();
    }
    public void resetprefs()

    {
        //startbutton
        PlayerPrefs.SetInt("startbutton", 0);
        //profilescreen
        PlayerPrefs.SetInt("stats", 0);
        //playerslection
        PlayerPrefs.SetInt("sex", 0);
        //createcharacterscreen
        PlayerPrefs.SetInt("createscreen", 0);
        //profile and createprofile button activation
        PlayerPrefs.SetInt("account", 0);
        //level progress to unlock next level
        PlayerPrefs.SetInt("level", 0);
        //velocitylevel easy mode stars
        PlayerPrefs.SetInt("VstarE", 0);
        //total stars
        PlayerPrefs.SetInt("mystars", 0);
        //total coins
        PlayerPrefs.SetInt("mycoins", 0);
        //headgear equip
        PlayerPrefs.SetInt("headgear", 0);
        //shouldergear equip
        PlayerPrefs.SetInt("shouldergear", 0);
        //armgear euip
        PlayerPrefs.SetInt("armgear", 0);
        //leggear equip
        PlayerPrefs.SetInt("leggear", 0);
        //headgear item no 0, sold or not
        PlayerPrefs.SetInt("HG0", 0);
        //headgear item no 1, sold or not
        PlayerPrefs.SetInt("HG1", 0);
        //shouldergear item no 0, sold or not
        PlayerPrefs.SetInt("SG0", 0);
        //shouldergear item no 1, sold or not
        PlayerPrefs.SetInt("SG1", 0);
        //armgear item no 0, sold or not
        PlayerPrefs.SetInt("AG0", 0);
        //armgear item no 1, sold or not
        PlayerPrefs.SetInt("AG1", 0);
        //leggear item no 0, sold or not
        PlayerPrefs.SetInt("LG0", 0);
        //leggear item no 1, sold or not
        PlayerPrefs.SetInt("LG1", 0);
    }
    IEnumerator entrancetransit()
    {
        yield return new WaitForSeconds(1);
        transitE.SetActive(false);
    }
    public void showsettings()
    {
        soundsettings.SetActive(true);
    }
    public void exitsettings()
    {
        soundsettings.SetActive(false);
        button1.Play();
    }
}
