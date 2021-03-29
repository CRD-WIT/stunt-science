using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerDetailsManager : MonoBehaviour
{
    public TMP_InputField playerName;
    public GameObject playerNameText;
    // Start is called before the first frame update
    public string PlayerName;
    int playerGender;

    public GameObject genderSelector;

    Dropdown m_Dropdown;

    void Start()
    {
        SetPlayerDetails();
    }

    public void SetPlayerGender(int value)
    {
        playerGender = value;
        PlayerPrefs.SetInt("gender", playerGender);
    }

    void SetPlayerDetails()
    {
        string p_name = PlayerPrefs.GetString("playerName");
        PlayerName = p_name;
        playerGender = PlayerPrefs.GetInt("gender");
         
        if (playerNameText != null)
        {
            playerNameText.GetComponent<TextMeshPro>().SetText(p_name);
        }
        if (m_Dropdown != null)
        {
            m_Dropdown = genderSelector.GetComponent<Dropdown>();
            m_Dropdown.onValueChanged.AddListener(delegate
            {
                DropdownValueChanged(m_Dropdown);
            });
        }


    }
    void DropdownValueChanged(Dropdown change)
    {
        PlayerPrefs.SetInt("gender", change.value);
    }

    public void SavePlayerDetails()
    {
        if (playerName != null)
        {
            PlayerPrefs.SetString("playerName", playerName.text);
            PlayerPrefs.Save();
        }

    }
}
