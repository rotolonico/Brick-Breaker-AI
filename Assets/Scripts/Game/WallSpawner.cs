using System;
using System.Linq;
using UnityEngine;
using Utils;

namespace Game
{
    public class WallSpawner : MonoBehaviour
    {
        private Camera mainCamera;

        public GameObject hWall;

        private int spawnedWalls;

        private void Start() => mainCamera = Camera.main;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                if (!(mousePosition.x < 5.5f) || !(mousePosition.x > -5.5f) || !(mousePosition.y > -0.5f) ||
                    !(mousePosition.y < 3.5f)) return;

                SpawnWall(mousePosition);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                if (!(mousePosition.x < 5.5f) || !(mousePosition.x > -5.5f) || !(mousePosition.y > -0.5f) ||
                    !(mousePosition.y < 3.5f)) return;

                DestroyWall(mousePosition);
            }
        }

        private void SpawnWall(Vector3 position)
        {
            if (Physics2D.OverlapPointAll(position).Any(col => col.CompareTag("HorizontalWall")))
                return;

            Instantiate(hWall, new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), 0),
                Quaternion.identity).name = spawnedWalls++.ToString();
        }

        private void DestroyWall(Vector3 position)
        {
            foreach (var col in Physics2D.OverlapPointAll(position))
                if (col.CompareTag("HorizontalWall"))
                    Destroy(col.gameObject);
        }
    }
}