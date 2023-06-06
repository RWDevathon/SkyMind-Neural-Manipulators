using RimWorld;
using Verse;

namespace SkyMind
{
    [DefOf]
    public static class SMNM_HediffDefOf
    {
        static SMNM_HediffDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(SMNM_HediffDefOf));
        }
        public static HediffDef SMNM_RampageProtocol;

        public static HediffDef SMNM_StasisProtocol;

        public static HediffDef SMNM_TerrorProtocol;

        public static HediffDef SMNM_NumbnessProtocol;

        public static HediffDef SMNM_CultivationProtocol;

        public static HediffDef SMNM_ConversionProtocol;

        public static HediffDef SMNM_ReplacementProtocol;
    }
}