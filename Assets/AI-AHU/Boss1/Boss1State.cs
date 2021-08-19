using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Boss1
{
    public class IdleState : IState
    {
        private Boss1FSM manager;
        private Parameter parameter;

        private float timer;
        public IdleState(Boss1FSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            //WaveEyes闭眼动画
            
        }

        public void OnUpdate()
        {
            timer += Time.deltaTime;
            if (timer >= parameter.attackInterval)
            {
                manager.TransitionState(Boss1StateType.React);
            }

        }
        public void OnExit()
        {
            
            timer = 0;
        }
    }
    public class ReactState : IState
    {
        private Boss1FSM manager;
        private Parameter parameter;
        private Vector3 direction;
        public ReactState(Boss1FSM manager)
        {
            
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            
            parameter.attackMode = parameter.attackCycle[parameter.attackTurn];
            //
            if (parameter.attackMode > 2)
            {
                parameter.attackHeight = parameter.heightMarks[1].position.y;
            }
            else
            {
                parameter.attackHeight = parameter.heightMarks[0].position.y;
            }

            if (parameter.attackMode % 2 == 0)
            {
                parameter.usingEye = parameter.WaveEyes[1];
                parameter.waveTerminalx = parameter.rangeMarks[0].position.x;
            }
            else
            {
                parameter.usingEye = parameter.WaveEyes[0];
                parameter.waveTerminalx = parameter.rangeMarks[1].position.x;
            }

            //
            parameter.eyePosition = parameter.usingEye.transform.position;
            parameter.p1 = new Vector3(parameter.eyePosition.x, parameter.attackHeight, 0);
            parameter.p2 = new Vector3(parameter.waveTerminalx, parameter.attackHeight, 0);
            direction = parameter.p1 - parameter.usingEye.transform.position;
        }

        public void OnUpdate()
        {
            parameter.usingEye.transform.Translate(direction * Time.deltaTime * parameter.wEMovingSpeed);
            if (Vector3.Distance(parameter.usingEye.transform.position, parameter.p1) <0.1f)
            {
                manager.TransitionState(Boss1StateType.Attack);
            }
        }


        public void OnExit()
        {
            
        }
    }
    public class AttackState : IState
    {
        private Boss1FSM manager;
        private Parameter parameter;
        private Vector3 direction;

        public AttackState(Boss1FSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            
            direction = parameter.p2 - parameter.usingEye.transform.position;
        }

        public void OnUpdate()
        {
            parameter.usingEye.transform.Translate(direction * Time.deltaTime * parameter.wEAttackSpeed, Space.World);
            if (Vector3.Distance(parameter.usingEye.transform.position, parameter.p2) < 0.1f)
            {
                manager.TransitionState(Boss1StateType.Restore);
            }
        }


        public void OnExit()
        {

        }
    }
    public class RestoreState : IState
    {
        private Boss1FSM manager;
        private Parameter parameter;
        private Vector3 direction;

        public RestoreState(Boss1FSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            direction = parameter.eyePosition - parameter.usingEye.transform.position;
        }

        public void OnUpdate()
        {
            parameter.usingEye.transform.Translate(direction * Time.deltaTime*parameter.wEMovingSpeed, Space.World);
            if (Vector3.Distance(parameter.usingEye.transform.position, parameter.eyePosition) < 0.1f)
            {
                manager.TransitionState(Boss1StateType.Idle);
            }
        }


        public void OnExit()
        {
            parameter.attackTurn++;
            if (parameter.attackTurn >= parameter.attackCycle.Length)
            {
                parameter.attackTurn = 0;
            }
        }
    }
    public class DeathState : IState
    {
        private Boss1FSM manager;
        private Parameter parameter;

        public DeathState(Boss1FSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            //动画
        }

        public void OnUpdate()
        {

        }


        public void OnExit()
        {

        }
    }
}