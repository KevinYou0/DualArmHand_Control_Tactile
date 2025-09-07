using UnityEngine;

public class IMUSyncObject : MonoBehaviour
{
    public IMUReceiverUDP imuReceiver;

    private MadgwickAHRS madgwick;
    private Quaternion imuToUnityCorrection;
    private Quaternion initialIMUQuat = Quaternion.identity;
    private bool initialAligned = false;

    public Vector3 offsetEuler = new Vector3(90f, 90f, 0f); // Example axis remap

    // Filter settling parameters
    private float filterSettleTime = 1.0f; // seconds to wait
    private float settleTimer = 0f;

    void Start()
    {
        madgwick = new MadgwickAHRS(1.0f / 100.0f);
        madgwick.Beta = 0.02f; // Lower gain for stability

        imuToUnityCorrection = Quaternion.Euler(offsetEuler);
        //imuToUnityCorrection = Quaternion.Euler(0,90,0);
    }

    void Update()
    {
        if (imuReceiver == null) return;

        float[] imuData = imuReceiver.GetIMUData();

        // Update Madgwick filter
        madgwick.Update(
            imuData[3] * Mathf.Deg2Rad, imuData[4] * Mathf.Deg2Rad, imuData[5] * Mathf.Deg2Rad,
            imuData[0], imuData[1], imuData[2]
        );

        // Convert to Unity Quaternion
        Quaternion currentIMUQuat = new Quaternion(
            madgwick.Quaternion[1],
            madgwick.Quaternion[2],
            madgwick.Quaternion[3],
            madgwick.Quaternion[0]
        );

        // Apply axis correction
        currentIMUQuat = imuToUnityCorrection * currentIMUQuat;

        // Wait for filter to settle first
        if (!initialAligned)
        {
            settleTimer += Time.deltaTime;
            if (settleTimer >= filterSettleTime)
            {
                // Now lock initial orientation
                initialIMUQuat = currentIMUQuat;
                initialAligned = true;
                Debug.Log("IMU initial alignment done.");
            }
            else
            {
                // Optionally: do nothing while settling, or keep Cube at identity rotation
                transform.rotation = Quaternion.identity;
                return;
            }
        }

        // Compute relative rotation from initial pose
        Quaternion deltaRotation = Quaternion.Inverse(initialIMUQuat) * currentIMUQuat;

        // Apply relative rotation to Cube
        transform.rotation = deltaRotation;
    }
}
