using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashCardManager : MonoBehaviour
{
    public Image FlashCardImageContainer;
    public Sprite MaleFlashCard;
    public Sprite FemaleFlashCard;

    public bool isMale = true;

    string gender = "Male";


    // Start is called before the first frame update
    void Start()
    {
        gender = PlayerPrefs.GetString("Gender");
    }

    // Update is called once per frame
    void Update()
    {
        FlashCardImageContainer.sprite = gender == "Male" ? MaleFlashCard : FemaleFlashCard;
    }
}
