// Created by LunarEclipse on 2024-6-21 1:53.

using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Luna;
using Luna.Extensions;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using USEN.Games.Common;
using USEN.Games.Roulette;

namespace USEN.Games.Janken
{
    public class JankenHomeView : Widget
    {
        public Button startButton;
        public Button settingsButton;
        public BottomPanel bottomPanel;
        
        public AssetReferenceGameObject spineCharacter;
        public JankenCharacterController characterController;
        
        private List<JankenCharacter> _categories;

        private void Awake()
        {
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        }

        private async void Start()
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            bottomPanel.exitButton.onClick.AddListener(OnExitButtonClicked);
            
            // Audio volume
            BgmManager.Volume = JankenPreferences.BgmVolume;
            SFXManager.Volume = JankenPreferences.SfxVolume;

            // Show loading indicator before necessary assets are loaded
            await UniTask.Yield(PlayerLoopTiming.PreLateUpdate);
            Navigator.ShowModal<CircularLoadingIndicator>();
            
            // Load audios
            var clip = await R.Audios.BgmJanken.Load();
            BgmManager.Play(clip);
            await R.Audios.BgmJankenFinal.Load();
            await Assets.Load("USEN.Games.Common", "Audio");
            
            // Load spine assets.
            await JankenCharacters.DefaultAsset.Load();
            await spineCharacter.LoadAssetAsync();
            
            Navigator.PopToRoot();
        }

        private void OnEnable()
        {
            // Network request
            RouletteManager.Instance.Sync();
            API.GetRandomSetting().ContinueWith(task => {
                RoulettePreferences.DisplayMode = (RouletteDisplayMode) task.Result.random;
            }, TaskScheduler.FromCurrentSynchronizationContext());
            
            // Bottom panel
            bottomPanel.onBlueButtonClicked += OnBlueButtonClicked;
        }

        private void OnDisable()
        {
            bottomPanel.onBlueButtonClicked -= OnBlueButtonClicked;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("Cancel")) 
                OnExitButtonClicked();
        }

        public void OnStartButtonClicked()
        { 
            Navigator.Push<JankenGameView>(view => {
               view.characterController = characterController; 
            });
        }
        
        public void OnSettingsButtonClicked()
        {
            Navigator.Push<JankenSettingsView>();
        }
        
        private void OnExitButtonClicked()
        {
            Application.Quit();
        }
        
        private void OnBlueButtonClicked()
        {
            Navigator.Push<JankenCharacterView>();
        }
    }
}