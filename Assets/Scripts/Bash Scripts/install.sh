#!/bin/bash

TARGET_DIR="/usr/local/bin"

echo "Installing system scripts to $TARGET_DIR"

if [ "$EUID" -ne 0 ]; then
    echo "Please run with sudo:"
    echo "sudo ./install.sh"
    exit 1
fi

SCRIPTS=(wifi*.sh bluetooth*.sh audio*.sh)

FOUND=0

for pattern in "${SCRIPTS[@]}"; do
    for file in $pattern; do
        if [ -f "$file" ]; then
            cp "$file" "$TARGET_DIR/"
            chmod +x "$TARGET_DIR/$file"
            echo "Installed $file"
            FOUND=1
        fi
    done
done

if [ "$FOUND" -eq 0 ]; then
    echo "No matching scripts found."
    exit 1
fi

echo "Installation complete."
