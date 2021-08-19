using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Boss1
{

    public enum Boss1StateType
    {
        Idle,React, Attack,Restore, Death
    }

    [Serializable]
    public class Parameter
    {
        public float wEMovingSpeed, wEAttackSpeed;
        public Transform[] heightMarks;
        public Transform[] rangeMarks;
        public GameObject[] WaveEyes;
        public int[] attackCycle;//1：左上挥击(左眼在上方攻击)，2：右上挥击，3：左下挥击：4右下挥击
        public float attackInterval;
       
        public float attackHeight;
        public float waveTerminalx;
        public int attackMode;
        public GameObject usingEye;
        public Vector3 eyePosition,p1,p2;
        public int attackTurn;


    }
    public class Boss1FSM : MonoBehaviour
    {
        private IState currentState;
        private Dictionary<Boss1StateType, IState> states = new Dictionary<Boss1StateType, IState>();

        public Parameter parameter;
        


        void Start()
        {
            states.Add(Boss1StateType.Idle, new IdleState(this));
            states.Add(Boss1StateType.React, new ReactState(this));
            states.Add(Boss1StateType.Attack, new AttackState(this));
            states.Add(Boss1StateType.Restore, new RestoreState(this));
            states.Add(Boss1StateType.Death, new DeathState(this));

            parameter.attackTurn = 0;
            TransitionState(Boss1StateType.Idle);

        }

        // Update is called once per frame
        void Update()
        {
            currentState.OnUpdate();
        }
        public void TransitionState(Boss1StateType type)
        {
            if (currentState != null)
                currentState.OnExit();
            currentState = states[type];
            currentState.OnEnter();
        }
    }
}