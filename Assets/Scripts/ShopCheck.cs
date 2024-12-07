using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthShop : MonoBehaviour
{
    [SerializeField] Fisherman fisher;
    [SerializeField] public string whichShop = "none";

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fisher")) {
            fisher.shop = gameObject.tag;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Fisher")) {
            fisher.shop = "none";
        }
    }
}
