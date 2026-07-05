using Fungus;
using UnityEngine;

public enum NPCType { Villager, Spearman, Knight }

/// <summary>
/// Manages interaction with NPCs and triggers Fungus dialogue flows.
/// Handles dialogue initiation and ensures dialogue blocks don't overlap.
/// </summary>
public class InteractableObject : MonoBehaviour
{
    [Tooltip("Type of NPC for game logic identification")]
    public NPCType npcType;
    public bool hasInteracted = false;

    [Header("Fungus Dialogue")]
    [SerializeField] private Flowchart targetFlowchart;
    [SerializeField] private string blockName;

    /// <summary>
    /// Triggers the associated Fungus dialogue block.
    /// Prevents multiple dialogue blocks from executing simultaneously.
    /// </summary>
    public void TriggerDialogue()
    {
        if (targetFlowchart == null) return;

        // Check if no other block is currently executing
        if (!targetFlowchart.HasExecutingBlocks())
        {
            targetFlowchart.ExecuteBlock(blockName);
        }
        hasInteracted = true;
    }
}
