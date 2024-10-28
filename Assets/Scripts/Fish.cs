using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [Header("Spawn Parameters")]
    [SerializeField] public float swimSpeed = 1f;
    [SerializeField] public float despawnTime = 3f;
    [SerializeField] float followSpeed = 5f;
    [SerializeField] public bool isCaught = false;
    [SerializeField] GameObject fish;

    [Header("AI Parameters")]
    [SerializeField] float moveSpeed = 1f;


    Transform bobberTransForm;
    Rigidbody2D rb;
    Bobber bobber;
    Vector3 move = Vector3.zero;
    bool isFollowing = false;
    bool isReeled = false;
    public bool onLand = false;

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
        if (isFollowing)
        {
            rb.velocity = Vector2.zero;

            Vector3 followPosition = new Vector3(bobberTransForm.position.x, bobberTransForm.position.y - 0.5f, bobberTransForm.position.z);
            Vector3 direction = (followPosition - transform.position).normalized;
            transform.position += direction * followSpeed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, bobberTransForm.position - transform.position);
            targetRotation *= Quaternion.Euler(0, 0, -90f);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * .5f);
        }
    }
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("FishDetector")) {
            isReeled = true;
            isFollowing = false;
            Debug.Log("Flying");
        }
        if(other.CompareTag("Bobber") && isReeled == false) {
            bobber = other.GetComponent<Bobber>();
            if(bobber.hasFishOnLine == true) {
                return;
            }

            isFollowing = true;
            isCaught = true;
            bobberTransForm = other.transform;

            Vector3 direction = (bobberTransForm.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        
    }

    // AI Stuff

    public void Move(float movement) {
        rb.velocity = new Vector2(movement * moveSpeed, rb.velocity.y);
    }

    public void MoveToward(Vector3 goalPos) {
        goalPos.z = 0;
        Vector3 direction = (goalPos - transform.position).normalized;
        Move(direction.x);
    }

    public void Stop() {
        move = Vector3.zero;
    }
}
