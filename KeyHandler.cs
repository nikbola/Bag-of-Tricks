using UnityEngine;

namespace BagOfTricks
{
    public static class KeyHandler
    {
        public static bool WaitingForInput = false;

        public static void HandleInput()
        {
            if (WaitingForInput)
            {
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Escape))
                {
                    WaitingForInput = false;
                    return;
                }
                if (Input.anyKeyDown)
                {
                    string inputString = Input.inputString;
                    if (RegisterKeyBinding(inputString, Storage.CurrentKeyAction))
                    {
                        switch (Storage.CurrentKeyAction)
                        {
                            case Storage.KeyActions.KillAllEnemies:
                                PlayerSettings.KillAllEnemiesKeyBindText = inputString.ToUpper();
                                break;
                            case Storage.KeyActions.ClearFog:
                                PlayerSettings.ClearFogKeyBindText = inputString.ToUpper();
                                break;
                            default:
                                break;
                        }
                    }
                    WaitingForInput = false;
                }
            }
            else if (Input.anyKeyDown)
            {
                Storage.KeyActions keyActions = KeyHandler.GetKeyAction(Input.inputString);
                if (keyActions != Storage.KeyActions.None)
                {
                    switch (keyActions)
                    {
                        case Storage.KeyActions.KillAllEnemies:
                            Cheats.KillAllEnemies();
                            break;
                        case Storage.KeyActions.ClearFog:
                            Cheats.ClearFogOfWar();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public static bool RegisterKeyBinding(string key, Storage.KeyActions keyAction) 
        {
            if (key == "")
            {
                PlayerSettings.mod.Logger.Log("The given key-binding is not valid.");
                return false;
            }
            string oldBinding = "";
            foreach (var binding in Storage.KeyBindings)
            {
                if (binding.Value == keyAction)
                {
                    oldBinding = binding.Key;
                }
            }
            if (oldBinding != "")
            {
                Storage.KeyBindings.Remove(oldBinding);
            }
            Storage.KeyBindings.Add(key, keyAction);
            PlayerSettings.mod.Logger.Log($"KeyAction {keyAction} bound to \"{key}\" key.");
            return true;
        }

        public static Storage.KeyActions GetKeyAction(string key)
        {
            if (Storage.KeyBindings.TryGetValue(key, out Storage.KeyActions keyAction))
            {
                return keyAction;
            }
            else
            {
                return Storage.KeyActions.None;
            }
        }
    }
}
