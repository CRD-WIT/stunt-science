using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PlayerGearManager : MonoBehaviour
{
    public Sprite[] spriteArray;
    private GameObject player;

    string[] parts = {
        "Helmet"
        };
    Shadow shadow;
    // Start is called before the first frame update
    void Start()
    {

        foreach (string item in parts)
        {   
            int index = PlayerPrefs.GetInt("hat");
            Debug.Log("Index: "+index);
            GameObject.Find(item).GetComponent<SpriteRenderer>().sprite = spriteArray[index];
        }

    }

    public void SetGear(int index)
    {
        foreach (string item in parts)
        {
            GameObject.Find(item).GetComponent<SpriteRenderer>().sprite = spriteArray[index];

            // Reset all outlines.
            GameObject[] all = GameObject.FindGameObjectsWithTag("GearButton");
            foreach (var o in all)
            {
                o.GetComponent<Outline>().enabled = false;
            }

            Outline activeButton = EventSystem.current.currentSelectedGameObject.GetComponent<Outline>();
            if (activeButton.enabled == true)
            {
                activeButton.enabled = false;
            }
            else
            {
                activeButton.enabled = true;
                PlayerPrefs.SetInt("hat", index);
                PlayerPrefs.Save();
            }

        }
    }


}
