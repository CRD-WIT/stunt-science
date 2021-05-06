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
    Animator thePlayerAnimation;
    public GameObject hook;
    bool isSimulating = false;
    public GameObject thePlayer;
    Vector3 thePlayer_position;
    Vector2 gravityGiven;
    Vector2 spawnPointValue;
    public GameObject hookLauncher;
    float distanceGiven;
    float angleGiven;
    float velocityX = 0;
    float velocityY = 0;
    float velocityInitial = 0;
    public GameObject DynamicPlatform;

    void Start()
    {
        // Given            
        distanceGiven = (float)System.Math.Round(UnityEngine.Random.Range(22f, 25f), 2);
        angleGiven = (float)System.Math.Round(UnityEngine.Random.Range(40f, 45f), 2);
        gravityGiven = Physics2D.gravity;

        Annotation line = transform.Find("Annotation").GetComponent<Annotation>();

        line.SetDistance(distanceGiven);

        

        //Problem
        levelName.SetText("Projectile Motion | Stage 2");
        question = $"[name] is instructed to cross another diff using this climbing device. If [name] shoot its gripping projectle at an angle of [angle] degrees up to precisedlly git the corner of the cliff [distance] meters away horizontally from the shooting device, at what velocity should the projectile be shot to hit the gripping part?";

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

        GenerateVelocities();
    }

    IEnumerator StuntResult()
    {
        //messageFlag = false;
        yield return new WaitForSeconds(4f);
        AfterStuntMessage.SetActive(true);
    }

    void GenerateVelocities()
    {
        velocityX = Mathf.Sqrt(Mathf.Abs((distanceGiven * gravityGiven.y) / (2 * Mathf.Tan(angleGiven * Mathf.Deg2Rad))));
        velocityInitial = Mathf.Abs((velocityX / Mathf.Cos(angleGiven * Mathf.Deg2Rad)));
        velocityY = Mathf.Abs(velocityInitial * Mathf.Sin(angleGiven * Mathf.Deg2Rad));



        Debug.Log($"VelocityX: {velocityX}");
        Debug.Log($"VelocityY: {velocityY}");
        Debug.Log($"VelocityInitial: {velocityInitial}");
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
