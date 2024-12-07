using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [Header("Spawn Parameters")]
    [SerializeField] public float swimSpeed = 1f;
    [SerializeField] public float despawnTime = 3f;
    [SerializeField] float followSpeed = 5f;
    [SerializeField] public bool isCaught = false;
    [SerializeField] GameObject fish;

    [Header("AI Parameters")]
    [SerializeField] float hopCooldown = 1f; // time it takes for fish to be able to move again, this helps it not fly
    [SerializeField] public float attackRange = 2f; // how far it sees you
    [SerializeField] public float idleTimeMin = 1f; // min time it takes when idling
    [SerializeField] public float idleTimeMax = 2f; // max time it takes when idling
    [SerializeField] float hopHeight = 1f; // how high
    [SerializeField] float hopSpeed = 1f; // how far forward
    [SerializeField] public float movementChanceTimer = 1f; // clock time for chance to do movement
    [SerializeField] public float attackChanceTimer = 1f; // clock time for chance to do attack

    [Header("Movement Chances (all chances must add up to 1)")]
    [SerializeField] public float moveTowardChance = 0.33f; // small hope toward player
    [SerializeField] public float moveAwayChance = 0.33f; // small hop away from player
    [SerializeField] public float idleChance = 0.33f; // doesnt do anything;

    [Header("Attack Chances (all chances must add up to 1)")]
    [SerializeField] public float attackChance = 0.33f; // jumps into the player
    [SerializeField] public float jumpOverChance = 0.33f; // jumps over the player to the other side of the player
    [SerializeField] public float idleAttackChance = 0.33f; // doesnt attack or jump over;

    [Header("Fight Parameters")]
    [SerializeField] float jumpMultiplier = 2f;
    [SerializeField] float attackMultiplier = 2f;
    [SerializeField] int damage = 1;
    [SerializeField] int maxHealth = 3;
    [SerializeField] int currHealth = 3;
    [SerializeField] int moneyWorth = 1;


    Transform bobberTransForm;
    Rigidbody2D rb;
    SpriteRenderer sr;
    BoxCollider2D bc;
    Bobber bobber;
    Fisherman man;
    Vector3 move = Vector3.zero;
    bool isFollowing = false;
    bool isReeled = false;
    public bool onLand = false;
    bool hopping = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowing)
        {
            rb.velocity = Vector2.zero;

            Vector3 followPosition = new Vector3(bobberTransForm.position.x, bobberTransForm.position.y - 0.5f, bobberTransForm.position.z);
            Vector3 direction = (followPosition - transform.position).normalized;
            transform.position += direction * followSpeed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, bobberTransForm.position - transform.position);
            targetRotation *= Quaternion.Euler(0, 0, -90f);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * .5f);
        }
    }
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("FishDetector")) {
            isReeled = true;
            isFollowing = false;
            //Debug.Log("Flying");
        }
        if(other.CompareTag("Bobber") && isReeled == false) {
            bobber = other.GetComponent<Bobber>();
            if(bobber.hasFishOnLine == true) {
                return;
            }

            isFollowing = true;
            isCaught = true;
            bobberTransForm = other.transform;

            Vector3 direction = (bobberTransForm.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Fisher")) {
            man = other.gameObject.GetComponent<Fisherman>();
            man.Damaged(damage);
        }
    }

    public void Damaged(int damage) {
        currHealth -= damage;
        Debug.Log("Hit for " + damage);
        
        if (currHealth <= 0) {
            Die();
        }
    }

    void Die() {
        man = FindObjectOfType<Fisherman>();
        man.AddMoney(moneyWorth);
        man.RestoreHealth();
        Destroy(this.gameObject);
    }

    // AI Stuff

    public void HopToward(float movement, float forwardMultiplier, float upwardMulitplier) {
        if (!hopping) {
            float horizontal = (movement * hopSpeed) * forwardMultiplier;
            float vertical = (Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * hopHeight)) * upwardMulitplier;

            hopping = true;
            rb.velocity = new Vector2(horizontal, vertical);

            StartCoroutine(JumpingCoolDownRoutine());
            IEnumerator JumpingCoolDownRoutine() {
                yield return new WaitForSeconds(hopCooldown);
                hopping = false;
            }
        }
    }

    public void HopAway(float movement) {
        if (!hopping) {
            float horizontal = -movement * hopSpeed;
            float vertical = Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * hopHeight);

            hopping = true;
            rb.velocity = new Vector2(horizontal, vertical);

            StartCoroutine(JumpingCoolDownRoutine());
            IEnumerator JumpingCoolDownRoutine() {
                yield return new WaitForSeconds(hopCooldown);
                hopping = false;
            }
        }
    }

    public void MoveFish(Vector3 goalPos, int toward) {
        goalPos.z = 0;
        Vector3 direction = (goalPos - transform.position).normalized;
        if (direction.x > 0) {
            sr.flipX = true;
            bc.offset = new Vector2(-Mathf.Abs(bc.offset.x), bc.offset.y);
        } else if (direction.x < 0) {
            sr.flipX = false;
            bc.offset = new Vector2(Mathf.Abs(bc.offset.x), bc.offset.y);
        }
        if (toward == 1) { //move toward
            HopToward(direction.x, 1f, 1f);
        } else if (toward == 2) { // move away
            HopToward(direction.x, -1f, 1f);
            //HopAway(direction.x);
        } else if (toward == 3) { // big jump toward / attack
            HopToward(direction.x, attackMultiplier, 1f);
        } else if (toward == 4) { // big jump over
            HopToward(direction.x, 1f, jumpMultiplier);
        }
    }

    public void Stop() {
        rb.velocity = Vector3.zero;
    }
}
