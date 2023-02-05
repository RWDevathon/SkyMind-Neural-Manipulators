using RimWorld;
using Verse;

namespace ATReforged
{
    [DefOf]
    public static class ATNM_MentalStateDefOf
    {
        static ATNM_MentalStateDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ATNM_MentalStateDefOf));
        }
        public static MentalStateDef ATNM_MentalState_Rampage;
    }
}