using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthViewer : MonoBehaviour
{
    [SerializeField] Image heartImage;
    [SerializeField] TextMeshProUGUI healthCountText;

    public void UpdateHealthCount(int healthCount, Sprite healthImage)
    {
        healthCountText.text = healthCount.ToString();
        heartImage.sprite = healthImage;
    }
}
