using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public GameObject[] Tabs;
    public Button[] TabButtons;
    public int activeIndex = 0;
    void Start()
    {
        TabButtons[0].Select();
        for (int i = 0; i < TabButtons.Length; i++)
        {
            int index = i;
            TabButtons[i].onClick.AddListener(delegate
            {
                Debug.Log($"Active: {index}; {TabButtons[index].name}");

                SetActiveIndex(index);
            });
        }
    }

    void SetActiveIndex(int index)
    {
        activeIndex = index;
    }

    // Update is called once per frame
    void Update()
    {
        var colorsActive = TabButtons[activeIndex].colors;
        var colorsInactive = TabButtons[TabButtons.Length - 1].colors;

        Color newCol;
        if (ColorUtility.TryParseHtmlString("#F8DA95", out newCol))
            colorsActive.normalColor = newCol;

        colorsInactive.normalColor = Color.white;

        TabButtons[activeIndex].colors = colorsInactive;

        for (int i = 0; i < TabButtons.Length; i++)
        {
            if (i != activeIndex)
            {
                TabButtons[i].colors = colorsInactive;
            }
            else
            {
                TabButtons[activeIndex].colors = colorsActive;
            }

        }

        for (int i = 0; i < Tabs.Length; i++)
        {
            Tabs[i].SetActive(i == activeIndex);
        }

    }
}
