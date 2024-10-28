using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingCheck : MonoBehaviour
{

    [SerializeField] Fisherman fisher;
    [SerializeField] bool canFish = false;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fisher")) {
            canFish = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Fisher")) {
            canFish = false;
        }
    }

    public bool GetCanFish() {
        return canFish;
    }
}
