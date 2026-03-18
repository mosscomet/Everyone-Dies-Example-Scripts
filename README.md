# Everyone Dies | 2D Wave Survival Scripts

### Project Overview
This 2D survival game was developed during the first semester of a game design and computer science course. The project focused on building core gameplay mechanics and gaining proficiency in C# and Unity. While the second semester of the course involved collaborative development and version control training, this repository highlights the foundation established in the first half of the year.

### Technical Implementation

**Data Management**
* Implemented a player inventory using a `Dictionary<Item, int>` to track item types and stack counts.
* Created a stat recalculation system that resets and applies modifiers to player attributes, ensuring accurate math when items are added or removed.
* Developed a wave-based scaling system that adjusts enemy health, speed, and damage using linear and exponential formulas.

**Combat and AI Logic**
* Wrote a targeting algorithm that performs a linear search to find and track the nearest enemy transform.
* Programmed homing projectile behavior using vector math and angle interpolation to follow moving targets.
* Built a shooting system that handles standard fire and cooldown-based special abilities with varying projectile patterns.
* Used physics-based collision handling to manage damage and projectile piercing mechanics.

**Development Workflow**
* Utilized GitHub Desktop for version control, handling commits and repository management during the collaborative phase of the project.
* Managed object communication between the player controller, shooting systems, and inventory managers to maintain a modular codebase.

### Core Scripts
* **InventoryManager.cs**: Handles item data and attribute modification logic.
* **Enemy.cs**: Manages enemy movement, stats, scaling, and loot drop probability.
* **player.cs**: Processes movement input and synchronizes stats with the combat system.
* **homingProjectile.cs**: Controls target finding and guided movement logic.
* **Shooting.cs**: Manages weapon firing states and ability cooldowns.
* **projectile.cs**: Handles standard projectiles and uses `Physics2D` systems to manage multi-target pierce mechanics.
