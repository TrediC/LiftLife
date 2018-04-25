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

        private float rotationMount;


        // Use this for initialization
        void Start()
        {
            clicker = GameObject.Find("GameController").GetComponent<ClickerController>();
            rotationMount = 180 / clicker.floors; // 180 is full rotation of arrow (-90 to 90)
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            
        }

        public void RotateArrow()
        {
            transform.Rotate(new Vector3(0, 0, arrowRotation));
            rotationMount++;
        }
    }
}