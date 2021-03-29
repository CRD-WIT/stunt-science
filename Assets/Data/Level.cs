class Level
{
    public string title;
    public string question;
    public int[] constants;
    public int argumentCount;
    public int minRange;
    public int maxRange;
    public string formula;
    public string expression;
    public string determinant;
    public float speed;
    public float distance;
    public float time;

    public void setSpeed(float s)
    {
        speed = s;
    }

    public void setDistance(float d)
    {
        distance = d;
    }

    public void setTime(float t)
    {
        time = t;
    }


    public float getSpeed()
    {
        return speed;
    }

    public float getDistance()
    {
        return distance;
    }

    public float getTime()
    {
        return time;
    }
}