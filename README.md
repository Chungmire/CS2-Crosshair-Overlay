# CS2-Crosshair-Overlay

There were no existing public projects which took a crosshair sharecode to render the CSGO/CS2 crosshair as an overlay over the entire screen, so I embarked upon this little C# project to accomplish this. It should go without saying that this is not VAC detected nor is it VAC detectable as it does not interact with the game process in any way.

## Use cases

Most obviously, having your crosshair always enabled is helpful in scenarios when the game otherwise disables the crosshair, e.g. when using an AWP or SSG 08.

![](https://github.com/Chungmire/CS2-Crosshair-Overlay/blob/main/Example.gif)

Additionally, this assists with the efficacy of object detection aimbot programs which grab images from the game's video stream rather than from the entire desktop. Without a crosshair taking up valuable pixels, the confidence rate increases when the crosshair would otherwise be over an enemy.

To account for this, the program outputs a `crosshair.png` file in the directory it is placed in for those of you who are disabling your crosshair in-game while using this but still want to add it in post when editing a game capture recording.


## Usage

1. Download from [Releases](https://github.com/Chungmire/CS2-Crosshair-Overlay/files/14324202/1.0.zip).
2. Copy crosshair sharecode from game.
3. Open config.txt and paste the sharecode after `Sharecode=`
4. (Optional) Change toggle key. Default is Insert. Keycodes found [here](https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.keys?view=windowsdesktop-8.0) 
5. In CS2 video settings, set your Display Mode to Windowed or Fullscreen-Windowed.
6. Run Crosshair.exe


---

## Limitations & Future Work
> _Note: Feel free to suggest changes. While I won't be working on this project personally, I will gladly review and accept pull requrests._
1. Renders only the crosshair as the game would at 1920x1080 resolution as the scaling factor for other resolutions is not implemented. You can play at any resolution.
2. The RGB color displayed is technically accurate, however it does not always match with what the game displays. The values "Red" "Green" "Blue" seem to have different weights in-game resulting in colors you wouldn't expect (ex. blue when it should be green). To get around this you can just adjust the values and try new sharecodes until it's to your liking.
3. Only static crosshairs are supported.
4. Only common and reasonable length, gap, etc. values are supported. You shouldn't be running into issues with this.
5. Fullscreen is not supported and likely cannot be supported given the invasive route needed to accomplish that.
