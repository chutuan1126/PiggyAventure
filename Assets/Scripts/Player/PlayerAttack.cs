using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float timeSinceAttack;

    public Transform attackPosition;
    public LayerMask enemies;
    public float attackRange;
    public int damage;

    void Update()
    {
        timeSinceAttack += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && timeSinceAttack > 0.25f)
        {
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, enemies);

            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                MonsterHeath enemy = enemiesToDamage[i].GetComponent<MonsterHeath>();
                if (enemy != null) enemy.TakeDamage(10f, false);
            }

            timeSinceAttack = 0.0f;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
}
