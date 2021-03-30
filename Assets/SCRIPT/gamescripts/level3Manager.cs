using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;



public class level3Manager : MonoBehaviour
{
    public GameManager thegamemnager;
    //public cameracontroller thecam;
    public cameraman thedirector;
    public camerascript thecamera;
    public actionCount theAct;
    public ScoreManager thescorer;
    private stopperController theStopper;
    private parallax theparax;
    public boardanim theboard;
    private camerashake thecamshake;
    public flag theflag;
    public Player thePlayer;
    public Player thePlayer2;
    private Vector3 PlayerStartPoint;
    float playerpos;
    float currentpos;


    public GameObject answerinput;
    public GameObject next;
    public GameObject next2;
    public GameObject start;
    public GameObject play;
    public GameObject retry;
    public GameObject question2;
    public GameObject question1;
    public GameObject check;
    public GameObject wrong;
    public GameObject level2;
    public GameObject nextlevelbutton;

    public Text giventime;
    float generatespeed;
     float generatespeedRO;
    public Text givendistance;
    float generatedistance;

    public Text stage;

    public InputField speed;
    float timeINput;

    public Text playerdistance;
    public Text timertxt;
    public Text msectxt;
    public Text sectxt;
    float distanceTimer;
    public float msec;
    private float sec;
    private float min;
    float Asec;
    float Amsec;
    float FAmsec;
    float pInput;
    float pinputspeed;
    float correctAnswer;
    float correctanswerRO;
    float pinputdistance;
     public bool levelstart;

    public bool tileDrawn;
    public tilemapgenerator thetilegenerator;
    public bool destroytiles;
    public GameObject distancemarking;
    private Vector3 distancemarkstartpoint;
    int selectedchar;
   

    public AudioSource button1;
    public AudioSource button2;
    public AudioSource button3;
    public AudioSource wrongsfx;
    public bool buzz;
    public AudioSource bustedsfx;
    public bool busted;
    public AudioSource correctsfx;
    public bool ding; 
    bool ragdollready = true;

    public bool destroystick;

    public GameObject stuntfailed;
    bool failprompt;
    public Text[] playername;
    public Text correctanswertxt;
    string fastslowtxt;
    string pronoun;
    int gender;
   
    
    public GameObject jumpers;
    public GameObject BGstates;
    public GameObject Stuntground;
     Vector2 stuntgroundstartpoint;
    public GameObject ceilingstopper;
    public GameObject safewall;

    public Text timequestion;
    public Text stuntsfailtxt;

    public GameObject meter;
    public GameObject meterimage;
    public Text metertxt;

    public GameObject transition;

    




    // Start is called before the first frame update
    void Start()
    {
        thegamemnager = FindObjectOfType<GameManager>();
        thetilegenerator = FindObjectOfType<tilemapgenerator>();
        thePlayer = FindObjectOfType<Player>();
        PlayerStartPoint = thePlayer.transform.position;
        playerpos = thePlayer.transform.position.x;
        distancemarkstartpoint = distancemarking.transform.position;
        //thecam = FindObjectOfType<cameracontroller>();
        thedirector = FindObjectOfType<cameraman>();
        theboard = FindObjectOfType<boardanim>();
        theflag = FindObjectOfType<flag>();
        thecamera = FindObjectOfType<camerascript>();
        theAct = FindObjectOfType<actionCount>();
        thescorer = FindObjectOfType<ScoreManager>();
        stuntgroundstartpoint = Stuntground.transform.position;
        theStopper = FindObjectOfType<stopperController>();
        theparax = FindObjectOfType<parallax>();
        thePlayer.positioning();
        thecamshake = FindObjectOfType<camerashake>();
        playername[0].text = PlayerPrefs.GetString("name") + (".");
        playername[2].text = PlayerPrefs.GetString("name");
        playername[1].text = PlayerPrefs.GetString("name") + (" is");
         if (gender == 0)
        {
            pronoun = ("he");
            
        }
        if (gender == 1)
        {
            pronoun = ("she");
           
        } 
          

         if (selectedchar == 1)
        {
            thePlayer = thePlayer2;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
         if (levelstart == true)
        {
             if (timeINput <= 0)
        {
            theStopper.moveSpeed = 6;
        }
              if (pInput == 0)
            {
                StopCoroutine("StopWatch");
                msectxt.text = ":" + Amsec;
                sectxt.text = ":0" + Asec;
                thePlayer.moveSpeed = 0;
                thePlayer.lost = true;
                retry.gameObject.SetActive(true);
                wrong.SetActive(true);
                theAct.losepoint();
                buzzzing();
                ceilingstopper.SetActive(false);
            }
             FAmsec = timeINput % 1;
            Amsec = Mathf.Round(FAmsec * 100f) / 1.0f;
            Asec = timeINput - FAmsec;
            currentpos = thePlayer.transform.position.x;
            if (sec <= Asec)
            {
               
                sectxt.text = string.Format(":{0:00}",sec);
                if (thePlayer.moveSpeed > 0)
                {
                    msectxt.text = string.Format(":{0:00}", msec);
                }
               
            }
            /*if (currentpos >= generatedistance)
            {
                thePlayer.moveSpeed = 0f;
                StopCoroutine("StopWatch");
            }*/
            if (pInput == correctanswerRO)
            {
                 if (currentpos >= generatedistance)
                 {
                    thePlayer.moveSpeed = 0;
                    if (thePlayer.moveSpeed == 0)
                    {
                        theparax.moveSpeed = 0;
                        StopCoroutine("StopWatch");
                        //thePlayer.happy = true;
                        thePlayer.transform.position = new Vector2(generatedistance, -1);
                        msectxt.text = ":" + Amsec;
                        sectxt.text = ":0" + Asec;
                        check.SetActive(true);
                        //nextlevel.SetActive(true);
                        //theflag.flagAnim = 1;
                        jumpers.SetActive(true);
                        //thescorer.finalstar();
                        next2.SetActive(true);
                        PlayerPrefs.SetInt ("level", 1);
                        dingling();
                        thescorer.addingoOneCoin();
                        ceilingstopper.SetActive(false);
                        safewall.SetActive(true);
                    }
                   

                 }

            }
            if (pInput != correctanswerRO)
            {
                  
                /*if (currentpos > pinputdistance)
                {
                    thePlayer.transform.position = new Vector2(pinputdistance, -1);

                }*/
                if(pInput < correctanswerRO)
                {
                    if (currentpos >= pinputdistance)
                    {
                          thePlayer.moveSpeed = 0;
                           if (thePlayer.moveSpeed == 0)
                        {
                            theparax.moveSpeed = 0;
                            theAct.losepoint();
                            StopCoroutine("StopWatch");
                            thePlayer.lost = true;
                            thePlayer.standup = true;
                            thePlayer.transform.position = new Vector2(pinputdistance, -1);
                            msectxt.text = ":" + Amsec;
                            sectxt.text = ":0" + Asec;
                            wrong.SetActive(true);
                            buzzzing();
                                fastslowtxt = ("slow!");
                           ceilingstopper.SetActive(false);
                                 if(failprompt == false)
                            {
                                StartCoroutine(stuntfailing());
                            }
                        }
                    }
                }
                if (currentpos >= generatedistance)
                {
                    if (pinputdistance < generatedistance + 1)
                    {
                        if (currentpos >= pinputdistance)
                        {
                             thePlayer.moveSpeed = 0f;
                            StopCoroutine("StopWatch");
                       
                        
                                if (thePlayer.moveSpeed == 0)
                            {
                                theparax.moveSpeed = 0;
                                ceilingstopper.SetActive(false);
                                thePlayer.transform.position = new Vector2(pinputdistance, -1);
                                msectxt.text = ":" + Amsec;
                                sectxt.text = ":0" + Asec;
                                thePlayer.lost = true;
                                thePlayer.standup = true;
                                play.gameObject.SetActive(false);
                                fastslowtxt = ("fast!");
                                //wrong.SetActive(true);
                                theAct.losepoint();
                                buzzzing();
                                if(failprompt == false)
                                {
                                    StartCoroutine(stuntfailing());
                                }
                            }
                        }
                        
                    }
                     if (pinputdistance > generatedistance + 1)
                    {
                        if (currentpos > generatedistance + 1)
                        {
                            thePlayer.moveSpeed = 0f;
                            StopCoroutine("StopWatch");
                            if (ragdollready)
                            {
                                thegamemnager.playerDead = true;
                                wasted();
                                StartCoroutine(playerspawn());
                                thecamera.stuntScene = true;
                                play.gameObject.SetActive(false);
                                ceilingstopper.SetActive(false);
                                fastslowtxt = ("fast!");
                                //wrong.SetActive(true);
                                theAct.losepoint();
                                if(failprompt == false)
                           
                                {
                                    thePlayer.standup = true;
                                    StartCoroutine(stuntfailing());
                                }
                           
                            }
                        }
                    }
                        
                        
                 }
                 
            }
                 
             
              playerdistance.text = currentpos.ToString("F2") + "m";

        }
            givendistance.text = generatedistance.ToString("F1");
            giventime.text = generatespeedRO.ToString("F2");
        
         
    }
    public void nextlevel()
    {
       StartCoroutine(continuetonext());
       thegamemnager.simulate = false;
       thePlayer.moveSpeed = 3;
       button3.Play();
       next.SetActive(false);
       question1.SetActive(false);
        check.SetActive(false);
        theStopper.stopperReset();
        theparax.moveSpeed = -0.2f;
        

    }
         
    
    IEnumerator startgame()
    {
        transition.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        button1.Play();
        question2.SetActive(true);
        generatespeed = (Random.Range(1.5f, 2.5f));
        generatedistance = (Random.Range(7, 9));
        generatespeedRO = (float)System.Math.Round(generatespeed, 1);
        distancemarking.transform.position = new Vector2(generatedistance, -1);
        correctAnswer = generatedistance / generatespeedRO;
        correctanswerRO = (float)System.Math.Round(correctAnswer, 2);
        //thecam.actioncam = 1;
        thetilegenerator.mapwitdh = (int)generatedistance + 1;
        play.gameObject.SetActive(true);
        theboard.transit = 1;
        tileDrawn = true;
        Stuntground.transform.position = new Vector3(generatedistance + 3, -3, 2);
        timequestion.text = ("The ceiling is still crumbling and the next safe spot is <b>")+ generatedistance + ("</b> meter away from  <b>") + PlayerPrefs.GetString("name") + ("</b>. If <b>") + PlayerPrefs.GetString("name") + ("</b> will now run at exactly <b>")+ generatespeedRO.ToString("F1")+ ("</b> m/s, how long should ")+ pronoun + (" run so that ")+ pronoun + (" will not get hit by the crumbling debris of the ceiling?");
        meter.transform.localScale = new Vector2(generatedistance, 0.05f);
        metertxt.text = generatedistance.ToString() + ("m");
        meterimage.transform.position = new Vector2(generatedistance/2, -1.67f);
         if (tileDrawn == true)
            {
                thetilegenerator.createQuadtilemap();
                tileDrawn = false;
            }

        

       

        start.SetActive(false);
       
    }
    IEnumerator continuetonext()
    {  
        transition.SetActive(false);
        yield return new WaitForSeconds(4);
        theAct.endbgexit();
        yield return new WaitForSeconds(1);
        thePlayer.transform.position = PlayerStartPoint;
        thePlayer.happy = false;
        BGstates.transform.position = new Vector2(1.5f, 2.8f);
        theparax.moveSpeed = 0;
       
        jumpers.SetActive(false);
        answerinput.SetActive(true);
       
        resetstopwatch();
        //level1manager.SetActive(false);
        StartCoroutine(resettiles());
        ceilingstopper.SetActive(true);
        meter.transform.localScale = new Vector2(0, 0.05f);
        metertxt.text = ("0m");
        meterimage.transform.position = new Vector2(0, -1.67f);
        
       
        distancemarking.transform.position = thegamemnager.distancemarkstartpoint;
        Stuntground.transform.position = stuntgroundstartpoint;
        theboard.transit = 0; 
        //theflag.flagAnim = 0;
        thedirector.action = false;
         buzz = true;
         busted = true;
         ding = true;
         
         thescorer.coinAvail = true;
         stage.text = ("time");
         thePlayer.posready = true;
         safewall.SetActive(false);
        StartCoroutine(startgame());
         //yield return new WaitForSeconds(2);
          //start.SetActive(true);
          
    }
    public void playgame()
    {
        
        timeINput = float.Parse(speed.text);
        thePlayer.moveSpeed = generatespeedRO;
        theparax.moveSpeed = -0.2f;
        pInput = timeINput;
        tileDrawn = true;
        pinputdistance = pInput * generatespeedRO;
        play.gameObject.SetActive(false);
        thecamera.camActive = true;
        levelstart = true;
        thecamshake.debrisRange = generatedistance;
        
        
        //theStopper.moveSpeed = generatespeedRO - 0.3f;
        //thecam.Action();
        //thecam.playercam = true;
       
        
    }
     public void reset()
     {
         buzz = true;
         ding = true;
        button2.Play();
        thePlayer.gameObject.SetActive(true);
        thePlayer.transform.position = PlayerStartPoint;
        thePlayer.moveSpeed = 0;
        retry.gameObject.SetActive(false);
        resetstopwatch();
        levelstart = false;
        thePlayer.happy = false;
        thePlayer.lost = false;
        wrong.SetActive(false);
        start.gameObject.SetActive(true);
        theboard.transit = 2; 
        failprompt = false;
        question2.SetActive(false);
        thedirector.action = false;
        thecamera.camActive = false;
        thecamera.stuntScene = false;
        theAct.losepointReady = false;
        ceilingstopper.SetActive(true);
         stuntfailed.SetActive(false);
         thePlayer.standup = false;
         thePlayer.positioning();
         StartCoroutine(resettiles());
         StartCoroutine(resetstick());
         Stuntground.transform.position = stuntgroundstartpoint;
         theStopper.stopperReset();
         ceilingstopper.SetActive(true);
         ragdollready = true;
        meter.transform.localScale = new Vector2(0, 0.05f);
        metertxt.text = ("0m");
        meterimage.transform.position = new Vector2(0, -1.67f);
        transition.SetActive(true);
        StartCoroutine(startgame());
     }
     public void proceedtonextlevel()
     {
        PlayerPrefs.SetInt ("level", 1);
        SceneManager.LoadScene("GameLevel");
        theflag.flagAnim = 0;
     }
      public void resetstopwatch()
    {
        distanceTimer = 0;
        //timertxt.text = "00:00:00";
    }
    public void FixedtimeStart()
    {
        StartCoroutine("StopWatch");
    }
    IEnumerator StopWatch()
    {
        while (true)
        {
            distanceTimer += Time.deltaTime;
            msec = (int)((distanceTimer - (int)distanceTimer) * 100);
            sec = (int)(distanceTimer % 60);
            min = (int)(distanceTimer / 60 % 60);
            timertxt.text = string.Format("{0:00}", min);



            yield return null;
        }
    }
    IEnumerator stuntfailing()
    {   
        failprompt = true;
        yield return new WaitForSeconds(2);
        buzzzing();
        stuntfailed.SetActive(true);      
        //correctanswertxt.text = correctanswerRO.ToString();
        stuntsfailtxt.text = PlayerPrefs.GetString("name") + (" is too") + fastslowtxt + (", the correct answer is ")+ correctanswerRO.ToString();
        yield return new WaitForSeconds(2);
        retry.gameObject.SetActive(true); 
    }
            
     IEnumerator resettiles()
    {
       
        destroytiles = true;
        yield return new WaitForSeconds(0);
        destroytiles = false;
    }
    IEnumerator playerspawn()
    {
        thePlayer.ragdollspawn();
        ragdollready = false;
        yield return new WaitForSeconds(3);
    }
     IEnumerator resetstick()
    {
       
        destroystick = true;
        yield return new WaitForSeconds(1);
        destroystick = false;
    }
    IEnumerator cameraAction()
    {
        yield return new WaitForSeconds(3);
        playgame();
        FixedtimeStart();
    }
    public void Action()
    {
        thedirector.playAction();
        StartCoroutine("cameraAction");
        play.gameObject.SetActive(false);
        
    }
     void buzzzing()
    {
        if(buzz)
        {
            wrongsfx.Play(0);
            thecamshake.collapse();
            buzz = false;       
        }
        
    }
     void wasted()
    {
        if(busted)
        {
            
            StartCoroutine(cutvoice());
            busted = false;       
        }
        
    }
    IEnumerator cutvoice()
    {
        yield return new WaitForSeconds(2);
        bustedsfx.Play(0);
    }
     void dingling()
    {
        if(ding)
        {
            thePlayer.happy = true;
            correctsfx.Play();
            StartCoroutine(cutvoice());
            thecamshake.collapse();
            ding = false;
        }
    }
   
}
