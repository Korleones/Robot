using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Boss1
{
    
    public class Boss1 : MonoBehaviour
    {
        public Parameter parameter;
        public float timer;
        public int attackMode;

        public float attackHeight;
        public float waveTerminalx;//
        public GameObject usingEye;

        private int attackTurn;
        
        private void Awake()
        {
            timer = 0;
            attackTurn = 0;
        }
        void Start()
        {
            
        }

        
        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= parameter.attackInterval)
            {
                timer -= parameter.attackInterval;
                Attack();    
            }
        }
        
        void Attack()
        {
            attackMode = parameter.attackCycle[attackTurn];
            //
            if (attackMode > 2)
            {
                attackHeight = parameter.heightMarks[1].position.y;
            }
            else
            {
                attackHeight = parameter.heightMarks[0].position.y;
            }

            if (attackMode % 2 == 0)
            {
                usingEye = parameter.WaveEyes[1];
                waveTerminalx = parameter.rangeMarks[0].position.x;
            }
            else
            {
                usingEye = parameter.WaveEyes[0];
                waveTerminalx = parameter.rangeMarks[1].position.x;
            }

            //

            

            attackTurn++;
            if (attackTurn >= parameter.attackCycle.Length)
            {
                attackTurn = 0;
            }
        }
    }
}
