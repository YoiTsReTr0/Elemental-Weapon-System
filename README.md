**_Greetings!_**

Welcome to the Elemental Weapon System project. This is a modular weapon system that lets players switch between three elemental powers: Fire, Ice, and Electricity. 
Each weapon has its own unique visual effects, brought to life using Unity's Shader Graph and VFX Graph, and designed to run smoothly on mobile devices.

This project is built with Object-Oriented Programming principles and DSA to keep it clean, modular, and ready for expansion if more weapons get added in the future. 
Whether you’re exploring the code, experimenting with visual effects, or just enjoying the demonstration, this project will be a mesmerizing experience and can be considered as a ready to use plug-in as well.

Dive in and enjoy the world of elemental combat! 👾

<br><br><br>

**Overall Breakdown**

This project thrives on ever-expansion and the framework is highly optimized as the entire system only communicates with different parts of the script as well as other scripts using UnityEvents.
The project development is inspired by Unreal Engine's basic concept of working only on events and callbacks. This gives the project freedowm of movement and least dependencies on other scripts, thus least amount of code line calls.

Talking about weapon system, the system works on types/classes of weapons like Assault Rifle, SMG, Shotgun, Sidearm, Melee etc. All weapons are further instances of weapon classes. 
Each class supports individual Shooting system and recoil plus spread methods while being backed by abstract Weapon class parent which holds common functions like data update, Reload, Events etc. Scriptable objects holds 
individual weapon data and each weapon will hold its own data object. 
Each weapon can possess its own power like Fire, Electricity/Lightning and Ice/Frost. Looking at the same, the makeup of Shader Graphs revolve around the same idea. Depending on the type of power the weapon has, specific shader/texture gets applied 
to the weapon which is further open for modifications and expansion endlessly. For this version, a basic concept of Weapon Skin is projected for each weapon.

Each character who can use weapon (including AI) all are managed through State Machine. A base class - Human State Manager works to manage states of character as well as weapon usage. 
The main PLayer script shall derive from Human state manager as well as all other human entities. But this project specifically holds focus on weapon system.

Character animations are smartly embedded with the Human State Manager and entirely follows event based system and Triggers only. For this demonstration which doesnt support movement, smart usage of Triggers in the Animator, makes the game super smooth and 
jerks free.

A simple demonstration has been provided in the project where you can play with the weapon system. Pickable weapos, basic inventory system using Weapon Slot Types and realtime shootable weapons.

<br><br><br>

**Platform Support**

This framework is designed to work on any platform as the idea behind its development is to make it a ready-to-use pulgin as well. You can experience the gameplay in Mobile devices which is the default platform set for this project.
Otherwise, Keyboard and Mouse controls are also setup and can be switched at the click on a button. Basic resolution set to match 2200x1080 which is the current standard screen size for mobile devices.

As for performance and optimization, this project's framework is designed in the way that there is not even a single line of unnecessary conversation between parts of scripts and other script either. 
Shader graphs are made with simple yet eye catching textures. The weapon models are low poly and total Tris count of the scene is 110k and Vert count is 71k which is only present because of the models used. At any time of the development cycle,
low poly weapon and/or character models can be used to get the counts in check. 

Each weapon shot, rapidly produces projectiles as bullets which is managed by the weapon data. Each projectile is expected physics managed in the following versions and follow gravity which leads to bullet drops, making the game more fun and strategic.
Also in the following versions of this project, projectiles spawned by shooting weapons, will be managed using Object pooling (which was not the topic of this task). Also for an artistic look, the projectiles dont immediately get destroyed on 
contact with the collide-able surfaces, gets rigidbody kinematic and can be seen on the surfaces (wall in this case) which can be looked at in the Cam 3.
