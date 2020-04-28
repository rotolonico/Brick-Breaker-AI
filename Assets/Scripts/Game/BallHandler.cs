using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Game
{
    public class BallHandler : MonoBehaviour
    {
        public Rigidbody2D rb;
        public PlatformController platform;

        private float wallRespawnDelay = 10;
        private float currentRespawnDelay;

        private readonly List<string> hitBlocks = new List<string>();

        public int brokenBricks;

        private void Start()
        {
            rb.velocity = new Vector2(Settings.Instance.gameSpeed,
                Settings.Scenario == 4 ? -Settings.Instance.gameSpeed : Settings.Instance.gameSpeed);
        }

        private void Update()
        {
            if (!platform.isPlayer && transform.position.y > -6) platform.genome.Genome.Score += Time.deltaTime * 50;

            return;
            currentRespawnDelay += Time.deltaTime;
            if (!(currentRespawnDelay > wallRespawnDelay)) return;
            var randomBlock = RandomnessHandler.Random.Next(0, hitBlocks.Count);
            if (platform.isPlayer || platform.genome.Best)
            {
                var hitObj = GameObject.Find(hitBlocks[randomBlock]);
                if (hitObj != null)
                    GameObject.Find(hitBlocks[randomBlock]).GetComponent<SpriteRenderer>().color = Color.green;
            }

            hitBlocks.RemoveAt(randomBlock);
            currentRespawnDelay = 0;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ball")) return;

            var newDirectionVector = new Vector2(1, -1);
            if (Math.Abs(transform.position.x - other.bounds.center.x) > other.GetComponent<SpriteRenderer>().size.x / 2
            ) newDirectionVector *= -1;

            if (other.CompareTag("Wall"))
            {
                rb.velocity *= newDirectionVector;
                if (Settings.Scenario == 3 && !platform.isPlayer) platform.genome.Genome.Score += 5;
            }

            if (Settings.Scenario != 1 && other.CompareTag("HorizontalWall") && !hitBlocks.Contains(other.name))
            {
                rb.velocity *= newDirectionVector;
                if (Settings.Scenario > 2)
                {
                    if (!platform.isPlayer) platform.genome.Genome.Score += 100;
                    if (platform.isPlayer || platform.genome.Best)
                        other.GetComponent<SpriteRenderer>().color = new Color(0.1320784f, 1, 0, 0.2f);
                }

                if (Settings.Scenario == 4)
                {
                    hitBlocks.Add(other.name);
                    brokenBricks++;
                }
            }

            if (other.CompareTag("Platform") && other.name == name)
            {
                var distanceFromCenter = transform.position.x - other.bounds.center.x;
                if (Math.Abs(distanceFromCenter) < 0.5f) distanceFromCenter = 0.5f * Math.Sign(distanceFromCenter);

                if (Settings.Scenario == 1) rb.velocity *= newDirectionVector;
                else rb.velocity = new Vector2(distanceFromCenter * 5f, -rb.velocity.y);
//                if (GameHandler.Instance.generation > 5 && !platform.isPlayer)
//                    platform.genome.Genome.Score += 200;
            }
        }
    }
}