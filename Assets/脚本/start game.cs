using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startgame : MonoBehaviour
{
    [Header("主菜单按钮")]
    public Button newGameButton;     // 开始新记录按钮
    public Button continueButton;    // 继续游戏按钮
    public Button settingsButton;    // 系统设置按钮
    public Button exitButton;        // 终止进程按钮
    
    [Header("系统设置")]
    public GameObject settingsPanel; // 系统设置面板
    public Toggle soundToggle;       // 音效开关
    public Toggle musicToggle;       // 音乐开关
    public Button applySettingsButton; // 应用设置按钮
    public Button backButton;        // 返回按钮
    
    [Header("音频管理")]
    public AudioSource soundEffectSource;
    public AudioSource musicSource;
    
    void Start()
    {
        // 初始化按钮事件
        InitializeButtons();
        
        // 加载保存的设置
        LoadSettings();
        
        // 隐藏设置面板
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }
    
    // 初始化所有按钮事件
    private void InitializeButtons()
    {
        // 主菜单按钮
        if (newGameButton != null)
            newGameButton.onClick.AddListener(StartNewGame);
        
        if (continueButton != null)
            continueButton.onClick.AddListener(ContinueGame);
        
        if (settingsButton != null)
            settingsButton.onClick.AddListener(ShowSettings);
        
        if (exitButton != null)
            exitButton.onClick.AddListener(ExitGame);
        
        // 设置面板按钮
        if (applySettingsButton != null)
            applySettingsButton.onClick.AddListener(ApplySettings);
        
        if (backButton != null)
            backButton.onClick.AddListener(HideSettings);
    }
    
    // 开始新记录 - 执行名字为"牌桌"的场景
    public void StartNewGame()
    {
        Debug.Log("Starting new game with scene '牌桌'");
        
        // 重置玩家数据
        if (typeof(player) != null)
        {
            player.playerstatus = 0;
            player.currentLevel = 1;
            player.ResetChoices();
        }
        
        // 使用数字映射的场景跳跃函数
        player.JumpToScene(player.GameScene.CardTable);
    }
    
    // 继续游戏 - 加载最新的存档
    public void ContinueGame()
    {
        Debug.Log("Continuing game from save");
        
        // 检查是否有存档（默认检查槽位1）
        if (SaveLoadManager.HasSave(1))
        {
            // 加载槽位1的存档
            SaveLoadManager.LoadGame(1);
            
            // 检查存档数据是否有效
            if (player.currentLevel > 0)
            {
                // 使用数字映射的场景跳跃函数
                player.JumpToScene(player.GameScene.CardTable);
                return;
            }
            else
            {
                //执行开始新游戏
                StartNewGame();
            }
        }
        
        // 如果槽位1没有有效存档，检查其他槽位
        for (int slot = 2; slot <= SaveLoadManager.MAX_SAVE_SLOTS; slot++)
        {
            if (SaveLoadManager.HasSave(slot))
            {
                SaveLoadManager.LoadGame(slot);
                if (player.currentLevel > 0)
                {
                    player.JumpToScene(player.GameScene.CardTable);
                    return;
                }
            }
        }
        
        // 如果没有任何有效存档
        Debug.LogWarning("No valid save found, starting new game");
        StartNewGame();
    }
    
    // 显示系统设置
    public void ShowSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }
    
    // 隐藏系统设置
    public void HideSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }
    
    // 加载设置
    public void LoadSettings()
    {
        bool soundEnabled = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
        bool musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        
        if (soundToggle != null)
            soundToggle.isOn = soundEnabled;
        
        if (musicToggle != null)
            musicToggle.isOn = musicEnabled;
        
        ApplyAudioSettings(soundEnabled, musicEnabled);
    }
    
    // 应用设置
    public void ApplySettings()
    {
        bool soundEnabled = soundToggle != null && soundToggle.isOn;
        bool musicEnabled = musicToggle != null && musicToggle.isOn;
        
        PlayerPrefs.SetInt("SoundEnabled", soundEnabled ? 1 : 0);
        PlayerPrefs.SetInt("MusicEnabled", musicEnabled ? 1 : 0);
        PlayerPrefs.Save();
        
        ApplyAudioSettings(soundEnabled, musicEnabled);
        
        Debug.Log("Settings applied: Sound=" + soundEnabled + ", Music=" + musicEnabled);
    }
    
    // 应用音频设置
    private void ApplyAudioSettings(bool soundEnabled, bool musicEnabled)
    {
        if (soundEffectSource != null)
            soundEffectSource.mute = !soundEnabled;
        
        if (musicSource != null)
            musicSource.mute = !musicEnabled;
    }
    
    // 终止进程
    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    
    // Update is called once per frame
    void Update()
    {
        // 可以在这里添加键盘快捷键等功能
    }
}