using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AudioTypeEnum {
    ShotgunFire, MachineGunFire, AcidBallSplat, LightningImpact, CrystalBulletImpact, PlayerDamaged, PlayerRespawn
}

[CreateAssetMenu(menuName =("SO Audio/Create SO Aduio"))]
public class SOAudio : ScriptableObject {

    [System.Serializable]
    private struct AudioStruct {

        public AudioTypeEnum type;
        public AudioClip[] audioClips;
        public VolumeRange volume;
        public PitchRange pitch;
    }
    [System.Serializable]
    private struct VolumeRange {

        [Range(0.0f, 1.0f)]
        public float min;
        [Range(0.0f, 1.0f)]
        public float max;
    }
    [System.Serializable]
    private struct PitchRange {

        [Range(0.0f, 2.0f)]
        public float min;
        [Range(0.0f, 2.0f)]
        public float max;
    }

    [SerializeField]
    private List<AudioStruct> _audioClips;

    //private Dictionary<AudioTypeEnum, AudioStruct> _clipTable = new Dictionary<AudioTypeEnum, AudioStruct>();

    private void OnEnable() {
        
        //foreach(AudioStruct source in _audioClips) {
        //    _clipTable.Add(source.type, source);
        //}
    }

    public void Play(AudioSource source, AudioTypeEnum type) {

        if (_audioClips[(int)type].audioClips.Length > 0) {
            AudioStruct instance = _audioClips[(int)type];
            int length = _audioClips[(int)type].audioClips.Length;

            // Pick a random clip from the clips and keep a reference of it.
            source.clip = instance.audioClips[Random.Range(0, length)];
            source.volume = Random.Range(instance.volume.min, instance.volume.max);
            source.pitch = Random.Range(instance.pitch.min, instance.pitch.max);
            source.Play();
        }
        else {
            Debug.Log("No audio clips available.");
        }

        //if (_clipTable[type].audioClips.Length > 0) {

        //    AudioStruct instance = _clipTable[type];
        //    int length = _clipTable[type].audioClips.Length;

        //    // Pick a random clip from the clips and keep a reference of it.
        //    source.clip = instance.audioClips[Random.Range(0, length - 1)];
        //    source.volume = Random.Range(instance.volume.min, instance.volume.max);
        //    source.pitch = Random.Range(instance.pitch.min, instance.pitch.max);
        //    source.Play();
        //}
        //else {
        //    Debug.Log("No audio clips available.");
        //}
    }
}
