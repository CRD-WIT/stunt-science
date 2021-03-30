using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skinManager : MonoBehaviour
{
    public GameObject headgear1;
    public GameObject[] shouldergear1;
    public GameObject[] armgear1;
    public GameObject[] leggear1;


    int headgears;
    int shouldergears;
    int armgears;
    int leggears;
    // Start is called before the first frame update
    void Start()
    {
        headgears = PlayerPrefs.GetInt("headgear");
        if (headgears == 1)
        {
            headgear1.SetActive(true);
        }
        shouldergears = PlayerPrefs.GetInt("shouldergear");
        if (shouldergears == 1)
        {
           shouldergear1[0].SetActive(true);
           shouldergear1[1].SetActive(true);
        }
        armgears = PlayerPrefs.GetInt("armgear");
        if (armgears == 1)
        {
            armgear1[0].SetActive(true);
            armgear1[1].SetActive(true);
        }
        leggears = PlayerPrefs.GetInt("leggear");
        if (leggears == 1)
        {
            leggear1[0].SetActive(true);
            leggear1[1].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
