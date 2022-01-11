using System.Collections;
using UnityEngine;
using TMPro;

public class testProjectile : MonoBehaviour
{
    public GameObject bullet, target;
    public float Vo, angle, distanceX,disyanceY, time, timer, angleAlpha, Vy, totalAngle;
    bool startTime;
    public TMP_Text timerTxt, timeTxt, voTxt, distanceXtxt,distanceYtxt;
    // Start is called before the first frame update
    void Start()
    {
        target.transform.position = new Vector2(transform.position.x + distanceX, transform.position.y + disyanceY);
        StartCoroutine(startShooting());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //time = (2 * Vo * Mathf.Sin((45)* Mathf.Deg2Rad)) / 9.8f;
        //angleAlpha = Mathf.Atan(disyanceY/ distanceX) * Mathf.Rad2Deg;
        //Vy = 9.8f * Mathf.Cos(angleAlpha* Mathf.Deg2Rad);
        timeTxt.text = ("time = ") + time.ToString("F2") + "s";
        voTxt.text = "Vo = " + Vo.ToString("F2")+" m/s";
        distanceXtxt.text = "distance = "  + distanceX.ToString("F2") + " m";
        distanceYtxt.text = "height = "+ disyanceY.ToString("F2")+ " m";
        timerTxt.text = timer.ToString("F2");
        angle = Mathf.Atan(((disyanceY + ((9.8f/2)*(time * time)) )/ distanceX)) * Mathf.Rad2Deg;
        Vo = distanceX / (Mathf.Cos((angle* Mathf.Deg2Rad))* time); 
        //angle = Mathf.Atan(((time * (disyanceY+ (9.8f/2) * (time*time)))/distanceX))*Mathf.Rad2Deg;
        //angle = Mathf.Acos(distanceX/(Vo* time)) * Mathf.Rad2Deg;
        //angle = Mathf.Asin((time * Vy)/(2 * Vo)) * Mathf.Rad2Deg;
        //totalAngle = angle + angleAlpha;
        this.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle);
        //time = distanceX / (Vo * (Mathf.Cos(angle * Mathf.Deg2Rad)));
        //Vo = ((Mathf.Sqrt((Mathf.Abs(Physics2D.gravity.y) / (2 * ((distanceX * (Mathf.Tan((angle) * Mathf.Deg2Rad))) - disyanceY))))) * distanceX) / (Mathf.Cos((angle) * Mathf.Deg2Rad));
        if(startTime)
        {
            timer += Time.fixedDeltaTime;
            if(timer >= time)
            {
                startTime = false;
                //Time.timeScale = 0;
            }
        }
    }
    public void Shoot()
    {
        startTime = true;
        bullet.SetActive(true);
        bullet.transform.position = transform.position;
        bullet.GetComponent<Rigidbody2D>().velocity = transform.right * Vo;
    }
    IEnumerator startShooting()
    {
        
        yield return new WaitForSeconds(2);
        Shoot();
    }
}
