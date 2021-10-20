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
    void Update()
    {

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
