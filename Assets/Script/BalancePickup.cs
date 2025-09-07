using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancePickup : MonoBehaviour
{
    public GameObject leftArm;
    public GameObject rightArm;
    public bool isLeftArmColliding = false;
    public bool isRightArmColliding = false;
    public bool isHolding = false;

    public GameObject ball;
    public GameObject left_side;
    public GameObject right_side;

    private string leftconfigName = "Left Device";
    private double[] lefthapticForce = new double[3] { 0, 0, 0 };
    private double[] lefthapticTorque = new double[3] { 0, 0, 0 };

    private string rightconfigName = "Right Device";
    private double[] righthapticForce = new double[3] { 0, 0, 0 };
    private double[] righthapticTorque = new double[3] { 0, 0, 0 };


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isLeftArmColliding && isRightArmColliding) //Input.GetKeyDown(KeyCode.Z) && 
        {
            isHolding = true;
        }
        if (Input.GetKeyDown(KeyCode.X) && isHolding)
        {
            ReleasePlate();

        }

        if (isHolding == true)
        {
            HoldPlate();
        }
    }
    void HoldPlate()
    {
        Vector3 centerPosition = (leftArm.transform.position + rightArm.transform.position) / 2;
        transform.position = centerPosition;

        Vector3 direction = rightArm.transform.position - leftArm.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z);

        isHolding = true;


        float left_length = Mathf.Abs(left_side.transform.position.z - ball.transform.position.z);
        float right_length = Mathf.Abs(right_side.transform.position.z - ball.transform.position.z);
        float total_length = Mathf.Abs(left_length - right_length);
        float total_force = 1f;

        lefthapticForce = new double[3] { 0, left_length / total_length * total_force, 0 };
        righthapticForce = new double[3] { 0, right_length / total_length * total_force, 0 };

        HapticPlugin.setForce(leftconfigName, lefthapticForce, lefthapticTorque);
        HapticPlugin.setForce(rightconfigName, righthapticForce, righthapticTorque);
    }

    void ReleasePlate()
    {
        // Add logic here if you need to reset the rotation or perform other actions upon release
        isHolding = false;

        lefthapticForce = new double[3] { 0, 0, 0 };
        righthapticForce = new double[3] { 0, 0, 0 };

        HapticPlugin.setForce(leftconfigName, lefthapticForce, lefthapticTorque);
        HapticPlugin.setForce(rightconfigName, righthapticForce, righthapticTorque);
    }

    // Assume these are called by Unity's physics engine when the appropriate collisions occur
    public void OnLeftArmCollisionEnter()
    {
        isLeftArmColliding = true;
    }

    public void OnLeftArmCollisionExit()
    {
        isLeftArmColliding = false;
    }

    public void OnRightArmCollisionEnter()
    {
        isRightArmColliding = true;
    }

    public void OnRightArmCollisionExit()
    {
        isRightArmColliding = false;
    }

}
