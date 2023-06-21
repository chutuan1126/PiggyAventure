using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Monster : MonsterAbstract
{
    public float speed;
    private float distance;

    public GameObject player;

    private Animator animator;
    private SpriteRenderer sprite;

    private float timeSinceAttack;
    public Transform attackPosition;
    public LayerMask layerPlayer;
    public float attackRange;
    public int damage;

    private bool isDeath;

    private void Start()
    {
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        timeSinceAttack += Time.deltaTime;

        if (player != null)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
            float directionX = player.transform.position.x - transform.position.x;

            if (directionX > 0) sprite.flipX = true;
            if (directionX < 0) sprite.flipX = false;

            if (!isDeath && distance >= 1f && MonsterController.Instance.GetIsAttack())
            {
                animator.SetBool("Running", true);
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                timeSinceAttack = 0.0f;
            }
            else
            {
                animator.SetBool("Running", false);
            }

            if (distance <= 1f && timeSinceAttack > 1f)
            {
                Collider2D[] playerToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, layerPlayer);

                for (int i = 0; i < playerToDamage.Length; i++)
                {
                    PlayerHealth playerTo = playerToDamage[i].GetComponent<PlayerHealth>();
                    if (playerTo != null)
                    {
                        animator.SetTrigger("Attack");
                        playerTo.TakeDamage(damage, false);
                    }
                }

                timeSinceAttack = 0.0f;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }

    public void MonsterDeath()
    {
        if (isDeath == false)
        {
            isDeath = true;
            animator.SetBool("Death", isDeath);

            StartCoroutine(DestroyGameObject());
        }
    }

    IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }
}
