using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDB : MonoBehaviour
{

    public string[] Card = new string[] {
        "UI/CardList/EmptyCard",
        "UI/CardList/N_Assassin",
        "UI/CardList/N_Berserker",
        "UI/CardList/N_BowKing",
        "UI/CardList/N_BowKnight",
        "UI/CardList/N_CastleKnight",
        "UI/CardList/N_CrossBow Knight",
        "UI/CardList/N_Dark Wizard",
        "UI/CardList/N_Gladiator",
        "UI/CardList/N_Hero",
        "UI/CardList/N_Hunter",
        "UI/CardList/N_Lich",
        "UI/CardList/N_Mage Knight",
        "UI/CardList/N_Priest",
        "UI/CardList/N_SwordMaster",
        "UI/CardList/N_Wizard"
    };

    public string[] CharacterAllClassAddr = new string[] {
        "Character/Player Characters/Long Ranged/N_Marine",
        "Character/Player Characters/Long Ranged/M_Marine",
        "Character/Player Characters/Long Ranged/R_Marine",
        "Character/Player Characters/Long Ranged/Q_Marine",
        "Character/Player Characters/Magic/N_Lich",
        "Character/Player Characters/Magic/M_Lich",
        "Character/Player Characters/Magic/R_Lich",
        "Character/Player Characters/Magic/Q_Lich",
        "Character/Player Characters/Short Ranged/N_Warrior",
        "Character/Player Characters/Short Ranged/M_Warrior",
        "Character/Player Characters/Short Ranged/R_Warrior",
        "Character/Player Characters/Short Ranged/Q_Warrior"
    };

    public string[] CharacterNoramlClassAddr = new string[] {
        "Character/Player Characters/Long Ranged/N_Marine",
        "Character/Player Characters/Magic/N_Lich",
        "Character/Player Characters/Short Ranged/N_Warrior"
    };
    public string[] CharacterMagicClassAddr = new string[] {
        "Character/Player Characters/Long Ranged/M_Marine",
        "Character/Player Characters/Magic/M_Lich",
        "Character/Player Characters/Short Ranged/M_Warrior"
    };
    public string[] CharacterRareClassAddr = new string[] {
        "Character/Player Characters/Long Ranged/R_Marine",
        "Character/Player Characters/Magic/R_Lich",
        "Character/Player Characters/Short Ranged/R_Warrior"
    };
    public string[] CharacterUniqueClassAddr = new string[] {
        "Character/Player Characters/Long Ranged/Q_Marine",
        "Character/Player Characters/Magic/Q_Lich",
        "Character/Player Characters/Short Ranged/Q_Warrior"
    };
    public string[] PreLoadingPrefabAddr = new string[]{
        //"Visual Effect/Skill Effect/Marine Skill",
        //"Visual Effect/Skill Effect/Marine SkillReady",
        //"Character/Boss/Dragon",
        //"Character/Enemy/Golem",
        //"Character/Enemy/Slime",
        //"Character/Enemy/TurtleShell",
        //"Character/Player Characters/Long Ranged/Marine",
        //"Character/Player Characters/Long Ranged/N_Marine",
        //"Character/Player Characters/Long Ranged/M_Marine",
        //"Character/Player Characters/Long Ranged/R_Marine",
        //"Character/Player Characters/Long Ranged/Q_Marine",
        //"Character/Player Characters/Magic/Lich",
        //"Character/Player Characters/Magic/N_Lich",
        //"Character/Player Characters/Magic/M_Lich",
        //"Character/Player Characters/Magic/R_Lich",
        //"Character/Player Characters/Magic/Q_Lich",
        //"Character/Player Characters/Short Ranged/Warrior",
        //"Character/Player Characters/Short Ranged/N_Warrior",
        //"Character/Player Characters/Short Ranged/M_Warrior",
        //"Character/Player Characters/Short Ranged/R_Warrior",
        //"Character/Player Characters/Short Ranged/Q_Warrior",
        //"Projectile/FireBall Hit Effect",
        //"Projectile/FireBall01",
        //"Projectile/Icebolt Hit Effect",
        //"Projectile/Icebolt",
    };

    public string[] Boss = new string[]{
        "Character/Boss/Dragon"
    };
}

