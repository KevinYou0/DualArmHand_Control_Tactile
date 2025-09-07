using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouPoseTransfer : MonoBehaviour
{
    public Transform Original_pose;
    private Quaternion addRot1 = Quaternion.Euler(0, 180, 0);
    private Quaternion addRotLeft = Quaternion.Euler(0, 0, 180);

    public Quaternion midrot1;
    public Quaternion midrot2;
    public Quaternion midrot3;
    public Quaternion test0;
    public Quaternion test1;
    public Quaternion test2;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Original_pose.position.z, -Original_pose.position.x, Original_pose.position.y);
        midrot1 = addRot1 * new Quaternion(Original_pose.rotation.x, Original_pose.rotation.y, Original_pose.rotation.z, Original_pose.rotation.w);
        midrot2 = new Quaternion(midrot1.x, -midrot1.y, midrot1.z, -midrot1.w);
        midrot3 = addRotLeft * new Quaternion(midrot2.x, midrot2.y, midrot2.z, midrot2.w);
        test1 = new Quaternion(midrot3.z, midrot3.x, -midrot3.y, -midrot3.w);
        test2 = new Quaternion(Original_pose.rotation.y, Original_pose.rotation.w, -Original_pose.rotation.z, -Original_pose.rotation.x);
        test0 = new Quaternion(Original_pose.rotation.x, Original_pose.rotation.y, Original_pose.rotation.z, Original_pose.rotation.w);
        transform.rotation = test1;
        //transform.rotation = new Quaternion((float)1.0, (float)0.0, (float)0.0, (float)0.0);
    }
}
