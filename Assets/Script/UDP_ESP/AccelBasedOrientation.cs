using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelBasedOrientation : MonoBehaviour
{
    public IMUReceiverUDP imuReceiver;
    private Vector3 smoothAccel = Vector3.zero;
    private float smoothingFactor = 0.1f;

    // Update is called once per frame
    void Update()
    {
        if (imuReceiver == null) return;

        float[] imu = imuReceiver.GetIMUData();

        Vector3 rawaccel = new Vector3(imu[0], imu[1], imu[2]);

        smoothAccel = Vector3.Lerp(smoothAccel, rawaccel, smoothingFactor);

        if (smoothAccel.sqrMagnitude < 0.01f || smoothAccel.magnitude > 3f) return;
        Vector3 gravityDir = smoothAccel.normalized;

        Quaternion targetRotation = Quaternion.FromToRotation(gravityDir, Vector3.down);

        transform.rotation = targetRotation;

    }
}
