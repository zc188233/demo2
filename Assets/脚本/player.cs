using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    // 场景枚举 - 直接包含场景路径字符串
    public enum GameScene
    {
        MainPage = 1,    // Scenes/主页面
        CardTable = 2    // Scenes/牌桌
    }
    
    public static int playerstatus = 0;
    public static int currentLevel = 1; // 当前关卡
    public static string currentScene = "牌桌"; // 当前场景名称
    public static int choiceCount = 0; // 已做选择数量
    public static List<int> choiceHistory = new List<int>(); // 选择历史记录
    public float moveSpeed = 5f; // 移动速度
    private Rigidbody rb;
    
    // 位置变化事件
    public delegate void PositionChanged(Vector3 newPosition);
    public static event PositionChanged OnPositionChanged;
    
    // 场景变化事件
    public delegate void SceneChanged(string newScene);
    public static event SceneChanged OnSceneChanged;
    
    // Start is called before the first frame update
    void Start()
    {
        // 获取当前场景名称
        currentScene = SceneManager.GetActiveScene().name;
        
        // 获取刚体组件
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            // 如果没有刚体组件，添加一个
            rb = gameObject.AddComponent<Rigidbody>();
            // 防止旋转
            rb.freezeRotation = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 获取键盘输入
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D 或 左右箭头
        float moveVertical = Input.GetAxis("Vertical"); // W/S 或 上下箭头
        
        // 创建移动向量
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        
        // 规范化移动向量并乘以速度
        movement = movement.normalized * moveSpeed * Time.deltaTime;
        
        // 使用封装的移动函数
        MovePosition(movement);
    }
    
    // 封装的位置移动函数
    public void MovePosition(Vector3 movement)
    {
        // 移动玩家
        transform.Translate(movement);
        
        // 可选：使用刚体移动（更适合物理交互）
        // if (rb != null)
        // {
        //     rb.MovePosition(rb.position + movement);
        // }
        
        // 触发位置变化事件
        if (OnPositionChanged != null)
        {
            OnPositionChanged(transform.position);
        }
    }
    
    // 获取场景枚举对应的场景路径
    private static string GetScenePath(GameScene scene)
    {
        switch (scene)
        {
            case GameScene.MainPage:
                return "Scenes/主页面";
            case GameScene.CardTable:
                return "Scenes/牌桌";
            default:
                Debug.LogWarning("Unexpected scene enum value: " + scene);
                return string.Empty;
        }
    }
    
    // 场景跳跃函数 - 参数改为GameScene枚举类型
    public static void JumpToScene(GameScene gameScene)
    {
        // 获取场景路径
        string sceneName = GetScenePath(gameScene);
        
        // 验证场景路径
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("Invalid scene enum value: " + gameScene);
            return;
        }
        
        // 保存当前场景名称
        currentScene = sceneName;
        
        // 加载新场景
        SceneManager.LoadScene(sceneName);
        
        // 触发场景变化事件
        if (OnSceneChanged != null)
        {
            OnSceneChanged(sceneName);
        }
        
        Debug.Log("Player jumped to scene: " + sceneName + " (" + gameScene + ")");
    }
    
    // 设置当前场景
    public static void SetCurrentScene(string sceneName)
    {
        currentScene = sceneName;
        
        // 触发场景变化事件
        if (OnSceneChanged != null)
        {
            OnSceneChanged(sceneName);
        }
    }
    
    // 获取玩家当前位置
    public static Vector3 GetCurrentPosition(GameObject playerObj)
    {
        if (playerObj != null)
        {
            return playerObj.transform.position;
        }
        return Vector3.zero;
    }
    
    // 记录玩家选择
    public static void RecordChoice(int choice)
    {
        choiceCount++;
        choiceHistory.Add(choice);
    }
    
    // 重置选择记录
    public static void ResetChoices()
    {
        choiceCount = 0;
        choiceHistory.Clear();
    }
    
    // 设置当前关卡
    public static void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }
}