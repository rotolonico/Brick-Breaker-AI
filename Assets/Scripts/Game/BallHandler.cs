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
            rb.velocity = new Vector2(Settings.Instance.gameSpeed, Settings.Scenario == 4 ? -Settings.Instance.gameSpeed : Settings.Instance.gameSpeed);
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
                if (Settings.Scenario > 2 && !platform.isPlayer) platform.genome.Genome.Score += 5;
                if (Settings.Scenario == 4) hitBlocks.Add(other.name);
            }

            if (other.CompareTag("Platform") && other.name == name)
            {
                if (Settings.Scenario == 1) rb.velocity *= newDirectionVector;
                else rb.velocity = new Vector2((transform.position.x - other.bounds.center.x) * 5f, -rb.velocity.y);
                if (!platform.isPlayer) platform.genome.Genome.Score++;
            }
        }
    }
}