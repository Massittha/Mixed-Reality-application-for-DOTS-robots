using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Ground = RosMessageTypes.Nav.OdometryMsg;

public class GroundSubscriber : MonoBehaviour
{
    public GameObject cube;
    private float timeElapsed;
    public float timer = 0.0f;

    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<Ground>("/ground_truth", Show);
        print("Odom Interface Connected");
    }

    void Show(Ground ground)
    {

        if (timeElapsed > timer)
        {

            print("pos_X:" + ground.pose.pose.position.x);
            print("pos_Y:" + ground.pose.pose.position.y);
            print("pos_Z:" + ground.pose.pose.position.z);


            timeElapsed = 0;
        }

    }
    private void Update()
    {
       timeElapsed += Time.deltaTime;
    }
}