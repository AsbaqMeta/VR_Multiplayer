# VR_Multiplayer

# **Multiplayer VR - Documentation**

## **Overview**
This project is a multiplayer VR game that utilizes **Photon Pun** for synchronization across players in a network. The core features include:

- A **Timer** that counts down the time for a VR event.
- **Score calculation** based on the completion time.
- **Player movement** and **hand animation synchronization** in a networked VR environment.

### **Key Components**:
- **EventTimer**: Manages the countdown timer for the event.
- **ScoreSystem**: Calculates and updates the player's score based on the timer.
- **NetworkPlayer**: Handles player movement and hand animations in VR, synchronized across the network.
- **NetworkPlayerSpawner**: Spawns the player when they join the room and handles the destruction of the player when they leave.
- **TransformLerp**: Smoothly moves an object to a target position over time with interpolation.
  
---

## **Setting Up the Project**
Follow these steps to set up and run the project locally:

### **1. Install Unity & Photon**
1. **Unity**: Download and install Unity (preferably version 2020 or higher).
2. **Photon Pun**: Go to the Unity Asset Store, search for **Photon PUN 2 - Free** and install it.

### **2. Set Up Photon Network**
1. Create a **Photon account** by visiting [Photon Engine](https://www.photonengine.com/).
2. Create a new **Photon App ID**.
3. In Unity, go to `Window -> Photon Unity Networking -> PUN Wizard`.
4. Enter your **Photon App ID** to connect Unity with Photon.

### **3. Import and Setup Scripts**
1. Create a new Unity scene and save it.
2. Import the **Photon PUN 2** package into your Unity project.
3. Create empty GameObjects in the scene and attach the provided scripts (e.g., `NetworkPlayer`, `EventTimer`, etc.) to these GameObjects.
4. Make sure the **PhotonView** component is added to objects that need network synchronization (like `NetworkPlayer`).

### **4. Setup XR for VR (if applicable)**
1. If working with VR, install the **XR Interaction Toolkit** via the Unity Package Manager.
2. Set up the **XR settings** for your specific VR hardware.
3. Create XR **camera rigs** for head and hand tracking in your scene.

### **5. Scene Setup**
- **Lobby Scene**: This scene serves as the entry point where players will connect to the Photon server and select a room to join. 
- **Room1 and Room2 Scenes**: These are the rooms where the event will occur. Players will transition to one of these rooms after joining the lobby.
  
---

## **How the System Works**

### **1. Player Synchronization**
The system uses **Photon PUN** to synchronize player actions across the network. The key aspects include:

#### **Player Movement and VR Syncing**:
- The `NetworkPlayer` script manages the player's movement (head, left hand, right hand).
- The local player’s movement is handled directly via the XR system, while other players' movements are smoothly interpolated using **lerping** to prevent jitter.
  
#### **Hand Animation Sync**:
- Hand animations (e.g., trigger, grip) are synchronized using Photon, allowing hand positions and animations to reflect on all connected players' devices.
  
#### **Networked Movement**:
- The `OnPhotonSerializeView` method syncs the player’s head and hand positions/rotations across the network.

---

### **2. Event Timer**
The `EventTimer` component controls the countdown for an event. It uses the following logic:
- The timer is synchronized across the network using **Photon RPC**.
- The timer starts with the `StartTimer` method and continues counting down.
- The **UI** is updated each second to reflect the current time.
- When the timer ends, the system logs a message indicating that the event has completed.

---

### **3. Score Calculation**
The `ScoreSystem` component calculates the score based on the time taken for the event. It uses the following logic:
- The score is calculated as a percentage of the maximum time (`maxTime`).
- The score is then synchronized across the network using **Photon RPC** to ensure all players see the same score at the same time.
- The score is displayed in the UI.

---

### **4. Player Spawning & Cleanup**
- When a player joins the room, the **`NetworkPlayerSpawner`** script instantiates a networked player prefab using **PhotonNetwork.Instantiate**.
- Upon leaving, the player prefab is destroyed to clean up the scene.

---

### **5. Scene Flow**

There are three primary scenes:

- **Lobby Scene**:
  - Players connect to Photon and can either join an existing room or create a new room.
  - Players can interact with the lobby interface to select a room.
  - Once a player joins a room, they are transitioned to either `Room1` or `Room2` based on available slots.

- **Room1** & **Room2 Scenes**:
  - These rooms serve as the event areas where the countdown timer starts, and the score is calculated based on event completion time.
  - Players spawn into the room and participate in the event with timers, animations, and score calculation.

**Room Transition Logic**:
- Players begin in the **Lobby Scene** where they can create or join a room.
- Upon successfully joining a room (via **PhotonNetwork.JoinRoom()**), the player is automatically transitioned to either **Room1** or **Room2**.
  
### **Scene Transitions:**
1. **Lobby Scene**:
   - A UI button allows the player to connect to the server and join a room.
   - The player is then sent to either **Room1** or **Room2**.
   
2. **Room1 and Room2 Scenes**:
   - The event timer starts once all players are in the room.
   - Players interact with the timer and score systems until the event is completed.
   - When the event ends, players may either leave or restart the process, moving back to the lobby if desired.

---

## **How to Start the Game**

1. **Start Unity**: Open the project in Unity.
2. **Build Settings**: Make sure that the **Build Settings** are configured for your platform (e.g., PC, Android, etc.).
3. **Photon Setup**: Ensure that Photon is properly set up with your Photon App ID.
4. **Enter Play Mode**:
   - Press **Play** in Unity’s editor.
   - The game should load the **Lobby Scene**.
   - Players can click on the UI to join a room, and they will be transferred to **Room1** or **Room2**.
   - The timer should start once players are all in the room, and each player's score will be calculated based on the event duration.

---

## **Handling Issues**

- **Missing PhotonView**: Ensure that **PhotonView** components are attached to objects that need network synchronization.
- **Network Jitter**: If players experience jitter, adjust the **smoothTime** value in the `NetworkPlayer` script for better interpolation.
- **Timer Sync Issues**: If the timer is out of sync, check that **RPC calls** are correctly sent and received on all clients.
- **Scene Transitions**: If scene transitions are not working, check Photon settings and ensure players are properly connected to the correct room (e.g., through `PhotonNetwork.LoadLevel()`).

---

## **Important Notes**
- Make sure that **PhotonNetwork** is properly initialized before trying to interact with networked objects.
- When using **XR devices**, ensure the XR setup in Unity matches the hardware and properly tracks the player’s head and hand movements.

---

This concludes the updated documentation for your multiplayer VR event timer and score system with **Lobby** and **Room1/Room2** scene transitions. Make sure to properly test the network functionality in a multiplayer environment to ensure smooth synchronization of player actions and scene transitions.
