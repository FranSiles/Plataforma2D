using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private float speedBackground;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    //Update for cameras , at end of frame
    private void LateUpdate()
    {
        Vector3 backgroundMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(backgroundMovement.x * speedBackground, backgroundMovement.y, 0);
        lastCameraPosition = cameraTransform.position;
    }
}
