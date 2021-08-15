using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Cryer
{
    public class IdleState : IState
    {
        private CryerFSM manager;
        private Parameter parameter;

        private float timer;
        public IdleState(CryerFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            //parameter.animator.Play("Idle");
        }

        public void OnUpdate()
        {
            timer += Time.deltaTime;

            if (parameter.getHit)
            {
                manager.TransitionState(CryerStateType.Hit);
            }
            if (timer >= parameter.idleTime)
            {
                if (parameter.findPlayer)
                {
                    manager.TransitionState(CryerStateType.Cry);
                }
                timer = 0;
            }

        }

        public void OnExit()
        {
            timer = 0;
        }
    }
    public class CryState : IState
    {
        private CryerFSM manager;
        private Parameter parameter;

        //private AnimatorStateInfo info;
        //测试用
        private float timer;
        public CryState(CryerFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            // parameter.animator.Play("Cry");
            parameter.cried = false;
        }

        public void OnUpdate()
        {
            timer += Time.deltaTime;
            if (timer > 1 && !parameter.cried) Cry();
            if(timer>2) manager.TransitionState(CryerStateType.Idle);
            //  info = parameter.animator.GetCurrentAnimatorStateInfo(0);
            if (parameter.getHit)
            {
                manager.TransitionState(CryerStateType.Hit);
            }/*
            if (info.normalizedTime >= .5f && info.normalizedTime < .8f && !parameter.cried)  //这里调整到合适的动画时间流出眼泪
            {
                Cry();
            }
            if (info.normalizedTime >= .95f)
            {
                manager.TransitionState(CryerStateType.Idle);
            }*/
        }

        public void OnExit()
        {
            timer = 0;
        }
        public void Cry()
        {
            GameObject.Instantiate(parameter.tear, manager.transform);
            parameter.cried = true;
        }

    }

    public class HitState : IState
    {
        private CryerFSM manager;
        private Parameter parameter;

       // private AnimatorStateInfo info;

        public HitState(CryerFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
           // parameter.animator.Play("Hit");
            parameter.health--;
        }

        public void OnUpdate()
        {
           // info = parameter.animator.GetCurrentAnimatorStateInfo(0);

            if (parameter.health <= 0)
            {
                manager.TransitionState(CryerStateType.Death);
            }
            manager.TransitionState(CryerStateType.Idle);
            /* if (info.normalizedTime >= .95f)
             {
                 manager.TransitionState(CryerStateType.Idle);
             }*/
        }

        public void OnExit()
        {
            parameter.getHit = false;
        }
    }

    public class DeathState : IState
    {
        private CryerFSM manager;
        private Parameter parameter;

        public DeathState(CryerFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
          //  parameter.animator.Play("Dead");
        }

        public void OnUpdate()
        {

        }

        public void OnExit()
        {

        }
    }

}
