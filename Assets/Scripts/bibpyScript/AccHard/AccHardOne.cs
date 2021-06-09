using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccHardOne : MonoBehaviour
{
    public float xg, xy, viT, aT, time, vB, angleB;
    //public Quaternion angleB;
    public GameObject gunBarrel,gun;
    public TruckManager theTruck;
    public Hellicopter theChopper;
    public ShootManager theShoot;
    float generateAngleB, generateViT, generateAT, generateVB;
    // Start is called before the first frame update
    void Start()
    {
        generateProblem();
    }

    // Update is called once per frame
    void Update()
    {
        gun.transform.rotation = Quaternion.Euler(gun.transform.rotation.x,gun.transform.rotation.y,angleB);
        theShoot.speed = vB;
        theTruck.accelerating = true;
        theTruck.moveSpeed = viT;
        theTruck.accelaration = aT;
    }
    public void generateProblem()
    {
        generateAngleB = Random.Range(-20f, -60f);
        angleB =(float)System.Math.Round(generateAngleB, 2);
        generateViT = Random.Range(3f,5f);
        viT = (float)System.Math.Round(generateViT, 2);
        generateAT = Random.Range(3f, 5f);
        aT = (float)System.Math.Round(generateAT, 2);
        generateVB = Random.Range(20f, 30f);
        vB = (float)System.Math.Round(generateVB, 2);
    }
}
