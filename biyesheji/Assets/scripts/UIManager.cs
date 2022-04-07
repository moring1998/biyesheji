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
    public GameObject[] panels;//  0.主菜单  1.单机模式游戏界面 2.登录界面 3.联网模式游戏界面 
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
    /// 单机模式
    /// </summary>
    public void standaloneMode()
    {   LoadGame();
        //panels[0].SetActive(false);
        //panels[1].SetActive(true);
        
    }

    /// <summary>
    /// 联网模式
    /// </summary>
    public void NetWorkingMode()
    {
        panels[0].SetActive(false);
        panels[2].SetActive(true);
    }

    //public void networking()
    //{
    //    panels[2].SetActive(false);
    //    panels[3].SetActive(true);
    //}

    /// <summary>
    ///  退出游戏
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion

    #region 加载游戏
    private void LoadGame()
    {
        gameManager.ResetGame();
        SetUI();
        panels[1].SetActive(true);
        panels[0].SetActive(false);
    }

    public void SetUI()
    {
        panels[0].SetActive(true);
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
    /// <summary>
    /// 返回
    /// </summary>
    public void ReturnToMain()
    {
        panels[1].SetActive(false);
        gameManager.Replay();
        gameManager.gameOver = true;
    }
    /// <summary>
    /// 下棋轮次及提示
    /// </summary>
    public void ShowTip(string str)
    {
        //测试
        TipUIText = TipUITexts[0];
        //
        TipUIText.text = str;
    }
    /// <summary>
    /// 联网模式下的开始
    /// </summary>
    public void StartNetWorKingMode()
    {

    }
    /// <summary>
    /// 认输
    /// </summary>
    public void GiveUp()
    {

    }
    #endregion
}
