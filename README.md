UnityPlatformerUtils
====================

Various scripts designed to make creating platformer games in Unity easier.

##Scripts

There are currently two scripts in this. Both are for working with tiles. All scripts have been modified to not require any other scripts along with them.

###TileColliderManager.cs

This script automatically generates the fewest needed BoxCollider2D colliders to cover the scene.
It includes options for what tag to give collider objects, tags of objects to create colliders for, tile size and 2D Physics Materials to apply to the colliders.

While this is meant to be attached to a GameController object, it can be attached to nearly any object in the scene. It will run one time as soon as it is created in
the scene, and can be safely destroyed afterwards if necessary.

###TileTextureManager.cs

This script is for automatically detecting tiles neighboring the one it is attached to, and then adjusting its parent tiles texture and rotation accordingly to match
based on what it has found. The main purpose of this is to allow simply placing tiles and then being able to forget about them, in addition to requiring only one
sprite for each situation, as opposed to one for each possible rotation.

Includes options for tags of objects to handle the textures of, and tile size. Sprites must be provided for every possible entry, though they don't have to be different
sprites. Must be attached to each of the tiles it will modify the texture of. It will run one time as soon as it is created in the scene, and can be safely
destroyed afterwards if necessary.
