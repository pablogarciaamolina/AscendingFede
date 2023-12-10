# AscendingFede

![](https://github.com/pablogarciaamolina/AscendingFede/blob/main/Images/Portada.jpg)

## Table Of Contents

1. [Project Description](#project-description)
2. [How to use the Game](#how-to-use-the-game)
3. [Project Report](#project-report)
   - [Development Partitions](#development-partitions)
      1. [SMC: Camera Movement System](#smc-camera-movement-system)
      2. [SMP: Player&#39;s Movement System](#smp-players-movement-system)
      3. [SIPCP: Camera-player Perspective Integration System](#sipcp-camera-player-perspective-integration-system)
      4. [SI: Inputs System](#si-inputs-system)
      5. [SIEO: Interaction between Objects System](#sieo-interaction-between-objects-system)
      6. [SMCD: Dragon&#39;s Control and Movement System](#smcd-dragons-control-and-movement-system)
      7. [SPE: Environment Properties System](#spe-environment-properties-system)
      8. [SP:Perspectives System](#sp-perspectives-system)
5. [UML Structure](#uml-structure)
6. [Credits](#credits)

## Project Description

AscendingFede is a Unity-based game that revolves (quite literally) around a tower. Grounded on the indie **FEZ**, it expands the original's functionalities by applying the concepts learned throughout the subject *Paradigmas de Programación*. Set in a medieval environment, the game will have the player control a knight's movements in order to get to the tower's apex.

Out of all the exciting functionalities, we must highlight a system of different platforms that modify the player's conditions (to put in the knowledge obtained on *OOP*). In addition to this, the player will have to deal with an extra factor; an enemy.

Therefore, the player's quest will not be as easy as it seems. The enemy, an evil dragon, will follow Fede (our brave knight) as he climbs the tower. While this happens, the dragon, pushed forward by a wicked but unsparing determination, will shoot fireballs at the knight, aiming to throw Fede off his quest.

## How to use the Game

Clone the project in your computer. Afterwards, use the *Add project from disk* option in the Unity Hub. Run, and enjoy!

## Project Report

### Development Partitions

Given the extent of the project, the first days of its blooming were devoted towards breaking it down into specific parts, which we designated as *Systems*. This had a double objective: Making the development much more tangible by establishing particular goals and making the tasks' distiribution between the 3 components of the group easier.

Thus, 8 different systems were identified, which we'll describe in the next paragraphs:

###### SMC: Camera Movement System

First of all, we focused on one of the coolest and most differentiating features of the game, this is, its camera. Even though the game is built in a 3D world, the player's perspective only features 2 dimensions, following the knight around the tower. To access the additional dimension, the player has the power of changing the camera perspective between 4 different positions (_looking_ at each face of our quadrangular tower).

This system, designated SMC, Camera Movement System, for its initials in Spanish, is encapsulated in the CameraSwitcher Script. In it, the principal function is the Pivot Interface, which uses an *Spherical Linear Interpolation* between the two positions of the camera. A natural question may arise: Why use slerp instead of a simple lerp? Given that our perspective is shifting in a 3D Space, a spherical interpolation that maps as though on a quarter segment of a circle provokes a much more visually appealing swap.

###### SMP: Player's Movement System

After making our camera mechanic, we took on the task of creating our character, one necessary feature in order to design other parts of the program (such as the functionalities of our platforms). In the FedeMovement script, we can find the physics that make possible our knight's movement. Using RigidBody and BoxCollider, we enable its interaction with other objects. By means of subscriptions to events, we make it so that the knight can move when directed to do so by inputs (which will be centralised by the MovementManager script).

One interesting feature of Fede's movement has to do with the perspectives we talked about in the SMC (and a problem we had from the beginning): Given that there are 4 different perspectives for the camera, a single set of inputs failed at moving Fede in the directions it should (as left-right changes mathematically depending on the perspective). That's why each point of view for the camera has a set of directions for the movement (contained in the Directions variable of the Constants script)

Each time the player jumps and lands on a new tile (once Fede's grounded), an event is raised (LevelChanged in the UML), which is centralised through the MovementManager so that both the dragon and the camera get on track with them. This will be more thoroughly explained in following sections. For now, we'll explain what criteria the program follows in order to know the player is stable on a tile. This is made through RayCasting such that, if the length of the rays casted towards a given surface is sufficiently small, Fede is considered to be grounded

###### SIPCP: Camera-player perspective integration system

While updating both the player's and camera's transforms, we ran into a problem which needed a simple solution: Even though both movements had to keep a certain independence, it was clear that the camera must follow Fede in its vertical position, as to keep him centered in the screen.

Using a LevelChanged Event, we were able to keep the camera and player's vertical position in lockstep (setting the camera a bit higher than Fede, to get a better perspective). This functionality, scripted as a Coroutine checks the character's y position and executes an slerp in the TransitionToLevel interface. It must be noted, though, that this is only done when Fede is proved to be grounded (stable on ground), as explained in the SMP.

Given that both objects (Fede and the camera) rotate at the same time (but at different *speeds*, being Fede's faster), the rotating functionality is blocked until both are finished. This is done via an event raised by the camera when it's done (going through the MovementManager, which lets Fede know that he can rotate)

###### SI: Inputs System

As learned throughout the last weeks of the course, we are using an Event-Driven Architecture in our game. This comes in hand when managing the inputs. Given that our movements don't need to know what's happening to the inputs (they only need a command of what to do), we created three different events in the InputManager script (for moving, jumping and rotating). Employing the Input's GetAxis (for horizontal movement, which may be left-right) and GetKeyDown methods (for unique effects, such as jumping), we invoke the different subscribed objects.

These inputs are different for each possible action. For instance, the jumps, as may be guessed, make up a boolean set (either jumping or not), while both rotations take -1 or 1 depending on the direction. Last, the horizontal movement takes 3 different values (-1, 0, 1) as Fede might remain still in his position.

Each frame, our player checks its vertical position, updating it, as well as checking the point at which it's looking (in order to make the movement afterwards reasonable). Finally, basing its movement on the inputs (the events it's subscribed to), it executes the necessary animations as well as the position-variables. Fede's position parameters are updated using Forces, instead of the usual movement updates, so we can limit the speed he can reach, aiming at a more physicially realistic work.

###### SIEO: Interaction between Objects System

Over the report, a term has and will be repeated that has been central around our game's development: Event-driven Architecture. Considering that our project is, though not unbearable size-wise, quite large, with loads of scripts and a good number of different classes interacting, independence between them when possible was a maxim. For that reason, most of the interactions between classes were built using this architecture.

Returning to the matter in concern, this system revolves around three of the managers that make up the structure of the interactions between each object: InputManager, EnvironmentManager and MovementManager. All three of them, in order to keep on respecting the good practices when expanding a software project, inherit their main frame from the Generic Singleton object. In this section, we'll explain on detail what objects each of them supervises.

We'll start with the Input: Following the lines of correct design patterns, the inputs and its effects are connected using event-driven architecture *talking* with the MovementManager (MovementEvent in the UML), aiming at maximum independence between objects.

EnvironmentManager, on the other hand, is a bit more complex. First of all, using the event-action pairs of the InputManager, it handles the correct movement of the player. At first glance, this might not make sense, but it should be noted that the player may not move into a platform whose level is not identical, for example. The different conditions in which the player moves around the world constrain its freedom, reason why this function exists. Additionally, there exists an special platform, InvisibleCube, which will be described in the SPE section. Apart from handling Fede's movement around its surroundings, this manager also tells the CameraSwapper what to do (based on the inputs).

The last manager, MovementManager, deals with the movements: both of the Camera and Fede's. In it, the different events related with each input are dealt with (Jumps and movements for Fede and Rotations for the camera)

The last part of this system was the possible situations arising between Fede and the objects around them, in particular fireballs and the different terrains. These relations are captured in the FedeHealth script, where the stats from the script FedeStats are changed, depending on the action that takes place. These possible actions are three: First, burn. When Fede comes in contact with a *FireTerrain*, a burning process is started (using Coroutines), updating Fede's health until he comes in contact with a normal tile, stopping this burning, or dies.

Second, the healing process works in a similar manner as the burning: it heals Fede (stopping at his maxHealth) while he's in contact with the tile. Last, the fireballs, when colliding with Fede, are managed through the FedeHealth script and update Fede's health.

###### SMCD: Dragon's Control and Movement System

Up to this point, the player felt safe while climbing, only having to care not to trip on its foot. Or so he thought! We, developers, aren't so pious about our games, so a dragon is introduced to the game. Now, the knight must dodge its magic fireballs, otherwise it'll suffer a gory death. the dragon's movement will consist of two main mechanics: It will constantly revolve around the tower, searching for the best angle at which to shoot poor Fede. This rotation, included in the DragonHolderMover script, is done through an interface, Pivot, which is powered with the update every frame. To make it more realist, the dragon will change its direction of rotation frequently (the frequence is randomly decided in the Awake).

The other mechanic will consist of a constant following of Fede's 	vertical position. This is coded in the DragonMover script, specifically in the ChangeLevel method, which uses the same event of the Movement Manager the camera uses (CharacterChangeOfLevel) to know when to update its vertical position. It has one difference, though. In order to have the high ground, which is a clear advantage in combat (see Obi-Wan's fight against Anakin, for instance), the dragon will always be a bit over Fede (the exact distance is a constant present in the Constants script).

At the beginning of the game, the player might not understand clearly how Fede's movement or the dragon work, so to make the learning curve a bit more attainable, the dragon (in a fit of empathy towards Fede, maybe) will but tease them, attacking with a fraction of its power. But the player must be aware, for the higher on the tower they get, the dragon will devote more of its will against Fede's objective. This, in the game, is achieved using the DragonStageManager script, which swaps between 3 different difficulties depending on the height. What, might the player ask, does this difficulty entail? Well, smart of them to ask, because they will have to deal with more fireballs, which will be fired at a higher rate. What a challenge!

###### SPE: Environment Properties System

To make the game more challenging and entertaining, different kinds of platforms have been designed by our most creative members. Three different kinds will interact with the player: Ice, Fire and a Healing one. Each one will have different effects on the player (either changing its movement stats or affecting the player's health). They all inherit their structure from the BaseTerrain object, expanding its functionalities. Fire and Healing were explained previously, so now we'll focus on Ice: Put simply, Fede's movement stat is changed from the normal initMaxHorizontalSpeed to the iceSpeed (Constants script).

It must be noted that, in order to affect the player, the terrains (by means of their methods, either ModifyMovement or ModifyHealth), when colliding with another object (which, as makes sense in the context, may only be Fede), change directly Fede's stats. Given that nothing else can collide with the terrain, this is a correct and simple approach. In the case that more objects with the capacity of colliding should be designed, it can be easily changed as to know what type of object the other component is.

Something that may not be so clear at first glance is the way these effects are applied. To understand it, we must take into account that the BaseTerrain class is the one that sets the modifying methods, which are overwritten by each special case. This creates some special behaviors with the cumulative effects of tiles. We can see that there are two main terrains: the ones affecting movement and the ones affecting health. Considering this, the way they affect each other is different: If Fede jumps from a movement-terrain to a movement-terrain, the effects are added together (same with health-terrain and health-terrain). If, on the other hand, the consecutive terrains are different, the effects of the first one disappear (as the second terrain uses the ModifyHealth or ModifyMovement base method, reseting the effect).

###### SP: Perspectives System

As mentioned previously, there are special *platforms:* InvisibleCube objects. Through the development, a problem arose: The perspective's mechanic, one of the main features of our game, made it so that all tiles were reachable even if they were on the other side of the tower, as they overlapped when projecting the 3D game into the 2D perspective. That's why the creation of invisible cubes was decided. Using them, we could make only the *visible* objects at each time reachable. At the start, using RayCasting, the manager gets the visible platforms from the actual perspective. Each time the player swaps the camera's view, the EnvironmentManager, if the player is on top of an ICube, moves Fede to the actual platform this ICube represents (making a movement untraceable by the player, as it's done in the dimension not currently manageable). When the rotation of the camera is finished, the manager moves Fede back to the ICube, so that he can move freely on its perspective. Additionally, it sends an event to all the PlayableEnvironment objects in seen to form an ICube (if the player is not in the way, obviously). Taking this into account, the player will be playing (most of the times but at the start) in the ICubes representing the actual platforms.

## UML Structure

Given the extent of the project, as well as the size of important classes like Fede and the Dragon, not all methods were included in their parts, only some of the (in our oppinon), most important to know how they work.

![](https://github.com/pablogarciaamolina/AscendingFede/blob/main/Images/UML_Diagram.png)

## Credits

This game was programmed as final project for the subject *Paradigmas de Programación* of the 3rd year of *Ingeniería Matemática e Inteligencia Artificial*, ICAI. As such, it was meant to wrap up all theory learnt throughtout the course. Created by Pablo García Molina, Andrés Martínez Fuentes and Fco. Javier Ríos Montes (aka Senador), this game is but the first of a list of projects done by these three fantastic people.

A big shout out to both our teachers, Arturo Serrano and Daniel Morell, for making it so much manageable all through the weeks of development.
