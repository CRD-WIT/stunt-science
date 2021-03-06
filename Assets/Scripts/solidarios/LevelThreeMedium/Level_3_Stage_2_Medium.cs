using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level_3_Stage_2_Medium : MonoBehaviour
{
    AnswerGuards answerGuards = new AnswerGuards();
    // Start is called before the first frame update
    string question, gender, pronoun, pronoun2;
    public TMP_Text initalVelociyText, questionText, levelName, VoTxt, angleTxt, timerTxt;
    bool isSimulating = false;
    public GameObject hook, hookLauncher, thePlayerRunning, shootPosTriger, puller, angularAnotation, gun, dimensions, target;
    public GameObject hookLine, trail, targetLock, targetHere;
    public GameObject hookIndicator;
    public playerProjectileMed thePlayer;

    public float distanceX, distanceY;
    public float angleGiven, projectileTime, Vo, timer, correctAnswer;

    public bool collided, preSetUp, startTime, showResult;
    public CircularAnnotation theCircular;
    public CameraScript cameraScript;
    public Hook theHook;
    public QuestionContProjMed questionController;
    public GameObject directorBubble;
    public TMP_Text debugAnswer;
    bool directorIsCalling;
    public TMP_Text diretorsSpeech;
    public TMP_InputField answerField;
    public Button playButton;
    float adjustedAnswer;
    static float playerAnswer;
    public HeartManager theHeart;
    public AudioSource lightsSfx, cameraSfx, actionSfx, cutSfx;
    public FirebaseManager firebaseManager;
    public AudioSource gunFire, maneuverGearSfx;
    void Start()
    {
        theHeart.startbgentrance();
        targetHere.SetActive(true);
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
        preSetUp = true;
        projectileTime = (float)System.Math.Round(Random.Range(2.7f, 3.2f), 2);
        questionController.SetQuestion("......");
    }
    public IEnumerator DirectorsCall()
    {
        if (directorIsCalling)
        {
            directorBubble.SetActive(true);
            //diretorsSpeech.text = "Take " + take + ("!");
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "Lights!";
            lightsSfx.Play();
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "Camera!";
            cameraSfx.Play();
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "Action!";
            actionSfx.Play();
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "";
            directorBubble.SetActive(false);
            questionController.isSimulating = true;
            directorIsCalling = false;
        }
        else
        {
            directorBubble.SetActive(true);
            diretorsSpeech.text = "Cut!";
            cutSfx.Play();
            yield return new WaitForSeconds(1);
            directorBubble.SetActive(false);
            diretorsSpeech.text = "";
        }
    }
    public void StartSimulation()
    {
        targetHere.SetActive(false);
        playerAnswer = questionController.GetPlayerAnswer();
        adjustedAnswer = questionController.AnswerTolerance(correctAnswer);     

        if (answerField.text == "" || adjustedAnswer > 89 || adjustedAnswer < 45)
        {

            questionController.errorText = ("answer must not exceed between 45?? to 89??");
            StartCoroutine(errorMesage());
        }
        else
        {
            directorIsCalling = true;
            StartCoroutine(DirectorsCall());
            playButton.interactable = false;
            {
                answerField.text = playerAnswer.ToString() + "??";
            }

        }
    }
    public IEnumerator errorMesage()
    {
        questionController.popupVisible = true;
        yield return new WaitForSeconds(3);
        questionController.popupVisible = false;
    }
    public void showProblem()
    {
        question = $"{PlayerPrefs.GetString("Name")} is now instructed to fire {pronoun2} climbing device and must hit the target to be able to cross on the other cliff. If the target horizontally <b>{distanceX}m</b> away and vertically <b>{distanceY}m</b> above from the barrel of the climbing device, What should be the shooting angle of the climbing device if the target can only be hit with a projectile time <b>{projectileTime} seconds</b> ?";
        questionController.SetQuestion(question);
        targetLock.SetActive(true);
        showResult = true;
         questionController.answerIsCorrect = false;

    }
    public IEnumerator shoot()
    {
        hook.SetActive(true);
        hook.transform.position = hookLauncher.transform.position;
        gunFire.Play();
        maneuverGearSfx.Play();
        
        if (adjustedAnswer == correctAnswer)
        {
             questionController.answerIsCorrect = true;
            hook.GetComponent<Rigidbody2D>().velocity = hookLauncher.transform.right * (Vo);
        }
        else
        {
            if (questionController.GetPlayerAnswer() > correctAnswer)
            {
                hook.GetComponent<Rigidbody2D>().velocity = hookLauncher.transform.right * (Vo - .5f);
            }
            if (questionController.GetPlayerAnswer() < correctAnswer)
            {
                hook.GetComponent<Rigidbody2D>().velocity = hookLauncher.transform.right * (Vo + .5f);
            }
            questionController.answerIsCorrect = false;
           
        }


        yield return new WaitForEndOfFrame();
        theHook.isTrailing = true;
        trail.GetComponent<TrailRenderer>().time = 3000;

    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(2f);
        if (adjustedAnswer == correctAnswer)
        {
           
            // questionController.ActivateResult((PlayerPrefs.GetString("Name") + " has successfully performed the stunt and able to hit the target"), true, false);
            questionController.ActivateResult($"{PlayerPrefs.GetString("Name")} aimed  and shot the gripping projectile in just the precise angle to hit the gripping point! Stunt successfully executed! ", true, false);
        }
        else
        {
             
            theHeart.ReduceLife();
            // questionController.ActivateResult((PlayerPrefs.GetString("Name") + " has unable to hit and grab the target"), false, false);
            if(playerAnswer > correctAnswer){
                questionController.ActivateResult($"{PlayerPrefs.GetString("Name")} aimed the gripping projectile too high and missed hitting the gripping point. Stunt failed! The correct answer is {correctAnswer} m/s", false, false);
            }else{
                questionController.ActivateResult($"{PlayerPrefs.GetString("Name")} aimed the gripping projectile too low and missed hitting the gripping point. Stunt failed! The correct answer is {correctAnswer} m/s", false, false);
            }
            
        }
    }
    public void action()
    {
        
        if (adjustedAnswer == correctAnswer)
        {
            SceneManager.LoadScene("LevelThreeStage3.1Medium");
        }
        else
        {
            SceneManager.LoadScene("LevelThreeStage2Medium");
        }


    }

    void FixedUpdate()
    {
        debugAnswer.SetText($"Answer: {System.Math.Round(correctAnswer, 2)}");

        hookLine.GetComponent<LineRenderer>().SetPosition(0, gun.transform.position);
        hookLine.GetComponent<LineRenderer>().SetPosition(1, hook.transform.position);

        if (thePlayerRunning.GetComponent<RunningPlayer>().isCollided)
        {
            if (!collided)
            {
                playButton.gameObject.SetActive(true);
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
            angleTxt.text = "?? = ?";
            angleTxt.gameObject.transform.position = hookLauncher.transform.position;
            angularAnotation.transform.position = gun.transform.position;
            hookLauncher.transform.position = gun.transform.position;
            trail.transform.position = hookLauncher.transform.position;
            theCircular._origin = new Vector2(hookLauncher.transform.position.x + .3f, hookLauncher.transform.position.y - .1f);
            theCircular._degrees = angleGiven;
            theCircular.initialAngle = 45 - (angleGiven - 45);
            distanceX = (float)System.Math.Round(target.transform.position.x - hookLauncher.transform.position.x, 2);
    
            angleGiven = (float)System.Math.Round(Mathf.Atan(((distanceY + ((9.81f / 2) * (projectileTime * projectileTime))) / distanceX)) * Mathf.Rad2Deg, 2);
            correctAnswer = angleGiven;
            Vo = distanceX / (Mathf.Cos((angleGiven * Mathf.Deg2Rad)) * projectileTime);
            angularAnotation.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angleGiven);
            theHook.correctAnswer = correctAnswer;

            //VoTxt.text = "Vo = "+ Vo.ToString("F2")+" m/s";
        }
        if (questionController.isSimulating)
        {
            targetLock.SetActive(false);
            startTime = true;
            angleTxt.text = "?? = " + questionController.GetPlayerAnswer().ToString("F2") + "??";
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
