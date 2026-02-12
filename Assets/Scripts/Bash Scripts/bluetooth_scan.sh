#!/bin/bash

bluetoothctl --timeout 5 scan on > /dev/null 2>&1 &
sleep 5
bluetoothctl scan off > /dev/null 2>&1

bluetoothctl devices
