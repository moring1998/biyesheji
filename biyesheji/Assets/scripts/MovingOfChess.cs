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
        AudioManager.Instance.PlaySound(1);
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
        AudioManager.Instance.PlaySound(2);
        gameManager.ShowLastPositionUI(firstChess.transform.position);//显示棋子移动前的位置
        GameObject secendChessGrid = secondChess.transform.parent.gameObject;//得到第二个棋子的父对象
        firstChess.transform.SetParent(secendChessGrid.transform);
        firstChess.transform.localPosition = Vector3.zero;
        gameManager.chesssBoard[x2, y2] = gameManager.chesssBoard[x1, y1];
        gameManager.chesssBoard[x1, y1] = 0;
        gameManager.BeEat(secondChess);
    }


    /// <summary>
    /// 判断当前点击到的是什么类型的棋子从而显示路径
    /// </summary>
    /// <param name="fromX"></param>
    /// <param name="fromY"></param>
    public void ClickChess(int fromX, int fromY)
    {
        int chessID = gameManager.chesssBoard[fromX, fromY];
        switch (chessID)
        {
            //王
            case 1:
            case 7:
                 GetKingMove(gameManager.chesssBoard, fromX, fromY);
                break;
            //车
            case 2:
            case 8:
                GetCheMove(gameManager.chesssBoard, fromX, fromY);
                break;
            //马
            case 3:
            case 9:
                GetMaMove(gameManager.chesssBoard, fromX, fromY);
                break;
            //王后
            case 4:
            case 10:
                GetQueenMove(gameManager.chesssBoard, fromX, fromY);
                break;
            //象
            case 5:
            case 11:
                GetXiangMove(gameManager.chesssBoard, fromX, fromY);
                break;
            //黑兵
            case 6:
                GetB_bingMove(gameManager.chesssBoard, fromX, fromY);
                break;
            //白兵
            case 12:
                GetW_bingMove(gameManager.chesssBoard, fromX, fromY);
                break;

            default:
                break;
        }
    }

    #region 对应棋子的可移动的路径方法
    /// <summary>
    /// 王的可移动路径
    /// </summary>
    /// <param name="position"></param>
    /// <param name="fromX"></param>
    /// <param name="fromY"></param>
    private void GetKingMove(int[,] position,int fromX,int fromY)
    {
        int x, y;
        //前
        x = fromX+1;
        y = fromY;
        if (x<8 && gameManager.rules.IsValidMove(position, fromX, fromY, x, y))
        {
            GetCanMovePos(position, fromX, fromY, x, y);
        }
        //后
        x = fromX - 1;
        y = fromY;
        if (x >=0 && gameManager.rules.IsValidMove(position, fromX, fromY, x, y))
        {
            GetCanMovePos(position, fromX, fromY, x, y);
        }
        //左
        x = fromX ;
        y = fromY- 1;
        if (y >= 0 && gameManager.rules.IsValidMove(position, fromX, fromY, x, y))
        {
            GetCanMovePos(position, fromX, fromY, x, y);
        }
        //右
        x = fromX ;
        y = fromY+ 1;
        if (y <8 && gameManager.rules.IsValidMove(position, fromX, fromY, x, y))
        {
            GetCanMovePos(position, fromX, fromY, x, y);
        }
        //右上
        x = fromX - 1;
        y = fromY+1;
        if ((x >= 0 && y < 8)&& gameManager.rules.IsValidMove(position, fromX, fromY, x, y))
        {
            GetCanMovePos(position, fromX, fromY, x, y);
        }
        //右下
        x = fromX + 1;
        y = fromY + 1;
        if ((x <8 && y < 8) && gameManager.rules.IsValidMove(position, fromX, fromY, x, y))
        {
            GetCanMovePos(position, fromX, fromY, x, y);
        }
        //左上
        x = fromX - 1;
        y = fromY -1;
        if ((x >= 0 && y >= 0) && gameManager.rules.IsValidMove(position, fromX, fromY, x, y))
        {
            GetCanMovePos(position, fromX, fromY, x, y);
        }
        //左下
        x = fromX+ 1;
        y = fromY - 1;
        if ((x <8 && y >= 0) && gameManager.rules.IsValidMove(position, fromX, fromY, x, y))
        {
            GetCanMovePos(position, fromX, fromY, x, y);
        }
    }


    /// <summary>
    /// 车的可移动路径
    /// </summary>
    /// <param name="position"></param>
    /// <param name="fromX"></param>
    /// <param name="fromY"></param>
    private void GetCheMove(int[,] position, int fromX, int fromY)
    {
        int x, y;
        int chessID;
        //得到当前选中棋子的ID，目的是为了遍历时判断第一个不为空格子的棋子跟我们是否是同一边
        chessID = position[fromX, fromY];
        //上
        x = fromX - 1;
        y = fromY;
        while (x >= 0)
        {
            if (position[x, y] == 0)//当前遍历到的位置ID是否为0（即空格子）
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
            else//不为空格子
            {
                if (!gameManager.rules.IsSameSide(chessID, position[x, y]))//不是一边的棋子则可以走，且遍历结束
                {
                    GetCanMovePos(position, fromX, fromY, x, y);
                }
                break;
            }
            x--;
        }

        //下
        x = fromX + 1;
        y = fromY;
        while (x < 8)
        {
            if (position[x, y] == 0)//当前遍历到的位置ID是否为0（即空格子）
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
            else//不为空格子
            {
                if (!gameManager.rules.IsSameSide(chessID, position[x, y]))//不是一边的棋子则可以走，且遍历结束
                {
                    GetCanMovePos(position, fromX, fromY, x, y);
                }
                break;
            }
            x++;
        }

        //左
        x = fromX;
        y = fromY - 1;
        while (y >= 0)
        {
            if (position[x, y] == 0)//当前遍历到的位置ID是否为0（即空格子）
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
            else//不为空格子
            {
                if (!gameManager.rules.IsSameSide(chessID, position[x, y]))//不是一边的棋子则可以走，且遍历结束
                {
                    GetCanMovePos(position, fromX, fromY, x, y);
                }
                break;
            }
            y--;
        }
        //右
        x = fromX;
        y = fromY + 1;
        while (y < 8)
        {
            if (position[x, y] == 0)//当前遍历到的位置ID是否为0（即空格子）
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
            else//不为空格子
            {
                if (!gameManager.rules.IsSameSide(chessID, position[x, y]))//不是一边的棋子则可以走，且遍历结束
                {
                    GetCanMovePos(position, fromX, fromY, x, y);
                }
                break;
            }
            y++;
        }
    }

    /// <summary>
    /// 马的可移动路径
    /// </summary>
    /// <param name="position"></param>
    /// <param name="fromX"></param>
    /// <param name="fromY"></param>
    private void GetMaMove(int[,] position, int fromX, int fromY)
    {
        int x, y;
        //竖日
            //右下
            x = fromX + 2;
            y = fromY + 1;
            if ((x < 8 && y < 8) && gameManager.rules.IsValidMove(position, fromX, fromY, x, y))//如果在棋盘内  且不符合移动规则
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
            //右上
            x = fromX - 2;
            y = fromY + 1;
            if ((x >= 0 && y < 8) && gameManager.rules.IsValidMove(position, fromX, fromY, x, y))
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
            //左下
            x = fromX + 2;
            y = fromY - 1;
            if ((x < 8 && y >= 0) && gameManager.rules.IsValidMove(position, fromX, fromY, x, y))
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
            //左上
            x = fromX - 2;
            y = fromY - 1;
            if ((x >= 0 && y >= 0) && gameManager.rules.IsValidMove(position, fromX, fromY, x, y))
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
        //横日
            //右下
            x = fromX + 1;
            y = fromY + 2;
            if ((x < 8 && y < 8) && gameManager.rules.IsValidMove(position, fromX, fromY, x, y))
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
            //右上
            x = fromX - 1;
            y = fromY + 2;
            if ((x >= 0 && y < 8) && gameManager.rules.IsValidMove(position, fromX, fromY, x, y))
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
            //左下
            x = fromX + 1;
            y = fromY - 2;
            if ((x < 8 && y >= 0) && gameManager.rules.IsValidMove(position, fromX, fromY, x, y))
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
            //左上
            x = fromX - 1;
            y = fromY - 2;
            if ((x >= 0 && y >= 0) && gameManager.rules.IsValidMove(position, fromX, fromY, x, y))
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }

    }
    private void GetQueenMove(int[,] position, int fromX, int fromY)
    {  
        int x, y;
        int chessID;
        //得到当前选中棋子的ID，目的是为了遍历时判断第一个不为空格子的棋子跟我们是否是同一边
        chessID = position[fromX, fromY];
        //上
        x = fromX - 1;
        y = fromY;
        while (x >= 0)
        {
            if (position[x, y] == 0)//当前遍历到的位置ID是否为0（即空格子）
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
            else//不为空格子
            {
                if (!gameManager.rules.IsSameSide(chessID, position[x, y]))//不是一边的棋子则可以走，且遍历结束
                {
                    GetCanMovePos(position, fromX, fromY, x, y);
                }
                break;
            }
            x--;
        }

        //下
        x = fromX + 1;
        y = fromY;
        while (x <8)
        {
            if (position[x, y] == 0)//当前遍历到的位置ID是否为0（即空格子）
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
            else//不为空格子
            {
                if (!gameManager.rules.IsSameSide(chessID, position[x, y]))//不是一边的棋子则可以走，且遍历结束
                {
                    GetCanMovePos(position, fromX, fromY, x, y);
                }
                break;
            }
            x++;
        }

        //左
        x = fromX;
        y = fromY - 1;
        while (y>=0)
        {
            if (position[x, y] == 0)//当前遍历到的位置ID是否为0（即空格子）
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
            else//不为空格子
            {
                if (!gameManager.rules.IsSameSide(chessID, position[x, y]))//不是一边的棋子则可以走，且遍历结束
                {
                    GetCanMovePos(position, fromX, fromY, x, y);
                }
                break;
            }
            y--;
        }
        //右
        x = fromX;
        y = fromY + 1;
        while (y < 8)
        {
            if (position[x, y] == 0)//当前遍历到的位置ID是否为0（即空格子）
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
            else//不为空格子
            {
                if (!gameManager.rules.IsSameSide(chessID, position[x, y]))//不是一边的棋子则可以走，且遍历结束
                {
                    GetCanMovePos(position, fromX, fromY, x, y);
                }
                break;
            }
            y++;
        }
        //右上
        x = fromX - 1;
        y = fromY + 1;
        while (x>=0 && y<8)
        {
            if (position[x, y] == 0)//当前遍历到的位置ID是否为0（即空格子）
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
            else//不为空格子
            {
                if (!gameManager.rules.IsSameSide(chessID, position[x, y]))//不是一边的棋子则可以走，且遍历结束
                {
                    GetCanMovePos(position, fromX, fromY, x, y);
                }
                break;
            }
            y++;
            x--;
        }

        //右下
        x = fromX + 1;
        y = fromY + 1;
        while (y < 8 && x <8 )
        {
            if (position[x, y] == 0)//当前遍历到的位置ID是否为0（即空格子）
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
            else//不为空格子
            {
                if (!gameManager.rules.IsSameSide(chessID, position[x, y]))//不是一边的棋子则可以走，且遍历结束
                {
                    GetCanMovePos(position, fromX, fromY, x, y);
                }
                break;
            }
            y++;
            x++;
        }
        //左上
        x = fromX - 1;
        y = fromY - 1;
        while (x >= 0 && y >=0)
        {
            if (position[x, y] == 0)//当前遍历到的位置ID是否为0（即空格子）
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
            else//不为空格子
            {
                if (!gameManager.rules.IsSameSide(chessID, position[x, y]))//不是一边的棋子则可以走，且遍历结束
                {
                    GetCanMovePos(position, fromX, fromY, x, y);
                }
                break;
            }
            y--;
            x--;
        }

        //左下
        x = fromX + 1; y = fromY - 1;
        while (y >=0 && x <8)
        {
            if (position[x, y] == 0)//当前遍历到的位置ID是否为0（即空格子）
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
            else//不为空格子
            {
                if (!gameManager.rules.IsSameSide(chessID, position[x, y]))//不是一边的棋子则可以走，且遍历结束
                {
                    GetCanMovePos(position, fromX, fromY, x, y);
                }
                break;
            }
            y--;
            x++;
        }
    }
    private void GetXiangMove(int[,] position, int fromX, int fromY)
    {
        int x, y;
        int chessID = position[fromX, fromY];
        //右上
        x = fromX - 1; 
        y = fromY + 1;
        while (x >= 0 && y < 8)
        {
            if (position[x, y] == 0)//当前遍历到的位置ID是否为0（即空格子）
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
            else//不为空格子
            {
                if (!gameManager.rules.IsSameSide(chessID, position[x, y]))//不是一边的棋子则可以走，且遍历结束
                {
                    GetCanMovePos(position, fromX, fromY, x, y);
                }
                break;
            }
            y++;
            x--;
        }

        //右下
        x = fromX + 1;
        y = fromY + 1;
        while (y < 8 && x < 8)
        {
            if (position[x, y] == 0)//当前遍历到的位置ID是否为0（即空格子）
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
            else//不为空格子
            {
                if (!gameManager.rules.IsSameSide(chessID, position[x, y]))//不是一边的棋子则可以走，且遍历结束
                {
                    GetCanMovePos(position, fromX, fromY, x, y);
                }
                break;
            }
            y++;
            x++;
        }
        //左上
        x = fromX - 1;
        y = fromY - 1;
        while (x >= 0 && y >= 0)
        {
            if (position[x, y] == 0)//当前遍历到的位置ID是否为0（即空格子）
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
            else//不为空格子
            {
                if (!gameManager.rules.IsSameSide(chessID, position[x, y]))//不是一边的棋子则可以走，且遍历结束
                {
                    GetCanMovePos(position, fromX, fromY, x, y);
                }
                break;
            }
            y--;
            x--;
        }

        //左下
        x = fromX + 1;
        y = fromY - 1;
        while (y >= 0 && x < 8)
        {
            if (position[x, y] == 0)//当前遍历到的位置ID是否为0（即空格子）
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
            else//不为空格子
            {
                if (!gameManager.rules.IsSameSide(chessID, position[x, y]))//不是一边的棋子则可以走，且遍历结束
                {
                    GetCanMovePos(position, fromX, fromY, x, y);
                }
                break;
            }
            y--;
            x++;
        }
    }
    private void GetB_bingMove(int[,] position, int fromX, int fromY)//黑兵
    {   int x, y;
        int chessID = position[fromX, fromY];
        //向前
        x = fromX + 1;
        y = fromY;
        if (x<8&&gameManager.rules.IsValidMove(position, fromX, fromY, x, y))
        {
            GetCanMovePos(position, fromX, fromY, x, y);
        }
        //向右前
        x = fromX + 1;
        y = fromY+1;
        if ((x <8 && y < 8) && position[x, y] != 0)//当斜方有棋子时，判断是否为敌人，是则显示移动ui
        {
            if (!gameManager.rules.IsSameSide(chessID, position[x, y]))
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
        }
        //向左前
        x = fromX + 1;
        y = fromY -1;
        if ((x <8  && y >= 0) && position[x, y] != 0)
        {
            if (!gameManager.rules.IsSameSide(chessID, position[x, y]))
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
        }
    }
    private void GetW_bingMove(int[,] position, int fromX, int fromY)//白兵
    {   
        int x, y;
        int chessID = position[fromX, fromY];
        //向前
        x = fromX - 1;
        y = fromY;
        if (x >=0 && gameManager.rules.IsValidMove(position, fromX, fromY, x, y))
        {
            GetCanMovePos(position, fromX, fromY, x, y);
        }

        //向右前
        x = fromX - 1;
        y = fromY + 1;
        if ((x >= 0&&y<8) && position[x, y] != 0)
        {
            if (!gameManager.rules.IsSameSide(chessID, position[x, y]))
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
        }
            
        //向左前
        x = fromX - 1;
        y = fromY - 1;
        if ((x >= 0&&y>=0) && position[x, y] != 0)
        {
            if (!gameManager.rules.IsSameSide(chessID, position[x, y]))
            {
                GetCanMovePos(position, fromX, fromY, x, y);
            }
        }
    }

    #endregion

    public void GetCanMovePos(int[,] position,int fromX,int fromY,int toX,int toY)
    {
        GameObject item;//游戏物体的变量 作为一个容器
        if (position[toX, toY] == 0)//是空格子，可移动的位置
        {
            item = gameManager.PopCanMoveUI();
        }
        else//是棋子,代表此棋子可吃
        {
            item = gameManager.PopCanEatUI();
        }
        item.transform.SetParent(gameManager.boardGrid[toX,toY].transform);//设置好ui的父对象
        item.transform.localPosition = Vector3.zero;
        item.transform.localScale = Vector3.one;
    }
}
