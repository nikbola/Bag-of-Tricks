using UnityEngine;

namespace BagOfTricks
{
    public static class KeyHandler
    {
        public static bool WaitingForInput = false;

        /// <summary>
        /// Listens for input to register if a key registration button has been pressed in the menu.
        /// If not, it checks if the given input is in the list of registered key binds.
        /// If it is, it performs the corresponding action.
        /// </summary>
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
                                ModUI.KillAllEnemiesKeyBindText = inputString.ToUpper();
                                break;
                            case Storage.KeyActions.ClearFog:
                                ModUI.ClearFogKeyBindText = inputString.ToUpper();
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

        /// <summary>
        /// Registers a key binding for a specific action.
        /// </summary>
        /// <param name="key">The given input as a string.</param>
        /// <param name="keyAction">The action to bind the key to.</param>
        /// <returns>True if binding was successful, otherwise false.</returns>
        public static bool RegisterKeyBinding(string key, Storage.KeyActions keyAction) 
        {
            if (key == "")
            {
                ModUI.mod.Logger.Log("The given key-binding is not valid.");
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
            ModUI.mod.Logger.Log($"KeyAction {keyAction} bound to \"{key}\" key.");
            return true;
        }

        /// <summary>
        /// Fetches the action corresponding to a given key.
        /// </summary>
        /// <param name="key">The string representation of a key input.</param>
        /// <returns>The action the given key corresponds to.</returns>
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
