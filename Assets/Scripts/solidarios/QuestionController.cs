using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;




public class QuestionController : MonoBehaviour
{
    public enum Difficulty : byte
    {
        Easy = 0,
        Hard = 1,
        Difficult = 2
    }

    public enum Orientation : byte
    {
        Horizontal = 0,
        Vertical = 1
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
    TMP_Text resultHeadingText;
    public Camera renderCamera;
    public Orientation orientation;
    public int stageNumber;
    public int levelNumber;
    public UnitOf unitOf;
    public Difficulty levelDifficulty;
    private Transform baseComponent;
    private Transform modalComponent;
    public string modalTitle;
    public string question;
    private Transform problemBox;
    public string levelName;
    Transform difficultyName;
    string answerUnit;
    float playerAnswer;

    TMP_InputField answerField;
    Transform stageName;

    // Start is called before the first frame update
    void Start()
    {
        baseComponent = transform.Find("Base");
        modalComponent = transform.Find("Modal");
        problemBox = baseComponent.Find("ProblemBox");       
        stageName = problemBox.Find("StageBar2").Find("StageName");
        difficultyName = problemBox.Find("StageBar3").Find("DifficultyName");
        baseComponent.GetComponent<Canvas>().worldCamera = renderCamera;
        modalComponent.GetComponent<Canvas>().worldCamera = renderCamera;
    }

    public void SetQuestion(string qstn)
    {
        question = qstn;
    }

    public float GetPlayerAnswer()
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
            // playBtn.interactable = false;
            // timerTxt.text = stuntTimer + "s";
        }
    }

    public void Retry()
    {
        modalComponent.gameObject.SetActive(false);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    IEnumerator IsEmpty()
    {
        // warningTxt.text = "Please enter your answer!";
        yield return new WaitForSeconds(1);
        // warningTxt.text = "";
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

    // Update is called once per frame
    void Update()
    {
        
        problemBox.Find("StageBar1").Find("LevelName").GetComponent<TMP_Text>().SetText($"{levelName}");
        stageName.GetComponent<TMP_Text>().SetText($"Stage {stageNumber}");
        difficultyName.GetComponent<TMP_Text>().SetText($"{levelDifficulty}");
        problemBox.Find("ProblemTextHorizontal").GetComponent<TMP_Text>().SetText(question);
    }
}
