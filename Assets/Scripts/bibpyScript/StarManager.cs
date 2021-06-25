using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour
{
    int VstarE, VstarM, VstarD, AcstarE, AcstarM, AcstarD, FrstarE, FrstarM, FrstarD;
    
    
    public GameObject[] starsVeE, starsVeM, starsVeD, starsAcE, starsAcM, starsAcD, starFrE, starFrM, starFrD;
    

    // Start is called before the first frame update
    void Start()
    {
        VstarE = PlayerPrefs.GetInt("VstarE");
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < VstarE; i++)
        {
            starsVeE[i].SetActive(true);
        }
        for(int i = 0; i < VstarM; i++)
        {
            starsVeM[i].SetActive(true);
        }
        for(int i = 0; i < VstarD; i++)
        {
            starsVeD[i].SetActive(true);
        }
        for(int i = 0; i < AcstarE; i++)
        {
            starsAcE[i].SetActive(true);
        }
        for(int i = 0; i < AcstarM; i++)
        {
            starsAcM[i].SetActive(true);
        }
        for(int i = 0; i < AcstarD; i++)
        {
            starsAcD[i].SetActive(true);
        }
        for(int i = 0; i < FrstarE; i++)
        {
            starFrE[i].SetActive(true);
        }
        for(int i = 0; i < FrstarM; i++)
        {
            starFrM[i].SetActive(true);
        }
        for(int i = 0; i < FrstarD; i++)
        {
            starFrD[i].SetActive(true);
        }
    }
}
