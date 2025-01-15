// Created by LunarEclipse on 2024-6-21 1:53.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using Luna;
using Luna.UI;
using Luna.UI.Navigation;
using Modules.UI.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
        
        public TextAsset categoriesJson;
        
        private List<JankenCharacter> _categories;
        
        private void Start()
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            bottomPanel.exitButton.onClick.AddListener(OnExitButtonClicked);
            
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
            
            // Audio volume
            BgmManager.Volume = JankenPreferences.BgmVolume;
            SFXManager.Volume = JankenPreferences.SfxVolume;
            
            BgmManager.Play(R.Audios.BgmJanken);
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
            Navigator.Push<JankenGameView>();
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