using Game;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace BagOfTricks
{
    [HarmonyPatch(typeof(TelemetryManager), "Initialize", MethodType.Normal)]
    public static class TelemetryManager_Initialize_Patch
    {
        [HarmonyPrefix]
        static bool InitializePrefix(TelemetryManager __instance)
        {
            if (Storage.BlockTelemetry)
            {
                PlayerSettings.mod.Logger.Log("Blocked Telemetry Initialization");
                return false;
            }
            return true;
        }
    }
}
