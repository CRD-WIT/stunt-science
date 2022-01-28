using System.Collections;
using UnityEngine;
using TMPro;
using GameConfig;

public class Level5EasyManager : MonoBehaviour
{
    public TMP_Text debugAnswer;
    [SerializeField] HingeJoint2D pipeHanger;
    [SerializeField] TMP_Text directorsSpeech;
    [SerializeField]
    GameObject gear3, playerHangerTrigger1, playerHangerTrigger2, playerHangerTrigger3, ragdollPrefab, stage1Layout,
                stage2Layout, stage3Layout, gearSet, directorsBubble, ragdoll, directorPlatform, UI1, UI2, UI3;
    // [SerializeField] PlayerCM2 myPlayer;
    [SerializeField] float elapsed, aVelocity, gameTime, angle;
    [SerializeField] int stage;
    [SerializeField] Rigidbody2D gearRB, player;
    [SerializeField] Animator crank;
    [SerializeField] Transform safeZone;
    [SerializeField] PolygonCollider2D slider;
    [SerializeField] PlayerCM2 myPlayer;
    Vector2 playerPos;
    StageManager sm = new StageManager();
    RoundOffHandler CustomRoundOff = new RoundOffHandler();
    HeartManager2 life;
    QuestionController2_0_1 qc;
    ScoreManager score;
    ButtonSelector btnSelect;
    ButtonSelector1 btnSelect1;
    string pronoun, pNoun, playerName, playerGender, messageTxt, question;
    bool directorIsCalling, isStartOfStunt, stuntReady, DC, isCranking, crankingDone, crankReset, DCisOn, isAnswerCorect, isEnd = false;
    public static bool isHanging, cranked, isAnswered;
    public static float playerAnswer, gear2Speed;
    void Start()
    {
        myPlayer = FindObjectOfType<PlayerCM2>();
        life = FindObjectOfType<HeartManager2>();
        qc = FindObjectOfType<QuestionController2_0_1>();
        score = FindObjectOfType<ScoreManager>();
        btnSelect = FindObjectOfType<ButtonSelector>();
        btnSelect1 = FindObjectOfType<ButtonSelector1>();
        qc.stage = 1;
        isAnswered = false;
        sm.SetGameLevel(4);
        playerName = PlayerPrefs.GetString("Name");
        playerGender = PlayerPrefs.GetString("Gender");
        playerPos = myPlayer.transform.position;
        player = myPlayer.gameObject.GetComponent<Rigidbody2D>();
        qc.levelDifficulty = Difficulty.Easy;
        PlayerPrefs.SetInt("Life", 3);
        Lvl5EasySetUp();
    }
    void Update()
    {

        if (directorIsCalling)
        {
            StartCoroutine(DirectorsCall());
        }

        switch (stage)
        {
            case 1:
                debugAnswer.SetText($"Answer: {aVelocity}");
                break;
            case 2:
                debugAnswer.SetText($"Answer: {gameTime}");
                break;
            case 3:
                debugAnswer.SetText($"Answer: {angle}");
                break;
            default:
                debugAnswer.SetText($"Answer: Not Set");
                break;
        }


        if (isAnswered)
        {
            CurvedLineFollower.answerIs = null;
            switch (stage)
            {
                case 1:
                    if (elapsed < gameTime)
                    {
                        isHanging = true;
                        qc.timer = elapsed.ToString("f2") + "s";
                        elapsed = GearHangers.hangTime;
                        //float e = CustomRoundOff.Round(elapsed, 2);
                        CurvedLineFollower.arc = playerAnswer * elapsed;
                        myPlayer.gameObject.transform.localScale = new Vector2(-1f, 1f);
                    }
                    else //(elapsed >= gameTime)
                    {
                        isHanging = false;
                        if (playerAnswer == aVelocity)
                        {
                            isAnswerCorect = true;
                            CurvedLineFollower.arc = 210;
                            qc.timer = gameTime.ToString("f2") + "s";
                            messageTxt = "<b>" + playerName + "</b> has landed <color=green>safely</color> at the other platform!";
                        }
                        else
                        {
                            isAnswerCorect = false;
                            RagdollSpawn();
                            life.ReduceLife();
                            if (playerAnswer < aVelocity)
                            {
                                messageTxt = "<b>" + playerName + "</b> spinned the gear too slow and " + pronoun + " fell down too soon before the release point.\nThe correct answer is <color=red>" + aVelocity + "°/s</color>.";
                            }
                            else //if(playerAnswer > Speed)
                            {
                                messageTxt = "<b>" + playerName + "</b> spinned the gear too fast and " + pronoun + " fell down too late after the release point.\nThe correct answer is <color=red>" + aVelocity + "°/s</color>.";
                            }
                        }
                        StartCoroutine(StuntResult());
                        isAnswered = false;
                        CurvedLineFollower.answerIs = isAnswerCorect;
                    }
                    break;
                case 2:
                    if (elapsed < playerAnswer)
                    {
                        qc.timer = elapsed.ToString("f2") + "s";
                        CurvedLineFollower.arc = aVelocity * elapsed;
                        elapsed = GearHangers.hangTime;
                    }
                    else //(elapsed >= gameTime)
                    {
                        isHanging = false;
                        if (playerAnswer == gameTime)
                        {
                            isAnswerCorect = true;
                            CurvedLineFollower.arc = 118;
                            qc.timer = gameTime.ToString("f2") + "s";
                            messageTxt = "<b>" + playerName + "</b> has crossed <color=green>safely</color> at the other platform!";
                        }
                        else
                        {
                            isAnswerCorect = false;
                            CurvedLineFollower.arc = aVelocity * playerAnswer;
                            qc.timer = playerAnswer.ToString("f2") + "s";
                            life.ReduceLife();
                            if (playerAnswer < aVelocity)
                            {
                                messageTxt = "<b>" + playerName + "</b> tried to grab the pipe too soon and " + pronoun + " fell down.\nThe correct answer is <color=red>" + gameTime + "s</color>.";
                            }
                            else //if(playerAnswer > Speed)
                            {
                                messageTxt = "<b><color=red>Stunt Failed!</color></b>\n\n\n" + "<b>" + playerName + "</b> tried to grab the pipe too late and " + pronoun + " fell down.\nThe correct answer is <color=red>" + gameTime + "s</color>.";
                            }
                        }
                        StartCoroutine(GrabPipe());
                        isAnswered = false;
                        CurvedLineFollower.answerIs = isAnswerCorect;
                    }
                    break;
                case 3:
                    if (elapsed < gameTime)
                    {
                        qc.timer = elapsed.ToString("f2") + "s";
                        CurvedLineFollower.arc = aVelocity * elapsed;
                        elapsed = GearHangers.hangTime;
                    }
                    else //(elapsed >= gameTime)
                    {
                        qc.timer = gameTime.ToString("f2") + "s";
                        isHanging = false;
                        if (playerAnswer == angle)
                        {
                            slider.enabled = false;
                            CurvedLineFollower.arc = playerAnswer;
                            isAnswerCorect = true;
                            isEnd = true;
                            messageTxt = "<b>" + playerName + "</b> has <color=green>entered</color> the tunnel!";
                        }
                        else
                        {
                            RagdollSpawn();
                            isAnswerCorect = false;
                            life.ReduceLife();
                            if (playerAnswer < angle)
                            {
                                messageTxt = "<b>" + playerName + "</b> grab the gear too near from the release point and " + pronoun + " overshoot the tunnel entrance.\nThe correct answer is <color=red>" + angle + "°</color>.";
                            }
                            else //if(playerAnswer > angle)
                            {
                                messageTxt = "<b>" + playerName + "</b> grab the gear too near from the release point and " + pronoun + " fell too soon from the tunnel entrance.\nThe correct answer is <color=red>" + angle + "°</color>.";
                            }
                        }
                        StartCoroutine(StuntResult());
                        isAnswered = false;
                        CurvedLineFollower.answerIs = isAnswerCorect;
                    }
                    break;
            }
        }
        else
        {
            if (stuntReady)
            {
                switch (stage)
                {
                    case 1:
                        crank.SetBool("crank", cranked);
                        crank.SetBool("crankReset", crankReset);
                        if (isCranking)
                        {
                            StartCoroutine(Cranking());
                        }
                        if (myPlayer.transform.position.x < -1.25)
                        {
                            isCranking = true;
                        }
                        else
                        {
                            myPlayer.moveSpeed = 0f;
                            if (DC)
                            {
                                DC = false;
                                isStartOfStunt = true;
                                directorIsCalling = true;
                            }
                        }
                        break;
                    case 2:
                        if (myPlayer.transform.position.x < -9.25)
                        {
                            myPlayer.moveSpeed = 1.99f;
                        }
                        else
                        {
                            myPlayer.moveSpeed = 0;
                        }
                        break;
                }
            }
            else
            {
                DC = true;
                crankingDone = false;
            }
        }
        if (qc.isSimulating)
            PlayButton();
        if (qc.retried)
            RetryButton();
        if (qc.nextStage)
            NextButton();
    }
    void Lvl5EasySetUp()
    {
        CurvedLineFollower.answerIs = null;
        stage = qc.stage;
        StartCoroutine(life.startBGgone());
        stage1Layout.SetActive(false);
        stage2Layout.SetActive(false);
        stage3Layout.SetActive(false);
        myPlayer.gameObject.SetActive(true);
        life.losslife = false;
        myPlayer.happy = false;
        DC = true;
        DCisOn = false;
        myPlayer.lost = false;
        myPlayer.standup = false;
        playerAnswer = 0;
        elapsed = 0;
        angle = 0;
        aVelocity = 0;
        gameTime = 0;
        gear2Speed = 0;
        gearRB.angularVelocity = 0;
        gearRB.rotation = 0f;
        CurvedLineFollower.arc = 0;
        if (playerGender == "Male")
        {
            pronoun = "he";
            pNoun = "his";
        }
        else
        {
            pronoun = "she";
            pNoun = "her";
        }
        switch (stage)
        {
            case 1:
                qc.SetUnitTo(UnitOf.angularVelocity);
                qc = UI1.gameObject.GetComponent<QuestionController2_0_1>();
                qc.stage = 1;
                qc.timer = "0.00s";
                qc.Unit(UnitOf.angularVelocity);
                playerHangerTrigger1.SetActive(false);
                stage1Layout.SetActive(true);
                float t = Random.Range(3.1f, 3.7f);
                gameTime = (float)System.Math.Round(t, 2);
                aVelocity = (float)System.Math.Round((210 / gameTime), 2);
                question = "<b>" + playerName + "</b> is trying to go accross the other platform by hanging at the tooth or the rotating gear from the starting platform and letting it go after <color=#006400>" + gameTime.ToString() + " seconds</color>. If the safe release point of the tooth is <color=red>210 degrees</color> from the grab point. At what <color=purple>angular velocity</color> should " + "<b>" + playerName + "</b> set the spinning gear at?";
                CurvedLineFollower.stage = 1;
                myPlayer.transform.position = playerPos;
                gearSet.transform.position = new Vector3(gearSet.transform.position.x, gearSet.transform.position.y, gearSet.transform.position.z);
                crankReset = true;
                qc.limit = 116;
                break;
            case 2:
                qc.SetUnitTo(UnitOf.time);
                qc = UI2.gameObject.GetComponent<QuestionController2_0_1>();
                qc.stage = 2;
                qc.Unit(UnitOf.time);
                qc.timer = "0.00s";
                playerHangerTrigger2.SetActive(false);
                player.gravityScale = 1;
                pipeHanger.enabled = false;
                directorPlatform.transform.position = new Vector3(-17, 2, 0);
                directorPlatform.transform.localScale = new Vector3(-1.20f, 01.20f, 0);
                directorsSpeech.transform.localScale = new Vector3(-1, directorsSpeech.transform.localScale.y, directorsSpeech.transform.localScale.z);
                stage2Layout.SetActive(true);
                float av = Random.Range(30f, 40f);
                aVelocity = (float)System.Math.Round(av, 2);
                gameTime = (float)System.Math.Round((118 / aVelocity), 2);
                question = "<b>" + playerName + "</b> is trying to cross the other platform by hanging at the rotating gear from the starting platform and grabbing the pipe above upon reaching the highest point of the gear. If the release point of the gear is <color=red>118 degrees</color> from the grab point, and the angular velocity of the gear is <color=purple>" + aVelocity + " degrees per second</color>, how <color=#006400>long</color> " + pronoun + " should hold on to the gear before reaching for the pipe?";
                CurvedLineFollower.stage = 2;
                myPlayer.transform.position = new Vector2(playerPos.x - 12, playerPos.y - 1);
                gearSet.transform.position = new Vector3(-6.15f, -0.667f, gearSet.transform.position.z);
                gear2Speed = aVelocity * 1.612f;
                gearRB.angularVelocity = -aVelocity;
                safeZone.position = new Vector3(4.5f, 5.5f, 0);
                stuntReady = true;
                qc.limit = 8;
                break;
            case 3:
                qc.SetUnitTo(UnitOf.angle);
                qc = UI3.gameObject.GetComponent<QuestionController2_0_1>();
                qc.stage = 3;
                qc.Unit(UnitOf.angle);
                slider.enabled = true;
                gearRB = gear3.GetComponent<Rigidbody2D>();
                qc.timer = "0.00s";
                playerHangerTrigger3.SetActive(false);
                gearSet.SetActive(false);
                stage3Layout.SetActive(true);
                gear3.SetActive(true);
                directorPlatform.transform.position = new Vector3(-8.5f, 6.7f, 90);
                directorPlatform.transform.localScale = new Vector3(-1f, 01f, 0);
                directorsSpeech.transform.localScale = new Vector3(-1, directorsSpeech.transform.localScale.y, directorsSpeech.transform.localScale.z);
                while (angle < 50f || angle > 75f)
                {
                    float a = Random.Range(20f, 30f);
                    aVelocity = (float)System.Math.Round(a, 2);
                    float t3 = Random.Range(2.5f, 3.5f);
                    gameTime = (float)System.Math.Round(t3, 2);
                    angle = (float)System.Math.Round((aVelocity * gameTime), 2);
                }
                question = "<b>" + playerName + "</b> needs to enter the tunnel at the other side and the only way to do that is to hang into the rotating gear and let go upon reaching the lowest part of the gear and land at the very edge of the tunnel floor. If the gear rotates counterclockwise at <color=purple>" + aVelocity + " degrees per second</color> and " + pronoun + " will only hold on into the gear at exactly <color=#006400>" + gameTime + " seconds</color> before letting go, at what <color=red>angle</color> from the release point should " + "<b>" + playerName + "</b> grab the gear?";
                CurvedLineFollower.stage = 3;
                myPlayer.transform.position = new Vector2(0, 3);
                gearRB.angularVelocity = aVelocity;
                safeZone.position = new Vector3(7.5f, -5.5f, 0);
                qc.limit = 360;
                break;
        }
        qc.SetQuestion(question);
    }
    void PlayButton()
    {
        qc.isSimulating = false;
        playerAnswer = qc.GetPlayerAnswer();
        if (stage == 1)
            stuntReady = true;
        else
        {
            isStartOfStunt = true;
            directorIsCalling = true;
        }
    }
    void RetryButton()
    {
        Destroy(ragdoll);
        qc.retried = false;
        myPlayer.gameObject.SetActive(true);
        Lvl5EasySetUp();
    }
    void NextButton()
    {
        qc.nextStage = false;
        // myPlayer.SetEmotion("");
        StartCoroutine(ExitStage());
    }
    public IEnumerator DirectorsCall()
    {
        DCisOn = true;
        stuntReady = false;
        directorIsCalling = false;
        if (isStartOfStunt)
        {
            directorsBubble.SetActive(true);
            directorsSpeech.text = "Lights!";
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "Camera!";
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "Action!";
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "";
            directorsBubble.SetActive(false);
            if (stage == 1)
                playerHangerTrigger1.SetActive(true);
            else if (stage == 2)
                playerHangerTrigger2.SetActive(true);
            else
            {
                myPlayer.jumpHang = true;
                yield return new WaitForSecondsRealtime(1.23f);
                playerHangerTrigger3.SetActive(true);
                myPlayer.jumpHang = false;
            }
            isAnswered = true;
        }
        else
        {
            if (stage == 1)
            {
                yield return new WaitForSeconds(0.3f);
                myPlayer.isLanded = false;
                myPlayer.brake = true;
                yield return new WaitForSeconds(0.7f);
                myPlayer.brake = false;
                myPlayer.gameObject.transform.position = new Vector2(myPlayer.gameObject.transform.position.x + 0.4f, myPlayer.gameObject.transform.position.y);
                myPlayer.gameObject.transform.localScale = new Vector2(1f, 1f);
            }
            else if (stage == 2)
            {
                yield return new WaitForSeconds(.5f);
                myPlayer.isLanded = false;
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return new WaitForSeconds(.5f);
                myPlayer.isLanded = false;
                yield return new WaitForSeconds(0.5f);
            }
            directorsBubble.SetActive(true);
            directorsSpeech.text = "Cut!";
            yield return new WaitForSeconds(1f);
            directorsBubble.SetActive(false);
            directorsSpeech.text = "";
            myPlayer.happy = true;
        }
    }
    IEnumerator ExitStage()
    {
        myPlayer.happy = false;
        yield return new WaitForSeconds(1f);
        myPlayer.moveSpeed = 5;
        yield return new WaitForSeconds(2f);
        // StartCoroutine(life.endBGgone());
        yield return new WaitForSeconds(2.8f);
        myPlayer.transform.position = new Vector2(0f, myPlayer.transform.position.y);
        myPlayer.moveSpeed = 0;
        playerAnswer = 0;
        Lvl5EasySetUp();
    }
    IEnumerator StuntResult()
    {
        if (stage == 1)
        {
            yield return new WaitForSeconds(0.7f);
            myPlayer.isLanded = true;
            yield return new WaitForSeconds(0.3f);
        }
        else if (stage == 2)
        {
            yield return new WaitForSeconds(0.15f);
            myPlayer.isLanded = true;
            yield return new WaitForSeconds(0.85f);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            myPlayer.isLanded = true;
        }
        directorIsCalling = true;
        isStartOfStunt = false;
        yield return new WaitForSeconds(3f);
        qc.ActivateResult(messageTxt, isAnswerCorect, isEnd);
    }
    IEnumerator Cranking()
    {
        isCranking = false;
        crankReset = false;
        if (!crankingDone)
        {
            cranked = true;
            yield return new WaitForSeconds(1.17f);
            gear2Speed = playerAnswer * 1.612f;
            gearRB.angularVelocity = -playerAnswer;
            cranked = false;
            yield return new WaitForEndOfFrame();
            myPlayer.moveSpeed = 1.99f;
            crankingDone = true;
        }
    }
    public void RagdollSpawn()
    {
        myPlayer.gameObject.SetActive(false);
        ragdoll = Instantiate(ragdollPrefab);
        ragdoll.transform.position = myPlayer.gameObject.transform.position;
        StartCoroutine(StuntResult());
    }
    IEnumerator HangWalk()
    {
        pipeHanger.enabled = true;
        player.gravityScale = 1;
        myPlayer.isGrabbing = false;
        myPlayer.hangWalk = true;
        for (int i = 0; i <= 250; i++)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            pipeHanger.anchor = new Vector2(pipeHanger.anchor.x + 0.0027f, pipeHanger.anchor.y);
        }
        myPlayer.hangWalk = false;
        pipeHanger.enabled = false;

        StartCoroutine(StuntResult());
    }
    IEnumerator GrabPipe()
    {
        myPlayer.isGrabbing = true;
        player.gravityScale = 0;
        if (isAnswerCorect)
        {
            yield return new WaitForSeconds(0.50f);
            StartCoroutine(HangWalk());
        }
        else
        {
            yield return new WaitForSeconds(0.29f);
            myPlayer.isFalling = true;
            player.gravityScale = 1;
            myPlayer.isGrabbing = false;
            myPlayer.hangWalk = false;
            yield return new WaitForSeconds(1.25f);
            myPlayer.isFalling = false;
            RagdollSpawn();
        }
    }
}