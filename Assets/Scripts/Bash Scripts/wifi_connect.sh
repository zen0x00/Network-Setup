#!/bin/bash

SSID="$1"
PASSWORD="$2"

if [ -z "$SSID" ]; then
    echo "ERROR: SSID missing"
    exit 1
fi

if [ -z "$PASSWORD" ]; then
    nmcli device wifi connect "$SSID"
else
    nmcli device wifi connect "$SSID" password "$PASSWORD"
fi

sleep 2

ping -c 1 -W 2 8.8.8.8 > /dev/null 2>&1
if [ $? -eq 0 ]; then
    echo "SUCCESS"
    exit 0
else
    echo "FAILED"
    exit 1
fi
