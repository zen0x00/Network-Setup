#!/bin/bash

SSID="$1"
PASS="$2"

OUTPUT=$(nmcli device wifi connect "$SSID" password "$PASS" 2>&1)

if echo "$OUTPUT" | grep -q "successfully activated"; then
    echo "Connected"
elif echo "$OUTPUT" | grep -q "No network with SSID"; then
    echo "Network Not Found"
elif echo "$OUTPUT" | grep -q "Secrets were required"; then
    echo "Wrong Password"
else
    echo "Connection Failed"
fi
