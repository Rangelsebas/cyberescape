# Cyber Escape - Frenetic FPS Parkour Game

## Overview
Cyber Escape is a high-octane, first-person parkour shooter developed in **Unity 2019.4 LTS**. You play as Jack, a prisoner attempting to escape a high-tech facility. The project focuses on high-speed movement mechanics, fluid gunplay, and a robust architectural foundation using professional design patterns.

**[🎮 Play it on itch.io]([https://tu-link-de-itch-io](https://rangelsebas.itch.io/cyber-escape))** 

## Technical Highlights
This project was built with a focus on **Software Architecture** and **Physics-based Gameplay**:

* **Architectural Patterns:**
    * Implemented a **Singleton Pattern** for core systems: `GameManager`, `PlayerSettings`, and `FPSArmsSettings`. This ensures global access to game state and user configurations while maintaining a decoupled structure.
    * Modular logic separation: Dedicated scripts for `Inventory`, `PlayerController`, and `WeaponScript`.
* **Physics-Based Movement & Control:**
    * The movement system is built entirely on **Rigidbodies**, allowing for realistic momentum conservation, wall-running, and crouch-sliding.
    * Developed custom camera logic: Dynamic FOV scaling that adjusts in real-time based on player velocity to enhance the sense of speed.
    * Configured a comprehensive **Input Manager** supporting both Keyboard/Mouse and Joystick/Gamepad for a cross-platform feel.
* **Procedural Animation & Visuals:**
    * **Custom Sway & Bobbing:** Implemented procedural Sin-wave algorithms for weapon bobbing and mouse-input-based sway for immersive gunplay.
    * **Dynamic Weapon Positioning:** Utilized `Vector3.Lerp` for reloading animations, with specific coordinate offsets for different weapon classes (Short, Medium, Long).

## Key Features
- **Dynamic Parkour:** Smooth transitions between running, jumping, and sliding based on physical forces.
- **Combat System:** Multiple weapon classes (Pistol, Assault Rifle, Sniper, etc.) with unique recoil and reload behaviors.
- **Player Customization:** A centralized `PlayerSettings` system allowing real-time adjustment of sensitivity, field of view, and UI feedback.

## Tech Stack
- **Engine:** Unity 2019.4.19f1 (C#)
- **Physics:** Rigidbody-based kinematics.
- **Input:** Legacy Input Manager (configured for high-precision Mouse X/Y and Gamepad support).
- **Patterns:** Singleton, State Machines (for animations/IA), and Object-Oriented Design.

## How to Run
1. Clone this repository.
2. Open the project folder in **Unity Hub** using version **2019.4.19f1**.
3. Navigate to `Scenes/MainMenu.unity` and press Play.

---
*Developed by Sebastián Rangel as a deep dive into advanced movement physics and software architecture.*
