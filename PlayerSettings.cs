using HarmonyLib;
using System.Reflection;
using UnityModManagerNet;
using UnityEngine;
using System.Globalization;
using UnityEngine.SceneManagement;
using Game;
using System;
using System.Collections.Generic;

namespace BagOfTricks
{
    public class PlayerSettings
    {
        public static UnityModManager.ModEntry mod;

        public static string KillAllEnemiesKeyBindText = "Key";
        public static string ClearFogKeyBindText = "Key";

        private static float RunSpeed = 8f;
        private static float WalkSpeed = 4f;
        private static float StealthSpeed = 2.5f;

        private static bool CheatsExpanded = false;
        private static bool MovementExpanded = false;
        private static bool AchievementsExpanded = false;
        private static bool StatsExpanded = false;
        private static bool PreviousGodModeValue = false;
        private static bool PreviousInvisibilityValue = false;

        private static readonly float DefaultRunSpeed = 8f;
        private static readonly float DefaultWalkSpeed = 4f;
        private static readonly float DefaultStealthSpeed = 2.5f;

        private static readonly float DefaultLabelWidth = 100f;
        private static readonly float DefaultCheatsLabelWidth = 300f;
        private static readonly float DefaultTextFieldWidth = 75f;
        private static readonly float DefaultButtonWidth = 75f;

        private static readonly float SpaceBetweenHeaders = 15f;

        private static readonly float SliderMin = 0f;
        private static readonly float SliderMax = 25f;

        private static string InputFieldText = "";

        private static int CurrencyToAdd = 1000;

        private static GUIStyle AchievementBackgroundStyle = new GUIStyle();
        private static GUIStyle AchievementHoverBackgroundStyle = new GUIStyle();

        private static int achievementHoverIndex = -1;

        private static List<Tuple<string, string>> achievementInfo = new List<Tuple<string, string>>();

        private static object[] partyMemberAIs;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            modEntry.OnGUI = OnGUI;
            modEntry.OnUpdate = OnUpdate;

            mod = modEntry;

            SceneManager.activeSceneChanged += ActiveSceneChanged;

            AchievementBackgroundStyle.normal.textColor = Color.white;
            AchievementHoverBackgroundStyle.normal.textColor = Color.green;

            modEntry.OnShowGUI = OnShowGUI;

            return true;
        }

        static void OnShowGUI(UnityModManager.ModEntry modEntry)
        {
            if (AchievementTracker.Instance != null)
            {
                achievementInfo = new List<Tuple<string, string>>();
                foreach (var achievement in AchievementTracker.Instance.Achievements)
                {
                    achievementInfo.Add(new Tuple<string, string>(achievement.AchievementName, achievement.AchievementAPIName));
                }
            }

            partyMemberAIs = Stats.GetPartyMembers();
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.ExpandWidth(false);
            GUILayout.Space(SpaceBetweenHeaders);
            DrawCheatSettings();
            GUILayout.Space(SpaceBetweenHeaders);
            DrawStatsSettings();
            GUILayout.Space(SpaceBetweenHeaders);
            DrawMovementSettings();
            GUILayout.Space(SpaceBetweenHeaders);
            DrawAchievementSettings();
        }

        static void OnUpdate(UnityModManager.ModEntry modEntry, float deltaTime)
        {
            KeyHandler.HandleInput();   
        }

        static void ActiveSceneChanged(Scene current, Scene next)
        {
            Storage.ResetData();
        }

        #region Cheat Settings
        public static void DrawCheatSettings()
        {
            DrawHeader("Cheats", ref CheatsExpanded);
            if (CheatsExpanded)
            {
                GUILayout.Space(5f);

                // BLOCK TELEMETRY TOGGLE
                GUILayout.BeginHorizontal();
                GUILayout.Label("Block Telemetry (Requires Restart)", GUILayout.Width(DefaultCheatsLabelWidth));
                GUILayout.Toggle(Storage.BlockTelemetry, string.Empty, GUILayout.Width(190f));

                // KILL ALL ENEMIES BUTTON
                if (GUILayout.Button("Kill All Enemies", GUILayout.Width(150f)))
                {
                    Cheats.KillAllEnemies();
                }
                if (GUILayout.Button(KillAllEnemiesKeyBindText, GUILayout.Width(DefaultButtonWidth)))
                {
                    if (!KeyHandler.WaitingForInput)
                    {
                        KeyHandler.WaitingForInput = true;
                        Storage.CurrentKeyAction = Storage.KeyActions.KillAllEnemies;
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(5f);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Enable Godmode", GUILayout.Width(DefaultCheatsLabelWidth));
                bool GodModeToggle = GUILayout.Toggle(Storage.GodModeEnabled, string.Empty, GUILayout.Width(190f));
                if (GodModeToggle != PreviousGodModeValue)
                {
                    Cheats.EnableGodMode(GodModeToggle);
                    Storage.GodModeEnabled = GodModeToggle;
                    PreviousGodModeValue = GodModeToggle;
                }
                if (GUILayout.Button("Clear Fog", GUILayout.Width(150f)))
                {
                    Cheats.ClearFogOfWar();
                }
                if (GUILayout.Button(ClearFogKeyBindText, GUILayout.Width(DefaultButtonWidth)))
                {
                    if (!KeyHandler.WaitingForInput)
                    {
                        KeyHandler.WaitingForInput = true;
                        Storage.CurrentKeyAction = Storage.KeyActions.ClearFog;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(5f);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Enable Invisibility", GUILayout.Width(DefaultCheatsLabelWidth));
                bool InvisibilityToggle = GUILayout.Toggle(Storage.InvisibilityEnabled, string.Empty);
                if (InvisibilityToggle != PreviousInvisibilityValue)
                {
                    Cheats.EnableInvisibility(InvisibilityToggle);
                    Storage.InvisibilityEnabled = InvisibilityToggle;
                    PreviousInvisibilityValue = InvisibilityToggle;
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(5f);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Add Currency", GUILayout.Width(DefaultCheatsLabelWidth));
                InputFieldText = CurrencyToAdd.ToString();
                InputFieldText = GUILayout.TextField(InputFieldText, GUILayout.Width(100f));
                string CurrencyButtonText = CurrencyToAdd >= 0 ? "Add" : "Subtract";
                if (GUILayout.Button(CurrencyButtonText, GUILayout.Width(DefaultButtonWidth)))
                {
                    bool parseSuccessful = int.TryParse(InputFieldText, out int value);
                    if (parseSuccessful)
                    {
                        Cheats.AddCurrency(value);
                        CurrencyToAdd = value;
                    }
                    else
                    {
                        mod.Logger.Error($"\"{InputFieldText}\" could not be parsed as an integer.");
                    }
                }
                else
                {
                    int.TryParse(InputFieldText, out CurrencyToAdd);
                }
                
                GUILayout.EndHorizontal();
            }
        }
        #endregion

        #region Stats Settings
        public static void DrawStatsSettings()
        {
            DrawHeader("Stats", ref StatsExpanded);
            if (!StatsExpanded) return;

            if (partyMemberAIs == null) return;

            foreach (PartyMemberAI partyMember in partyMemberAIs)
            {
                GUILayout.Label(partyMember.name);
                GUILayout.Label("Attributes:");
                foreach (CharacterStats.AttributeScoreType type in Enum.GetValues(typeof(CharacterStats.AttributeScoreType)))
                {
                    GUILayout.BeginHorizontal();
                    string LabelString = "";
                    switch (type)
                    {
                        case CharacterStats.AttributeScoreType.Might:
                            LabelString = "Might ";
                            break;
                        case CharacterStats.AttributeScoreType.Resolve:
                            LabelString = "Resolve ";
                            break;
                        case CharacterStats.AttributeScoreType.Finesse:
                            LabelString = "Finesse ";
                            break;
                        case CharacterStats.AttributeScoreType.Quickness:
                            LabelString = "Quickness ";
                            break;
                        case CharacterStats.AttributeScoreType.Wits:
                            LabelString = "Wits ";
                            break;
                        case CharacterStats.AttributeScoreType.Vitality:
                            LabelString = "Vitality ";
                            break;
                        case CharacterStats.AttributeScoreType.Count:
                            LabelString = "Count ";
                            break;
                        default:
                            break;
                    }
                    int statValue = Stats.GetBaseAttributeScore(type, partyMember);
                    GUILayout.Label(LabelString, GUILayout.Width(DefaultLabelWidth));
                    if (GUILayout.Button("-", GUILayout.Width(25)))
                    {
                        Stats.SetBaseAttributeScore(type, partyMember, statValue - 1);
                    }
                    var style = GUI.skin.GetStyle("Label");
                    style.alignment = TextAnchor.UpperCenter;
                    GUILayout.Label(statValue.ToString() + " ", style, GUILayout.Width(45));
                    style.alignment = TextAnchor.UpperLeft;
                    if (GUILayout.Button("+", GUILayout.Width(25)))
                    {
                        Stats.SetBaseAttributeScore(type, partyMember, statValue + 1);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.Space(10f);

                GUILayout.Label("Skills: ");

                DrawSkillRow(CharacterStats.SkillType.One_Handed, partyMember, "One-Handed ");
                DrawSkillRow(CharacterStats.SkillType.Unarmed, partyMember, "Unarmed ");
                DrawSkillRow(CharacterStats.SkillType.Bows, partyMember, "Bows ");
                DrawSkillRow(CharacterStats.SkillType.Dual_Wield, partyMember, "Dual Wield ");
                DrawSkillRow(CharacterStats.SkillType.Magical_Weapons, partyMember, "Magic Staff ");
                DrawSkillRow(CharacterStats.SkillType.Two_Handed, partyMember, "Two-Handed ");
                DrawSkillRow(CharacterStats.SkillType.Athletics, partyMember, "Athletics ");
                DrawSkillRow(CharacterStats.SkillType.Dodge, partyMember, "Dodge ");
                DrawSkillRow(CharacterStats.SkillType.Lore, partyMember, "Lore ");
                DrawSkillRow(CharacterStats.SkillType.Parry, partyMember, "Parry ");
                DrawSkillRow(CharacterStats.SkillType.Stealth, partyMember, "Subterfuge ");

                GUILayout.Space(10f);
            }
        }
        #endregion

        #region Movement Settings
        public static void DrawMovementSettings()
        {
            DrawHeader("Movement Settings", ref MovementExpanded);
            if (MovementExpanded)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"Run Speed:", GUILayout.Width(DefaultLabelWidth));
                InputFieldText = RunSpeed.ToString("0.0#", CultureInfo.InvariantCulture);
                InputFieldText = GUILayout.TextField(InputFieldText, 10, GUILayout.Width(DefaultTextFieldWidth));
                
                if (GUILayout.Button("Default", GUILayout.Width(DefaultButtonWidth)))
                {
                    RunSpeed = DefaultRunSpeed;
                }
                else if (!float.TryParse(InputFieldText, out RunSpeed))
                {
                    RunSpeed = DefaultRunSpeed;
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                RunSpeed = GUILayout.HorizontalSlider(RunSpeed, 0.0f, 25.0f);
                Storage.RunSpeed = RunSpeed;

                // Min and max values for first slider
                GUILayout.BeginHorizontal();
                var defaultAlignment = GUI.skin.label.alignment;
                GUI.skin.label.alignment = TextAnchor.UpperLeft;
                GUILayout.Label(SliderMin.ToString());
                GUI.skin.label.alignment = TextAnchor.UpperRight;
                GUILayout.Label(SliderMax.ToString());
                GUI.skin.label.alignment = defaultAlignment;
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label($"Walk Speed:", GUILayout.Width(DefaultLabelWidth));
                InputFieldText = WalkSpeed.ToString("0.0#", CultureInfo.InvariantCulture);
                InputFieldText = GUILayout.TextField(InputFieldText, 10, GUILayout.Width(DefaultTextFieldWidth));
                
                if (GUILayout.Button("Default", GUILayout.Width(DefaultButtonWidth)))
                {
                    WalkSpeed = DefaultWalkSpeed;
                }
                else if (!float.TryParse(InputFieldText, out WalkSpeed))
                {
                    WalkSpeed = DefaultWalkSpeed;
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                WalkSpeed = GUILayout.HorizontalSlider(WalkSpeed, 0.0f, 25.0f);
                Storage.WalkSpeed = WalkSpeed;

                // Min and max values for second slider
                GUILayout.BeginHorizontal();
                GUI.skin.label.alignment = TextAnchor.UpperLeft;
                GUILayout.Label(SliderMin.ToString());
                GUI.skin.label.alignment = TextAnchor.UpperRight;
                GUILayout.Label(SliderMax.ToString());
                GUI.skin.label.alignment = defaultAlignment;
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label($"Stealth Speed:", GUILayout.Width(DefaultLabelWidth));
                InputFieldText = StealthSpeed.ToString("0.0#", CultureInfo.InvariantCulture);
                InputFieldText = GUILayout.TextField(InputFieldText, 10, GUILayout.Width(DefaultTextFieldWidth));
                
                if (GUILayout.Button("Default", GUILayout.Width(DefaultButtonWidth)))
                {
                    StealthSpeed = DefaultStealthSpeed;
                }
                else if (!float.TryParse(InputFieldText, out StealthSpeed))
                {
                    StealthSpeed = DefaultStealthSpeed;
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                StealthSpeed = GUILayout.HorizontalSlider(StealthSpeed, 0.0f, 25.0f);
                Storage.StealthSpeed = StealthSpeed;

                // Min and max values for third slider
                GUILayout.BeginHorizontal();
                GUI.skin.label.alignment = TextAnchor.UpperLeft;
                GUILayout.Label(SliderMin.ToString());
                GUI.skin.label.alignment = TextAnchor.UpperRight;
                GUILayout.Label(SliderMax.ToString());
                GUI.skin.label.alignment = defaultAlignment;
                GUILayout.EndHorizontal();
            }
        }
        #endregion

        #region Achievement Settings
        public static void DrawAchievementSettings()
        {
            DrawHeader("Achievements", ref AchievementsExpanded);
            if (!AchievementsExpanded) return;
            if (AchievementTracker.Instance == null)
            {
                GUILayout.Label("AchievementTracker not instantiated. Load a save to access achievements.");
                return;
            }
            GUILayout.Space(5f);
            GUILayout.BeginVertical();

            int i = 0;
            foreach (var achievement in achievementInfo)
            {
                GUILayout.Space(6f);
                if (i % 2 == 0)
                {
                    GUIStyle style = new GUIStyle();
                    Texture2D texture = new Texture2D(1, 1);
                    texture.SetPixel(0, 0, new Color(0,0,0));
                    texture.Apply();
                    style.normal.background = texture;
                    GUILayout.BeginHorizontal(style);
                }
                else
                {
                    GUILayout.BeginHorizontal();
                }

                if (i == achievementHoverIndex)
                {
                    GUILayout.Label(achievement.First);
                }
                else
                {
                    GUILayout.Label(achievement.First);
                }

                if (GUILayout.Button("Unlock", GUILayout.Width(DefaultButtonWidth)))
                {
                    AchievementTracker.Instance.SetAchievement(achievement.Second);
                }
                GUILayout.EndHorizontal();
                i++;
            }
            GUILayout.EndVertical();
        }
        #endregion

        private static void DrawHeader(string name, ref bool expanded)
        {
            GUILayout.BeginHorizontal();
            GUIStyle settingsLabelStyle = new GUIStyle();
            settingsLabelStyle.fontStyle = FontStyle.Bold;
            settingsLabelStyle.normal.textColor = Color.white;
            GUILayout.Label(name, settingsLabelStyle, GUILayout.Width(500f));
            string buttonText;
            Color defaultColor = GUI.backgroundColor;
            if (expanded)
            {
                buttonText = "Collapse";
                GUI.backgroundColor = Color.red;
            }
            else
            {
                buttonText = "Expand";
            }
            if (GUILayout.Button(buttonText))
            {
                expanded = !expanded;
            }

            GUI.backgroundColor = defaultColor;
            GUILayout.EndHorizontal();
        }

        private static void DrawSkillRow(CharacterStats.SkillType skillType, PartyMemberAI partyMember, string labelText)
        {
            GUILayout.BeginHorizontal();
            int skillValue = Stats.GetBaseSkillScore(skillType, partyMember);
            GUILayout.Label(labelText, GUILayout.Width(DefaultLabelWidth));
            if (GUILayout.Button("-", GUILayout.Width(25)))
            {
                Stats.SetBaseSkillScore(skillType, partyMember, skillValue - 1);
            }
            var skillStyle = GUI.skin.GetStyle("Label");
            skillStyle.alignment = TextAnchor.UpperCenter;
            GUILayout.Label(skillValue.ToString() + " ", skillStyle, GUILayout.Width(45));
            skillStyle.alignment = TextAnchor.UpperLeft;
            if (GUILayout.Button("+", GUILayout.Width(25)))
            {
                Stats.SetBaseSkillScore(skillType, partyMember, skillValue + 1);
            }
            GUILayout.EndHorizontal();
        }
    }
}