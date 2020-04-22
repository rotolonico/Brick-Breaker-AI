using System;
using System.Linq;
using AI;
using AI.NEAT;
using NNUtils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class GameHandler : MonoBehaviour
    {
        public static GameHandler Instance;

        public GameObject player;
        public bool playerAlive;

        public int generation = 1;

        public float gameTime;

        public Toggle useTrainedNetwork;
        public TextMeshProUGUI useTrainedNetworkText;

        private bool isWebGL;
        private bool genomeDownloaded;
        private GenomeWrapper trainedGenome;

        private void Awake() => Instance = this;

        private void Start()
        {
            isWebGL = Application.streamingAssetsPath.Contains("://") ||
                      Application.streamingAssetsPath.Contains(":///");
            LoadTrainedGenome();
        }

        private void LoadTrainedGenome()
        {
            var genomePath = Application.streamingAssetsPath + "/Genome";

            if (isWebGL)
                NetworkStorage.Instance.DownloadGenome(genomePath, downloadedGenome =>
                {
                    trainedGenome = new GenomeWrapper(new Genome(downloadedGenome));
                    genomeDownloaded = true;
                }, Debug.Log);
            else
            {
                trainedGenome = new GenomeWrapper(new Genome(NetworkStorage.LoadGenome(genomePath)));
                genomeDownloaded = true;
            }
        }

        private void Update()
        {
            gameTime += Time.deltaTime;
            if (Settings.Instance.gameTime / Settings.Instance.gameSpeed > gameTime) return;
            if (generation % 3 == 0) Settings.Instance.gameTime += 100;
            generation++;
            if (!useTrainedNetwork.isOn)
            {
                foreach (var platformController in NEATHandler.Instance.alivePopulation.Where(platformController =>
                    platformController.ball.transform.position.y < -6))
                    platformController.genome.Genome.Score = Math.Max(0, platformController.genome.Genome.Score - 50);
                NEATHandler.Instance.evaluator.Evaluate();
                ResetGame();
            }
            else SwitchNetwork();

            gameTime = 0;
        }

        public void SwitchNetwork()
        {
            if (!genomeDownloaded)
            {
                useTrainedNetwork.SetIsOnWithoutNotify(!useTrainedNetwork.isOn);
                return;
            }

            if (!useTrainedNetwork.isOn)
            {
                SceneManager.LoadScene(0);
                return;
            }

            ResetGameAndNetwork(useTrainedNetwork.isOn ? trainedGenome : null);
            useTrainedNetworkText.text = "Train new network";
        }

        public void ResetGameAndNetwork(GenomeWrapper startingGenome = null)
        {
            foreach (var food in GameObject.FindGameObjectsWithTag("Ball")) Destroy(food);
            foreach (var tail in GameObject.FindGameObjectsWithTag("Platform")) Destroy(tail);
            foreach (var wall in GameObject.FindGameObjectsWithTag("HorizontalWall"))
                wall.GetComponent<SpriteRenderer>().color = Color.green;

            NEATHandler.Instance.InitializeNetwork(startingGenome);

            if (!useTrainedNetwork.isOn)
            {
                Instantiate(player, transform.position, Quaternion.identity);
                playerAlive = true;
                Settings.Instance.gameTime = 100;
            }
            else Settings.Instance.gameTime = 1000;

            generation = 0;
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