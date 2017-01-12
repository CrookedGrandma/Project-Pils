using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonCreator : MonoBehaviour
{
    #region Variable Declaration
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
    private Vector3 playerPos, endPointPos;
    private Dungeon_Room[] rooms;
    private Dungeon_Corridor[] corridors;
    private GameObject dungeonHolder, enemyHolder;
    private int numberOfEnemies, spawnedEnemies;
    private bool maySpawnEnemyAtPosition;
    #endregion

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
        spawnedEnemies = 0;

        // Make the dungeon
        SetupTilesArray();
        CreateRoomsAndCorridors();
        SetTilesInRooms();
        SetTilesInCorridors();
        InstantiateTiles();
        InstantiateOuterWalls();
        SpawnEnemies(numberOfEnemies);
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

    #region Creating the Dungeon
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
        playerPos = new Vector3(rooms[0].xPos + 0.5f, 0.35f, rooms[0].zPos + 0.5f);
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
                endPointPos = new Vector3(rooms[i].xPos + rooms[i].roomWidth / 2 - 0.5f, 0.35f, rooms[i].zPos + rooms[i].roomHeight / 2 - 0.5f);
                
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
    #endregion

    #region Spawning the Enemies
    // Method that creates the enemies
    private void SpawnEnemies(int totalNumberOfEnemiesToSpawn)
    {
        spawnedEnemies = 0;
        while (spawnedEnemies < totalNumberOfEnemiesToSpawn)
        {
            // Spawn an enemy
            maySpawnEnemyAtPosition = true;
            Vector3 enemyPos = new Vector3(Random.Range(0, columns) + 0.5f, 0.35f, Random.Range(0, rows) + 0.5f);

            // Methods which check the position of the enemy
            CheckForWallTiles(enemyPos);
            CheckCorridors(enemyPos);
            CheckBlockingOfCorridors(enemyPos);
            CheckSpecialCases(enemyPos);
            CheckDistanceToPlayer(enemyPos);
            CheckDistanceToEndPoint(enemyPos);

            if (maySpawnEnemyAtPosition)
            {
                // Actually spawn the enemy
                SpawnEnemy(enemyPos);
            }
        }
    }

    // Checks if the enemy will be spawned inside a wall
    private void CheckForWallTiles(Vector3 enemyPos)
    {
        if (tiles[(int)(enemyPos.x)][(int)(enemyPos.z)] == TileType.Wall)
        {
            // Enemyposition is inside a wall
            maySpawnEnemyAtPosition = false;
            return;
        }
    }

    // Checks if the enemy will be spawned inside a corridor
    private void CheckCorridors(Vector3 enemyPos)
    {
        // Don't look at the first and last column, because those will be outside the array
        if (enemyPos.x > 1 && enemyPos.x < columns - 1)
        {
            // If the positions left and right of the possible spawnposition are walls, this position is in a corridor. So we can't spawn here
            if (tiles[(int)(enemyPos.x - 1)][(int)(enemyPos.z)] == TileType.Wall && tiles[(int)(enemyPos.x + 1)][(int)(enemyPos.z)] == TileType.Wall)
            {
                // In a horizontal corridor
                maySpawnEnemyAtPosition = false;
                return;
            }
        }

        // If the enemy is in the first column
        if (enemyPos.x < 1)
        {
            // We only have to check the tiles to the left of the enemyposition, because to the right are the outerwalltiles
            if (tiles[(int)(enemyPos.x + 1)][(int)(enemyPos.z)] == TileType.Wall)
            {
                // In a horizontal corridor in the first column
                maySpawnEnemyAtPosition = false;
                return;
            }
        }

        // If the enemy is in the last column
        if (enemyPos.x > columns - 1)
        {
            // We only have to check the tiles to the right of the enemyposition, because to the left are the outerwalltiles
            if (tiles[(int)(enemyPos.x - 1)][(int)(enemyPos.z)] == TileType.Wall)
            {
                // In a horizontal corridor in the last column
                maySpawnEnemyAtPosition = false;
                return;
            }
        }

        // Don't look at the first and last row, because those will be outside the array
        if (enemyPos.z > 1 && enemyPos.z < rows - 1)
        {
            // If the positions above and below the possible spawnposition are walls, this position is in a corridor. So we can't spawn here
            if (tiles[(int)(enemyPos.x)][(int)(enemyPos.z - 1)] == TileType.Wall && tiles[(int)(enemyPos.x)][(int)(enemyPos.z + 1)] == TileType.Wall)
            {
                // In vertical corridor
                maySpawnEnemyAtPosition = false;
                return;
            }
        }

        // If the enemy is in the first row
        if (enemyPos.z < 1)
        {
            // We only have to check the tiles above the enemyposition, because the tiles below it are outerwalltiles
            if (tiles[(int)(enemyPos.x)][(int)(enemyPos.z + 1)] == TileType.Wall)
            {
                // In a vertical corridor at the bottom
                maySpawnEnemyAtPosition = false;
                return;
            }
        }

        // If the enemy is in the last row
        if (enemyPos.z > rows - 1)
        {
            // We only have to check the tiles below the enemyposition, because the tiles above it are outerwalltiles
            if (tiles[(int)(enemyPos.x)][(int)(enemyPos.z - 1)] == TileType.Wall)
            {
                // In a vertical corridor at the top
                maySpawnEnemyAtPosition = false;
                return;
            }
        }
    }

    // Checks if the enemy will block a corridor
    private void CheckBlockingOfCorridors(Vector3 enemyPos)
    {
        // If the end of a corridor is at the first or last row of column, there is no place for an enemy to spawn, so we won't check it
        if (enemyPos.x > 1 && enemyPos.z > 1 && enemyPos.x < columns - 1 && enemyPos.z < rows - 1)
        {
            // Left corridor. The position directly left of the enemyposition should not be a wall, while the positions directly above and below the 
            // position to the left of the enemyposition should be walls. This means the player is directly to the right of a corridor.
            if (tiles[(int)(enemyPos.x - 1)][(int)(enemyPos.z + 1)] == TileType.Wall &&
                tiles[(int)(enemyPos.x - 1)][(int)(enemyPos.z - 1)] == TileType.Wall &&
                tiles[(int)(enemyPos.x - 1)][(int)(enemyPos.z)] != TileType.Wall)
            {
                maySpawnEnemyAtPosition = false;
                return;
            }

            // Upper corridor. The position directly above the enemyposition should not be a wall, while the positions directly to the left and 
            // right of the position above the enemyposition should be walls. This means the player is directly below a corridor.
            if (tiles[(int)(enemyPos.x - 1)][(int)(enemyPos.z + 1)] == TileType.Wall &&
                tiles[(int)(enemyPos.x + 1)][(int)(enemyPos.z + 1)] == TileType.Wall &&
                tiles[(int)(enemyPos.x)][(int)(enemyPos.z + 1)] != TileType.Wall)
            {
                maySpawnEnemyAtPosition = false;
                return;
            }

            // Right corridor. The position directly right of the enemyposition should not be a wall, while the positions directly above and below the 
            // position to the right of the enemyposition should be walls. This means the player is directly to the left of a corridor.
            if (tiles[(int)(enemyPos.x + 1)][(int)(enemyPos.z + 1)] == TileType.Wall &&
                tiles[(int)(enemyPos.x + 1)][(int)(enemyPos.z - 1)] == TileType.Wall &&
                tiles[(int)(enemyPos.x + 1)][(int)(enemyPos.z)] != TileType.Wall)
            {
                maySpawnEnemyAtPosition = false;
                return;
            }

            // Down corridor. The position directly below the enemyposition should not be a wall, while the positions directly to the left and 
            // right of the position below the enemyposition should be walls. This means the player is directly above of a corridor.
            if (tiles[(int)(enemyPos.x - 1)][(int)(enemyPos.z - 1)] == TileType.Wall &&
                tiles[(int)(enemyPos.x + 1)][(int)(enemyPos.z - 1)] == TileType.Wall &&
                tiles[(int)(enemyPos.x)][(int)(enemyPos.z - 1)] != TileType.Wall)
            {
                maySpawnEnemyAtPosition = false;
                return;
            }
        }
    }

    // Checks some special cases
    private void CheckSpecialCases(Vector3 enemyPos)
    {
        // If the enemyposition is in the first or last row or column, we don't have to check these
        if (enemyPos.x > 1 && enemyPos.z > 1 && enemyPos.x < columns - 1 && enemyPos.z < rows - 1)
        {
            // These are some special cases where an enemy would block something, which won't be checked in the methods above
            // For instance, 2 rooms with just 1 overlapping tile
            if (tiles[(int)(enemyPos.x - 1)][(int)(enemyPos.z + 1)] == TileType.Wall && tiles[(int)(enemyPos.x + 1)][(int)(enemyPos.z - 1)] == TileType.Wall)
            {
                // Meaning the tiles directly to the top left and the bottom right of the enemy
                maySpawnEnemyAtPosition = false;
                return;
            }
            if (tiles[(int)(enemyPos.x + 1)][(int)(enemyPos.z + 1)] == TileType.Wall && tiles[(int)(enemyPos.x - 1)][(int)(enemyPos.z - 1)] == TileType.Wall)
            {
                // Meaning the tiles directly to the top right and the bottom left of the enemy
                maySpawnEnemyAtPosition = false;
                return;
            }
        }

        // Or this case (draw it, it will be clear. It is quite hard to explain this situation)
        // The first if-statements make sure every possibility is checked, without getting IndexOutOfRangeException
        if (enemyPos.x < columns - 1 && enemyPos.z > 1 && enemyPos.z < rows - 1)
        {
            if (tiles[(int)(enemyPos.x)][(int)(enemyPos.z + 1)] == TileType.Wall && tiles[(int)(enemyPos.x + 1)][(int)(enemyPos.z - 1)] == TileType.Wall)
            {
                // Meaning the tiles directly above and to the bottom right of the enemy
                maySpawnEnemyAtPosition = false;
                return;
            }

            if (tiles[(int)(enemyPos.x)][(int)(enemyPos.z - 1)] == TileType.Wall && tiles[(int)(enemyPos.x + 1)][(int)(enemyPos.z + 1)] == TileType.Wall)
            {
                // Meaning the tiles directly below and to the top right of the enemy
                maySpawnEnemyAtPosition = false;
                return;
            }
        }

        if (enemyPos.x > 1 && enemyPos.z > 1 && enemyPos.z < rows - 1)
        {
            if (tiles[(int)(enemyPos.x)][(int)(enemyPos.z + 1)] == TileType.Wall && tiles[(int)(enemyPos.x - 1)][(int)(enemyPos.z - 1)] == TileType.Wall)
            {
                // Meaning the tiles directly above and to the bottom left of the enemy
                maySpawnEnemyAtPosition = false;
                return;
            }

            if (tiles[(int)(enemyPos.x)][(int)(enemyPos.z - 1)] == TileType.Wall && tiles[(int)(enemyPos.x - 1)][(int)(enemyPos.z + 1)] == TileType.Wall)
            {
                // Meaning the tiles directly below and to the top left of the enemy
                maySpawnEnemyAtPosition = false;
                return;
            }
        }

        if (enemyPos.x > 1 && enemyPos.x < columns - 1 && enemyPos.z < rows - 1)
        {
            if (tiles[(int)(enemyPos.x - 1)][(int)(enemyPos.z)] == TileType.Wall && tiles[(int)(enemyPos.x + 1)][(int)(enemyPos.z + 1)] == TileType.Wall)
            {
                // Meaning the tiles directly to the left and the top right of the enemy
                maySpawnEnemyAtPosition = false;
                return;
            }

            if (tiles[(int)(enemyPos.x - 1)][(int)(enemyPos.z + 1)] == TileType.Wall && tiles[(int)(enemyPos.x + 1)][(int)(enemyPos.z)] == TileType.Wall)
            {
                // Meaning the tiles directly to the right and the top left of the enemy
                maySpawnEnemyAtPosition = false;
                return;
            }
        }

        if (enemyPos.x > 1 && enemyPos.x < columns - 1 && enemyPos.z > 1)
        {
            if (tiles[(int)(enemyPos.x - 1)][(int)(enemyPos.z)] == TileType.Wall && tiles[(int)(enemyPos.x + 1)][(int)(enemyPos.z - 1)] == TileType.Wall)
            {
                // Meaning the tiles directly to the left and the bottom right of the enemy
                maySpawnEnemyAtPosition = false;
                return;
            }

            if (tiles[(int)(enemyPos.x - 1)][(int)(enemyPos.z - 1)] == TileType.Wall && tiles[(int)(enemyPos.x + 1)][(int)(enemyPos.z)] == TileType.Wall)
            {
                // Meaning the tiles directly to the right and the bottom left of the enemy
                maySpawnEnemyAtPosition = false;
                return;
            }
        }
    }

    // Checks if the enemy is too close to the player
    private void CheckDistanceToPlayer(Vector3 enemyPos)
    {
        // Loop through all the positions in a 3 × 3 grid around the enemy
        for (int x = -3; x <= 3; x++)
        {
            for (int z = -3; z <= 3; z++)
            {
                Vector3 pos = new Vector3(enemyPos.x + x, 0.35f, enemyPos.z + z);

                // If it the position of the player is somewhere in this little grid
                if (pos == playerPos)
                {
                    // Do not spawn enemy here, because he is too close to the player
                    maySpawnEnemyAtPosition = false;
                    return;
                }
            }
        }
    }

    // Checks if the enemy is too close to the endpoint
    private void CheckDistanceToEndPoint(Vector3 enemyPos)
    {
        // Loop through all the positions in a 2 × 2 grid around the enemy
        for (int x = -2; x <= 2; x++)
        {
            for (int z = -2; z <= 2; z++)
            {
                Vector3 pos = new Vector3(enemyPos.x + x, 0.35f, enemyPos.z + z);

                // If it the position of the endpoint is somewhere in this little grid
                if (pos == endPointPos)
                {
                    // Do not spawn enemy here, because he is too close to the endpoint
                    maySpawnEnemyAtPosition = false;
                    return;
                }
            }
        }
    }

    // Spawns an enemy
    private void SpawnEnemy(Vector3 enemyPos)
    {
        GameObject enemyClone = Instantiate(enemy, enemyPos, Quaternion.identity) as GameObject;
        enemyClone.transform.parent = enemyHolder.transform;
        spawnedEnemies++;
    }
    #endregion
}
