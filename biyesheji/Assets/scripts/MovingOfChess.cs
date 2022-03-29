using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 棋子的移动类
/// </summary>

public class MovingOfChess //不需要挂载在游戏物体上，不必继承MonoBehaviour类
{

    private GameManager gameManager;
    public  MovingOfChess(GameManager mGameManager)
    {
        gameManager = mGameManager;
    }

    /// <summary>
    /// 棋子的移动方法
    /// </summary>
    /// <param name="chessGo">当前要移动的棋子游戏物体</param>
    /// <param name="targetGrid">要移动到的格子游戏物体</param>
    /// <param name="x1">来位置的x索引</param>
    /// <param name="y1">Y索引</param>
    /// <param name="x2">去的X索引</param>
    /// <param name="y2">Y索引</param>
   public void IsMove(GameObject chessGo,GameObject targetGrid,int x1,int y1,int x2,int y2)
    {
        gameManager.ShowLastPositionUI(chessGo.transform.position);
        chessGo.transform.SetParent(targetGrid.transform);//将格子设为棋子的父对象
        chessGo.transform.localPosition = Vector3.zero;//棋子自身位置重置
        gameManager.chesssBoard[x2, y2] =gameManager.chesssBoard[x1,y1];//来的时候的id赋给去的时候的id
        gameManager.chesssBoard[x1, y1] = 0;//来的时候的位置是空
        
    }

    /// <summary>
    /// 棋子吃子的方法
    /// </summary>
    /// <param name="firstChess">想要移动的棋子</param>
    /// <param name="secondChess">想要吃掉的棋子</param>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="x2"></param>
    /// <param name="y2"></param>
    public void IsEat(GameObject firstChess,GameObject secondChess, int x1, int y1, int x2, int y2)
    {
        gameManager.ShowLastPositionUI(firstChess.transform.position);//显示棋子移动前的位置
        GameObject secendChessGrid = secondChess.transform.parent.gameObject;//得到第二个棋子的父对象
        firstChess.transform.SetParent(secendChessGrid.transform);
        firstChess.transform.localPosition = Vector3.zero;
        gameManager.chesssBoard[x2, y2] = gameManager.chesssBoard[x1, y1];
        gameManager.chesssBoard[x1, y1] = 0;
        gameManager.BeEat(secondChess);
    }
}
