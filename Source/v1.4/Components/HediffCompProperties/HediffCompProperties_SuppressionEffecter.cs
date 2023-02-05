using System.Collections.Generic;
using Verse;

namespace ATReforged
{
    public class HediffCompProperties_SuppressionEffecter : HediffCompProperties
    {
        public HediffCompProperties_SuppressionEffecter()
        {
            compClass = typeof(HediffComp_SuppressionEffecter);
        }

        public HediffDef organicHediff;
        public List<BodyPartDef> organicBodyPartsToAffect;

        public HediffDef mechanicalHediff;
        public List<BodyPartDef> mechanicalBodyPartsToAffect;

        public float meanTimeBeforeOccurring = 2500;
    }
}
