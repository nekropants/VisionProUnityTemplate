using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionTrigger : MonoBehaviour
{
    [SerializeField] private int _direction = 1;

    private void OnTriggerEnter(Collider other)
    {
        LavaBlob lavaBlob = other.gameObject.GetComponent<LavaBlob>();

        lavaBlob.direction = _direction;
        
    }
}
