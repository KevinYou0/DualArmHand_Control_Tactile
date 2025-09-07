//using Microsoft.MixedReality.Toolkit;
//using Microsoft.MixedReality.Toolkit.Input;
//using Microsoft.MixedReality.Toolkit.Utilities;
//using System;
//using System.Collections.Generic;
//using UnityEngine;
//public class HandPoseAligner : MonoBehaviour
//{
//    // Visual spheres for each joint
//    private Dictionary<TrackedHandJoint, GameObject> jointSpheres = new Dictionary<TrackedHandJoint, GameObject>();

//    public float sphereSize = 0.01f;
//    public Material jointMaterial;
//    public Material palmMaterial;

//    // Public reference to alignment target
//    public GameObject targetObject;

//    // Relative fingertip positions to palm
//    private Vector3 relThumb, relIndex, relMid, relRing;

//    // Track these joints
//    private TrackedHandJoint[] trackedJoints = new TrackedHandJoint[]
//    {
//        TrackedHandJoint.Palm,
//        TrackedHandJoint.ThumbTip,
//        TrackedHandJoint.IndexTip,
//        TrackedHandJoint.MiddleTip,
//        TrackedHandJoint.RingTip,
//    };

//    void Start()
//    {
//        foreach (TrackedHandJoint joint in trackedJoints)
//        {
//            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//            sphere.transform.localScale = Vector3.one * sphereSize;
//            sphere.name = "Left_" + joint.ToString();

//            if (joint == TrackedHandJoint.Palm && palmMaterial != null)
//                sphere.GetComponent<Renderer>().material = palmMaterial;
//            else if (jointMaterial != null)
//                sphere.GetComponent<Renderer>().material = jointMaterial;

//            Destroy(sphere.GetComponent<Collider>());
//            jointSpheres[joint] = sphere;
//        }
//    }

//    void Update()
//    {
//        // Track hand pose only if palm joint is available
//        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Left, out MixedRealityPose palmPose))
//        {
//            Quaternion flipY180 = Quaternion.Euler(0, 0, 180f);
//            //Quaternion rotatedPalmRotation = palmPose.Rotation * flipY180;

//            // Capture all relative fingertip positions
//            foreach (TrackedHandJoint joint in trackedJoints)
//            {
//                if (joint == TrackedHandJoint.Palm)
//                    continue;

//                if (HandJointUtils.TryGetJointPose(joint, Handedness.Left, out MixedRealityPose tipPose))
//                {
//                    Vector3 tipWorld = tipPose.Position;
//                    //Vector3 relativeTipPosition = Quaternion.Inverse(rotatedPalmRotation) * (tipWorld - palmPose.Position);

//                    Vector3 relativeTipPosition = (tipWorld - palmPose.Position);
//                    switch (joint)
//                    {
//                        case TrackedHandJoint.ThumbTip:
//                            relThumb = relativeTipPosition;
//                            break;
//                        case TrackedHandJoint.IndexTip:
//                            relIndex = relativeTipPosition;
//                            break;
//                        case TrackedHandJoint.MiddleTip:
//                            relMid = relativeTipPosition;
//                            break;
//                        case TrackedHandJoint.RingTip:
//                            relRing = relativeTipPosition;
//                            break;
//                    }
//                }
//            }

//            // Align palm and reconstruct tip positions relative to targetObject
//            if (targetObject != null)
//            {
//                Vector3 alignedPalmPosition = targetObject.transform.position;
//                Quaternion alignedPalmRotation = targetObject.transform.rotation;

//                // Visualize aligned palm
//                if (jointSpheres.TryGetValue(TrackedHandJoint.Palm, out GameObject palmSphere))
//                {
//                    palmSphere.transform.position = alignedPalmPosition;
//                    palmSphere.transform.rotation = alignedPalmRotation;
//                }

//                // Reconstruct tips
//                foreach (TrackedHandJoint joint in trackedJoints)
//                {
//                    if (joint == TrackedHandJoint.Palm)
//                        continue;

//                    Vector3 relTip = Vector3.zero;
//                    switch (joint)
//                    {
//                        case TrackedHandJoint.ThumbTip: relTip = relThumb; break;
//                        case TrackedHandJoint.IndexTip: relTip = relIndex; break;
//                        case TrackedHandJoint.MiddleTip: relTip = relMid; break;
//                        case TrackedHandJoint.RingTip: relTip = relRing; break;
//                    }

//                    Vector3 tipWorldPos = alignedPalmPosition + alignedPalmRotation * relTip;
//                    Quaternion tipRot = alignedPalmRotation;

//                    if (jointSpheres.TryGetValue(joint, out GameObject tipSphere))
//                    {
//                        tipSphere.transform.position = tipWorldPos;
//                        tipSphere.transform.rotation = tipRot;
//                    }
//                }
//            }
//        }
//    }
//}


using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class handTracker_left : MonoBehaviour
{
    public float sphereSize = 0.01f;
    public Material jointMaterial;
    public Material palmMaterial;

    public GameObject targetObject;

    private Dictionary<TrackedHandJoint, GameObject> jointSpheres = new Dictionary<TrackedHandJoint, GameObject>();
    private Dictionary<TrackedHandJoint, Vector3> relativePositions = new Dictionary<TrackedHandJoint, Vector3>();

    public GameObject leftThumb;
    public GameObject leftIndex;
    public GameObject leftMid;
    public GameObject leftRing;

    private TrackedHandJoint[] trackedJoints = new TrackedHandJoint[]
    {
            TrackedHandJoint.Palm,
            TrackedHandJoint.ThumbTip,
            TrackedHandJoint.IndexTip,
            TrackedHandJoint.MiddleTip,
            TrackedHandJoint.PinkyTip,
    };


    void Start()
    {
        foreach (TrackedHandJoint joint in System.Enum.GetValues(typeof(TrackedHandJoint)))
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = Vector3.one * sphereSize;
            sphere.name = "Left_" + joint.ToString();

            if (joint == TrackedHandJoint.Palm && palmMaterial != null)
                sphere.GetComponent<Renderer>().material = palmMaterial;
            else if (jointMaterial != null)
                sphere.GetComponent<Renderer>().material = jointMaterial;

            Destroy(sphere.GetComponent<Collider>());
            jointSpheres[joint] = sphere;
        }
    }

    void Update()
    {
        if (!HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Left, out MixedRealityPose palmPose))
            return;

        // Capture relative position of each joint in the palm's local coordinate frame
        foreach (TrackedHandJoint joint in jointSpheres.Keys)
        {
            if (HandJointUtils.TryGetJointPose(joint, Handedness.Left, out MixedRealityPose jointPose))
            {
                Vector3 relativePosition = Quaternion.Inverse(palmPose.Rotation) * (jointPose.Position - palmPose.Position);
                relativePositions[joint] = relativePosition;
            }
        }

        if (targetObject != null)
        {
            Vector3 alignedPalmPosition = targetObject.transform.position;
            Quaternion alignedPalmRotation = targetObject.transform.rotation;

            // Apply desired rotation offsets around local axes
            Quaternion rotX = Quaternion.AngleAxis(-90, targetObject.transform.right);
            Quaternion rotZ = Quaternion.AngleAxis(-90, targetObject.transform.forward);
            Quaternion finalPalmRotation = rotZ * rotX * alignedPalmRotation;

            foreach (var kvp in relativePositions)
            {
                TrackedHandJoint joint = kvp.Key;
                Vector3 relPos = kvp.Value;

                Vector3 newWorldPos = alignedPalmPosition + finalPalmRotation * relPos;

                if (jointSpheres.TryGetValue(joint, out GameObject sphere))
                {
                    sphere.transform.position = newWorldPos;
                    sphere.transform.rotation = finalPalmRotation;

                    switch (joint)
                    {
                        case TrackedHandJoint.ThumbTip:
                            leftThumb.transform.position = newWorldPos;
                            leftThumb.transform.rotation = finalPalmRotation;

                            break;
                        case TrackedHandJoint.IndexTip:
                            leftIndex.transform.position = newWorldPos;
                            leftIndex.transform.rotation = finalPalmRotation;
                            break;
                        case TrackedHandJoint.MiddleTip:
                            leftMid.transform.position = newWorldPos;
                            leftMid.transform.rotation = finalPalmRotation;
                            break;
                        case TrackedHandJoint.PinkyTip:
                            leftRing.transform.position = newWorldPos;
                            leftRing.transform.rotation = finalPalmRotation;
                            break;
                    }
                }
            }
        }
    }
}