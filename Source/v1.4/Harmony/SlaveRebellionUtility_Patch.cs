using Verse;
using HarmonyLib;
using RimWorld;

namespace SkyMind
{
    // Slaves currently being terrorized can not participate in slave rebellions.
    internal class SlaveRebellionUtility_Patch
    {
        [HarmonyPatch(typeof(SlaveRebellionUtility), "CanParticipateInSlaveRebellion")]
        public class CanParticipateInSlaveRebellion_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn pawn, ref bool __result)
            {
                if (!__result)
                {
                    return;
                }

                if (pawn.health.hediffSet.HasHediff(SMNM_HediffDefOf.SMNM_TerrorProtocol))
                {
                    __result = false;
                }
            }
        }
    }
}