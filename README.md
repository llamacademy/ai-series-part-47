# Enemy AI With a Finite State Machine

In this tutorial repository you will learn how you can implement Enemy AI with a Finite State Machine. This is a full implementation using Unity Hierarchical State Machine ([Unity HFSM](https://github.com/Inspiaaa/UnityHFSM) which is marked as a package manager dependency already), a code-driven state machine framework for Unity.

[![Youtube Tutorial](./Video%20Screenshot.jpg)](https://youtu.be/jnETyJUiCiM)

## Quickstart
Most of the dependencies are already marked with the Package Manager. However [Unity Particle Pack](https://assetstore.unity.com/packages/vfx/particles/particle-pack-127325) and the [Quirky Llama](https://assetstore.unity.com/packages/3d/characters/animals/mammals/llama-quirky-series-178234?aid=1101l9QvC) (<-- affiliate link) are not included. Not clear on the redistribution rights of Unity Particle Pack, but definitely can't give away the paid asset.

This is intended to be used as reference with the [associated video tutorial](https://youtu.be/jnETyJUiCiM), but you can of course use it for whatever you want.

* Import Unity Particle Pack
* Replace Llama with your favorite 3d model or import the Quirky Llama
  * Ensure your `Animator` for your model has the state names as we have defined in `ChaseState`, `IdleState`, `SpitState`, `BounceState`, `AttackState`, and `RollState`. The names can be changed to whatever animations you have.
* Maybe run Built-In -> URP Render Pipeline Material Converter (Window > Rendering > Render Pipeline Converter)
* Click play!

## Supporters
Have you been getting value out of these tutorials? Do you believe in LlamAcademy's mission of helping everyone make their game dev dream become a reality? Consider becoming a Patreon supporter and get your name added to this list, as well as other cool perks.
Head over to the [LlamAcademy Patreon Page](https://patreon.com/llamacademy), join as a [YouTube Member](https://www.youtube.com/channel/UCnWm6pMD38R1E2vCAByGb6w/join), or even a [GitHub Sponsor](https://github.com/sponsors/llamacademy) to show your support.

### Phenomenal Supporter Tier
* Andrew Bowen
* YOUR NAME HERE!

### Tremendous Supporter Tier
* Bruno Bozic
* YOUR NAME HERE!

### Awesome Supporter Tier
* AudemKay
* Matt Parkin
* Ivan
* Reulan
* Iffy Obelus
* Dwarf
* SolarInt
* YOUR NAME HERE!

### Supporters
* Trey Briggs
* Matt Sponholz
* Dr Bash
* Tarik
* Sean
* ag10g
* Elijah Singer
* Lurking Ninja
* Josh Meyer
* Ewald Schulte
* Dom C
* Andrew Allbright
* AudemKay
* Claduiu Barsan-Pipu
* Ben
* Xuul
* Christiaan Van Steenwijk
* Michael Creel
* Joseph Janosko
* Joao Henrique Machado Silva
* YOUR NAME HERE!

## Other Projects
Interested in other Topics in Unity? 

* [Check out the LlamAcademy YouTube Channel](https://youtube.com/c/LlamAcademy)!
* [Check out the LlamAcademy GitHub for more projects](https://github.com/llamacademy)

## Requirements
* Requires Unity 2022.3 LTS or higher.
* Universal Render Pipeline
* For the Llama Model, you'll need the [Quirky Llama](https://assetstore.unity.com/packages/3d/characters/animals/mammals/llama-quirky-series-178234?aid=1101l9QvC) (<-- affiliate link) which is a paid asset. You can choose to replace this with any other 3d model and animations you wish. If you own this Llama, you can simply import it and it will hook it self up. You may need to change to the `ShaderGraphs/` variant of the included shader since this is a URP project.
* [Unity Particle Pack](https://assetstore.unity.com/packages/vfx/particles/particle-pack-127325)
* [Navigation Components 1.1+](https://docs.unity3d.com/Packages/com.unity.ai.navigation@1.1/manual/index.html)
