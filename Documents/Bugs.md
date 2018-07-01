# Bugs

## H1 Critical

* [ ] Passage into cave is not blocked
* [ ] Add missing items to item database

## H1 Major

* [ ] Missing text when entering upper deck
* [ ] The orange boxes around the ladders
* [ ] On moving platforms, R is jumping like crazy and moving weird
* [ ] Fix camera transitions
* [ ] Sometimes in conversations, full text pops out immediately
* [ ] Polish camera transitions
    * Proposal: Make camera rooms overlap so after transition Randolph isn't off screen

## H1 Minor

* [ ] Locked sound not playing on locked doors in airship
* [ ] Randolphs bubble does not disappear on death
* [ ] Disable camera transitions and mouse event when there is camera transition
    * Now player disapears for a moment before transition exit finishes
* [ ] Sometimes can't see the whole bubble when R too high
* [ ] Jumping is a bit chaotic still
* [ ] Disable restarting while pause or in menus
* [ ] When grappling and camera is transitioning, hook is still changing length
* [ ] Investigate null ref exception:

```txt
System.NullReferenceException: Object reference not set to an instance of an object
  at Randolph.Core.EditorMethods.<FindComponentsOfPrefabs`1>m__1[T] (UnityEngine.GameObject prefab) [0x00001] in C:\Projects\Randolph\Assets\_Core\Scripts\Editor\EditorMethods.cs:24 
  at System.Linq.Enumerable+SelectListIterator`2[TSource,TResult].MoveNext () [0x00048] in <839a3cb835c04d14aeb58d83bb7bc4bd>:0 
  at System.Linq.Enumerable+WhereEnumerableIterator`1[TSource].ToList () [0x00030] in <839a3cb835c04d14aeb58d83bb7bc4bd>:0 
  at System.Linq.Enumerable.ToList[TSource] (System.Collections.Generic.IEnumerable`1[T] source) [0x0001f] in <839a3cb835c04d14aeb58d83bb7bc4bd>:0 
  at Randolph.Core.EditorMethods.FindComponentsOfPrefabs[T] (System.String[] relativeFolderPaths) [0x00029] in C:\Projects\Randolph\Assets\_Core\Scripts\Editor\EditorMethods.cs:24 
  at Randolph.Core.EditorInitializer.CheckSpriteRenderers () [0x00002] in C:\Projects\Randolph\Assets\_Core\Scripts\Editor\Initializers\EditorInitializer.cs:47 
  at Randolph.Core.EditorInitializer..cctor () [0x00006] in C:\Projects\Randolph\Assets\_Core\Scripts\Editor\Initializers\EditorInitializer.cs:14 
UnityEditor.EditorAssemblies:ProcessInitializeOnLoadAttributes()
```

## H1 Cosmetic

* [ ] Moving platform is not in cave style
* [ ] Add cut shrub asset
* [ ] Crows' sprite doesn't look like they're touching the ground
* [ ] Boulders are outlined (shouldn't be)
* [ ] Pickup sound could be better
* [ ] Put apple on a dead tree to fall from
* [ ] Add dead tree and background boulders assets

## H1 Solved

* [x] Captains scenario in Mountains wont reset
* [x] Can't jump off moonstone
* [x] Outline color for spitter
* [x] Bats and boulder always outlined
* [x] Howards scenario won't reset
* [x] Captains scenario won't reset
* [x] Sail stays cut off if reset
* [x] Bats outline not working
* [x] Outline still shows when the game is paused
* [x] Restarting of objects created wihh Instantiate
* [x] You can talk to characters out of applicable range
* [x] Fix sail IRestartable
* [x] Fix missing captains message on level 1
* [x] Clickable objest don't reset their `ShouldOutline` attribute
* [x] Crows don't restart facing the right direction
* [x] Bats animation playing even when static and doesn't reset
* [x] Mountains Area 2, if R dies with seed or pit in inventory, apple won't reset but fall down through the canvas
* [x] When you walk through door, Randolph transports to other location before camera shift
* [x] Hanging rope does not reset when randolph dies in airship
* [x] The tutorial does not end with the story screen
* [x] Nullref exception if game is restarted while player is on `ClimbableRope`
* [x] On death, Randolph doesn't disappear and keeps moving
* [x] Camera transitions in mountains level
