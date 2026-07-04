using Fungus;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("Fungus Dialogue")]
    [SerializeField] private Flowchart targetFlowchart;
    [SerializeField] private string blockName;

    public void TriggerDialogue()
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
            Debug.Log("Triggering dialogue for " + gameObject.name);
#endif
        if (targetFlowchart== null) return;

        if (!targetFlowchart.HasExecutingBlocks()) // Check if no other block is currently executing
        {
            targetFlowchart.ExecuteBlock(blockName);
        }
    }
}
