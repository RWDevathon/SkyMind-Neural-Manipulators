using Verse;
using UnityEngine;

namespace ATReforged
{
    [StaticConstructorOnStartup]
    static class Tex
    {
        static Tex()
        {
        }

        // Gizmos
        public static readonly Texture2D ManipulationIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gizmos/ManipulationIcon");
    }
}