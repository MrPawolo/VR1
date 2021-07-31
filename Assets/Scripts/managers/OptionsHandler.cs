using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Audio;
public class OptionsHandler : MonoBehaviour
{
    public ScriptableRendererFeature ao;
    public RenderPipelineAsset[] qualitySettings;

    const string _AO = "AO";
    const string _QUALITY_LEVEL = "QUALITY_LEVEL";
    private void OnEnable()
    {
        LoadSettings();
    }
    void LoadSettings()
    {
        bool aoState = PlayerPrefsHandler.GetBool(_AO);
        if (aoState)
        {
            EnableAO();
        }
        else
        {
            DisableAO();
        }
        ChangeQualityLevel(PlayerPrefsHandler.GetInt(_QUALITY_LEVEL));

    }

    
    public void EnableAO()
    {
        ao.SetActive(true);
        PlayerPrefsHandler.SetBool(_AO, true);
    }
    public void DisableAO()
    {
        ao.SetActive(false);
        PlayerPrefsHandler.SetBool(_AO, false);
    }
    public void ChangeQualityLevel(int i)
    {
        QualitySettings.SetQualityLevel(i);
        QualitySettings.renderPipeline = qualitySettings[i];
        PlayerPrefsHandler.SetInt(_QUALITY_LEVEL, i);
    }


}
