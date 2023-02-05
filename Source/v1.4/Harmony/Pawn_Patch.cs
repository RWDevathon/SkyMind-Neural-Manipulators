using Verse;
using HarmonyLib;
using RimWorld;
using System.Collections.Generic;

namespace ATReforged
{
    // HediffComp gizmos are not called on pawns that in a mental state. This patch makes sure to still add it if the pawn is in our particular mental state.
    internal class Pawn_Patch
    {
        [HarmonyPatch(typeof(Pawn), "GetGizmos")]
        public static class GetGizmos_Patch
        {
            public static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> values, Pawn __instance)
            {
                foreach (Gizmo gizmo in values)
                {
                    yield return gizmo;
                }

                // Only yield the hediff gizmos if in the mental state, or else it'd be added twice.
                if (__instance.MentalStateDef == ATNM_MentalStateDefOf.ATNM_MentalState_Rampage)
                {
                    foreach (Gizmo gizmo in __instance.health.GetGizmos())
                    {
                        yield return gizmo;
                    }
                }
            }
        }
    }
}