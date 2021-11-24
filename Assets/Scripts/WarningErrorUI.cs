using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WarningErrorUI : MonoBehaviour
{
    public bool isOpen;
    public string message;
    public GameObject panel;
    public Text[] messageComponent;
    public bool intertnetPanelIsOpen;
    public GameObject internetPanel;

    public bool promptInternetConnection = true;

    void Start()
    {
        if (promptInternetConnection)
        {
            StartCoroutine(checkInternetConnection((isConnected) =>
            {
                if (!isConnected)
                {
                    this.message = "Internet connection is required to test the game.";
                    toggleInternetConnectionError();
                }
            }));
        }
    }

    public void SetMessage(string m)
    {
        this.message = m;
    }

    public void togglePanel()
    {
        isOpen = !isOpen;
    }

    public void toggleInternetConnectionError()
    {
        Debug.Log(message);
        intertnetPanelIsOpen = !isOpen;
    }

    public IEnumerator checkInternetConnection(Action<bool> action)
    {
        WWW www = new WWW("https://google.com");
        yield return www;
        if (www.error != null)
        {
            action(false);
        }
        else
        {
            action(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in messageComponent)
        {
            if (item)
            {
                item.text = message;
            }
        }

        panel.SetActive(isOpen);

        if (internetPanel && promptInternetConnection)
        {
            internetPanel.SetActive(intertnetPanelIsOpen);
        }
    }
}
