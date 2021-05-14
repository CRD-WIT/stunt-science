using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SimulationUIManager : MonoBehaviour
{
    TMP_Text questionTxt, warningTxt, resultHeadingTxt, resultMessageTxt, timerTxt;
    Button playBtn, nextBtn, retryBtn;
    TMP_InputField answerField;
    GameObject result;
    float playerAnswer;
    int currentLevel, passedLevel = 0;
    string answerUnit;
    public UnitOf unitOf;
    public int stage;
    public float stuntTimer;
    StageManager level = new StageManager();
    public void SetQuestion(string qstn)
    {
        questionTxt.text = qstn;
    }
    public float GetAnswer()
    {
        return playerAnswer;
    }
    public void SetAnswer()
    {
        if (answerField.text == "")
            StartCoroutine(IsEmpty());
        else
        {
            playerAnswer = float.Parse(answerField.text);
            answerField.text = playerAnswer + answerUnit;
            playBtn.interactable = false;
            timerTxt.text = stuntTimer + "s";
        }
    }
    public void ActivateResult(string message, bool isCorrect)
    {
        result.SetActive(true);
        if (isCorrect)
        {
            nextBtn.gameObject.SetActive(true);
            retryBtn.gameObject.SetActive(false);
            resultHeadTxt.text = "Correct!";
        }
        else
        {
            retryBtn.gameObject.SetActive(true);
            nextBtn.gameObject.SetActive(false);
            resultHeadTxt.text = "Wrong!";
        }
        resultMessageTxt.text = message;
        playBtn.interactable = true;
        timerTxt.text = "";
    }
    public void Next()
    {
        result.SetActive(false);
        if (stage == 1)
            stage = 2;
        else if (stage == 2)
            stage = 3;
        else
        {
            string difficulty = level.GetDifficulty();
            if (difficulty == "easy")
            {
                stage = 1;
                level.SetDifficulty(2);
            }
            else if (difficulty == "medium")
            {
                stage = 1;
                level.SetDifficulty(3);
            }
            else
            {
                passedLevel++;
                level.SetDifficulty(1);
                stage = 1;
                switch (level.GetGameLevel())
                {
                    case "Velocity":
                        level.SetGameLevel(2);
                        break;
                    case "Acceleration":
                        level.SetGameLevel(3);
                        break;
                    case "Free Fall":
                        level.SetGameLevel(4);
                        break;
                    case "Projectile Motion":
                        level.SetGameLevel(5);
                        break;
                    case "Circular Motion":
                        level.SetGameLevel(6);
                        break;
                    case "Forces":
                        level.SetGameLevel(7);
                        break;
                    case "Work":
                        level.SetGameLevel(8);
                        break;
                    case "Energy":
                        level.SetGameLevel(9);
                        break;
                    case "Power":
                        level.SetGameLevel(10);
                        break;
                    case "Momentum":
                        //Done
                        break;
                }
            }
            SceneManager.LoadScene("LevelSelection");
        }
    }
    public void Retry()
    {
        result.SetActive(false);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public string Unit()
    {
        //TODO: passed the appropriate unit in the answerField suffixed to the answer
        switch (unitOf)
        {
            case UnitOf.distance:
                answerUnit = "m";
                break;
            case UnitOf.time:
                answerUnit = "s";
                break;
            case UnitOf.velocity:
                answerUnit = "m/s";
                break;
            case UnitOf.acceleration:
                answerUnit = "m/s²";
                break;
            case UnitOf.angle:
                answerUnit = "°";
                break;
            case UnitOf.angularVelocity:
                answerUnit = "°/s";
                break;
            case UnitOf.force:
                answerUnit = "N";
                break;
            case UnitOf.mass:
                answerUnit = "kg";
                break;
            case UnitOf.work:
                answerUnit = "J";
                break;
            case UnitOf.energy:
                answerUnit = "kW";
                break;
            case UnitOf.power:
                answerUnit = "kWh";
                break;
            case UnitOf.momuntum:
                answerUnit = "kg•m/s";
                break;
        }
        return answerUnit;
    }
    void Start()
    { //provide gameObject.name inside the quotation marks.
        questionTxt = GameObject.Find("").GetComponent<TMP_Text>();
        warningTxt = GameObject.Find("").GetComponent<TMP_Text>();
        resultHeadingTxt = GameObject.Find("").GetComponent<TMP_Text>();
        resultMessageTxt = GameObject.Find("").GetComponent<TMP_Text>();
        timerTxt = GameObject.Find("").GetComponent<TMP_Text>();

        playBtn = GameObject.Find("").GetComponent<Button>();
        nextBtn = GameObject.Find("").GetComponent<Button>();
        retryBtn = GameObject.Find("").GetComponent<Button>();

        answerField = GameObject.Find("").GetComponent<TMP_InputField>();

        result = GameObject.Find("");

        result.SetActive(false);
    }
    void Update()
    {
        currentLevel = PlayerPrefs.GetInt("Level");
        if (currentLevel >= passedLevel)
        {
            level.SetGameLevel(currentLevel + 1);
        }
        else
        {
            level.SetGameLevel(currentLevel);
        }
    }
    IEnumerator IsEmpty()
    {
        warningTxt.text = "Please enter your anser!";
        yield return new WaitForSeconds(1);
        warningTxt.text = "";
    }
}

public enum UnitOf : byte
{
    distance = 0,
    time = 1,
    velocity = 2,
    acceleration = 3,
    angle = 4,
    angularVelocity = 5,
    force = 6,
    mass = 7,
    work = 8, //J
    energy = 9,//kW
    power = 10,//kWh
    momuntum = 11//kgm/s
}


