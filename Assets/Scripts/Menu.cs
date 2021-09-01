using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject continueButton;
    public GameObject newGameButton;
    public AudioSource swoosh;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        string playerName = PlayerPrefs.GetString("Name");

        if (playerName.Length > 0)
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }

    }

    public void PlaySwoosh()
    {
        swoosh.Play();
    }
}
