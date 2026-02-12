using JetBrains.Annotations;
using RainMeadow;
using System;
using System.Runtime.CompilerServices;

namespace MirosOverseers;

public class MeadowCompat
{
    public readonly MirosOverseers modInstance;
    public MeadowCompat(MirosOverseers modInstance)
    {
        this.modInstance = modInstance;
        On.Overseer.ctor += OnOverseerCtor;
    }
    public void OnOverseerCtor(On.Overseer.orig_ctor orig, Overseer self, AbstractCreature abstractCreature, World world)
    {
        orig(self, abstractCreature, world);
        //abstractCreature.GetOnlineCreature().AddData(new OnlineOverseerData());
    }


    public static ConditionalWeakTable<OnlineCreature, OnlineOverseerWrapper> OnlineOverseerCwt = new();
    public static ConditionalWeakTable<OnlineCreature, OnlineOverseerGraphicsWrapper> OnlineOverseerGraphicsCwt = new();
    public class OnlineOverseerWrapper
    {
        public int laserCounter;
        public LightSource laserLight;
    }
    public class OnlineOverseerGraphicsWrapper
    {
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
                LaserTimer = MirosOverseers.GetOverseerLaserCounter((onlineEntity as OnlineCreature).realizedCreature as Overseer);
            }
            public override void ReadTo(OnlineEntity.EntityData data, OnlineEntity onlineEntity)
            {
                if ((onlineEntity as OnlineCreature)?.apo is AbstractCreature abstractCreature)
                {
                    MirosOverseers.SetOverseerLaserCounter((onlineEntity as OnlineCreature).realizedCreature as Overseer, LaserTimer);
                }
            }
            public override Type GetDataType()
            {
                return typeof(OnlineOverseerData);
            }
        }
    }
}