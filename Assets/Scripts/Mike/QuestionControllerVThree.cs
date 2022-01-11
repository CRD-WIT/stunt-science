using System.Collections;
using UnityEngine;
using TMPro;
using GameConfig;
using UnityEngine.UI;

public class QuestionControllerVThree : MonoBehaviour
{
    float playerAnswer;
    public accSimulation simulationManager;
    public float limit = 0;
    private Transform baseComponent, problemBox, extraComponent, levelBadge;
    public bool answerIsCorrect = false, isModalOpen = true, isSimulating, nextStage, retried;
    public Color correctAnswerColor, givenColor, wrongAnswerColor;
    public Difficulty levelDifficulty;
    public int levelNumber, stage;
    public string levelName, modalTitle, question, timer;
    public TextColorMode colorMode;
    public Settings settingUI;
    public UnitOf unit;
    public string answerUnit, difficulty;
    public HeartManager heartManager;
    int passedLevel;
    [SerializeField] bool timerOn = false, loaded = false;
    [SerializeField] TMP_InputField answerFieldHorizontal;
    [SerializeField] Transform difficultyName, stageName;
    public string modalText, errorText;
    [SerializeField] bool popupVisible, extraOn;
    public FirebaseManager firebaseManager;

    [SerializeField]
    GameObject modalComponentHorizontal, popupComponentHorizontal, playButtonHorizontal, timerComponentHorizontal, problemBoxHorizontal,
            problemTextHorizontal, modalTextHorizontal, modalTitleHorizontal, wrongIconHorizontal, correctIconHorizontal;
    [SerializeField] TMP_Text popupTextHorizontal;
    [SerializeField] Button actionBtn;
    StageManager level = new StageManager();
    HeartManager life;

    // Start is called before the first frame update
    void Start()
    {
        baseComponent = transform.Find("Base");
        extraComponent = transform.Find("extraCanvas").Find("Extra");
        levelBadge = baseComponent.Find("LevelBadge");

        Transform[] components = { baseComponent, modalComponentHorizontal.transform, extraComponent };
        problemBox = baseComponent.Find("ProblemBox");
        stageName = problemBox.Find("StageBar2").Find("StageName");
        difficultyName = problemBox.Find("StageBar3").Find("DifficultyName");

        givenColor = new Color32(0x73, 0x2b, 0xc2, 0xff);
        correctAnswerColor = new Color32(150, 217, 72, 255);
        wrongAnswerColor = Color.red;

        levelName = level.GetGameLevel();
        switch (levelDifficulty)
        {
            case Difficulty.Easy:
                difficulty = "Easy";
                break;
            case Difficulty.Medium:
                difficulty = "Medium";
                break;
            case Difficulty.Hard:
                difficulty = "Hard";
                break;
        }
        difficultyName.GetComponent<TMP_Text>().text = difficulty;

        levelNumber = level.GetLevelNum(levelName);
        if (level.GetLevelNum(levelName) > 3)
            levelNumber--;

        life = FindObjectOfType<HeartManager>();
    }
    public void ActionBtn(bool endLevel)
    {
        if (endLevel)
        {

        }
        else
        {
            answerFieldHorizontal.text = "";
            if (answerIsCorrect)
                Next();
            else
                StartCoroutine(Retry());
            isModalOpen = false;
            timerOn = false;
        }

    }

    public void EndLevel()
    {

    }

    public void EvaluatePlayerScore()
    {
        string playerPrefsName = "";
        switch (levelDifficulty)
        {
            case Difficulty.Easy:
                playerPrefsName = ($"level{levelName}Easy");
                break;
            case Difficulty.Medium:
                playerPrefsName = ($"level{levelName}Medium");
                break;
            case Difficulty.Hard:
                playerPrefsName = ($"level{levelName}Hard");
                break;
        }

        Debug.Log($"Player Score: {playerPrefsName}:{heartManager.life}");
        PlayerPrefs.SetInt(playerPrefsName, heartManager.life);
        settingUI.ToggleFlashCardEnd();
        //SceneManager.LoadScene("LevelSelectV2");

    }
    public void ActivateResult(string message, bool isCorrect, bool isComplete = false)
    {
        Debug.Log("ActivateResult Triggered");
        answerIsCorrect = isCorrect;
        isModalOpen = true;
        if (isCorrect)
        {
            // NOTE: Use this template when ending levels.
            if (isComplete)
            {
                Debug.Log($"QCV3: isComplete {isComplete}");
                // firebaseManager.GameLogMutation(levelNumber, stage, difficulty, Actions.Completed, 0);
                actionBtn.GetComponent<Button>().onClick.RemoveAllListeners();
                actionBtn.GetComponent<Button>().onClick.AddListener(EvaluatePlayerScore);

                actionBtn.transform.Find("BtnName").GetComponent<TMP_Text>().text = "Finish";
                modalTitle = "Stunts Completed!";
                modalText = message;
                SetColor(modalTitleHorizontal.GetComponent<TMP_Text>(), TextColorMode.Correct);
            }
            else
            {
                actionBtn.transform.Find("BtnName").GetComponent<TMP_Text>().text = "Next";
                modalTitle = "Correct Answer!";
                modalText = message;
                SetColor(modalTitleHorizontal.GetComponent<TMP_Text>(), TextColorMode.Correct);
            }
        }
        else
        {
            //TODO: Check current life points. If life == 0, hide the action button.

            actionBtn.transform.Find("BtnName").GetComponent<TMP_Text>().text = "Retry";
            modalTitle = "Wrong Answer!";
            modalText = message;
            SetColor(modalTitleHorizontal.GetComponent<TMP_Text>(), TextColorMode.Wrong);
        }
        actionBtn.interactable = true;
    }
    public void SetQuestion(string qstn)
    {
        question = qstn;
    }

    public float GetPlayerAnswer()
    {
        return this.playerAnswer;
    }

    public void AnswerLimiter()
    {
        string value = answerFieldHorizontal.text;
        // Bug when using whole number
        string[] splitted = value.Split('.');
        answerFieldHorizontal.characterLimit = splitted[0].Length + 3;
    }
    public void SetAnswer()
    {
        if (answerFieldHorizontal.text == "")
        {
            errorText = "Please enter your answer!";
            StartCoroutine(IsEmpty());
        }
        else
        {
            playerAnswer = float.Parse(answerFieldHorizontal.text);
            answerFieldHorizontal.text = playerAnswer + answerUnit;
            if (limit < playerAnswer)
            {
                errorText = $"Invalid answer! Answer must not exceed {limit}{answerUnit}.";
                StartCoroutine(IsEmpty());
            }
            // TODO: Replicate to all question controllers
            else if(playerAnswer <= 0){
                errorText = $"Invalid answer! Answer must be a greater than 0.";
                StartCoroutine(IsEmpty());
            }
            else
            {
                timerOn = true;
                isSimulating = true;
            }
        }
        extraOn = true;
    }

    public float AnswerTolerance(float correctAnswer){
        float answer = 0, offset = 0;
        offset = (float)System.Math.Round(Mathf.Abs(correctAnswer - GetPlayerAnswer()),2);
        if(offset <= 0.01f){
            answer = correctAnswer;
        }
        else
        {
            answer = GetPlayerAnswer(); 
        }
        Debug.Log(offset + "offset");
        return answer;
    }
    
    public void Next()
    {
        if (stage == 1)
        {
            stage = 2;
            nextStage = true;
        }
        else if (stage == 2)
        {
            stage = 3;
            nextStage = true;
        }
    }
    IEnumerator Retry()
    {
        // TODO: Fix delay for background intro.
        extraOn = false;
        answerFieldHorizontal.text = "";
        retried = true;
        if (stage == 1)
            stage = 1;
        else if (stage == 2)
            stage = 2;
        else
            stage = 3;
        yield return new WaitForEndOfFrame();
        isModalOpen = false;
    }

    IEnumerator IsEmpty()
    {
        popupVisible = true;
        //errorText = "Please enter your answer!";
        yield return new WaitForSeconds(3);
        popupVisible = false;
        answerFieldHorizontal.text = "";
        //errorText = "";
    }

    public string Unit(UnitOf unitOf)
    {
        string unit = "";
        switch (unitOf)
        {
            case UnitOf.distance:
                unit = " m";
                break;
            case UnitOf.time:
                unit = " s";
                break;
            case UnitOf.velocity:
                unit = " m/s";
                break;
            case UnitOf.acceleration:
                unit = " m/s²";
                break;
            case UnitOf.angle:
                unit = " °";
                break;
            case UnitOf.angularVelocity:
                unit = " °/s";
                break;
            case UnitOf.force:
                unit = " N";
                break;
            case UnitOf.mass:
                unit = " kg";
                break;
            case UnitOf.work:
                unit = " J";
                break;
            case UnitOf.energy:
                unit = " kW";
                break;
            case UnitOf.power:
                unit = " kWh";
                break;
            case UnitOf.momuntum:
                unit = " kg•m/s";
                break;
        }
        return unit;
    }
    public void SetUnitTo(UnitOf unitOf)
    {
        switch (unitOf)
        {
            case UnitOf.distance:
                answerUnit = " m";
                break;
            case UnitOf.time:
                answerUnit = " s";
                break;
            case UnitOf.velocity:
                answerUnit = " m/s";
                break;
            case UnitOf.acceleration:
                answerUnit = " m/s²";
                break;
            case UnitOf.angle:
                answerUnit = "°";
                break;
            case UnitOf.angularVelocity:
                answerUnit = "°/s";
                break;
            case UnitOf.force:
                answerUnit = " N";
                break;
            case UnitOf.mass:
                answerUnit = " kg";
                break;
            case UnitOf.work:
                answerUnit = " J";
                break;
            case UnitOf.energy:
                answerUnit = " kW";
                break;
            case UnitOf.power:
                answerUnit =  "kWh";
                break;
            case UnitOf.momuntum:
                answerUnit = " kg•m/s";
                break;
        }
    }
    public Color getHexColor(TextColorMode mode)
    {
        switch (mode)
        {
            case TextColorMode.Wrong:
                return wrongAnswerColor;
            case TextColorMode.Correct:
                return correctAnswerColor;
            default:
                return givenColor;
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
        //levelNumber = level.GetLevelNum(levelName);
        correctIconHorizontal.SetActive(answerIsCorrect);
        wrongIconHorizontal.SetActive(!answerIsCorrect);
        popupComponentHorizontal.SetActive(popupVisible);
        popupTextHorizontal.SetText(errorText);
        modalComponentHorizontal.gameObject.SetActive(isModalOpen);
        modalTitleHorizontal.GetComponent<TMP_Text>().SetText(modalTitle);
        problemTextHorizontal.GetComponent<TMP_Text>().SetText(question);
        modalTextHorizontal.GetComponent<TMP_Text>().SetText(modalText);
        problemTextHorizontal.SetActive(!isModalOpen);
        // playButtonHorizontal.SetActive(!isModalOpen);
        answerFieldHorizontal.gameObject.SetActive(!isModalOpen);

        extraComponent.gameObject.SetActive(timerOn || popupVisible);
        playButtonHorizontal.SetActive(!timerOn);
        // timerComponentHorizontal.gameObject.SetActive(timerOn);
        timerComponentHorizontal.GetComponent<TMP_Text>().SetText(timer);

        problemBox.Find("StageBar1").Find("LevelName").GetComponent<TMP_Text>().SetText($"{levelName}");
        levelBadge.Find("LevelNumber").GetComponent<TMP_Text>().SetText($"{levelNumber}");
        stageName.GetComponent<TMP_Text>().SetText($"Stage {stage}");
        difficultyName.GetComponent<TMP_Text>().SetText($"{levelDifficulty}");
    }
}
/*That means that Bolt's speed during his world-record run was 10.44 meters per second.
Since many people are more familiar with automobiles and speed limits, it might be more
useful to think of this in terms of kilometers per hour or miles per hour: 37.58 or 23.35, respectively
Usain Bolt of Jamaica, set at the 2009 World Athletics Championships final in Berlin, Germany on 16 August 2009,

Average human speed is 1.4m/s.*/


// TODO: Implement limiter