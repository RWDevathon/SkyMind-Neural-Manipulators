using Verse;

namespace SkyMind
{
    public class HediffCompProperties_PotentialSideEffectOnApplied : HediffCompProperties
    {
        public HediffCompProperties_PotentialSideEffectOnApplied()
        {
            compClass = typeof(HediffComp_PotentialSideEffectOnApplied);

        }

        public HediffDef hediff;

        public float chanceToOccur;

        public bool removeOriginalIfEffectOccurs;
    }
}
