using UnityEngine;
using System.Collections.Generic;

public class IKJointAngleReader_right : MonoBehaviour
{
    [Header("Assign all joint transforms in order (base tip)")]
    public Transform[] joints;

    [Header("Print angles every frame?")]
    public bool debugPrint = true;

    [Header("Live joint angles (degrees)")]
    public List<float> jointAnglesRad = new List<float>();
    

    void Start()
    {
        if (joints == null || joints.Length == 0)
        {
            Debug.LogError("No joints assigned!");
            return;
        }

        jointAnglesRad = new List<float>(new float[joints.Length]);
    }

    void Update()
    {
        for (int i = 0; i < joints.Length; i++)
        {
            float angle = NormalizeAngle(joints[i].localEulerAngles.x);  // or x/y if needed
            if (i == 0 || i == 4 || i == 8 || i == 13) 
            {
                angle = NormalizeAngle(joints[i].localEulerAngles.y);
            }
            
            if (i == 12)
            {
                angle = NormalizeAngle(joints[i].localEulerAngles.z+180);
                print(angle);
            }
            float rad = angle * Mathf.Deg2Rad;

            jointAnglesRad[i] = rad;

            if (debugPrint)
                Debug.Log($"Joint {i}: {angle}°");
        }
    }

    // Normalize angle to [-180, 180]
    private float NormalizeAngle(float angle)
    {
        return (angle > 180f) ? angle - 360f : angle;
    }

}
