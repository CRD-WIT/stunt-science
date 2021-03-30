using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class profile : MonoBehaviour
{
    public Text ign;
    public Text gender;
    public Toggle velocity;
    public Toggle acceleration;
    public Toggle freefall;
    public Text mystar;
    public Text mycoin;
    
    string playername;
    public GameObject profilestatus;
    public GameObject createcharacter;
   

    public InputField entername;
    public GameObject profilescreen;
   
    
    int CreateChar;
    public int status;
    int genderselect;
    int levelprogress;
    int star;
    int coin;
    public GameObject transitE;
    public AudioSource buttonsfx1;
    public AudioSource buttonsfx2;


    // Start is called before the first frame update
    void Start()
    {
        star = PlayerPrefs.GetInt("mystars");
        coin = PlayerPrefs.GetInt("mycoins");
        StartCoroutine(entrancetransit());

        

       
        levelprogress = PlayerPrefs.GetInt ("level");
    }

    // Update is called once per frame
    void Update()
    {
        mystar.text = star.ToString("F0");
        mycoin.text = coin.ToString("F0");


        status = PlayerPrefs.GetInt ("stats");
        genderselect = PlayerPrefs.GetInt ("sex");
        CreateChar = PlayerPrefs.GetInt ("createscreen");
        playername = entername.text;
        ign.text = PlayerPrefs.GetString("name");
        if(CreateChar == 1)
        createcharacter.SetActive(false);
        else
        createcharacter.SetActive(true);
        if(status == 1)
        profilestatus.SetActive(true);
        else
        profilestatus.SetActive(false);
        if (genderselect == 1)
        gender.text = "female";
        else
        gender.text = "male";

        if (levelprogress > 0)
        {
        velocity.isOn = true;
        }
        if (levelprogress > 1)
       {
           acceleration.isOn = true;
       }
        if (levelprogress > 2)
        {
            freefall.isOn = true;
        }

        

        
    }
    public void shop()
    {
        PlayerPrefs.SetInt("lastscene", 1);
        SceneManager.LoadScene("ShopScene");
        buttonsfx1.Play();
    }
    
    public void save()
    {
        //SceneManager.LoadScene("MenuScene");
       
        PlayerPrefs.SetInt ("startbutton", 1);
        PlayerPrefs.SetInt ("createscreen", 1);
        PlayerPrefs.SetString("name", playername);
        PlayerPrefs.SetInt ("account", 1);
         PlayerPrefs.SetInt("stats", 1);
         buttonsfx2.Play();
       
        //PlayerPrefs.SetInt ("sex", 0);

    }
    public void gotolevel()
    {
        
        SceneManager.LoadScene("GameLevel");
        buttonsfx2.Play();
        /*PlayerPrefs.SetInt ("startbutton", 0);
        PlayerPrefs.SetInt ("createscreen", 0);
        PlayerPrefs.SetInt ("stats", 0);
        PlayerPrefs.SetInt ("level", 0);*/
    }
    public void maleselect()
    {
       PlayerPrefs.SetInt ("sex", 0);
       buttonsfx2.Play();
    }
     public void femaleselect()
    {
        PlayerPrefs.SetInt ("sex", 1);
        buttonsfx2.Play();
    }
    public void back()
    {
        SceneManager.LoadScene("MenuScene");
        buttonsfx1.Play();
       
       /*PlayerPrefs.SetInt ("startbutton", 0);
        PlayerPrefs.SetInt ("createscreen", 0);
        PlayerPrefs.SetInt ("stats", 0);
        PlayerPrefs.SetInt ("level", 0);*/
    }
    IEnumerator entrancetransit()
    {
        yield return new WaitForSeconds(1);
        transitE.SetActive(false);
    }
}
