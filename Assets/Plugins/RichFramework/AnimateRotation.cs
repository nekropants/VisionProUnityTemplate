using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateRotation : MonoBehaviour
{
    [SerializeField] private float _frequency =1;
    [SerializeField] private float _amplitude =20 ;
    [SerializeField] private Vector3 _euler = Vector3.up ;
     private Quaternion _offset = Quaternion.identity;
    
    

    // Update is called once per frame
    void Update()
    {
        transform.localRotation *= Quaternion.Inverse(_offset);
        var amplitude = (Mathf.Sin(Time.time * _frequency) * _amplitude);
        Debug.Log(amplitude);
        _offset = Quaternion.Euler(_euler.normalized * amplitude);
        transform.localRotation *= _offset;
    }
}
