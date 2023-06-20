using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> prefabs;

    public int monsterCount;
    public float timeSinceSpawn;

    private void Update()
    {
        timeSinceSpawn += Time.deltaTime;
        if (timeSinceSpawn > 1f && monsterCount > 0)
        {
            float positionX = 12 + monsterCount;
            int prefabIndex = Random.Range(0, prefabs.Count);
            Vector3 position = new(positionX, -1.5f, transform.position.z);

            GameObject monster = Instantiate(prefabs[prefabIndex], position, Quaternion.identity);
            monster.transform.SetParent(transform);

            monsterCount--;
        }
    }
}
