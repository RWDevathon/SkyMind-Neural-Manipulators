using RimWorld;

namespace SkyMind
{
    [DefOf]
    public static class SMNM_ThoughtDefOf
    {
        static SMNM_ThoughtDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(SMNM_ThoughtDefOf));
        }
        public static ThoughtDef SMNM_PostTerrorStress;
    }
}