
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

This project was buildt in Unity 6 using URP with the Occulus XR plugin and XR interaction toolkit which comes with an XR rig with Ray interactors wich allow for interaction with world space UI elements. The core functionality conisists of a DemoManager component attached to the XR rig with controls all of the individual demo components.

## Procedural Mesh Creation
This script procedurally generates a UV sphere gameobject(Object A) and a cone mesh as its child pointing in the forward direction. It creates vertices and triangles manually based on configurable parameters such as resolution and size. and assigns it a red material.

#### References used:
https://danielsieger.com/blog/2021/03/27/generating-spheres.html

https://catlikecoding.com/unity/tutorials/procedural-meshes/cube-sphere/

https://www.youtube.com/watch?v=6xs0Saff940&t=20925s

## Secondary Object
This similarly procedurally generates a UV sphere mesh(Object B) and a cone mesh as its child pointing in the forward direction and assigns it a blue material.

## Lissajous Animation
This script procedurally generates a UV sphere gameobject(Object A) and a cone mesh as its child pointing in the forward direction. It creates vertices and triangles manually based on configurable parameters such as resolution and size. and assigns it a red material.

#### References used:
https://danielsieger.com/blog/2021/03/27/generating-spheres.html

https://www.youtube.com/watch?v=6xs0Saff940&t=20925s


## Object Rotation
This script procedurally generates a UV sphere gameobject(Object A) and a cone mesh as its child pointing in the forward direction. It creates vertices and triangles manually based on configurable parameters such as resolution and size. and assigns it a red material.


## Color Change Based on Angle
This script procedurally generates a UV sphere gameobject(Object A) and a cone mesh as its child pointing in the forward direction. It creates vertices and triangles manually based on configurable parameters such as resolution and size. and assigns it a red material.

#### References used:

## Mesh Vertex Animation
This script procedurally generates a UV sphere gameobject(Object A) and a cone mesh as its child pointing in the forward direction. It creates vertices and triangles manually based on configurable parameters such as resolution and size. and assigns it a red material.

#### References used:
https://danielsieger.com/blog/2021/03/27/generating-spheres.html

https://www.youtube.com/watch?v=6xs0Saff940&t=20925s

## VR integration
This script allows VR object attraction toward the user's left and right controllers within a defined radius. The user can use sliders to control the attraction force which allows it to smoothly or quickly interpolates object positions toward the controllers using Vector3.Lerp().

## Improvements
This project could benefit from improvements such as cleaning up code structure to improve readability and better compartmentalised functions.

+ The Procedural mesh generation only generates a UV sphere mesh which has uneven vetex distribution near the poles. there are other sphere apporximation meshes such as Icospheres and Cube spheres that can 











