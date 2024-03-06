using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAStart : MonoBehaviour
{
    //第一个立方体位置
    public int beginX=-3;
    public int beginY=5;
    //间距
    public int offsetX;
    public int offsetY;
    //地图宽高
    public int mapW;
    public int mapH;
    //材质球 
    public Material red;
    public Material yellow;
    public Material green;
    public Material normal;
    List<AStartNode> list;

    private Vector2 beginPos = Vector2.right * -1;
    private Dictionary<string, GameObject> cubes = new Dictionary<string, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        AStartMgr.GerInstance().InitMapINfo(mapW, mapH);
        for (int i = 0; i < mapW; ++i)
        {
            for (int j = 0; j < mapH; ++j)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.position = new Vector3(beginX+i*offsetX, beginY+j*offsetY,0);

                //名字
                obj.name = i + "_" + j;
                cubes.Add(obj.name, obj); 
                
                AStartNode node = AStartMgr.GerInstance().nodes[i, j];
                if(node.type==E_Node_Type.Stop)
                {
                    obj.GetComponent<MeshRenderer>().material =red;
                }
            }
        }



    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit info;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //射线检测
            if (Physics.Raycast(ray, out info, 1000))
            {
                //点击立方体得到开始

                if (beginPos == Vector2.right * -1)
                {
                    //清理上一次的
                    if (list != null)
                    {
                        for (int i = 0; i < list.Count; ++i)
                        {
                            cubes[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material = normal;

                        }
                    }

                    string[] strs = info.collider.gameObject.name.Split('_');
                    //得到行列位置
                    beginPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));
                    info.collider.gameObject.GetComponent<MeshRenderer>().material = yellow;
                  

                }
                else //点出终点
                {
                    string[] strs = info.collider.gameObject.name.Split('_');
                    Vector2 endPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));
                    //寻路
                   list= AStartMgr.GerInstance().FindPath(beginPos, endPos);
                    if (list != null)
                    {
                        for (int i = 0; i < list.Count; ++i)
                        {
                            cubes[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material = green;

                        }
                    }
                    cubes[(int)beginPos.x + "_" + (int)beginPos.y].GetComponent<MeshRenderer>().material = normal;
                    beginPos = Vector2.right * -1;
                }
            }

        }
        
    }
}
