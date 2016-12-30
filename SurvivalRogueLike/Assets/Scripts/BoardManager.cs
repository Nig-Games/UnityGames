using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);

    public GameObject exit;                                         //Prefab to spawn for exit.
    public GameObject[] floorTiles;                                 //Array of floor prefabs.
    public GameObject[] wallTiles;                                  //Array of wall prefabs.
    public GameObject[] foodTiles;                                  //Array of food prefabs.
    public GameObject[] enemyTiles;                                 //Array of enemy prefabs.
    public GameObject[] outerWallTiles;                             //Array of outer tile prefabs.
                                                                    // Use this for initialization

    private Transform boardHolder;                                  //A variable to store a reference to the transform of our Board object.
    private List<Vector3> cuadriculaPosicion = new List<Vector3>();      //A list of possible locations to place tiles. Es como el cuadrito donde vamos a poner algo, ya sea un enemigo, o una pared, comida, etc.

    void InitialiseList()
    {
        //Clear our list cuadriculaPosicion. Limpiamos nuestra lista
        cuadriculaPosicion.Clear();

        //Pensar en una matriz...o un cuadrado de x por y...Vamos a iterar sobre ese cuadrado/matriz

        //Loop through x axis (columns).
        for (int x = 1; x < columns - 1; x++)
        {
            //Within each column, loop through y axis (rows).
            for (int y = 1; y < rows - 1; y++)
            {
                //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                cuadriculaPosicion.Add(new Vector3(x, y, 0f));
            }
        }
        //La razon de por que recorremos x e y hasta x-1 e y-1 es para dejar un pasillo libre hasta la salida 
        //para asi no generar un nivel que sea imposible
    }

    //Digamos que crea el escenario principal...las paredes y el suelo. Aqui no estamos creando ni disponiendo ni la comida...ni enemigos...ni nada que no sea suelo o paredes.
    void BoardSetup()
    {
        //Instantiate Board and set boardHolder to its transform.
        boardHolder = new GameObject("Board").transform;

        //La razon de por que estamos iterando desde x-1 e y-1 hasta columns+1 y rows+1 es porque estamos rellenando los bordes del "escenario principal"
        //Loop through x axis (columns).
        for (int x = -1; x < columns + 1; x++)
        {
            //Within each column, loop through y axis (rows).
            for (int y = -1; y < rows + 1; y++)
            {
                //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it, es decir, del array anteriormente declarado de suelos, escogemos un numero al azar (para tener un sprite aleatorio) y se lo metemos a un gameObject.
                //Primero suponemos que estamos en un suelo, en el siguiente paso comprobaremos si en vez de un suelo, es una pared...y si lo es, machacamos el suelo por una pared.
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                //Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                //Basicamente, instanciamos un nuevo gameObject. El gameObject que vamos a instanciar es el anteriormente creado, en la posicion x e y en la que estemos, sin rotacion?. Lo casteamos a GameObject...no se muy bien por que
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPositionMethod()
    {
        //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List cuadriculaPosicion.
        int randomIndex = Random.Range(0, cuadriculaPosicion.Count);

        //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List cuadriculaPosicion
        Vector3 randomPosition = cuadriculaPosicion[randomIndex];

        //Para prevenir que no ponemos dos cosas en el mismo sitio, quitamos esa posicion de cuadriculaPosicion
        cuadriculaPosicion.RemoveAt(randomIndex);

        //Return the randomly selected Vector3 position.
        return randomPosition;
    }

    //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        //Choose a random number of objects to instantiate within the minimum and maximum limits
        int objectCount = Random.Range(minimum, maximum + 1);

        //Instantiate objects until the randomly chosen limit objectCount is reached
        for (int i = 0; i < objectCount; i++)
        {
            //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in cuadriculaPosicion
            Vector3 randomPosition = RandomPositionMethod();

            //Choose a random tile from tileArray and assign it to tileChoice
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    //SetupScene initializes our level and calls the previous functions to lay out the game board
    public void SetupScene(int level)
    {
        //Creates the outer walls and floor.
        BoardSetup();

        //Reset our list of cuadriculaPosicion.
        InitialiseList();

        //Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);

        //Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

        //Determine number of enemies based on current level number, based on a logarithmic progression
        int enemyCount = (int)Mathf.Log(level, 2f);

        //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

        //Instantiate the exit tile in the upper right hand corner of our game board
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
}
