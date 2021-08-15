using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LaserShooter
{
    public class PatrolState : IState
    {
        private LSFSM manager;
        private Parameter parameter;

        private int patrolPointNum;

        private float timer;
        private int patrolPosition;
        public PatrolState(LSFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
            this.patrolPointNum = parameter.patrolPointPositions.Length;
        }
        public void OnEnter()
        {
            //    parameter.animator.Play("Walk");
        }

        public void OnUpdate()
        {
            timer += Time.deltaTime;
            
            if (patrolPosition >= patrolPointNum)
            {
                patrolPosition = 0;
            }
           
            manager.FlipTo(parameter.patrolPointPositions[patrolPosition]);

            manager.transform.position = Vector2.MoveTowards(manager.transform.position,
                parameter.patrolPointPositions[patrolPosition], parameter.moveSpeed * Time.deltaTime);

            if (parameter.getHit)
            {
                manager.TransitionState(LSStateType.Hit);
            }
            if (parameter.target != null && timer >= parameter.intervalTime) 
            {
                timer = 0;
                manager.TransitionState(LSStateType.Shoot);
            }
            if (Vector2.Distance(manager.transform.position, parameter.patrolPointPositions[patrolPosition]) < .1f)
            {
                patrolPosition++;
                
                if (patrolPosition >= patrolPointNum)
                {
                    patrolPosition = 0;
                }
            }


        }

        public void OnExit()
        {
            
        }
    }

    public class ShootState : IState
    {
        private LSFSM manager;
        private Parameter parameter;

        //   private AnimatorStateInfo info;
        private float timer;
        public ShootState(LSFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            //   parameter.animator.Play("Shoot");
            parameter.hasShot = false;
        }

        public void OnUpdate()
        {
            //   info = parameter.animator.GetCurrentAnimatorStateInfo(0);
            timer += Time.deltaTime;
            if (timer > 2 && !manager.parameter.hasShot) Shoot();
            if (timer > 4) manager.TransitionState(LSStateType.Patrol);
            if (parameter.getHit)
            {
                parameter.health--;
                if (parameter.health <= 0)
                {
                    manager.TransitionState(LSStateType.Death);
                }
                //没死播放受伤特效但不切换状态
            }/*
            if (info.normalizedTime >= .5f && info.normalizedTime < .8f && !manager.parameter.hasShot)  //这里调整到合适的动画时间发射
            {
                Shoot();
            }
            
            if (info.normalizedTime >= .95f)
            {
                manager.TransitionState(LSStateType.Idle);
            }*/
        }

        public void OnExit()
        {
            parameter.shooting = false;
            timer = 0;
        }
        public void Shoot()
        {
            GameObject thisLaser;
            parameter.shooting = true;
            Quaternion rotation = manager.transform.rotation;
            thisLaser = GameObject.Instantiate(parameter.laser, manager.transform.position, rotation);//此处方向改为朝上
            parameter.hasShot = true;
            thisLaser.transform.Rotate(new Vector3(0, 0, 90));
            thisLaser.GetComponent<Laser>().laseShooter = manager.gameObject;
        }

    }

    public class HitState : IState
    {
        private LSFSM manager;
        private Parameter parameter;

        //   private AnimatorStateInfo info;

        public HitState(LSFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            //    parameter.animator.Play("Hit");
            parameter.health--;
        }

        public void OnUpdate()
        {
            //info = parameter.animator.GetCurrentAnimatorStateInfo(0);

            if (parameter.health <= 0)
            {
                manager.TransitionState(LSStateType.Death);
            }
            /*if (info.normalizedTime >= .95f)
            {
                manager.TransitionState(LSStateType.Idle);
            }*/
            manager.TransitionState(LSStateType.Patrol);
        }

        public void OnExit()
        {
            parameter.getHit = false;
        }
    }

    public class DeathState : IState
    {
        private LSFSM manager;
        private Parameter parameter;

        public DeathState(LSFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            //     parameter.animator.Play("Dead");
        }

        public void OnUpdate()
        {

        }

        public void OnExit()
        {

        }
    }
}