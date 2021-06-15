using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AccHardTwo : MonoBehaviour
{
    public float dX, dY, viT, aT, timer, vB, angleB, angleA, sideB, sideC, totalDistance, correctAnswer, answer, playerAnswer;
    //public Quaternion angleB;
    public GameObject gunBarrel, gun, target, targetWheel, projectileLine, dimensions, cam;
    public GameObject verticalOne, horizontal;
    public GameObject bulletPos, wheelPos, bulletHere, targetHere;
    public TruckManager theTruck;
    public MulticabManager theMulticab;
    public Hellicopter theChopper;
    public ShootManager theShoot;
    public DistanceMeter[] theMeter;
    public CircularAnnotation theCurve;
    public QuestionController theQuestion;
    private BulletManager theBullet;
    public AccHardSimulation theSimulate;
    float generateAngleB, generateViT, generateAT, generateVB, generateDX, generateDY;
    float ChopperY, chopperX, truckTime, bulletTime;
    bool shoot, shootReady, gas;
    public bool posCheck;
     public TMP_Text timertxt, timertxtTruck, actiontxt;
     float camPos;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(positioning());
        camPos = cam.transform.position.x - theChopper.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        cam.transform.position = new Vector3(theChopper.transform.position.x + camPos, cam.transform.position.y, cam.transform.position.z);
    }
    public void generateProblem()
    {

    }
    IEnumerator positioning()
    {
        Time.timeScale = 1;
        theTruck.moveSpeed = 3;
        theChopper.flySpeed = 10;
        theMulticab.moveSpeed = 10;
        yield return new WaitForSeconds(3);
        generateProblem();
        theMulticab.moveSpeed = 0;
        theTruck.moveSpeed = 0;
        theChopper.flySpeed = 0;
    }
}
