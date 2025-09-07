using RosSharp.RosBridgeClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ForceActuator : MonoBehaviour
{
    public GameObject DefaultSubscriber;
    public GameObject LeftSubscriber;
    public GameObject RightSubscriber;
    public string defaultconfigName = "Default Device";
    public string leftconfigName = "Left Device";
    public string rightconfigName = "Right Device";

    public double[] defaulthapticForce = new double[3] { 0, 0, 0 };
    public double[] lefthapticForce = new double[3] { 0, 0, 0 };
    public double[] lefthapticTorque = new double[3] { 0, 0, 0 };
    public double[] righthapticForce = new double[3] { 0, 0, 0 };
    public double[] righthapticTorque = new double[3] { 0, 0, 0 };

    public double[] leftForce;
    public double[] rightForce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lefthapticForce = LeftSubscriber.GetComponent<LeftLoadSubscriber>().hapticForce;
        righthapticForce = RightSubscriber.GetComponent<RightLoadSubscriber>().hapticForce;
        //defaulthapticForce = DefaultSubscriber.GetComponent<DefaultLoadSubscriber>().hapticForce;

        double mg_lf = Math.Sqrt(lefthapticForce[0] * lefthapticForce[0] + lefthapticForce[1] * lefthapticForce[1] + lefthapticForce[2] * lefthapticForce[2]);
        leftForce = new double[3]
        {
            (-lefthapticForce[1]/mg_lf) * Math.Tanh(mg_lf),
            (-lefthapticForce[2]/mg_lf) * Math.Tanh(mg_lf),
            (lefthapticForce[0]/mg_lf) * Math.Tanh(mg_lf)
        };

        double mg_rt = Math.Sqrt(righthapticForce[0] * righthapticForce[0] + righthapticForce[1] * righthapticForce[1] + righthapticForce[2] * righthapticForce[2]);
        rightForce = new double[3]
        {
            (-righthapticForce[1]/mg_rt) * Math.Tanh(mg_rt),
            (-righthapticForce[2]/mg_rt) * Math.Tanh(mg_rt),
            (righthapticForce[0]/mg_rt) * Math.Tanh(mg_rt)
        };


        HapticPlugin.setGravityForce(leftconfigName, leftForce);
        HapticPlugin.setGravityForce(rightconfigName, rightForce);
        //HapticPlugin.setGravityForce(defaultconfigName, new double[3] { defaulthapticForce[1], defaulthapticForce[2], defaulthapticForce[1] });
    }
}
