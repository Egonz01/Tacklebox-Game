using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAI : MonoBehaviour
{
    [SerializeField] public Fish currFish;
    [SerializeField] Fisherman targetFisherman;
    [SerializeField] float fishSightDistance = 5f;
    [SerializeField] float howClose = 2f;

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
        AITick();
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
        if(Vector3.Distance(transform.position, targetFisherman.transform.position) > howClose){
            currFish.MoveToward(targetFisherman.transform.position);
        }else{
            currFish.Stop();
        }

        if(!CanSeeTarget())
        {
            lastTargetPos = targetFisherman.transform.position;
            ChangeState(IdleState);
            return;
        }

    }
}
