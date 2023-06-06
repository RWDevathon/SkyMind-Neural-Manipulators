using Verse;

namespace SkyMind
{
    public class HediffCompProperties_MentalStateProtocol : HediffCompProperties
    {
        public HediffCompProperties_MentalStateProtocol()
        {
            compClass = typeof(HediffComp_MentalStateProtocol);

        }

        public MentalStateDef mentalState;

        public bool endingMayCauseConfusion;
    }
}
