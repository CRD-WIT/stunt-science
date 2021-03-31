using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class level2Manager : MonoBehaviour
{
    public GameManager thegamemnager;
    private level3Manager thelevel3;
    public monster themonster;
    public actionCount theAct;
    private parallax theparax;
    public bool monsterready = true;
    private Vector3 monsterstartpoint;
    //public cameracontroller thecam;
    public cameraman thedirector;
    public camerascript thecamera;
    public boardanim theboard;
    public flag theflag;
    public ScoreManager thescorer;
    private stopperController theStopper;
    private camerashake thecamshake;

    float playerpos;
    float currentpos;
     public Player thePlayer;
    public Player thePlayer2;
    private Vector3 PlayerStartPoint;

    public GameObject level1manager;
    public GameObject question;
    public GameObject level1;
    public GameObject answerinput;
    public GameObject next;
    public GameObject next2;
    public GameObject start;
    public GameObject play;
    public GameObject retry;
    public GameObject question2;
    public GameObject check;
    public GameObject wrong;
    public GameObject monster;
    public GameObject BGstates;

    float generatespeed;
    float generatespeedRO;
    public float generatetimeRO;
    float pinputtime;
    float generatetime;
    public Text giventime;
    public Text givenspeed;

    public Text stage;

    public InputField distance;
    float distanceInput;

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
    float correctAnswer;
    float correctanswerRO;
    public bool levelstart;

    public bool tileDrawn;
    public tilemapgenerator thetilegenerator;
    public bool destroytiles;
     public GameObject distancemarking;
   public Vector3 distancemarkstartpoint;
    public bool backwardstunt;
    public GameObject Stuntground;
    Vector2 stuntgroundstartpoint;
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
    
    public bool destroystick;
    public GameObject jumpers;
    bool reposition;
    float playerrepos;
    bool repos;
    float Corectrepos;
    float PIdistance;
    public GameObject ceilingstopper;
    public GameObject safewall;
    public GameObject manhole;
    public GameObject manholecover;

    public GameObject stuntfailed;
    bool failprompt;
    public Text correctanswertxt;
    string fastslowtxt;
    public Text[] playername;
    string pronoun;
    int gender;
     
    public Text distancequestion;
    public Text stuntsfailtxt;

    public GameObject meter;
    public GameObject meterwall;
    public GameObject meterimage;
    public Text metertxt;

    public GameObject transition; 
    
    

    


    

    // Start is called before the first frame update
    void Start()
    {
        thetilegenerator = FindObjectOfType<tilemapgenerator>();
        thePlayer = FindObjectOfType<Player>();
        PlayerStartPoint = new Vector2 (-3, -1);
        playerpos = thePlayer.transform.position.x;
        thegamemnager = FindObjectOfType<GameManager>();
        thelevel3 = FindObjectOfType<level3Manager>();
        thedirector = FindObjectOfType<cameraman>();
        distancemarkstartpoint = distancemarking.transform.position;
        //themonster = FindObjectOfType<monster>();
        //monsterstartpoint = monster.transform.position;
        stuntgroundstartpoint = Stuntground.transform.position;
        //thecam = FindObjectOfType<cameracontroller>();
        theboard = FindObjectOfType<boardanim>();
        theflag = FindObjectOfType<flag>();
        thecamera = FindObjectOfType<camerascript>();
        theAct = FindObjectOfType<actionCount>();
        thescorer = FindObjectOfType<ScoreManager>();
        theStopper = FindObjectOfType<stopperController>();
        theparax = FindObjectOfType<parallax>();
        thePlayer.positioning();
        thecamshake = FindObjectOfType<camerashake>();

        playername[0].text = PlayerPrefs.GetString("name") + (".");
        playername[2].text = PlayerPrefs.GetString("name");
        playername[1].text = PlayerPrefs.GetString("name") + (" is");

        
       
        if (selectedchar == 1)
        {
            thePlayer = thePlayer2;
        }
          if (gender == 0)
        {
            pronoun= ("he");
            
        }
        if (gender == 1)
        {
            pronoun = ("she");
            
        } 
        
        


    }

    // Update is called once per frame
    void Update()
    {
        if(repos == true)
        {
            thePlayer.moveSpeed = 4;
            if(currentpos >= playerrepos)
            {
                thePlayer.moveSpeed = 0;
                thePlayer.transform.position = new Vector2(playerrepos, -1);
                repos = false;
                meterwall.transform.position = new Vector2(10 - pInput, -1.9f);
                metertxt.text = distance.text + ("m");
                meter.transform.localScale = new Vector2(pInput, 0.05f);
                meterimage.transform.position = new Vector2((10 - pInput/2), -1.67f);
                StartCoroutine("cameraAction");
            }
        }
      
        playerrepos = 10 - pInput;
        Corectrepos = 10 - correctAnswer;
        PIdistance = playerrepos - Corectrepos + 10;
        currentpos = thePlayer.transform.position.x;
        
        givenspeed.text = generatespeedRO.ToString("F1");
        giventime.text = generatetimeRO.ToString("F2");
        /*if (reposition == true)
        {
            if(currentpos >= playerrepos)
            {
                thePlayer.moveSpeed = 0;
                thePlayer.transform.position = new Vector2(playerrepos, -1);
                reposition = false;

            }
        }*/
       
        if (levelstart == true)
        {
         
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
                theparax.moveSpeed = 0;
                ceilingstopper.SetActive(false);  
            }
            pinputtime = pInput / generatespeedRO;
            FAmsec = pinputtime % 1;
            Amsec = Mathf.Round(FAmsec * 100f) / 1.0f;
            Asec = pinputtime - FAmsec;
            
            
            if (sec <= Asec)
            {
               
                sectxt.text = string.Format(":{0:00}",sec);
                if (thePlayer.moveSpeed > 0)
                {
                    msectxt.text = string.Format(":{0:00}", msec);
                }
               
            }
            if (sec >= Asec & msec >= Amsec)
            {
               
                
            }
            if (pInput == correctanswerRO)
            {
                manholecover.SetActive(false);
                if (currentpos >= 10)
                {                   
                    thePlayer.moveSpeed = 0;
                    theparax.moveSpeed = 0;
                    if(thePlayer.moveSpeed == 0)
                    {
                       StartCoroutine(runsafe());
                       ceilingstopper.SetActive(false);   
                    }
                    
                }
            }
            if (pInput != correctanswerRO)
            {
                if (pInput > 0)
                {
                    if (currentpos >= PIdistance)
                    {
                        thePlayer.moveSpeed = 0;
                        if (thePlayer.moveSpeed == 0)
                        {
                            ceilingstopper.SetActive(false);  
                             if(failprompt == false)
                            {
                                StartCoroutine(stuntfailing());
                            }
                            //manholecover.SetActive(false);
                            theparax.moveSpeed = 0;
                            thePlayer.transform.position = new Vector2(PIdistance, -1);
                            theAct.losepoint();
                            thePlayer.lost = true;
                            thePlayer.standup = true;
                            
                            wrong.SetActive(true);
                               
                            StopCoroutine("StopWatch");
                            msectxt.text = ":" + Amsec;
                            sectxt.text = ":0" + Asec;
                            buzzzing();
                        }
                    }
                }
                if (pInput > correctanswerRO)
                {
                    fastslowtxt = ("far!");
                }
                 if (pInput < correctanswerRO)
                {
                    fastslowtxt = ("near!");
                }
                /*if (pInput > 0)
                {
                    if (currentpos >= pInput)
                    {
                        if (pInput < correctanswerRO)
                        {
                            
                            thePlayer.moveSpeed = 0;
                           
                            if (thePlayer.moveSpeed == 0)
                            {
                                thePlayer.transform.position = new Vector3(pInput, -1.0f, 0);
                                theAct.losepoint();
                                thePlayer.lost = true;
                                retry.gameObject.SetActive(true);
                                wrong.SetActive(true);
                               
                                StopCoroutine("StopWatch");
                                msectxt.text = ":" + Amsec;
                                sectxt.text = ":0" + Asec;
                                buzzzing();
                                
                            }
                        }
                       



                    }
                     if (pInput > correctanswerRO)
                     {
                    
                         if (currentpos >= correctanswerRO)
                        {
                             if (monsterready)
                            {
                               StartCoroutine(monsterAction());
                            }
                            
                                                    
                            retry.gameObject.SetActive(true);
                            wrong.SetActive(true);
                             theAct.losepoint();
                             buzzzing();
                          
                           
                           
                             
                           if (thePlayer.ragdollblow == true)
                            {
                                thePlayer.ragdollspawn();
                                wasted();
                                
                            }
                            if (currentpos >= pInput)
                            {
                                thePlayer.moveSpeed = 0;
                                 thePlayer.transform.position = new Vector2(pInput, -1.0f);
                                 
                                 
                                
                            }
                            
                            
                        }
                    }
                }*/
            }
            playerdistance.text = currentpos.ToString("F2") + "m";
           
            
            


        }



    }
    public void nextlevel()
    {
       StartCoroutine(continuetonext());
       theparax.moveSpeed = -0.2f;
       thelevel3.levelstart = false;
        thePlayer.moveSpeed = 3;
       button3.Play();
       next2.SetActive(false);
       question.SetActive(false);
        check.SetActive(false);
        thePlayer.posready = false;
        ceilingstopper.SetActive(true);
        theStopper.stopperReset();
        distancemarking.transform.position = distancemarkstartpoint;
        
        

        

    }
    IEnumerator startgame()
    {
        yield return new WaitForSeconds(0.2f);
        button1.Play();
        thetilegenerator.mapwitdh = 20;
        start.SetActive(false);
        play.SetActive(true);
        question2.SetActive(true);
        generatespeed = (Random.Range(3.0f, 4.0f));       
        generatetime = (Random.Range(1.5f, 2.4f));
        generatetimeRO = (float)System.Math.Round(generatetime, 1);
        generatespeedRO = (float)System.Math.Round(generatespeed, 1);
       
        meter.transform.localScale = new Vector2(10, 0.05f);
        metertxt.text = ("10m");
        meterimage.transform.position = new Vector2(5, -1.67f);
       
        correctAnswer = generatespeedRO * generatetimeRO;
        correctanswerRO = (float)System.Math.Round(correctAnswer, 2);
       
        start.gameObject.SetActive(false);
        distancequestion.text = ("The entire ceiling is now crumbling and the only safe way is to go down the manhole few meters away from <b>") + PlayerPrefs.GetString("name") + ("</b>. If <b>") + PlayerPrefs.GetString("name") + ("</b> now runs at <b>") + generatespeedRO.ToString("F1")+ ("</b> m/s for <b>") + generatetimeRO.ToString("F1") + ("</b> seconds, how far from the manhole ") + pronoun + (" should start runnning?");
       
        
        //thecam.actioncam = 1;
        //distancemarking.transform.position = new Vector2(correctanswerRO, -1);
        tileDrawn = true;
        theboard.transit = 1;
         if (tileDrawn == true)
            {
                thetilegenerator.createQuadtilemap2();
                tileDrawn = false;
            }
    }
    public void playgame()
    {
        
       
       thePlayer.moveSpeed = generatespeedRO;
       theStopper.moveSpeed = generatespeedRO - 0.3f;
       theparax.moveSpeed = -0.2f;
       thecamshake.debrisRange = 15;
        
       
        play.gameObject.SetActive(false);
        levelstart = true;
        backwardstunt = true;
       
        /*if (pInput > correctanswerRO)
        {
          monster.transform.position = new Vector2(correctanswerRO + 2, transform.position.y + 10);
        }*/
        //thecam.Action();
        //thecam.playercam = true;
        thecamera.camActive = true;
        
    }
   
    public void reset()
    {
        button2.Play();
        buzz = true;
        busted = true;
        ding = true; 
        thePlayer.gameObject.SetActive(true);
        thePlayer.transform.position = PlayerStartPoint;
        thePlayer.moveSpeed = 0;
        retry.gameObject.SetActive(false);
        resetstopwatch();
        levelstart = false;
        thePlayer.happy = false;
        thePlayer.lost = false;
        wrong.SetActive(false);
         theStopper.stopperReset();
         thePlayer.standup = false;
         manholecover.SetActive(true);
        stuntfailed.SetActive(false);
        failprompt = false;
        //monster.transform.position = monsterstartpoint;
        distancemarking.transform.position = distancemarkstartpoint;
        question2.SetActive(false);
        start.gameObject.SetActive(true);
        StartCoroutine(resettiles());
        theboard.transit = 2; 
        thedirector.action = false;
         thecamera.camActive = false;
         thecamera.stuntScene = false;
         monsterready = true;
         theAct.losepointReady = false;
        thePlayer.positioning();  
         StartCoroutine(resetstick());
         ceilingstopper.SetActive(true);
        meterwall.transform.position = new Vector2(0, -1.38f);
        metertxt.text = ("0m");
        meter.transform.localScale = new Vector2(0, 0.05f);
        meterimage.transform.position = new Vector2(0, -1.67f);
        transition.SetActive(true);
        StartCoroutine(startgame());
    }
    IEnumerator continuetonext()
    {  
        transition.SetActive(false);
        yield return new WaitForSeconds(4);
        theAct.endbgexit();
        yield return new WaitForSeconds(1);
        thePlayer.transform.position = PlayerStartPoint;
        thePlayer.happy = false;
        manhole.SetActive(true);
        theparax.moveSpeed = 0;
        
        BGstates.transform.position = new Vector2(-12f, 2.8f);
        jumpers.SetActive(false);
        answerinput.SetActive(true);
       
        resetstopwatch();
        //level1manager.SetActive(false);
        StartCoroutine(resettiles());
        
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
         stage.text = ("distance");
         thePlayer.posready = true;
         safewall.SetActive(false);
         StartCoroutine(startgame());
         //yield return new WaitForSeconds(2);
          //start.SetActive(true);
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
    IEnumerator resettiles()
    {
       
        destroytiles = true;
        yield return new WaitForSeconds(0);
        destroytiles = false;
    }
     IEnumerator resetstick()
    {
       
        destroystick = true;
        yield return new WaitForSeconds(1);
        destroystick = false;
    }
    
    IEnumerator cameraAction()
    {
        thedirector.action = true;
        thedirector.playAction();
        yield return new WaitForSeconds(3);
        playgame();
        FixedtimeStart();
    }
    IEnumerator runsafe()
    {
        yield return new WaitForSeconds(0.5f);
        //thePlayer.moveSpeed = generatespeed;
        //theparax.moveSpeed = -0.2f;
       
        //thePlayer.happy = true;
                        
        check.SetActive(true);
                       
                       
        thescorer.addingoOneCoin();
        msectxt.text = ":" + Amsec;
        sectxt.text = ":0" + Asec;
        StopCoroutine("StopWatch");
        //backwardstunt = false;
         //theflag.flagAnim = 1;
        dingling();
        levelstart = false;
        yield return new WaitForSeconds(3);
        thescorer.finalstar();
    }
    public void Action()
    {
        repos = true;
        
        
        distanceInput = float.Parse(distance.text);
        pInput = distanceInput;
        play.gameObject.SetActive(false);
    }
    IEnumerator monsterAction()
    {
        themonster.attack = true;
        yield return new WaitForSeconds(1);
        themonster.attack = false;
        monsterready = false;
    }
     void buzzzing()
    {
        if(buzz)
        {
            wrongsfx.Play(0);
            thecamshake.collapse();
            StartCoroutine(cutvoice());
            buzz = false;       
        }
        
    }
     void wasted()
    {
        if(busted)
        {
            bustedsfx.Play(0);
            
            busted = false;       
        }
        
    }
     void dingling()
    {
        if(ding)
        {
            correctsfx.Play();
            thecamshake.collapse();
            StartCoroutine(cutvoice());
            ding = false;
        }
    }
    IEnumerator cutvoice()
    {
        yield return new WaitForSeconds(2);
        bustedsfx.Play(0);
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
   
    
   

}

