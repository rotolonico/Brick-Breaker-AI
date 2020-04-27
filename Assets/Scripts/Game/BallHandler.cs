using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BallHandler : MonoBehaviour
    {
        public Rigidbody2D rb;
        public PlatformController platform;

        private readonly List<string> hitBlocks = new List<string>();

        private void Start()
        {
            rb.velocity = new Vector2(Settings.Instance.gameSpeed,
                Settings.Scenario == 4 ? -Settings.Instance.gameSpeed : Settings.Instance.gameSpeed);
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
                    if (!platform.isPlayer) platform.genome.Genome.Score += 5;
                    if (platform.isPlayer || platform.genome.Best)
                        other.GetComponent<SpriteRenderer>().color = new Color(0.1320784f, 1 ,0, 0.1f);
                }

                if (Settings.Scenario == 4) hitBlocks.Add(other.name);
            }

            if (other.CompareTag("Platform") && other.name == name)
            {
                var distanceFromCenter = transform.position.x - other.bounds.center.x;
                if (Math.Abs(distanceFromCenter) < 0.5f) distanceFromCenter = 0.5f * Math.Sign(distanceFromCenter);

                if (Settings.Scenario == 1) rb.velocity *= newDirectionVector;
                else rb.velocity = new Vector2(distanceFromCenter * 5f, -rb.velocity.y);
                if (GameHandler.Instance.generation < 10 && !platform.isPlayer)
                    platform.genome.Genome.Score = Math.Max(0, platform.genome.Genome.Score - 3);
            }
        }
    }
}