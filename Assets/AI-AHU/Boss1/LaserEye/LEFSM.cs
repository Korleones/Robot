using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LaserEye
{
    public enum LEStateType
    {
        Idle, Shoot, Death
    }

    [Serializable]
    public class Parameter
    {
        public int health;
        public float idleTime;
        public Transform target;
        public Animator animator;
        public GameObject laser;
        public bool getHit;
        public bool hasShot;
        public bool shooting;
        public bool dead;
    }
    public class LEFSM : MonoBehaviour
    {
        private IState currentState;
        private Dictionary<LEStateType, IState> states = new Dictionary<LEStateType, IState>();

        public Parameter parameter;

        void Start()
        {
            states.Add(LEStateType.Idle, new IdleState(this));
            states.Add(LEStateType.Shoot, new ShootState(this));
            states.Add(LEStateType.Death, new DeathState(this));

            TransitionState(LEStateType.Idle);
        }


        void Update()
        {
            currentState.OnUpdate();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                parameter.getHit = true;
            }                                           //此处改为接受来自player的信息

        }
        public void TransitionState(LEStateType type)
        {
            if (currentState != null)
                currentState.OnExit();
            currentState = states[type];
            currentState.OnEnter();
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