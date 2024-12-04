// Created by LunarEclipse on 2024-9-9 5:33.

using UnityEngine;

namespace USEN.Games.Janken
{
    public static class JankenPreferences
    {
        public static int SelectedCharacter
        {
            get => PlayerPrefs.GetInt("Janken.SelectedCharacter", 0);
            set => PlayerPrefs.SetInt("Janken.SelectedCharacter", value);
        }
    }
}