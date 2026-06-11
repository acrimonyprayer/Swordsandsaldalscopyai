🛡️ 2D Gladiator Combat (Turn-Based)
A turn-based Gladiator Combat prototype developed with Unity. This project features energy management, distance-based combat, and a basic AI system.
🚀 Features
•	Turn-Based System: Combat flows through distinct phases for the Player and the Enemy.
•	Energy Mechanics: Attacking consumes energy (-15 Energy), while defending restores it (+25 Energy).
•	Distance Control: Combatants must be within range (distance <= 3.5) to land an attack.
•	Basic AI: The enemy gladiator decides whether to approach, attack, or defend/heal based on distance and health.
•	Dynamic UI: Real-time updates for Health Points (HP), Energy, and an Action Log to follow the battle.
•	Restart Functionality: Quickly reload the scene after a victory or defeat.
🎮 How to Play
1.	Attack: Deal damage to the enemy if they are in range. Requires energy.
2.	Defend: Halve the damage taken in the next turn and recover energy.
3.	Move Left / Right: Position yourself to enter attack range or escape the enemy.
🛠️ Setup & Requirements
•	Unity Version: Unity 6 or later is recommended.
•	Packages: TextMeshPro (required for UI elements).
Steps:
1.	Open the project in Unity.
2.	Ensure that the buttons and text elements on the Canvas are linked to the references in the PlayerGladiator script.
3.	Ensure both Player and Enemy objects have a Sprite Renderer component.
4.	Press the Play button to start.
📁 File Structure
•	PlayerGladiator.cs: Manages player controls, UI logic, and core game state.
•	EnemyGladiator.cs: Handles enemy AI behavior and stats.
📜 License
This project is for educational purposes. Feel free to modify and expand upon it!
________________________________________
Note: All variable and function names in the scripts follow software development standards and are written in English.

