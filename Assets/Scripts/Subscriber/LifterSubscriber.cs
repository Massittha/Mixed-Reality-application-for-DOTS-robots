using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Lifter = RosMessageTypes.Std.Float32Msg;

public class LifterSubscriber : MonoBehaviour
{
    public GameObject cube;
    private float timeElapsed;
    private float timer = 5.0f;

    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<Lifter>("/r1/lifter_pos", Show);
        print("Lifter Position Indicator Connected");
    }

    void Show(Lifter lifter)
    {

        if (timeElapsed > timer)
        {

            print(lifter);
            
            timeElapsed = 0;
        }

    }
    private void Update()
    {
       timeElapsed += Time.deltaTime;
    }
}