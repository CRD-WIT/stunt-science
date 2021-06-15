using System.Collections;
using UnityEngine;
using TMPro;

public class StageThreeManager : MonoBehaviour
{
    // Start is called before the first frame update

    float height = 5.0f;
    float targetTime = 0f;
    string question;
    public float elapsed;
    public TMP_Text playerNameText, messageText, timerText, questionText, levelName;
    public GameObject AfterStuntMessage;
    Animator thePlayerAnimation;
    public TMP_InputField playerAnswer;
    public GameObject playerOnRope;
    bool isSimulating = false;
    public GameObject playerHingeJoint;
    public GameObject thePlayer;
    public GameObject playerHangingFixed;
    public GameObject FirstCamera;
    public GameObject SecondCamera;
    Vector3 thePlayer_position;
    public GameObject accurateCollider;

    public GameObject platformBar;

    Vector3 playerOnRopeTransform;

    float timeGiven;
    Vector2 gravityGiven;
    float correctAnswer;
    Vector2 spawnPointValue;
    float distance;
    void Start()
    {
        // Given        
        timeGiven = (float)System.Math.Round(UnityEngine.Random.Range(0.8f, 1.0f), 2);
        gravityGiven = Physics2D.gravity;
        // Formula
        correctAnswer = Mathf.Abs((gravityGiven.y / 2) * Mathf.Pow(timeGiven, 2));

        transform.Find("Annotation1").GetComponent<Annotation>().SetDistance(correctAnswer);
        transform.Find("Annotation1").GetComponent<Annotation>().SetSpawningPoint(new Vector2(15, playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y - correctAnswer));

        Debug.Log($"Distance: {correctAnswer}");
        Debug.Log($"Hinge: {playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y}");
        Debug.Log($"Time Generated: {timeGiven}");
        Debug.Log($"Correct Answer: {System.Math.Round(correctAnswer, 2)}");

        //Problem
        levelName.SetText("Free Fall | Stage 1");
        question = $"[name] is hanging from a rope and [pronoun] is instructed to let go of it, drop down, and hang again to the horizontal pole below. If [name] will let go ang grab the pole after exactly <color=purple>{timeGiven} sec</color>, at what <color=red>distance</color> should [pronoun] hands above the pole before letting go?";

        if (questionText != null)
        {
            questionText.SetText(question);
        }
        else
        {
            Debug.Log("QuestionText object not loaded.");
        }
        thePlayerAnimation = thePlayer.GetComponent<Animator>();

        thePlayerAnimation.SetBool("isHanging", true);
        thePlayer_position = thePlayer.transform.position;

        distance = transform.Find("Annotation1").GetComponent<Annotation>().distance;
        playerOnRopeTransform = playerOnRope.transform.position;

        spawnPointValue = transform.Find("Annotation1").GetComponent<Annotation>().SpawnPointValue();

        platformBar.transform.position = new Vector3(spawnPointValue.x - 9, spawnPointValue.y, 0);


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
                elapsed += Time.fixedDeltaTime;
                timerText.text = elapsed.ToString("f2") + "s";

                playerHingeJoint.GetComponent<HingeJoint2D>().enabled = false;
                thePlayerAnimation.SetBool("isFalling", true);


                // Correct Answer
                if (System.Math.Round(float.Parse(playerAnswer.text), 2) == System.Math.Round(correctAnswer, 2))
                {
                    Debug.Log("Distance is correct!");
                    if (accurateCollider.GetComponent<StageThreePlayerScript>().isCollided)
                    {
                        FirstCamera.SetActive(false);
                        SecondCamera.SetActive(true);

                        thePlayer.SetActive(false);
                        playerHangingFixed.SetActive(true);
                        playerHangingFixed.transform.position = new Vector3(spawnPointValue.x - 0.2f, platformBar.transform.position.y - 1.5f, 1);
                        platformBar.GetComponent<Animator>().SetBool("collided", true);
                        playerHangingFixed.GetComponent<Animator>().SetBool("isHangingInBar", true);
                        isSimulating = false;
                    }
                }
                else
                {
                    if (float.Parse(playerAnswer.text) < System.Math.Round(correctAnswer, 2))
                    {
                        if (accurateCollider.GetComponent<StageThreePlayerScript>().isCollided)
                        {
                            Debug.Log("Distance is too short!");
                            isSimulating = false;

                        }
                    }
                    else
                    {
                        if (accurateCollider.GetComponent<StageThreePlayerScript>().isCollided)
                        {
                            Debug.Log("Distance is too long!");
                            isSimulating = false;
                        }
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
            //platformBar.transform.position = new Vector3(spawnPointValue.x - 9, playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y - distance, 1);
            timerText.text = elapsed.ToString("f2") + "s";
            isSimulating = false;
        }


    }
}
