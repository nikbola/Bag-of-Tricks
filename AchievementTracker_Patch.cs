/*using HarmonyLib;
using System.Collections.Generic;

namespace BagOfTricks
{
    [HarmonyPatch(typeof(AchievementTracker), "GetAchievementDebugOutput", MethodType.Normal)]
    public static class AchievementTracker_Patch
    {
        static AccessTools.FieldRef<AchievementTracker, List<string>> m_completedAchievementRef =
            AccessTools.FieldRefAccess<AchievementTracker, List<string>>("m_completedAchievements");

        static AchievementTracker Instance;

        [HarmonyPrefix]
        static bool GetAchievementDebugOutput_Prefix(AchievementTracker __instance)
        {
            Instance = __instance;
            return false;
        }

        [HarmonyPostfix]
        static void GetAchievementDebugOutput_Postfix(ref string __result)
        {
            string text = "";
            foreach (string str in m_completedAchievementRef(Instance))
            {
                text = text + str + ",";
            }
            __result = text;
        }
    }
}*/
