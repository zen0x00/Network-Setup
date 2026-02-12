#!/bin/bash

CURRENT=$(pactl get-default-sink)

pactl -f json list sinks | jq -r --arg CUR "$CURRENT" \
'.[] | select(.name == $CUR) | .description'
