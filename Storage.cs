using System.Collections.Generic;

namespace BagOfTricks
{
    public static class Storage
    {
        // PLAYER MOVEMENT
        public static float RunSpeed = 8f;
        public static float WalkSpeed = 4f;
        public static float StealthSpeed = 2.5f;

        public static object[] PartyMemberAIs;
        public static object[] Factions;

        public static bool GodModeEnabled = false;
        public static bool InvisibilityEnabled = false;
        public static bool BlockTelemetry = true;

        public static Dictionary<string, KeyActions> KeyBindings = new Dictionary<string, KeyActions>();
        public static KeyActions CurrentKeyAction;

        public enum KeyActions
        {
            None,
            KillAllEnemies,
            ClearFog
        }

        public static void ResetData()
        {
            PartyMemberAIs = null;
            Factions = null;
        }
    }
}
