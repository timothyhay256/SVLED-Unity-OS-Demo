using System;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    public bool enabled = true;
    public GameObject bullet;
    public float spawnAreaHeight = 5f;
    public float spawnAreaWidth = 5f;
    public float spawnAreaOffset = 0f;

    public float spawnRate = 1f; // seconds between spawns
    private float timer;

    public int scoreCount;
    public int deathCount;

    void Update()
    {
        if (!enabled || bullet == null) return;

        // Debug.Log(Mathf.Max(0f, spawnRate - 0.1f * scoreCount));

        timer += Time.deltaTime;
        if (timer >= Mathf.Max(.5f, spawnRate - 0.10f * scoreCount))
        {
            timer = 0f;
            SpawnBullet();
        }
    }

    void SpawnBullet()
    {
        Vector3 center = new Vector3(transform.position.x + spawnAreaOffset, transform.position.y, transform.position.z);
        float randomY = UnityEngine.Random.Range(-spawnAreaHeight / 2f, spawnAreaHeight / 2f);
        float randomZ = UnityEngine.Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f);

        Vector3 spawnPosition = new Vector3(center.x, center.y + randomY, center.z + randomZ);

        var bulletPartHolder = Instantiate(bullet, spawnPosition, Quaternion.identity).transform.GetChild(0);
        var speed = bulletPartHolder.GetComponent<RotateAndMoveLeft>().moveSpeed;

        var newSpeed = speed + 0.02f * scoreCount;
        Debug.Log("newspeed");
        Debug.Log(newSpeed);
        bulletPartHolder.GetComponent<RotateAndMoveLeft>().moveSpeed = newSpeed;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireCube(
            new Vector3(transform.position.x + spawnAreaOffset, transform.position.y, transform.position.z),
            new Vector3(0, spawnAreaHeight, spawnAreaWidth)
        );
    }
}
