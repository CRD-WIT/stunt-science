using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;

public class Menu : MonoBehaviour
{
    public GameObject continueButton;
    public GameObject newGameButton;
    public AudioSource swoosh;
    public WarningErrorUI warningErrorUI;
    string playerName;
    public SceneNavigator sceneNavigator;

    FirebaseApp app;
    void Start()
    {
        playerName = PlayerPrefs.GetString("Name");

    }

    public void CheckExistingData()
    {
        if (playerName.Length > 0)
        {
            warningErrorUI.message = "You have an existing player data. Creating a new account will overwite your progress.";
            warningErrorUI.togglePanel();
        }
        else
        {
            sceneNavigator.GotoScene("Registration");
        }

    }

    // Update is called once per frame
    void Update()
    {


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
