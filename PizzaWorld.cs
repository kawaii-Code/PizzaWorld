using System.IO;
using Microsoft.Xna.Framework;
using PizzaWorld.Code.Items.Food;
using PizzaWorld.Code.NPCs.Bosses;
using PizzaWorld.Code.Utilities;
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
            int x = reader.ReadInt32();
            int y = reader.ReadInt32();
            
            SpawnNPCForServer(type, netId, x, y);
            return;
        }

        if (messageType == Message.SpawnItem)
        {
            int type = reader.ReadInt32();
            int netId = reader.ReadInt32();
            int x = reader.ReadInt32();
            int y = reader.ReadInt32();

            SpawnItemForServer(type, netId, x, y);
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

    public static void SpawnNPC<T>(int spawnX = -1, int spawnY = -1)
        where T : ModNPC
    {
        Player player = Main.player[0];
        int x = spawnX == -1 ? (int)player.Bottom.X + player.direction * 200 : spawnX;
        int y = spawnY == -1 ? (int)player.Bottom.Y : spawnY;
        
        if (Main.netMode == NetmodeID.SinglePlayer)
        {
            NPC.NewNPCDirect(new EntitySource_Film(), x, y, ModContent.NPCType<T>());
            return;
        }
        
        ModPacket packet = ModContent.GetInstance<PizzaWorld>().GetPacket();
        ModNPC npc = ModContent.GetModNPC(ModContent.NPCType<T>());
        packet.Write((byte)Message.SpawnNPC);
        packet.Write(npc.Type);
        packet.Write(npc.NPC.netID);
        packet.Write(x);
        packet.Write(y);
        packet.Send();
    }

    public static void SpawnItem<T>(int x, int y) where T : ModItem
    {
        if (Main.netMode == NetmodeID.SinglePlayer)
        {
            return;
        }

        ModPacket packet = ModContent.GetInstance<PizzaWorld>().GetPacket();
        ModItem item = ModContent.GetModItem(ModContent.ItemType<T>());
        packet.Write((byte)Message.SpawnItem);
        packet.Write(item.Type);
        packet.Write(item.Item.netID);
        packet.Write(x);
        packet.Write(y);
        packet.Send();
        
        Debug.Log("Send packet");
    }

    private static void SpawnNPCForServer(int type, int netId, int spawnX, int spawnY)
    {
        Player player = Main.player[0];
        
        int x = spawnX == -1 ? (int)player.Bottom.X + player.direction * 200 : spawnX;
        int y = spawnY == -1 ? (int)player.Bottom.Y : spawnY;
        NPC spawnedNpc = NPC.NewNPCDirect(new EntitySource_Film(), x, y, type);

        if (netId < 0)
        {
            spawnedNpc.SetDefaults(netId);
        }
    }

    private static void SpawnItemForServer(int type, int netId, int spawnX, int spawnY)
    {
        var myPlayer = Main.player[Main.myPlayer];
        
        /*var spawnedId= Item.NewItem(new EntitySource_Loot(Main.npc[ModContent.NPCType<PizzaBoss>()]), new Vector2(spawnX, spawnY),
            new Vector2(20, 20), ModContent.GetModItem(ModContent.ItemType<Napoletana>()).Item);*/
        
        //np.DropItemInstanced(np.Center + new Vector2(0, 30), new Vector2(20, 20), ModContent.ItemType<Napoletana>());

        int id = myPlayer.QuickSpawnItem(new EntitySource_Film(), ModContent.ItemType<Napoletana>());

        var spawnedItem = Main.item[id];
        
        Debug.Log("Spawn item" );
        if (netId < 0)
        {
            spawnedItem.SetDefaults(netId);
        }
    }
}

public enum Message : byte
{
    SpawnNPC = 0,
    SpawnItem = 1
}

