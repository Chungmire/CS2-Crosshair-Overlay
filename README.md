# CS2-Crosshair-Overlay

There were no existing public projects which took a crosshair sharecode to render the CSGO/CS2 crosshair as an overlay over the entire screen, so I embarked upon this little C# project to accomplish this.

## Use cases

Most obviously, having your crosshair always enabled is helpful in scenarios when the game otherwise disables the crosshair, e.g. when using an AWP or SSG 08.

![](https://github.com/Chungmire/CS2-Crosshair-Overlay/blob/main/Example.gif)

Additionally, this assists with the efficacy of object detection aimbot programs which grab images from the game's video stream rather than from the entire desktop. Without a crosshair taking up valuable pixels, the confidence rate increases when the crosshair would otherwise be over an enemy.


## Usage

1. Download from Releases.
2. Copy crosshair sharecode from game.
3. Open config.txt and paste the sharecode after `Sharecode=`
4. (Optional) Change toggle key. Default is Insert. Keycodes found [here](https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.keys?view=windowsdesktop-8.0) 

---
