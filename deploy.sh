#!/bin/bash

echo "Checking out to main branch..."
git checkout main

echo "Building to production..."
dotnet publish --configuration Release

echo "Uploading build to server..."
scp -r bin/Release/* hjalte@192.168.0.234:/var/www/kombuchaApi/

echo "Done"
