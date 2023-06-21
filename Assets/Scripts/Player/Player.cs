using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    [SerializeField] float speed = 6.0f;
    [SerializeField] float jumpForce = 14f;
    [SerializeField] float rollForce = 6.0f;
    [SerializeField] bool noBlood = false;
    [SerializeField] GameObject slideDust;

    private Animator animator;
    private Rigidbody2D body2d;
    private BoxSensor groundSensor;
    private BoxSensor wallSensorR1;
    private BoxSensor wallSensorR2;
    private BoxSensor wallSensorL1;
    private BoxSensor wallSensorL2;
    private bool isWallSliding = false;
    private bool grounded = false;
    private bool rolling = false;
    private int facingDirection = 1;
    private int currentAttack = 0;
    private float timeSinceAttack = 0.0f;
    private float delayToIdle = 0.0f;
    private float rollDuration = 8.0f / 14.0f;
    private float rollCurrentTime;

    public Transform attackPosition;
    public LayerMask enemies;
    public float attackRange;
    public int damage;

    private bool isDeath;
    public PlayerHealth playerHealth;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        playerHealth = transform.GetComponent<PlayerHealth>();
        groundSensor = transform.Find("GroundSensor").GetComponent<BoxSensor>();
        wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<BoxSensor>();
        wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<BoxSensor>();
        wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<BoxSensor>();
        wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<BoxSensor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDeath == false)
        {
            // Increase timer that controls attack combo
            timeSinceAttack += Time.deltaTime;

            //Check if character just landed on the ground
            if (!grounded && groundSensor.State())
            {
                grounded = true;
                animator.SetBool("Grounded", grounded);
            }

            //Check if character just started falling
            if (grounded && !groundSensor.State())
            {
                grounded = false;
                animator.SetBool("Grounded", grounded);
            }

            //Wall Slide
            isWallSliding = (wallSensorR1.State() && wallSensorR2.State()) || (wallSensorL1.State() && wallSensorL2.State());
            animator.SetBool("WallSlide", isWallSliding);

            PlayerAttack();
            PlayerRoll();
            PlayerBlock();
            PlayerJump();
            PlayerRun();
        }
    }

    public void PlayerRun()
    {
        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            facingDirection = 1;
        }

        if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            facingDirection = -1;
        }

        // Move
        if (!rolling)
            body2d.velocity = new Vector2(inputX * speed, body2d.velocity.y);

        //Set AirSpeed in animator
        animator.SetFloat("AirSpeedY", body2d.velocity.y);

        if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            delayToIdle = 0.05f;
            animator.SetInteger("AnimState", 1);
        }
        else
        {
            // Prevents flickering transitions to idle
            delayToIdle -= Time.deltaTime;
            if (delayToIdle < 0)
                animator.SetInteger("AnimState", 0);
        }
    }

    public void PlayerJump()
    {
        if (Input.GetKeyDown("space") && grounded && !rolling)
        {
            animator.SetTrigger("Jump");
            grounded = false;
            animator.SetBool("Grounded", grounded);
            body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
            groundSensor.Disable(0.2f);
        }
    }

    public void PlayerBlock()
    {
        if (Input.GetMouseButtonDown(1) && !rolling)
        {
            animator.SetTrigger("Block");
            animator.SetBool("IdleBlock", true);
        }

        if (Input.GetMouseButtonUp(1))
            animator.SetBool("IdleBlock", false);
    }

    public void PlayerRoll()
    {
        // Increase timer that checks roll duration
        if (rolling) rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if (rollCurrentTime > rollDuration)
        {
            rolling = false;
            playerHealth.SetRollingState(false);
        }

        if (Input.GetKeyDown("left shift") && !rolling && !isWallSliding)
        {
            rolling = true;
            animator.SetTrigger("Roll");
            body2d.velocity = new Vector2(body2d.velocity.x + rollForce, body2d.velocity.y);
            playerHealth.SetRollingState(true);
        }
    }

    public void PlayerTriggerHurt()
    {
        if (!rolling) animator.SetTrigger("Hurt");
    }

    public void PlayerAttack()
    {
        if (Input.GetMouseButtonDown(0) && timeSinceAttack > 0.25f && !rolling)
        {
            int rangeValue = Random.Range(0, 9);
            bool isCritical = false;
            float damageRange = Random.Range(damage + currentAttack * 2, damage + 3 + currentAttack * 2);

            if (rangeValue % 2 == 0 && currentAttack == 2)
            {
                damageRange *= 2;
                isCritical = true;
            }

            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, enemies);

            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                MonsterHeath enemy = enemiesToDamage[i].GetComponent<MonsterHeath>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damageRange, isCritical);
                }
            }

            animator.SetTrigger("Attack" + (currentAttack + 1));

            currentAttack++;
            timeSinceAttack = 0.0f;
            if (currentAttack >= 3 || timeSinceAttack > 1.0f) currentAttack = 0;
        }
    }

    public void PlayerDeath()
    {
        if (isDeath == false)
        {
            isDeath = true;
            animator.SetBool("noBlood", noBlood);
            animator.SetTrigger("Death");

            StartCoroutine(DestroyGameObject());
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }

    IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (facingDirection == 1)
            spawnPosition = wallSensorR2.transform.position;
        else
            spawnPosition = wallSensorL2.transform.position;

        if (slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(facingDirection, 1, 1);
        }
    }
}
