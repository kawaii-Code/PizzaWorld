using PizzaWorld.Code.Tiles;
using Terraria;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Systems;

public class PizzaBiome : ModBiome
{
    public override int Music =>
        MusicLoader.GetMusicSlot(Mod, "Music/PizzaBiomeSoundtrack");

    public override bool IsBiomeActive(Player player)
    {
        int tileCount = Main.SceneMetrics.GetTileCount((ushort)ModContent.TileType<PizzaTile>());
        if (tileCount > 50)
        {
            return true;
        }
        return false;
    }
}