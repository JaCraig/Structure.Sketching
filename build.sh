#!/usr/bin/env bash

#exit if any command fails
set -e

artifactsFolder="./artifacts"

if [ -d $artifactsFolder ]; then  
  rm -R $artifactsFolder
fi

dotnet restore

cd ./Structure.Sketching.Tests

dotnet test -c Release -f netcoreapp1.1

cd ..

revision=${TRAVIS_JOB_ID:=1}  
revision=$(printf "%04d" $revision) 

dotnet pack ./Structure.Sketching -c Release -o ./artifacts --version-suffix=$revision  