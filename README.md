# 🎮 Final Wave

A first-person VR wave-based shooter built using Unity and XR Interaction Toolkit, designed for Meta Quest 2 with a focus on responsive combat and immersive world-space UI.

---

## 🧠 Overview

**Final Wave** is a survival VR FPS where the player fights against increasingly difficult waves of enemies.

The core loop is simple and engaging:

* Survive waves of enemies
* Eliminate targets efficiently
* Achieve the highest possible score

The game is built with performance and clarity in mind, making it suitable for standalone VR devices like the Meta Quest 2.

---

## 🎯 Gameplay

* Enemies spawn in waves with increasing difficulty

* Each wave introduces more enemies and improved accuracy

* Player uses a gun system featuring:

  * Shooting
  * Reloading
  * Ammo management
  * Recoil
  * Headshots

* Enemies:

  * Navigate toward the player
  * Track and face the player
  * Shoot with an accuracy + spread system (not perfectly accurate)

* Player:

  * Has a health system
  * Receives audio + visual feedback when damaged
  * Dies when health reaches zero

---

## 🧩 Core Systems

### 🔫 Combat System

* Raycast-based shooting
* Headshot detection
* Hit feedback system

### 🤖 Enemy AI

* NavMesh-based movement
* Player tracking
* Shooting with probabilistic accuracy

### 🌊 Wave System

* Progressive scaling (1, 2, 3...)
* Clean wave transitions
* Stable spawning system

### ❤️ Player System

* Health + UI feedback
* Damage effects
* Clean death handling

---

## 🏆 Scoring & Progression

* Score increases by defeating enemies
* Each player profile stores its own **high score**
* Scores are saved locally using PlayerPrefs
* Only higher scores overwrite previous ones

---

## 👤 Player Profiles

* Create new players
* Select existing players
* Each player has:

  * A unique identity
  * A separate high score

---

## 📊 Scoreboard

* Displays all players
* Sorted by highest score
* Updates dynamically

---

## 🧭 UI & VR Experience

* UI is implemented in **world space** for immersion
* Panels appear naturally in front of the player
* No screen-locked UI

Includes:

* Lobby system
* Player selection
* Scoreboard
* Game over panel (spawns in front of player)

---

## ⚙️ Tech Stack

* Unity (XR Interaction Toolkit)
* C#
* Meta Quest 2

---

## 🚀 Features Summary

* Full playable gameplay loop
* VR-optimized UI
* Enemy AI with scaling difficulty
* Player profile system
* Persistent high scores
* Scoreboard system
* Clean game over flow

---

## 🧪 Current State

The project is fully playable with a complete loop:

> Lobby → Player Selection → Gameplay → Game Over → Score Saving → Replay

---

## 📌 Notes

* Built with simplicity and performance in mind
* Optimized for standalone VR
* Designed as a strong foundation for further expansion

---

## 🔮 Future Improvements

* Combat polish (effects, recoil feel)
* UI animations
* Additional enemy types
* Weapon variety
* Audio and visual enhancements

---

## 📄 License

This project is licensed under the MIT License. See the `LICENSE` file for details.

---

## 👨‍💻 Credits

Developed by **J3DI**
