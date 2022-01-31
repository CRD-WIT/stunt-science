using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject continueButton;
    public GameObject newGameButton;
    public AudioSource swoosh;
    int counter = 0;
    void OnGUI()
    {
        GUI.Label(new Rect(40, 20, 200, 50), "COUNTER: " + counter);
        {

            if (counter >= 16)
            {
                Debug.Log("CounterComplete.");
            }

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
