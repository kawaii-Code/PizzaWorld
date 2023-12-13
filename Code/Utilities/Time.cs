namespace PizzaWorld.Code.Utilities;

public static class Time
{
    public static int Seconds(int value)
    {
        return 60 * value;
    }
    
    public static int Minutes(int value)
    {
        return 60 * Seconds(value);
    }
}