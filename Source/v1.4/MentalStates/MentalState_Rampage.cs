using RimWorld;
using Verse;
using Verse.AI;

namespace ATReforged
{
    // Attack anything that isn't also rampaging.
    public class MentalState_Rampage : MentalState
    { 
        public override bool ForceHostileTo(Thing t)
        {
            if (t is Pawn pawn && pawn.MentalStateDef == ATNM_MentalStateDefOf.ATNM_MentalState_Rampage)
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