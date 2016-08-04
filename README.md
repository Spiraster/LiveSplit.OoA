# LiveSplit.OoA
This is a LiveSplit autosplitter component for The Legend of Zelda: Oracle of Ages on emulator.

This autosplitter works with either BGB or Gambatte. It will autodetect which one you're using.

**Supported emulators:**
- BGB 1.5.1
- BGB 1.5.2
- Gambatte r571

## Features
- Automatically start the timer when you select a file.
- Automatically select the file when the timer starts. (Useful for SRL racing through LiveSplit)
- Automatically reset the timer when you hard reset the emulator/ROM.

**Note:**
The automatic file select function involves bringing the emulator to the foreground and sending the "]" key a number of times in quick succession when the timer starts.
To use it you'll need to map either your start button or A button to "]". There's no option yet to change which key is used.

## Installation
- Go to "Edit Splits..." in LiveSplit.
- Click the "Activate" button to download and enable the autosplitter (make sure the game name is correct).

## Set-up
- Create your splits how you'd like.
  - Choose from any number of the supported splits.
  - They can be in any order.
- Enter your split names into the settings dialog if yours differ from the defaults.
  - If your split names don't match what's in the settings, the autosplitter will think that split is missing.

## Credits
- [Spiraster](http://twitch.tv/spiraster)
- Work initially based upon old [Yoshi's Island](https://github.com/LiveSplit/LiveSplit.YoshisIsland) autosplitter.
- Further improvements guided by the [LiveSplit.Dishonored](https://github.com/fatalis/LiveSplit.Dishonored) component by [Fatalis](http://twitch.tv/fatalis_).
