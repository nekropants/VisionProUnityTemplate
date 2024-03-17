#!/bin/sh

# Navigate to your Xcode project directory
cd "$1"

# Build the project for a connected iOS device
xcodebuild -project Unity-iPhone.xcodeproj -scheme Unity-iPhone -configuration Release -destination 'generic/platform=iOS'

# Assuming the build produces an .app file, replace MyApp.app with your app's name
# Install the app on the connected device
xcrun simctl install booted /path/to/your/app/MyApp.app

# Launch the app on the device
xcrun simctl launch booted your.app.bundle.identifier
