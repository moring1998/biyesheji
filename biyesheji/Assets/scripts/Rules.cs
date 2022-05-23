using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 棋子的规则类
/// </summary>
public class Rules
{
    /// <summary>
    /// 检查当前移动是否合法
    /// </summary>
    /// <param name="position">当前棋盘的状况</param>
    /// <param name="FromX">来的位置X索引</param>
    /// <param name="FromY">来的位置Y索引</param>
    /// <param name="ToX">去的位置X索引</param>
    /// <param name="ToY">去的位置Y索引</param>
    /// <returns></returns>
    public bool IsValidMove(int [,] position,int FromX,int FromY,int ToX,int ToY)
    {
        //获得棋子的来去位置
        int moveChessID, targetID;
        moveChessID = position[FromX, FromY];
        targetID = position[ToX, ToY];
        if (IsSameSide(moveChessID, targetID))
        {
            return false;//当前是同一颜色棋子时，移动不合法
        }

        Debug.Log("当前位置" + FromX+","+ FromY);
        Debug.Log("结束位置" + ToX + "," + ToY);
        return IsValid(moveChessID, position, FromX, FromY, ToX, ToY);
        //return true;
    }
    /// <summary>
    /// 判断当前两个棋子是否为同一方的棋
    /// </summary>
    /// <param name="x"></param>
    /// <param name="Y"></param>
    /// <returns></returns>
    public bool IsSameSide(int x,int Y)
    {
        if (IsBlack(x)&&IsBlack(Y)||IsWrite(x)&&IsWrite(Y))
        {
            
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 判断当前游戏物体是否为黑棋
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public bool IsBlack(int x)//根据id判断 0-6是黑棋
    {
        if (x > 0 && x < 7)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 判断当前游戏物体是否是白棋
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public bool IsWrite(int x)//7-12是白棋
    {
        if (x > 6 && x < 13)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 所有种类棋子的走法是否合法
    /// </summary>
    /// <param name="moveChessID"></param>
    /// <param name="position"></param>
    /// <param name="FromX"></param>
    /// <param name="FromY"></param>
    /// <param name="ToX"></param>
    /// <param name="ToY"></param>
    /// <returns></returns>
    public bool IsValid(int moveChessID,int[,] position, int FromX, int FromY, int ToX, int ToY)
    {
        //目的地于原位置相同的情况
        if (FromX == ToX && FromY == ToY)
        {
            return false;
        }
        switch (moveChessID)
        {
            //王的走棋规则：横直斜都可走但每次限制一步
            case 1://黑王 
            case 7://白王
                if (Mathf.Abs(ToX - FromX) > 1 || Mathf.Abs(ToY - FromY) > 1)
                {
                    return false;
                }
                break;


            //兵的行走规则：只能往前直走，且走一格,斜前方一格有敌人可吃子
            case 6://黑兵
                    
                    if (ToX-FromX!=1)//不是一格或者不是向前走
                        {
                            Debug.Log("不是向前一格");
                            return false;                   
                         }
               
                    if (position[FromX + 1, FromY] != 0)//前方有棋子，判断一哈斜方有无棋子
                    {
                        if (Mathf.Abs(FromX - ToX) == Mathf.Abs(FromY - ToY))//判断去的位置是斜前方一格
                        {
                            Debug.Log("斜方没有棋子，不能吃子");
                            if (position[ToX, ToY] == 0)//如果没有棋子就不走
                            {
                                return false;
                            }
                        }
                        else
                        {
                            Debug.Log("前方有棋子不能走");
                            return false;
                        }
                    }
                else//前方可以移动的情况下判断斜方
                {
                    if (Mathf.Abs(FromX - ToX) == Mathf.Abs(FromY - ToY))//判断去的位置是斜前方一格
                    {
                        Debug.Log("斜方没有棋子，不能走");
                        if (position[ToX, ToY] == 0)//如果没有棋子就不走
                        {
                            return false;
                        }
                    }
                }
                break;
                




            case 12://白兵
                    if (FromX - ToX!= 1)//不是往前走一格
                        {
                            Debug.Log("不是向前一格");
                            return false;
                        }
                    if (position[FromX - 1, FromY] != 0)
                    {
                        if (Mathf.Abs(FromX - ToX) == Mathf.Abs(FromY - ToY))//判断去的位置是斜前方一格
                        {
                            Debug.Log("斜方没有棋子，不能吃子");
                            if (position[ToX, ToY] == 0)//如果没有棋子就不走
                            {
                                return false;
                            }
                        }
                        else
                        {
                            Debug.Log("前方有棋子不能走");
                            return false;
                        }
                }
                else
                {
                    if (Mathf.Abs(FromX - ToX) == Mathf.Abs(FromY - ToY))//判断去的位置是斜前方一格
                    {
                        Debug.Log("斜方没有棋子，不能走");
                        if (position[ToX, ToY] == 0)//如果没有棋子就不走
                        {
                            return false;
                        }
                    }
                }



                break;


            //车的行走规则：横竖都可以走，步数不受限制，但不能斜走也不能越子
            case 8://白车
            case 2://黑车 
                if (FromX!=ToX&&FromY!=ToY)//斜着走
                {
                    return false;
                }
                int a = 0, b = 0;
                if (FromX == ToX)//走横线时
                {
                    if (FromY < ToY)//右走
                    {
                        //遍历移动路径上有无棋子，有则不能越子行走
                        for (a = FromY + 1; a < ToY; a++)
                        {
                            if (position[FromX, a] != 0)
                            {
                                return false;
                            }
                        }
                    }
                    else//左走
                    {
                        for (a = ToY + 1; a < FromY; a++)
                        {
                            if (position[FromX, a] != 0)
                            {
                                return false;
                            }
                        }
                    }
                }
                if (FromY == ToY)//走竖线时
                {
                    if (FromX < ToX)//往下走
                    {
                        for (b = FromX + 1; b < ToX; b++)
                        {
                            if (position[b, FromY] != 0)
                            {
                                return false;
                            }
                        }
                    }
                    else //往上走
                    {
                        for (b = ToX + 1; b < FromX; b++)
                        {
                            if (position[b, FromY] != 0)
                            {
                                return false;
                            }
                        }
                    }
                }
                    break;


            //马的走棋规则：走日，可越子行走
            case 3://黑马 
            case 9://白马
                    //竖日和横日两种情况
                if (!(Mathf.Abs(ToY-FromY)==1&&Mathf.Abs(ToX-FromX)==2
                    ||Mathf.Abs(ToY-FromY)==2&&Mathf.Abs(ToX-FromX)==1))
                {
                    return false;
                }
                break;

            //王后的走棋规则：横直斜都可走不限步数，不可越子
            case 4://黑后
            case 10://白后
                //横线，直线，斜线
                if (FromX!=ToX&&FromY!=ToY&& Mathf.Abs(FromX - ToX) != Mathf.Abs(FromY - ToY))
                {
                    return false;
                }
                //int a = 0, b = 0;
                if (FromX==ToX)//走横线时
                {
                    if (FromY<ToY)//右走
                    { 
                        //遍历移动路径上有无棋子，有则不能越子行走
                        for ( a = FromY+1; a < ToY; a++)
                        {
                            if (position[FromX,a]!=0)
                            {
                                return false;
                            }
                        }
                    }
                    else//左走
                    {
                        for (a = ToY+1; a < FromY; a++)
                        {
                            if (position[FromX,a]!=0)
                            {
                                return false;
                            }
                        }
                    }
                }
                if (FromY==ToY)//走竖线时
                {
                    if (FromX < ToX)//往下走
                    {
                        for ( b = FromX+1; b < ToX; b++)
                        {
                            if (position[b,FromY]!=0)
                            {
                                return false;
                            }
                        }
                    }
                    else //往上走
                    {
                        for ( b = ToX+1; b < FromX; b++)
                        {
                            if (position[b,FromY]!=0)
                            {
                                return false;
                            }
                        }
                    }
                    
                }

                if (Mathf.Abs(FromX - ToX) == Mathf.Abs(FromY - ToY))//走斜线的时候
                {
                    if (ToX > FromX && ToY > FromY)//右下的情况
                    {
                        for (a = FromX + 1, b = FromY + 1; a < ToX && b < ToY; a++, b++)
                        {

                            if (position[a, b] != 0)
                            {
                                Debug.Log("右下有棋子，过不了");
                                return false;
                            }
                            Debug.Log("右下");

                        }
                    }

                    if (ToX > FromX && ToY < FromY)//左下的情况
                    {
                        for (a = FromX + 1, b = FromY - 1; a <ToX && b > ToY; a++, b--)
                        {

                            if (position[a, b] != 0)
                            {
                                Debug.Log("左下有棋子，过不了");
                                return false;
                            }
                            Debug.Log("左下");

                        }
                    }


                    if (FromX > ToX && ToY > FromY)//往右上走
                    {
                        for (a = FromX - 1, b = FromY + 1; a > ToX && b < ToY; a--, b++)
                        {

                            if (position[a, b] != 0)
                            {
                                Debug.Log("右上有棋子，过不了");
                                return false;
                            }
                            Debug.Log("右上");

                        }
                    }

                    if (FromX > ToX && ToY < FromY)//往左上
                    {
                        for (a = FromX - 1, b = FromY - 1; a > ToX && b > ToY; a--, b--)
                        {

                            if (position[a, b] != 0)
                            {
                                Debug.Log("左上有棋子，过不了");
                                return false;
                            }
                            Debug.Log("左上");

                        }
                    }
                }
                break;


            //象的走棋规则：只能斜走，步数不限，但不能越子
            case 5://黑象
            case 11://白象
                if (Mathf.Abs(FromX - ToX) !=  Mathf.Abs(FromY - ToY) )//只能斜走
                {
                    return false;
                }
                //遍历斜走移动路径上有无棋子，有则不能越子行走
                if (Mathf.Abs(FromX - ToX) == Mathf.Abs(FromY - ToY))//走斜线的时候
                {
                    if (ToX > FromX && ToY > FromY)//右下的情况
                    {
                        for (a = FromX + 1, b = FromY + 1; a < ToX && b < ToY; a++, b++)
                        {

                            if (position[a, b] != 0)
                            {
                                Debug.Log("右下有棋子，过不了");
                                return false;
                            }
                            Debug.Log("右下");

                        }
                    }

                    if (ToX > FromX && ToY < FromY)//左下的情况
                    {
                        for (a = FromX + 1, b = FromY - 1; a < ToX && b > ToY; a++, b--)
                        {

                            if (position[a, b] != 0)
                            {
                                Debug.Log("左下有棋子，过不了");
                                return false;
                            }
                            Debug.Log("左下");

                        }
                    }


                    if (FromX > ToX && ToY > FromY)//往右上走
                    {
                        for (a = FromX - 1, b = FromY + 1; a > ToX && b < ToY; a--, b++)
                        {

                            if (position[a, b] != 0)
                            {
                                Debug.Log("右上有棋子，过不了");
                                return false;
                            }
                            Debug.Log("右上");

                        }
                    }

                    if (FromX > ToX && ToY < FromY)//往左上
                    {
                        for (a = FromX - 1, b = FromY - 1; a > ToX && b > ToY; a--, b--)
                        {

                            if (position[a, b] != 0)
                            {
                                Debug.Log("左上有棋子，过不了");
                                return false;
                            }
                            Debug.Log("左上");

                        }
                    }
                }
                break;
            default:
                break;
        }
        return true;
    }
    
}
