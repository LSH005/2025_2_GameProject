using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance { get; private set; }

    [Header("Achievement Settings")]
    public List<AchievementData> allAchievement = new List<AchievementData>();

    [Header("UI References")]
    public GameObject achievementPopUpPrefab;
    public Transform popupParent;
    public GameObject achievementPanel;
    public Transform achievementListContent;
    public GameObject achievementSlotPrefab;

    private Dictionary<AchievementType, int> progressData = new Dictionary<AchievementType, int>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetAllAchievements();
        foreach (AchievementType type in System.Enum.GetValues(typeof(AchievementType)))
        {
            progressData[type] = 0;
        }
        LoadAchievements();
        UpdateAchievementUI();
    }

    public float GetProgress(AchievementData achievement)
    {
        if (achievement.isUnlocked) return 1f;
        int current = progressData.ContainsKey(achievement.achievementType) ? progressData[achievement.achievementType] : 0;
        // »ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?»ïÇ×?
        return Mathf.Min((float)current / achievement.requiredAmount, 1f);
    }

    public void UpdateAchievementUI()
    {
        if (achievementListContent == null || achievementPopUpPrefab == null) return;

        foreach (Transform child in achievementListContent)
        {
            Destroy(child.gameObject);
        }

        foreach (AchievementData achievement in allAchievement)
        {
            GameObject slot = Instantiate(achievementSlotPrefab, achievementListContent);
            AchievementSlot slotScript = slot.GetComponent<AchievementSlot>();

            if (slotScript != null)
            {
                slotScript.SetAchievement(achievement, GetProgress(achievement));
            }

        }
    }

    void SaveAchievement()
    {
        foreach (var kyp in progressData)
        {
            PlayerPrefs.SetInt("Achievement_" + kyp.Key, kyp.Value);
        }

        foreach (AchievementData achievement in allAchievement)
        {
            PlayerPrefs.SetInt("Unlocked_" + achievement.name, achievement.isUnlocked ? 1 : 0);
            // À¸¾Ç »ïÇ×¿¬»ê!!
        }

        PlayerPrefs.Save();
    }

    void LoadAchievements()
    {
        foreach (AchievementType type in System.Enum.GetValues(typeof(AchievementType)))
        {
            progressData[type] = PlayerPrefs.GetInt("Achievement_" + type, 0);
        }

        foreach (AchievementData achievement in allAchievement)
        {
            achievement.isUnlocked = PlayerPrefs.GetInt("Ublocked_" + achievement.name, 0) == 1;
        }
    }

    public void ResetAllAchievements()
    {
        foreach (AchievementType type in System.Enum.GetValues(typeof(AchievementType)))
        {
            progressData[type] = 0;
            PlayerPrefs.DeleteKey("Achievement_" + type);
        }

        foreach (AchievementData achievement in allAchievement)
        {
            achievement.isUnlocked = false;
            PlayerPrefs.DeleteKey("Unlocked_" + achievement.name);
        }

        PlayerPrefs.Save();
        UpdateAchievementUI();
    }

    void ShowAchievementPopup(AchievementData achievement)
    {
        if (achievementPopUpPrefab != null && popupParent != null)
        {
            GameObject popup = Instantiate(achievementPopUpPrefab, popupParent);

            Text titleText = popup.transform.Find("Title")?.GetComponent<Text>();
            Text descText = popup.transform.Find("Description")?.GetComponent<Text>();

            if (titleText != null) titleText.text = "¾÷Àû ´Þ¼º";
            if (descText != null) descText.text = achievement.achievementName;

            Destroy(popup, 3.0f);
        }
    }

    void UnlockAchievement(AchievementData achievement)
    {
        achievement.isUnlocked = true;
        ShowAchievementPopup(achievement);
        UpdateAchievementUI();
    }

    public void UpdateProgress(AchievementType type, int amount)
    {
        progressData[type] += amount;

        foreach (AchievementData achievement in allAchievement)
        {
            if (achievement.achievementType == type && !achievement.isUnlocked)
            {
                if (progressData[type] >= achievement.requiredAmount)
                {
                    UnlockAchievement(achievement);
                }
            }
        }
    }
}
