using UnityEngine;

using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingManager : Singleton<PostProcessingManager>
{
    [SerializeField] Volume volume;
    [SerializeField] Transform darknessTransform;

    private ColorAdjustments colorAdjustments;
    const float minValue = -100;
    const float maxValue = 60;

    private void Awake()
    {
        volume.profile.TryGet(out colorAdjustments);
    }

    private void Update()
    {
        var playerPos = PlayerController.Instance.transform.position;
        var distance = Vector2.Distance(playerPos, darknessTransform.position);
        var value = distance * 100 / 155 * (-minValue / maxValue) - 100;
       
        ChangeSaturation(value);
    }

    private void ChangeSaturation(float saturation)
    {
        ClampedFloatParameter newSaturation = new ClampedFloatParameter(saturation, minValue, maxValue);
        colorAdjustments.saturation.value = newSaturation.value;
    }
}
