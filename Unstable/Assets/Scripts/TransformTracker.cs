using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTracker : MonoBehaviour
{
    [SerializeField]
    private Transform transformToTrack = null;
    [SerializeField]
    private float lerpSpeed = 0;

    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - transformToTrack.position;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, transformToTrack.position + offset, lerpSpeed * Time.deltaTime);
    }
}