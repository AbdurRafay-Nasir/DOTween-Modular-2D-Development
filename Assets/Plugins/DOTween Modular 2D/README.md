# DOTween Modular 2D
Collection of Modular components for DOTween

## Features
- Tween objects without writing code
- Organized inspector
- Visual Editing Tools
- Preview Tweens within Editor
- Utility Methods for generating curves
- Extension Methods for 2D Look At

## Prerequisite
- [DOTween](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)
- DOTween asmdef 
(Tools -> Demigiant -> DOTween Utility Panel -> Create ASMDEF)

## Installation
- Window -> Package Manager
- Click **+**, Select **Add Package Form Git URL**
- Paste this URL: https://github.com/Linked-Games/DOTween-Modular-2D.git, Click add
- There will be some errors, ignore them
- Project Window -> Packages -> DOTween Modular 2D -> RIGHT_CLICK -> Show in Explorer
- Cut com.linkedgames.dotweenmodular2d, Paste at YOUR_PROJECT -> Assets -> Plugins
- Add DOTween asmdef reference at com.linkedgames.dotweenmodular2d -> Runtime -> linkedgames.dotweenmoudular2d
- Click Apply and you are all set, you can now rename com.linkedgames.dotweenmodular2d
to DOTween Modular 2D or whatever you like

## Issues
- targetPosition and pathPoints in DOMove and DOPath respectively is recalculated when changing value of 'relative', Undo/Redo of 'relative' does not correctly apply previous value of targetPosition and pathPoints 

## Contribute
If you would like to contribute to the package visit this [link](https://github.com/Linked-Games/DOTween-Modular-2D-Development)

## Author
- Linked Games
- linkedgames07@gmail.com
- https://github.com/Linked-Games