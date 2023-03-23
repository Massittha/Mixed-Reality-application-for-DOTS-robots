using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Lifter = RosMessageTypes.Std.BoolMsg;

/// <summary>
///
/// </summary>
public class LifterOn : MonoBehaviour
{
    ROSConnection ros;
    public string topicName;
    public bool data;

    // Publish the cube's position and rotation every N seconds
    public float publishMessageFrequency = 0.5f;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;

    void Start()
    {
        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<Lifter>(topicName);
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
            Lifter lifter = new Lifter(data);

            // Finally send the message to server_endpoint.py running in ROS
            //ros.Publish(topicName, lifterOn);
            ros.Publish(topicName, lifter);
            timeElapsed = 0;
        }
    }
}