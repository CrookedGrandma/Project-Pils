using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonCreator : MonoBehaviour
{
    // The tiletypes used
    public enum TileType
    {
        Wall, Floor
    }

    public int columns = 30, rows = 30, minNumRooms = 10, maxNumRooms = 12, minRoomWidth = 4, maxRoomWidth = 8, minRoomHeight = 4, maxRoomHeight = 8, 
               minCorridorLength = 8, maxCorridorLength = 14, minNumberOfEnemies = 2, maxNumberOfEnemies = 6;
    public GameObject[] wallTiles, outerWallTiles;                          // Used so there can be multiple models for a certain type, for diversity
    public GameObject player, endPoint, enemy;                              // Normal gameobjects for the player, endpoint and enemies

    private TileType[][] tiles;
    private Dungeon_Room[] rooms;
    private Dungeon_Corridor[] corridors;
    private GameObject dungeonHolder, enemyHolder;
    private int currentEnemies = 0, numberOfEnemies;

    private void Start()
    {
        // Create the dungeonHolder and enemyHolder GameObjects if they not exist
        if (!dungeonHolder)
        {
            dungeonHolder = new GameObject("DungeonHolder");
        }

        if (!enemyHolder)
        {
            enemyHolder = new GameObject("EnemyHolder");
        }

        // Setup the maximum amount of enemies
        numberOfEnemies = Random.Range(minNumberOfEnemies, maxNumberOfEnemies + 1);

        SetupTilesArray();
        CreateRoomsAndCorridors();
        SetTilesInRooms();
        SetTilesInCorridors();
        InstantiateTiles();
        InstantiateOuterWalls();
    }

    private void Reset()
    {
        // Just reload the level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Update()
    {
        // DEBUG ONLY
        if (Input.GetButtonDown("Reset"))
        {
            Reset();
        }
        // END DEBUG ONLY
    }

    // Set up the tiles array to the correct size
    private void SetupTilesArray()
    {
        tiles = new TileType[columns][];

        for(int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = new TileType[rows];
        }
    }

    // Method that creates the rooms and corridors
    private void CreateRoomsAndCorridors()
    {
        rooms = new Dungeon_Room[Random.Range(minNumRooms, maxNumRooms + 1)];       // Setup a random amount of rooms
        corridors = new Dungeon_Corridor[rooms.Length - 1];                         // There is always one less corridor than the amount of rooms

        rooms[0] = new Dungeon_Room();                                              // Create the first room
        corridors[0] = new Dungeon_Corridor();                                      // Create the first corridor

        // Setup the first room
        rooms[0].SetupFirstRoom(Random.Range(minRoomWidth, maxRoomWidth + 1), Random.Range(minRoomHeight, maxRoomHeight + 1), columns, rows);

        // Setup the first corridor
        corridors[0].SetupCorridor(rooms[0], minCorridorLength, maxCorridorLength, rooms[0].roomWidth, rooms[0].roomHeight, columns, rows, true);

        // Instantiate the player in the first room
        Vector3 playerPos = new Vector3(rooms[0].xPos + 0.5f, 0.35f, rooms[0].zPos + 0.5f);
        Instantiate(player, playerPos, Quaternion.identity);

        // Create and Setup the other rooms
        for (int i = 1; i < rooms.Length; i++)
        {
            rooms[i] = new Dungeon_Room();
            rooms[i].SetupRoom(Random.Range(minRoomWidth, maxRoomWidth + 1), Random.Range(minRoomHeight, maxRoomHeight + 1), columns, rows, corridors[i - 1]);

            if (i < corridors.Length)
            {
                corridors[i] = new Dungeon_Corridor();
                corridors[i].SetupCorridor(rooms[i], minCorridorLength, maxCorridorLength, rooms[i].roomWidth, rooms[i].roomHeight, columns, rows, false);
            }

            // Put the endpoint in the last room
            if (i == rooms.Length - 1)
            {
                Vector3 endPointPos = new Vector3(rooms[i].xPos + rooms[i].roomWidth / 2 - 0.5f, 0.35f, rooms[i].zPos + rooms[i].roomHeight / 2 - 0.5f);
                
                // Makes sure the endpoint is somewhere above the player, with at least 20 tiles distance
                for (int x = -30; x <= 30; x++)
                {
                    for (int z = -20; z <= 0; z++)
                    {
                        Vector3 pos = new Vector3(endPointPos.x + x, 0.35f, endPointPos.z + z);
                        if (pos == playerPos)
                        {
                            // Do not spawn endpoint here, because it is too close to the player
                            Reset();
                            // Resetting the level is just a temporary solution. 
                            // It may cause conflicts with the progress-saving and players are able to see the reset happening
                            /// TODO, think of a better solution
                        }
                    }
                }

                Instantiate(endPoint, endPointPos, Quaternion.identity);
            }

            // Spawn enemies in random rooms
            if (currentEnemies < numberOfEnemies)
            {
                // Try to spawn an enemy
                float chance = (float)numberOfEnemies / (float)rooms.Length;
                if (Random.Range(0f, 1f) >= chance)
                {
                    // Spawn an enemy
                    /// TODO, zorg dat de enemies overal in de kamer kunnen spawnen, maar niet voor gangen
                    /// Niet voor een eindpunt zodat je niet de dungeon uit kan
                    bool maySpawnEnemy = true;
                    Vector3 enemyPos = new Vector3(rooms[i].xPos + Random.Range(1, rooms[i].roomWidth - 1) + 0.5f, 0.35f, rooms[i].zPos + 
                                       Random.Range(1, rooms[i].roomHeight - 1) + 0.5f);
                    
                    // Make sure the enemy does not spawn too close to the player
                    for (int x = -3; x <= 3; x++)
                    {
                        for (int z = -3; z <= 3; z++)
                        {
                            Vector3 pos = new Vector3(enemyPos.x + x, 0.35f, enemyPos.z + z);
                            // Too close to the player
                            if (pos == playerPos)
                            {
                                // Do not spawn enemy here, because he is too close to the player
                                Debug.Log("Enemy too close to player to spawn");
                                maySpawnEnemy = false;
                            }

                            // Too close to the endpoint
                            /*if (pos == endPointPos) // Can't use endpointpos, so this will be added later
                            {
                                // Do not spawn enemy here, because he is too close to the endpoint
                                Debug.Log("Enemy too close to endpoint to spawn");
                                maySpawnEnemy = false;
                            }*/
                        }
                    }

                    if (maySpawnEnemy)
                    {
                        GameObject enemyClone = Instantiate(enemy, enemyPos, Quaternion.identity) as GameObject;
                        enemyClone.transform.parent = enemyHolder.transform;
                        currentEnemies++;
                    }
                }
            }
        }
    }

    // Set the values for all the tiles in each room to floor
    private void SetTilesInRooms()
    {
        // Go through all the rooms
        for (int i = 0; i < rooms.Length; i++)
        {
            // Cover the entire width
            for (int j = 0; j < rooms[i].roomWidth; j++)
            {
                int xCoordinate = rooms[i].xPos + j;

                // Cover the entire height
                for (int k = 0; k < rooms[i].roomHeight; k++)
                {
                    int zCoordinate = rooms[i].zPos + k;

                    // Set the values for the tiles
                    tiles[xCoordinate][zCoordinate] = TileType.Floor;
                }
            }
        }
    }

    // Set the tile values for al the tiles in each corridor to floor
    private void SetTilesInCorridors()
    {
        // Go through all the corridors
        for (int i = 0; i < corridors.Length; i++)
        {
            // Cover the entire length
            for (int j = 0; j < corridors[i].corridorLength; j++)
            {
                // Set the coordinates at the start of the corridor
                int xCoordinate = corridors[i].startXPos;
                int zCoordinate = corridors[i].startZPos;

                // Depending on the direction, change the values of the coordinates
                if (corridors[i].direction == CorridorDirection.Up)
                {
                    zCoordinate += j;
                }
                else if (corridors[i].direction == CorridorDirection.Right)
                {
                    xCoordinate += j;
                }
                else if (corridors[i].direction == CorridorDirection.Down)
                {
                    zCoordinate -= j;
                }
                else if (corridors[i].direction == CorridorDirection.Left)
                {
                    xCoordinate -= j;
                }

                // Set the values for the tiles
                tiles[xCoordinate][zCoordinate] = TileType.Floor;
            }
        }
    }

    // Instantiate the tiles
    private void InstantiateTiles()
    {
        // Go through all the tiles
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                // If the tiletype is a wall, instantiate a wall here
                if (tiles[i][j] == TileType.Wall)
                {
                    InstantiateFromArray(wallTiles, i, j);
                }
            }
        }
    }

    // Instantiate tiles around the dungeon so the player will not be able to escape it
    private void InstantiateOuterWalls()
    {
        float leftXEdge = -1f, rightXEdge = columns, bottomZEdge = -1f, topZEdge = rows;

        // Instantiate the outer wall tiles on the left and right of the dungeon
        InstantiateVerticalOuterWalls(leftXEdge, bottomZEdge, topZEdge);
        InstantiateVerticalOuterWalls(rightXEdge, bottomZEdge, topZEdge);

        // Instantiate the outer wall tiles on the bottom and top of the dungeon
        InstantiateHorizontalOuterWalls(leftXEdge + 1f, rightXEdge - 1f, bottomZEdge);
        InstantiateHorizontalOuterWalls(leftXEdge + 1f, rightXEdge - 1f, topZEdge);
    }

    // Method for instantiating the vertical outer walls
    private void InstantiateVerticalOuterWalls(float xCoordinate, float startZ, float endZ)
    {
        for (float currentZ = startZ; currentZ <= endZ; currentZ++)
        {
            InstantiateFromArray(outerWallTiles, xCoordinate, currentZ);
        }
    }

    // Method for instantiating the horizontal outer walls
    private void InstantiateHorizontalOuterWalls(float startX, float endX, float zCoordinate)
    {
        for (float currentX = startX; currentX <= endX; currentX++)
        {
            InstantiateFromArray(outerWallTiles, currentX, zCoordinate);
        }
    }

    // Method for instantiating things from an array at a random index
    private void InstantiateFromArray(GameObject[] prefabs, float xCoordinate, float zCoordinate)
    {
        // Create the random index for the array
        int randomIndex = Random.Range(0, prefabs.Length);

        // Get the position for the object
        Vector3 position = new Vector3(xCoordinate + 0.5f, 0.35f, zCoordinate + 0.5f);

        // Create an instance of the object
        GameObject instance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

        // Put the tiles in de dungeonholder
        instance.transform.parent = dungeonHolder.transform;
    }
}
