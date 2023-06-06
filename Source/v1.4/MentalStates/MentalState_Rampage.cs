using RimWorld;
using Verse;
using Verse.AI;

namespace SkyMind
{
    // Attack anything that isn't also rampaging.
    public class MentalState_Rampage : MentalState
    { 
        public override bool ForceHostileTo(Thing t)
        {
            if (t is Pawn pawn && pawn.MentalStateDef == SMNM_MentalStateDefOf.SMNM_MentalState_Rampage)
            {
                return false;
            }
            return true;
        }

        public override bool ForceHostileTo(Faction f)
        {
            return true;
        }

        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Off;
        }
    }
}