﻿using System;
using AI.NEAT;
using IO;
using NN;
using UnityEngine;

namespace Game
{
    public class PlatformController : MonoBehaviour
    {
        public GameObject ballPrefab;
        public GameObject wallPrefab;

        public float speed;
        public GenomeWrapper genome;
        public int instanceId;

        public BallHandler ball;

        public bool isPlayer;

        private void Start()
        {
            var newBall = Instantiate(ballPrefab, new Vector3(Settings.Scenario == 4 ? -2 : 0, -1f), Quaternion.identity);
            newBall.name = instanceId.ToString();
            ball = newBall.GetComponent<BallHandler>();
            ball.platform = this;

            if (isPlayer || genome.Best)
            {
                var platformSpriteRenderer = GetComponent<SpriteRenderer>();
                ball.GetComponent<SpriteRenderer>().color = platformSpriteRenderer.color;
                platformSpriteRenderer.color = Color.white;
            }
        }

        private void FixedUpdate()
        {
            if (isPlayer) UpdatePosition(Time.deltaTime * Input.GetAxisRaw("Horizontal") * speed * Settings.Instance.gameSpeed);
            else
            {
                var outputs = NetworkCalculator.TestNetworkGenome(genome.Network, InputsRetriever.GetInputs(this));
                if (outputs[0] - outputs[1] > 0) Left();
                if (outputs[2] - outputs[3] > 0) Right();
            }
        }

        private void Left() => UpdatePosition(Time.deltaTime * speed * Settings.Instance.gameSpeed);

        private void Right() => UpdatePosition(-Time.deltaTime * speed * Settings.Instance.gameSpeed);

        private void UpdatePosition(float value)
        {
            var position = transform.position;
            position.x = Mathf.Clamp(position.x + value, -5, 5);
            transform.position = position;
        }
    }
}