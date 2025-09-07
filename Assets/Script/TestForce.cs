using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TestForce : MonoBehaviour
{
    public string configName = "Default Device";
    public Vector3 hapticForce = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 hapticTorque = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 gForce = new Vector3(0.0f, 0.0f, 0.0f);
    // Start is called before the first frame update
    void Start()
    {

    }

    private static double[] Vector3ToDoubleArray(Vector3 vec)
    {
        double[] darray = new double[3];

        darray[0] = vec.x;
        darray[1] = vec.y;
        darray[2] = vec.z;

        return darray;
    }
    // Update is called once per frame
    void Update()
    {
        //HapticPlugin.setForce(configName, Vector3ToDoubleArray(hapticForce), Vector3ToDoubleArray(hapticTorque));
        HapticPlugin.setGravityForce(configName, Vector3ToDoubleArray(gForce));
    }
}
