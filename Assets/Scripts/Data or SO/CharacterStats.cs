using UnityEngine;

/// <summary>
/// Scriptable object that stores base character statistics.
/// Can be created via Assets > Create > Character Stats menu.
/// </summary>
[CreateAssetMenu(fileName = "New Character Stats", menuName = "Character Stats")]
public class CharacterStats : ScriptableObject
{
    [Tooltip("Whether this character is the player")]
    public bool isPlayerCharacter;

    [Tooltip("Maximum health points")]
    public int maxHealth;

    [Tooltip("Attack damage value")]
    public int attackDamage;
}
