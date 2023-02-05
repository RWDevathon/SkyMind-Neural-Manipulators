using Verse;

namespace ATReforged
{
    // This hediff comp takes in a hediff def and a chance to occur from comp properties, and tries to add that hediff to the pawn when created.
    // If properties indicates the original should be removed when the effect occurs, this comp will delete it.
    public class HediffComp_PotentialSideEffectOnApplied : HediffComp
    {
        HediffCompProperties_PotentialSideEffectOnApplied Props => (HediffCompProperties_PotentialSideEffectOnApplied)props;

        public override void CompPostMake()
        {
            base.CompPostMake();

            if (Rand.Chance(Props.chanceToOccur))
            {
                Pawn.health.AddHediff(Props.hediff);
                if (Props.removeOriginalIfEffectOccurs)
                {
                    Pawn.health.RemoveHediff(parent);
                }
            }
        }
    }
}
