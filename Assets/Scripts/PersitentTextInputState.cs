using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PersitentTextInputState : MonoBehaviour
{
    public string storeName;
    // Start is called before the first frame update
    void Start()
    {
        TMP_InputField field = this.gameObject.GetComponent<TMP_InputField>();
        string store = PlayerPrefs.GetString(storeName);
        field.text = store;
    }

    public void SaveValue()
    {
        TMP_InputField field = this.gameObject.GetComponent<TMP_InputField>();
        PlayerPrefs.SetString(storeName, field.text);
        Debug.Log($"Value {storeName} saved.");
    }

}
