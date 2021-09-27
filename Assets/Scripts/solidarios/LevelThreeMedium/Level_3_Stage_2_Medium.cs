using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level_3_Stage_2_Medium : MonoBehaviour
{
    // Start is called before the first frame update
    string question;
    public TMP_Text initalVelociyText, playerNameText, stuntMessageText, timerText, questionText, levelName, VoTxt,angleTxt;
    bool isSimulating = false;
    public GameObject hook, hookLauncher, thePlayerRunning, shootPosTriger, puller, angularAnotation, gun, dimensions, target;
    public GameObject hookLine, trail;
    public GameObject hookIndicator;
    public TextMeshPro timeIndicator;
    public playerProjectileMed thePlayer;
    public float distanceX, distanceY;
    public float angleGiven, projectileTime, Vo;

    public bool collided, preSetUp;
    public CircularAnnotation theCircular;
    public CameraScript cameraScript;
    public Hook theHook;
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
        preSetUp = true;
        projectileTime = (float)System.Math.Round(Random.Range(2f, 2.5f), 2);
    }
    public IEnumerator shoot()
    {
        hook.SetActive(true);
        hook.transform.position = hookLauncher.transform.position;
        hook.GetComponent<Rigidbody2D>().velocity = hookLauncher.transform.right * (Vo);
        yield return new WaitForEndOfFrame();
        theHook.isTrailing = true;
        trail.GetComponent<TrailRenderer>().time = 3000;

    }

    void FixedUpdate()
    {
        hookLine.GetComponent<LineRenderer>().SetPosition(0, gun.transform.position);
        hookLine.GetComponent<LineRenderer>().SetPosition(1, hook.transform.position);

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
        if (preSetUp)
        {
            angleTxt.text = "Θ = ?";
            angleTxt.gameObject.transform.position = hookLauncher.transform.position;
            angularAnotation.transform.position = gun.transform.position;
            hookLauncher.transform.position = gun.transform.position;
            trail.transform.position = hookLauncher.transform.position;
            theCircular._origin = new Vector2(hookLauncher.transform.position.x, hookLauncher.transform.position.y);
            theCircular._degrees = angleGiven;
            theCircular.initialAngle = 45 - (angleGiven - 45);
            distanceX = (float)System.Math.Round(target.transform.position.x - hookLauncher.transform.position.x, 2);
            distanceY = (float)System.Math.Round(target.transform.position.y - hookLauncher.transform.position.y, 2);
            angleGiven = (float)System.Math.Round(Mathf.Atan(((distanceY + ((9.81f / 2) * (projectileTime * projectileTime))) / distanceX)) * Mathf.Rad2Deg, 2);
            Vo = distanceX / (Mathf.Cos((angleGiven * Mathf.Deg2Rad)) * projectileTime);
            angularAnotation.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angleGiven);
            //VoTxt.text = "Vo = "+ Vo.ToString("F2")+" m/s";
        }
        if (questionController.isSimulating)
        {
            angleTxt.text = "Θ = "+ questionController.GetPlayerAnswer().ToString("F2")+"°";
            preSetUp = false;
            theCircular._degrees = angleGiven;
            theCircular.initialAngle = 45 - (questionController.GetPlayerAnswer() - 45);
            hookLauncher.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, questionController.GetPlayerAnswer());
            angularAnotation.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, questionController.GetPlayerAnswer());
            hook.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            hook.GetComponent<Rigidbody2D>().WakeUp();
            hookLine.SetActive(true);
            StartCoroutine(shoot());
            questionController.isSimulating = false;
        }

    }
}
