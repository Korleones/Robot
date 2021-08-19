using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LaserEye
{
    public class IdleState : IState
    {
        private LEFSM manager;
        private Parameter parameter;

        private float timer;
        public IdleState(LEFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            parameter.animator.Play("Idle");
        }

        public void OnUpdate()
        {
            timer += Time.deltaTime;

            if (parameter.getHit)
            {
                parameter.health--;
                if (parameter.health <= 0)
                {
                    manager.TransitionState(LEStateType.Death);
                }
                //红一下
            }
            if (parameter.target != null&& timer >= parameter.idleTime)
            {
                manager.TransitionState(LEStateType.Shoot);
            }

        }

        public void OnExit()
        {
            timer = 0;
        }
    }

    public class ShootState : IState
    {
        private LEFSM manager;
        private Parameter parameter;

        private AnimatorStateInfo info;

        public ShootState(LEFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            parameter.animator.Play("Shoot");
            parameter.hasShot = false;
        }

        public void OnUpdate()
        {
            info = parameter.animator.GetCurrentAnimatorStateInfo(0);

            if (parameter.getHit)
            {
                parameter.health--;
                if (parameter.health <= 0)
                {
                    manager.TransitionState(LEStateType.Death);
                }
                //没死播放受伤特效
            }
            if (info.normalizedTime >= .5f && info.normalizedTime < .8f && !manager.parameter.hasShot)  //这里调整到合适的动画时间发射
            {
                Shoot();
            }

            if (info.normalizedTime >= .95f)
            {
                manager.TransitionState(LEStateType.Idle);
            }
        }

        public void OnExit()
        {
            parameter.shooting = false;
        }
        public void Shoot()
        {
            parameter.shooting = true;
            Vector3 v = parameter.target.position - manager.transform.position;
            v.z = 0;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, v);
            GameObject.Instantiate(parameter.laser, manager.transform.position, rotation);//方向
            parameter.hasShot = true;
        }

    }


    public class DeathState : IState
    {
        private LEFSM manager;
        private Parameter parameter;

        public DeathState(LEFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            parameter.animator.Play("Dead");
            parameter.dead = true;
        }

        public void OnUpdate()
        {

        }

        public void OnExit()
        {

        }
    }
}