using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

//[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
[RequireComponent(requiredComponent:typeof(ARRaycastManager), requiredComponent2:typeof(ARPlaneManager))]

public class ARPlace : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    private ARRaycastManager aRRaycastManager;
    private ARPlaneManager aRPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRPlaneManager = GetComponent<ARPlaneManager>();
    }

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
        
    }
    private void OnDisable()
    {
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }
    

    private void FingerDown(EnhancedTouch.Finger finger)
    {
        if (finger.index != 0) return;

        if (aRRaycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon))
        //if(aRRaycastManager.Raycast(finger.currentTouch.screenPosition), 
        //hits,TrackableType.PlaneWithinPolygon)
        
        {
            foreach (ARRaycastHit hit in hits)
            {
                Pose pose = hit.pose;
                GameObject obj = Instantiate(prefab, pose.position, pose.rotation);

                if (aRPlaneManager.GetPlane(trackableId: hit.trackableId).alignment == PlaneAlignment.HorizontalUp)
                {
                    Vector3 position = obj.transform.position;
                    Vector3 cameraPosition = Camera.main.transform.position;
                    Vector3 direction = cameraPosition - position;
                    Vector3 targetRotationEuler = Quaternion.LookRotation
                        (forward: direction).eulerAngles;
                    Vector3 scaledEuler = Vector3.Scale(a: targetRotationEuler,
                        b: obj.transform.up.normalized);
                    Quaternion targetRotation = Quaternion.Euler
                        (euler: scaledEuler);
                    obj.transform.rotation = obj.transform.rotation * targetRotation;
                }
            }
        }
            
    }
}
