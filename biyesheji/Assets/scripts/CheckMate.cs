using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 检测是否将军的类
/// </summary>
public class CheckMate 
{
    public GameManager gameManager;
    public CheckMate()
    {
        gameManager = GameManager.instance;
    }

    private int B_wangx, B_wangy, W_wangx, W_wangy;
    /// <summary>
    /// 是否将军的检测放法
    /// </summary>
   public void JudgeIfCheckmate()
    {
        GetKingPosition();
        //遍历中获取的索引位置上没有黑王，表示已经被吃掉了
        if (gameManager.chesssBoard[B_wangx,B_wangy]!=1)
        {
            UIManager.Instance.ShowTip("白色棋子胜利");
            gameManager.gameOver = true;
            return;
        }
        //白王不存在
        else if (gameManager.chesssBoard[W_wangx,W_wangy]!=7)
        {
            UIManager.Instance.ShowTip("黑色棋子胜利");
            gameManager.gameOver = true;
            return;
        }
        //以下是将军的判断
        bool ifCheckmate;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                switch (gameManager.chesssBoard[i, j])
                {
                    case 2:
                        ifCheckmate = gameManager.rules.IsValidMove(gameManager.chesssBoard, i, j, W_wangx, W_wangy);
                        if (ifCheckmate)
                        {
                            UIManager.Instance.ShowTip("白王被黑车将军了");
                        }
                        break;
                    case 3:
                        ifCheckmate = gameManager.rules.IsValidMove(gameManager.chesssBoard, i, j, W_wangx, W_wangy);
                        if (ifCheckmate)
                        {
                            UIManager.Instance.ShowTip("白王被黑马将军了");
                        }
                        break;
                    case 4:
                        ifCheckmate = gameManager.rules.IsValidMove(gameManager.chesssBoard, i, j, W_wangx, W_wangy);
                        if (ifCheckmate)
                        {
                            UIManager.Instance.ShowTip("白王被黑后将军了");
                        }
                        break;
                    case 5:
                        ifCheckmate = gameManager.rules.IsValidMove(gameManager.chesssBoard, i, j, W_wangx, W_wangy);
                        if (ifCheckmate)
                        {
                            UIManager.Instance.ShowTip("白王被黑象将军了");
                        }
                        break;
                    case 6:
                        ifCheckmate = gameManager.rules.IsValidMove(gameManager.chesssBoard, i, j, W_wangx, W_wangy);
                        if (ifCheckmate)
                        {
                            UIManager.Instance.ShowTip("白王被黑兵将军了");
                        }
                        break;
                    case 8:
                        ifCheckmate = gameManager.rules.IsValidMove(gameManager.chesssBoard, i, j, B_wangx, B_wangy);
                        if (ifCheckmate)
                        {
                            UIManager.Instance.ShowTip("黑王被白车将军了");
                        }
                        break;
                    case 9:
                        ifCheckmate = gameManager.rules.IsValidMove(gameManager.chesssBoard, i, j, B_wangx, B_wangy);
                        if (ifCheckmate)
                        {
                            UIManager.Instance.ShowTip("黑王被白马将军了");
                        }
                        break;
                    case 10:
                        ifCheckmate = gameManager.rules.IsValidMove(gameManager.chesssBoard, i, j, B_wangx, B_wangy);
                        if (ifCheckmate)
                        {
                            UIManager.Instance.ShowTip("黑王被白后将军了");
                        }
                        break;
                    case 11:
                        ifCheckmate = gameManager.rules.IsValidMove(gameManager.chesssBoard, i, j, B_wangx, B_wangy);
                        if (ifCheckmate)
                        {
                            UIManager.Instance.ShowTip("黑王被白象将军了");
                        }
                        break;
                    case 12:
                        ifCheckmate = gameManager.rules.IsValidMove(gameManager.chesssBoard, i, j, B_wangx, B_wangy);
                        if (ifCheckmate)
                        {
                            UIManager.Instance.ShowTip("黑王被白兵将军了");
                        }
                        break;
                    default:
                        break;
                }
            }
        }

    }
    
    //获取王的位置
    private void GetKingPosition()
    {
        //找到黑王的所在位置
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (gameManager.chesssBoard[i,j]==1)
                {
                    B_wangx = i;
                    B_wangy = j;
                }
            }
        }
        //找到白王的所在位置
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (gameManager.chesssBoard[i, j] == 7)
                {
                    W_wangx = i;
                    W_wangy = j;
                }
            }
        }
    }
}
