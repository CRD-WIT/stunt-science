using UnityEngine;
using UnityEngine.U2D.Animation;

public class CharacterSwapPartsForRegistration : MonoBehaviour
{
    [SerializeField] SpriteResolver[] swappableParts;
    string gender;
    public bool isMale;  
    void Start()
    {
        if (isMale)
            gender = "Male";
        else
            gender = "Female";
    }
    void Update()
    {
        swappableParts[0].SetCategoryAndLabel("Head", gender);
        swappableParts[4].SetCategoryAndLabel("LowerBody", gender);

        /*
            For Skins:
                swappableParts[0].SetCategoryAndLabel("Head", skinName);
                swappableParts[1].SetCategoryAndLabel("UpperBody", skinName);
                swappableParts[2].SetCategoryAndLabel("R_Arm", skinName);
                swappableParts[3].SetCategoryAndLabel("L_Arm", skinName);
                swappableParts[4].SetCategoryAndLabel("LowerBody", skinName);
        */
    }
}
