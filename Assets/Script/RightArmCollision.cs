using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightArmCollision : MonoBehaviour
{
    public BalancePickup BalancePickup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is the specific child we're interested in
        if (other.gameObject.name == "Right") // Make sure the name matches the child object's name
        {
            BalancePickup.OnRightArmCollisionEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Right")
        {
            BalancePickup.OnRightArmCollisionExit();
        }
    }

}
