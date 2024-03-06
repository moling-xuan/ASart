using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AStartMgr :BaseManager<AStartMgr>
{
    //A*寻路管理器单例模式
  
    //地图宽高
    private int mapW;
    private int mapH; 
    //地图相关所有格子对象容器
    public AStartNode[,] nodes;
    //开启列表
    private List<AStartNode> openList=new List<AStartNode>();
    //关闭列表
    private List<AStartNode> closeList=new List<AStartNode>();
    //初始化地图信息
    public void InitMapINfo(int w,int h)
    {
        //申明格子容器
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
         
        

        //首先判断 传入的两个点 是否合法 
        //在地图范围内，是否是阻挡
        //如果不合法 则直接返回null 意味着不能寻路
        //如果合法  则得到对应的终点和起点对应的格子
        if (startPos.x < 0 || startPos.x >= mapW ||
            startPos.y < 0 || startPos.y >= mapH ||
            endPos.x < 0 || endPos.x >= mapW ||
            endPos.y < 0 || endPos.y >= mapH)
            
        {
            Debug.Log("开始点或者结束点不合法");
            return null;
        }
        AStartNode start = nodes[(int)startPos.x, (int)startPos.y];
        AStartNode end = nodes[(int)endPos.x, (int)endPos.y];
        if (start.type == E_Node_Type.Stop || end.type == E_Node_Type.Stop)
        {
            Debug.Log("开始点或者结束点被遮挡");
            return null;
        }
        //清空上一次的数据
        closeList.Clear();
        openList.Clear();
        start.father = null;
        start.f = 0;
        start.h = 0;
        start.g = 0;
        //开始点放入OpenList
        //从起点开始找周围的点 判断是否是边界阻挡（已经在列表中则不放入）
        closeList.Add(start);
        while (true)
        {
            //左上
            FindNarlyNodeToOpenList(start.x - 1, start.y - 1, 1.4f, start, end);
            //上
            FindNarlyNodeToOpenList(start.x, start.y - 1, 1, start, end);
            //右上
            FindNarlyNodeToOpenList(start.x + 1, start.y - 1, 1.4f, start, end);
            //左
            FindNarlyNodeToOpenList(start.x - 1, start.y, 1, start, end);
            //右
            FindNarlyNodeToOpenList(start.x + 1, start.y, 1, start, end);
            //左下
            FindNarlyNodeToOpenList(start.x - 1, start.y + 1, 1.4f, start, end);
            //下
            FindNarlyNodeToOpenList(start.x, start.y + 1, 1, start, end);
            //右下
            FindNarlyNodeToOpenList(start.x + 1, start.y + 1, 1.4f, start, end);
            //如果openlist里面为空还未找到则开始点周围为死路
            if (openList.Count == 0)
            {
                Debug.Log("死路");
                return null;
            }
                
            //把最小消耗放入CloseList中再从OpenList中移除 
            openList.Sort(SortOpenList);
            closeList.Add(openList[0]);
            //将此点选为最新起点
            start = openList[0];
            openList.RemoveAt(0);
            //如果已经是终点了则把结果返回如果不是则继续寻路
            if (start == end)
            {
                List<AStartNode> path = new List<AStartNode>();
                path.Add(end);
                while (end.father != null)
                {
                    path.Add(end.father);
                    end = end.father;
                }
                //反转列表
                path.Reverse();
                return path;
            }


            
        }
    }
    //Openlist 排序函数
    private int SortOpenList(AStartNode a, AStartNode b)
    {
        if (a.f > b.f)
            return 1;
        else if (a.f == b.f)
            return 1;
        else
            return -1;
    }
    //临近列表放入开启列表 并且将最小寻路消耗的点放入关闭列表
    private void FindNarlyNodeToOpenList(int x, int y,float g,AStartNode father,AStartNode end)
    {
        //边界判断
        if (x < 0 || x >= mapH || y < 0 || y >= mapW)
            return;
        //取点
        AStartNode node = nodes[x, y];
        //判断是否阻挡 是否在列表中 
        if (node == null || node.type == E_Node_Type.Stop||
            closeList.Contains(node)||openList.Contains(node))
            return;
        //计算寻路消耗f(f=g+h)
        node.father = father;
        node.g = g + father.g;
        node.h = Mathf.Abs(end.x - node.x) + Mathf.Abs(end.y - node.y);
        node.f = node.g + node.h;
        //合法放入开启列表中

        openList.Add(node);




    }
}
