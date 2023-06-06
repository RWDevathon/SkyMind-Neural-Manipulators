using RimWorld;
using UnityEngine;
using Verse;

namespace SkyMind
{
    // This hediff comp modifies the pawn's Suppression need as long as they remain a slave of the colony. Can rarely give negative side effects.
    public class HediffComp_SuppressionEffecter : HediffComp
    {
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
        }

        // When the effect is removed, a longer-term but less significant thought is applied to represent enduring stress.
        public override void CompPostPostRemoved()
        {
            MemoryThoughtHandler thoughts = Pawn.needs?.mood?.thoughts?.memories;
            if (thoughts != null)
            {
                thoughts.TryGainMemory(SMNM_ThoughtDefOf.SMNM_PostTerrorStress);
            }
        }

        Need_Suppression suppression;
    }
}
