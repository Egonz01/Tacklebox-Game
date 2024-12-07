using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] public bool hasFishOnLine = false;
    [SerializeField] float fishLineTime = 1f;
    Rigidbody2D rb;
    bool waiting = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(float movement) {
        rb.velocity = new Vector2(rb.velocity.x, movement * moveSpeed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fish") && !waiting)
        {
            hasFishOnLine = true;
            //Debug.Log("Reel!");
            StartCoroutine(FishLineRoutine());
        }
    }

    IEnumerator FishLineRoutine() {
        //Debug.Log("Waiting");
        waiting = true;
        yield return new WaitForSeconds(fishLineTime);
        waiting = false;
    }

    public void IncreaseSpeed(float increase) {
        moveSpeed += increase;
    }

    public float GetSpeed() {
        return moveSpeed;
    }

    public void SetSpeed(float speed) {
        moveSpeed = speed;
    }
}
