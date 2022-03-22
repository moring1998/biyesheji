using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


///<summary>
///存储游戏数据、游戏物体的引用、游戏资源、模式的选择与切换
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set; }

    public int ChessPeople;//对战人数，单机模式1 联网模式2；
    /// <summary>
    /// 数据
    /// </summary>
    public int[,] chesssBoard;//当前棋盘的状况
    public GameObject[,] boardGrid;//棋盘上所有格子
    private const int gridWidth = 80;//格子宽
    private const int gridHeight = 80;//格子高
    private const int gridTotalNum = 64;//棋盘上格子数

    /// <summary>
    /// 开关
    /// </summary>
    public bool chessMove;//该哪方移动，白棋是true黑是false
    public bool gameOver;//游戏结束不能移动


    /// <summary>
    /// 资源
    /// </summary>
    public GameObject gridGo;//格子
    public Sprite[] sprites;//所有棋子的sprite
    public GameObject chessGo;//棋子

    /// <summary>
    /// 引用
    /// </summary>
    [HideInInspector]
    public GameObject boardGo;//棋盘
    public GameObject[] boardGos;//0单机 1联网
    public chessOrgrid LastChessOrGrid;//上次点击到的对象（棋子或者格子）
    public Rules rules; //规则类
    public MovingOfChess movingOfChess;//移动类
    public CheckMate checkMate;
    public GameObject eatChessPool;//被吃掉的棋子存放池


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //仅做测试
        ChessPeople=1;
        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 重置游戏
    /// </summary>
    public void ResetGame()
    {
        //初始化棋盘
        //棋子编号为1黑王  2黑车 3黑马 4黑后 5黑象 6黑兵 7白王 8白车 9白马 10白后 11白象 12白兵 
        chessMove = true;
        chesssBoard = new int[8, 8]
        {
            {2, 3, 5, 4, 1,5,3,2 },
            {6,6,6,6,6,6,6,6},
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {12,12,12,12,12,12,12,12},
            {8,9,11,10,7,11,9,8 }
        };
        //初始化格子
        boardGrid = new GameObject[8, 8];

        if(ChessPeople == 1)
        {
            boardGo = boardGos[0];
        }
        else
        {
            boardGo = boardGos[1];
        }
        InitGrid();
        InitChess();
        rules = new Rules();//实例化出rules类
        movingOfChess = new MovingOfChess(this);//移动类
        checkMate = new CheckMate();//将军类

    }
    ///<summary>
    ///实例化格子
    /// </summary>
    private void InitGrid()
    {
        float posX = 0, posY = 0;
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                GameObject itemGo = Instantiate(gridGo);//获取到游戏对象
                itemGo.transform.SetParent(boardGo.transform);//格子的坐标根据棋盘下来设置
                itemGo.name = "Item[" + i.ToString() + "," + j.ToString()+ "]";//格子名称（几行几列）
                itemGo.transform.localPosition = new Vector3(posX, posY, 0);//new一个局部坐标
                posX += gridWidth;//每次实例化了累加
                if (posX >= gridWidth*8)//换行
                {
                    posY -= gridHeight;
                    posX = 0;
                }
                itemGo.GetComponent<chessOrgrid>().xIndex = i;//记录当前的x索引
                itemGo.GetComponent<chessOrgrid>().yIndex = j;//记录当前的y索引
                //Debug.Log(i + ":" + j);
                boardGrid[i, j] = itemGo;//当前格子存储到数组中；
                
            }
        }
    } 
    /// <summary>
    /// 实例化棋子
    /// </summary>
    private void InitChess()
    {
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                GameObject item = boardGrid[i, j];//获取游戏物体
                switch (chesssBoard[i,j])//根据相应的参数调用对应的事件
                {
                    case 1:
                        CreateChess(item, "b_wang", sprites[0], false);
                        break;
                    case 2:
                        CreateChess(item, "b_che", sprites[1], false);
                        break;
                    case 3:
                        CreateChess(item, "b_ma", sprites[2], false);
                        break;
                    case 4:
                        CreateChess(item, "b_hou", sprites[3], false);
                        break;
                    case 5:
                        CreateChess(item, "b_xiang", sprites[4], false);
                        break;
                    case 6:
                        CreateChess(item, "b_bing", sprites[5], false);
                        break;
                    case 7:
                        CreateChess(item, "w_wang", sprites[6], true);
                        break;
                    case 8:
                        CreateChess(item, "w_che", sprites[7], true);
                        break;
                    case 9:
                        CreateChess(item, "w_ma", sprites[8], true);
                        break;
                    case 10:
                        CreateChess(item, "w_hou", sprites[9], true);
                        break;
                    case 11:
                        CreateChess(item, "w_xaing", sprites[10], true);
                           break;
                    case 12:
                        CreateChess(item, "w_bing", sprites[11], true);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    /// <summary>
    /// 生成棋子游戏物体
    /// </summary>
    /// <param name="gridItem">作为父对象的格子</param>
    /// <param name="name">棋子名称</param>
    /// <param name="chessIcon">棋子标志样式</param>
    /// <param name="ifwhite">是否为白棋</param>
    private void CreateChess(GameObject gridItem,string name,Sprite chessIcon,bool ifwhite)
    {
        GameObject item =Instantiate(chessGo);//获取到游戏物体
        item.transform.SetParent(gridItem.transform);//设为格子的子对象
        item.name = name;
        item.GetComponent<Image>().sprite = chessIcon;//设置image
        item.transform.localPosition = Vector3.zero;//位置归0 跟父对象保持一致
        item.transform.localScale = Vector3.one;
        item.GetComponent<chessOrgrid>().isWrite = ifwhite;
    }

    /// <summary>
    /// 被吃掉棋子的处理方法
    /// </summary>
    /// <param name="itemGo">被吃掉棋子的游戏物体</param>
    public void BeEat(GameObject itemGo)
    {
        itemGo.transform.SetParent(eatChessPool.transform);
        itemGo.transform.localPosition = Vector3.zero;
    }
}
