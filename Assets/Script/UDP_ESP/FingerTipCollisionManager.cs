using System;
using System.Collections.Generic;
using UnityEngine;

public class FingertipCollisionManager : MonoBehaviour
{
    //[Tooltip("Assign exactly 5 FingertipSensor components in order:\n" +
    //         "0 = Thumb, 1 = Index, 2 = Middle, 3 = Ring, 4 = Pinky")]
    //public FingertipSensor[] fingertipSensors = new FingertipSensor[4];

    //// Internal state map
    //private Dictionary<string, bool> fingerStates = new Dictionary<string, bool>();

    ///// <summary>
    ///// Fired when any single fingertip changes state.
    ///// Parameters: (fingerName, isColliding)
    ///// </summary>
    //public event Action<string, bool> OnFingerCollisionChanged;

    ///// <summary>
    ///// (Optional) Fired whenever any fingertip changes, providing full snapshot.
    ///// </summary>
    //public event Action<Dictionary<string, bool>> OnAnyFingerChanged;

    //void Awake()
    //{
    //    string[] fingerNames = { "Thumb", "Index", "Middle", "Ring", "Pinky" };

    //    if (fingertipSensors.Length != 4)
    //        Debug.LogWarning($"Expected 5 sensors, got {fingertipSensors.Length}");

    //    for (int i = 0; i < fingertipSensors.Length; i++)
    //    {
    //        var sensor = fingertipSensors[i];
    //        if (sensor == null)
    //        {
    //            Debug.LogError($"FingertipSensor slot {i} is null. Assign in inspector.");
    //            continue;
    //        }

    //        // Auto‑assign the fingerId
    //        sensor.fingerId = fingerNames[i];
    //        fingerStates[sensor.fingerId] = sensor.IsColliding;

    //        // Subscribe
    //        sensor.OnCollisionStatusChanged += HandleFingerChanged;
    //    }
    //}

    //private void HandleFingerChanged(string fingerId, bool isColliding)
    //{
    //    // Update internal map
    //    fingerStates[fingerId] = isColliding;

    //    // Fire the single-finger event
    //    OnFingerCollisionChanged?.Invoke(fingerId, isColliding);

    //    // (Optional) Fire the full snapshot event
    //    OnAnyFingerChanged?.Invoke(new Dictionary<string, bool>(fingerStates));
    //}

    // Array to store the collision status of the five objects
    public FingertipSensor[] fingertipSensors = new FingertipSensor[4];
    private bool[] collisionStatus = new bool[5];
    public int combinedBinaryCommand = 0;
    public string binaryString;
    // Predefined 5-bit binary strings for each object
    private int[] objectCollisionBinary = new int[5]
    {
        0b10001,  // Object 1 collision: 10000
        0b10010,  // Object 2 collision: 01000
        0b10100,  // Object 3 collision: 00100
        0b01001,  // Object 4 collision: 00010
        0b01010   // Object 5 collision: 00001
    };



    void Update()
    {

        for (int i = 0; i < fingertipSensors.Length; i++)
        {
            if (fingertipSensors[i].IsColliding) 
            {
                // Use bitwise OR to combine the binary string for this object
                combinedBinaryCommand |= objectCollisionBinary[i];
            }
            else
            {
                // Use bitwise AND with the negation to remove this object's binary string
                combinedBinaryCommand &= ~objectCollisionBinary[i];
            }
        }

        // Print the binary control command
        // Debug log to show the current combined binary command
        binaryString = Convert.ToString(combinedBinaryCommand, 2).PadLeft(5, '0');
        Debug.Log("Control Command (Binary): " + binaryString);
    }
}