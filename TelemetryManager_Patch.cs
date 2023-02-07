using Game;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace BagOfTricks
{
    /// <summary>
    /// Hijacks the Initialize() method from the TelemetryManager class and blocks it
    /// if the "Block Telemetry" option is active. If not, telemetry initializes normally.
    /// </summary>
    [HarmonyPatch(typeof(TelemetryManager), "Initialize", MethodType.Normal)]
    public static class TelemetryManager_Initialize_Patch
    {
        [HarmonyPrefix]
        static bool InitializePrefix(TelemetryManager __instance)
        {
            if (Storage.BlockTelemetry)
            {
                ModUI.mod.Logger.Log("Blocked Telemetry Initialization");
                return false;
            }
            return true;
        }
    }
}
