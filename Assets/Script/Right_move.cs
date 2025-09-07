using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Right_move : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float rotateSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();

    }
    void Move()
    {
        // Get input from the numeric keypad or arrow keys
        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.Keypad8))
        {
            moveZ = 1;
        }
        if (Input.GetKey(KeyCode.Keypad5))
        {
            moveZ = -1;
        }
        if (Input.GetKey(KeyCode.Keypad4))
        {
            moveX = -1;
        }
        if (Input.GetKey(KeyCode.Keypad6))
        {
            moveX = 1;
        }

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
    }

    void Rotate()
    {
        if (Input.GetKey(KeyCode.Keypad7))
        {
            transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Keypad9))
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        }
    }


}
