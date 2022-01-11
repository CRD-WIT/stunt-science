using System.Collections;
using UnityEngine;

public class stampSfxManager : MonoBehaviour
{
    public AudioSource stampSfx, swooshSfx;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(stamping());
    }

    IEnumerator stamping()
    {
        swooshSfx.Play();
        yield return new WaitForSeconds(1);
        stampSfx.Play();
    }
}
