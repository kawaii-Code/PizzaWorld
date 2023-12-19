using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.Localization;

namespace PizzaWorld.Code.Utilities;

public static class Debug
{
    public static void Log(object message)
    {
        Message(message.ToString(), Color.White);
    }

    public static void Log(object message, Color color)
    {
        Message(message.ToString(), color);
    }
    
    public static void LogWarning(string message)
    {
        Message(message, Color.Orange);
    }

    public static void LogError(string message)
    {
        Message(message, Color.Red);
    }

    private static void Message(string message, Color color)
    {
        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(message), color);
    }
}