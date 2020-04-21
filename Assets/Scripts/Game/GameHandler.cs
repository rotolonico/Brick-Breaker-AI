using System;
using System.Linq;
using AI;
using AI.NEAT;
using UnityEngine;

namespace Game
{
    public class GameHandler : MonoBehaviour
    {
        public static GameHandler Instance;

        public GameObject player;
        public bool playerAlive;
        
        public int generation = 1;

        public float gameTime;

        private void Awake() => Instance = this;

        private void Update()
        {
            gameTime += Time.deltaTime;
            if (Settings.Instance.gameTime / Settings.Instance.gameSpeed > gameTime) return;
            if (generation % 3 == 0) Settings.Instance.gameTime += 100;
            generation++;
            foreach (var platformController in NEATHandler.Instance.alivePopulation.Where(platformController =>
                platformController.ball.transform.position.y < -6)) platformController.genome.Genome.Score = Math.Max(0, platformController.genome.Genome.Score - 50);

            NEATHandler.Instance.evaluator.Evaluate();
            ResetGame();
            gameTime = 0;
        }

        public void ResetGameAndNetwork(Genome startingGenome = null)
        {
            foreach (var food in GameObject.FindGameObjectsWithTag("Ball")) Destroy(food);
            foreach (var tail in GameObject.FindGameObjectsWithTag("Platform")) Destroy(tail);
            foreach (var wall in GameObject.FindGameObjectsWithTag("HorizontalWall"))
                wall.GetComponent<SpriteRenderer>().color = Color.green;

            NEATHandler.Instance.InitializeNetwork(startingGenome);
            //Instantiate(player, transform.position, Quaternion.identity);
            //playerAlive = true;
        }

        public void ResetGame()
        {
            foreach (var food in GameObject.FindGameObjectsWithTag("Ball")) Destroy(food);
            foreach (var tail in GameObject.FindGameObjectsWithTag("Platform")) Destroy(tail);
            foreach (var wall in GameObject.FindGameObjectsWithTag("HorizontalWall"))
                wall.GetComponent<SpriteRenderer>().color = Color.green;

            NEATHandler.Instance.InitiateGeneration();
            //Instantiate(snakePlayer, transform.position, Quaternion.identity);
            //playerAlive = true;
        }
    }
}