using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 悔棋类的实现
/// </summary>
public class ChessReseting 
{
    private GameManager gameManager;//需要获取到一些成员变量和方法 所以封装成一个成员变量
    //计数器，用来计数当前一共走了几步棋
    public int resetCount;
    //悔棋数组，用来存放所有已经走过的步数，用来悔棋
    public Chess[] chessSteps;
    
    public ChessReseting()
    {
        gameManager = GameManager.instance;//一旦调用就实例化出来
    }
    /// <summary>
    /// 记录每一步悔棋的具体棋子的结构体
    /// </summary>
    public struct Chess
    {
        public ChessSteps from;
        public ChessSteps to;
        public GameObject gridOne;//来的位置所在格子
        public GameObject gridTwo;//去的位置所在格子
        public GameObject chessOne;//拿到两个棋子
        public GameObject chessTWO;
        public int chessOneID;//拿到棋子的ID
        public int chessTwoID;
    }
    /// <summary>
    /// 棋子位置
    /// </summary>
    public struct ChessSteps
    {
        public int x;
        public int t;
    }
    /// <summary>
    /// 悔棋方法
    /// </summary>
    public void ResetChess()
    {

    }

    /// <summary>
    /// 添加悔棋步骤
    /// </summary>
    /// <param name="resetStepNum">具体的悔棋步数索引</param>
    /// <param name="fromX"></param>
    /// <param name="fromY"></param>
    /// <param name="toX"></param>
    /// <param name="toY"></param>
    /// <param name="ID1">对应悔棋哪一步的第一个棋子的ID</param>
    /// <param name="ID2">对应悔棋哪一步的第二个棋子（格子）的ID</param>
    public void AddChess(int resetStepNum,int fromX,int fromY,int toX,int toY,int ID1,int ID2)
    {

    }
}
