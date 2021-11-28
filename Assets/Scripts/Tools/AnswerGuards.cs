using UnityEngine;
public class AnswerGuards : MonoBehaviour
{
    public bool AnswerIsInRange(float targetValue, float currentValue, float minMaxValue)
    {
        float tm_min = (float)(targetValue - minMaxValue);
        float tm_max = (float)(targetValue + minMaxValue);
        float curr = (float)(currentValue);

        return  ((tm_min == curr) || (tm_max == curr) || (targetValue == currentValue));
               
    }
}