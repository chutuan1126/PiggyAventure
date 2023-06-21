using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonsterAbstract
{
    private GameObject player;

    public List<GameObject> prefabs;

    public int attackCount;
    public int monsterCount;
    public float timeSinceSpawn;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        timeSinceSpawn += Time.deltaTime;

        if (player.transform.position.x >= this.distanceAttack)
        {
            MonsterController.Instance.SetIsAttack(true);
            MonsterController.Instance.SetAttackCount(attackCount);
        }

        if (player.transform.position.x >= this.distanceAttack) MonsterController.Instance.SetIsAttack(true);

        if (MonsterController.Instance.GetIsAttack() && MonsterController.Instance.GetAttackCount() == attackCount && timeSinceSpawn > 1f && monsterCount > 0)
        {
            float positionX = transform.position.x;
            int prefabIndex = Random.Range(0, prefabs.Count);
            Vector3 position = new(positionX, transform.position.y, transform.position.z);

            GameObject monster = Instantiate(prefabs[prefabIndex], position, Quaternion.identity);
            monster.transform.SetParent(transform);

            monsterCount--;
            timeSinceSpawn = 0.0f;
        }
    }
}
