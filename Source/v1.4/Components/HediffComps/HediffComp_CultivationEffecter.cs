using Verse;

namespace SkyMind
{
    // This hediff comp produces skill points idly.
    public class HediffComp_CultivationEffecter : HediffComp
    {
        public override void CompPostMake()
        {
            base.CompPostMake();

            connection = Pawn.GetComp<CompSkyMind>();
        }

        public override void CompExposeData()
        {
            base.CompExposeData();

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                connection = Pawn.GetComp<CompSkyMind>();
            }
        }

        // Only check every 6000 ticks (10 times a day) and only provides points if the pawn is connected.
        public override void CompPostTick(ref float severityAdjustment)
        {
            if (!Pawn.IsHashIntervalTick(6000) || !connection.connected)
            {
                return;
            }

            SMN_Utils.gameComp.ChangeServerPoints(1, SMN_ServerType.SkillServer);
        }

        CompSkyMind connection;
    }
}
