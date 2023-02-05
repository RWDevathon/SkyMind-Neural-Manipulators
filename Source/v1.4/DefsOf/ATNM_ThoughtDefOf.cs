using RimWorld;

namespace ATReforged
{
    [DefOf]
    public static class ATNM_ThoughtDefOf
    {
        static ATNM_ThoughtDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ATNM_ThoughtDefOf));
        }
        public static ThoughtDef ATNM_PostTerrorStress;
    }
}