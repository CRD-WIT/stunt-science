using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberLabelIncrementor : MonoBehaviour
{
    public int CurrentValue = 0;
    public int MaxValue;
    public int rate = 1;
    float DelayAmount = 1f; // Second count
    // Start is called before the first frame update
    protected float Timer;
    void Start()
    {
        this.GetComponent<TextMeshProUGUI>().SetText(CurrentValue.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= (DelayAmount / rate) && CurrentValue <= MaxValue)
        {
            Timer = 0f;
            CurrentValue++;
            this.GetComponent<TextMeshProUGUI>().SetText((CurrentValue - 1).ToString());
        }
    }
}
