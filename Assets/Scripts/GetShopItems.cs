using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shop;
using TMPro;
// using Firebase.Firestore;
// using Firebase.Extensions;
// using Firebase;
public class GetShopItems : MonoBehaviour
{
    public Sprite[] spriteArray;
    ShopItem itemInstance;
    public GameObject shopItemPrefab;
    // Start is called before the first frame update

    void Start()
    {
        // itemInstance = new ShopItem();

        // FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        // CollectionReference docsRef = db.Collection("shopItems");
        // docsRef.Listen((snapshot) =>
        // {
        //     foreach (Transform child in this.transform)
        //     {
        //         GameObject.Destroy(child.gameObject);
        //     }
        //     foreach (DocumentSnapshot document in snapshot.Documents)
        //     {
        //         Dictionary<string, object> documentDictionary = document.ToDictionary();
        //         Debug.Log(documentDictionary["itemName"].ToString());
                
        //         GameObject item = Instantiate(shopItemPrefab, this.transform.position, this.transform.rotation);
                
        //         item.transform.SetParent(transform);
        //         item.transform.localScale = new Vector3(1, 1, 1);
        //         item.GetComponent<ShopItem>().itemPrice = documentDictionary["price"].ToString();
        //         item.GetComponent<ShopItem>().SetTitle(documentDictionary["itemName"].ToString());
        //         item.GetComponent<ShopItem>().purchased = documentDictionary["purchased"].ToString() == "True" ? true : false;
        //     }
        // });

    }
}
