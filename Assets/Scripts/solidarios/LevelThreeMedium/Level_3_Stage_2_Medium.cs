using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level_3_Stage_2_Medium : MonoBehaviour
{
    // Start is called before the first frame update
    string question;
    public TMP_Text initalVelociyText, playerNameText, stuntMessageText, timerText, questionText, levelName;
    bool isSimulating = false;
    public GameObject hookLauncher, thePlayerRunning, shootPosTriger, puller, angularAnotation, gun, dimensions;
    public GameObject hookLine;
    public GameObject hookIndicator;
    public TextMeshPro timeIndicator;
    public playerProjectileMed thePlayer;
    public float distance;
    public float angleGiven;

    public bool collided;
    public GameObject annotation;
    public CameraScript cameraScript;
    public QuestionController questionController;
    void Start()
    {

    }
    public void StartSimulation()
    {
        cameraScript.directorIsCalling = true;
        cameraScript.isStartOfStunt = true;
        questionController.SetAnswer();
    }
    public void showProblem()
    {
        angleGiven = (float)System.Math.Round(Random.Range(50f, 60.5f), 2);
        angularAnotation.transform.position = gun.transform.position;
        angularAnotation.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angleGiven);
    }

    void FixedUpdate()
    {
        if (thePlayerRunning.GetComponent<RunningPlayer>().isCollided)
        {
            if (!collided)
            {
                thePlayer.gameObject.SetActive(true);
                thePlayerRunning.SetActive(false);
                shootPosTriger.SetActive(false);
                puller.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                thePlayer.aim = true;
                dimensions.SetActive(true);
                showProblem();
                Debug.Log("collide");
                collided = true;
            }

        }

    }
}
