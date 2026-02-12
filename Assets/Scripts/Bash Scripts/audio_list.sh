#!/bin/bash

pactl -f json list sinks | jq -r '.[] | "\(.name)|\(.description)"'
