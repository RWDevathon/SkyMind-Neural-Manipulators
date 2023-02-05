using RimWorld;
using UnityEngine;
using Verse;

namespace ATReforged
{
    // This hediff comp modifies the pawn's Suppression need as long as they remain a slave of the colony. Can rarely give negative side effects.
    public class HediffComp_SuppressionEffecter : HediffComp
    {
        HediffCompProperties_SuppressionEffecter Props => (HediffCompProperties_SuppressionEffecter)props;

        public override void CompPostTick(ref float severityAdjustment)
        {
            if (!Pawn.IsSlaveOfColony)
            {
                return;
            }

            if (suppression == null)
            {
                suppression = Pawn.needs.TryGetNeed<Need_Suppression>();
            }

            suppression.CurLevel = Mathf.Clamp01(suppression.CurLevel + 0.005f);

            if (Rand.MTBEventOccurs(Props.meanTimeBeforeOccurring, 60000, 60))
            {
                if (!Utils.IsConsideredMechanical(Pawn))
                {
                    HediffGiverUtility.TryApply(Pawn, Props.organicHediff, Props.organicBodyPartsToAffect);
                }
                else
                {
                    HediffGiverUtility.TryApply(Pawn, Props.mechanicalHediff, Props.mechanicalBodyPartsToAffect);
                }
            }
        }

        // When the effect is removed, a longer-term but less significant thought is applied to represent enduring stress.
        public override void CompPostPostRemoved()
        {
            MemoryThoughtHandler thoughts = Pawn.needs?.mood?.thoughts?.memories;
            if (thoughts != null)
            {
                thoughts.TryGainMemory(ATNM_ThoughtDefOf.ATNM_PostTerrorStress);
            }
        }

        Need_Suppression suppression;
    }
}
