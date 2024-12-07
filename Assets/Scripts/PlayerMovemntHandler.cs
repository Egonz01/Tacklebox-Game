using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovemntHandler : MonoBehaviour
{
    [Header("Attachments")]
    [SerializeField] Fisherman fisher;
    [SerializeField] FishingCheck fishingCheck;
    [SerializeField] Bobber bobber;
    [SerializeField] AnimationStateChanger stateChanger;

    [Header("Cameras")]
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera fishingCamera;

    [Header("Parameters")]
    [SerializeField] bool isFishing = false;
    [SerializeField] float waitTime = 1f;

    [Header("Parameters")]
    [SerializeField] TextMeshProUGUI AlertText;
    [SerializeField] ShopController shop;

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
        if (Input.GetKeyDown(KeyCode.E) && fisher.shop == "HealthShop" && fisher.currMoney >= shop.healthPrice) {
                Debug.Log("Health Get!");
                fisher.BuyHealth(shop.healthPrice);
                shop.SetText(2);
        }
        if (Input.GetKeyDown(KeyCode.E) && fisher.shop == "DamageShop" && fisher.currMoney >= shop.damagePrice) {
                Debug.Log("Damage Get!");
                fisher.BuyDamage(shop.damagePrice);
                shop.SetText(3);
        }
        if (Input.GetKeyDown(KeyCode.E) && fisher.shop == "BobberShop" && fisher.currMoney >= shop.speedPrice) {
                Debug.Log("Speed Get!");
                fisher.BuyBobberSpeed(shop.speedPrice);
                shop.SetText(4);
        }

        if (fishingCheck.GetCanFish())
        {

            if (Input.GetKeyDown(KeyCode.E) && canSwitch)
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

         if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("MainMenu");
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

            if (movement != 0) {
               stateChanger.ChangeAnimationState("FisherManWalk"); 
            } else {
                stateChanger.ChangeAnimationState("FisherManIdleFull");
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
        //Debug.Log("Started Fishing");
        isFishing = true;
        mainCamera.enabled = false;
        fishingCamera.enabled = true;
    }

    void StopFishing()
    {
        //Debug.Log("Stopped Fishing");
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
