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
        return true;
    }
}
