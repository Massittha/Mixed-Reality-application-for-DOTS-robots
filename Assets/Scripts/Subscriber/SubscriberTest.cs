using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Talker = RosMessageTypes.Std.StringMsg;

public class SubscriberTest : MonoBehaviour
{
    public GameObject cube;

    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<Talker>("chatter", SayHello);
        print("Connected");
    }

    void SayHello(Talker talker)
    {
        print(talker.data);
    }
}