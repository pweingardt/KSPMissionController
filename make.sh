#!/bin/bash

FOLDER=MissionController
MISSIONMANAGER=/home/paul/games/MissionManager/MissionManager.exe

rm -rf $FOLDER

mkdir $FOLDER
mkdir -p $FOLDER/icons

mkdir -p $FOLDER/Plugins/PluginData/MissionController

mono $MISSIONMANAGER missions/stock.mxml $FOLDER/Plugins/PluginData/MissionController/Stock.mpkg


cp images/*.png $FOLDER/icons

cp plugin/bin/MissionController.dll plugin/bin/MissionLibrary.dll $FOLDER/Plugins
