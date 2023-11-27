using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld;

public class PizzaWorld : Mod
{
    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        Message messageType = (Message)reader.ReadByte();

        if (messageType == Message.SpawnNPC)
        {
            int type = reader.ReadInt32();
            int netId = reader.ReadInt32();
            
            SpawnNPCForServer(type, netId);
        }  
    }
    
#if false
// TODO: Test this syncing method (a lot simpler)
   int slot = NPC.NewNPC(new EntitySource_DebugCommand($"{nameof(ExampleMod)}_{nameof(ExampleSummonCommand)}"), xSpawnPosition, ySpawnPosition, type);

    // Sync of NPCs on the server in MP
    if (Main.netMode == NetmodeID.Server && slot < Main.maxNPCs) {
        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, slot);
    } 
#endif

    public static void SpawnNPC<T>()
        where T : ModNPC
    {
        if(Main.netMode == NetmodeID.SinglePlayer)
            // TODO: Just spawn an NPC.
            return;
        
        ModPacket packet = ModContent.GetInstance<PizzaWorld>().GetPacket();
        ModNPC npc = ModContent.GetModNPC(ModContent.NPCType<T>());
        packet.Write((byte)Message.SpawnNPC);
        packet.Write(npc.Type);
        packet.Write(npc.NPC.netID);
        packet.Send();
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

public enum Message : byte
{
    SpawnNPC = 0,
}