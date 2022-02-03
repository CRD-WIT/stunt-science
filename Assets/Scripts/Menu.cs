using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject continueButton;
    public GameObject newGameButton;
    public AudioSource swoosh;
    int counter = 0;

    public void AddCounter()
    {
        counter++;

        if (counter >= 8)
        {
            PlayerPrefs.SetInt("DebugMode", 1); // 1==true; 0==false
            counter = 0; //reset
        }
    }

    // Update is called once per frame
    void Update()
    {
        string playerName = PlayerPrefs.GetString("Name");

        if (playerName.Length > 0)
        {
            continueButton.SetActive(true);
            newGameButton.SetActive(false);
        }
        else
        {
            continueButton.SetActive(false);
            newGameButton.SetActive(true);
        }

    }

    public void PlaySwoosh()
    {
        swoosh.Play();
    }
}
