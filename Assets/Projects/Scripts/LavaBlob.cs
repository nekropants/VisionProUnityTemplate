using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBlob : MonoBehaviour
{
    
    [SerializeField] private Transform _center;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _multiplier = 1;
    [SerializeField] public float direction = 1;
    // Start is called before the first frame update
    void Start()
    {
        if (Random.value > 0.5f)
        {
            direction = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 delta = _center.position - transform.position;
        // float y = delta.y*direction;
        Vector3 force = Vector3.up * (direction );
        Debug.DrawRay(transform.position, force);
        _rigidbody.AddForce(force * (Time.deltaTime * _multiplier), ForceMode.Impulse);
    }
}
