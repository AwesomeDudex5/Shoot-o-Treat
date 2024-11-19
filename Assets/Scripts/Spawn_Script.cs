using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Script : MonoBehaviour
{

    //radius to spawn enemies in
    public int range;

    private float xRange;
    private float zRange;
    private float yRange;

    //if flag is triggered, spawn enemy
    public float spawnFlag;

    // Start is called before the first frame update
    void Start()
    {
        xRange = 0;
        zRange = 0;
        yRange = transform.position.y + 1f;
    }

    public void spawnEnemy(GameObject enemy)
    {
        xRange = transform.position.x + Random.Range(0, range);
        zRange = transform.position.z + Random.Range(0, range);
        Instantiate(enemy, new Vector3(xRange, yRange, zRange), Quaternion.identity);
    }
}
