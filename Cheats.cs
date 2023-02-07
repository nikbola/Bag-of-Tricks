using Game;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityModManagerNet;
using static UnityModManagerNet.UnityModManager.Param;

namespace BagOfTricks
{
    /// <summary>
    /// All of these cheats use the same code as a corresponding console command, 
    /// but they forego the required "iroll20s" command and thus do not disable achievements.
    /// </summary>
    public static class Cheats
    {
        /// <summary>
        /// Subscribe to UI events
        /// </summary>
        public static void InitializeEventSubscriptions()
        {
            ModUI.OnKillEnemiesPressed += KillAllEnemies;
            ModUI.OnGodModeToggled += ToggleGodMode;
            ModUI.OnClearFogOfWarPressed += ClearFogOfWar;
            ModUI.OnInvisibilityToggled += EnableInvisibility;
            ModUI.OnAddCurrency += AddCurrency;
        }

        /// <summary>
        /// Enables/disables god mode.
        /// </summary>
        /// <param name="enable">Whether to enable or disable.</param>
        public static void ToggleGodMode(bool enabled)
        {
            object[] arr = UnityEngine.Object.FindObjectsOfType(typeof(PartyMemberAI));
            foreach (PartyMemberAI partyMemberAI in arr)
            {
                Health component = partyMemberAI.GetComponent<Health>();
                if (component != null)
                {
                    component.TakesDamage = !enabled;
                }
                else
                {
                    ModUI.mod.Logger.Error("Health component could not be found on object \"" + partyMemberAI.gameObject.name + "\"!");
                    return;
                }
            }
            if (enabled)
            {
                ModUI.mod.Logger.Log("God mode enabled.");
            }
            else
            {
                ModUI.mod.Logger.Log("God mode disabled.");
            }
        }

        /// <summary>
        /// Enable invisibility. I.e., make player untargetable by enemies.
        /// </summary>
        /// <param name="enable">Whether to enable or disable.</param>
        public static void EnableInvisibility(bool enable)
        {
            object[] array = UnityEngine.Object.FindObjectsOfType(typeof(PartyMemberAI));
            foreach (PartyMemberAI partyMemberAI in array)
            {
                Health component = partyMemberAI.GetComponent<Health>();
                if (component)
                {
                    component.Targetable = !enable;
                    
                }
                else
                {
                    ModUI.mod.Logger.Log("Could not find Health component on object \"" + partyMemberAI.gameObject.name + "\"!.");
                    return;
                }
            }
            if (enable)
            {
                ModUI.mod.Logger.Log("Invisibility Enabled.");
            }
            else
            {
                ModUI.mod.Logger.Log("Invisibility Disabled.");
            }
        }

        /// <summary>
        /// Kills all enemies in the vicinity.
        /// </summary>
        public static void KillAllEnemies()
        {
            if (!GameState.s_playerCharacter)
            {
                return;
            }
            Faction component = GameState.s_playerCharacter.GetComponent<Faction>();
            object[] arr = UnityEngine.Object.FindObjectsOfType(typeof(Faction));
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < arr.Length; i++)
            {
                Faction faction = arr[i] as Faction;
                if (faction.IsHostile(component))
                {
                    list.Add(faction.gameObject);
                }
            }
            for (int j = 0; j < list.Count; j++)
            {
                Health component2 = list[j].GetComponent<Health>();
                component2.KillCheat();
            }
        }

        /// <summary>
        /// Adds currency of the specified amount.
        /// </summary>
        /// <param name="amount">The amount of currency to add.</param>
        public static void AddCurrency(int amount)
        {
            Player s_playerCharacter = GameState.s_playerCharacter;
            if (s_playerCharacter == null)
            {
                ModUI.mod.Logger.Error("Could not add currency. Player not found.");
                return;
            }
            PlayerInventory component = s_playerCharacter.GetComponent<PlayerInventory>();
            if (component == null)
            {
                ModUI.mod.Logger.Error("Could not add currency. PlayerInventory not found.");
                return;
            }
            component.currencyTotalValue.amount += amount;
        }

        /// <summary>
        /// Removes the fog of war from the current map.
        /// </summary>
        public static void ClearFogOfWar()
        {
            if (FogOfWar.Instance != null)
            {
                FogOfWar.Instance.QueueDisable();
            }
            ModUI.mod.Logger.Log("Fog of War Cleared");
        }
    }
}
