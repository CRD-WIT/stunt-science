using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Level_3_Stage_3_Medium : MonoBehaviour
{
    // Start is called before the first frame update
    string question, gender, pronoun, pronoun2;
    public TMP_Text initalVelociyText, questionText, levelName, VoTxt, angleTxt, timerTxt;
    bool isSimulating = false;
    public GameObject hook, hookLauncher, thePlayerRunning, shootPosTriger, puller, angularAnotation, gun, dimensions, target;
    public GameObject hookLine, trail, playButton, targetLock,targetWall;
    public GameObject hookIndicator;
    public playerProjectileMed thePlayer;
    public float distanceX, distanceY;
    public float angleGiven, projectileTime, Vo, timer, correctAnswer,projectileTimeCheck;

    public bool collided, preSetUp, startTime, showResult;
    public CircularAnnotation theCircular;
    public CameraScript cameraScript;
    public Hook theHook;
    public QuestionController questionController;
    void Start()
    {
        gender = PlayerPrefs.GetString("Gender");
        if (gender == "Male")
        {
            pronoun = ("he");
            pronoun2 = ("his");
        }
        if (gender == "Female")
        {
            pronoun = ("she");
            pronoun2 = ("her");
        }
        questionController.SetQuestion("......");
    }
    public void StartSimulation()
    {
        playButton.SetActive(false);
        timerTxt.gameObject.SetActive(true);
        cameraScript.directorIsCalling = true;
        cameraScript.isStartOfStunt = true;
        questionController.SetAnswer();
    }
    public void showProblem()
    {
        targetLock.SetActive(true);
        showResult = true;
        preSetUp = true;
        angleGiven = (float)System.Math.Round(Random.Range(55f, 65f), 2);
    }
    public IEnumerator shoot()
    {
        hook.SetActive(true);
        hook.transform.position = hookLauncher.transform.position;
        if (questionController.GetPlayerAnswer() == correctAnswer)
        {
            hook.GetComponent<Rigidbody2D>().velocity = hookLauncher.transform.right * (Vo);
            targetWall.SetActive(false);
        }
        if (questionController.GetPlayerAnswer() > correctAnswer)
        {
            hook.GetComponent<Rigidbody2D>().velocity = hookLauncher.transform.right * (Vo - .5f);
        }
        if (questionController.GetPlayerAnswer() < correctAnswer)
        {
            hook.GetComponent<Rigidbody2D>().velocity = hookLauncher.transform.right * (Vo + .5f);
        }

        yield return new WaitForEndOfFrame();
        theHook.isTrailing = true;
        trail.GetComponent<TrailRenderer>().time = 3000;

    }
    IEnumerator StuntResult()
    {
        if (questionController.GetPlayerAnswer() == correctAnswer)
        {
            yield return new WaitForSeconds(4);
            questionController.answerIsCorrect = true;
            questionController.ToggleModal($"<b>Stunt Success!!!</b>", $"{PlayerPrefs.GetString("Name")} safely grabbed the pole!", "Next");

        }
        if (questionController.GetPlayerAnswer() != correctAnswer)
        {
            yield return new WaitForSeconds(1);
            questionController.answerIsCorrect = false;
            questionController.ToggleModal($"<b>Stunt failed!!!</b>", $"{PlayerPrefs.GetString("Name")} missed the target!", "retry");

        }

    }
    public void action()
    {
        if (questionController.GetPlayerAnswer() == correctAnswer)
        {
            SceneManager.LoadScene("LevelThreeStage3Medium");
        }
        else
        {
            SceneManager.LoadScene("LevelThreeStage2Medium");
        }


    }

    void FixedUpdate()
    {
        hookLine.GetComponent<LineRenderer>().SetPosition(0, gun.transform.position);
        hookLine.GetComponent<LineRenderer>().SetPosition(1, hook.transform.position);

        if (thePlayerRunning.GetComponent<RunningPlayer>().isCollided)
        {
            if (!collided)
            {
                playButton.SetActive(true);
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
            theCircular._origin = new Vector2(hookLauncher.transform.position.x + .3f, hookLauncher.transform.position.y - .1f);
            theCircular._degrees = angleGiven;
            theCircular.initialAngle = 45 - (angleGiven - 45);
            distanceX = (float)System.Math.Round(target.transform.position.x - hookLauncher.transform.position.x, 2);
            distanceY = (float)System.Math.Round(target.transform.position.y - hookLauncher.transform.position.y, 2);
            //angleGiven = Mathf.Atan(((distanceY + ((9.81f / 2) * (projectileTime * projectileTime))) / distanceX)) * Mathf.Rad2Deg;
            correctAnswer = angleGiven;
            projectileTime = Mathf.Sqrt((((2 * distanceX)*(Mathf.Tan(angleGiven* Mathf.Deg2Rad)))-distanceY)/9.81f);
            Vo = ((Mathf.Sqrt((Mathf.Abs(Physics2D.gravity.y) / (2 * ((distanceX * (Mathf.Tan((angleGiven) * Mathf.Deg2Rad))) - distanceY))))) *distanceX) / (Mathf.Cos((angleGiven) * Mathf.Deg2Rad));
            angularAnotation.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angleGiven);
            theHook.correctAnswer = correctAnswer;
            question = $"{PlayerPrefs.GetString("Name")} is now instructed to fire {pronoun2} climbing device and must hit the target to be able to cross on the other cliff. If the target horizontally <b>{distanceX}m</b> away and vertically <b>{distanceY}m</b> above from the barrel of the climbing device, What should be the shooting angle of the climbing device if the target can only be hit with a projectile time <b>{projectileTime} seconds</b> ?";
            questionController.SetQuestion(question);
            //VoTxt.text = "Vo = "+ Vo.ToString("F2")+" m/s";
        }
        if (questionController.isSimulating)
        {
            targetLock.SetActive(false);
            startTime = true;
            angleTxt.text = "Θ = " + questionController.GetPlayerAnswer().ToString("F2") + "°";
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
        if (startTime)
        {
            timer += Time.fixedDeltaTime;
            timerTxt.text = timer.ToString("F2") + "s";
            if (timer >= projectileTime)
            {
                startTime = false;
                timer = projectileTime;
                timerTxt.text = timer.ToString("F2") + "s";
            }
        }
        if (showResult)
        {
            if (theHook.isCollided)
            {
                StartCoroutine(StuntResult());
                showResult = false;
            }
        }


    }
}
