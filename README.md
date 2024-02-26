# RTVF-376-Animation-Template-Game
 A template 3D game for RTVF 376 Animation for Games students to test their animations.

## Instructions for Students
The demo scene is located in the `Assets/Scenes` folder. The scene is called `SampleScene`.

The animations that you must edit are found in the `Assets/Animations` folder. You will be editing the `PlayerIdle`, 
`PlayerRun`, `PlayerJump`, and `PlayerAction` animations.

If you wish to tweak some parameters related to the character, find the `Player` object in the hierarchy and change 
any of the following parameters on the `PlayerController` component:
- `Speed`: The speed at which the character moves.
- `JumpForce`: The force applied to the character when jumping.
- `ActionKey`: The keyboard key used to trigger the `PlayerAction` animation.
- `TurnTime`: The amount of time it takes for the player to rotate 180 degrees when changing the direction of movement.
- `GroundCheckSize`: A `Vector3` representing how large the box checking for whether the player is touching the ground is.

Any questions? Ask Mercedes, Jack, or Eric.
