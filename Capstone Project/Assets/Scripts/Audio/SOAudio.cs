using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName =("SO Audio/Create SO Aduio"))]
public class SOAudio : ScriptableObject {

    [SerializeField]
    private AudioClip _audioClip;
    [SerializeField]
    private VolumeRange _volume;
    [SerializeField]
    private PitchRange _pitch;

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

    public void Play(AudioSource source) {

        if (_audioClip != null) {

            source.clip = _audioClip;
            source.volume = Random.Range(_volume.min, _volume.max);
            source.pitch = Random.Range(_pitch.min, _pitch.max);
            source.Play();
        }
        else {
            Debug.Log("No audio clips available.");
        }

        //if (_audioClips[(int)type].audioClips.Length > 0) {
        //    AudioStruct instance = _audioClips[(int)type];
        //    int length = _audioClips[(int)type].audioClips.Length;

        //    // Pick a random clip from the clips and keep a reference of it.
        //    source.clip = instance.audioClips[Random.Range(0, length)];
        //    source.volume = Random.Range(instance.volume.min, instance.volume.max);
        //    source.pitch = Random.Range(instance.pitch.min, instance.pitch.max);
        //    source.Play();
        //}
        //else {
        //    Debug.Log("No audio clips available.");
        //}

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
