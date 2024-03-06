using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//格子类型
public enum E_Node_Type
{
    //可以通过的地方
    Walk,
    //阻挡物
    Stop,
}


//A*格子类
public class AStartNode 
{  //格子对象坐标
    public int x;
    public int y;

    //寻路消耗
    public float f;
    //离起点的距离
    public float g;
    //离终点的距离
    public float h;
    //父对象
    public AStartNode father;
    //格子类型
    public E_Node_Type type;
    public AStartNode(int x, int y, E_Node_Type type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }
        
}
