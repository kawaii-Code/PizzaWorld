using PizzaWorld.Code.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Systems;

public class PizzaBiome : ModBiome
{
    public override int Music =>
        MusicLoader.GetMusicSlot(Mod, "Music/PizzaBiome");
    public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<PizzaBiomeSurfaceBackgroundStyle>();

    public override bool IsBiomeActive(Player player)
    {
        bool enoughTiles = Main.SceneMetrics.GetTileCount((ushort)ModContent.TileType<PizzaTile>()) > 40;
        bool notAbove = player.ZoneSkyHeight || player.ZoneOverworldHeight;
        return enoughTiles && notAbove;
    }
}

public class PizzaBiomeSurfaceBackgroundStyle : ModSurfaceBackgroundStyle
{
    public override void Load()
    {
        BackgroundTextureLoader.AddBackgroundTexture(Mod, "PizzaWorld/Assets/Textures/PizzaBiomeClose");
        BackgroundTextureLoader.AddBackgroundTexture(Mod, "PizzaWorld/Assets/Textures/PizzaBiomeMid");
        BackgroundTextureLoader.AddBackgroundTexture(Mod, "PizzaWorld/Assets/Textures/PizzaBiomeFar");
    }

    public override void ModifyFarFades(float[] fades, float transitionSpeed)
    {
        for (int i = 0; i < fades.Length; i++)
        {
            if (i == Slot)
            {
                fades[i] += transitionSpeed;
                if (fades[i] > 1f)
                    fades[i] = 1f;
            }
            else
            {
                fades[i] -= transitionSpeed;
                if (fades[i] < 0f)
                    fades[i] = 0f;
            }
        }
    }

    public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
    {
        return BackgroundTextureLoader.GetBackgroundSlot("PizzaWorld/Assets/Textures/PizzaBiomeClose");
    }

    public override int ChooseMiddleTexture()
    {
        return BackgroundTextureLoader.GetBackgroundSlot("PizzaWorld/Assets/Textures/PizzaBiomeMid");
    }

    public override int ChooseFarTexture()
    {
        return BackgroundTextureLoader.GetBackgroundSlot("PizzaWorld/Assets/Textures/PizzaBiomeFar");
    }
}