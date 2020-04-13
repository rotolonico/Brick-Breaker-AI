using System;
using AI.NEAT;
using IO;
using NN;
using UnityEngine;

namespace Game
{
    public class PlatformController : MonoBehaviour
    {
        public GameObject ballPrefab;

        public float speed;
        public GenomeWrapper genome;
        public int instanceId;

        public BallHandler ball;

        public bool isPlayer;

        private void Start()
        {
            var newBall = Instantiate(ballPrefab, new Vector3(0, 2.5f), Quaternion.identity);
            newBall.name = instanceId.ToString();
            ball = newBall.GetComponent<BallHandler>();
            ball.platform = this;
            if (isPlayer || genome.Best)
            {
                GetComponent<SpriteRenderer>().color = Color.white;
                ball.GetComponent<SpriteRenderer>().color = Color.white;
                
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