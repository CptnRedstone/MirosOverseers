using JetBrains.Annotations;
using Menu;
using MonoMod.RuntimeDetour;
using RainMeadow;
using System;
using System.Reflection;
using UnityEngine;

namespace MirosOverseers;

public static class MeadowCompat
{
    //----------Bonus todo yay my favorite----------
    //Sync remix settings
    //Sync explosions?
    //Lights are getting left behind...
    //Sound loop discard issue
    //Is onlineCreature ever not OnlineCreature?
    public delegate RealizedCreatureState orig_GetRealizedState(
        AbstractCreatureState self,
        OnlineCreature onlineEntity
    );

    public static void InitiateMeadowCompat()
    {
        new Hook( //Thanks for the help UO!
            typeof(AbstractCreatureState).GetMethod(
                "GetRealizedState",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            ),
            new Func<
                orig_GetRealizedState,
                AbstractCreatureState,
                OnlineCreature,
                RealizedCreatureState
            >(GetRealizedState_Hook)
        );

        OnlineResource.OnAvailable += OnlineResource_OnAvailable;
    }

    public static RealizedCreatureState GetRealizedState_Hook(
        orig_GetRealizedState orig,
        AbstractCreatureState self,
        OnlineCreature onlineCreature
    )
    {
        if (!(onlineCreature is OnlineCreature)) { MirosOverseers.LogWarning("IF YOU SEE THIS TELL REDSTONE THE SANITY CHECK IS IMPORTANT"); }
        if (
            onlineCreature is OnlineCreature oc //TODO is this *ever* false?
            && oc.abstractCreature.creatureTemplate.type == CreatureTemplate.Type.Overseer
        )
        {
            return new OnlineOverseerData(oc);
        }

        return orig(self, onlineCreature);
    }

    public static void OnlineResource_OnAvailable(OnlineResource obj)
    {
        obj.AddData(new MeadowInputData());
    }
}




public class OnlineOverseerData : RealizedOverseerState
{
    [OnlineField]
    public int LaserTimer;

    public OnlineOverseerData() { }

    public OnlineOverseerData(OnlineCreature entity)
        : base(entity)
    {
        if (entity.realizedCreature is Overseer overseer)
        {
            if (MirosOverseers.OverseerCwt.TryGetValue(overseer, out _))
            {
                this.LaserTimer = MirosOverseers.GetOverseerLaserCounter(overseer);
            }
        }
    }

    public override void ReadTo(OnlineEntity onlineEntity)
    {
        base.ReadTo(onlineEntity);

        if (onlineEntity is OnlineCreature { realizedCreature: Overseer overseer })
        {
            MirosOverseers.SetOverseerLaserCounter(overseer, LaserTimer);
        }
    }
}




public class MeadowInputData : OnlineResource.ResourceData
{
    public MeadowInputData() { }
    
    public override ResourceDataState MakeState(OnlineResource inResource)
    {
        return new State();
    }

    public class State : ResourceDataState
    {
        [OnlineField]
        public float overseerReaimCooldown;
        [OnlineField]
        public float overseerFiringCooldown;
        [OnlineField]
        public float overseerAimingDuration;
        [OnlineField]
        public bool artificerVulnerability;
        [OnlineField]
        public bool overseersIgnoreRain;
        [OnlineField]
        public bool overseersOverseerImmune;
        [OnlineField]
        public bool overseersSpearImmune;
        [OnlineField]
        public bool overseersExplosionImmune;
        [OnlineField]
        public bool overseersImmortal;
        [OnlineField]
        public bool disableDuringCutscenes;
        [OnlineField]
        public bool disableDuringForcedInput;
        [OnlineField]
        public bool disableNearPuppets;
        [OnlineField]
        public bool disableDuringDialogue;

        public State()
        {
            overseerReaimCooldown = MirosOverseers.optionsInstance.OverseerReaimCooldown.Value;
            overseerFiringCooldown = MirosOverseers.optionsInstance.OverseerFiringCooldown.Value;
            overseerAimingDuration = MirosOverseers.optionsInstance.OverseerAimingDuration.Value;
            artificerVulnerability = MirosOverseers.optionsInstance.ArtificerVulnerability.Value;
            overseersIgnoreRain = MirosOverseers.optionsInstance.OverseersIgnoreRain.Value;
            overseersOverseerImmune = MirosOverseers.optionsInstance.OverseersOverseerImmune.Value;
            overseersSpearImmune = MirosOverseers.optionsInstance.OverseersSpearImmune.Value;
            overseersExplosionImmune = MirosOverseers.optionsInstance.OverseersExplosionImmune.Value;
            overseersImmortal = MirosOverseers.optionsInstance.OverseersImmortal.Value;
            disableDuringCutscenes = MirosOverseers.optionsInstance.DisableDuringCutscenes.Value;
            disableDuringForcedInput = MirosOverseers.optionsInstance.DisableDuringForcedInput.Value;
            disableNearPuppets = MirosOverseers.optionsInstance.DisableNearPuppets.Value;
            disableDuringDialogue = MirosOverseers.optionsInstance.DisableDuringDialogue.Value;
        }

        public override void ReadTo(OnlineResource.ResourceData data, OnlineResource resource)
        {
            MirosOverseers.optionsInstance.OverseerReaimCooldown.Value = overseerReaimCooldown;
            MirosOverseers.optionsInstance.OverseerFiringCooldown.Value = overseerFiringCooldown;
            MirosOverseers.optionsInstance.OverseerAimingDuration.Value = overseerAimingDuration;
            MirosOverseers.optionsInstance.ArtificerVulnerability.Value = artificerVulnerability;
            MirosOverseers.optionsInstance.OverseersIgnoreRain.Value = overseersIgnoreRain;
            MirosOverseers.optionsInstance.OverseersOverseerImmune.Value = overseersOverseerImmune;
            MirosOverseers.optionsInstance.OverseersSpearImmune.Value = overseersSpearImmune;
            MirosOverseers.optionsInstance.OverseersExplosionImmune.Value = overseersExplosionImmune;
            MirosOverseers.optionsInstance.OverseersImmortal.Value = overseersImmortal;
            MirosOverseers.optionsInstance.DisableDuringCutscenes.Value = disableDuringCutscenes;
            MirosOverseers.optionsInstance.DisableDuringForcedInput.Value = disableDuringForcedInput;
            MirosOverseers.optionsInstance.DisableNearPuppets.Value = disableNearPuppets;
            MirosOverseers.optionsInstance.DisableDuringDialogue.Value = disableDuringDialogue;
        }

        public override Type GetDataType()
        {
            return typeof(MeadowInputData);
        }
    }
}