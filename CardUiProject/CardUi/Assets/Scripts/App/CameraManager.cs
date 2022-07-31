using System.Collections;
using System.Collections.Generic;
using App.Utility;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviourSingleton<CameraManager>
{
   [SerializeField] private CinemachineVirtualCamera DragCamera;
   [SerializeField] private CinemachineVirtualCamera FollowCamera;

   public void ActivateDragCamera()
   {
      DragCamera.gameObject.SetActive(true);
      FollowCamera.gameObject.SetActive(false);
   }

   public void ActivateFollowCamera()
   {
      DragCamera.gameObject.SetActive(false);
      FollowCamera.gameObject.SetActive(true);
   }

   public void SetFollowTarget(GameObject target)
   {
      FollowCamera.Follow = target.transform;
   }
}
