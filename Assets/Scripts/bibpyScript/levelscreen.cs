using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class levelscreen : MonoBehaviour
{
    public Button lvl1hard;
    public Button lvl1dif;
    public Button lvl2eas;
    public Button lvl2hard;
    public Button lvl2dif;
    public Button lvl3eas;
    public Button lvl3hard;
    public Button lvl3dif;
    public Button lvl4eas;
    public Button lvl4hard;
    public Button lvl4dif;
    public Button lvl5eas;
    public Button lvl5hard;
    public Button lvl5dif;
    public Button lvl6eas;
    public Button lvl6hard;
    public Button lvl6dif;
    public Button lvl7eas;
    public Button lvl7hard;
    public Button lvl7dif;
    public Button lvl8eas;
    public Button lvl8hard;
    public Button lvl8dif;
    public Button lvl9eas;
    public Button lvl9hard;
    public Button lvl9dif;
    public Button lvl10eas;
    public Button lvl10hard;
    public Button lvl10dif;
    public Button lvl11eas;
    public Button lvl11hard;
    public Button lvl11dif;
   
    public GameObject lock1H;
    public GameObject lock1D;
    public GameObject lock2E;
    public GameObject lock2H;
    public GameObject lock2D;
    public GameObject lock3E;
    public GameObject lock3H;
    public GameObject lock3D;
    public GameObject lock4E;
    public GameObject lock4H;
    public GameObject lock4D;
    public GameObject lock5E;
    public GameObject lock5H;
    public GameObject lock5D;
    public GameObject lock6E;
    public GameObject lock6H;
    public GameObject lock6D;
    public GameObject lock7E;
    public GameObject lock7H;
    public GameObject lock7D;
    public GameObject lock8E;
    public GameObject lock8H;
    public GameObject lock8D;
    public GameObject lock9E;
    public GameObject lock9H;
    public GameObject lock9D;
    public GameObject lock10E;
    public GameObject lock10H;
    public GameObject lock10D;
    public GameObject lock11E;
    public GameObject lock11H;
    public GameObject lock11D;
    int lvllock;
    public GameObject transitE;
    public AudioSource buttonsfx;
    public AudioSource buttonsfx1;



    
    

    // Start is called before the first frame update
    void Start()
    {
        lvllock = PlayerPrefs.GetInt ("level");  
        StartCoroutine(entrancetransit());
        
    }
   

    // Update is called once per frame
    void Update()
    {
       
        // if(lvllock < 1)
        // lvl1hard.interactable = false;
        // else
        // lvl1hard.interactable = true;
        if(lvllock < 2)
        lvl1dif.interactable = false;
        else
        lvl1dif.interactable = true;
        if(lvllock < 3)
        lvl2eas.interactable = false;
        else
        lvl2eas.interactable = true;
        if(lvllock < 4)
        lvl2hard.interactable = false;
        else
        lvl2hard.interactable = true;
         if(lvllock < 5)
        lvl2dif.interactable = false;
        else
        lvl2dif.interactable = true;
        if(lvllock < 6)
        lvl3eas.interactable = false;
        else
        lvl3eas.interactable = true;
        if(lvllock < 7)
        lvl3hard.interactable = false;
        else
        lvl3hard.interactable = true;
         if(lvllock < 8)
        lvl3dif.interactable = false;
        else
        lvl3dif.interactable = true;
        if(lvllock < 9)
        lvl4eas.interactable = false;
        else
        lvl4eas.interactable = true;
        if(lvllock < 10)
        lvl4hard.interactable = false;
        else
        lvl4hard.interactable = true;
         if(lvllock < 11)
        lvl4dif.interactable = false;
        else
        lvl4dif.interactable = true;
        if(lvllock < 12)
        lvl5eas.interactable = false;
        else
        lvl5eas.interactable = true;
        if(lvllock < 13)
        lvl5hard.interactable = false;
        else
        lvl5hard.interactable = true;
         if(lvllock < 14)
        lvl5dif.interactable = false;
        else
        lvl5dif.interactable = true;
        if(lvllock < 15)
        lvl6eas.interactable = false;
        else
        lvl6eas.interactable = true;
        if(lvllock < 16)
        lvl6hard.interactable = false;
        else
        lvl6hard.interactable = true;
         if(lvllock < 17)
        lvl6dif.interactable = false;
        else
        lvl6dif.interactable = true;
        if(lvllock < 18)
        lvl7eas.interactable = false;
        else
        lvl7eas.interactable = true;
        if(lvllock < 19)
        lvl7hard.interactable = false;
        else
        lvl7hard.interactable = true;
         if(lvllock < 20)
        lvl7dif.interactable = false;
        else
        lvl7dif.interactable = true;
        if(lvllock < 21)
        lvl8eas.interactable = false;
        else
        lvl8eas.interactable = true;
        if(lvllock < 22)
        lvl8hard.interactable = false;
        else
        lvl8hard.interactable = true;
         if(lvllock < 23)
        lvl8dif.interactable = false;
        else
        lvl8dif.interactable = true;
        if(lvllock < 24)
        lvl9eas.interactable = false;
        else
        lvl9eas.interactable = true;
        if(lvllock < 25)
        lvl9hard.interactable = false;
        else
        lvl9hard.interactable = true;
         if(lvllock < 26)
        lvl9dif.interactable = false;
        else
        lvl9dif.interactable = true;
        if(lvllock < 27)
        lvl10eas.interactable = false;
        else
        lvl10eas.interactable = true;
        if(lvllock < 28)
        lvl10hard.interactable = false;
        else
        lvl10hard.interactable = true;
         if(lvllock < 29)
        lvl10dif.interactable = false;
        else
        lvl10dif.interactable = true;
        if(lvllock < 30)
        lvl11eas.interactable = false;
        else
        lvl11eas.interactable = true;
        if(lvllock < 31)
        lvl11hard.interactable = false;
        else
        lvl11hard.interactable = true;
         if(lvllock < 32)
        lvl11dif.interactable = false;
        else
        lvl11dif.interactable = true;


        if(lvllock < 1)
        lock1H.SetActive(true);
        else
        lock1H.SetActive(false);
        if(lvllock < 2)
        lock1D.SetActive(true);
        else
        lock11D.SetActive(false);
        if(lvllock < 3)
        lock2E.SetActive(true);
        else
        lock2E.SetActive(false);
        if(lvllock < 4)
        lock2H.SetActive(true);
        else
        lock2H.SetActive(false);
        if(lvllock < 5)
        lock2D.SetActive(true);
        else
        lock2D.SetActive(false);
        if(lvllock < 6)
        lock3E.SetActive(true);
        else
        lock3E.SetActive(false);
        if(lvllock < 7)
        lock3H.SetActive(true);
        else
        lock3H.SetActive(false);
        if(lvllock < 8)
        lock3D.SetActive(true);
        else
        lock3D.SetActive(false);
         if(lvllock < 9)
        lock4E.SetActive(true);
        else
        lock4E.SetActive(false);
        if(lvllock < 10)
        lock4H.SetActive(true);
        else
        lock4H.SetActive(false);
        if(lvllock < 11)
        lock4D.SetActive(true);
        else
        lock4D.SetActive(false);
        if(lvllock < 12)
        lock5E.SetActive(true);
        else
        lock5E.SetActive(false);
        if(lvllock < 13)
        lock5H.SetActive(true);
        else
        lock5H.SetActive(false);
        if(lvllock < 14)
        lock5D.SetActive(true);
        else
        lock5D.SetActive(false);
        if(lvllock < 15)
        lock6E.SetActive(true);
        else
        lock6E.SetActive(false);
        if(lvllock < 16)
        lock6H.SetActive(true);
        else
        lock6H.SetActive(false);
        if(lvllock < 17)
        lock11D.SetActive(true);
        else
        lock11D.SetActive(false);
        if(lvllock < 18)
        lock6D.SetActive(true);
        else
        lock6D.SetActive(false);
        if(lvllock < 19)
        lock7E.SetActive(true);
        else
        lock7E.SetActive(false);
        if(lvllock < 20)
        lock7H.SetActive(true);
        else
        lock7H.SetActive(false);
        if(lvllock < 21)
        lock7D.SetActive(true);
        else
        lock7D.SetActive(false);
        if(lvllock < 22)
        lock8E.SetActive(true);
        else
        lock8E.SetActive(false);
        if(lvllock < 23)
        lock8H.SetActive(true);
        else
        lock8H.SetActive(false);
        if(lvllock < 24)
        lock8D.SetActive(true);
        else
        lock8D.SetActive(false);
         if(lvllock < 25)
        lock9E.SetActive(true);
        else
        lock9E.SetActive(false);
        if(lvllock < 26)
        lock9H.SetActive(true);
        else
        lock9H.SetActive(false);
        if(lvllock < 27)
        lock9D.SetActive(true);
        else
        lock9D.SetActive(false);
         if(lvllock < 28)
        lock11E.SetActive(true);
        else
        lock11E.SetActive(false);
        if(lvllock < 29)
        lock11H.SetActive(true);
        else
        lock10H.SetActive(false);
        if(lvllock < 30)
        lock10D.SetActive(true);
        else
        lock10D.SetActive(false);
         if(lvllock < 31)
        lock11E.SetActive(true);
        else
        lock11E.SetActive(false);
        if(lvllock < 32)
        lock11H.SetActive(true);
        else
        lock11H.SetActive(false);
       
       
        
        
    }
    public void gotoshop()
    {
       SceneManager.LoadScene("ShopScene");
       buttonsfx1.Play();
    }
    public void gotomenu()
    {
       SceneManager.LoadScene("MenuScene");
       buttonsfx1.Play();
       //PlayerPrefs.SetInt ("level", 0);
        


    }
    public void level1()
    {
        SceneManager.LoadScene("LevelOneEasy");
        buttonsfx.Play();
        
    }
    public void level2()
    {
         SceneManager.LoadScene("LevelOneMedium");
        buttonsfx.Play();
    }
    public void level3()
    {
        buttonsfx.Play();
    }
    public void level4()
    {
        buttonsfx.Play();
    }
    public void level5()
    {
        buttonsfx.Play();
    }
    public void level6()
    {
        buttonsfx.Play();
    }
    public void level7()
    {
        buttonsfx.Play();
    }
    public void level8()
    {
        buttonsfx.Play();
    }
    public void level9()
    {
        buttonsfx.Play();
    }
    public void level10()
    {
        buttonsfx.Play();
    }
    public void level11()
    {
        buttonsfx.Play();
    }
    IEnumerator entrancetransit()
    {
        yield return new WaitForSeconds(1);
        transitE.SetActive(false);
    }
}
