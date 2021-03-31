using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonManager : MonoBehaviour
{
    public Button answerButton, retryButton; //continueButton;
    public TMP_InputField answerField;
    public TMP_Text questionTextBox;
    public static string question;
    public static float playerAnswer;
    public static bool isSimulating;

    StageManager sm = new StageManager();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        questionTextBox.SetText(question);        
    }

    public void Play(){
        isSimulating =true;
        string errorMessage = answerField.text != "" ? "":"Please enter a value";
        playerAnswer = float.Parse(answerField.text);
    }

    public void Retry(){
        int stage = sm.GetStageFromPlayerPrefs();
        if(stage == 1){

        }
    }

}
