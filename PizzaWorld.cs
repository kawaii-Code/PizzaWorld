using System.IO;
using Terraria;
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
            SpawnNPC(type, netId);
        }
        
        // base.HandlePacket(reader, whoAmI);
    }

    private void SpawnNPC(int type, int netId)
    {
        Player player = Main.player[0];
        
        int x = (int)player.Bottom.X + player.direction * 200;
        int y = (int)player.Bottom.Y;
        NPC spawnedNpc = NPC.NewNPCDirect(Entity.GetSource_NaturalSpawn(), x, y, type);

        if (netId < 0)
        {
            spawnedNpc.SetDefaults(netId);
        }
    }
}