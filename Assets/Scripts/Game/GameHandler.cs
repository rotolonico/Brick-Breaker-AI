﻿using System;
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

        public float gameTime;

        private void Awake() => Instance = this;

        private void Update()
        {
            gameTime += Time.deltaTime;
            if (Settings.Instance.gameTime / Settings.Instance.gameSpeed > gameTime) return;
            NEATHandler.Instance.evaluator.Evaluate();
            ResetGame();
            gameTime = 0;
        }

        public void ResetGameAndNetwork(Genome startingGenome = null)
        {
            foreach (var food in GameObject.FindGameObjectsWithTag("Ball")) Destroy(food);
            foreach (var tail in GameObject.FindGameObjectsWithTag("Platform")) Destroy(tail);

            NEATHandler.Instance.InitializeNetwork(startingGenome);
            //Instantiate(player, transform.position, Quaternion.identity);
            //playerAlive = true;
        }

        public void ResetGame()
        {
            foreach (var food in GameObject.FindGameObjectsWithTag("Ball")) Destroy(food);
            foreach (var tail in GameObject.FindGameObjectsWithTag("Platform")) Destroy(tail);
            
            NEATHandler.Instance.InitiateGeneration();
            //Instantiate(snakePlayer, transform.position, Quaternion.identity);
            //playerAlive = true;
        }
    }
}