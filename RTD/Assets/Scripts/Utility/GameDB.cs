using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDB : MonoBehaviour
{   
    public string[] Card = new string[] {
        "UI/EmptyCard",
        "UI/N_Magician",
        "UI/N_Mutant"
    };

    public string[] CharacterAllClassAddr = new string[] {
        "Character/N_Magician",
        "Character/N_Mutant",
        "Character/M_Magician",
        "Character/M_Mutant",
        "Character/R_Magician",
        "Character/R_Mutant",
        "Character/Q_Magician",
        "Character/Q_Mutant",
    };

    public string[] CharacterNoramlClassAddr = new string[] {
        "Character/N_Magician",
        "Character/N_Mutant"
    };
    public string[] CharacterMagicClassAddr = new string[] {
        "Character/M_Magician",
        "Character/M_Mutant"
    };
    public string[] CharacterRareClassAddr = new string[] {
        "Character/R_Magician",
        "Character/R_Mutant"
    };
    public string[] CharacterUniqueClassAddr = new string[] {
        "Character/Q_Magician",
        "Character/Q_Mutant"
    };

    public string[] CharacterClass = new string[] {
        "Magician",
        "Mutant"
    };

    public string[] CharacterGrade = new string[] {
        "Magic",
        "Rare",
        "Unique"
    };
}

