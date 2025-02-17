﻿using System;
using System.Collections.Generic;
using System.Linq;
using AI.LiteNN;
using AI.NEAT;
using Game;
using UnityEngine;
using UnityEngine.UI;

namespace AI
{
    public class NEATHandler : MonoBehaviour
    {
        public static NEATHandler Instance;

        public GameObject playerAI;
        public int populationSize;
        public List<PlatformController> alivePopulation = new List<PlatformController>();

        public Evaluator evaluator;

        private void Awake() => Instance = this;

        private void Start()
        {
            if (GameHandler.initialized) InitializeNetwork();
        }

        public void InitializeNetwork(GenomeWrapper startingGenome = null)
        {
            if (!GameHandler.Instance.useTrainedNetwork.isOn)
            {
                evaluator = new Evaluator(populationSize, new Counter(), new Counter(), g => Mathf.Pow(g.Score, 3),
                    startingGenome?.Genome);
                InitiateGeneration();
            }
            else InitializeGenome(startingGenome);
        }

        public void InitiateGeneration()
        {
            alivePopulation.Clear();

            for (var i = 0; i < evaluator.Genomes.Count; i++)
            {
                var genome = evaluator.Genomes[i];
                var newPlayerAI = Instantiate(playerAI, transform.position, Quaternion.identity);
                newPlayerAI.name = i.ToString();
                var newPlayerController = newPlayerAI.GetComponent<PlatformController>();
                newPlayerController.genome = genome;
                newPlayerController.instanceId = i;
                alivePopulation.Add(newPlayerAI.GetComponent<PlatformController>());

                if (!genome.Best) continue;
                NetworkDisplayer.Instance.DisplayNetwork(genome);
                newPlayerAI.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        public void InitializeGenome(GenomeWrapper genome)
        {
            var newPlayerAI = Instantiate(playerAI, transform.position, Quaternion.identity);
            newPlayerAI.name = "0";
            var newPlayerController = newPlayerAI.GetComponent<PlatformController>();
            newPlayerController.genome = genome;
            newPlayerController.genome.Best = true;
            newPlayerController.instanceId = 0;
            alivePopulation.Add(newPlayerAI.GetComponent<PlatformController>());

                NetworkDisplayer.Instance.DisplayNetwork(genome);
            newPlayerAI.GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }
}