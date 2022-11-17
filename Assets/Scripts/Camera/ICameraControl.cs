using UnityEngine;

public interface ICameraControl
{
    Camera GetCamera();
    void SetGameObject(Transform transform);
    void CameraZoomCalculation(Vector3 pos);
}
