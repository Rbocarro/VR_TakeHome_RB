
# Take Home Task

In this project I was tasked with developing a series of small demos consisting of procedural 3D mesh generation, animation, interaction with optional VR integration.

# Project Video Demonstration
https://www.youtube.com/watch?v=vpM1LpFZr8U


# Setup Instructions

1. Clone the repo:
   ```bash
   git clone https://github.com/Rbocarro/VR_TakeHome_RB

   ```
2. Open the project in Unity 6000.0.33f1 or later with URP enabled
3. Load the Demo scene in: Assets/Scenes/DemoScene.unity



# Implementation

This project was built in Unity 6 using URP with the Occulus XR plugin and XR interaction toolkit which comes with an XR rig prefab. The core functionality conisists of a DemoManager component attached to the XR rig which controls all of the individual demo components using coroutines or toggling induvidual components when needed.

## Procedural Mesh Creation
This script procedurally generates a UV sphere gameobject(Object A) and a cone mesh as its child pointing in the forward direction. It creates vertices and triangles based on configurable parameters such as resolution and size and assigns it a red material.

#### References used:
https://danielsieger.com/blog/2021/03/27/generating-spheres.html

https://catlikecoding.com/unity/tutorials/procedural-meshes/cube-sphere/

https://www.youtube.com/watch?v=6xs0Saff940&t=20925s

## Secondary Object
This similarly procedurally generates a UV sphere mesh(Object B) and a cone mesh as its child pointing in the forward direction and assigns it a blue material.

## Lissajous Animation
This was implmented by creating a LissajousMovement script attached to a gameobject which animates it along different types of Lissajous and parametric curves such as the standard Lissajous formula, Heart and Butterfly by adjusting values such as amplitudes, frequencies and phase shift using UI elements. some of the UI fucntionality for this was moved to the LissajousUIBindings class which helped with deculttering the main DemoManager class;

#### References used:
https://www.intmath.com/trigonometric-graphs/7-lissajous-figures.php


## Object Rotation
This was implemented by randomly placing Object B around object A along a given radius. Object A then smoothly rotates object A to face object B using Quaternion.RotateTowards() and an angularSpeed muitiplier. Once Object A is within a treshhold, Object B moves to a diffrent positon around object A.


## Color Change Based on Angle
This script takes the dot product of the forward vector of object and and the direction from it to object B. which is then sent to an simple unlit HLSL shader wich interpolates the color in the fragment shader. there were some rendering issues when writing this shader related to URL and how it handels single and multi pass instancing.This was remedied by adding some macros to the shader.

#### References used:
+ https://www.youtube.com/watch?v=JiCJN8EvoCA
+ https://docs.unity3d.com/Manual/SinglePassInstancing.html

## Mesh Vertex Animation
This script get reference to the mesh componet of and displaces its vertices using unity's Mathf.PerlinNoise() function with controllable parameters such as noise scale, displacement and scroll speed. A simple Fresnel shader is applied to the mesh to improve visibility.

## VR integration
This script allows VR object attraction toward the user's left and right controllers within a defined radius. The user can use sliders to control the attraction force which allows it to smoothly or quickly interpolates object positions toward the controllers using Vector3.Lerp(). Addtionally each demo postions the demo objects infront of the VR camera's forward vector.

## Improvements
If given more time, This project could benefit from improvements such as cleaning up code structure to improve readability and better compartmentalised functions.

+ The Procedural mesh generation only generates a UV sphere mesh which has uneven vetex distribution near the poles. there are other sphere apporximation meshes such as Icospheres and Cube spheres that can 
+ The vetrex displacement can be implmented in a vertex shader which can offload the calculations to the GPU allowing for better perfromance. Additionally Using a precalculated Perlin Noise texture can also allow for a performance improvement especially on VR devices.











