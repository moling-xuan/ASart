using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//��������
public enum E_Node_Type
{
    //����ͨ���ĵط�
    Walk,
    //�赲��
    Stop,
}


//A*������
public class AStartNode 
{  //���Ӷ�������
    public int x;
    public int y;

    //Ѱ·����
    public float f;
    //�����ľ���
    public float g;
    //���յ�ľ���
    public float h;
    //������
    public AStartNode father;
    //��������
    public E_Node_Type type;
    public AStartNode(int x, int y, E_Node_Type type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }
        
}
