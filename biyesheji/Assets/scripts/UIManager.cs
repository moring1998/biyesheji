using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 控制页面之间的显示与页面跳转、加载游戏、按钮的触发方法，在GameManager之后实例化
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance
    {
        get;private set;
    }
    public GameObject[] panels;//  0.主菜单  1.风格选择  2.中国风 3.卡通风 4.拟人风 
    public Text TipUIText;//当前需要改变具体文本的显示UI
    public Text[] TipUITexts;//两个对应显示UI的引用
    private GameManager gameManager;



    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region 页面跳转
    /// <summary>
    /// 中国风的开始游戏
    /// </summary>
    public void standaloneMode()
    {   
        LoadGame();
    }
    /// <summary>
    /// 卡通风的开始游戏
    /// </summary>
    public void standaloneMode1()
    {
        LoadGame1();
    }
    /// <summary>
    /// 拟人风的开始游戏
    /// </summary>
    public void standaloneMode2()
    {
        LoadGame2();
    }

    /// <summary>
    /// 模式选择
    /// </summary>
    public void sty()
    {
        panels[1].SetActive(true);
        panels[0].SetActive(false);
    }
    /// <summary>
    ///  退出游戏
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion

    #region 加载游戏
    /// <summary>
    /// 中国风加载
    /// </summary>
    private void LoadGame()
    {
        gameManager.ResetGame();
        SetUI();
        panels[2].SetActive(true);
        panels[1].SetActive(false);
        TipUIText = TipUITexts[0];
        gameManager.ChessPeople = 1;
    }

    public void SetUI()
    {
        panels[0].SetActive(true);
    }
    /// <summary>
    /// 卡通风游戏加载
    /// </summary>
    public void SetUI1()
    {
        panels[0].SetActive(true);
    }
    private void LoadGame1()
    {
        gameManager.ResetGame1();
        SetUI1();
        panels[3].SetActive(true);
        panels[1].SetActive(false);
        TipUIText = TipUITexts[2];
        gameManager.ChessPeople = 2;
    }

    /// <summary>
    /// 拟人风加载
    /// </summary>
    public void SetUI2()
    {
        panels[0].SetActive(true);
    }
    private void LoadGame2()
    {
        gameManager.ResetGame2();
        SetUI2();
        panels[4].SetActive(true);
        panels[1].SetActive(false);
        TipUIText = TipUITexts[1];
        gameManager.ChessPeople = 3;
    }
   
    #endregion

    #region 游戏中的UI方法
    /// <summary>
    /// 悔棋
    /// </summary>
    public void UnDo()
    {
        gameManager.chessReseting.ResetChess();
    }
    /// <summary>
    /// 重玩
    /// </summary>
    public void Replay()
    {
        gameManager.Replay();
    }
    private void ClearAllData()
    {
        if (gameManager.boardGrid != null)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    GameObject item = gameManager.boardGrid[i, j];//获取游戏物体
                    if (item != null)
                    {
                        foreach (Transform t in item.transform)
                            Destroy(t.gameObject);
                    }
                }
            }
            gameManager.boardGrid = null;
        }
        gameManager.boardGo = null;
        gameManager.rules = null;
        gameManager.movingOfChess = null;
        gameManager.checkMate = null;
        gameManager.canMoveUIStack.Clear();
        gameManager.canMoveUIStack = null;
        gameManager.canEatUIStack.Clear();
        gameManager.canEatUIStack = null;
        gameManager.currentCanEatUIStack.Clear();
        gameManager.currentCanEatUIStack = null;
        gameManager.currentCanMoveUIStack.Clear();
        gameManager.currentCanMoveUIStack = null;
        gameManager.chessReseting.chessSteps = null;
        gameManager.chessReseting = null;
        gameManager.chesssBoard = null;
    }
    /// <summary>
    /// 返回
    /// </summary>
    public void ReturnToMain()
    {
        panels[2].SetActive(false);
        panels[0].SetActive(true);
        gameManager.Replay();
        gameManager.gameOver = true;
        ClearAllData();
    }
    public void ReturnToMain1()
    {
        panels[3].SetActive(false);
        panels[0].SetActive(true);
        gameManager.Replay();
        gameManager.gameOver = true;
        ClearAllData();
    }
    public void ReturnToMain2()
    {
        panels[4].SetActive(false);
        panels[0].SetActive(true);
        gameManager.Replay();
        gameManager.gameOver = true;
        ClearAllData();
    }
    /// <summary>
    /// 下棋轮次及提示
    /// </summary>
    public void ShowTip(string str)
    {
        TipUIText.text = str;
    }
    
    #endregion
}
