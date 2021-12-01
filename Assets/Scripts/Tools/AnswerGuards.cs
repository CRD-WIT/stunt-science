using UnityEngine;
public class AnswerGuards : MonoBehaviour
{
    public bool AnswerIsInRange(float targetValue, float currentValue, float minMaxValue)
    {
        float tm_min = (float)(targetValue - minMaxValue);
        float tm_max = (float)(targetValue + minMaxValue);
        float curr = (float)(currentValue);
        bool result = ((tm_min == curr) || (tm_max == curr) || (targetValue == currentValue));
        Debug.Log($"tmmin: {tm_min}, tmmax: {tm_max}, curr:{curr}");
        return result;
    }

    public float AdjustAnswer(float targetValue, float currentValue, float minMaxValue){
        if(targetValue>currentValue){
            return targetValue - minMaxValue;
        }else{
            return targetValue + minMaxValue;
        }
    }
}