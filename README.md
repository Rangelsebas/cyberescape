# Cyber Escape - Frenetic FPS Parkour Game

## Overview
Cyber Escape is a high-octane, first-person parkour shooter developed in **Unity**. You play as Jack, a prisoner attempting to escape a high-tech facility. The project focuses on high-speed movement mechanics, fluid gunplay, and a robust architectural foundation using professional design patterns.

**[🎮 Play it on itch.io]([https://tu-link-de-itch-io](https://rangelsebas.itch.io/cyber-escape))** 

## Technical Highlights
This project was built with a focus on **Software Architecture** and **Physics-based Gameplay**:

* **Architectural Patterns:** * Implemented a **Singleton Pattern** for the `GameManager`, `PlayerSettings`, and `FPSArmsSettings` to ensure global access to core game states and configurations.
    * Modular logic separation: Dedicated scripts for `Inventory`, `PlayerController`, and `WeaponScript`.
* **Physics-Based Movement:** * The movement system is built entirely on **Rigidbodies**, allowing for realistic momentum conservation, wall-running, and crouch-sliding.
    * Dynamic FOV scaling: The camera's field of view adjusts in real-time based on the player's velocity to enhance the sense of speed.
* **Advanced Weapon & Animation Systems:**
    * **Procedural Sway & Bobbing:** Implemented custom Sin-wave algorithms for weapon bobbing and mouse-input-based sway for immersive gunplay.
    * **Contextual Reloading:** Weapon positioning is calculated dynamically using `Vector3.Lerp` based on the weapon type (Short, Medium, Long) during reload cycles.
* **New Input System:** Utilized the `UnityEngine.InputSystem` for rebindable controls and improved input handling for jump-buffering and dash mechanics.

## Key Features
- **Dynamic Parkour:** Smooth transitions between running, jumping, and sliding.
- **Combat System:** Multiple weapon classes (Pistol, Assault Rifle, Sniper, Banana, etc.) with unique behaviors.
- **Customizable Settings:** Real-time adjustment of sensitivity (including Slow-mo sensitivity), FOV, and UI elements via the `PlayerSettings` singleton.

## Tech Stack
- **Engine:** Unity (C#)
- **Physics:** Rigidbody-based movement
- **Input:** Unity New Input System
- **Patterns:** Singleton, State Machines, Observer-like UI updates.

## Project Structure
- `PlayerController.cs`: Core physics and movement logic.
- `PlayerSettings.cs`: Persistent global configurations using the Singleton pattern.
- `FPSArmsSettings.cs`: Procedural animation logic for weapon sway, bobbing, and positioning.
- `WeaponScript.cs`: Modular weapon behavior and reloading logic.

## How to Run
1. Clone this repository.
2. Open the project folder in **Unity Hub** (Recommended version: 2020.3+).
3. Open `Scenes/MainMenu.unity` and press Play.

---
*Developed by Sebastián Rangel as a project in advanced game mechanics and software design.*
