using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    Vector2 worldSize = new Vector2(4, 4);
    
    Room[,] rooms;
    List<Vector2> takenPositions = new List<Vector2>();
    public int numberOfRooms = 15;
    int gridSizeX, gridSizeY;
    public GameObject floorTile;
    public GameObject doorTile;
    public GameObject stairTile;
    public GameObject player;
    public GameObject thinWall;
    public GameObject wall;
    public GameObject formation;
    public GameObject e_position;
    public GameObject boss;
    public GameObject[] enemies;
    public GameObject[] presets1;       //1 door
    public GameObject[] presets2;       //2 door across
    public GameObject[] presets3;       //2 door adjacent
    public GameObject[] presets4;       //3 doors
    public GameObject[] presets5;       //4 doors
    public LevelManager level_manager;

    void Start()
    {
        gridSizeX = numberOfRooms;
        gridSizeY = numberOfRooms;
        if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2))
        { // make sure we dont try to make more rooms than can fit in our grid
            numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
        }
        gridSizeX = Mathf.RoundToInt(worldSize.x); //note: these are half-extents
        gridSizeY = Mathf.RoundToInt(worldSize.y);
        CreateRooms(); //lays out the actual map
                       //SetRoomDoors(); //assigns the doors where rooms would connect
        DrawMap(); //instantiates objects to make up a map
        GameObject slime = Instantiate(player, rooms[gridSizeX, gridSizeY].gridPos * 1.28f, Quaternion.identity);
    }

    private void Update()
    {
        if (!FindObjectOfType<PlayerEntity>())
            level_manager.GetComponent<LevelManager>().LoadLevel("Game Over");
    }

    void CreateRooms()
    {
        //setup
        rooms = new Room[gridSizeX * 2, gridSizeY * 2];
        rooms[gridSizeX, gridSizeY] = new Room(Vector2.zero, -1);
        takenPositions.Insert(0, Vector2.zero);
        Vector2 checkPos = Vector2.zero;
        //magic numbers
        float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;
        //add rooms
        for (int i = 0; i < numberOfRooms - 1; i++)
        {
            float randomPerc = ((float)i) / (((float)numberOfRooms - 1));
            randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
            //grab new position
            checkPos = NewPosition();
            //test new position
            if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare)
            {
                int iterations = 0;
                do
                {
                    checkPos = SelectiveNewPosition();
                    iterations++;
                } while (NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);
                if (iterations >= 50)
                    print("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, takenPositions));
            }
            //finalize position

            rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = new Room(checkPos, 0);

            takenPositions.Insert(0, checkPos);
        }

        //set boss room = to first dead end found that isnt spawn
        for (int i = 0; i < takenPositions.Count - 1; i++)
        {
            if (NumberOfNeighbors(takenPositions[i], takenPositions) == 1)
            {
                rooms[(int)takenPositions[i].x + gridSizeX, (int)takenPositions[i].y + gridSizeY].type = 1;
                break;
            }
        }

    }

    Vector2 NewPosition()
    {
        int x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;
        do
        {
            int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1)); // pick a random room
            x = (int)takenPositions[index].x;//capture its x, y position
            y = (int)takenPositions[index].y;
            bool UpDown = (Random.value < 0.5f);//randomly pick wether to look on hor or vert axis
            bool positive = (Random.value < 0.5f);//pick whether to be positive or negative on that axis
            if (UpDown)
            { //find the position bnased on the above bools
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }
            checkingPos = new Vector2(x, y);
        } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY); //make sure the position is valid
        return checkingPos;
    }

    Vector2 SelectiveNewPosition()
    { // method differs from the above in the two commented ways
        int index = 0, inc = 0;
        int x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;
        do
        {
            inc = 0;
            do
            {
                //instead of getting a room to find an adject empty space, we start with one that only 
                //as one neighbor. This will make it more likely that it returns a room that branches out
                index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
                inc++;
            } while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);
            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;
            bool UpDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);
            if (UpDown)
            {
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }
            checkingPos = new Vector2(x, y);
        } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
        if (inc >= 100)
        { // break loop if it takes too long: this loop isnt garuanteed to find solution, which is fine for this
            print("Error: could not find position with only one neighbor");
        }
        return checkingPos;
    }

    int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions)
    {
        int ret = 0; // start at zero, add 1 for each side there is already a room
        if (usedPositions.Contains(checkingPos + Vector2.right))
        { //using Vector.[direction] as short hands, for simplicity
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.left))
        {
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.up))
        {
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.down))
        {
            ret++;
        }
        return ret;
    }

    void DrawMap()
    {
        foreach (Room room in rooms)
        {
            if (room == null)
            {
                continue; //skip where there is no room
            }
            Vector2 drawPos = room.gridPos;
            drawPos.x *= 1.28f;//aspect ratio of map sprite
            drawPos.y *= 1.28f;
            GameObject tile = Instantiate(floorTile, drawPos, Quaternion.identity);
            SetRoomDoorsAndWalls(room);
        }
    }

    void SetRoomDoorsAndWalls(Room room)
    {
        Vector2 drawPos = room.gridPos;
        drawPos.x *= 1.28f;//aspect ratio of map sprite
        drawPos.y *= 1.28f;
        int numDoors = 0;
        bool North = false;
        bool East = false;
        bool South = false;
        bool West = false;

        GameObject door0 = null, door1 = null, door2 = null, door3 = null, stairs = null;
        GameObject formation0 = null;
        int stairlocation = 4;
        //---------------------------------------------------SET WALLS/DOORS--------------------------------------------------------
        if (takenPositions.Contains(room.gridPos + Vector2.right))
        { //using Vector.[direction] as short hands, for simplicity
            door0 = Instantiate(doorTile, new Vector3(drawPos.x + .51f, drawPos.y, 0), Quaternion.Euler(0, 0, -90f));
            //door0.GetComponent<Doors>().mobs = formation0.GetComponent<EnemySpawner>();
            GameObject thinwall0 = Instantiate(thinWall, new Vector3(drawPos.x + .64f, drawPos.y, 0), Quaternion.identity);
            thinwall0.layer = 12;
            numDoors++;
            East = true;
        }
        else
        {
            GameObject wall0 = Instantiate(wall, new Vector3(drawPos.x + 1.28f, drawPos.y, 0), Quaternion.identity);
            wall0.layer = 12;
        }
        if (takenPositions.Contains(room.gridPos + Vector2.left))
        {
            door2 = Instantiate(doorTile, new Vector3(drawPos.x - .51f, drawPos.y, 0), Quaternion.Euler(0, 0, 90f));
            //door2.GetComponent<Doors>().mobs = formation0.GetComponent<EnemySpawner>();
            GameObject thinwall2 = Instantiate(thinWall, new Vector3(drawPos.x - .64f, drawPos.y, 0), Quaternion.identity);
            thinwall2.layer = 12;
            numDoors++;
            West = true;
        }
        else
        {
            GameObject wall2 = Instantiate(wall, new Vector3(drawPos.x - 1.28f, drawPos.y, 0), Quaternion.identity);
            wall2.layer = 12;
        }
        if (takenPositions.Contains(room.gridPos + Vector2.up))
        {
            door1 = Instantiate(doorTile, new Vector3(drawPos.x, drawPos.y + .51f, 0), Quaternion.Euler(0, 0, 0));
            //door1.GetComponent<Doors>().mobs = formation0.GetComponent<EnemySpawner>();
            GameObject thinwall1 = Instantiate(thinWall, new Vector3(drawPos.x, drawPos.y + .64f, 0), Quaternion.Euler(0, 0, 90));
            thinwall1.layer = 12;
            numDoors++;
            North = true;
        }
        else
        {
            GameObject wall1 = Instantiate(wall, new Vector3(drawPos.x, drawPos.y + 1.28f, 0), Quaternion.identity);
            wall1.layer = 12;
        }
        if (takenPositions.Contains(room.gridPos + Vector2.down))
        {
            door3 = Instantiate(doorTile, new Vector3(drawPos.x, drawPos.y - .51f, 0), Quaternion.Euler(0, 0, 180));
            //door3.GetComponent<Doors>().mobs = formation0.GetComponent<EnemySpawner>();
            GameObject thinwall3 = Instantiate(thinWall, new Vector3(drawPos.x, drawPos.y - .64f, 0), Quaternion.Euler(0, 0, 90));
            thinwall3.layer = 12;
            numDoors++;
            South = true;
        }
        else
        {
            GameObject wall3 = Instantiate(wall, new Vector3(drawPos.x, drawPos.y - 1.28f, 0), Quaternion.identity);
            wall3.layer = 12;
        }
        if (room.type == 1)
        {
            if (takenPositions.Contains(room.gridPos + Vector2.right))
            {
                stairs = Instantiate(stairTile, new Vector3(drawPos.x - .51f, drawPos.y, 0), Quaternion.Euler(0, 0, 90f));
                stairlocation = 2;
                //stairs.GetComponent<Doors>().mobs = formation0.GetComponent<EnemySpawner>();
            }
            else if (takenPositions.Contains(room.gridPos + Vector2.left))
            {
                stairs = Instantiate(stairTile, new Vector3(drawPos.x + .51f, drawPos.y, 0), Quaternion.Euler(0, 0, -90f));
                stairlocation = 0;
                //stairs.GetComponent<Doors>().mobs = formation0.GetComponent<EnemySpawner>();
            }
            else if (takenPositions.Contains(room.gridPos + Vector2.up))
            {
                stairs = Instantiate(stairTile, new Vector3(drawPos.x, drawPos.y - .51f, 0), Quaternion.Euler(0, 0, 180));
                stairlocation = 3;
                //stairs.GetComponent<Doors>().mobs = formation0.GetComponent<EnemySpawner>();
            }
            else if (takenPositions.Contains(room.gridPos + Vector2.down))
            {
                stairs = Instantiate(stairTile, new Vector3(drawPos.x, drawPos.y + .51f, 0), Quaternion.Euler(0, 0, 0));
                stairlocation = 1;
                //stairs.GetComponent<Doors>().mobs = formation0.GetComponent<EnemySpawner>();
            }
        }
        //----------------------------------------------SET PRESET OR ENEMY FORMATION-------------------------------------------------
        // = Instantiate(formation, drawPos, Quaternion.identity);
        /**
         * -1 = empty
         * 0 = random spawn [0-4]
         * 1 = boss
         * 
        */
        if (room.type == -1)
        {

            //make an enemy formation for it
            formation0 = Instantiate(formation, drawPos, Quaternion.identity);
            //with random number of enemies with random positions inside the room
        }
        //if regular room 
        else if (room.type == 0)
        {
            GameObject preset = null;
            int ran = Random.Range(0, 2);
            if (ran == 1)           //NOT PRESET
            {
                //make an enemy formation for it
                formation0 = Instantiate(formation, drawPos, Quaternion.identity);
                //with random number of enemies with random positions inside the room
                int num_enemies = Random.Range(0, 4);
                for (int i = 0; i < num_enemies; i++)
                {
                    float x_pos = Random.Range(drawPos.x - .35f, drawPos.x + .35f);
                    float y_pos = Random.Range(drawPos.y - .35f, drawPos.y + .35f);
                    GameObject e_position0 = Instantiate(e_position, new Vector3(x_pos, y_pos, 0), Quaternion.identity);
                    e_position0.transform.parent = formation0.transform;
                    int random_enemy = Random.Range(0, enemies.Length-1);
                    e_position0.GetComponent<Position>().enemy = enemies;
                }
            }
            else                    //PRESET
            {
                //check room type and pull from appropriate array
                
                if(numDoors == 1)
                {
                    preset = Instantiate(presets1[Random.Range(0, presets1.Length)], drawPos, Quaternion.identity);
                    if(North)
                        preset.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                    else if(East)
                        preset.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    else if(West)
                        preset.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                }
                else if (numDoors == 2)
                {
                    if ((North && South) || (West && East))
                    {
                        preset = Instantiate(presets2[Random.Range(0, presets2.Length)], drawPos, Quaternion.identity);
                        if (North)
                            preset.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                        
                    }
                    else
                    {
                        preset = Instantiate(presets3[Random.Range(0, presets3.Length)], drawPos, Quaternion.identity);
                        if (South && West)
                        {
                            preset.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                        }
                        else if (North)
                        {
                            if (East)
                                preset.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                            else if (West)
                                preset.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                        }
                    }

                }
                else if(numDoors == 3)
                {
                    preset = Instantiate(presets4[Random.Range(0, presets4.Length)], drawPos, Quaternion.identity);
                    if(!North)
                        preset.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    else if(!West)
                        preset.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                    else if(!South)
                        preset.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                }
                else
                {
                    preset = Instantiate(presets5[Random.Range(0, presets5.Length)], drawPos, Quaternion.identity);
                }
                foreach (Transform child in preset.gameObject.transform)
                {
                    if (child.gameObject.GetComponent<EnemySpawner>() != null)
                    {
                        formation0 = child.gameObject;
                    }
                }

            }
        }
        //if boss
        else if (room.type == 1)
        {
            //make an enemy formation for it
            formation0 = Instantiate(formation, drawPos, Quaternion.identity);
            int num_enemies = 1;

            for (int i = 0; i < num_enemies; i++)
            {
                float x_pos = drawPos.x;
                float y_pos = drawPos.y;
                GameObject e_position0 = Instantiate(e_position, new Vector3(x_pos, y_pos, 0), Quaternion.identity);
                e_position0.transform.parent = formation0.transform;
                e_position0.GetComponent<Position>().enemy[0] = boss;
            }
        }
        //------------------------------LINKING DOORS AND MOBS--------------------------------------------------
        if (formation0 != null)
        {
            if (door0 != null)
                formation0.GetComponent<EnemySpawner>().doors[0] = door0;
                //door0.GetComponent<Doors>().mobs = formation0.GetComponent<EnemySpawner>();
            if (door1 != null)
                formation0.GetComponent<EnemySpawner>().doors[1] = door1;
            //door1.GetComponent<Doors>().mobs = formation0.GetComponent<EnemySpawner>();
            if (door2 != null)
                formation0.GetComponent<EnemySpawner>().doors[2] = door2;
            //door2.GetComponent<Doors>().mobs = formation0.GetComponent<EnemySpawner>();
            if (door3 != null)
                formation0.GetComponent<EnemySpawner>().doors[3] = door3;
            //door3.GetComponent<Doors>().mobs = formation0.GetComponent<EnemySpawner>();
            if (stairlocation != 4)
            {
                formation0.GetComponent<EnemySpawner>().doors[stairlocation] = stairs;
            }
        }
        
        /*
        setWalls(room, formation0, drawPos);

        setRoom(room, drawPos);
        */
    }

    
    
    /*-----------------------------------------------------OUTDATED-----------------------------------------------------
    void setRoom(Room room, Vector2 drawPos)
    {
        GameObject formation0;// = Instantiate(formation, drawPos, Quaternion.identity);

        if (room.type == -1)
        {
            
            //make an enemy formation for it
            formation0 = Instantiate(formation, drawPos, Quaternion.identity);
            //with random number of enemies with random positions inside the room
        }
        //if regular room 
        else if (room.type == 0)
        {

            //make an enemy formation for it
            formation0 = Instantiate(formation, drawPos, Quaternion.identity);
            //with random number of enemies with random positions inside the room
            int num_enemies = Random.Range(0, 4);
            for (int i = 0; i < num_enemies; i++)
            {
                float x_pos = Random.Range(drawPos.x - .35f, drawPos.x + .35f);
                float y_pos = Random.Range(drawPos.y - .35f, drawPos.y + .35f);
                GameObject e_position0 = Instantiate(e_position, new Vector3(x_pos, y_pos, 0), Quaternion.identity);
                e_position0.transform.parent = formation0.transform;
                int random_enemy = Random.Range(1, enemies.Length);
                e_position0.GetComponent<Position>().enemy = enemies[random_enemy];
            }
        }
        //if boss
        else if (room.type == 1)
        {
            //make an enemy formation for it
            formation0 = Instantiate(formation, drawPos, Quaternion.identity);
            int num_enemies = 1;

            for (int i = 0; i < num_enemies; i++)
            {
                float x_pos = drawPos.x;
                float y_pos = drawPos.y;
                GameObject e_position0 = Instantiate(e_position, new Vector3(x_pos, y_pos, 0), Quaternion.identity);
                e_position0.transform.parent = formation0.transform;
                int random_enemy = Random.Range(0, 0);
                e_position0.GetComponent<Position>().enemy = enemies[random_enemy];
            }
        }
    }

    //checks for surrounding rooms for room just made and applies approriate walls and doors.
    void setWalls(Room room, GameObject formation0, Vector2 drawPos)
    {

        if (takenPositions.Contains(room.gridPos + Vector2.right))
        { //using Vector.[direction] as short hands, for simplicity
            GameObject door0 = Instantiate(doorTile, new Vector3(drawPos.x + .51f, drawPos.y, 0), Quaternion.Euler(0, 0, -90f));
            door0.GetComponent<Doors>().mobs = formation0.GetComponent<EnemySpawner>();
            GameObject thinwall0 = Instantiate(thinWall, new Vector3(drawPos.x + .64f, drawPos.y, 0), Quaternion.identity);
            thinwall0.layer = 12;
        }
        else
        {
            GameObject wall0 = Instantiate(wall, new Vector3(drawPos.x + 1.28f, drawPos.y, 0), Quaternion.identity);
            wall0.layer = 12;
        }
        if (takenPositions.Contains(room.gridPos + Vector2.left))
        {
            GameObject door2 = Instantiate(doorTile, new Vector3(drawPos.x - .51f, drawPos.y, 0), Quaternion.Euler(0, 0, 90f));
            door2.GetComponent<Doors>().mobs = formation0.GetComponent<EnemySpawner>();
            GameObject thinwall2 = Instantiate(thinWall, new Vector3(drawPos.x - .64f, drawPos.y, 0), Quaternion.identity);
            thinwall2.layer = 12;
        }
        else
        {
            GameObject wall2 = Instantiate(wall, new Vector3(drawPos.x - 1.28f, drawPos.y, 0), Quaternion.identity);
            wall2.layer = 12;
        }
        if (takenPositions.Contains(room.gridPos + Vector2.up))
        {
            GameObject door1 = Instantiate(doorTile, new Vector3(drawPos.x, drawPos.y + .51f, 0), Quaternion.Euler(0, 0, 0));
            door1.GetComponent<Doors>().mobs = formation0.GetComponent<EnemySpawner>();
            GameObject thinwall1 = Instantiate(thinWall, new Vector3(drawPos.x, drawPos.y + .64f, 0), Quaternion.Euler(0, 0, 90));
            thinwall1.layer = 12;
        }
        else
        {
            GameObject wall1 = Instantiate(wall, new Vector3(drawPos.x, drawPos.y + 1.28f, 0), Quaternion.identity);
            wall1.layer = 12;
        }
        if (takenPositions.Contains(room.gridPos + Vector2.down))
        {
            GameObject door3 = Instantiate(doorTile, new Vector3(drawPos.x, drawPos.y - .51f, 0), Quaternion.Euler(0, 0, 180));
            door3.GetComponent<Doors>().mobs = formation0.GetComponent<EnemySpawner>();
            GameObject thinwall3 = Instantiate(thinWall, new Vector3(drawPos.x, drawPos.y - .64f, 0), Quaternion.Euler(0, 0, 90));
            thinwall3.layer = 12;
        }
        else
        {
            GameObject wall3 = Instantiate(wall, new Vector3(drawPos.x, drawPos.y - 1.28f, 0), Quaternion.identity);
            wall3.layer = 12;
        }
        if (room.type == 1)s
        {
            GameObject stairs;
            if (takenPositions.Contains(room.gridPos + Vector2.right))
            {
                stairs = Instantiate(stairTile, new Vector3(drawPos.x - .51f, drawPos.y, 0), Quaternion.Euler(0, 0, 90f));
                stairs.GetComponent<Doors>().mobs = formation0.GetComponent<EnemySpawner>();
            }
            else if (takenPositions.Contains(room.gridPos + Vector2.left))
            {
                stairs = Instantiate(stairTile, new Vector3(drawPos.x + .51f, drawPos.y, 0), Quaternion.Euler(0, 0, -90f));
                stairs.GetComponent<Doors>().mobs = formation0.GetComponent<EnemySpawner>();
            }
            else if (takenPositions.Contains(room.gridPos + Vector2.up))
            {
                stairs = Instantiate(stairTile, new Vector3(drawPos.x, drawPos.y - .51f, 0), Quaternion.Euler(0, 0, 180));
                stairs.GetComponent<Doors>().mobs = formation0.GetComponent<EnemySpawner>();
            }
            else if (takenPositions.Contains(room.gridPos + Vector2.down))
            {
                stairs = Instantiate(stairTile, new Vector3(drawPos.x, drawPos.y + .51f, 0), Quaternion.Euler(0, 0, 0));
                stairs.GetComponent<Doors>().mobs = formation0.GetComponent<EnemySpawner>();
            }
        }
    }
    */

}

