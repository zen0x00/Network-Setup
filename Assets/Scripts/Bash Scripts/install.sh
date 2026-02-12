#!/bin/bash

TARGET_DIR="/usr/local/bin"

echo "Starting installation..."

if [ "$EUID" -ne 0 ]; then
    echo "Please run with sudo:"
    echo "sudo ./install.sh"
    exit 1
fi

install_packages() {
    if command -v pacman &> /dev/null; then
        echo "Arch detected"
        pacman -S --needed --noconfirm jq bluez bluez-utils

    elif command -v apt &> /dev/null; then
        echo "Debian/Ubuntu detected"
        apt update
        apt install -y jq bluez bluez-tools

    elif command -v dnf &> /dev/null; then
        echo "Fedora detected"
        dnf install -y jq bluez bluez-tools

    elif command -v xbps-install &> /dev/null; then
        echo "Void detected"
        xbps-install -Sy jq bluez

    elif command -v emerge &> /dev/null; then
        echo "Gentoo detected"
        emerge --ask=n app-misc/jq net-wireless/bluez

    else
        echo "Unsupported distro. Please install jq and bluez manually."
    fi
}

enable_bluetooth() {
    if command -v systemctl &> /dev/null; then
        systemctl enable bluetooth
        systemctl start bluetooth
    else
        echo "systemctl not found. Enable bluetooth service manually."
    fi
}

install_packages
enable_bluetooth

# Copy scripts
SCRIPTS=(wifi*.sh bluetooth*.sh audio*.sh power*.sh)

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
