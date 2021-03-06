using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class LevelFinishedPanel : MonoBehaviour
{
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;

    public Sprite star1Active;
    public Sprite star1Inactive;

    public Sprite star2Active;
    public Sprite star2Inactive;

    public Sprite star3Active;
    public Sprite star3Inactive;

    [Range(0, 3)] public int score;
    public HeartManager heartManager;
    public AudioSource starSound;

    // Start is called before the first frame update

    // Update is called once per frame

    IEnumerator PlayStarSound(float count)
    {
        yield return new WaitForSeconds(1f + count);
        starSound.Play();
        yield return new WaitForSeconds(2f + count);
        starSound.Play();
        yield return new WaitForSeconds(3f + count);
        starSound.Play();
    }
    void Update()
    {
        switch (heartManager.life)
        {
            case 1:
                //StartCoroutine(PlayStarSound(1f));
                star1.GetComponent<Image>().sprite = star1Inactive;
                star2.GetComponent<Image>().sprite = star2Active;
                star3.GetComponent<Image>().sprite = star3Inactive;
                break;
            case 2:
                //StartCoroutine(PlayStarSound(2f));
                star1.GetComponent<Image>().sprite = star1Active;
                star2.GetComponent<Image>().sprite = star2Active;
                star3.GetComponent<Image>().sprite = star3Inactive;
                break;
            case 3:
                //StartCoroutine(PlayStarSound(3f));
                star1.GetComponent<Image>().sprite = star1Active;
                star2.GetComponent<Image>().sprite = star2Active;
                star3.GetComponent<Image>().sprite = star3Active;
                break;
            default:
                star1.GetComponent<Image>().sprite = star1Inactive;
                star2.GetComponent<Image>().sprite = star2Inactive;
                star3.GetComponent<Image>().sprite = star3Inactive;
                break;
        }
    }
}
