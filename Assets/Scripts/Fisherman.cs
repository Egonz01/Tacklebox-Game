using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fisherman : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float jumpPower = 5f;
    [SerializeField] float jumpCooldown = 1;
    [Header("PlayerData")]
    [SerializeField] public int currMoney;
    [SerializeField] int attackStrength = 1;
    [SerializeField] float bobberSpeed = 3f;
    [SerializeField] int maxHealth = 3;
    [SerializeField] int currHealth = 3;
    [SerializeField] Transform attackPoint;
    Rigidbody2D rb;
    SpriteRenderer sr;
    BoxCollider2D bc;
    [Header("Shop")]
    [SerializeField] public string shop;
    [SerializeField] playerCombat combat;
    [SerializeField] Bobber bobber;
    [SerializeField] int damageIncrease = 1;
    [SerializeField] float bobberSpeedIncrease = 0.5f;
    [SerializeField] ShopController shopController;
    int healthPrice;
    int damagePrice;
    int speedPrice;
    bool jumping = false;
    [SerializeField] AudioSource audioSource;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        NDSaveLoad.LoadFromFile();
        maxHealth = NDSaveLoad.LoadInt("Health", 3);
        Debug.Log($"health is {maxHealth}");
        currHealth = maxHealth;

        attackStrength = NDSaveLoad.LoadInt("Damage", 1);
        Debug.Log($"attack is {attackStrength}");
        combat.SetAttack(attackStrength);

        bobberSpeed = NDSaveLoad.LoadFloat("Bobber", 3f);
        Debug.Log($"speed is {bobberSpeed}");
        bobber.SetSpeed(bobberSpeed);

        healthPrice = NDSaveLoad.LoadInt("HealthPrice", 5);
        shopController.setPrice(1, healthPrice);

        damagePrice = NDSaveLoad.LoadInt("DamagePrice", 3);
        shopController.setPrice(2, damagePrice);

        speedPrice = NDSaveLoad.LoadInt("SpeedPrice", 2);
        shopController.setPrice(3, speedPrice);

        currMoney = NDSaveLoad.LoadInt("Money", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FishZone")) {
            shopController.SetText(1);
        }
        else if (other.CompareTag("HealthShop")) {
            shopController.SetText(2);
        }
        else if (other.CompareTag("DamageShop")) {
            shopController.SetText(3);
        }
        else if (other.CompareTag("BobberShop")) {
            shopController.SetText(4);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        shopController.RemoveText();
    }

    public void Move(float movement) {
        rb.velocity = new Vector2(movement * moveSpeed, rb.velocity.y);
        if (movement == 1) {
            sr.flipX = false;
            attackPoint.localPosition = new Vector3(Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, attackPoint.localPosition.z);
            bc.offset = new Vector2(-Mathf.Abs(bc.offset.x), bc.offset.y);
        } else if(movement == -1) {
            sr.flipX = true;
            attackPoint.localPosition = new Vector3(-Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, attackPoint.localPosition.z);
            bc.offset = new Vector2(Mathf.Abs(bc.offset.x), bc.offset.y);
        }
    }

    public void Jump() {
        if (!jumping) {
            jumping = true;
            rb.velocity = new Vector3(rb.velocity.x, jumpPower);
            StartCoroutine(JumpingCoolDownRoutine());
            IEnumerator JumpingCoolDownRoutine() {
                yield return new WaitForSeconds(jumpCooldown);
                jumping = false;
            }
        }
    }

    public void Damaged(int damage) {
        currHealth -= damage;
        audioSource.Play();

        if (currHealth <= 0) {
            Die();
        }
    }

    void Die() {
        Debug.Log("You Died!");
        NDSaveLoad.SaveInt("Money", 0);
        NDSaveLoad.SaveInt("SpeedPrice", 2);
        NDSaveLoad.SaveFloat("Bobber", 3f);
        NDSaveLoad.SaveInt("DamagePrice", 3);
        NDSaveLoad.SaveInt("Damage", 1);
        NDSaveLoad.SaveInt("HealthPrice", 5);
        NDSaveLoad.SaveInt("Health", 3);
        NDSaveLoad.Flush();
        SceneManager.LoadScene("MainMenu");
    }

    public void AddMoney(int money) {
        currMoney += money;
        NDSaveLoad.SaveInt("Money", currMoney);
        NDSaveLoad.Flush();
    }

    public void RemoveMoney(int money) {
        currMoney -= money;
        NDSaveLoad.SaveInt("Money", currMoney);
        NDSaveLoad.Flush();
    }

    public void RestoreHealth() {
        currHealth = maxHealth;
    }

    public int GetMoney() {
        return currMoney;
    }

    public int GetHealth() {
        return currHealth;
    }

    public int GetDamage() {
        return combat.GetAttack();
    }

    public void BuyHealth(int price) {
        maxHealth += 1;
        currHealth = maxHealth;
        RemoveMoney(price);
        shopController.IncreasePrice(1);
        NDSaveLoad.SaveInt("Health", maxHealth);
        NDSaveLoad.Flush();
    }

    public void BuyDamage(int price) {
        combat.IncreaseAttack(damageIncrease);
        RemoveMoney(price);
        shopController.IncreasePrice(2);
        NDSaveLoad.SaveInt("Damage", combat.GetAttack());
        NDSaveLoad.Flush();
    }

    public void BuyBobberSpeed(int price) {
        bobber.IncreaseSpeed(bobberSpeedIncrease);
        RemoveMoney(price);
        shopController.IncreasePrice(3);
        NDSaveLoad.SaveFloat("Bobber", bobber.GetSpeed());
        NDSaveLoad.Flush();
    }
}
