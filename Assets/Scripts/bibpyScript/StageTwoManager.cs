using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageOneManager : MonoBehaviour
{
    private Player thePlayer;
    float distance;
    float speed;
    float finalSpeed;
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<Player>();
        generateProblem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void generateProblem()
    {
        distance = Random.Range(5, 10);
        speed = Random.Range(2.0f, 5.0f);
        finalSpeed = (float)System.Math.Round(speed, 1);
    }
}
