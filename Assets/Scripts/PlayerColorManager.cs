using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerColorManager : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    string[] parts = {
        "Helmet"
        // "Right Thigh",
        // "Left Thigh",
        // "Body",
        // "Head",
        // "Left Arm",
        // "Left Calf",
        // "Left Foot",
        // "Left Hand",
        // "Left Thigh",
        // "Left Wrist",
        // "Right Arm",
        // "Right Calf",
        // "Right Foot",
        // "Right Hand",
        // "Right Thigh",
        // "Right Wrist",
        };
    void Start()
    {
        foreach (string item in parts)
        {
            setColor(PlayerPrefs.GetString("activeColor"));
        }
    }

    public void setColor(string color)
    {
        Color activeColor;
        switch (color)
        {
            case "1":
                activeColor = new Color(255, 0, 0);
                break;
            case "2":
                activeColor = new Color(0, 248, 255);
                break;
            case "3":
                activeColor = new Color(0, 255, 0);
                break;
            case "4":
                activeColor = new Color(255, 255, 0);
                break;
            case "5":
                activeColor = new Color(255, 0, 246);
                break;
            case "6":
                activeColor = new Color(0, 189, 255);
                break;
            case "7":
                activeColor = new Color(172, 0, 255);
                break;
            case "8":
                activeColor = new Color(255, 255, 255);
                break;
            default:
                activeColor = new Color(255, 255, 255);
                break;
        }

        foreach (string item in parts)
        {
            GameObject.Find(item).GetComponent<SpriteRenderer>().color = activeColor;
        }

        PlayerPrefs.SetString("activeColor", color);
    }
}
