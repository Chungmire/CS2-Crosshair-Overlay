# CS2-Crosshair-Overlay

There were no existing public projects which took a crosshair sharecode to render the CSGO/CS2 crosshair as an overlay over the entire screen, so I embarked upon this little C# project to accomplish this.

## Use cases

Most obviously, having your crosshair always enabled is helpful in scenarios when the game otherwise disables the crosshair, e.g. when using an AWP or SSG 08.

![](https://github.com/Chungmire/CS2-Crosshair-Overlay/blob/main/Example.gif)

Additionally, this assists with the efficacy of object detection aimbot programs which grab images from the game's video stream rather than from the entire desktop. Without a crosshair taking up valuable pixels, the confidence rate increases when the crosshair would otherwise be over an enemy.


## Usage

1. Download from [Releases](https://github.com/Chungmire/CS2-Crosshair-Overlay/files/14324202/1.0.zip).
2. Copy crosshair sharecode from game.
3. Open config.txt and paste the sharecode after `Sharecode=`
4. (Optional) Change toggle key. Default is Insert. Keycodes found [here](https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.keys?view=windowsdesktop-8.0) 

---

## Limitations & Future Work
1. Renders the same crosshair as the game would at 1920x1080 resolution as the scaling factor for other resolutions is not implemented.
2. The RGB color displayed is technically accurate, however it does not match with what the game displays. The values "Red" "Green" "Blue" seem to have different weights in-game resulting in colors you wouldn't expect (ex. blue when it should be green)
3. Only static crosshairs are supported.
4. Only common and reasonable length, gap, etc. values are supported. You shouldn't be running into issues with this.
