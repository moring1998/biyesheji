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
    public GameObject canMovePosUIGo;//可以移动到的位置UI显示

    /// <summary>
    /// 引用
    /// </summary>
    [HideInInspector]
    public GameObject boardGo;//棋盘
    public GameObject[] boardGos;//0单机 1联网
    [HideInInspector]
    public chessOrgrid LastChessOrGrid;//上次点击到的对象（棋子或者格子）
    public Rules rules; //规则类
    public MovingOfChess movingOfChess;//移动类
    public CheckMate checkMate;//将军类
    public GameObject eatChessPool;//被吃掉的棋子存放池
    public GameObject clickChessUIGo;//选中棋子的UI显示
    public GameObject lastPosUIGo;//棋子移动前位置UI的显示
    public GameObject canEatPosUIGo;//可以吃掉棋子的UI显示
    public Stack<GameObject> canMoveUIStack;//存储所有移动位置UI显示游戏物体的栈（总的
    public Stack<GameObject> currentCanMoveUIStack;//存储当前移动位置UI 已经显示 出来的游戏物体的栈（显示的
    public Stack<GameObject> canEatUIStack;//存储所有移动位置UI显示游戏物体的栈（总的
    public Stack<GameObject> currentCanEatUIStack;//存储当前移动位置UI 已经显示 出来的游戏物体的栈（显示的
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
        rules = new Rules();//实例化出rules类对象
        movingOfChess = new MovingOfChess(this);//移动类对象
        checkMate = new CheckMate();//将军检测对象
        //移动UI显示的栈
        canMoveUIStack = new Stack<GameObject>();
        //吃子UI显示的栈
        canEatUIStack = new Stack<GameObject>();
       //实例化四十个移动位置ui显示
        for(int i = 0; i < 40; i++)
        {
            canMoveUIStack.Push(Instantiate(canMovePosUIGo));
        }
        //实例化十个可吃子位置显示
        for (int j = 0; j < 10; j++)
        {
            canEatUIStack.Push(Instantiate(canEatPosUIGo));
        }
        currentCanEatUIStack = new Stack<GameObject>();//初始化吃子显示的栈
        currentCanMoveUIStack = new Stack<GameObject>();//初始化移动显示的栈

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
        item.GetComponent<Image>().sprite = chessIcon;//获取图案
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
    #region 关于游戏进行中UI的显示隐藏方法
     /// <summary>
     /// 显示或隐藏 点击选中棋子 的UI
     /// </summary>
     /// <param name="targetTrans">父对象的位置</param>     
    public void ShowClickUI(Transform targetTrans)//在选中棋子时显示
    {
        clickChessUIGo.transform.SetParent(targetTrans);
        clickChessUIGo.transform.localPosition = Vector3.zero;

    }
    public void HideClickUI()//在走棋完成或者吃子完成后隐藏
    {
        clickChessUIGo.transform.SetParent(eatChessPool.transform);
        clickChessUIGo.transform.localPosition = Vector3.zero;
    }
    /// <summary>
    /// 显示或隐藏 棋子移动前的位置 UI
    /// </summary>
    /// <param name="ShowPostion"></param>
    public void ShowLastPositionUI(Vector3 ShowPostion)//吃子或者移动之前显示上一次棋子的位置
    {
        lastPosUIGo.transform.position = ShowPostion;
    }
    public void HideLastPositionUI()
    {
        lastPosUIGo.transform.position = new Vector3(100, 100, 100);//放到看不见的位置相当于隐藏
    }

    /// <summary>
    /// 隐藏 可以吃掉该棋子 的UI显示
    /// </summary>
    //public void HideCanEatUI()
    //{
    //    canEatPosUIGo.transform.SetParent(eatChessPool.transform);
    //    canEatPosUIGo.transform.localPosition = Vector3.zero;
    //}

    public GameObject PopCanEatUI()//显示当前吃子位置UI
    {
        GameObject itemGo = canEatUIStack.Pop();//出栈
        currentCanEatUIStack.Push(itemGo);//将要显示的UI存放在当前栈中
        itemGo.SetActive(true);//激活当前游戏物体
        return itemGo;
    }
    public void PushCanEatUI(GameObject itemGo)//隐藏当前吃子位置UI
    {
        canEatUIStack.Push(itemGo);//入栈
        itemGo.transform.SetParent(eatChessPool.transform);//设置父对象
        itemGo.SetActive(false);
    }
    public void ClearCurrentCanEatUIStack()//清空当前显示ui
    {
        while (currentCanEatUIStack.Count > 0)//当前栈内count大于0则需要清空
        {
            PushCanEatUI(currentCanEatUIStack.Pop());//将currentcaneatuistack中的弹出的物体 存入canmoveuistack
        }
    }
    /// <summary>
    /// 显示或者隐藏 当前选中棋子可以移动到的位置UI
    /// </summary>
    /// <returns></returns>
    public GameObject PopCanMoveUI()//显示当前移动位置UI
    {
        GameObject itemGo = canMoveUIStack.Pop();//出栈
        currentCanMoveUIStack.Push(itemGo);//将要显示的UI存放在当前栈中
        itemGo.SetActive(true);//激活当前游戏物体
        return itemGo;
    }
    public void PushCanMoveUI(GameObject itemGo)//隐藏当前移动位置UI
    {
        canMoveUIStack.Push(itemGo);//入栈
        itemGo.transform.SetParent(eatChessPool.transform);//设置父对象
        itemGo.SetActive(false);
    }
    public void ClearCurrentCanMoveUIStack()//清空当前显示ui
    {
        while (currentCanMoveUIStack.Count>0)//当前栈内count大于0则需要清空
        {
            PushCanMoveUI(currentCanMoveUIStack.Pop());//将currentcanmoveuistack中的弹出的物体 存入canmoveuistack
        }
    }
    #endregion
}
