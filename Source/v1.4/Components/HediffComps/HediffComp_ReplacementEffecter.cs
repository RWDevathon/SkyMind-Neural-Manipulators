using Verse;
using RimWorld;

namespace SkyMind
{
    // This hediff comp will generate and apply a new pawn to this body when it is removed.
    public class HediffComp_ReplacementEffecter : HediffComp
    {
        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();

            Pawn newPawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(Pawn.kindDef, Pawn.Faction, PawnGenerationContext.NonPlayer, forceGenerateNewPawn: true, canGeneratePawnRelations: false, fixedBiologicalAge: Pawn.ageTracker.AgeBiologicalYearsFloat));
            SMN_Utils.Duplicate(newPawn, Pawn, true, false);
        }
    }
}
