using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject menuList; //菜单列表
    [SerializeField] private bool menuKeys = true;  
    [SerializeField] private AudioSource bgmSource;   //背景音乐
    [Header("游戏内菜单")]
    public GameObject inGameMenuPanel;
    public Button backToMainButton;     // 回到主菜单
    public Button saveButton;           // 存档
    public Button loadButton;           // 读档
    public Button settingsButton;       // 系统设置
    public Button exitGameButton;       // 退出游戏
    
    [Header("存档菜单")]
    public GameObject saveMenuPanel;
    public Button[] saveButtons;
    public Text[] saveInfoTexts;
    public Button backFromSaveButton;
    
    [Header("读档菜单")]
    public GameObject loadMenuPanel;
    public Button[] loadButtons;
    public Text[] loadInfoTexts;
    public Button backFromLoadButton;
    
    [Header("系统设置")]
    public GameObject settingsPanel;
    public Toggle soundToggle;
    public Toggle musicToggle;
    public Button applySettingsButton;
    public Button backFromSettingsButton;
    
    [Header("音频管理")]
    public AudioSource soundEffectSource;
    public AudioSource musicSource;
    void Update()
    {
        if(menuKeys)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuList.SetActive(true);
                menuKeys = false;
                Time.timeScale = 0; //暂停
            if(bgmSource != null) bgmSource.Pause();//音乐暂停
            }
        } else if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuList.SetActive(false);
            menuKeys = true;
            Time.timeScale = 1; //恢复正常
            if(bgmSource != null) bgmSource.Play();//音乐播放
        }
    }
    void Start()
    {
        menuList.SetActive(false);
        
        // 初始化按钮事件
        InitializeButtons();
        
        // 加载保存的设置
        LoadSettings();
        
        // 更新存档信息显示
        UpdateSaveInfo();
        
        // 隐藏所有菜单面板
        HideAllPanels();
    }
    
    // 隐藏所有菜单面板
    private void HideAllPanels()
    {
        if (inGameMenuPanel != null) inGameMenuPanel.SetActive(false);
        if (saveMenuPanel != null) saveMenuPanel.SetActive(false);
        if (loadMenuPanel != null) loadMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }
    
    // 初始化所有按钮事件
    private void InitializeButtons()
    {
        // 游戏内菜单按钮 - 移除了回到主菜单按钮
        
        if (saveButton != null)
            saveButton.onClick.AddListener(ShowSaveMenu);
        
        if (loadButton != null)
            loadButton.onClick.AddListener(ShowLoadMenu);
        
        if (settingsButton != null)
            settingsButton.onClick.AddListener(ShowSettingsMenu);
        
        if (exitGameButton != null)
            exitGameButton.onClick.AddListener(ExitGame);
        
        // 存档菜单按钮
        if (backFromSaveButton != null)
            backFromSaveButton.onClick.AddListener(ShowInGameMenu);
        
        // 初始化存档按钮
        for (int i = 0; i < SaveLoadManager.MAX_SAVE_SLOTS; i++)
        {
            int slot = i + 1;
            
            if (saveButtons != null && i < saveButtons.Length && saveButtons[i] != null)
                saveButtons[i].onClick.AddListener(() => SaveGame(slot));
        }
        
        // 读档菜单按钮
        if (backFromLoadButton != null)
            backFromLoadButton.onClick.AddListener(ShowInGameMenu);
        
        // 初始化读档按钮
        for (int i = 0; i < SaveLoadManager.MAX_SAVE_SLOTS; i++)
        {
            int slot = i + 1;
            
            if (loadButtons != null && i < loadButtons.Length && loadButtons[i] != null)
                loadButtons[i].onClick.AddListener(() => LoadGame(slot));
        }
        
        // 设置菜单按钮
        if (applySettingsButton != null)
            applySettingsButton.onClick.AddListener(ApplySettings);
        
        if (backFromSettingsButton != null)
            backFromSettingsButton.onClick.AddListener(ShowInGameMenu);
    }
    
    // 保存游戏
    public void SaveGame(int slot)
    {
        SaveLoadManager.SaveGame(slot);
        UpdateSaveInfo();
        Debug.Log("Game saved to slot " + slot);
    }
    
    // 加载游戏
    public void LoadGame(int slot)
    {
        SaveLoadManager.LoadGame(slot);
        
        // 检查是否有存档数据
        if (player.currentLevel > 0)
        {
            // 重新加载当前场景以应用存档数据
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.LogWarning("No valid save data to load");
        }
    }
    
    
    
    // 加载并应用设置
    public void LoadSettings()
    {
        // 加载音效设置
        bool soundEnabled = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
        if (soundToggle != null)
            soundToggle.isOn = soundEnabled;
        
        // 加载音乐设置
        bool musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        if (musicToggle != null)
            musicToggle.isOn = musicEnabled;
        
        // 应用设置
        ApplyAudioSettings(soundEnabled, musicEnabled);
    }
    
    // 应用设置
    public void ApplySettings()
    {
        bool soundEnabled = soundToggle != null && soundToggle.isOn;
        bool musicEnabled = musicToggle != null && musicToggle.isOn;
        
        // 保存设置
        PlayerPrefs.SetInt("SoundEnabled", soundEnabled ? 1 : 0);
        PlayerPrefs.SetInt("MusicEnabled", musicEnabled ? 1 : 0);
        PlayerPrefs.Save();
        
        // 应用音频设置
        ApplyAudioSettings(soundEnabled, musicEnabled);
        
        Debug.Log("Settings applied: Sound=" + soundEnabled + ", Music=" + musicEnabled);
    }
    
    // 应用音频设置
    private void ApplyAudioSettings(bool soundEnabled, bool musicEnabled)
    {
        // 设置音效源
        if (soundEffectSource != null)
            soundEffectSource.mute = !soundEnabled;
        
        // 设置音乐源
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
    
    // 更新存档信息显示
    public void UpdateSaveInfo()
    {
        for (int i = 0; i < SaveLoadManager.MAX_SAVE_SLOTS; i++)
        {
            int slot = i + 1;
            
            // 更新读档信息
            if (loadInfoTexts != null && i < loadInfoTexts.Length && loadInfoTexts[i] != null)
            {
                if (SaveLoadManager.HasSave(slot))
                {
                    loadInfoTexts[i].text = string.Format("存档槽位 {0}\n关卡: {1}\n时间: {2}", 
                        slot, SaveLoadManager.GetSaveLevel(slot), SaveLoadManager.GetSaveTime(slot));
                    
                    // 启用有存档的加载按钮
                    if (loadButtons != null && i < loadButtons.Length && loadButtons[i] != null)
                    {
                        loadButtons[i].interactable = true;
                    }
                }
                else
                {
                    loadInfoTexts[i].text = string.Format("存档槽位 {0}\n空存档", slot);
                    
                    // 禁用空存档的加载按钮
                    if (loadButtons != null && i < loadButtons.Length && loadButtons[i] != null)
                    {
                        loadButtons[i].interactable = false;
                    }
                }
            }
            
            // 更新存档信息
            if (saveInfoTexts != null && i < saveInfoTexts.Length && saveInfoTexts[i] != null)
            {
                if (SaveLoadManager.HasSave(slot))
                {
                    saveInfoTexts[i].text = string.Format("存档槽位 {0}\n关卡: {1}\n时间: {2}", 
                        slot, SaveLoadManager.GetSaveLevel(slot), SaveLoadManager.GetSaveTime(slot));
                }
                else
                {
                    saveInfoTexts[i].text = string.Format("存档槽位 {0}\n空存档", slot);
                }
            }
        }
    }
    
    // 显示游戏内菜单
    public void ShowInGameMenu()
    {
        if (inGameMenuPanel != null)
            inGameMenuPanel.SetActive(true);
        
        if (saveMenuPanel != null)
            saveMenuPanel.SetActive(false);
        
        if (loadMenuPanel != null)
            loadMenuPanel.SetActive(false);
        
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }
    
    // 显示存档菜单
    public void ShowSaveMenu()
    {
        UpdateSaveInfo();
        
        if (inGameMenuPanel != null)
            inGameMenuPanel.SetActive(false);
        
        if (saveMenuPanel != null)
            saveMenuPanel.SetActive(true);
        
        if (loadMenuPanel != null)
            loadMenuPanel.SetActive(false);
        
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }
    
    // 显示读档菜单
    public void ShowLoadMenu()
    {
        UpdateSaveInfo();
        
        if (inGameMenuPanel != null)
            inGameMenuPanel.SetActive(false);
        
        if (saveMenuPanel != null)
            saveMenuPanel.SetActive(false);
        
        if (loadMenuPanel != null)
            loadMenuPanel.SetActive(true);
        
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }
    
    // 显示系统设置
    public void ShowSettingsMenu()
    {
        if (inGameMenuPanel != null)
            inGameMenuPanel.SetActive(false);
        
        if (saveMenuPanel != null)
            saveMenuPanel.SetActive(false);
        
        if (loadMenuPanel != null)
            loadMenuPanel.SetActive(false);
        
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }
}