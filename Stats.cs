using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BagOfTricks
{
    public static class Stats
    {
        static object[] partyMembers = null;

        /// <summary>
        /// Subscribe to UI events
        /// </summary>
        public static void InitializeEventSubscriptions()
        {
            ModUI.OnAttributeChanged += SetBaseAttributeScore;
            ModUI.OnSkillChanged += SetBaseSkillScore;
        }

        /// <summary>
        /// Fetches the base value of a given attribute for a given party member.
        /// </summary>
        /// <param name="type">The attribute to fetch.</param>
        /// <param name="partyMember">The party member to fetch the attribute value from.</param>
        /// <returns>The base attribute score for a specific party member based on the given attribute type.</returns>
        public static int GetBaseAttributeScore(CharacterStats.AttributeScoreType type, PartyMemberAI partyMember)
        {
            CharacterStats stats = partyMember.GetComponent<CharacterStats>();
            return stats.GetBaseAttributeScore(type);
        }

        /// <summary>
        /// Sets the base value of a given attribute for a given party member.
        /// </summary>
        /// <param name="type">The attribute to apply the value to.</param>
        /// <param name="partyMember">The party member to apply the attribute value to.</param>
        /// <param name="value">The new attribute value to apply.</param>
        public static void SetBaseAttributeScore(CharacterStats.AttributeScoreType type, PartyMemberAI partyMember, int value)
        {
            CharacterStats stats = partyMember.GetComponent<CharacterStats>();
            switch (type)
            {
                case CharacterStats.AttributeScoreType.Might:
                    stats.BaseMight = value;
                    break;
                case CharacterStats.AttributeScoreType.Resolve:
                    stats.BaseResolve = value;
                    break;
                case CharacterStats.AttributeScoreType.Finesse:
                    stats.BaseFinesse = value;
                    break;
                case CharacterStats.AttributeScoreType.Quickness:
                    stats.BaseQuickness = value;
                    break;
                case CharacterStats.AttributeScoreType.Wits:
                    stats.BaseWits = value;
                    break;
                case CharacterStats.AttributeScoreType.Vitality:
                    stats.BaseVitality = value;
                    break;
                case CharacterStats.AttributeScoreType.Count:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Fetches the base value of a given skill from a given party member.
        /// </summary>
        /// <param name="type">The skill type to fetch.</param>
        /// <param name="partyMember">The party member to fetch the skill value from.</param>
        /// <returns>The base value for a specific party member based on the given skill type.</returns>
        public static int GetBaseSkillScore(CharacterStats.SkillType type, PartyMemberAI partyMember)
        {
            CharacterStats stats = partyMember.GetComponent<CharacterStats>();
            return stats.GetSkillRank(type);
        }

        /// <summary>
        /// Sets the base value of a given skill for a given party member.
        /// </summary>
        /// <param name="type">The skill apply the value to.</param>
        /// <param name="partyMember">The party member to apply the skill value to.</param>
        /// <param name="value">The new skill value to apply.</param>
        public static void SetBaseSkillScore(CharacterStats.SkillType type, PartyMemberAI partyMember, int value)
        {
            CharacterStats stats = partyMember.GetComponent<CharacterStats>();
            switch (type)
            {
                case CharacterStats.SkillType.One_Handed:
                    stats.OneHandedSkill = value;
                    break;
                case CharacterStats.SkillType.Unarmed:
                    stats.UnarmedSkill = value;
                    break;
                case CharacterStats.SkillType.Bows:
                    stats.BowsSkill = value;
                    break;
                case CharacterStats.SkillType.Dual_Wield:
                    stats.DualWieldSkill = value;
                    break;
                case CharacterStats.SkillType.Magical_Weapons:
                    stats.MagicalWeaponSkill = value;
                    break;
                case CharacterStats.SkillType.Two_Handed:
                    stats.TwoHandedSkill = value;
                    break;
                case CharacterStats.SkillType.Athletics:
                    stats.AthleticsSkill = value;
                    break;
                case CharacterStats.SkillType.Dodge:
                    stats.DodgeSkill = value;
                    break;
                case CharacterStats.SkillType.Lore:
                    stats.LoreSkill = value;
                    break;
                case CharacterStats.SkillType.Parry:
                    stats.ParrySkill = value;
                    break;
                case CharacterStats.SkillType.Stealth:
                    stats.StealthSkill = value;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Fetches all party members in the current party, including the player character.
        /// </summary>
        /// <returns>The party members in the current party as an object array.</returns>
        public static object[] GetPartyMembers()
        {
            if (partyMembers != null && partyMembers.Length > 0)
            {
                return partyMembers;
            }
            else
            {
                object[] arr = UnityEngine.Object.FindObjectsOfType(typeof(PartyMemberAI));
                if (arr == null || arr.Length == 0)
                {
                    return null;
                }
                return arr;
            }
        }
    }
}
