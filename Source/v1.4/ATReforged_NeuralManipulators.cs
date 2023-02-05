using HarmonyLib;
using System.Reflection;
using Verse;

namespace ATReforged
{
    public class ATReforged_NeuralManipulators : Mod
    {
        public ATReforged_NeuralManipulators(ModContentPack content) : base(content)
        {
            new Harmony("ATNeuralManipulators").PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}