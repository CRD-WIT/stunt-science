using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using GameConfig;

public class QuestionController : MonoBehaviour
{
    float playerAnswer;
    GameObject correctIcon;
    GameObject wrongIcon;
    private Transform baseComponent;
    private Transform modalComponent;
    private Transform extraComponent;
    private Transform problemBox;
    public bool answerIsCorrect = false;
    public bool isModalOpen = true;
    public Camera renderCamera;
    public Color correctAnswerColor;
    public Color givenColor;
    public Color wrongAnswerColor;
    public Difficulty levelDifficulty;
    public int levelNumber;
    public int stageNumber;
    public Orientation orientation;
    public string levelName;
    public string modalTitle;
    public string question;
    public TextColorMode colorMode;
    public UnitOf unitOf;
    string answerUnit;
    TMP_InputField answerField;
    Transform difficultyName;
    Transform stageName;
    public bool isSimulating;
    GameObject playButton;
    GameObject timerComponent;




    // Start is called before the first frame update
    void Start()
    {

        baseComponent = transform.Find("Base");
        modalComponent = transform.Find("Modal");
        extraComponent = transform.Find("Extra");

        Transform[] components = { baseComponent, modalComponent, extraComponent };

        foreach (Transform component in components)
        {
            component.GetComponent<Canvas>().worldCamera = renderCamera;
        }


        problemBox = baseComponent.Find("ProblemBox");
        playButton = problemBox.Find("ProblemBoxHorizontal").Find("AnswerBox").Find("PlayButton").gameObject;
        stageName = problemBox.Find("StageBar2").Find("StageName");
        difficultyName = problemBox.Find("StageBar3").Find("DifficultyName");
        correctIcon = modalComponent.Find("WrongIcon").gameObject;
        wrongIcon = modalComponent.Find("CorrectIcon").gameObject;
        timerComponent = extraComponent.Find("Timer").gameObject;

        correctIcon.SetActive(false);
        wrongIcon.SetActive(false);

        givenColor = new Color32(0x73, 0x2b, 0xc2, 0xff);
        correctAnswerColor = new Color32(150, 217, 72, 255);
        wrongAnswerColor = new Color32(237, 66, 66, 255);
    }

    public void ToggleModal()
    {
        isModalOpen = !isModalOpen;
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

    public string getHexColor(TextColorMode mode)
    {
        switch (mode)
        {
            case TextColorMode.Wrong:
                return ColorUtility.ToHtmlStringRGB(wrongAnswerColor);
            case TextColorMode.Correct:
                return ColorUtility.ToHtmlStringRGB(correctAnswerColor);
            default:
                return ColorUtility.ToHtmlStringRGB(givenColor);
        }
    }

    public void SetColor(TMP_Text target, TextColorMode mode)
    {
        switch (mode)
        {
            case TextColorMode.Wrong:
                target.color = wrongAnswerColor;
                break;
            case TextColorMode.Correct:
                target.color = correctAnswerColor;
                break;
            default:
                target.color = givenColor;
                break;
        }
    }



    void Update()
    {
        TMP_Text modalTitleObj = modalComponent.Find("ModalTitle").GetComponent<TMP_Text>();
        modalTitleObj.SetText(modalTitle);

        playButton.SetActive(!isSimulating);
        timerComponent.SetActive(isSimulating);

        correctIcon.SetActive(!answerIsCorrect);
        wrongIcon.SetActive(answerIsCorrect);

        if (answerIsCorrect)
        {
            SetColor(modalTitleObj, TextColorMode.Correct);
        }
        else
        {
            SetColor(modalTitleObj, TextColorMode.Wrong);

        }
        modalComponent.gameObject.SetActive(isModalOpen);
        problemBox.Find("StageBar1").Find("LevelName").GetComponent<TMP_Text>().SetText($"{levelName}");
        extraComponent.Find("LevelNumber").GetComponent<TMP_Text>().SetText($"{levelNumber}");
        stageName.GetComponent<TMP_Text>().SetText($"Stage {stageNumber}");
        difficultyName.GetComponent<TMP_Text>().SetText($"{levelDifficulty}");
        problemBox.Find("ProblemTextHorizontal").GetComponent<TMP_Text>().SetText(question);
    }
}
