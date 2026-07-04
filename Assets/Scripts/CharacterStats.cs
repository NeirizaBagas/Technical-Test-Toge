using UnityEngine;

[CreateAssetMenu(fileName = "New Character Stats", menuName = "Character Stats") ]
public class CharacterStats : ScriptableObject
{
    public bool isPlayerCharacter;
    public int maxHealth;
    public int attackDamage;

}
