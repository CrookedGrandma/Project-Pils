using UnityEngine;
using System.Collections;

public enum CorridorDirection
{
    Up, Down, Left, Right
}

public class Dungeon_Corridor
{
    public int startXPos, startZPos, corridorLength;
    public CorridorDirection direction;

    // Getter for the X coordinate of the endposition of the corridor
    public int EndXPos
    {
        get
        {
            if (direction == CorridorDirection.Up || direction == CorridorDirection.Down)
            {
                return startXPos;                                       // XPos stays the same when going up or down
            }
            else if (direction == CorridorDirection.Right)
            {
                return startXPos + corridorLength - 1;                  // XPos plus length, -1 to get the last tile of the corridor
            }
            else
            {
                return startXPos - corridorLength + 1;                  // So this is left | XPos minus length, +1 to get the last tile of the corridor
            }
        }
    }

    // Getter for the Z coordinate of the endposition of the corridor
    public int EndZPos
    {
        get
        {
            if (direction == CorridorDirection.Right || direction == CorridorDirection.Left)
            {
                return startZPos;                                       // ZPos stays the same when going left or right
            }
            else if (direction == CorridorDirection.Up)
            {
                return startZPos + corridorLength - 1;                  // ZPos plus length, -1 to get the last tile of the corridor
            }
            else
            {
                return startZPos - corridorLength + 1;                  // So this is down | ZPos minus length, +1 to get the last tile of the corridor
            }
        }
    }

    // Method which sets up a corridor
    public void SetupCorridor(Dungeon_Room room, int minCorridorLength, int maxCorridorLength, int roomWidth, int roomHeight, int columns, int rows, bool isFirstCorridor)
    {
        // Set a direction for the corridor, a random int cast to a CorridorDirection
        direction = (CorridorDirection)Random.Range(0, 4);

        // This calculates the opposite direction from the corridor that enters the room.
        CorridorDirection oppositeDirection = (CorridorDirection)(((int)room.enteringCorridor + 2) % 4);

        // Only the direction of the first corridor has to be checked
        if (isFirstCorridor)
        {
            CheckFirstCorridorDirection(direction);
        }

        if (!isFirstCorridor && direction == oppositeDirection)
        {
            // It can't be the first corridor since it doesn't have an opposite direction
            // To make it so all the rooms are not connected by corridors all going the same way
            // This rotates the corridor 90 degrees instead of 180 degrees.
            int intDirection = (int)direction;
            intDirection = (intDirection + 1) % 4;
            direction = (CorridorDirection)intDirection;
        }

        // Get a random length for the corridor
        corridorLength = Random.Range(minCorridorLength, maxCorridorLength + 1);

        // Make a variable which caps the maximum length of the corridor. This is necessary for making sure the corridor won't be able to exit the maximum size of the dungeon 
        int maxLength = maxCorridorLength;

        if (direction == CorridorDirection.Up)
        {
            startXPos = Random.Range(room.xPos, room.xPos + room.roomWidth - 1);            // Get a random position along the X-axis
            startZPos = room.zPos + room.roomHeight;                                        // The Z position must be at the top of the room
            maxLength = rows - startZPos - room.roomHeight;                                 // This makes sure the corridor does not exit the dungeon on the top side
        }
        else if (direction == CorridorDirection.Right)
        {
            startXPos = room.xPos + room.roomWidth;
            startZPos = Random.Range(room.zPos, room.zPos + room.roomHeight - 1);
            maxLength = columns - startXPos - room.roomWidth;
        }
        else if (direction == CorridorDirection.Down)
        {
            startXPos = Random.Range(room.xPos, room.xPos + room.roomWidth);
            startZPos = room.zPos;
            maxLength = startZPos - room.roomHeight;
        }
        else if (direction == CorridorDirection.Left)
        {
            startXPos = room.xPos;
            startZPos = Random.Range(room.zPos, room.zPos + room.roomHeight);
            maxLength = startXPos - room.roomWidth;
        }
        
        // Finally, clamp the corridorlength using the maxLength calculated just before this
        corridorLength = Mathf.Clamp(corridorLength, 1, maxLength);
    }

    // If the first corridor is going down or to the left, meaning directly into the wall, choose another direction
    void CheckFirstCorridorDirection(CorridorDirection corridorDirection)
    {
        if (corridorDirection == CorridorDirection.Left || corridorDirection == CorridorDirection.Down)
        {
            corridorDirection = (CorridorDirection)Random.Range(0, 4);
            direction = corridorDirection;

            // Do this again untill the direction is up or to the right
            CheckFirstCorridorDirection(corridorDirection);
        }
    }
}
