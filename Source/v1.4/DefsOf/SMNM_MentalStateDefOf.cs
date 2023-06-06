using RimWorld;
using Verse;

namespace SkyMind
{
    [DefOf]
    public static class SMNM_MentalStateDefOf
    {
        static SMNM_MentalStateDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(SMNM_MentalStateDefOf));
        }
        public static MentalStateDef SMNM_MentalState_Rampage;
    }
}