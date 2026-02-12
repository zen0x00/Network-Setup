#!/bin/bash

MAC=$1

# First try direct connect
OUTPUT=$(bluetoothctl connect $MAC 2>&1)

if echo "$OUTPUT" | grep -q "Connection successful" || echo "$OUTPUT" | grep -q "already connected"; then
    echo "Connected"
    exit 0
fi

# If direct connect failed, try full pairing flow
OUTPUT=$(bluetoothctl --timeout 20 <<EOF
power on
agent on
default-agent
pair $MAC
trust $MAC
connect $MAC
EOF
)

if echo "$OUTPUT" | grep -q "Connection successful"; then
    echo "Connected"
else
    echo "Connection Failed"
fi
