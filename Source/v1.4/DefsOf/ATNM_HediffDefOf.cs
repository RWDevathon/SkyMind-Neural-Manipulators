using RimWorld;
using Verse;

namespace ATReforged
{
    [DefOf]
    public static class ATNM_HediffDefOf
    {
        static ATNM_HediffDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ATNM_HediffDefOf));
        }
        public static HediffDef ATNM_RampageProtocol;

        public static HediffDef ATNM_StasisProtocol;

        public static HediffDef ATNM_TerrorProtocol;

        public static HediffDef ATNM_NumbnessProtocol;

        public static HediffDef ATNM_CultivationProtocol;

        public static HediffDef ATNM_ConversionProtocol;

        public static HediffDef ATNM_ReplacementProtocol;
    }
}