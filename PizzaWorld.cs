using System.IO;
using PizzaWorld.NPCs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace PizzaWorld;

public class PizzaWorld : Mod
{
    public static PizzaWorld Instance;

    public override void Load()
    {
        Instance = this;
    }
    
    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        int messageType = reader.ReadInt32();

        if (messageType == 0)
        {
            int type = reader.ReadInt32();
            int netId = reader.ReadInt32();
            
            SpawnNPCForServer(type, netId);
        }  
    }

    private static void SpawnNPCForServer(int type, int netId)
    {
        Player player = Main.player[0];
        
        int x = (int)player.Bottom.X + player.direction * 200;
        int y = (int)player.Bottom.Y;
        NPC spawnedNpc = NPC.NewNPCDirect(new EntitySource_Film(), x, y, type);

        if (netId < 0)
        {
            spawnedNpc.SetDefaults(netId);
        }
    }
    
    
}