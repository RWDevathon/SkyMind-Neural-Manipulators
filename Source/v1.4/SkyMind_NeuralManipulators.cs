using HarmonyLib;
using System.Reflection;
using Verse;

namespace SkyMind
{
    public class SkyMind_NeuralManipulators : Mod
    {
        public SkyMind_NeuralManipulators(ModContentPack content) : base(content)
        {
            new Harmony("SMNeuralManipulators").PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}