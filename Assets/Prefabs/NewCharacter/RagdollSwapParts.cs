using UnityEngine;
using UnityEngine.U2D.Animation;

public class RagdollSwapParts : MonoBehaviour
{
    [SerializeField] SpriteResolver[] swappableParts;
    string gender;
    public bool isMale;  
    void Start()
    {
        gender = PlayerPrefs.GetString("Gender"); //::Actual value of gender
        
        swappableParts[0].SetCategoryAndLabel("Head", gender);
        swappableParts[1].SetCategoryAndLabel("LowerBody", gender);
    }
}
