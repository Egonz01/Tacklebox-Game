using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class Fisherman : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float jumpPower = 5f;
    [SerializeField] float jumpCooldown = 1;
    Rigidbody2D rb;
    bool jumping = false;

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
        rb.velocity = new Vector2(movement * moveSpeed, rb.velocity.y);
    }

    public void Jump() {
        if (!jumping) {
            jumping = true;
            rb.velocity = new Vector3(rb.velocity.x, jumpPower);
            StartCoroutine(JumpingCoolDownRoutine());
            IEnumerator JumpingCoolDownRoutine() {
                yield return new WaitForSeconds(jumpCooldown);
                jumping = false;
            }
        }
    }
}
