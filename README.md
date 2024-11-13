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

Each weapon usable character (including AI) all are managed through State Machine. A base class - Human State Manager works to manage states of character as well as weapon usage. 
