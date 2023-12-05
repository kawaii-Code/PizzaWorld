namespace PizzaWorld.Code.Utilities;

public static class Price
{
    public static int Silver(int value)
    {
        return 100 * value;
    }

    public static int Gold(int value)
    {
        return 100 * Silver(value);
    }

    public static int Platinum(int value)
    {
        return 1000 * Gold(value);
    }
}