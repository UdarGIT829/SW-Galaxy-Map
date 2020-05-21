# StarWars Star Map
## Coded by- Viraat Udar
## Map source: https://blog.naver.com/copulso/120031200656

Program to display a starmap, which the user can find planets &amp; star systems based on visual inpection or input information.

Although I have provided a map from the mentioned source, because it came with an index, it technically would work with any similar map. I have been slowly populating the file "sector-text" based on the provided map, however.

Compiled on Ubuntu 18.04 using Mono:
    * mcs sw-mapper.cs -pkg:dotnet
    * mono sw-mapper.cs

###### Known Bugs:
    * When set to user input, even with a blank star sector textbox failed searches in dictionary still say "added to system <whatever>"
    * "Could not set X locale modifiers" error, as I am rusty on C# I neglected to add destructors or the 'dispose()' call

##### Planned features:
    * ADD COMMENTS!
    * Checking for input files existence and format
    * Overlay image of user inputed systems, because goodness knows I havent found a complete SW star map
    * Search function
    * Highlighting of sector or star systems
    * Detailed descriptions of star systems
