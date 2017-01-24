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
               minCorridorLength = 8, maxCorridorLength = 14, minNumberOfEnemies = 2, maxNumberOfEnemies = 6, endPointMinimumDistanceToPlayer = 25;
    public GameObject endPoint, enemy, wallTile;                            // Normal gameobjects for the endpoint and enemies and walltile

    private TileType[][] tiles;
    private Vector3 playerPos = new Vector3(0.5f, 0.5f, 0.5f);              // Used to check the spawnpositions of other objects 
    private Vector3 endPointPos;
    private Dungeon_Room[] rooms;
    private Dungeon_Corridor[] corridors;
    private GameObject dungeonHolder, enemyHolder;
    private int numberOfEnemies, spawnedEnemies, amountOfWallsInSave, amountOfEnemiesInSave;
    private bool firstTimeCreatingDungeon, maySpawnAtPosition;
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

        string scene = SceneManager.GetActiveScene().name;
        if (scene == "Dungeon_PiPi")
        {
            firstTimeCreatingDungeon = PlayerPrefsManager.GetFirstTimePiPiDungeon();
            amountOfEnemiesInSave = PlayerPrefsManager.GetAmountOfEnemiesInPiPiDungeon();
            amountOfWallsInSave = PlayerPrefsManager.GetAmountOfWallsInPiPiDungeon();
        }
        else if (scene == "Dungeon_FaceBeer")
        {
            firstTimeCreatingDungeon = PlayerPrefsManager.GetFirstTimeFaceBeerDungeon();
            amountOfEnemiesInSave = PlayerPrefsManager.GetAmountOfEnemiesInFaceBeerDungeon();
            amountOfWallsInSave = PlayerPrefsManager.GetAmountOfWallsInFaceBeerDungeon();
        }

        if (firstTimeCreatingDungeon)
        {
            // Setup the maximum amount of enemies
            numberOfEnemies = Random.Range(minNumberOfEnemies, maxNumberOfEnemies + 1);
            spawnedEnemies = 0;

            // Make the dungeon
            SetupTilesArray();
            CreateRoomsAndCorridors();
            SetTilesInRooms();
            SetTilesInCorridors();
            InstantiateTiles();
            SpawnEndpoint();
            SpawnEnemies(numberOfEnemies);
            SaveLayoutOfWallsAndEndpoint();
            SaveLayoutOfEnemies();

            if (scene == "Dungeon_PiPi")
            {
                PlayerPrefsManager.SetFirstTimePiPiDungeon(0);
            }
            else if (scene == "Dungeon_FaceBeer")
            {
                PlayerPrefsManager.SetFirstTimeFaceBeerDungeon(0);
            }
        }
        else
        {
            // This is not the first time loading this dungeon, so we have to load the one we saved when we created it
            LoadLayout();
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            Reset();
        }
    }

    private void Reset()
    {
        SceneManager.LoadScene("Dungeon_FaceBeer");
    }

    #region Creating the Dungeon
    // Set up the tiles array to the correct size
    private void SetupTilesArray()
    {
        tiles = new TileType[columns][];

        for (int i = 0; i < tiles.Length; i++)
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
                    InstantiateFromArray(wallTile, i, j);
                }
            }
        }
    }

    // Method for instantiating things from an array at a random index
    private void InstantiateFromArray(GameObject prefab, float xCoordinate, float zCoordinate)
    {
        // Get the position for the object
        Vector3 position = new Vector3(3 * xCoordinate + 1.5f, -2f, 3 * zCoordinate + 1.5f);

        // Create an instance of the object
        GameObject instance = Instantiate(prefab, position, Quaternion.identity) as GameObject;

        // Put the tiles in de dungeonholder
        instance.transform.parent = dungeonHolder.transform;
    }
    #endregion

    #region Spawning GameObjects
    // Method that creates the enemies
    private void SpawnEnemies(int totalNumberOfEnemiesToSpawn)
    {
        spawnedEnemies = 0;
        while (spawnedEnemies < totalNumberOfEnemiesToSpawn)
        {
            // Spawn an enemy
            maySpawnAtPosition = true;

            // Create the position for the enemy
            float enemyXPos = Random.Range(0, columns * 3 - 1);
            float enemyZPos = Random.Range(0, rows * 3 - 1);
            enemyXPos -= enemyXPos % 3;
            enemyZPos -= enemyZPos % 3;
            Vector3 enemyPos = new Vector3(enemyXPos + 1.5f, 0.5f, enemyZPos + 1.5f);

            // Methods which checks the position of the enemy
            CheckForWallTiles(enemyPos);
            // If we can't spawn at this position, do not execute the rest of the methods
            if (maySpawnAtPosition)
            {
                CheckCorridors(enemyPos);
            }
            if (maySpawnAtPosition)
            {
                CheckBlockingOfCorridors(enemyPos);
            }
            if (maySpawnAtPosition)
            {
                CheckSpecialCases(enemyPos);
            }
            if (maySpawnAtPosition)
            {
                CheckDistanceToPlayer(enemyPos);
            }
            if (maySpawnAtPosition)
            {
                CheckDistanceToEndPoint(enemyPos);
            }

            if (maySpawnAtPosition)
            {
                // Actually spawn the enemy
                SpawnEnemy(enemyPos);
            }
        }
    }

    // Mehtod that spawns the endpoint
    private void SpawnEndpoint()
    {
        // Jump here if spawning the endpoint failed
        TrySpawningEndPointAgain:

        maySpawnAtPosition = true;
        float endPointXPos, endPointZPos;

        // Choose a random x coordinate
        endPointXPos = Random.Range(0, columns * 3 - 1);
        endPointXPos -= endPointXPos % 3;
        if (endPointXPos >= endPointMinimumDistanceToPlayer * 3)
        {
            // It is far away enough from the player to allow every z coordinate
            endPointZPos = Random.Range(0, rows * 3 - 1);
            endPointZPos -= endPointZPos % 3;
        }
        else
        {
            // Too close to the player to allow every z coordinate, it should be at least 25
            endPointZPos = Random.Range(endPointMinimumDistanceToPlayer * 3, rows * 3 - 1);
            endPointZPos -= endPointZPos % 3;
        }

        //endPointPos = new Vector3(Random.Range(0, columns) + 0.5f, 0.35f, Random.Range(0, rows) + 0.5f);
        endPointPos = new Vector3(endPointXPos + 1.5f, 0.5f, endPointZPos + 1.5f);

        // Check the position of the endpoint
        CheckForWallTiles(endPointPos);
        // If we can't spawn at this position, do not execute the rest of the methods
        if (maySpawnAtPosition)
        {
            CheckCorridors(endPointPos);
        }
        if (maySpawnAtPosition)
        {
            CheckBlockingOfCorridors(endPointPos);
        }
        if (maySpawnAtPosition)
        {
            CheckSpecialCases(endPointPos);
        }
        // The distance to the player and the endpoint does not have to be checked.
        // This is because the position is defined at a great distance to the player, and we are trying to spawn the endpoint.

        if (maySpawnAtPosition)
        {
            // Actually spawn the endpoint
            SpawnEndPoint(endPointPos);
        }
        else
        {
            goto TrySpawningEndPointAgain;
        }
    }

    // Checks if something will be spawned inside a wall
    private void CheckForWallTiles(Vector3 pos)
    {
        if (tiles[(int)(pos.x / 3)][(int)(pos.z / 3)] == TileType.Wall)
        {
            // Position is inside a wall
            maySpawnAtPosition = false;
            return;
        }
    }

    // Checks if something will be spawned inside a corridor
    private void CheckCorridors(Vector3 pos)
    {
        // Don't look at the first and last column, because those will be outside the array
        if (pos.x > 1 && pos.x < columns - 1)
        {
            // If the positions left and right of the possible spawnposition are walls, this position is in a corridor. So we can't spawn here
            if (tiles[(int)((pos.x - 3) / 3)][(int)(pos.z / 3)] == TileType.Wall && tiles[(int)((pos.x + 3)) / 3][(int)(pos.z / 3)] == TileType.Wall)
            {
                // In a horizontal corridor
                maySpawnAtPosition = false;
                return;
            }
        }

        // If the enemy is in the first column
        if (pos.x < 1)
        {
            // We only have to check the tiles to the left of the position, because to the right are the outerwalltiles
            if (tiles[(int)((pos.x + 3) / 3)][(int)(pos.z / 3)] == TileType.Wall)
            {
                // In a horizontal corridor in the first column
                maySpawnAtPosition = false;
                return;
            }
        }

        // If the enemy is in the last column
        if (pos.x > columns - 1)
        {
            // We only have to check the tiles to the right of the position, because to the left are the outerwalltiles
            if (tiles[(int)((pos.x - 3) / 3)][(int)(pos.z / 3)] == TileType.Wall)
            {
                // In a horizontal corridor in the last column
                maySpawnAtPosition = false;
                return;
            }
        }

        // Don't look at the first and last row, because those will be outside the array
        if (pos.z > 1 && pos.z < rows - 1)
        {
            // If the positions above and below the possible spawnposition are walls, this position is in a corridor. So we can't spawn here
            if (tiles[(int)(pos.x / 3)][(int)((pos.z - 3) / 3)] == TileType.Wall && tiles[(int)(pos.x / 3)][(int)((pos.z + 3) / 3)] == TileType.Wall)
            {
                // In vertical corridor
                maySpawnAtPosition = false;
                return;
            }
        }

        // If the enemy is in the first row
        if (pos.z < 1)
        {
            // We only have to check the tiles above the position, because the tiles below it are outerwalltiles
            if (tiles[(int)(pos.x / 3)][(int)((pos.z + 3) / 3)] == TileType.Wall)
            {
                // In a vertical corridor at the bottom
                maySpawnAtPosition = false;
                return;
            }
        }

        // If the enemy is in the last row
        if (pos.z > rows - 1)
        {
            // We only have to check the tiles below the position, because the tiles above it are outerwalltiles
            if (tiles[(int)(pos.x / 3)][(int)((pos.z - 3) / 3)] == TileType.Wall)
            {
                // In a vertical corridor at the top
                maySpawnAtPosition = false;
                return;
            }
        }
    }

    // Checks if something will block a corridor
    private void CheckBlockingOfCorridors(Vector3 pos)
    {
        // If the end of a corridor is at the first or last row of column, there is no place for a gameobject to spawn, so we won't check it
        if (pos.x > 3 && pos.z > 3 && pos.x < columns * 3 - 3 && pos.z < rows * 3 - 3)
        {
            // Left corridor. The position directly left of the given position should not be a wall, while the positions directly above and below the 
            // position to the left of the given position should be walls. This means the given position is directly to the right of a corridor.
            if (tiles[(int)((pos.x - 3) / 3)][(int)((pos.z + 3) / 3)] == TileType.Wall &&
                tiles[(int)((pos.x - 3) / 3)][(int)((pos.z - 3) / 3)] == TileType.Wall &&
                tiles[(int)((pos.x - 3) / 3)][(int)(pos.z / 3)] != TileType.Wall)
            {
                maySpawnAtPosition = false;
                return;
            }

            // Upper corridor. The position directly above the given position should not be a wall, while the positions directly to the left and 
            // right of the position above the given position should be walls. This means the given position is directly below a corridor.
            if (tiles[(int)((pos.x - 3) / 3)][(int)((pos.z + 3) / 3)] == TileType.Wall &&
                tiles[(int)((pos.x + 3) / 3)][(int)((pos.z + 3) / 3)] == TileType.Wall &&
                tiles[(int)(pos.x / 3)][(int)((pos.z + 3) / 3)] != TileType.Wall)
            {
                maySpawnAtPosition = false;
                return;
            }

            // Right corridor. The position directly right of the given position should not be a wall, while the positions directly above and below the 
            // position to the right of the given position should be walls. This means the given position is directly to the left of a corridor.
            if (tiles[(int)((pos.x + 3) / 3)][(int)((pos.z + 3) / 3)] == TileType.Wall &&
                tiles[(int)((pos.x + 3) / 3)][(int)((pos.z - 3) / 3)] == TileType.Wall &&
                tiles[(int)((pos.x + 3) / 3)][(int)(pos.z / 3)] != TileType.Wall)
            {
                maySpawnAtPosition = false;
                return;
            }

            // Down corridor. The position directly below the given position should not be a wall, while the positions directly to the left and 
            // right of the position below the given position should be walls. This means the given position is directly above of a corridor.
            if (tiles[(int)((pos.x - 3) / 3)][(int)((pos.z - 3) / 3)] == TileType.Wall &&
                tiles[(int)((pos.x + 3) / 3)][(int)((pos.z - 3) / 3)] == TileType.Wall &&
                tiles[(int)(pos.x / 3)][(int)((pos.z - 3) / 3)] != TileType.Wall)
            {
                maySpawnAtPosition = false;
                return;
            }
        }
    }

    // Checks some special cases
    private void CheckSpecialCases(Vector3 pos)
    {
        // If the given position is in the first or last row or column, we don't have to check these
        if (pos.x > 3 && pos.z > 3 && pos.x < columns * 3 - 3 && pos.z < rows * 3 - 3)
        {
            // These are some special cases where a gameobject would block something, which won't be checked in the methods above
            // For instance, 2 rooms with just 1 overlapping tile
            if (tiles[(int)((pos.x - 3) / 3)][(int)((pos.z + 3) / 3)] == TileType.Wall && tiles[(int)((pos.x + 3) / 3)][(int)((pos.z - 3) / 3)] == TileType.Wall)
            {
                // Meaning the tiles directly to the top left and the bottom right of the enemy
                maySpawnAtPosition = false;
                return;
            }
            if (tiles[(int)((pos.x + 3) / 3)][(int)((pos.z + 3) / 3)] == TileType.Wall && tiles[(int)((pos.x - 3) / 3)][(int)((pos.z - 3) / 3)] == TileType.Wall)
            {
                // Meaning the tiles directly to the top right and the bottom left of the enemy
                maySpawnAtPosition = false;
                return;
            }
        }

        // Or this case (draw it, it will be clear. It is quite hard to explain this situation)
        // The first if-statements make sure every possibility is checked, without getting IndexOutOfRangeException
        if (pos.x < columns * 3 - 3 && pos.z > 3 && pos.z < rows * 3 - 3)
        {
            if (tiles[(int)(pos.x / 3)][(int)((pos.z + 3) / 3)] == TileType.Wall && tiles[(int)((pos.x + 3) / 3)][(int)((pos.z - 3) / 3)] == TileType.Wall)
            {
                // Meaning the tiles directly above and to the bottom right of the given position
                maySpawnAtPosition = false;
                return;
            }

            if (tiles[(int)(pos.x / 3)][(int)((pos.z - 3) / 3)] == TileType.Wall && tiles[(int)((pos.x + 3) / 3)][(int)((pos.z + 3) / 3)] == TileType.Wall)
            {
                // Meaning the tiles directly below and to the top right of the given position
                maySpawnAtPosition = false;
                return;
            }
        }

        if (pos.x > 3 && pos.z > 3 && pos.z < rows * 3 - 3)
        {
            if (tiles[(int)(pos.x / 3)][(int)((pos.z + 3) / 3)] == TileType.Wall && tiles[(int)((pos.x - 3) / 3)][(int)((pos.z - 3) / 3)] == TileType.Wall)
            {
                // Meaning the tiles directly above and to the bottom left of the given position
                maySpawnAtPosition = false;
                return;
            }

            if (tiles[(int)(pos.x / 3)][(int)((pos.z - 3) / 3)] == TileType.Wall && tiles[(int)((pos.x - 3) / 3)][(int)((pos.z + 3) / 3)] == TileType.Wall)
            {
                // Meaning the tiles directly below and to the top left of the given position
                maySpawnAtPosition = false;
                return;
            }
        }

        if (pos.x > 3 && pos.x < columns * 3 - 3 && pos.z < rows * 3 - 3)
        {
            if (tiles[(int)((pos.x - 3) / 3)][(int)(pos.z / 3)] == TileType.Wall && tiles[(int)((pos.x + 3) / 3)][(int)((pos.z + 3) / 3)] == TileType.Wall)
            {
                // Meaning the tiles directly to the left and the top right of the given position
                maySpawnAtPosition = false;
                return;
            }

            if (tiles[(int)((pos.x - 3) / 3)][(int)((pos.z + 3) / 3)] == TileType.Wall && tiles[(int)((pos.x + 3) / 3)][(int)(pos.z / 3)] == TileType.Wall)
            {
                // Meaning the tiles directly to the right and the top left of the given position
                maySpawnAtPosition = false;
                return;
            }
        }

        if (pos.x > 3 && pos.x < columns * 3 - 3 && pos.z > 3)
        {
            if (tiles[(int)((pos.x - 3) / 3)][(int)(pos.z / 3)] == TileType.Wall && tiles[(int)((pos.x + 3) / 3)][(int)((pos.z - 3) / 3)] == TileType.Wall)
            {
                // Meaning the tiles directly to the left and the bottom right of the given position
                maySpawnAtPosition = false;
                return;
            }

            if (tiles[(int)((pos.x - 3) / 3)][(int)((pos.z - 3) / 3)] == TileType.Wall && tiles[(int)((pos.x + 3) / 3)][(int)(pos.z / 3)] == TileType.Wall)
            {
                // Meaning the tiles directly to the right and the bottom left of the given position
                maySpawnAtPosition = false;
                return;
            }
        }
    }

    // Checks if something is too close to the player
    private void CheckDistanceToPlayer(Vector3 pos)
    {
        // Loop through all the positions in a 3 × 3 grid around the given position
        for (int x = -9; x <= 9; x++)
        {
            for (int z = -9; z <= 9; z++)
            {
                Vector3 position = new Vector3(pos.x + x, playerPos.y, pos.z + z);

                // If the position of the player is somewhere in this little grid
                if (position == playerPos)
                {
                    // Do not spawn gameobject here, because it is too close to the player
                    maySpawnAtPosition = false;
                    return;
                }
            }
        }
    }

    // Checks if something is too close to the endpoint
    private void CheckDistanceToEndPoint(Vector3 pos)
    {
        // Loop through all the positions in a 2 × 2 grid around the given position
        for (int x = -6; x <= 6; x++)
        {
            for (int z = -6; z <= 6; z++)
            {
                Vector3 position = new Vector3(pos.x + x, endPointPos.y, pos.z + z);

                // If the position of the endpoint is somewhere in this little grid
                if (position == endPointPos)
                {
                    // Do not spawn gameobject here, because it is too close to the endpoint
                    maySpawnAtPosition = false;
                    return;
                }
            }
        }
    }

    // Spawns an enemy
    private void SpawnEnemy(Vector3 pos)
    {
        GameObject enemyClone = Instantiate(enemy, pos, Quaternion.identity) as GameObject;
        enemyClone.transform.parent = enemyHolder.transform;
        spawnedEnemies++;
    }

    // Spawns the endpoint
    private void SpawnEndPoint(Vector3 pos)
    {
        Instantiate(endPoint, pos, Quaternion.identity);
    }
    #endregion

    // Saves the positions of the walls and the endpoint in a scene
    private void SaveLayoutOfWallsAndEndpoint()
    {
        // Set the active scene to a string
        string scene = SceneManager.GetActiveScene().name;

        // Store the position of each wall
        int indexOfWalls = 0;
        foreach (Transform wall in dungeonHolder.transform)
        {
            // Loop through all the walls in the dungeonHolder and store the X and Z coordinates. The Y has a constant value, so we don't save this
            PlayerPrefs.SetFloat(scene + "_wall_" + indexOfWalls + "_x", wall.position.x);
            PlayerPrefs.SetFloat(scene + "_wall_" + indexOfWalls + "_z", wall.position.z);
            indexOfWalls++;
            amountOfWallsInSave++;
        }

        // Depending on the scene, store the values on another location
        if (scene == "Dungeon_PiPi")
        {
            // Store the position of the endpoint and the amount of walls in the scene
            PlayerPrefsManager.SetEndPointPosPiPiDungeon(endPointPos);
            PlayerPrefsManager.SetAmountOfWallsInPiPiDungeon(amountOfWallsInSave);
        }
        else if (scene == "Dungeon_FaceBeer")
        {
            // Store the position of the endpoint and the amount of walls in the walls
            PlayerPrefsManager.SetEndPointPosFaceBeerDungeon(endPointPos);
            PlayerPrefsManager.SetAmountOfWallsInFaceBeerDungeon(amountOfWallsInSave);
        }
    }

    // Saves the locations of the enemies. Different method, because it will probably be used in another script
    private void SaveLayoutOfEnemies()
    {
        // Store the active scene in a string
        string scene = SceneManager.GetActiveScene().name;

        // Store the position of each enemy
        int indexOfEnemies = 0;
        foreach (Transform enemy in enemyHolder.transform)
        {
            // Loop through each enemy in enemyHolder and store its X and Z coordinates. The Y has a constant value, so we don't store this.
            PlayerPrefs.SetFloat(scene + "_enemy_" + indexOfEnemies + "_x", enemy.position.x);
            PlayerPrefs.SetFloat(scene + "_enemy_" + indexOfEnemies + "_z", enemy.position.z);
            indexOfEnemies++;
            amountOfEnemiesInSave++;
        }

        // Depending on the scene, store the value on another location
        if (scene == "Dungeon_PiPi")
        {
            // Store the amount of enemies
            PlayerPrefsManager.SetAmountOfEnemiesInPiPiDungeon(amountOfEnemiesInSave);
        }
        else if (scene == "Dungeon_FaceBeer")
        {
            // Store the amount of enemies
            PlayerPrefsManager.SetAmountOfEnemiesInFaceBeerDungeon(amountOfEnemiesInSave);
        }
    }

    // Load the dungeon again in the same layout
    private void LoadLayout()
    {
        // Store the active scene in a string
        string scene = SceneManager.GetActiveScene().name;

        // Loop through all the walls and get their coordinates back. Then instantiate it.
        for (int w = 0; w < amountOfWallsInSave; w++)
        {
            float x = PlayerPrefs.GetFloat(scene + "_wall_" + w + "_x");
            float y = -2.0f;
            float z = PlayerPrefs.GetFloat(scene + "_wall_" + w + "_z");
            Vector3 pos = new Vector3(x, y, z);
            GameObject wallClone = Instantiate(wallTile, pos, Quaternion.identity) as GameObject;

            wallClone.transform.parent = dungeonHolder.transform;
        }

        // Loop through all the enemies and get their coordinates. Then instantiatie it.
        for (int e = 0; e < amountOfEnemiesInSave; e++)
        {
            float x = PlayerPrefs.GetFloat(scene + "_enemy_" + e + "_x");
            float y = 0.5f;
            float z = PlayerPrefs.GetFloat(scene + "_enemy_" + e + "_z");
            Vector3 pos = new Vector3(x, y, z);
            GameObject enemyClone = Instantiate(enemy, pos, Quaternion.identity) as GameObject;

            enemyClone.transform.parent = enemyHolder.transform;
        }

        // Get the position of the endpoint and instantiate it
        Vector3 _pos = Vector3.zero;
        if (scene == "Dungeon_PiPi")
        {
            _pos = PlayerPrefsManager.GetEndPointPosPiPiDungeon();
        }
        else if (scene == "Dungeon_FaceBeer")
        {
            _pos = PlayerPrefsManager.GetEndPointPosFaceBeerDungeon();
        }
        Instantiate(endPoint, _pos, Quaternion.identity);
    }
}