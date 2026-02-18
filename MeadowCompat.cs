using JetBrains.Annotations;
using RainMeadow;
using System;

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

    public static readonly MirosOverseers modInstance;
    public static void ApplyHooks()
    {
        On.Overseer.ctor += delegate(On.Overseer.orig_ctor orig, Overseer self, AbstractCreature abstractCreature, World world)
        {
            orig(self, abstractCreature, world);

            if (OnlineManager.lobby != null)
            {
                abstractCreature.GetOnlineCreature().AddData(new OnlineOverseerData());
            }
        };
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


    public class OnlineOverseerData : OnlineEntity.EntityData
    {
        [UsedImplicitly]
        public OnlineOverseerData() { }
        public override EntityDataState MakeState(OnlineEntity entity, OnlineResource inResource)
        {
            return new State(entity);
        }
        public class State : EntityDataState
        {
            [OnlineField]
            public int LaserTimer;

            [UsedImplicitly]
            public State() { }
            public State(OnlineEntity onlineEntity)
            {

                //if ((onlineEntity as OnlineCreature)?.apo is AbstractCreature abstractCreature && abstractCreature != null && abstractCreature.realizedCreature != null)
                //{
                //    if (MirosOverseers.OverseerCwt.TryGetValue(abstractCreature.realizedCreature as Overseer, out _))
                //    {
                //        LaserTimer = MirosOverseers.GetOverseerLaserCounter(abstractCreature.realizedCreature as Overseer);
                //    }
                //}   
            }
            public override void ReadTo(OnlineEntity.EntityData data, OnlineEntity onlineEntity)
            {
                //if ((onlineEntity as OnlineCreature)?.apo is AbstractCreature abstractCreature && abstractCreature != null && abstractCreature.realizedCreature != null)
                //{
                //    MirosOverseers.SetOverseerLaserCounter(abstractCreature.realizedCreature as Overseer, LaserTimer);
                //}
            }
            public override Type GetDataType()
            {
                return typeof(OnlineOverseerData);
            }
        }
    }
}