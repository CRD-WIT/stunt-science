using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject mainUI;
    public GameObject purchaseUI;
    // Start is called before the first frame update
    int talentFee = 0;
    void Start()
    {
        talentFee = PlayerPrefs.GetInt("talentFee");
    }

    public void TriggerBuy()    {
        mainUI.GetComponent<GraphicRaycaster>().enabled = false;
        purchaseUI.SetActive(true);
    }

    public void PurchaseItem()
    {
        mainUI.GetComponent<GraphicRaycaster>().enabled = true;
        purchaseUI.SetActive(false);
    }

    public void CancelPurchase()
    {
        mainUI.GetComponent<GraphicRaycaster>().enabled = true;
        purchaseUI.SetActive(false);
    }

}
