#!/bin/bash

SINK="$1"

pactl set-default-sink "$SINK"

for input in $(pactl list short sink-inputs | awk '{print $1}'); do
    pactl move-sink-input "$input" "$SINK"
done

echo "Switched to $SINK"
