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
        bool enoughTiles = Main.SceneMetrics.GetTileCount((ushort)ModContent.TileType<PizzaTile>()) > 40;
        bool notAbove = player.ZoneSkyHeight || player.ZoneOverworldHeight;
        return enoughTiles && notAbove;
    }
}