using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drop : MonoBehaviour
{
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
    
}
