using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AvatarImageManager : MonoBehaviour
{
    public Sprite maleAvatarImage;
    public Sprite maleAvatarEyeClosed;
    public Sprite femaleAvatarImage;
    public Sprite femaleAvatarEyeClosed;
    public GameObject avatarImageContainer;

    public bool isMale = true;

    public Sprite activeSprite;

    void Start()
    {
        isMale = PlayerPrefs.GetString("Gender") == "Male" ? true : false;
        activeSprite = isMale ? maleAvatarImage : femaleAvatarImage;
        avatarImageContainer.GetComponent<Image>().sprite = activeSprite;
        StartCoroutine(EyeLoop());
    }

    IEnumerator EyeLoop()
    {

        while (true)
        {


            yield return new WaitForSeconds(Random.Range(2, 4));

            activeSprite = isMale ? maleAvatarEyeClosed : femaleAvatarEyeClosed;
            avatarImageContainer.GetComponent<Image>().sprite = activeSprite;
            Debug.Log(activeSprite.name);

            yield return new WaitForSeconds(0.3f);

            activeSprite = isMale ? maleAvatarImage : femaleAvatarImage;
            avatarImageContainer.GetComponent<Image>().sprite = activeSprite;

            Debug.Log(activeSprite.name);


        }

    }
}
