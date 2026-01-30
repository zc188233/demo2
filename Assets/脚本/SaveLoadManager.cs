using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    // 最大存档槽数量
    public const int MAX_SAVE_SLOTS = 3;
    
    // 保存游戏状态到指定槽位
    public static void SaveGame(int saveSlot)
    {
        // 验证存档槽位
        if (saveSlot < 1 || saveSlot > MAX_SAVE_SLOTS)
        {
            Debug.LogError("Invalid save slot! Must be between 1 and " + MAX_SAVE_SLOTS);
            return;
        }
        
        string prefix = "Save_" + saveSlot + "_";
        
        // 保存玩家状态
        PlayerPrefs.SetInt(prefix + "PlayerStatus", player.playerstatus);
        PlayerPrefs.SetInt(prefix + "CurrentLevel", player.currentLevel);
        PlayerPrefs.SetString(prefix + "CurrentScene", player.currentScene);
        PlayerPrefs.SetInt(prefix + "ChoiceCount", player.choiceCount);
        
        // 保存选择历史记录（将列表转换为逗号分隔的字符串）
        string choiceHistoryString = string.Join(",", player.choiceHistory);
        PlayerPrefs.SetString(prefix + "ChoiceHistory", choiceHistoryString);
        
        // 保存玩家位置
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Vector3 position = playerObj.transform.position;
            PlayerPrefs.SetFloat(prefix + "PlayerPositionX", position.x);
            PlayerPrefs.SetFloat(prefix + "PlayerPositionY", position.y);
            PlayerPrefs.SetFloat(prefix + "PlayerPositionZ", position.z);
        }
        
        // 保存StateSequenceController状态
        StateSequenceController sequenceController = FindObjectOfType<StateSequenceController>();
        if (sequenceController != null)
        {
            PlayerPrefs.SetInt(prefix + "SequenceCurrentIndex", sequenceController.currentIndex);
            PlayerPrefs.SetInt(prefix + "SequenceIsFinished", sequenceController.isSequenceFinished ? 1 : 0);
            
            // 保存状态序列
            string sequenceString = string.Join(",", sequenceController.numberSequence);
            PlayerPrefs.SetString(prefix + "SequenceData", sequenceString);
        }
        
        // 保存存档时间戳
        PlayerPrefs.SetString(prefix + "SaveTime", System.DateTime.Now.ToString());
        
        // 确保所有数据都被保存
        PlayerPrefs.Save();
        
        Debug.Log("Game saved successfully to slot " + saveSlot + "!");
    }
    
    // 从指定槽位加载游戏状态
    public static void LoadGame(int saveSlot)
    {
        // 验证存档槽位
        if (saveSlot < 1 || saveSlot > MAX_SAVE_SLOTS)
        {
            Debug.LogError("Invalid save slot! Must be between 1 and " + MAX_SAVE_SLOTS);
            return;
        }
        
        string prefix = "Save_" + saveSlot + "_";
        
        // 检查存档是否存在
        if (!PlayerPrefs.HasKey(prefix + "SaveTime"))
        {
            Debug.LogWarning("No save data found in slot " + saveSlot + "!");
            return;
        }
        
        // 加载玩家状态
        if (PlayerPrefs.HasKey(prefix + "PlayerStatus"))
        {
            player.playerstatus = PlayerPrefs.GetInt(prefix + "PlayerStatus");
        }
        
        if (PlayerPrefs.HasKey(prefix + "CurrentLevel"))
        {
            player.currentLevel = PlayerPrefs.GetInt(prefix + "CurrentLevel");
        }
        
        if (PlayerPrefs.HasKey(prefix + "CurrentScene"))
        {
            player.currentScene = PlayerPrefs.GetString(prefix + "CurrentScene");
        }
        
        if (PlayerPrefs.HasKey(prefix + "ChoiceCount"))
        {
            player.choiceCount = PlayerPrefs.GetInt(prefix + "ChoiceCount");
        }
        
        // 加载选择历史记录
        if (PlayerPrefs.HasKey(prefix + "ChoiceHistory"))
        {
            string choiceHistoryString = PlayerPrefs.GetString(prefix + "ChoiceHistory");
            if (!string.IsNullOrEmpty(choiceHistoryString))
            {
                string[] choiceArray = choiceHistoryString.Split(',');
                player.choiceHistory.Clear();
                foreach (string choice in choiceArray)
                {
                    if (int.TryParse(choice, out int choiceValue))
                    {
                        player.choiceHistory.Add(choiceValue);
                    }
                }
            }
        }
        
        // 加载玩家位置
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null && PlayerPrefs.HasKey(prefix + "PlayerPositionX"))
        {
            float x = PlayerPrefs.GetFloat(prefix + "PlayerPositionX");
            float y = PlayerPrefs.GetFloat(prefix + "PlayerPositionY");
            float z = PlayerPrefs.GetFloat(prefix + "PlayerPositionZ");
            
            playerObj.transform.position = new Vector3(x, y, z);
        }
        
        // 加载StateSequenceController状态
        StateSequenceController sequenceController = FindObjectOfType<StateSequenceController>();
        if (sequenceController != null)
        {
            if (PlayerPrefs.HasKey(prefix + "SequenceCurrentIndex"))
            {
                sequenceController.currentIndex = PlayerPrefs.GetInt(prefix + "SequenceCurrentIndex");
            }
            
            if (PlayerPrefs.HasKey(prefix + "SequenceIsFinished"))
            {
                sequenceController.isSequenceFinished = PlayerPrefs.GetInt(prefix + "SequenceIsFinished") == 1;
            }
            
            // 加载状态序列
            if (PlayerPrefs.HasKey(prefix + "SequenceData"))
            {
                string sequenceString = PlayerPrefs.GetString(prefix + "SequenceData");
                if (!string.IsNullOrEmpty(sequenceString))
                {
                    string[] sequenceArray = sequenceString.Split(',');
                    List<int> newSequence = new List<int>();
                    foreach (string num in sequenceArray)
                    {
                        if (int.TryParse(num, out int numValue))
                        {
                            newSequence.Add(numValue);
                        }
                    }
                    if (newSequence.Count > 0)
                    {
                        sequenceController.numberSequence = newSequence;
                    }
                }
            }
            
            // 应用加载的状态
            sequenceController.ApplyStateByIndex(sequenceController.currentIndex);
        }
        
        Debug.Log("Game loaded successfully from slot " + saveSlot + "!");
    }
    
    // 删除指定槽位的存档
    public static void DeleteSave(int saveSlot)
    {
        // 验证存档槽位
        if (saveSlot < 1 || saveSlot > MAX_SAVE_SLOTS)
        {
            Debug.LogError("Invalid save slot! Must be between 1 and " + MAX_SAVE_SLOTS);
            return;
        }
        
        string prefix = "Save_" + saveSlot + "_";
        
        // 删除所有相关的存档键
        PlayerPrefs.DeleteKey(prefix + "PlayerStatus");
        PlayerPrefs.DeleteKey(prefix + "CurrentLevel");
        PlayerPrefs.DeleteKey(prefix + "CurrentScene");
        PlayerPrefs.DeleteKey(prefix + "ChoiceCount");
        PlayerPrefs.DeleteKey(prefix + "ChoiceHistory");
        PlayerPrefs.DeleteKey(prefix + "PlayerPositionX");
        PlayerPrefs.DeleteKey(prefix + "PlayerPositionY");
        PlayerPrefs.DeleteKey(prefix + "PlayerPositionZ");
        PlayerPrefs.DeleteKey(prefix + "SequenceCurrentIndex");
        PlayerPrefs.DeleteKey(prefix + "SequenceIsFinished");
        PlayerPrefs.DeleteKey(prefix + "SequenceData");
        PlayerPrefs.DeleteKey(prefix + "SaveTime");
        
        // 重置玩家的选择记录
        player.ResetChoices();
        
        PlayerPrefs.Save();
        
        Debug.Log("Save deleted successfully from slot " + saveSlot + "!");
    }
    
    // 检查指定槽位是否有存档
    public static bool HasSave(int saveSlot)
    {
        // 验证存档槽位
        if (saveSlot < 1 || saveSlot > MAX_SAVE_SLOTS)
        {
            return false;
        }
        
        return PlayerPrefs.HasKey("Save_" + saveSlot + "_SaveTime");
    }
    
    // 获取指定槽位的存档时间
    public static string GetSaveTime(int saveSlot)
    {
        // 验证存档槽位
        if (saveSlot < 1 || saveSlot > MAX_SAVE_SLOTS)
        {
            return "";
        }
        
        return PlayerPrefs.GetString("Save_" + saveSlot + "_SaveTime", "");
    }
    
    // 获取指定槽位的当前关卡
    public static int GetSaveLevel(int saveSlot)
    {
        // 验证存档槽位
        if (saveSlot < 1 || saveSlot > MAX_SAVE_SLOTS)
        {
            return 0;
        }
        
        return PlayerPrefs.GetInt("Save_" + saveSlot + "_CurrentLevel", 0);
    }
    
    // 向后兼容：支持旧的无槽位保存方式
    public static void SaveGame()
    {
        SaveGame(1); // 默认保存到槽位1
    }
    
    // 向后兼容：支持旧的无槽位加载方式
    public static void LoadGame()
    {
        LoadGame(1); // 默认加载槽位1
    }
    
    // 向后兼容：支持旧的无槽位删除方式
    public static void DeleteSave()
    {
        DeleteSave(1); // 默认删除槽位1
    }
}