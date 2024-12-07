using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAI : MonoBehaviour
{
    [SerializeField] public Fish currFish;
    [SerializeField] Fisherman targetFisherman;
    [SerializeField] float fishSightDistance = 5f;
    float randomChanceTimePast = 0f;
    float randomAttackTimePast = 0f;
    bool isIdling = false;

    delegate void AIState();
    AIState currentState;


    float stateTime = 0;
    bool justChangedState = false;
    Vector3 lastTargetPos;

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(IdleState);
    }

    void Update()
    {
        if (currFish != null) {
          AITick();  
        }
    }

    void AITick(){
        if(justChangedState){
            stateTime = 0;
            justChangedState = false;
        }
        currentState();
        stateTime += Time.deltaTime;

    }

    void ChangeState(AIState newAIState){
        currentState = newAIState;
        justChangedState = true;
    }

    bool CanSeeTarget(){
        if(targetFisherman == null){
            return false;
        }

        return Vector3.Distance(currFish.transform.position, targetFisherman.transform.position) < fishSightDistance;
    }

    void IdleState(){
        if (CanSeeTarget())
        {
            ChangeState(MoveState);
            return;
        }
       
    }

    void MoveState() {
        if(Vector3.Distance(currFish.transform.position, targetFisherman.transform.position) > currFish.attackRange){
            float randomChance = Random.value;
            randomChanceTimePast += Time.deltaTime;

            if (randomChanceTimePast >= currFish.movementChanceTimer) {
                randomChanceTimePast = 0f;
                if (randomChance <= currFish.moveTowardChance) {
                    Debug.Log("moving toward");
                    currFish.MoveFish(targetFisherman.transform.position, 1);
                } else if (randomChance <= currFish.moveTowardChance + currFish.moveAwayChance) {
                    currFish.MoveFish(targetFisherman.transform.position, 2);
                    Debug.Log("moving away");
                } else {
                    Debug.Log("idling");
                    StartCoroutine(IdleTimeRoutine());
                }
            }

        } else {
            if (Vector3.Distance(currFish.transform.position, targetFisherman.transform.position) <= currFish.attackRange) {
                ChangeState(AttackState);
            }
            return;
        }

        if(!CanSeeTarget())
        {
            lastTargetPos = targetFisherman.transform.position;
            ChangeState(IdleState);
            return;
        }

    }

    void AttackState(){
        if(Vector3.Distance(currFish.transform.position, targetFisherman.transform.position) <= currFish.attackRange){
            float randomChance = Random.value;
            randomAttackTimePast += Time.deltaTime;

            if (randomAttackTimePast >= currFish.attackChanceTimer) {
                randomAttackTimePast = 0f;
                if (randomChance <= currFish.attackChance) {
                    Debug.Log("attacking");
                    currFish.MoveFish(targetFisherman.transform.position, 3);
                } else if (randomChance <= currFish.attackChance + currFish.jumpOverChance) {
                    currFish.MoveFish(targetFisherman.transform.position, 4);
                    Debug.Log("big jump");
                } else {
                    Debug.Log("attack idling");
                    StartCoroutine(IdleTimeRoutine());
                }
            }
        } else {
            ChangeState(MoveState);
        }

    }

    IEnumerator IdleTimeRoutine() {
        if(!isIdling) {
            isIdling = true;
            yield return new WaitForSeconds(Random.Range(currFish.idleTimeMin, currFish.idleTimeMax));
            isIdling = false;
        }
    }
}
