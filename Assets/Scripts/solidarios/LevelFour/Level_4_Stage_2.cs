using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level_4_Stage_2 : MonoBehaviour
{
    // Start is called before the first frame update
    string question;
    public TMP_Text questionText, levelName;
    public GameObject AfterStuntMessage;
    public GameObject movingToHookHand;
    Animator thePlayerAnimation;
    public GameObject hook;
    bool isSimulating = false;
    public GameObject thePlayer;
    Vector3 thePlayer_position;
    float timeGiven;
    Vector2 gravityGiven;
    Vector2 spawnPointValue;
    public GameObject hookLauncher;
    float distanceGiven;
    float angleGiven;

    void Start()
    {
        // Given        
        timeGiven = (float)System.Math.Round(UnityEngine.Random.Range(20f, 25f), 2);
        distanceGiven = transform.Find("Annotation1").GetComponent<Annotation>().distance;
        angleGiven = (float)System.Math.Round(UnityEngine.Random.Range(35f, 40f), 2);
        //angleGiven = 40f;
        gravityGiven = Physics2D.gravity;

        //Problem
        levelName.SetText("Free Fall | Stage 4");
        question = $"Wants to cross to the cliff at the other side using his climbing device. If [pronoun] shoots its gripping projectile at a velocity of [vo] at an angle of [a] degrees, at what precise time should [name] trigger the gripper if it will grip at the exact moment when it will touch the gripping point of the cliff accross which is at the same horizontal level of the shooting device.";

        if (questionText != null)
        {
            questionText.SetText(question);
        }
        else
        {
            Debug.Log("QuestionText object not loaded.");
        }

        thePlayerAnimation = thePlayer.GetComponent<Animator>();
        thePlayer_position = thePlayer.transform.position;

        spawnPointValue = transform.Find("Annotation1").GetComponent<Annotation>().SpawnPointValue();

        transform.Find("CircularAnnotation").GetComponent<CircularAnnotation>().SetAngle(angleGiven);

        transform.Find("AngularAnnotation").GetComponent<AngularAnnotation>().SetAngle(angleGiven);

        hook.GetComponent<Rigidbody2D>().Sleep();
        hook.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        hookLauncher.transform.Rotate(new Vector3(0, 0, angleGiven));
    }

    IEnumerator StuntResult()
    {
        //messageFlag = false;
        yield return new WaitForSeconds(4f);
        AfterStuntMessage.SetActive(true);
    }

    void GenerateVelocities()
    {

    }

    IEnumerator DropRope()
    {
        yield return new WaitForSeconds(1.5f);      
    }

    IEnumerator PullRope()
    {
        yield return new WaitForSeconds(1.3f);
    }

    public void StartSimulation()
    {
        isSimulating = true;
    }
    void FixedUpdate()
    {

    }
}
