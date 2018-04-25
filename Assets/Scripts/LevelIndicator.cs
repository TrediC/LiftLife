using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameController
{

    public class LevelIndicator : MonoBehaviour
    {

        ClickerController clicker;
        public GameObject Arrow;
        public float arrowRotation;
        private float rotX;
        private float rotY;
        public bool levelFinish = false;
        private float rotationMount;
        Quaternion endRot;


        // Use this for initialization
        void Start()
        {
            clicker = GameObject.Find("GameController").GetComponent<ClickerController>();
            rotationMount = 180 / clicker.floors; // 180 is full rotation of arrow (-90 to 90)
            rotX = transform.rotation.x;
            rotY = transform.rotation.y;
        }

        private void Update()
        {
            if (levelFinish)
                transform.Rotate(0, 0, Mathf.Lerp(arrowRotation, arrowRotation + rotationMount, Time.deltaTime / clicker.waitTime));
            
        }
        public void RotateArrow()
        {
            rotationMount++;
        }
    }
}