using Verse;
using RimWorld;

namespace ATReforged
{
    // This hediff comp takes in a mental state from the comp properties and applies it to the pawn when made and removes it when the hediff comp is removed.
    public class HediffComp_MentalStateProtocol : HediffComp
    {
        HediffCompProperties_MentalStateProtocol Props => (HediffCompProperties_MentalStateProtocol)props;

        // Hediff applied, should force the mental state provided by properties to the pawn.
        public override void CompPostMake()
        {
            base.CompPostMake();

            if (!Pawn.mindState.mentalStateHandler.TryStartMentalState(Props.mentalState, forceWake: true, transitionSilently: true))
            {
                Log.Warning($"[ATR] {Pawn} failed to enter the {Props.mentalState.label} mental state as expected!");
            }
        }

        // Hediff removed, should end the mental state provided by properties to the pawn.
        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();

            if (Pawn.MentalStateDef == Props.mentalState)
            {
                if (Props.endingMayCauseConfusion && Rand.Chance(0.02f))
                {
                    Pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_Psychotic);
                }
                else
                {
                    Pawn.MentalState.RecoverFromState();
                }
            }
        }
    }
}
