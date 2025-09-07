using System;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class FingertipSensor : MonoBehaviour
{
    [Tooltip("Identifier sent back to the manager/UDP. " +
             "This is auto‑set by the manager, but you can override if needed.")]
    //public string fingerId;

    // current collision state
    public bool IsColliding;

    // fired whenever IsColliding changes
    public event Action<string, bool> OnCollisionStatusChanged;

    void Awake()
    {
        // ensure trigger + kinematic rigidbody
        var col = GetComponent<Collider>();
        col.isTrigger = true;
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void OnTriggerEnter(Collider other)
    {
        IsColliding = true;
        //if (!IsColliding)
        //    UpdateState(true);
    }
    void OnTriggerStay(Collider other)
    {
        IsColliding = true;
        //if (!IsColliding)
        //    UpdateState(true);
    }

    void OnTriggerExit(Collider other)
    {
        IsColliding = false;
        //if (IsColliding)
        //    UpdateState(false);
    }

    void UpdateState(bool newState)
    {
        IsColliding = newState;
    }
}
