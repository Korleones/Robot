using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Cryer
{
    public enum CryerStateType
    {
        Idle, Cry, Hit, Death
    }

    [Serializable]
    public class Parameter
    {
        public int health;
        public float idleTime;
        //public Animator animator;
        public GameObject tear;
        public bool findPlayer;
        public bool getHit;
        public bool cried;
    }

    public class CryerFSM : MonoBehaviour
    {

        private IState currentState;
        private Dictionary<CryerStateType, IState> states = new Dictionary<CryerStateType, IState>();

        public Parameter parameter;

        void Start()
        {
            states.Add(CryerStateType.Idle, new IdleState(this));
            states.Add(CryerStateType.Cry, new CryState(this));
            states.Add(CryerStateType.Hit, new HitState(this));
            states.Add(CryerStateType.Death, new DeathState(this));

            TransitionState(CryerStateType.Idle);

           // parameter.animator = transform.GetComponent<Animator>();
        }

        void Update()
        {
            currentState.OnUpdate();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                parameter.getHit = true;
            }                                           //此处改为接受来自player的信息
        }

        public void TransitionState(CryerStateType type)
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
                parameter.findPlayer = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                parameter.findPlayer = false;
            }
        }
    }
}