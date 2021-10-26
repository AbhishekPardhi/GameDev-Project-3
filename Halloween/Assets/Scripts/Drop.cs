using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drop : MonoBehaviour
{
    RaycastHit hit; 
    float Reach = 2.0f;
    bool RayHit;
    Camera cam;
    Vector3 pos = new Vector3(1, 1, 0);


    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(pos);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
    }
    void ObjectDetection()
    {
        var fwd = transform.TransformDirection (Vector3.forward);
        Debug.DrawRay(transform.position, fwd * Reach, Color.yellow);
         
         if (Physics.Raycast (transform.position, fwd, Reach)&&hit.transform.gameObject.tag == "Objects") 
         {
             RayHit = true;
             Debug.Log("You can pick up the object");
         }
         else 
         {
            RayHit = false;
         }
     }
}
