using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using NeuralNetwork;
using System.Linq;
using System.Collections.Generic;

public class AITesting : LevelScript {

    //AI
    public float aiRestartTime = 120;

    
    private float updateTime;
    private AI[] aiSpawns;

    

    // Use this for initialization
    void Start () {

        numNodeHiddenLayers = (numInputs + numOutputs) / 2;
        aiSpawns = new AI[population];
        updateTime = aiRestartTime;

        //-------------Basic Visual Representation of Neural Network---------------------------//
        //nView = GetComponent<NeuralNetworkView>();
        //nView.CreateNetwork(numInputs, numOutputs, numHiddenLayers, numNodeHiddenLayers);

        //Intiation of neural network array for the population
        nNetwork = new NeuralNet[population];

        source = GetComponent<AudioSource>();

        if (!debug)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
        
        RunGameSetup();
    }
	
	// Update is called once per frame
	void Update () {

        if (!setup)
        {
            Timer();
            gameInfo.text = "Generation: " + genAlg.generation;

            //TimeScale
            if (Input.GetKeyDown(KeyCode.Comma))
            {
                Time.timeScale *= .5f;
            }
            if (Input.GetKeyDown(KeyCode.Period))
            {
                Time.timeScale *= 2;
            }
            //----------------Use the first spawn as the selected object in visual representation--------------//

            //nView.NodeUpdate(aiSpawns[0].inputs, nNetwork[0]);

            //More Stats
            genAlg.CalculateBestWorstAvTot();
            gameInfo.text += "\nBest Fitness: " + genAlg.bestFitness + "\nAverage Fitness: " + genAlg.averageFitness + "\nLowest Fitness: " + genAlg.worstFitness;

            if (Time.time > updateTime)
            {
                updateTime += aiRestartTime;

                string path = string.Format(@"C:\Users\Chinedu\Documents\GitHub\BubbleLife\Bubble Life\Saves\generation {0}.txt", genAlg.generation);
                File.WriteAllText(path, string.Join(" ", new List<double>(genAlg.population[0].weights).ConvertAll(i => i.ToString()).ToArray()));
                //--------------The main function that creates the new population into the genAlg class--------//
                genAlg.Epoch(genAlg.population);

                for (int i = 0; i < population; i++)
                {
                    int index = UnityEngine.Random.Range(0, grid.walkableNodes.Count);
                    Node node = grid.walkableNodes[index];
                    //change all of the spawns positions
                    aiSpawns[i].gameObject.SetActive(true);
                    aiSpawns[i].transform.position = node.worldPosition;
                    aiSpawns[i].fitness = 100;
                    aiSpawns[i].health = 100;

                    //Update the spawns neural networks with the new weights
                    nNetwork[i].PutWeights(genAlg.population[i].weights);
                    aiSpawns[i].neuralNet = nNetwork[i];
                    aiSpawns[i].index = i;
                }
            }
        }

    }

    private void RunGameSetup()
    {
        
        //Setup Game UI
        gameRestartButton = GameObject.FindGameObjectWithTag("restart_button").GetComponent<Button>();
        gameExitButton = GameObject.FindGameObjectWithTag("exit_button").GetComponent<Button>();

        gameInfo = GameObject.FindGameObjectWithTag("info").GetComponent<Text>();
        gameTimer = GameObject.FindGameObjectWithTag("timer").GetComponent<Text>();

        gameOverPanel = GameObject.FindGameObjectWithTag("game_over_panel");
        gameOverText = GameObject.FindGameObjectWithTag("game_over_text").GetComponent<Text>();

        gameRestartButton.onClick.AddListener(() => { RestartGame(); });
        gameExitButton.onClick.AddListener(() => { gameController.ExitGame(); });

        //Hide Game Over Panel
        gameOverPanel.SetActive(false);

        //Create Collision check grid
        grid = new GameObject("Grid").AddComponent<Grid>();
        grid.levelScript = this;

        minWorldPosX = grid.grid[0, 0].worldPosition.x;
        minWorldPosY = grid.grid[0, 0].worldPosition.y;
        maxWorldPosX = grid.grid[grid.gridSizeX - 1, grid.gridSizeY - 1].worldPosition.x;
        maxWorldPosY = grid.grid[grid.gridSizeX - 1, grid.gridSizeY - 1].worldPosition.y;

        grid.CreateGrid();

        // spawn Asteroid and Stars and Background
        AddStars(starCount);
        AddAsteroids(asteroidCount);

        GameObject borderBlock = Instantiate(border, Vector3.zero, Quaternion.identity) as GameObject;
        borderBlock.transform.localScale = new Vector3(gridWorldSize.x, gridWorldSize.y, 1);

        //Add Player
        //int index = UnityEngine.Random.Range(0, grid.walkableNodes.Count);
        //Node node = grid.walkableNodes[index];
        //player = Instantiate(playerInstance, node.worldPosition, Quaternion.identity) as GameObject;
        ////player.GetComponent<SpriteRenderer>().color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        ////player.transform.localScale = new Vector3(1.8f, 1.8f);
        ////BasePlayer playerScript = player.AddComponent<Player>();
        //player.name = "player1";
        //player.tag = "Player";

        //Spawn AI
        AddAI(aiCount);
        

    }

    private void AddAI(int aiCount)
    {
        GameObject aiEmpty = new GameObject("AIOpponents");
        for (int i = 0; i < aiCount; i++)
        {
            int index = UnityEngine.Random.Range(0, grid.walkableNodes.Count);
            Node node = grid.walkableNodes[index];
            GameObject ai = Instantiate(aiOpponent, node.worldPosition, Quaternion.identity, aiEmpty.transform) as GameObject;
            ai.GetComponent<SpriteRenderer>().color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            ai.name = "AI "+ i;

            //-------Create neural network with defined inputs and ouputs for each spawn----------//
            nNetwork[i] = new NeuralNet(numInputs, numOutputs, numHiddenLayers, numNodeHiddenLayers);
            

            //-------Creates on Genetic algorithm to handle the whole popluation-----------------//
            if (i == 0)
            {
                genAlg = new GeneticAlgorithm(population, .2, .7, nNetwork[0].GetNumWeights());
            }
            //--------Set the random weights generated in genAlg into--------------------------//
            nNetwork[i].PutWeights(genAlg.population[i].weights);
            aiSpawns[i] = ai.GetComponent<AI>();
            aiSpawns[i].neuralNet = nNetwork[i];
            aiSpawns[i].index = i;
            
        }

    }
    private void AddAsteroids(int astroidCount)
    {
        asteroidEmpty = new GameObject("Astroids");

        for (int i = 0; i < asteroidCount; i++)
        {
            int index = UnityEngine.Random.Range(0, grid.walkableNodes.Count);
            int asteroidSize = UnityEngine.Random.Range(minAsteroidSize, maxAsteroidSize);
            float asteroidVel = UnityEngine.Random.Range(minAsteroidVel, maxAsteroidVel);
            Vector2 velDirection = Random.insideUnitCircle.normalized;

            GameObject asteroid = asteroids[UnityEngine.Random.Range(0, asteroids.Length)];
            Node node = grid.walkableNodes[index];
            GameObject asteroidObj = Instantiate(asteroid, node.worldPosition, Quaternion.identity) as GameObject;
            asteroidObj.transform.localScale = new Vector3(asteroidSize, asteroidSize);
            asteroidObj.GetComponent<Rigidbody2D>().velocity = velDirection * asteroidVel;
            asteroidObj.transform.SetParent(asteroidEmpty.transform);
        }
        
    }

    private void AddStars(float num)
    {
        GameObject starEmpty = new GameObject("Stars");
        for (int i = 0; i < num; i++)
        {
            float starSize = Random.Range(m_minStarSize, m_maxStarSize);
            int index = UnityEngine.Random.Range(0, grid.walkableNodes.Count);
            Node node = grid.walkableNodes[index];
            GameObject star = Instantiate(starInstance, node.worldPosition, Quaternion.identity, starEmpty.transform) as GameObject;
            star.transform.localScale = new Vector3(starSize, starSize);
        }
        
    }

    //public void SaveName()
    //{
    //    playerName = menuEnterNameField.text;

    //    BinaryFormatter bf = new BinaryFormatter();

    //    FileStream file = File.Create(savePathPlayerName);

    //    bf.Serialize(file, playerName);
    //    file.Close();
    //}

    //public void SaveHighScores()
    //{
    //    BinaryFormatter bf = new BinaryFormatter();

    //    FileStream file = File.Create(savePathHighScores);

    //    bf.Serialize(file, highScores);
    //    file.Close();


    //}

    private void Timer()
    {
        timer += Time.deltaTime;
        gameTimer.text = "Time: " + timer.ToString("0.0");
    }
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
