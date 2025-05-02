# Fantasy Fiefdoms

## Overview

This was my first non-trivial look at Unity with the long-term aim of developing a turn-based strategy game in the style of the original X-COM games. This repo follows a tutorial by "Soul's Game Dev Journey" describing how to implement a TBS using a one layer hex grid. This repo has to follow the tutorial reasonably closely as I don't yet know enough about Unity to deviate too far from it. However, when possible the opportunity was taken to tidy up some of the scruffier bits of C# (i.e. where the author doesn't make use of language features and syntax, leaves lots of nesting, or repetition in place etc).

n.b. The above is not intended as a criticism of the author's C# skills. It should be considered they were writing C# for a tutorial that was aimed at people who were not necessarily C# experts. As such the C# is reasonably quite different to what you might write for the enterprise.

## Initial Setup

The tutorial used editor version 2022.3.4f1. This was not listed as a version I could install using Unity Hub. The closest I had was 2022.3.61f1. I located the exact version from the [download archive](https://unity.com/releases/editor/archive). Using this version seemed like the best idea, even though it was almost two years out of date, rather than risk potential issues caused by version mismatches that I did not yet possess the knowledge to resolve.

The tutorial used the template `3D (URP)`. This was not listed as an option for me. The closest match I had was `Universal 3D`. These both had the same description for the template, and indicated they used URP. This was in spite of the icon for the template seeming to indicate SRP. As with versions I did not possess the knowledge to tell if they were exactly the same. At the time I set the project up the limit of my knowledge was that URP was a type of SRP (along with HDRP), and distinct from BiRP.

This repository was initialised with LFS. Although it was not certain we'd end up with a lot of large assets it seemed better to err on the side of caution. For a relatively short tutorial, and not building a full game it probably did not make a lot of difference either way.

The tutorial was shown using Visual Studio, and presumably the community edition that is installable with the Unity editor. I stuck with using my preferred editor of Rider which was at version 2025.1.1 when I started the tutorial. Similarly, I saw no good reason to not stick to my preferred tooling for Git integration.

## Chapters

## [Chapter 1 - Learn to Create a Turn-Based Strategy with Unity](https://www.youtube.com/watch?v=khpL0NJ4QMM)

Note that this was simply an introduction and thus there was nothing to actually do.

## [Chapter 2 - Connect Unity with GitHub](https://www.youtube.com/watch?v=KrR4_uVSGKk)

- Initial setup of the Unity project
- Initial setup of the Rider project / solution
- Updated the JetBrains Rider Editor package in Unity to 3.0.36
- Updated the Visual Studio Editor package in Unity to 20.0.23
- Updated the TextMeshPro package in Unity to 3.0.9
- Update the Timeline package in Unity to 1.7.7

The tutorial did not suggest updating packages, and this was simply something I elected to do. This means all packages are at the latest versions.

## [Chapter 3 - How To Make A Hex Grid in Unity - Basic Setup](https://www.youtube.com/watch?v=0g0I9V_RhJ8)

- Add the `Assets\Scripts` folder
- Add the `HexGrid` script to the newly created folder, and outline five things we'd like this to do
- Add the `HexOrientation` enum
- Add properties to `HexGrid` to define size, orientation etc
- Add a new scene and empty game object, and attach the `HexGrid` script to the new game object
- Add the `HexMetrics` script
  - Add methods for radii, corners, and centre (noting that the `Corner` method shown has a bug that affects mesh generation later on, so I've fixed it)
- Add `OnDrawGizmos` to `HexGrid` to draw the grid in the Unity editor window

## [Chapter 4 - Custom Editor and Procedural Mesh Generation](https://www.youtube.com/watch?v=OTWzyUKkCxE)

- Add the `HexGridMeshGenerator` script with appropriate `RequireComponent` attributes
  - Add method for generating the hex mesh
  - Add method for clearing the hex mesh
- Add a custom editor for `HexGridMeshGenerator`
- Add a custom editor for `HexGrid`
  - Note the positioning of labels used in the tutorial causes them to overlap
- Add a `Grid` layer to the Grid object (from the `Layer` drop down in the Inspector) (optional)
- Add the `HexGridMeshGenerator` script to the Grid object
  - Set the `HexGrid` property to the Grid object
  - Set the `Grid Layer` property to it (optional)

### Notes

- This requires methods adding to the `HexMetrics` class that are _not_ shown in the tutorial video. These can be found from the repo associated with the tutorial
- Assigning the `HexGridMeshGenerator` to the Grid object is not explicitly shown in the tutorial
- Labels created by the `HexGridEditor` overlap if positioned as shown in the tutorial
- Adding the `Grid` layer is optional

## Useful Links

- [YouTube Playlist - Learn to Create a Turn-Based Strategy With Unity](https://www.youtube.com/playlist?list=PLHaBJbUxcrnnUAIB_z5mRprsRwt5ZRCdb)
- [SoulGameDev Fantasy Fiefdoms Repo](https://github.com/SoulsGameDev/Fantasy-Fiefdoms)
- [Red Blob Games - Hexagonal Grids](https://www.redblobgames.com/grids/hexagons/)
