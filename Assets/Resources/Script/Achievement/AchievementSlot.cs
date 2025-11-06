using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class AchievementSlot : MonoBehaviour
{

    [Header("UI References")]
    public Image iconImage;
    public Text nameText;
    public Text descriptionText;
    public Text progressText;
    public Slider ProgressSlider;

    public void SetAchievement(AchievementData achievement, float progress)
    {
        if (nameText != null) nameText.text = achievement.achievementName;

        if (descriptionText != null) descriptionText.text = achievement.description;

        if (iconImage != null && achievement.icon != null) iconImage.sprite = achievement.icon;

        if (ProgressSlider != null) ProgressSlider.value = achievement.isUnlocked ? 1f : progress;
        // »ïÇ×¿¬»ê »ç¶ûÇØ¿ä!!!!!

        if (progressText != null)
        {
            if (achievement.isUnlocked)
            {
                progressText.text = "¿Ï·á";
            }
            else
            {
                int current = Mathf.FloorToInt(progress * achievement.requiredAmount);
                progressText.text = current + " / " + achievement.requiredAmount;
            }
        }

    }
}
