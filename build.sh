#!/usr/bin/env bash

#exit if any command fails
set -e

artifactsFolder="./artifacts"

if [ -d $artifactsFolder ]; then  
  rm -R $artifactsFolder
fi

dotnet restore

dotnet test ./Structure.Sketching.Tests/Structure.Sketching.Tests.xproj -c Release -f netcoreapp1.1

revision=${TRAVIS_JOB_ID:=1}  
revision=$(printf "%04d" $revision) 

dotnet pack ./Structure.Sketching/Structure.Sketching.csproj -c Release -o ./artifacts --version-suffix=$revision  