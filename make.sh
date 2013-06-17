#!/bin/bash

FOLDER=MissionController
MISSIONMANAGER=/home/paul/games/MissionManager/MissionManager.exe

rm -rf $FOLDER

mkdir $FOLDER
mkdir -p $FOLDER/icons

mkdir -p $FOLDER/Plugins/PluginData/MissionController

mono $MISSIONMANAGER missions/stock.mxml $FOLDER/Plugins/PluginData/MissionController/Stock.mpkg

for img in $(ls images/*.png); do
    base=$(basename "$img" ".png")
    convert $img -resize 70x70 $FOLDER/icons/$base.png
done

cp plugin/bin/MissionController.dll plugin/bin/MissionLibrary.dll $FOLDER/Plugins
