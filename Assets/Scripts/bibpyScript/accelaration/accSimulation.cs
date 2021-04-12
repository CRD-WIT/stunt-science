using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class accSimulation : MonoBehaviour
{
    public Button playButton;
    //public Player thePlayer;
    public TMP_InputField answerField;
    public static string question;
    public TMP_Text questionTextBox, errorTextBox, levelText;
    public static float playerAnswer;
    public static bool simulate;
    public static bool playerDead;
    
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        questionTextBox.SetText(question);
        
    }
    public void PlayButton()
    {
        

        if (answerField.text == "")
        {
            errorTextBox.SetText("Please enter your answer!");
        }
        else
        {
            /*isStartOfStunt = true;
            directorIsCalling = true;
            //answerField.placeholder = playerAnswer.ToString()+"m/s";*/
            playerAnswer = float.Parse(answerField.text);
            simulate = true;
            playButton.interactable = false;
            
            {
            
                answerField.text = playerAnswer.ToString() + "m/s²";
            }
        
        }
    }
}
