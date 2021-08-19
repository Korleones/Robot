using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LaserShooter
{
    public enum LSStateType
    {
        Patrol, Shoot, Hit, Death
    }

    [Serializable]
    public class Parameter
    {
        public int health;
        public float intervalTime;
        public float moveSpeed;
        public Transform[] patrolPoints;
        public Vector3[] patrolPointPositions;
        public Transform target;
        // public Animator animator;
        public GameObject laser;
        public bool getHit;
        public bool hasShot;
        public bool shooting;
    }
    public class LSFSM : MonoBehaviour
    {
        private IState currentState;
        private Dictionary<LSStateType, IState> states = new Dictionary<LSStateType, IState>();

        public Parameter parameter;

        void Start()
        {
            parameter.patrolPointPositions = new Vector3[parameter.patrolPoints.Length];  
            for (int i = 0; i < parameter.patrolPoints.Length; i++)
            {
                parameter.patrolPointPositions[i] = parameter.patrolPoints[i].position;
            }
            transform.DetachChildren();

            states.Add(LSStateType.Patrol, new PatrolState(this));
            states.Add(LSStateType.Shoot, new ShootState(this));
            states.Add(LSStateType.Hit, new HitState(this));
            states.Add(LSStateType.Death, new DeathState(this));



            TransitionState(LSStateType.Patrol);
        }


        void Update()
        {
            currentState.OnUpdate();

            //if (Input.GetKeyDown(KeyCode.Return))
            //{
            //    parameter.getHit = true;
            //}                                           //此处改为接受来自player的信息

        }
        public void TransitionState(LSStateType type)
        {
            if (currentState != null)
                currentState.OnExit();
            currentState = states[type];
            currentState.OnEnter();
        }
        public void FlipTo(Transform target)
        {
            if (target != null)
            {
                if (transform.position.x > target.position.x)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else if (transform.position.x < target.position.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
        public void FlipTo(Vector3 target)
        {
            if (target != null)
            {

                Vector3 v = target - transform.position;
                v.z = 0;
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, v);
                transform.rotation = rotation;
                /*
                if (transform.position.x > target.x)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else if (transform.position.x < target.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                */
            }
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            
            if (other.CompareTag("Player"))
            {
                parameter.target = other.transform;
            }
        }


        private void OnTriggerExit2D(Collider2D other)
        {

            if (other.CompareTag("Player"))
            {
                parameter.target = null;
            }
        }
    }
}