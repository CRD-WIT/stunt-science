using System.Collections;
using UnityEngine;

public class stampSfxManager1 : MonoBehaviour
{
    public AudioSource swoosh, stamp;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Stamping());
    }

    IEnumerator Stamping()
    {
        swoosh.Play();
        yield return new WaitForEndOfFrame();
        stamp.Play();
    }
}
