using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseTracker : MonoBehaviour
{
    private Camera mainCamera;
    private float originalSize;
    private float maxSize = 20f;


    [SerializeField]
    private Transform horse;

    public bool focusOnHorse = false;

    [SerializeField]
    private float focusOrthographicSize;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        originalSize = mainCamera.orthographicSize;
    }

    private void Update()
    {
        if (focusOnHorse)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, focusOrthographicSize, Time.deltaTime);
        }
        else
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, Mathf.Clamp(horse.position.y + originalSize, originalSize, maxSize), Time.deltaTime);
        }
    }
}