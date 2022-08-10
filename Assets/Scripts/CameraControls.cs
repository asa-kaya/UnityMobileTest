using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Lean.Touch;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CameraControls : MonoBehaviour
{
    public float XSensitivity;
    public float YSensitivity;
    
    CinemachineFreeLook cinemachine;
    LeanFingerFilter ff;

    // Start is called before the first frame update
    void Start()
    {
        cinemachine = GetComponent<CinemachineFreeLook>();
        ff = new LeanFingerFilter(true);
    }

    // Update is called once per frame
    void Update()
    {
        List<LeanFinger> fingers = ff.UpdateAndGetFingers();
        Vector2 screenDelta = LeanGesture.GetScreenDelta(fingers);

        // perform camera rotation
        cinemachine.m_XAxis.Value += screenDelta.x * XSensitivity;
        cinemachine.m_YAxis.Value -= screenDelta.y * YSensitivity;
    }
}
