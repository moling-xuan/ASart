using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAStart : MonoBehaviour
{
    //��һ��������λ��
    public int beginX=-3;
    public int beginY=5;
    //���
    public int offsetX;
    public int offsetY;
    //��ͼ���
    public int mapW;
    public int mapH;
    //������ 
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

                //����
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
            //���߼��
            if (Physics.Raycast(ray, out info, 1000))
            {
                //���������õ���ʼ

                if (beginPos == Vector2.right * -1)
                {
                    //������һ�ε�
                    if (list != null)
                    {
                        for (int i = 0; i < list.Count; ++i)
                        {
                            cubes[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material = normal;

                        }
                    }

                    string[] strs = info.collider.gameObject.name.Split('_');
                    //�õ�����λ��
                    beginPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));
                    info.collider.gameObject.GetComponent<MeshRenderer>().material = yellow;
                  

                }
                else //����յ�
                {
                    string[] strs = info.collider.gameObject.name.Split('_');
                    Vector2 endPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));
                    //Ѱ·
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
