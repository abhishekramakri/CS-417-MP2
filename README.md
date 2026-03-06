# CS 417 MP2: Minimal Sim

## Overview
A VR idle/incremental farming sim. The player accumulates two resources, Sunlight and Money, passively over time and through direct interaction. Resources can be spent to deploy generators, purchase power-ups, and unlock a second resource area. The game saves progress between sessions and picks up where you left off.

**Theme:** Low-poly nature farm

**Scene:** `MP2_MinimalSim/Assets/mp2scene.unity`

---

## How to Play
- Approach any glowing interactable object and use the **Grip button** to interact
- Sunlight and Money accumulate automatically over time
- Click the **sunflower** to earn Sunlight manually, or the **green flower** to earn Money manually
- Buy **generators** to permanently increase your passive earn rate
- Buy **power-ups** to multiply all rates
- Spend **50 Sunlight** at the gate to unlock the Money resource area
- Watch for **achievement trees** that grow as your Sunlight increases

**Controls (Quest 3)**
- Left Thumbstick: Move
- Right Thumbstick: Turn
- Grip Button: Interact with XR interactables

---

## Implemented Features

### Required Features

**Ramping Resources (3pts):** Two resource counters, Sunlight and Money, are Euler integrated over timesteps using `sunlightRate` and `moneyRate` in `GameManager`. Both are displayed on a world-space canvas that updates every frame.

**Planting Generators (3pts):** Two generator objects (SunlightGenerator and MoneyGenerator) are XR interactables. Gripping them deploys a generator that permanently adds to the corresponding resource's passive rate. Each costs resources to deploy.

**Purchasing Power-ups (4pts):** A PowerupStand XR interactable costs both Sunlight and Money and multiplies all generation rates by 1.5x. The stand disappears after purchase.

**Unlockable UI (4pts):** A gate object costs 50 Sunlight to unlock. On unlock, the gate wall disappears and the Money resource display area becomes visible and active. The trigger object and its sign also disappear.

---

### Optional Features

**Clicker (1pt):** Gripping the sunflower manually adds Sunlight. Gripping the green flower manually adds Money. Both are wired to XR Simple Interactable Select Entered events.

**Exponential Costs (1pt):** Each new generator costs 1.5x more than the previous one. Formula: `baseCost * 1.5^deployedCount`.

**Cooldown Timers (1pt):** Each generator and power-up has a `CooldownTimer` component that enforces a wait between interactions. The countdown is displayed directly on the interactable's sign (e.g. "Ready in 3s" → "Ready!").

**Achievement Trophies (2pts):** Three trees spawn at preset world positions when Sunlight reaches 100, 250, and 500. Handled by `AchievementTrophies.cs` attached to an empty manager object.

**Inter-session Saves (1pt):** Resources, rates, generator deploy counts, power-up purchase count, and unlock state are all saved to PlayerPrefs on quit and restored on load.

**Idle Progress (1pt):** On load, the game calculates how long the player was away using a saved timestamp and adds the resources they would have earned during that time.

---

## Project Structure
```
MP2_MinimalSim/Assets/
  mp2scene.unity                  <- Main game scene
  GameManager.cs                  <- Euler integration, save/load, idle progress
  ResourceUI.cs                   <- Updates Sunlight and Money text each frame
  GeneratorDeployer.cs            <- XR interactable: deploy generators with exponential costs
  PowerupPurchaser.cs             <- XR interactable: buy rate multiplier power-ups
  UnlockableUI.cs                 <- XR interactable: unlock second resource area
  InteractableSign.cs             <- Dynamic world-space sign showing cost, effect, cooldown
  CooldownTimer.cs                <- Reusable cooldown enforcement component
  AchievementTrophies.cs          <- Spawns trophy trees at resource milestones
  SunflowerClickVR.cs             <- Clicker: adds Sunlight on grip
  GreenFlowerClickVR.cs           <- Clicker: adds Money on grip
```

---

## Scripts

**GameManager.cs**
Singleton. Euler integrates Sunlight and Money each frame. Saves all resource and rate values to PlayerPrefs on quit. On load, restores saved values and applies idle progress based on elapsed time since last session.

**GeneratorDeployer.cs**
Attached to each generator object alongside an XR Simple Interactable. On grip, deducts the current cost from the chosen resource and permanently boosts the corresponding rate. Cost scales exponentially with each deployment. Supports cooldown via `CooldownTimer`.

**PowerupPurchaser.cs**
Attached to the PowerupStand. On grip, deducts both Sunlight and Money and multiplies both `sunlightRate` and `moneyRate` by the configured multiplier. Hides itself and its sign after purchase.

**UnlockableUI.cs**
Attached to the gate trigger object. On grip, checks if the player has enough Sunlight, deducts the cost, activates the locked content area, and hides the gate wall, trigger object, and sign.

**InteractableSign.cs**
Reads from a `GeneratorDeployer` or `PowerupPurchaser` each frame and displays current cost, effect, and cooldown status on a world-space TextMeshPro text object.

**CooldownTimer.cs**
Tracks time remaining after an interaction. Exposes `IsReady` and `TimeRemaining` for other scripts to read. Does not display anything itself; the sign reads from it.

**AchievementTrophies.cs**
Watches a resource counter each frame. When it crosses a configured threshold, instantiates a prefab at a set world position. Each trophy only spawns once.

---

## Build Instructions
1. Open the project in Unity 6
2. Open `MP2_MinimalSim/Assets/mp2scene.unity`
3. File → Build Settings → Android → Add Open Scenes → Build
4. Sideload the APK onto Quest 3
