using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelColliderActivator : MonoBehaviour
{
    // If the collider is not a trigger, it is used to make sure the player does not fall of the map

    // Setup an array with all the load level colliders in this scene
    private LoadLevel[] loadLevelArray;

    private void Start()
    {
        // Fill the array with all the load level colliders
        loadLevelArray = GameObject.FindObjectsOfType<LoadLevel>();
    }

    private void OnTriggerEnter(Collider trigger)
    {
        // Only when the player enters the collider, else the other colliders will be able to activate this
        if (trigger.name == "Player")
        {
            // When the player enters the ColliderActivators, activate all the colliders in the scene
            foreach (LoadLevel loadLevel in loadLevelArray)
            {
                loadLevel.isActive = true;
            }
        }
    }
}
