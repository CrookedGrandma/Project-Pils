using UnityEngine;
using System.Collections;

public class Dungeon_Room
{
    public int xPos, zPos, roomWidth, roomHeight;
    public CorridorDirection enteringCorridor;

    // Method which sets up the first room
    public void SetupFirstRoom(int width, int height, int columns, int rows)
    {
        // Setup the width and height
        roomWidth = width;
        roomHeight = height;

        // Set the X and Z coordinates for the room so it is roughly in the center
        xPos = columns / 2 - width / 2;
        zPos = rows / 2 - height / 2;
    }

    // Method for setting up the other rooms
    public void SetupRoom(int width, int height, int columns, int rows, Dungeon_Corridor corridor)
    {
        // Set the direction of the entering corridor
        enteringCorridor = corridor.direction;

        // Setup the width and height
        roomWidth = width;
        roomHeight = height;

        // The corridor.direction is the direction of the entering corridor
        // These if-statements are used to make sure the rooms are always within the size of the dungeon
        if (corridor.direction == CorridorDirection.Up)
        {
            roomHeight = Mathf.Clamp(roomHeight, 1, rows - corridor.EndZPos);                   // Clamp the roomheight so the room won't go out of the dungeon
            zPos = corridor.EndZPos;                                                            // Set the Z position of the room
            xPos = Random.Range(corridor.EndXPos - roomWidth + 1, corridor.EndXPos + 1);        // Set a random X position for the room
            xPos = Mathf.Clamp(xPos, 0, columns - roomWidth);                                   // Clamp the X position of the room so it won't go out of the dungeon
        }
        else if (corridor.direction == CorridorDirection.Right)
        {
            roomWidth = Mathf.Clamp(roomWidth, 1, columns - corridor.EndXPos);
            xPos = corridor.EndXPos;
            zPos = Random.Range(corridor.EndZPos - roomHeight + 1, corridor.EndZPos + 1);
            zPos = Mathf.Clamp(zPos, 0, rows - roomHeight);
        }
        else if (corridor.direction == CorridorDirection.Down)
        {
            roomHeight = Mathf.Clamp(roomHeight, 1, corridor.EndZPos);
            zPos = corridor.EndZPos - roomHeight + 1;
            xPos = Random.Range(corridor.EndXPos - roomWidth + 1, corridor.EndXPos + 1);
            xPos = Mathf.Clamp(xPos, 0, columns - roomWidth);
        }
        else if (corridor.direction == CorridorDirection.Left)
        {
            roomWidth = Mathf.Clamp(roomWidth, 1, corridor.EndXPos);
            xPos = corridor.EndXPos - roomWidth + 1;
            zPos = Random.Range(corridor.EndZPos - roomHeight + 1, corridor.EndZPos + 1);
            zPos = Mathf.Clamp(zPos, 0, rows - roomHeight);
        }
    }
}
