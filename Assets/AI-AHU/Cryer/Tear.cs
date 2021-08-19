using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cryer
{


    public class Tear : MonoBehaviour
    {
        float maxExistTime = 2;
        float timeCount = 0;
        public Animator animator;
        void Start()
        {

        }


        private void FixedUpdate()
        {
            timeCount += Time.fixedDeltaTime;
            if(timeCount > maxExistTime)
            {
                Destroy(gameObject);
            }
        }
        //private void OnTriggerEnter2D(Collider2D other)
        //{

            
        //    if (other.CompareTag("Player") || other.CompareTag("Ground"))
        //    {
        //        Destroy(gameObject);
        //    }
        //}


    }
}