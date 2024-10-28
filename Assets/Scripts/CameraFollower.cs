using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform cameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerTransform.position.x, cameraTransform.position.y, -10);
    }
}
