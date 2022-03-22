using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 棋子或者格子的脚本
/// </summary>
public class chessOrgrid : MonoBehaviour
{
    public int xIndex, yIndex;//格子索引
    public bool isWrite;//是白棋还是黑棋
    public bool isGrid;//是否是格子
    private GameManager gameManager;//游戏管理的引用
    private GameObject gridGo;//移动的时候需要设置的父对象
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        gridGo = gameObject;//gridGo是格子的时候，是他自身
    }

    /// <summary>
    /// 点击棋子格子时触发的检测方法
    /// </summary>
    public void ClickCheck()
    {
        if (gameManager.gameOver)
        {
            return;
        }
        int itemColorId;//3个用枚举或者int
        if (isGrid)
        {
            itemColorId = 0;//格子
        }
        else
        {
            gridGo = transform.parent.gameObject;//gridGo是棋子的情况下得到父对象
            //获得格子的索引
            chessOrgrid chessOrgrid = gridGo.GetComponent<chessOrgrid>();
            xIndex = chessOrgrid.xIndex;
            yIndex = chessOrgrid.yIndex;
            if(isWrite)
            {
                itemColorId = 2;//白色
            }
            else
            {
                itemColorId = 1;//黑色
            }
        }
        GridOrChessBehavior(itemColorId, xIndex, yIndex);
    }

    /// <summary>
    /// 格子与棋子的行为逻辑
    /// </summary>
    /// <param name="itemColorID">格子颜色0是格子，1是黑棋2是白棋</param>
    /// <param name="x">当前格子的x索引</param>
    /// <param name="y">当前格子的y索引</param>
    
    private void GridOrChessBehavior(int itemColorID,int x,int y)
    {
        int FromX, FromY, ToX, ToY;
        switch (itemColorID)
        {
            //空格子
            case 0:
                //当前要去的位置
                ToX = x;
                ToY = y;
                //第一次点到空格子
                if (gameManager.LastChessOrGrid==null)
                {
                    gameManager.LastChessOrGrid = this;
                    return;
                }
                if (gameManager.chessMove)//白色轮次
                {
                    if (gameManager.LastChessOrGrid.isGrid)//上次点击是否是格子
                    {
                        return;
                    }
                    if (!gameManager.LastChessOrGrid.isWrite)//上次点击是否是黑色
                    {
                        gameManager.LastChessOrGrid = null;
                        return;
                    }
                    //上次的位置
                    FromX = gameManager.LastChessOrGrid.xIndex;
                    FromY = gameManager.LastChessOrGrid.yIndex;
                    //当前的移动是否合法
                    bool canMove = gameManager.rules.IsValidMove(gameManager.chesssBoard,FromX,FromY,ToX,ToY);
                    if(!canMove)
                    {
                        return;
                    }
                    //棋子进行移动 
                    gameManager.movingOfChess.IsMove(gameManager.LastChessOrGrid.gameObject,gridGo,FromX,FromY,ToX,ToY);
                    UIManager.Instance.ShowTip("黑方走");//UI改变
                    gameManager.checkMate.JudgeIfCheckmate();//检测是否将军
                    gameManager.chessMove = false;//黑棋走的轮次
                    gameManager.LastChessOrGrid = this;//上一个选中的位置设为当前位置
                }
                else//黑色轮次
                {
                    if (gameManager.LastChessOrGrid.isGrid)//上次点击是否是格子
                    {
                        return;
                    }
                    if (gameManager.LastChessOrGrid.isWrite)//上次点击是否是白色
                    {
                        gameManager.LastChessOrGrid = null;
                        return;
                    }
                    //上次的位置
                    FromX = gameManager.LastChessOrGrid.xIndex;
                    FromY = gameManager.LastChessOrGrid.yIndex;
                    //当前的移动是否合法
                    bool canMove = gameManager.rules.IsValidMove(gameManager.chesssBoard, FromX, FromY, ToX, ToY);
                    if (!canMove)
                    {
                        return;
                    }
                    gameManager.movingOfChess.IsMove(gameManager.LastChessOrGrid.gameObject, gridGo, FromX, FromY, ToX, ToY);
                    UIManager.Instance.ShowTip("白方走");//UI改变
                    gameManager.checkMate.JudgeIfCheckmate();//检测是否将军
                    gameManager.chessMove = true;//白棋走的轮次
                    gameManager.LastChessOrGrid = this;//上一个选中的位置设为当前位置
                }

                break;


           //黑色棋子
            case 1:
                //黑色轮次
                if(!gameManager.chessMove)
                {
                    FromX = x;
                    FromY = y;
                    //显示所有可以移动的路径
                    gameManager.LastChessOrGrid = this;
                }
                //白色轮次
                else
                {
                    //白色棋子要吃黑色棋子
                    if (gameManager.LastChessOrGrid == null)
                    {
                        return;
                    }
                    if (!gameManager.LastChessOrGrid.isWrite)
                    {
                        gameManager.LastChessOrGrid = this;
                        return;
                    }
                    FromX = gameManager.LastChessOrGrid.xIndex;
                    FromY = gameManager.LastChessOrGrid.yIndex;
                    ToX = x;
                    ToY = y;
                    bool canMove = gameManager.rules.IsValidMove(gameManager.chesssBoard,FromX,FromY,ToX,ToY);
                    if (!canMove)
                    {
                        return;
                    }
                    gameManager.movingOfChess.IsEat(gameManager.LastChessOrGrid.gameObject, gameObject, FromX, FromY, ToX, ToY);
                    gameManager.chessMove = false;//吃子完成后为黑色轮次
                    UIManager.Instance.ShowTip("黑色走");
                    gameManager.LastChessOrGrid = null;//吃子后 上一布制空
                    gameManager.checkMate.JudgeIfCheckmate();
                    }
                break;


            //白色棋子
            case 2:
                if (gameManager.chessMove)//当前是白色轮次
                {
                    FromX = x;
                    FromY = y;
                    //显示所有可行路径
                    gameManager.LastChessOrGrid = this;
                }
                else//黑色轮次
                {
                    //黑色棋子要吃白色棋子
                    if (gameManager.LastChessOrGrid == null)
                    {
                        return;
                    }
                    if (gameManager.LastChessOrGrid.isWrite)
                    {
                        gameManager.LastChessOrGrid = this;//当前为黑色轮次，白色无法行动
                        return;
                    }
                    FromX = gameManager.LastChessOrGrid.xIndex ;
                    FromY = gameManager.LastChessOrGrid.yIndex;
                    ToX = x;
                    ToY = y;
                    bool canMove = gameManager.rules.IsValidMove(gameManager.chesssBoard, FromX, FromY, ToX, ToY);
                    if(!canMove)
                    {
                        return;
                    }
                    gameManager.movingOfChess.IsEat(gameManager.LastChessOrGrid.gameObject, gameObject, FromX, FromY, ToX, ToY);
                    gameManager.chessMove = true;
                    UIManager.Instance.ShowTip("白棋走");
                    gameManager.LastChessOrGrid = null;
                    gameManager.checkMate.JudgeIfCheckmate();
                }
                break;
            default:
                break;
        }
    }
}
