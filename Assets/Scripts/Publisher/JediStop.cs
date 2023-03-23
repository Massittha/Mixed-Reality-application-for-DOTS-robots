using UnityEngine;

//ROS Mod
using Unity.Robotics.ROSTCPConnector;
using Move = RosMessageTypes.Geometry.TwistMsg;
using Linear = RosMessageTypes.Geometry.Vector3Msg;
using Angular = RosMessageTypes.Geometry.Vector3Msg;

public class JediStop : MonoBehaviour
{
    

    ROSConnection ros;
    public string topicName;

    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<Move>(topicName);

    }

    // Update is called once per frame
    private void Update()
    {
            Linear linear = new Linear(0.0f, 0.0f, 0.0f);
            Angular angular = new Angular(0.0f, 0.0f, 0.0f);
           
            Move move = new Move(linear, angular);
            ros.Publish(topicName, move);
    }
   
}
