using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollText : MonoBehaviour
{
    [Header("滚动设置")]
    public TextMeshProUGUI scrollText;
    public float scrollSpeed = 50f;      // 滚动速度
    public float startDelay = 2f;        // 开始延迟时间
    public float endDelay = 3f;          // 结束停留时间
    
    // 自动计算的位置（无需手动设置）
    private Vector3 startPosition;        // 开始位置（使用text的初始位置）
    private Vector3 endPosition;          // 结束位置（y轴加1000）
    
    private float timer = 0f;            // 计时器
    private bool isScrolling = false;    // 是否正在滚动
    private bool isFinished = false;     // 是否完成
    
    void Start()
    {
        // 初始化文字位置
        if(scrollText != null)
        {
            // 获取scrollText的初始位置作为开始位置
            startPosition = scrollText.transform.position;
            // 计算结束位置（y轴加1000）
            endPosition = new Vector3(startPosition.x, startPosition.y + 1000f, startPosition.z);
        }
    }
    
    void Update()
    {
        if (isFinished)
        {
            // 使用数字映射的场景跳跃函数
            player.JumpToScene(player.GameScene.MainPage);
        }

            
        timer += Time.deltaTime;
        
        // 开始延迟
        if(timer < startDelay)
            return;
        
        // 开始滚动
        if(!isScrolling)
        {
            isScrolling = true;
            timer = 0f;
        }
        
        // 滚动文字 - 使用scrollSpeed控制实际速度
        if(isScrolling)
        {
            // 计算当前位置
            Vector3 currentPosition = scrollText.transform.position;
            
            // 向上移动
            currentPosition.y += scrollSpeed * Time.deltaTime;
            
            // 检查是否到达结束位置
            if(currentPosition.y >= endPosition.y)
            {
                currentPosition.y = endPosition.y;
                isFinished = true;
            }
            
            // 更新位置
            scrollText.transform.position = currentPosition;
        }
    }
    
    // 重置滚动
    public void ResetScroll()
    {
        timer = 0f;
        isScrolling = false;
        isFinished = false;
        if(scrollText != null)
        {
            scrollText.transform.position = startPosition;
        }
    }
}