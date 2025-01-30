using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;

namespace HUD.Menu
{
    public class InGameMenuController : MonoBehaviour
    {
        public Slider musicSlider;
        public Slider sfxSlider;

        public AudioSource[] musicSources;


        private float _currentMusicVolumeModifier = 1f;
        private List<KeyValuePair<AudioSource, float>> _musicSourcesWithInitialVolume;

        private float _currentSfxVolumeModifier = 1f;
        private List<KeyValuePair<AudioSource, float>> _sfxSourcesWithInitialVolume;

        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();

            var allAudioSources =
                FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            _sfxSourcesWithInitialVolume = allAudioSources.Except(musicSources)
                .Select(source => new KeyValuePair<AudioSource, float>(source, source.volume))
                .ToList();

            _musicSourcesWithInitialVolume = musicSources
                .Select(source => new KeyValuePair<AudioSource, float>(source, source.volume))
                .ToList();
        }

        private void OnEnable()
        {
            EventManager.InGameMenuEvent.OnPressBackEvent += ToggleMainMenu;
            EventManager.DemonChargeEvent.OnSummon += RegisterAudioSource;
        }

        private void OnDisable()
        {
            EventManager.InGameMenuEvent.OnPressBackEvent -= ToggleMainMenu;
            EventManager.DemonChargeEvent.OnSummon -= RegisterAudioSource;
        }

        private void RegisterAudioSource(Component source, AudioSource[] audioSources)
        {
            foreach (var audioSource in audioSources)
            {
                _sfxSourcesWithInitialVolume.Add(
                    new KeyValuePair<AudioSource, float>(audioSource, audioSource.volume)
                );
                audioSource.volume *= _currentSfxVolumeModifier;
            }
        }

        private void ToggleMainMenu(Component arg0)
        {
            _canvas.enabled = !_canvas.enabled;
        }

        public void RestartGame()
        {
            SceneManager.LoadScene("TerrainTest");
        }


        public void OnSfxSliderChanged(float value)
        {
            _currentSfxVolumeModifier = value;

            foreach (var pair in _sfxSourcesWithInitialVolume)
            {
                if (pair.Key)
                {
                    pair.Key.volume = pair.Value * _currentSfxVolumeModifier;
                }
            }
        }

        public void OnMusicSliderChanged(float value)
        {
            _currentMusicVolumeModifier = value;

            foreach (var pair in _musicSourcesWithInitialVolume)
            {
                if (pair.Key)
                {
                    pair.Key.volume = pair.Value * _currentMusicVolumeModifier;
                }
            }
        }
    }
}