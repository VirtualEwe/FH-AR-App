//scaling

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class RaycastScript : MonoBehaviour
{
    public GameObject spawnPrefab;
    GameObject spawnedObject;
    bool objectSpawned;
    ARRaycastManager arrayman;
    Vector2 FirstTouch;
    Vector2 secondTouch;
    float distanceCurrent;
    float distancePrevious;
    bool firstPinch=true;
    List <ARRaycastHit> hits=new List<ARRaycastHit>();
    // Start is called before the first frame update
    void Start()
    {
        objectSpawned=false;
        arrayman=GetComponent<ARRaycastManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount>0 && !objectSpawned)
        {
            if(arrayman.Raycast(Input.GetTouch(0).position,hits,TrackableType.PlaneWithinPolygon))
            {
                var hitpose=hits[0].pose;
                spawnedObject=Instantiate(spawnPrefab,hitpose.position,hitpose.rotation);
                objectSpawned=true;
                

            }
        }
        if(Input.touchCount>1 && objectSpawned)
        {
            FirstTouch=Input.GetTouch(0).position;
            secondTouch=Input.GetTouch(1).position;
            distanceCurrent=secondTouch.magnitude-FirstTouch.magnitude;
            if(firstPinch)
            {
                distancePrevious=distanceCurrent;
                firstPinch=false;
            }
            if(distanceCurrent!=distancePrevious)
            {
                Vector3 scaleValue=spawnedObject.transform.localScale*(distanceCurrent/distancePrevious);
                spawnedObject.transform.localScale=scaleValue;
                distancePrevious=distanceCurrent;

            }

        }
        else{
            firstPinch=true;
        }
        
    }
}