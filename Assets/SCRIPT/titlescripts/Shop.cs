using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Shop : MonoBehaviour
{
    public GameObject transitE;
    int lastloadedscene;
    int HG1price = 1;
    int HG0price = 0;
    int SG1price = 1;
    int SG0price = 0;
    int AG1price = 1;
    int AG0price = 0;
    int LG1price = 1;
    int LG0price = 0;

    public int[] HGsold;
    
    public int[] SGsold;
    
    public int[] AGsold;
   
    public int[] LGsold;
   
    public Text HG1tag;
    public Text HG0tag;
    public Text SG1tag;
    public Text SG0tag;
    public Text AG1tag;
    public Text AG0tag;
    public Text LG1tag;
    public Text LG0tag;

    public Text mycoin;
    int coin;
    int price;
    
    public Text[] SoldHG;
    public Text[] SoldSG;
    public Text[] SoldAG;
    public Text[] SoldLG;
    
   

    public Button[] selectHG;
    public Button[] selectSG;
    public Button[] selectAG;
    public Button[] selectLG;
    public Button buybuttonHG;
    public Button buybuttonSG;
    public Button buybuttonAG;
    public Button buybuttonLG;
    public GameObject[] headgears;
    public GameObject[] shouldergears;
    public GameObject[] shouldergearspair;
    public GameObject[] armgears;
    public GameObject[] armgearspair;
    public GameObject[] leggears;
    public GameObject[] leggearspair;
    public int selectedHG;
    public int selectedSG;
    public int selectedAG;
    public int selectedLG;
    
    int buttonHG;
    int buttonSG;
    int buttonAG;
    int buttonLG;

    public GameObject[] gearshop;

    public AudioSource buttonsfx1;
    public AudioSource buttonsfx2;
   
    // Start is called before the first frame update
    void Start()
    {
        coin = PlayerPrefs.GetInt("mycoins");
        price = 1;
        StartCoroutine(entrancetransit());
        
    }

    // Update is called once per frame
    void Update()
    {
        HGsold[0] = PlayerPrefs.GetInt("HG0");
        HGsold[1] = PlayerPrefs.GetInt("HG1");
        SGsold[0] = PlayerPrefs.GetInt("SG0");
        SGsold[1] = PlayerPrefs.GetInt("SG1");
        AGsold[0] = PlayerPrefs.GetInt("AG0");
        AGsold[1] = PlayerPrefs.GetInt("AG1");
        LGsold[0] = PlayerPrefs.GetInt("LG0");
        LGsold[1] = PlayerPrefs.GetInt("LG1");

        PlayerPrefs.SetInt("mycoins", coin);
        mycoin.text = coin.ToString("F0");

        HG0tag.text = HG0price.ToString();
        HG1tag.text = HG1price.ToString();
        SG0tag.text = SG0price.ToString();
        SG1tag.text = SG1price.ToString();
        AG0tag.text = AG0price.ToString();
        AG1tag.text = AG1price.ToString();
        LG0tag.text = LG0price.ToString();
        LG1tag.text = LG1price.ToString();
        buttonHG = PlayerPrefs.GetInt("headgear");
        buttonSG = PlayerPrefs.GetInt("shouldergear");
        buttonAG = PlayerPrefs.GetInt("armgear");
        buttonLG = PlayerPrefs.GetInt("leggear");
        lastloadedscene = PlayerPrefs.GetInt("lastscene");
       
        headgears[selectedHG].SetActive(true);
        shouldergears[selectedSG].SetActive(true);
        shouldergearspair[selectedSG].SetActive(true);
        
        armgears[selectedAG].SetActive(true);
        armgearspair[selectedAG].SetActive(true);

        leggears[selectedLG].SetActive(true);
        leggearspair[selectedLG].SetActive(true);
        if (coin >= price)
        {
           buybuttonHG.interactable = true;
           buybuttonSG.interactable = true;
           buybuttonAG.interactable = true;
           buybuttonLG.interactable = true;
        }
        if (coin < price )
        {
           buybuttonHG.interactable = false;
           buybuttonSG.interactable = false;
           buybuttonAG.interactable = false;
           buybuttonLG.interactable = false;
        }
        /*if (buttonHG > 0)
        {
            selectHG[buttonHG].interactable = false;
            SoldHG[buttonHG].text = "sold";
        }
        if (buttonSG > 0)
        {
            selectSG[buttonSG].interactable = false;
            SoldSG[buttonSG].text = "sold";
        }
        if (buttonAG > 0)
        {
            selectAG[buttonAG].interactable = false;
            SoldAG[buttonAG].text = "sold";
        }
        if (buttonAG > 0)
        {
            selectLG[buttonLG].interactable = false;
            SoldLG[buttonLG].text = "sold";
        }*/
        if(HGsold[0] > 0)
        {
            HG0price = 0;
            SoldHG[0].text = "owned";
        }
        if(HGsold[1] > 0)
        {
            HG1price = 0;
            SoldHG[1].text = "owned";
        }

         if(SGsold[0] > 0)
        {
            SG0price = 0;
            SoldSG[0].text = "owned";
        }
        if(SGsold[1] > 0)
        {
            SG1price = 0;
            SoldSG[1].text = "owned";
        }

         if(AGsold[0] > 0)
        {
            AG0price = 0;
            SoldAG[0].text = "owned";
        }
        if(AGsold[1] > 0)
        {
            AG1price = 0;
            SoldAG[1].text = "owned";
        }

         if(LGsold[0] > 0)
        {
            LG0price = 0;
            SoldLG[0].text = "owned";
        }
        if(LGsold[1] > 0)
        {
            LG1price = 0;
            SoldLG[1].text = "owned";
        }
       
        
    }
    IEnumerator entrancetransit()
    {
        yield return new WaitForSeconds(1);
        transitE.SetActive(false);
    }
    public void resetskins()
    {
        selectedHG = 0;
        selectedSG = 0;
        selectedAG = 0;
        selectedLG = 0;
        for(int i = 0; i < headgears.Length; i++)
         {
             headgears[i].SetActive(false);
         }

         for(int i = 0; i < shouldergears.Length; i++)
         {
            shouldergears[i].SetActive(false);
         }
         for(int i = 0; i < shouldergearspair.Length; i++)
         {
            shouldergearspair[i].SetActive(false);
         }

          for(int i = 0; i < armgears.Length; i++)
         {
            armgears[i].SetActive(false);
         }
         for(int i = 0; i < armgearspair.Length; i++)
         {
            armgearspair[i].SetActive(false);
         }
         
         for(int i = 0; i < leggears.Length; i++)
         {
            leggears[i].SetActive(false);
         }
         for(int i = 0; i < leggearspair.Length; i++)
         {
            leggearspair[i].SetActive(false);
         }
    }
    public void showEquip()
    {
        selectedHG = PlayerPrefs.GetInt("headgear");
        selectedSG = PlayerPrefs.GetInt("shouldergear");
        selectedAG = PlayerPrefs.GetInt("armgear");
        selectedLG = PlayerPrefs.GetInt("leggear"); 
    }
    public void HGshop()
    {
        gearshop[0].SetActive(true);
        gearshop[1].SetActive(false);
        gearshop[2].SetActive(false);
        gearshop[3].SetActive(false);
        gearshop[4].SetActive(false);
        buttonsfx1.Play();
    }
    public void SGshop()
    {
        gearshop[0].SetActive(false);
        gearshop[1].SetActive(true);
        gearshop[2].SetActive(false);
        gearshop[3].SetActive(false);
        gearshop[4].SetActive(false);
        buttonsfx1.Play();
    }
    public void AGshop()
    {
        gearshop[0].SetActive(false);
        gearshop[1].SetActive(false);
        gearshop[2].SetActive(true);
        gearshop[3].SetActive(false);
        gearshop[4].SetActive(false);
        buttonsfx1.Play();
    }
    public void LGshop()
    {
        gearshop[0].SetActive(false);
        gearshop[1].SetActive(false);
        gearshop[2].SetActive(false);
        gearshop[3].SetActive(true);
        gearshop[4].SetActive(false);
        buttonsfx1.Play();
    }
    public void gotoProfile()
    {
        buttonsfx1.Play();
        SceneManager.LoadScene("ProfileScene");   
    }
    public void gotoMenu()
    {
        buttonsfx1.Play();
        SceneManager.LoadScene("MenuScene");
        
    }
    public void gotoLevel()
    {
        buttonsfx1.Play();
        SceneManager.LoadScene("GameLevel");
    }
    public void buyHeadgear()
    {
        buttonsfx2.Play();
        coin -= price;
        PlayerPrefs.SetInt("headgear", selectedHG);
        if(selectedHG == 0)
        { 
          PlayerPrefs.SetInt("HG0", 1);
        }
        if(selectedHG == 1)
        { 
          PlayerPrefs.SetInt("HG1", 1);
        }
    }
    public void buyshouldergear()
    {
        buttonsfx2.Play();
        coin -= price;
        PlayerPrefs.SetInt("shouldergear", selectedSG); 
        if(selectedSG == 0)
        { 
          PlayerPrefs.SetInt("SG0", 1);
        }
        if(selectedSG == 1)
        { 
          PlayerPrefs.SetInt("SG1", 1);
        }
    }
    public void buyarmgear()
    {
        buttonsfx2.Play();
        coin -= price;
        PlayerPrefs.SetInt("armgear", selectedAG); 
        if(selectedAG == 0)
        { 
          PlayerPrefs.SetInt("AG0", 1);
        }
        if(selectedAG == 1)
        { 
          PlayerPrefs.SetInt("AG1", 1);
        }
    }
    public void buyleggear()
    {
        buttonsfx2.Play();
        coin -= price;
        PlayerPrefs.SetInt("leggear", selectedLG); 
        if(selectedLG == 0)
        { 
          PlayerPrefs.SetInt("LG0", 1);
        }
        if(selectedLG == 1)
        { 
          PlayerPrefs.SetInt("LG1", 1);
        }
    }
    
     public void gearHG0()
    {
        buttonsfx1.Play();
        selectedHG = 0;
        price = HG0price;
        for(int i = 0; i < headgears.Length; i++)
         {
             headgears[i].SetActive(false);
         }
         buybuttonHG.interactable = true;
    }
    public void gearHG1()
    {
        buttonsfx1.Play();
        selectedHG = 1;
        price = HG1price;
        for(int i = 0; i < headgears.Length; i++)
         {
             headgears[i].SetActive(false);
         }
         buybuttonHG.interactable = true;
    }
     public void gearSG0()
    {
        buttonsfx1.Play();
        selectedSG = 0;
        price = SG0price;
        for(int i = 0; i < shouldergears.Length; i++)
         {
            shouldergears[i].SetActive(false);
         }
         for(int i = 0; i < shouldergearspair.Length; i++)
         {
            shouldergearspair[i].SetActive(false);
         }
         buybuttonSG.interactable = true;
    }
    public void gearSG1()
    {
        buttonsfx1.Play();
        selectedSG = 1;
        price = SG1price;
        for(int i = 0; i < shouldergears.Length; i++)
         {
             shouldergears[i].SetActive(false);
         }
         for(int i = 0; i < shouldergearspair.Length; i++)
         {
             shouldergearspair[i].SetActive(false);
         }
         buybuttonSG.interactable = true;
    }
    public void gearAG0()
    {
        buttonsfx1.Play();
        selectedAG = 0;
        price = AG0price;
        for(int i = 0; i < armgears.Length; i++)
         {
            armgears[i].SetActive(false);
         }
         for(int i = 0; i < armgearspair.Length; i++)
         {
            armgearspair[i].SetActive(false);
         }
         buybuttonAG.interactable = true;
    }
    public void gearAG1()
    {
        buttonsfx1.Play();
        selectedAG = 1;
        price = AG1price;
        for(int i = 0; i < armgears.Length; i++)
         {
             armgears[i].SetActive(false);
         }
         for(int i = 0; i < armgearspair.Length; i++)
         {
            armgearspair[i].SetActive(false);
         }
         buybuttonAG.interactable = true;
    }
    public void gearLG0()
    {
        buttonsfx1.Play();
        selectedLG = 0;
        price = LG0price;
        for(int i = 0; i < leggears.Length; i++)
         {
            leggears[i].SetActive(false);
         }
         for(int i = 0; i < leggearspair.Length; i++)
         {
            leggearspair[i].SetActive(false);
         }
         buybuttonLG.interactable = true;
    }
    public void gearLG1()
    {
        buttonsfx1.Play();
        selectedLG = 1;
        price = LG1price;
        for(int i = 0; i < leggears.Length; i++)
         {
             leggears[i].SetActive(false);
         }
         for(int i = 0; i < leggearspair.Length; i++)
         {
            leggearspair[i].SetActive(false);
         }
         buybuttonLG.interactable = true;
    }
}
