using System;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class WallSpawner : MonoBehaviour
    {
        private Camera mainCamera;

        public GameObject hWall;

        private void Start() => mainCamera = Camera.main;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                if (!(mousePosition.x < 5.5f) || !(mousePosition.x > -5.5f) || !(mousePosition.y > -0.5f) ||
                    !(mousePosition.y < 3.5f)) return;

                if (Physics2D.OverlapPointAll(mousePosition).Any(col => col.CompareTag("HorizontalWall")))
                    return;

                Instantiate(hWall, new Vector3(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y), 0),
                    Quaternion.identity);
                StartCoroutine(BlocksHandler.Instance.InitializeBlocks());
            }
            else if (Input.GetMouseButtonDown(1))
            {
                var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                if (!(mousePosition.x < 5.5f) || !(mousePosition.x > -5.5f) || !(mousePosition.y > -0.5f) ||
                    !(mousePosition.y < 3.5f)) return;

                foreach (var col in Physics2D.OverlapPointAll(mousePosition))
                    if (col.CompareTag("HorizontalWall"))
                        Destroy(col.gameObject);

                StartCoroutine(BlocksHandler.Instance.InitializeBlocks());
            }
        }
    }
}