using HarmonyLib;

namespace BagOfTricks
{
    [HarmonyPatch(typeof(Mover), "Update", MethodType.Normal)]
    public class Mover_Patch
    {
        [HarmonyPrefix]
        static void UpdatePrefix(Mover __instance)
        {
            string moverName = __instance.gameObject.name;
            if (!moverName.CustomStartsWith("Player") && !moverName.CustomStartsWith("Comp"))
            {
                return;
            }

            __instance.SetRunSpeed(Storage.RunSpeed);
            __instance.SetWalkSpeed(Storage.WalkSpeed);
            __instance.StealthSpeed = Storage.StealthSpeed;
        }
    }
}
