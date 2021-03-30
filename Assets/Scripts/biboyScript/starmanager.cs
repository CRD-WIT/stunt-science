using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class starmanager : MonoBehaviour
{
    public GameObject[] stars;
    int VstarE;
    int VstarH;
    int VstarD;
    int AstarE;
    int AstarH;
    int AstarD;
    int FstarE;
    int FstarH;
    int FstarD;
    int PstarE;
    int PstarH;
    int PstarD;
    int CstarE;
    int CstarH;
    int CstarD;
    int FCstarE;
    int FCstarH;
    int FCstarD;
    int WstarE;
    int WstarH;
    int WstarD;
    int EstarE;
    int EstarH;
    int EstarD;
    int PWstarE;
    int PWstarH;
    int PWstarD;
    int MstarE;
    int MstarH;
    int MstarD;
    int IstarE;
    int IstarH;
    int IstarD;

    int velostars;
    int Accstars;
    int freestars;
    int projstars;
    int circstars;
    int forcstars;
    int workstars;
    int enestars;
    int powstars;
    int momstars;
    int impstars;
    int allStars;
    public Text totalstarvelo;
    public Text totalStar;
    public Text mycoin;
    int allCoin;
    
   

    // Start is called before the first frame update
    void Start()
    {
        VstarE = PlayerPrefs.GetInt("VstarE");
        VstarH = PlayerPrefs.GetInt("VstarH");
        VstarD = PlayerPrefs.GetInt("VstarD");
        allCoin = PlayerPrefs.GetInt("mycoins");
       
    }

    // Update is called once per frame
    void Update()
    {
        velostars = VstarE + VstarH + VstarD;
        allStars = velostars + Accstars + freestars + projstars + circstars + forcstars + workstars + enestars + powstars + momstars + impstars;
        totalstarvelo.text = velostars.ToString("F0");
        totalStar.text = allStars.ToString("F0");
        mycoin.text = allCoin.ToString("F0");

        PlayerPrefs.SetInt("mystars", allStars);
        PlayerPrefs.SetInt("mycoins", allCoin);



        if (VstarE > 0)
        {
            stars[1].SetActive(true);
            
        }
         if (VstarE > 1)
        {
            stars[2].SetActive(true);
        }
         if (VstarE > 2)
        {
            stars[3].SetActive(true);
        }
         if (VstarH > 0)
        {
            stars[4].SetActive(true);
        }
    }
}
