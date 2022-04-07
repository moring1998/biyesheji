using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 悔棋类的实现
/// </summary>
public class ChessReseting 
{
    private GameManager gameManager;//需要获取到一些成员变量和方法 所以封装成一个成员变量
    public int resetCount;//计数器，用来计数当前一共走了几步棋
    public Chess[] chessSteps;//悔棋数组，用来存放所有已经走过的步数，用来悔棋
    
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
        public GameObject chessTwo;
        public int chessOneID;//拿到棋子的ID
        public int chessTwoID;
    }
    /// <summary>
    /// 获取棋子位置
    /// </summary>
    public struct ChessSteps
    {
        public int x;
        public int y;
    }

    /// <summary>
    /// 悔棋方法
    /// </summary>
    public void ResetChess()
    {
        gameManager.HideLastPositionUI();
        gameManager.HideClickUI();
        
       if (gameManager.ChessPeople==1)
        {
            if (resetCount<=0)//没有走过棋，不能悔棋
            {
                return;
            }
            int f = resetCount - 1;//得到这一步棋
            int oneID = chessSteps[f].chessOneID;//棋子原来位置的ID
            int twoID = chessSteps[f].chessTwoID;//棋子移动到的位置ID
            GameObject gridOne, gridTwo, chessOne, chessTwo;
            gridOne = chessSteps[f].gridOne;
            gridTwo = chessSteps[f].gridTwo;
            chessOne = chessSteps[f].chessOne;
            chessTwo = chessSteps[f].chessTwo;
            //吃子的情况
            if (chessTwo!=null)
            {   
                //将第一个棋子的父对象设为第一个格子,第二个的棋子的父对象设为第二个
                chessOne.transform.SetParent(gridOne.transform);
                chessTwo.transform.SetParent(gridTwo.transform);
                chessOne.transform.localPosition = Vector3.zero;
                chessTwo.transform.localPosition = Vector3.zero;
                //将ID更新
                gameManager.chesssBoard[chessSteps[f].from.x, chessSteps[f].from.y] = oneID;
                gameManager.chesssBoard[chessSteps[f].to.x, chessSteps[f].to.y] = twoID;
            }
            //移动的情况
            else
            {
                
                chessOne.transform.SetParent(gridOne.transform);
                chessOne.transform.localPosition = Vector3.zero;
                //将ID更新
                gameManager.chesssBoard[chessSteps[f].from.x, chessSteps[f].from.y] = oneID;
                gameManager.chesssBoard[chessSteps[f].to.x, chessSteps[f].to.y] = 0;
            }

            //该黑方走了，但是白方悔棋
            if (gameManager.chessMove==false)
            {
                UIManager.Instance.ShowTip("白方走");
                gameManager.chessMove = true;
            }
            //该白方走了，但是黑方要悔棋
            else
            {
                UIManager.Instance.ShowTip("黑方走");
                gameManager.chessMove = false;
            }
            resetCount--;
            chessSteps[f] = new Chess();
        }  

    }

    /// <summary>
    /// 添加悔棋步骤（用来之后悔棋）
    /// </summary>
    /// <param name="resetStepNum">具体的悔棋步数索引</param>
    /// <param name="fromX"></param>
    /// <param name="fromY"></param>
    /// <param name="toX"></param>
    /// <param name="toY"></param>
    /// <param name="ID1">对应悔棋那一步的第一个棋子的ID</param>
    /// <param name="ID2">对应悔棋那一步的第二个棋子（格子）的ID</param>
    public void AddChess(int resetStepNum,int fromX,int fromY,int toX,int toY,int ID1,int ID2)
    {
        //当前需要记录的这步棋中的数据存入我们的chess结构体里，然后存入结构体数组
        GameObject item1 = gameManager.boardGrid[fromX, fromY];
        GameObject item2 = gameManager.boardGrid[toX, toY];
        chessSteps[resetStepNum].from.x = fromX;
        chessSteps[resetStepNum].from.y = fromY;
        chessSteps[resetStepNum].to.x = toX;
        chessSteps[resetStepNum].to.y = toY;
        chessSteps[resetStepNum].gridOne = item1;
        chessSteps[resetStepNum].gridTwo = item2;
        gameManager.HideClickUI();
        GameObject firstChess = item1.transform.GetChild(0).gameObject;
        chessSteps[resetStepNum].chessOne = firstChess;
        chessSteps[resetStepNum].chessOneID = ID1;
        chessSteps[resetStepNum].chessTwoID = ID2;
        //如果是吃子
        if (item2.transform.childCount!=0)
        {
            GameObject secondChess = item2.transform.GetChild(0).gameObject;
            chessSteps[resetStepNum].chessTwo = secondChess;
        }
        resetCount++;
        
        //Debug.Log("第" + resetCount + "步添加");
        //Debug.Log("Item1:" + item1.name);
        //Debug.Log("Item2:" + item2.name);
        //Debug.Log("firstChess:" + firstChess.name);
        //if (chessSteps[resetStepNum].chessTwo != null)
        //{
        //    Debug.Log("secondChess:" + chessSteps[resetStepNum].chessTwo.name);
        //}

    }
}
