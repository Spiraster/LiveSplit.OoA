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
- The tree view lists all the events supported by the autosplitter.
- Select (by checking the box) each event for which you would like the autosplitter to split.
  - Note: The autosplitter does NOT check against split names, so make sure to have only as many splits as selected events.

## Credits
- [Spiraster](http://twitch.tv/spiraster)
- Work initially based upon old [Yoshi's Island](https://github.com/LiveSplit/LiveSplit.YoshisIsland) autosplitter.
- Many improvements guided by the [LiveSplit.Dishonored](https://github.com/fatalis/LiveSplit.Dishonored) component by [Fatalis](http://twitch.tv/fatalis_).
