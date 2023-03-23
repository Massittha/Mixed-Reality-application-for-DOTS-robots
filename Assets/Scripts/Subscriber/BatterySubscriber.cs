using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Battery = RosMessageTypes.Sensor.BatteryStateMsg;
using TMPro;



public class BatterySubscriber : MonoBehaviour
{
    private float timeElapsed;
    public string topicName;
    public TextMeshPro textMesh;
    public float percentage;
    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<Battery>(topicName, Show);
        print("Battery State Indicator Connected");
        textMesh = GetComponent<TextMeshPro>();

    }

    void Show(Battery battery)
    {

        
            percentage = battery.percentage;
            textMesh.text = "Battery Level: " + Mathf.RoundToInt(percentage).ToString() + "%";
            timeElapsed = 0;
        

    }
    private void Update()
    {
       timeElapsed += Time.deltaTime;
    }
}