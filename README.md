# AscendingFede

![f](Images/example.jpg)

## Table Of Contents

1. [Project Description](#project-description)
2. [Development Partitions](#development-partitions)
   - [SMC: Camera Movement System](#smc-camera-movement-system)
   - [SPC: Player&#39;s Movement System](#smp-players-movement-system)
   - [SIPCP: Camera-player Perspective Integration System](#sipcp-camera-player-perspective-integration-system)
   - [SI: Inputs System](#si-inputs-system)
   - [SP: Player&#39;s System](#sp-players-system)
   - [SIEO: Interaction between Objects System](#sieo-interaction-between-objects-system)
   - [SMCD: Dragon&#39;s Control and Movement System](#smcd-dragons-control-and-movement-system)
3. [Installing and Running the Game](#installing-and-running-the-game)
4. [How to play the Game](#how-to-use-the-game)
5. [Contribute](#contribute)
6. [Credits](#credits)

## Project Description

AscendingFede is a Unity-based game that revolves (quite literally) around a tower. Grounded on the indie **FEZ**, it expands the original's functionalities by applying the concepts learned throughout the subject *Paradigmas de Programación*. Set in a medieval environment, the game will have the player control a knight's movements in order to get to the tower's apex.

Out of all the new exciting functionalities, we must highlight a system of power-ups and different kinds of platforms that modify the player's conditions (to put in the knowledge obtained on *OOP*). In addition to this, the player will have to deal with an extra factor; an enemy.

Therefore, the player's quest will not be as easy as it seems. The enemy, an evil dragon (who represents our deepest fears as well as communism), will follow the player as it climbs the tower. While this happens, the dragon, pushed forward by wicked and ancient ideas (communism), will shoot fireballs (depicting the constant hunger its victims suffer) at the knight, aiming to throw him off his quest.

The last thing we have to mention is that the dragon, in order to make it even harder for the player, will constantly learn newer and smarter ways of following the player and shooting him (function implemented by means of *MLAgents*)

## Installing and Running the Game

## How to use the Game

## Project Report

### Development Partitions

Given the extent of the project, the first days of its blooming were devoted towards breaking it down into specific parts, which we designated as *Systems*. This had a double objective: Making the development much more tangible by establishing particular goals and making the tasks' distiribution between the 3 components of the group easier.

Thus, (insert number of systems) different systems were identified, which we'll describe in the next paragraphs:

###### SMC: Camera Movement System

First of all, we focused on one of the coolest and most differentiating features of the game, this is, its camera. Even though the game is built in a 3D world, the player's perspective only features 2 dimensions, following the knight around the tower. To access the additional dimension, the player has the power of changing the camera perspective between 4 different positions (_looking_ at each face of our quadrangular tower).

This system, designated SMC, Camera Movement System, for its initials in Spanish, is encapsulated in the CameraSwitcher Script. In it, the principal function is the Pivot Interface, which uses an *Spherical Linear Interpolation* between the two positions of the camera. A natural question may arise: Why use slerp instead of a simple lerp? Given that our perspective is shifting in a 3D Space, a spherical interpolation that maps as though on a quarter segment of a circle provokes a much more visually appealing swap.

###### SMP: Player's Movement System

After making our camera mechanic, we took on the task of creating our character, one necessary feature in order to design other parts of the program (such as the functionalities of our platforms). In the FedeMovement script, we can find the physics that arise our knight's movement. Using RigidBody and BoxCollider, we enable its interaction with other objects. By means of subscriptions to events, we make it so that the knight can move when directed to do so by inputs (which will be centralised by the MovementManager script).

Each frame, our player checks its vertical position, updating it, as well as checking the point at which it's looking (in order to make the movement afterwards reasonable). Finally, basing its movement on the inputs (the events it's subscribed to), it executes the necessary animations as well as the position-variables (applying a force).

###### SIPCP: Camera-player perspective integration system

While updating both the player's and camera's transforms, we ran into a problem which needed a simple solution: Even though both movements had to keep a certain independence, it was clear that the camera must follow the player in its vertical position, as to keep him centered in the screen.

Using a LevelChanged Event, we were able to keep the camera and player's vertical position in lockstep. This functionality, scripted as a Coroutine checks the character's y position and executes an slerp in the TransitionToLevel interface

###### SI: Inputs System

As learned throughout the last weeks of the course, we are using an Event-Driven Architecture in our game. This comes in hand when managing the inputs. Given that our movements don't need to know what's happening to the inputs (they only need a command of what to do), we created three different events in the InputManager script (for moving, jumping and rotating). Employing the Input's GetAxis (for horizontal movement, which may be left-right) and GetKeyDown methods (for unique effects, such as jumping), we invoke the different subscribed objects. All the possible inputs are centralised in a Input Manager.

###### SIEO: Interaction between Objects System

In this partition, we must take into account the different managers that make up the structure of the interactions between each object: InputManager, EnvironmentManager and MovementManager. All three of them, in order to keep on respecting the good practices when expanding a software project, inherit their main frame from the Generic Singleton object. In this section, we'll explain on detail what objects each of them supervises

We'll start with the Input: Following the lines of correct design patterns, the inputs and its effects are connected using event-driven architecture, aiming at maximum independence between objects.

EnvironmentManager, on the other hand, is a bit more complex. First of all, using the event-action pairs of the InputManager, it handles the correct movement of the player. At first glance, this might not make sense, but it should be noted that the player may not move into a platform whose level is not identical, for example. The different conditions in which the player moves around the world constrain its freedom, reason why this function exists. Additionally, there exists an special platform, InvisibleCube, which will be described in the SPE section. Apart from handling Fede's movement around its surroundings, this manager also tells the CameraSwapper what to do (based on the inputs).

To end with this system, we needed one more Manager: one concerning the movements: both of the Camera and Fede's. In it, the different events related with each input are invoked (Jumps and movements for Fede and Rotations for the camera)

###### SMCD: Dragon's Control and Movement System

Up to this point, the player felt safe while climbing, only having to care not to trip on its foot. Or so he thought! We, developers, aren't so pious about our games, so a dragon is introduced to the game. Now, the knight must dodge its magic fireballs, otherwise it'll suffer a gory death. This constant chase-like mechanic is contained in the DragonHolder Script. In addition to this, to make its movement as realistic as possible, we had it keep its vertical position in a sway-like manner (), going up and down. This, together with the movement in the horizontal axis, made it much more realistic. Both these actions are encapsulated in the DragonMover Script

###### SPE: Environment Properties System

To make the game more challenging and entertaining, different kinds of platforms have been designed by our most creative members. Three different kinds will interact with the player: Ice, Fire and a Healing one. Each one will have different effects on the player (either changing its movement stats by communicating to the MovementManager or affecting the player's health via the HealthManager). They all inherit their functionalities from the BaseTerrain object, expanding its functionalities

As mentioned previously, there are special *platforms:* InvisibleCube objects. Through the development, a problem arose: ANDREW DESCRIBEMELO. That's why the creation of invisible cubes was decided. Using them, we could ...

## Contribute!

## Credits

This game was programmed as final project for the subject *Paradigmas de Programación* of the 3rd year of *Ingeniería Matemática e Inteligencia Artificial*, ICAI. As such, it was meant to wrap up all theory learnt throughtout the course. Created by Pablo García Molina, Andrés Martínez Fuentes and Fco. Javier Ríos Montes (aka Senador), this game is but the first of a list of projects done by these three fantastic people.

A big shout out to both our teachers, Arturo Serrano and Daniel Morell, for making it so much manageable all through the weeks of development.
