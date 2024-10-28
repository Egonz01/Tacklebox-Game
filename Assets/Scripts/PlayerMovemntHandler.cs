using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMovemntHandler : MonoBehaviour
{
    [Header("Attachments")]
    [SerializeField] Fisherman fisher;
    [SerializeField] FishingCheck fishingCheck;
    [SerializeField] Bobber bobber;

    [Header("Cameras")]
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera fishingCamera;

    [Header("Parameters")]
    [SerializeField] bool isFishing = false;
    [SerializeField] float waitTime = 1f;

    bool canSwitch = true;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera.enabled = true;
        fishingCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFishing)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                fisher.Jump();
            }
        }

        if (fishingCheck.GetCanFish())
        {

            if (Input.GetKeyDown(KeyCode.Space) && canSwitch)
            {
                if (!isFishing)
                {
                    StartFishing();
                }
                else
                {
                    StopFishing();
                }
            }

            StartCoroutine(FishSwitchCooldownRoutine());
        }
    }

    void FixedUpdate()
    {
        if (!isFishing)
        {
            float movement = 0;
            if (Input.GetKey(KeyCode.A))
            {
                movement = -1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                movement = 1;
            }

            fisher.Move(movement);
        }
        else {
            float movement = 0;
            if (Input.GetKey(KeyCode.S))
            {
                movement = -1;
            }
            if (Input.GetKey(KeyCode.W))
            {
                movement = 1;
            }

            bobber.Move(movement);
        }
    }

    void StartFishing()
    {
        Debug.Log("Started Fishing");
        isFishing = true;
        mainCamera.enabled = false;
        fishingCamera.enabled = true;
    }

    void StopFishing()
    {
        Debug.Log("Stopped Fishing");
        isFishing = false;
        mainCamera.enabled = true;
        fishingCamera.enabled = false;
    }

    IEnumerator FishSwitchCooldownRoutine(){
        canSwitch = false;
        yield return new WaitForSeconds(waitTime);
        canSwitch = true;
    }
}
