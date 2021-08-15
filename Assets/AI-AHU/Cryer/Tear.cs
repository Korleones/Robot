using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cryer
{


    public class Tear : MonoBehaviour
    {
        public Animator animator;
        void Start()
        {

        }


        void Update()
        {

        }
        private void OnTriggerEnter2D(Collider2D other)
        {

            
            if (other.CompareTag("Player") || other.CompareTag("Ground"))
            {
                //animator.Play("Bia");
                if (other.CompareTag("Player"))
                {
                    //让玩家受伤
                }
                Destroy(gameObject);
            }
        }


    }
}