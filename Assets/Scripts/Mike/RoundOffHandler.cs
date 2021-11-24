public class RoundOffHandler
{
    public float num;
    public float decimalValue, finalValue, finalDecimalValue;
    public float Round(float decimalNumber, int decimalPlace)
    {
        switch (decimalPlace)
        {
            case 1:
                decimalValue = decimalNumber % 1;
                num = decimalNumber - decimalValue;
                if (decimalValue < 0.5f)
                {
                    float Wd = decimalValue * 10;
                    float Dd = Wd % 1;
                    float cappedValue = Wd - Dd;
                    finalDecimalValue = (cappedValue) * 0.1f;
                    finalValue = num + finalDecimalValue;
                }
                else
                {
                    float Wd = decimalValue * 10;
                    float Dd = Wd % 1;
                    float cappedValue = (Wd - Dd) + 1;
                    finalDecimalValue = (cappedValue) * 0.1f;
                    finalValue = num + finalDecimalValue;
                }
                break;
            case 2:
                decimalValue = decimalNumber % 1;
                num = decimalNumber - decimalValue;
                float dv = (decimalValue * 100) % 1;
                if (dv < 0.5f)
                {
                    float Wd = dv * 10;
                    float Dd = Wd % 1;
                    float cappedValue = (Wd - Dd) + (decimalValue - (decimalNumber % 1));
                    finalDecimalValue = (decimalValue);
                    finalValue = num + finalDecimalValue;
                }
                else
                {
                    dv = 1;
                    finalDecimalValue = (decimalValue + 0.01f);
                    finalValue = num + finalDecimalValue;
                }
                break;
        }
        return finalValue;
    }
}