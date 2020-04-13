using System;
using UnityEngine;

namespace Game
{
    public class BallHandler : MonoBehaviour
    {
        public Rigidbody2D rb;
        public PlatformController platform;

        private void Start()
        {
            rb.velocity = new Vector2(Settings.Instance.gameSpeed, Settings.Instance.gameSpeed);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ball")) return;
            
            var newDirectionVector = new Vector2(1, -1);
            if (Math.Abs(transform.position.x - other.bounds.center.x) > other.GetComponent<SpriteRenderer>().size.x / 2) newDirectionVector *= -1;

            if (other.CompareTag("Wall")){ rb.velocity *= newDirectionVector;}

            if (other.CompareTag("Platform") && other.name == name)
            {
                rb.velocity *= newDirectionVector;
                if (!platform.isPlayer) platform.genome.Genome.Score++;
            }
        }
    }
}