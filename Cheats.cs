using Game;
using System.Collections.Generic;
using UnityEngine;
using UnityModManagerNet;

namespace BagOfTricks
{
    public static class Cheats
    {
        public static void EnableGodMode(bool enable)
        {
            object[] arr = UnityEngine.Object.FindObjectsOfType(typeof(PartyMemberAI));
            foreach (PartyMemberAI partyMemberAI in arr)
            {
                Health component = partyMemberAI.GetComponent<Health>();
                if (component != null)
                {
                    component.TakesDamage = !enable;
                    if (enable)
                    {
                        PlayerSettings.mod.Logger.Log("God mode enabled.");
                    }
                    else
                    {
                        PlayerSettings.mod.Logger.Log("God mode disabled.");
                    }
                }
                else
                {
                    PlayerSettings.mod.Logger.Error("Health component could not be found!");
                }
            }
        }

        public static void EnableInvisibility(bool enable)
        {
            object[] array = UnityEngine.Object.FindObjectsOfType(typeof(PartyMemberAI));
            foreach (PartyMemberAI partyMemberAI in array)
            {
                Health component = partyMemberAI.GetComponent<Health>();
                if (component)
                {
                    component.Targetable = !enable;
                    if (enable)
                    {
                        PlayerSettings.mod.Logger.Log("Invisibility Enabled.");
                    }
                    else
                    {
                        PlayerSettings.mod.Logger.Log("Invisibility Disabled.");
                    }
                }
            }
        }

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

        public static void AddCurrency(int amount)
        {
            Player s_playerCharacter = GameState.s_playerCharacter;
            if (s_playerCharacter == null)
            {
                PlayerSettings.mod.Logger.Error("Could not add currency. Player not found.");
                return;
            }
            PlayerInventory component = s_playerCharacter.GetComponent<PlayerInventory>();
            if (component == null)
            {
                PlayerSettings.mod.Logger.Error("Could not add currency. PlayerInventory not found.");
                return;
            }
            component.currencyTotalValue.amount += amount;
        }

        public static void ClearFogOfWar()
        {
            if (FogOfWar.Instance != null)
            {
                FogOfWar.Instance.QueueDisable();
            }
            PlayerSettings.mod.Logger.Log("Fog of War Cleared");
        }
    }
}
