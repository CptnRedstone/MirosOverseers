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
    //Sync explosions?
    //Lights are getting left behind...
    //Sound loop discard issue
    //Is onlineCreature ever not OnlineCreature?
    public delegate RealizedCreatureState orig_GetRealizedState(
        AbstractCreatureState self,
        OnlineCreature onlineEntity
    );

    public static readonly MirosOverseers modInstance;

    public static void ApplyHooks() //Thanks for the help UO!
    {
        new Hook(
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
    }

    public static RealizedCreatureState GetRealizedState_Hook(
        orig_GetRealizedState orig,
        AbstractCreatureState self,
        OnlineCreature onlineCreature
    )
    {
        if (
            onlineCreature is OnlineCreature oc //TODO is this *ever* false?
            && oc.abstractCreature.creatureTemplate.type == CreatureTemplate.Type.Overseer
        )
        {
            return new OnlineOverseerData(oc);
        }

        return orig(self, onlineCreature);
    }

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
