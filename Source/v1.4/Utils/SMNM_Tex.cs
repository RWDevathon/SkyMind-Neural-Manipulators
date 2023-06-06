using Verse;
using UnityEngine;

namespace SkyMind
{
    [StaticConstructorOnStartup]
    static class SMNM_Tex
    {
        static SMNM_Tex()
        {
        }

        // Gizmos
        public static readonly Texture2D ManipulationIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gizmos/ManipulationIcon");
    }
}