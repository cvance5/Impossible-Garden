using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float YawLimit;
    public float PitchLimit;

    public float RotationDeadening;

    private float minPitch;
    private float maxPitch;
    private float minYaw;
    private float maxYaw;

    private Vector3 _eulerAngles;
    private Vector3 _orientation;

    public void LookUpdate()
    {
        float yawAmount = Input.GetAxis("Mouse X");
        float pitchAmount = Input.GetAxis("Mouse Y");

        Vector3 targetAngles = _eulerAngles + new Vector3(pitchAmount, yawAmount, 0);

        float pitchDifference = 1 - (Mathf.Abs(targetAngles.x - _orientation.x) / PitchLimit);
        float yawDifference = 1 - (Mathf.Abs(targetAngles.y - _orientation.y) / YawLimit);

        pitchDifference = Mathf.Pow(pitchDifference, RotationDeadening);
        yawDifference = Mathf.Pow(yawDifference, RotationDeadening);

        _eulerAngles = new Vector3(Mathf.Lerp(_eulerAngles.x, targetAngles.x, pitchDifference),
                                   Mathf.Lerp(_eulerAngles.y, targetAngles.y, yawDifference),
                                   0);

        _eulerAngles = new Vector3(Mathf.Clamp(_eulerAngles.x, minPitch, maxPitch),
                                   Mathf.Clamp(_eulerAngles.y, minYaw, maxYaw),
                                   0);

        transform.eulerAngles = _eulerAngles;
    }

    public void OrientTowards(Vector3 target)
    {
        transform.LookAt(target);
        _eulerAngles = transform.eulerAngles;
        _orientation = transform.eulerAngles;

        minPitch = _eulerAngles.x - PitchLimit;
        maxPitch = _eulerAngles.x + PitchLimit;
        minYaw = _eulerAngles.y - YawLimit;
        maxYaw = _eulerAngles.y + YawLimit;
    }
}