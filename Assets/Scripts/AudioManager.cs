using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance = null;
    public static AudioManager Instance => _instance ? _instance : null;

    [Header("# BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    private AudioSource _bgmPlayer;
    private AudioHighPassFilter _bgmEffect;
    
    [Header("# SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    private AudioSource[] _sfxPlayers;
    private int _channelIndex;

    public enum Sfx
    {
        Dead,
        Hit,
        LevelUp = 3,
        Lose,
        Melee,
        Range = 7,
        Select,
        Win,
    }
    
    public void Awake()
    {
        if (_instance != null)
            Destroy(this.gameObject);
        else
            _instance = this;

        Init();
    }

    private void Init()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        _bgmPlayer = bgmObject.AddComponent<AudioSource>();
        _bgmPlayer.playOnAwake = false;
        _bgmPlayer.loop = true;
        _bgmPlayer.volume = bgmVolume;
        _bgmPlayer.clip = bgmClip;
        _bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();
        
        
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        _sfxPlayers = new AudioSource[channels];

        for (int i = 0; i < _sfxPlayers.Length; i++)
        {
            _sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            _sfxPlayers[i].playOnAwake = false;
            _sfxPlayers[i].bypassListenerEffects = true;
            _sfxPlayers[i].volume = sfxVolume;
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
            _bgmPlayer.Play();
        else
            _bgmPlayer.Stop();
    }
    
    public void EffectBgm(bool isPlay)
    {
        _bgmEffect.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int i = 0; i < _sfxPlayers.Length; i++)
        {
            int next = (i + _channelIndex) % _sfxPlayers.Length;
            
            if (_sfxPlayers[next].isPlaying)
                continue;

            int ran = 0;
            if (sfx == Sfx.Hit || sfx == Sfx.Melee)
                ran = Random.Range(0, 2);
            
            _sfxPlayers[next + ran].clip = sfxClips[(int)sfx];
            _sfxPlayers[next + ran].Play();
            break;
        }
    }
}
