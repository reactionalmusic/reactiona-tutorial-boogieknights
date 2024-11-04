# Reactional Tutorial - Music Puzzler

![Alt text](MusicCrypt.png?raw=true "Title")

This project serves as a showcase and tutorial on how to easily retrofit musical gameplay into an existing game. When making this project, more time was spent tweaking colors than making it musical.

[Watch video](https://youtu.be/vlPF6UoaDwo)

## Important information

> [!CAUTION]
> In order to play this game, you will need to download the Reactional Unity Plugin.
> You will also need to download a music bundle.
> These can be found on https://app.reactionalmusic.com 

### Download music asset bundle from the Reactional Demo Project
Find the Demo Project field to the left and open the Reactional Demo Project. Click the project export button on the top right. Download and unzip the content into a folder, which you must then place in the StreamingAssets/Reactional folder in Unity.

The "section" of the project in question that is used is the "Music Game Stingers". If you want to create your own curated list of songs, make a new project, add the Theme "Music Game Stingers", and some tracks of your choosing.

## Gameplay

The game features a knight that kills stuff to the beat!

### How to play

Move the knight with WASD. You need to hold the keys in order to Move on Beat!
Swing the sword with SPACE.

## Musical Interactivity

The game has been embellished with several musical stingers based on the gameplay and user input.

- When the knight moves, a piano note will play.
- When an enemy is struck, a stinger will play.
- When the knight takes damage, a different stinger will play.

These stingers all automatically adapt to the current tempo and chord of the current backing track being played.

Aside from this a lot of additional musical juice is added:

- Lights pulse to the beat.
- All movement is done on beat subdivisions.
- Enemies and knight moves on beat.
- Sound effects are scheduled to always play on beat.
- A lightning strike triggers every 4 bars.
