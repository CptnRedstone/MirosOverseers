using BepInEx;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using Noise;
using RWCustom;
using System;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using Menu.Remix.MixedUI.ValueTypes;
using System.Linq;

#pragma warning disable CS0618

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace MirosOverseers;

[BepInPlugin("CaptainRedstone.MirosOverseers", "MirosOverseers", "1.0.0")]
public partial class MirosOverseers : BaseUnityPlugin
{
    public static MirosOverseers modInstance;
    public static MirosOverseersOptions optionsInstance;
    public MirosOverseers()
    {
        try
        {
            optionsInstance = new MirosOverseersOptions(this);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
        }
    }
    private void OnEnable()
    {
        modInstance = this;
        On.RainWorld.OnModsInit += RainWorldOnOnModsInit;
    }
    public void LogInfo(object data)
    {
        modInstance.Logger.LogInfo(data);
    }

    //Todo thumbnail
    //Review sound loop discard issue (definite for meadow)
    //STUPID CONTROLLER NAVIGATION
    //Use a different type of scroller? Does that fix the wrapper selection issue?
    //Lasers turn red when disappearing
    //Lasers graphics have a max length
    //More specifically prevent overseers from killing each other but allow other explosions?

    //Meadow compat and remix setting sync

    private bool IsInit;
    private void RainWorldOnOnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
    {
        orig(self);
        if (IsInit) return;

        try
        {
            IsInit = true;
            MachineConnector.SetRegisteredOI("CaptainRedstone.MirosOverseers", optionsInstance);
        }
        catch (Exception ex) { modInstance.Logger.LogError(ex); }

        try { IL.Explosion.Update += IL_Explosion_Update; } catch (Exception ex) { modInstance.Logger.LogError(ex); }
        On.Overseer.ctor += On_Overseer_Ctor;
        On.Overseer.Die += On_Overseer_Die;
        On.Overseer.Update += On_Overseer_Update;
        On.Overseer.Violence += On_Overseer_Violence;
        On.OverseerGraphics.ctor += On_OverseerGraphics_Ctor;
        On.Region.ctor_string_int_int_RainWorldGame_Timeline += HolyFunctionNameBatman;
        try { IL.WorldLoader.GeneratePopulation += IL_Worldloader_Generate_Population; } catch (Exception ex) { modInstance.Logger.LogError(ex); }

        On.WorldLoader.GeneratePopulation += OnOverseerSpawnDebug;
        try { IL.WorldLoader.GeneratePopulation += ILOverseerSpawnDebug; } catch (Exception ex) { modInstance.Logger.LogError(ex); }
    }
    private void OnOverseerSpawnDebug(On.WorldLoader.orig_GeneratePopulation orig, WorldLoader self, bool fresh)
    {
        orig(self, fresh);

        modInstance.Logger.LogInfo("Overseer Guaranteed Region: " + (self.world.region.name == "UW" || (ModManager.MSC && (self.world.region.name == "LC" || self.world.region.name == "LM"))));
        modInstance.Logger.LogInfo("Overseer Local Spawn Chance: " + (self.world.region.regionParams.overseersSpawnChance * Mathf.InverseLerp(-1f, 21f, (self.game.session as StoryGameSession).saveState.cycleNumber + ((self.game.StoryCharacter == SlugcatStats.Name.Red) ? 17 : 1))));
        modInstance.Logger.LogInfo("Overseer Spawns Allowed: " + (!ModManager.MSC || !(self.playerCharacter == MoreSlugcats.MoreSlugcatsEnums.SlugcatStatsName.Artificer) || (self.game.session as StoryGameSession).saveState.cycleNumber != 0));
        modInstance.Logger.LogInfo("Overseer Local Min Max: [" + self.world.region.regionParams.overseersMin + ", " + self.world.region.regionParams.overseersMax + "]");
        modInstance.Logger.LogInfo("Iggy's Opinion of Player: " + self.game.GetStorySession.saveState.miscWorldSaveData.playerGuideState.likesPlayer);
        modInstance.Logger.LogInfo("Iggy is Depressed: " + !self.game.GetStorySession.saveState.miscWorldSaveData.playerGuideState.increaseLikeOnSave);
        modInstance.Logger.LogInfo("Iggy Hates Player: " + self.game.GetStorySession.saveState.miscWorldSaveData.playerGuideState.angryWithPlayer);
    }
    private void ILOverseerSpawnDebug(ILContext il)
    {
        try
        {
            int loc = 0;
            ILCursor cursor = new(il);
            cursor.GotoNext(MoveType.After,
                x => x.MatchLdfld(typeof(Region.RegionParams), nameof(Region.regionParams.overseersMax)),
                x => x.MatchCall(typeof(UnityEngine.Random), nameof(UnityEngine.Random.Range)),
                x => x.MatchStloc(out loc));
            cursor.Emit(OpCodes.Ldloc_S, (byte)loc);
            cursor.EmitDelegate(delegate(int x) { modInstance.Logger.LogInfo("Wild Overseers spawned this cycle; count is " + x); });
        }
        catch (Exception ex) { modInstance.Logger.LogError(ex); }
    }
    private void IL_Worldloader_Generate_Population(ILContext il)
    {
        try
        {
            ILCursor cursor = new(il);
            cursor.GotoNext(MoveType.Before, x => x.MatchLdcR4(2), x => x.MatchLdcR4(21));
            cursor.Index++;
            cursor.Emit(OpCodes.Pop);
            cursor.EmitDelegate(delegate() { return optionsInstance.AllowEarlyOverseers.Value ? -1f : 2f; });
        }
        catch (Exception ex) { modInstance.Logger.LogError(ex); }
    }
    private void HolyFunctionNameBatman(On.Region.orig_ctor_string_int_int_RainWorldGame_Timeline orig, Region self, string name, int firstRoomIndex, int regionNumber, RainWorldGame game, SlugcatStats.Timeline timelineIndex)
    {
        orig(self, name, firstRoomIndex, regionNumber, game, timelineIndex);
        if (optionsInstance.GuaranteeIggy.Value) { self.regionParams.playerGuideOverseerSpawnChance = 999999999; }
        if (optionsInstance.GuaranteeWildOverseers.Value) { self.regionParams.overseersSpawnChance = 999999999; }
        if (optionsInstance.AdjustOverseerSpawns.Value)
        {
            self.regionParams.overseersMin = (int)((self.regionParams.overseersMin + optionsInstance.OverseersMinPlus.Value) * optionsInstance.OverseersMinTimes.Value);
            self.regionParams.overseersMax = (int)((self.regionParams.overseersMax + optionsInstance.OverseersMaxPlus.Value) * optionsInstance.OverseersMaxTimes.Value);
        }
    }
    public void On_Overseer_Violence(On.Overseer.orig_Violence orig, Overseer self, BodyChunk source, Vector2? directionAndMomentum, BodyChunk hitChunk, PhysicalObject.Appendage.Pos hitAppendage, Creature.DamageType type, float damage, float stunBonus)
    {
        if ((type != Creature.DamageType.Explosion || !optionsInstance.OverseersExplosionImmune.Value))
        {
            orig(self, source, directionAndMomentum, hitChunk, hitAppendage, type, damage, stunBonus);
        }
    }
    public void IL_Explosion_Update(ILContext il)
    {
        //if (optionsInstance.ArtificerVulnerability.Value && sourceObject is Overseer) {num8 *= 5}
        try
        {
            ILCursor cursor = new(il);
            ILLabel stloc_label = cursor.DefineLabel();
            int stloc_int = 0;
            cursor.GotoNext(MoveType.After, x => x.MatchLdfld(nameof(Explosion), nameof(Explosion.damage)), x => x.MatchStloc(out stloc_int));
            cursor.GotoNext(MoveType.After, x => x.MatchLdloc(stloc_int), x => x.MatchLdcR4(0.2f), x => x.MatchMul());
            cursor.EmitDelegate(delegate() { return optionsInstance.ArtificerVulnerability.Value; });
            cursor.Emit(OpCodes.Brfalse, stloc_label); //Brnull
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Ldfld, typeof(Explosion).GetField(nameof(Explosion.sourceObject))); //Thanks Yuzugamer for the help!
            cursor.Emit(OpCodes.Isinst, typeof(Overseer));
            cursor.Emit(OpCodes.Brfalse, stloc_label); //Brnull
            cursor.Emit(OpCodes.Ldc_R4, 5f);
            cursor.Emit(OpCodes.Mul);
            cursor.MarkLabel(stloc_label);
        }
        catch (Exception ex) { modInstance.Logger.LogError(ex); }
    }




    public static ConditionalWeakTable<Overseer, OverseerWrapper> OverseerCwt = new();
    public static ConditionalWeakTable<OverseerGraphics, OverseerGraphicsWrapper> OverseerGraphicsCwt = new();
    public class OverseerWrapper
    {
        public int laserCounter;
        public LightSource laserLight;
    }
    public class OverseerGraphicsWrapper
    {
        public OverseerMirosLaser laserBeam;
    }
    public static void SetOverseerLaserCounter(Overseer overseer, int laserCounter) { OverseerCwt.GetOrCreateValue(overseer).laserCounter = laserCounter; }
    public static void SetOverseerLaserLight(Overseer overseer, LightSource lightSource) { OverseerCwt.GetOrCreateValue(overseer).laserLight = lightSource; }
    public static int GetOverseerLaserCounter(Overseer overseer) { OverseerCwt.TryGetValue(overseer, out var results); return results.laserCounter; }
    public static LightSource GetOverseerLaserLight(Overseer overseer) { OverseerCwt.TryGetValue(overseer, out var results); return results.laserLight; }
    public static void SetOverseerLaserBeam(OverseerGraphics overseer, OverseerMirosLaser laserBeam) { OverseerGraphicsCwt.GetOrCreateValue(overseer).laserBeam = laserBeam; }
    public static OverseerMirosLaser GetOverseerLaserBeam(OverseerGraphics overseer) { OverseerGraphicsCwt.TryGetValue(overseer, out var results); return results.laserBeam; }

    private void On_Overseer_Ctor(On.Overseer.orig_ctor orig, Overseer self, AbstractCreature abstractCreature, World world)
    {
        orig(self, abstractCreature, world);

        SetOverseerLaserCounter(self, 0);
    }
    private void On_OverseerGraphics_Ctor(On.OverseerGraphics.orig_ctor orig, OverseerGraphics self, PhysicalObject ow)
    {
        orig(self, ow);

        SetOverseerLaserBeam(self, new OverseerMirosLaser(self, self.totalSprites));
        self.AddSubModule(GetOverseerLaserBeam(self));
        self.Reset();
    }
    private void On_Overseer_Die(On.Overseer.orig_Die orig, Overseer self)
    {
        if (optionsInstance.OverseersImmortal.Value) { return; }

        orig(self);

        GetOverseerLaserLight(self)?.Destroy();
    }
    private void On_Overseer_Update(On.Overseer.orig_Update orig, Overseer self, bool eu)
    {
        orig(self, eu);

        self.canBeHitByWeapons = !optionsInstance.OverseersSpearImmune.Value; //As much as I hate it, it makes sense to have vanilla change this in Update() since it does need to change based on zip status.

        int laserReaimCooldown = Math.Max((int)(optionsInstance.OverseerReaimCooldown.Value * 40), 0);
        int laserFiringCooldown = Math.Max((int)(optionsInstance.OverseerFiringCooldown.Value * 40), 0);
        int laserCooldownThreshold = Math.Max(laserFiringCooldown, laserReaimCooldown);
        int laserAimingDuration = Math.Max((int)(optionsInstance.OverseerAimingDuration.Value * 40), 0);

        bool inCutscene = false;
        bool dialogueExistsInRoom = false;
        for (int i = 0; i < self.room?.game?.cameras?.Length; i++)
        {
            //Are these == trues unnecessary? Probably. However, at one point, evaluting InCutscene directly somehow broke OE_PUMP01. Naturally I can no longer reproduce this. I feel like I'm going insane.
            inCutscene |= (self.room.game.cameras[i].InCutscene == true && optionsInstance.DisableDuringCutscenes.Value);
            dialogueExistsInRoom |= (self.room.game.cameras[i].hud.dialogBox?.ShowingAMessage == true && optionsInstance.DisableDuringDialogue.Value);
        }
        bool playerLacksControl = false;
        for (int i = 0; i < self.room?.PlayersInRoom.Count; i++)
        {
            playerLacksControl |= (self.room.PlayersInRoom[i].controller != null && optionsInstance.DisableDuringForcedInput.Value);
        }
        bool puppetExistsInRoom = false;
        for (int i = 0; i < self.room?.physicalObjects[1].Count(); i++) //Oracles are always on collision layer 1
        {
            puppetExistsInRoom |= (self.room.physicalObjects[1][i] is Oracle && optionsInstance.DisableNearPuppets.Value);
        }

        //modInstance.Logger.LogInfo(self.mode + " " + GetOverseerLaserCounter(self));
        if ((self.mode == Overseer.Mode.Watching || self.mode == Overseer.Mode.Conversing || self.mode == Overseer.Mode.Projecting) && !self.dead && !inCutscene && !dialogueExistsInRoom && !puppetExistsInRoom && !playerLacksControl)
        {
            SetOverseerLaserCounter(self, GetOverseerLaserCounter(self) - 1);

            if (GetOverseerLaserCounter(self) <= (0 - Math.Max(laserCooldownThreshold, 1)))
            {
                SetOverseerLaserCounter(self, laserAimingDuration);
                if (self.room != null)
                {
                    SetOverseerLaserLight(self, new LightSource(new Vector2(-1000f, -1000f), environmentalLight: false, new Color(0.1f, 1f, 0.1f), self));
                    self.room.AddObject(GetOverseerLaserLight(self));
                    GetOverseerLaserLight(self).HardSetAlpha(1f);
                    GetOverseerLaserLight(self).HardSetRad(200f);
                }
            }
            if (GetOverseerLaserCounter(self) == 0)
            {
                SetOverseerLaserCounter(self, (0-laserCooldownThreshold) + laserFiringCooldown);
                GetOverseerLaserLight(self)?.Destroy();
                if (self.room == null)
                {
                    return;
                }
                Vector2 pos = self.mainBodyChunk.pos;
                Vector2 vector = Custom.DirVec(self.AI.lookAt, pos);
                Vector2 corner = Custom.RectCollision(pos, pos - vector * 100000f, self.room.RoomRect.Grow(200f)).GetCorner(FloatRect.CornerLabel.D);
                IntVector2? intVector = SharedPhysics.RayTraceTilesForTerrainReturnFirstSolid(self.room, pos, corner);
                if (!intVector.HasValue)
                {
                    return;
                }
                Color color = new(1f, 0.4f, 0.3f);
                corner = Custom.RectCollision(corner, pos, self.room.TileRect(intVector.Value)).GetCorner(FloatRect.CornerLabel.D);
                self.room.AddObject(new Explosion(self.room, self, corner, 7, 250f, 6.2f, 2f, 280f, optionsInstance.DisableTinnitus.Value ? 0f : 0.25f, self, 0.3f, 160f, 1f));
                self.room.AddObject(new Explosion.ExplosionLight(corner, 280f, 1f, 7, color));
                self.room.AddObject(new Explosion.ExplosionLight(corner, 230f, 1f, 3, new Color(1f, 1f, 1f)));
                self.room.AddObject(new ShockWave(corner, 330f, 0.045f, 5));
                for (int i = 0; i < 25; i++)
                {
                    Vector2 vector2 = Custom.RNV();
                    if (self.room.GetTile(corner + vector2 * 20f).Solid)
                    {
                        if (!self.room.GetTile(corner - vector2 * 20f).Solid)
                        {
                            vector2 *= -1f;
                        }
                        else
                        {
                            vector2 = Custom.RNV();
                        }
                    }
                    for (int j = 0; j < (optionsInstance.LightweightExplosions.Value ? 1 : 3); j++)
                    {
                        self.room.AddObject(new Spark(corner + vector2 * Mathf.Lerp(30f, 60f, UnityEngine.Random.value), vector2 * Mathf.Lerp(7f, 38f, UnityEngine.Random.value) + Custom.RNV() * (20f * UnityEngine.Random.value), Color.Lerp(color, new Color(1f, 1f, 1f), UnityEngine.Random.value), null, 11, 28));
                    }
                    if (!optionsInstance.LightweightExplosions.Value) { self.room.AddObject(new Explosion.FlashingSmoke(corner + vector2 * (40f * UnityEngine.Random.value), vector2 * Mathf.Lerp(4f, 20f, Mathf.Pow(UnityEngine.Random.value, 2f)), 1f + 0.05f * UnityEngine.Random.value, new Color(1f, 1f, 1f), color, UnityEngine.Random.Range(3, 11))); }
                }
                for (int k = 0; k < (optionsInstance.LightweightExplosions.Value ? 0 : 6); k++)
                {
                    self.room.AddObject(new ScavengerBomb.BombFragment(corner, Custom.DegToVec((k + UnityEngine.Random.value) / 6f * 360f) * Mathf.Lerp(18f, 38f, UnityEngine.Random.value)));
                }
                self.room.ScreenMovement(corner, default, 0.9f);
                for (int l = 0; l < self.abstractPhysicalObject.stuckObjects.Count; l++)
                {
                    self.abstractPhysicalObject.stuckObjects[l].Deactivate();
                }
                self.room.PlaySound(SoundID.Bomb_Explode, corner, self.abstractCreature);
                self.room.InGameNoise(new InGameNoise(corner, 9000f, self, 1f));
            }
            else if (GetOverseerLaserLight(self) != null)
            {
                Vector2 pos = self.mainBodyChunk.pos;
                Vector2 vector = Custom.DirVec(self.AI.lookAt, pos);
                Vector2 corner = Custom.RectCollision(pos, pos - vector * 100000f, self.room.RoomRect.Grow(200f)).GetCorner(FloatRect.CornerLabel.D);
                IntVector2? intVector = SharedPhysics.RayTraceTilesForTerrainReturnFirstSolid(self.room, pos, corner);
                if (intVector.HasValue)
                {
                    corner = Custom.RectCollision(corner, pos, self.room.TileRect(intVector.Value)).GetCorner(FloatRect.CornerLabel.D);
                    GetOverseerLaserLight(self).HardSetPos(corner);
                    GetOverseerLaserLight(self).HardSetRad((GetOverseerLaserCounter(self) * 200f) / laserAimingDuration);
                    GetOverseerLaserLight(self).color = new Color((laserAimingDuration - (float)GetOverseerLaserCounter(self)) / laserAimingDuration, (float)GetOverseerLaserCounter(self) / laserAimingDuration, 0.1f);
                    GetOverseerLaserLight(self).HardSetAlpha(1f);
                }
            }
        }
        else
        {
            SetOverseerLaserCounter(self, GetOverseerLaserCounter(self) > 0
                ? Math.Min(laserReaimCooldown - laserCooldownThreshold, 0)
                : Math.Max(GetOverseerLaserCounter(self), Math.Min(laserReaimCooldown - laserCooldownThreshold, 0)));
            GetOverseerLaserLight(self)?.Destroy();
        }
    }
    public class OverseerMirosLaser : ComplexGraphicsModule.GraphicsSubModule
    {
        private readonly OverseerGraphics overseerGraphics;
        private Color laserColor;
        private Color lastLaserColor;
        private ChunkDynamicSoundLoop soundLoop;
        private float laserActive;
        private float lastLaserActive;
        private readonly float laserActiveLerpSpeed = optionsInstance.DisableLaserShader.Value ? 0.20f : 0.05f;
        private float flash;
        private float lastFlash;
        private Vector2 lastLookAt;
        private Vector2 lastLastLookAt;
        private readonly int laserAimingDuration;
        public OverseerMirosLaser(OverseerGraphics overseerGraphics, int firstSprite) : base(overseerGraphics, firstSprite)
        {
            this.overseerGraphics = overseerGraphics;
            totalSprites = 1;
            laserColor = optionsInstance.ColorChangingLaser.Value ? new Color(0f, 1f, 0.1f) : new Color(1f, 0.9f, 0f);
            lastLaserColor = laserColor;
            laserAimingDuration = Math.Max((int)(optionsInstance.OverseerAimingDuration.Value * 40), 0);
        }
        public override void Update()
        {
            lastLaserActive = laserActive;
            laserActive = Custom.LerpAndTick(laserActive, (GetOverseerLaserCounter(overseerGraphics.owner as Overseer) <= 0) ? 0f : 1f, laserActiveLerpSpeed, laserActiveLerpSpeed);
            lastLaserColor = laserColor;
            if (optionsInstance.ColorChangingLaser.Value && GetOverseerLaserCounter(overseerGraphics.owner as Overseer) >= 0) { laserColor = new Color((laserAimingDuration - (float)GetOverseerLaserCounter(overseerGraphics.owner as Overseer)) / laserAimingDuration, (float)GetOverseerLaserCounter(overseerGraphics.owner as Overseer) / laserAimingDuration, 0.1f); }
            lastFlash = flash;
            lastLastLookAt = lastLookAt; //Order of operations nonsense, don't worry about it
            lastLookAt = (overseerGraphics.owner as Overseer).AI.lookAt;
            flash = Custom.LerpAndTick(flash, 0f, 0.02f, 0.025f);

            if (soundLoop == null && laserActive > 0f)
            {
                soundLoop = new ChunkDynamicSoundLoop((overseerGraphics.owner as Overseer).mainBodyChunk) { sound = SoundID.Vulture_Grub_Laser_LOOP };
            }
            else if (soundLoop != null)
            {
                soundLoop.Volume = Mathf.InverseLerp(0.3f, 1f, laserActive);
                soundLoop.Pitch = 0.2f + 0.8f * Mathf.Pow(laserActive, 0.6f);
                soundLoop.Update();
                if (laserActive == 0f || (overseerGraphics.owner as Overseer).dead)
                {
                    if (soundLoop.emitter != null)
                    {
                        soundLoop.emitter.slatedForDeletetion = true;
                    }
                    soundLoop = null;
                }
            }
        }
        public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            sLeaser.sprites[firstSprite] = new CustomFSprite("Futile_White");
            if (!optionsInstance.DisableLaserShader.Value) { sLeaser.sprites[firstSprite].shader = rCam.game.rainWorld.Shaders["GateHologram"]; }
        }
        public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            Vector2 vector = Vector2.Lerp((overseerGraphics.owner as Overseer).mainBodyChunk.lastPos, (overseerGraphics.owner as Overseer).mainBodyChunk.pos, timeStacker);
            Vector2 vector2 = Custom.DirVec(Vector2.Lerp(lastLastLookAt, lastLookAt, timeStacker), vector);
            float lerpedLaserActive = Mathf.Lerp(lastLaserActive, laserActive, timeStacker);
            Color col = Color.Lerp(lastLaserColor, laserColor, timeStacker);
            float lerpedFlash = Mathf.Lerp(lastFlash, flash, timeStacker);
            if (lerpedLaserActive <= 0f)
            {
                sLeaser.sprites[firstSprite].isVisible = false;
            }
            else
            {
                sLeaser.sprites[firstSprite].isVisible = true;
                sLeaser.sprites[firstSprite].alpha = lerpedLaserActive;
                Vector2 corner = Custom.RectCollision(vector, vector - vector2 * 100000f, rCam.room.RoomRect.Grow(200f)).GetCorner(FloatRect.CornerLabel.D);
                IntVector2? intVector = SharedPhysics.RayTraceTilesForTerrainReturnFirstSolid(rCam.room, vector, corner);
                if (intVector.HasValue)
                {
                    corner = Custom.RectCollision(corner, vector, rCam.room.TileRect(intVector.Value)).GetCorner(FloatRect.CornerLabel.D);
                }
                (sLeaser.sprites[firstSprite] as CustomFSprite).verticeColors[0] = Custom.RGB2RGBA(col, lerpedLaserActive);
                (sLeaser.sprites[firstSprite] as CustomFSprite).verticeColors[1] = Custom.RGB2RGBA(col, lerpedLaserActive);
                (sLeaser.sprites[firstSprite] as CustomFSprite).verticeColors[2] = Custom.RGB2RGBA(col, Mathf.Pow(lerpedLaserActive, 2f) * Mathf.Lerp(0.5f, 1f, lerpedFlash));
                (sLeaser.sprites[firstSprite] as CustomFSprite).verticeColors[3] = Custom.RGB2RGBA(col, Mathf.Pow(lerpedLaserActive, 2f) * Mathf.Lerp(0.5f, 1f, lerpedFlash));
                (sLeaser.sprites[firstSprite] as CustomFSprite).MoveVertice(0, vector - vector2 * 4f + Custom.PerpendicularVector(vector2) * 0.5f - camPos);
                (sLeaser.sprites[firstSprite] as CustomFSprite).MoveVertice(1, vector - vector2 * 4f - Custom.PerpendicularVector(vector2) * 0.5f - camPos);
                (sLeaser.sprites[firstSprite] as CustomFSprite).MoveVertice(2, corner - Custom.PerpendicularVector(vector2) * 0.5f - camPos);
                (sLeaser.sprites[firstSprite] as CustomFSprite).MoveVertice(3, corner + Custom.PerpendicularVector(vector2) * 0.5f - camPos);
            }
        }
    }
}
