using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class playerCombat : MonoBehaviour
{
    [SerializeField] AnimationStateChanger stateChanger;
    [SerializeField] int attackStrength;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] LayerMask fishLayer;
    [SerializeField] bool showWireSphere = true;
    [SerializeField] float attackRate = 1f;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;
    float nextAttackTime = 0f;


    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAttackTime ) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                //Debug.Log("Attack!");
                stateChanger.ChangeAnimationState("FisherManAttack");
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack() {
        Collider2D[] hitFish = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, fishLayer);
        audioSource.clip = audioClips[0];
        audioSource.Play();

        foreach(Collider2D fish in hitFish) {
            //Debug.Log("Hit " + fish.name);
            fish.GetComponent<Fish>().Damaged(attackStrength);
            audioSource.clip = audioClips[1];
            audioSource.Play();
        }
    }

    void OnDrawGizmosSelected()
    {
       if (attackPoint == null || showWireSphere == false) {
        return;
       }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void IncreaseAttack(int increase) {
        attackStrength += increase;
    }

    public int GetAttack() {
        return attackStrength;
    }

    public void SetAttack(int attack) {
        attackStrength = attack;
    }
}
