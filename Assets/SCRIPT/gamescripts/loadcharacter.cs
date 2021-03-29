using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadcharacter : MonoBehaviour
{
    public GameObject playermale;
    public GameObject playerfemale;
    int selectedplayer;

    
    // Start is called before the first frame update
   void Start()
    {
        selectedplayer = PlayerPrefs.GetInt ("sex");
       
        
        if (selectedplayer == 0)
        {
            playermale.SetActive(true);
            playerfemale.SetActive(false);
            
            
        }
        if (selectedplayer == 1)
        {
            playermale.SetActive(false);
            playerfemale.SetActive(true);
            
            
        }
    }

    // Update is called once per frame
    void Update()
    {
       selectedplayer = PlayerPrefs.GetInt ("sex");
         
        
    }
}
