using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovablePlatform : Platform
{
    public Vector3[] point;     //存储转折点
    public bool circle;         //是否循环移动
    public float movingSpeed;

    int direction;      //定义移动的方向
    int startPoint;     //当前位移路径中的起点
    int endPoint;       //当前位移路径中的终点
    int pointNum;       //转折点的数量
    float positionX;      
    float positionY;      
    
    // Start is called before the first frame update
    void Start()
    {
        direction = 1;
        pointNum = point.Length;
        transform.position = point[0];
        startPoint = 0;
        endPoint = 1;
        positionX = (point[endPoint].x - point[startPoint].x) * Time.deltaTime * movingSpeed;
        positionY = (point[endPoint].y - point[startPoint].y) * Time.deltaTime * movingSpeed;
    }

    private void FixedUpdate()
    {
        if(circle)
        {
            AsACircle();
        }
        else
        {
            AsNotACitcle();
        }
    }

    void AsACircle()
    {
        if (Mathf.Abs(Vector3.Distance(transform.position, point[endPoint])) < 0.03f)
        {
            transform.position = point[endPoint];
            startPoint++;
            endPoint++;
            startPoint %= pointNum;
            endPoint %= pointNum;
            positionX = (point[endPoint].x - point[startPoint].x) * Time.deltaTime * movingSpeed;
            positionY = (point[endPoint].y - point[startPoint].y) * Time.deltaTime * movingSpeed;
        }
        transform.position += new Vector3(positionX, positionY, transform.position.z);
    }
    void AsNotACitcle()
    {
        if (Mathf.Abs(Vector3.Distance(transform.position, point[endPoint])) < 0.03f)
        {
            transform.position = point[endPoint];
            startPoint += direction;
            if (endPoint + direction >= pointNum || endPoint + direction < 0)
            {
                direction *= -1;
            }
            endPoint += direction;
            positionX = (point[endPoint].x - point[startPoint].x) * Time.deltaTime * movingSpeed;
            positionY = (point[endPoint].y - point[startPoint].y) * Time.deltaTime * movingSpeed;
        }
        transform.position += new Vector3(positionX, positionY, transform.position.z);
        transform.position += new Vector3(positionX, positionY, transform.position.z);
    }
}
