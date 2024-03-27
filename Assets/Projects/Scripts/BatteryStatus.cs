using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BatteryStatus : MonoBehaviour
{
    [SerializeField] private TextMeshPro _textMesh;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _textMesh.text = SystemInfo.batteryLevel +"";

    }
}
