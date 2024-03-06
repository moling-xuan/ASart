using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AStartMgr :BaseManager<AStartMgr>
{
    //A*Ѱ·����������ģʽ
  
    //��ͼ���
    private int mapW;
    private int mapH; 
    //��ͼ������и��Ӷ�������
    public AStartNode[,] nodes;
    //�����б�
    private List<AStartNode> openList=new List<AStartNode>();
    //�ر��б�
    private List<AStartNode> closeList=new List<AStartNode>();
    //��ʼ����ͼ��Ϣ
    public void InitMapINfo(int w,int h)
    {
        //������������
        this.mapW = w;
        this.mapH = h;

        nodes = new AStartNode[w, h];
        for (int i = 0; i < w;++i)
        {
            for (int j = 0; j < h; ++j)
            {
                AStartNode node = new AStartNode(i, j,Random.Range(0,100)<30?E_Node_Type.Stop:E_Node_Type.Walk);
                nodes[i, j] = node;
            }
        }
    }
    public List<AStartNode> FindPath(Vector2 startPos,Vector2 endPos)
    {
         
        

        //�����ж� ����������� �Ƿ�Ϸ� 
        //�ڵ�ͼ��Χ�ڣ��Ƿ����赲
        //������Ϸ� ��ֱ�ӷ���null ��ζ�Ų���Ѱ·
        //����Ϸ�  ��õ���Ӧ���յ������Ӧ�ĸ���
        if (startPos.x < 0 || startPos.x >= mapW ||
            startPos.y < 0 || startPos.y >= mapH ||
            endPos.x < 0 || endPos.x >= mapW ||
            endPos.y < 0 || endPos.y >= mapH)
            
        {
            Debug.Log("��ʼ����߽����㲻�Ϸ�");
            return null;
        }
        AStartNode start = nodes[(int)startPos.x, (int)startPos.y];
        AStartNode end = nodes[(int)endPos.x, (int)endPos.y];
        if (start.type == E_Node_Type.Stop || end.type == E_Node_Type.Stop)
        {
            Debug.Log("��ʼ����߽����㱻�ڵ�");
            return null;
        }
        //�����һ�ε�����
        closeList.Clear();
        openList.Clear();
        start.father = null;
        start.f = 0;
        start.h = 0;
        start.g = 0;
        //��ʼ�����OpenList
        //����㿪ʼ����Χ�ĵ� �ж��Ƿ��Ǳ߽��赲���Ѿ����б����򲻷��룩
        closeList.Add(start);
        while (true)
        {
            //����
            FindNarlyNodeToOpenList(start.x - 1, start.y - 1, 1.4f, start, end);
            //��
            FindNarlyNodeToOpenList(start.x, start.y - 1, 1, start, end);
            //����
            FindNarlyNodeToOpenList(start.x + 1, start.y - 1, 1.4f, start, end);
            //��
            FindNarlyNodeToOpenList(start.x - 1, start.y, 1, start, end);
            //��
            FindNarlyNodeToOpenList(start.x + 1, start.y, 1, start, end);
            //����
            FindNarlyNodeToOpenList(start.x - 1, start.y + 1, 1.4f, start, end);
            //��
            FindNarlyNodeToOpenList(start.x, start.y + 1, 1, start, end);
            //����
            FindNarlyNodeToOpenList(start.x + 1, start.y + 1, 1.4f, start, end);
            //���openlist����Ϊ�ջ�δ�ҵ���ʼ����ΧΪ��·
            if (openList.Count == 0)
            {
                Debug.Log("��·");
                return null;
            }
                
            //����С���ķ���CloseList���ٴ�OpenList���Ƴ� 
            openList.Sort(SortOpenList);
            closeList.Add(openList[0]);
            //���˵�ѡΪ�������
            start = openList[0];
            openList.RemoveAt(0);
            //����Ѿ����յ�����ѽ������������������Ѱ·
            if (start == end)
            {
                List<AStartNode> path = new List<AStartNode>();
                path.Add(end);
                while (end.father != null)
                {
                    path.Add(end.father);
                    end = end.father;
                }
                //��ת�б�
                path.Reverse();
                return path;
            }


            
        }
    }
    //Openlist ������
    private int SortOpenList(AStartNode a, AStartNode b)
    {
        if (a.f > b.f)
            return 1;
        else if (a.f == b.f)
            return 1;
        else
            return -1;
    }
    //�ٽ��б���뿪���б� ���ҽ���СѰ·���ĵĵ����ر��б�
    private void FindNarlyNodeToOpenList(int x, int y,float g,AStartNode father,AStartNode end)
    {
        //�߽��ж�
        if (x < 0 || x >= mapH || y < 0 || y >= mapW)
            return;
        //ȡ��
        AStartNode node = nodes[x, y];
        //�ж��Ƿ��赲 �Ƿ����б��� 
        if (node == null || node.type == E_Node_Type.Stop||
            closeList.Contains(node)||openList.Contains(node))
            return;
        //����Ѱ·����f(f=g+h)
        node.father = father;
        node.g = g + father.g;
        node.h = Mathf.Abs(end.x - node.x) + Mathf.Abs(end.y - node.y);
        node.f = node.g + node.h;
        //�Ϸ����뿪���б���

        openList.Add(node);




    }
}
