# Mp3Renamer
Console utility for renaming .mp3, .wma, .wav audio files. (Written in C#.NET)

# Description

Renames audio files in current folder _.mp3_, _.wma_, _.wav_ or any other format you provide (i.e. _.jpg_, _.png_). Supports renaming in order specified in **Windows Media Player** playlist **_.wpl_** file. _.wpl_ file must be in the same folder where audio files sit, any name.

By default it renames all current folder files in ascending alphabetic order. If _.wpl_ is found in current directory, the order is used from playlist file.

You can specify target file extension as an argument:

  **>mp3_renamer.exe .mp3**

Requires Microsoft .NET Framework.

[Download Link](https://github.com/pnetesa/Mp3Renamer/raw/master/build/mp3_renamer.zip)
