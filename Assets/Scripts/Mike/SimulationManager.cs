using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SimulationManager : MonoBehaviour
{
    public GameObject transition;
    public VelocityEasyStage1 VelocityEasyStage1;
    public Player thePlayer;
    public Button answerButton, retryButton, nextButton;
    public TMP_InputField answerField;
    public TMP_Text questionTextBox, errorTextBox;
    public static string question;
    public static float playerAnswer;
    public static bool isSimulating, isAnswerCorrect;

    StageManager sm = new StageManager();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        questionTextBox.SetText(question);
        if(isAnswerCorrect){
            retryButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(true);
        }else{
            retryButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(false);
        } 
        Debug.Log(PlayerPrefs.GetInt("stageNumber"));       
    }

    public void PlayButton(){        
        //string errorMessage = answerField.text != "" ? "":"Please enter a value";
        if(answerField.text == ""){
            errorTextBox.SetText("Please enter your answer!");
        }else{
            playerAnswer = float.Parse(answerField.text);
            isSimulating =true;
        }        
    }

    public void RetryButton(){
        int stage = sm.GetStageFromPlayerPrefs();
        if(stage == 1){
            VelocityEasyStage1.VelocityEasyStage1SetUp();
        }
        else if(stage == 2){
        }
        else{
        }
    }
    public void NextButton(){
        int stage = sm.GetStageFromPlayerPrefs();
        if(stage == 1){
            sm.SetStage(2);
            StartCoroutine(ExitStage());
            VelocityEasyStage1.gameObject.SetActive(false);
        }else if(stage == 2){
            sm.SetStage(3);
        }
    }
    IEnumerator ExitStage(){
        VelocityEasyStage1.AfterStuntMessage.SetActive(false);
        thePlayer.moveSpeed = 3;
        yield return new WaitForSeconds(3);
        transition.SetActive(true);
        yield return new WaitForSeconds(1);
        thePlayer.moveSpeed = 0;
        transition.SetActive(false);
        thePlayer.transform.position = new Vector2(0f, thePlayer.transform.position.y);
    }
}
