using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class CatchDetector : MonoBehaviour
{
    [SerializeField] Transform fishArcMid;
    [SerializeField] Transform fishArcEnd;
    [SerializeField] float flyTime;
    [SerializeField] Bobber bobber;
    [SerializeField] FishAI fishAI;
    Transform fishPostition;
    Rigidbody2D rb;
    Collider2D fishCollider;
    Fish currFish;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Bobber")) {
            bobber.hasFishOnLine = false;
        }
        if(other.CompareTag("Fish")) {
            bobber.hasFishOnLine = false;
            fishPostition = other.GetComponent<Transform>();
            currFish = other.GetComponent<Fish>();
            StartCoroutine(FishArcRoutine());
            rb = other.GetComponent<Rigidbody2D>();
            fishCollider = other.GetComponent<Collider2D>();
            rb.gravityScale = 1f;
            fishCollider.isTrigger = false;
            currFish.onLand = true;
            fishAI.currFish = currFish;

        }
    }

    IEnumerator FishArcRoutine() {
        float time = 0f;

        Vector3 start = fishPostition.position;
        Vector3 mid = fishArcMid.position;
        Vector3 end = fishArcEnd.position;

        while(time < flyTime) {
            fishPostition.transform.position = Vector3.Lerp(start, mid, time / flyTime);
            time += Time.deltaTime;
            yield return null;
        }

        time = 0;
        start = mid;

        while(time < flyTime) {
           fishPostition.transform.position = Vector3.Lerp(start, end, time / flyTime); 
           time += Time.deltaTime;
           yield return null;
        }

        fishPostition.position = end;
        fishPostition.rotation = Quaternion.Euler(0,0,0);
    }
}
