using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenderManager : MonoBehaviour
{
    public Sprite[] spriteArray;
    public int playerGender;
    // Start is called before the first frame update
    void Start()
    {
        playerGender = PlayerPrefs.GetInt("gender");
        if (playerGender == 0)
        {
            GameObject.Find("Head").GetComponent<SpriteRenderer>().sprite = spriteArray[0];
            GameObject.Find("Body").GetComponent<SpriteRenderer>().sprite = spriteArray[1];
        }
        else
        {
            GameObject.Find("Head").GetComponent<SpriteRenderer>().sprite = spriteArray[2];
            GameObject.Find("Body").GetComponent<SpriteRenderer>().sprite = spriteArray[3];
        }
    }

    public void ChangeGender(int gender)
    {
        if (gender == 0)
        {
            playerGender = gender;
            GameObject.Find("Head").GetComponent<SpriteRenderer>().sprite = spriteArray[0];
            GameObject.Find("Body").GetComponent<SpriteRenderer>().sprite = spriteArray[1];
        }
        else
        {
            playerGender = gender;
            GameObject.Find("Head").GetComponent<SpriteRenderer>().sprite = spriteArray[2];
            GameObject.Find("Body").GetComponent<SpriteRenderer>().sprite = spriteArray[3];
        }
        PlayerPrefs.SetInt("gender", playerGender);
        PlayerPrefs.Save();
    }
}
