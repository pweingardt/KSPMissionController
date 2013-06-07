#!/bin/bash

FOLDER=MissionController

rm -rf $FOLDER

mkdir $FOLDER

mkdir -p $FOLDER/Plugins/PluginData/MissionController

cp missions/* images/icon.png $FOLDER/Plugins/PluginData/MissionController

cp src/bin/MissionController.dll src/bin/MissionLibrary.dll $FOLDER/Plugins
