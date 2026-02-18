using System;
using System.Reflection;
using Menu;
using MonoMod.RuntimeDetour;
using RainMeadow;
using UnityEngine;

namespace MirosOverseers;

public static class MeadowCompat
{
    //----------Bonus todo yay my favorite----------
    //Sync remix settings
    //Sync the laser timer
    //Sync explosions?
    //Sync look direction??
    //Static class thingy
    //Breaks in singleplayer with meadow
    //Laser light stops updating if pointed into the sky
    public delegate RealizedPhysicalObjectState orig_GetRealizedState(
        AbstractPhysicalObjectState self,
        OnlinePhysicalObject onlineEntity
    );

    public static readonly MirosOverseers modInstance;

    public static void ApplyHooks()
    {
        new Hook(
            typeof(AbstractPhysicalObjectState).GetMethod(
                "GetRealizedState",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            ),
            new Func<
                orig_GetRealizedState,
                AbstractPhysicalObjectState,
                OnlinePhysicalObject,
                RealizedPhysicalObjectState
            >(GetRealizedState_Hook)
        );
    }

    public static RealizedPhysicalObjectState GetRealizedState_Hook(
        orig_GetRealizedState orig,
        AbstractPhysicalObjectState self,
        OnlinePhysicalObject onlineEntity
    )
    {
        if (
            onlineEntity is OnlineCreature oc
            && oc.abstractCreature.creatureTemplate.type == CreatureTemplate.Type.Overseer
        )
        {
            return new OnlineOverseerData(oc);
        }

        return orig(self, onlineEntity);
    }

    //public static void OnOverseerCtor(On.Overseer.orig_ctor orig, Overseer self, AbstractCreature abstractCreature, World world)
    //{
    //    orig(self, abstractCreature, world);
    //    abstractCreature.GetOnlineCreature().AddData(new OnlineOverseerData());
    //}

    //public static ConditionalWeakTable<OnlineCreature, OnlineOverseerWrapper> OnlineOverseerCwt = new();
    public class OnlineOverseerWrapper
    {
        public int laserCounter;
        public LightSource laserLight;
        public MirosOverseers.OverseerMirosLaser laserBeam;
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

            if ((onlineEntity as OnlineCreature)?.realizedCreature is Overseer overseer)
            {
                MirosOverseers.SetOverseerLaserCounter(overseer, LaserTimer);
            }
        }
    }
}
