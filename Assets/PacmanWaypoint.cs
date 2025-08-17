// File: Assets/Scripts/TeleportWaypointInfo.cs
using UnityEngine;

public class TeleportWaypointInfo : WaypointInfo
{
    [Header("Teleport Settings")]
    [Tooltip("If true, this waypoint will teleport the car.")]
    public bool teleportOnReach = true;

    [Tooltip("Where the car should be teleported to.")]
    public Transform teleportDestination;

    // Optional: offset to spawn slightly above the ground
    public Vector3 spawnOffset = Vector3.up * 0.2f;

    /// <summary>
    /// Teleport the given NPC to the teleport destination.
    /// Returns the nextTarget so NPCPathing knows where to go next.
    /// </summary>
    public Transform HandleTeleport(Transform npcTransform)
    {
        if (teleportOnReach && teleportDestination != null)
        {
            npcTransform.position = teleportDestination.position + spawnOffset;
            npcTransform.rotation = teleportDestination.rotation;
        }

        return nextTarget;
    }
}
