using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Common;

namespace Lean.Gui
{
    public class Note : MonoBehaviour
    {
        // Start is called before the first frame update
        public Camera cam;
        PlayerMovement player;
        public AudioSource grab;
        public AudioSource slam;
        public GameObject noteUI;
        void Start()
        {
            player = FindObjectOfType<PlayerMovement>();
        }

        // Update is called once per frame
        void Update()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity) && Input.GetMouseButton(0))
            {
                if (hit.transform.tag == "Note" && !player.cantMove)
                {
                    player.cantMove = true;
                    if (slam.isPlaying) slam.Stop();
                    grab.Play();
                    noteUI.GetComponent<LeanWindow>().On = true;
                    Debug.Log("clicked");
                }
            }
        }
        public void note()
        {
            player.cantMove = false;
            if (grab.isPlaying) grab.Stop();
            slam.Play();
        }
    }
}