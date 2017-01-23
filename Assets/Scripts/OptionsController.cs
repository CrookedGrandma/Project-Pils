using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class OptionsController : MonoBehaviour
{
    public Camera camera;
    public Scrollbar soundFXScrollbar;
    public Scrollbar musicScrollbar;
    public Scrollbar depthOfFieldScrollbar;
    public Scrollbar motionBlurScrollbar;
    public Scrollbar SSAOScrollbar;
    public Toggle noiseAndGrainToggle;
    public Toggle bloomToggle;
    public Dropdown antiAliasingDropdown;
    public LevelManager levelManager;

    private MusicManager musicManager;
    private SoundFXManager soundFXManager;
    private MotionBlur motionBlur;
    private DepthOfField depthOfField;
    private ScreenSpaceAmbientOcclusion SSAO;
    private NoiseAndGrain noiseAndGrain;
    private Bloom bloom;
    private Antialiasing antiAliasing;

    private void Start()
    {
        musicManager = GameObject.FindObjectOfType<MusicManager>();
        soundFXManager = GameObject.FindObjectOfType<SoundFXManager>();
        motionBlur = camera.GetComponent<MotionBlur>();
        depthOfField = camera.GetComponent<DepthOfField>();
        SSAO = camera.GetComponent<ScreenSpaceAmbientOcclusion>();
        noiseAndGrain = camera.GetComponent<NoiseAndGrain>();
        bloom = camera.GetComponent<Bloom>();
        antiAliasing = camera.GetComponent<Antialiasing>();

        musicScrollbar.value = PlayerPrefsManager.GetMusicVolume();
        soundFXScrollbar.value = PlayerPrefsManager.GetSoundFXVolume();
        motionBlurScrollbar.value = PlayerPrefsManager.GetMotionBlur();
        depthOfFieldScrollbar.value = PlayerPrefsManager.GetDepthOfField();
        SSAOScrollbar.value = PlayerPrefsManager.GetSSAO();
        noiseAndGrainToggle.isOn = PlayerPrefsManager.GetNoiseAndGrain();
        bloomToggle.isOn = PlayerPrefsManager.GetBloom();
        antiAliasingDropdown.value = PlayerPrefsManager.GetAntiAliasing();
    }

    private void Update()
    {
        musicManager.ChangeVolume(musicScrollbar.value);
        soundFXManager.ChangeVolume(soundFXScrollbar.value);
        motionBlur.blurAmount = motionBlurScrollbar.value * 0.2f;
        depthOfField.focalSize = depthOfFieldScrollbar.value * 0.5f + 1.5f;
        SSAO.m_Radius = SSAOScrollbar.value * 0.4f;
        noiseAndGrain.enabled = noiseAndGrainToggle.isOn;
        bloom.enabled = bloomToggle.isOn;
        antiAliasing.mode = (AAMode)(antiAliasingDropdown.value + 4);
    }

    public void SaveAndExit()
    {
        PlayerPrefsManager.SetMusicVolume(musicScrollbar.value);
        PlayerPrefsManager.SetSoundFXVolume(soundFXScrollbar.value);
        PlayerPrefsManager.SetMotionBlur(motionBlurScrollbar.value);
        PlayerPrefsManager.SetDepthOfField(depthOfFieldScrollbar.value);
        PlayerPrefsManager.SetSSAO(SSAOScrollbar.value);

        if (noiseAndGrainToggle.isOn)
        {
            PlayerPrefsManager.SetNoiseAndGrain(1);
        }
        else
        {
            PlayerPrefsManager.SetNoiseAndGrain(0);
        }

        if (bloomToggle.isOn)
        {
            PlayerPrefsManager.SetBloom(1);
        }
        else
        {
            PlayerPrefsManager.SetBloom(0);
        }

        PlayerPrefsManager.SetAntiAliasing(antiAliasingDropdown.value);
 
        levelManager.Loadlevel("MainMenu");
    }
}
