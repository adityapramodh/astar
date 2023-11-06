using System;
using UnityEngine;

public class MoveObj : MonoBehaviour
{
    private Camera mainCamera;
    private float cameraDistanceZ;

    // Use this for Initialization.
    void Start()
    {
        // Main camera
        mainCamera = Camera.main;

        // Z axis of the game object for the screen view.
        cameraDistanceZ = mainCamera.WorldToScreenPoint(transform.position).z; 
    }
    void OnMouseDrag()
    {
        // z  axis added to screen point.
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDistanceZ);

        //screenPosition.x = Mathf.Clamp(screenPosition.x, -0.5f, 0.5f);

        // Screen Point converted to World Point.
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(screenPosition); 
        transform.position = newPosition;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}