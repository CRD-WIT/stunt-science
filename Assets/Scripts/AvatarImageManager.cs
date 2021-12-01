using UnityEngine;
using UnityEngine.UI;

public class AvatarImageManager : MonoBehaviour
{
    public Sprite maleAvatarImage;
    public Sprite femaleAvatarImage;
    public GameObject avatarImageContainer;

    public bool isMale = true;

    void Start()
    {
        isMale = PlayerPrefs.GetString("Gender") == "Male" ? true : false;
    }
    void Update()
    {
        avatarImageContainer.GetComponent<Image>().sprite = isMale ? maleAvatarImage : femaleAvatarImage;
    }
}
