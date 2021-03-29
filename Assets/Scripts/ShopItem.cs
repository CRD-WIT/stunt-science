using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public string itemTitle;
    public int spriteID;
    public string itemPrice;
    public bool purchased;

    // Start is called before the first frame update
    public void SetTitle(string title)
    {
        this.itemTitle = title;
    }

    void Update()
    {
        GameObject.Find("ItemTitle").GetComponent<TextMeshProUGUI>().SetText(itemTitle);
        GameObject.Find("ItemPrice").GetComponent<TextMeshProUGUI>().SetText(itemPrice);
        GameObject.Find("PurchasedOverlay").GetComponent<Image>().enabled = purchased;
    }
}
