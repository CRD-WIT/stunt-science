using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level_4_Stage_1 : MonoBehaviour
{
    // Start is called before the first frame update
    string question;
    public float elapsed;

    public TMP_Text playerNameText, stuntMessageText, timerText, questionText, levelName;
    public GameObject AfterStuntMessage;
    Animator thePlayerAnimation;
    public TMP_InputField playerAnswer;
    public GameObject hook;
    bool isSimulating = false;
    public float speed = 1f;
    public GameObject thePlayer;
    public GameObject FirstCamera;
    public GameObject SecondCamera;
    Vector3 thePlayer_position;
    float timeGiven;
    Vector2 gravityGiven;
    float correctAnswer;
    Vector2 spawnPointValue;
    float distance;
    float distanceGiven;
    bool doneFiring = false;
    void Start()
    {

        // Given        
        timeGiven = (float)System.Math.Round(UnityEngine.Random.Range(20f, 25f), 2);
        distanceGiven = (float)System.Math.Round(UnityEngine.Random.Range(25f, 28.0f), 2);
        gravityGiven = Physics2D.gravity;

        // Formula
        correctAnswer = Mathf.Sqrt(Mathf.Abs((2 * distanceGiven) / gravityGiven.y));

        Debug.Log($"Correct Answer: {System.Math.Round(correctAnswer, 2)}");

        //Problem
        levelName.SetText("Free Fall | Stage 4");
        question = $"[name] is hanging on a horizontal pole and [pronoun] is instructed to let go of it, drop down, and grab the elastic cord below to slow down his fall and safely land him into the ground. If the hands of [name] is exactly <color=red>{distanceGiven}</color> meters above the second pole, <color=purple>how long</color> should [name] fall down before [pronoun] grabs the second pole?";

        if (questionText != null)
        {
            questionText.SetText(question);
        }
        else
        {
            Debug.Log("QuestionText object not loaded.");
        }

        thePlayerAnimation = thePlayer.GetComponent<Animator>();
        thePlayerAnimation.SetBool("isHangingInBar", true);
        thePlayer_position = thePlayer.transform.position;

        distance = transform.Find("Annotation1").GetComponent<Annotation>().distance;

        // playerOnRopeTransform = playerOnRope.transform.position;

        spawnPointValue = transform.Find("Annotation1").GetComponent<Annotation>().SpawnPointValue();

        hook.GetComponent<Rigidbody2D>().Sleep();

    }

    IEnumerator StuntResult()
    {
        //messageFlag = false;
        yield return new WaitForSeconds(4f);
        AfterStuntMessage.SetActive(true);
    }

    public void StartSimulation()
    {
        isSimulating = true;
    }
    void FixedUpdate()
    {
        float thePlayer_x = thePlayer_position.x;
        float thePlayer_y = thePlayer_position.y;
        if (isSimulating)
        {
            if (playerAnswer.text.Length > 0)
            {

                transform.Find("Annotation1").GetComponent<Annotation>().Hide();
                transform.Find("CircularAnnotation").GetComponent<CircularAnnotation>().Hide();
                elapsed += Time.fixedDeltaTime;
                timerText.text = elapsed.ToString("f2") + "s";

                if (!doneFiring)
                {
                    hook.GetComponent<Rigidbody2D>().WakeUp();               
                    // Use the angle from the indicator
                    hook.GetComponent<Rigidbody2D>().velocity =  new Vector3(1,0.5f,0) * speed / hook.GetComponent<Rigidbody2D>().mass;
                    doneFiring = true;
                }


                // Correct Answer
                if (System.Math.Round(float.Parse(playerAnswer.text), 2) == System.Math.Round(correctAnswer, 2))
                {
                    Debug.Log("Time is correct!");
                }
                else
                {
                    if (float.Parse(playerAnswer.text) < System.Math.Round(correctAnswer, 2))
                    {
                        // Too short

                    }
                    else
                    {
                        // Too long
                    }
                }
            }
            else
            {
                Debug.Log("No value was added");
            }
        }
        else
        {
            //platformBarBottom.transform.position = new Vector3(spawnPointValue.x - 9, playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y - distance, 1);            
            isSimulating = false;

            timerText.text = $"{(elapsed).ToString("f2")}s";
        }
    }
}
