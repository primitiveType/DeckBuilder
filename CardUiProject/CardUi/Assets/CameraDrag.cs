using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    [SerializeField] public float m_Speed = 6f;

    [SerializeField] private CinemachineVirtualCamera m_Camera;

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            m_Camera.transform.Translate(new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"), 0) * m_Speed * -1);
            CameraManager.Instance.ActivateDragCamera();
        }
    }
}
