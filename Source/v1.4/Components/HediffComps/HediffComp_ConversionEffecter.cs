using RimWorld;
using UnityEngine;
using Verse;

namespace ATReforged
{
    // This hediff comp reduces a pawn's resistance, will, and certainty (in that order) constantly. It does nothing if all three are at zero.
    public class HediffComp_ConversionEffecter : HediffComp
    {
        public override void CompPostMake()
        {
            base.CompPostMake();

            connection = Pawn.GetComp<CompSkyMind>();
            guestTracker = Pawn.guest;
            ideoTracker = Pawn.ideo;
        }

        public override void CompExposeData()
        {
            base.CompExposeData();

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                connection = Pawn.GetComp<CompSkyMind>();
                guestTracker = Pawn.guest;
                ideoTracker = Pawn.ideo;
            }
        }

        // Only check every 6000 ticks (10 times a day) and only reduce if the pawn is connected to an active SkyMind Core with available hacking points.
        public override void CompPostTick(ref float severityAdjustment)
        {
            if (!Pawn.IsHashIntervalTick(6000) || !connection.connected || Utils.gameComp.GetSkyMindCloudCapacity() == 0)
            {
                return;
            }

            float conversionCost = 50 / ((!Find.IdeoManager.classicMode && Pawn.Ideo != null) ? Pawn.GetStatValueForPawn(StatDefOf.CertaintyLossFactor, Pawn) : 1);
            if (Utils.gameComp.GetPoints(ServerType.HackingServer) < conversionCost)
            {
                return;
            }

            // 50 hacking points is worth 1 resistance.
            if (guestTracker.resistance > 0)
            {
                guestTracker.resistance = Mathf.Clamp(guestTracker.resistance - 1, 0, 999);
                Utils.gameComp.ChangeServerPoints(-conversionCost, ServerType.HackingServer);
            }
            // 50 hacking points is worth 1 will.
            else if (guestTracker.will > 0)
            {
                guestTracker.will = Mathf.Clamp(guestTracker.will - 1, 0, 999);
                Utils.gameComp.ChangeServerPoints(-conversionCost, ServerType.HackingServer);
            }
            // 50 hacking points is worth 2% certainty.
            else if (!Find.IdeoManager.classicMode && Pawn.Ideo != null && ideoTracker.Certainty > 0)
            {
                ideoTracker.OffsetCertainty(-0.02f);
                Utils.gameComp.ChangeServerPoints(-conversionCost, ServerType.HackingServer);
            }
        }

        CompSkyMind connection;
        Pawn_GuestTracker guestTracker;
        Pawn_IdeoTracker ideoTracker;
    }
}
