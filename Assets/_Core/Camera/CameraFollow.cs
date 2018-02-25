using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform leftBump;
    [SerializeField] private Transform rightBump;
    [SerializeField] private float smoothing = 5f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        float x = Mathf.Lerp(transform.position.x, BumpOrthographic(), smoothing * Time.deltaTime);
        float y = transform.position.y;
        float z = transform.position.z;

        transform.position = new Vector3(x, y, z);
    }

    private bool IsBumpVisible(Transform bump)
    {
        Vector3 view = cam.WorldToViewportPoint(bump.position);
        return view.x >= 0 && view.x <= 1 && view.z > 0;
    }

    private float BumpOrthographic()
    {
        Debug.Assert(cam.orthographic);

        float horizontalSize = cam.orthographicSize * cam.aspect;
        float left = leftBump.position.x;
        float right = rightBump.position.x;
        
        if (target.position.x < left + horizontalSize)
            return left + horizontalSize;
        if (target.position.x > right - horizontalSize)
            return right - horizontalSize;
        return target.position.x;
    }
}