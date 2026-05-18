#!/usr/bin/env bash
set -e

echo "Cleaning up AutoVerdikt local environment..."
# Clean docker resources
# docker-compose down -v --remove-orphans

# Clean temporary build and package files
find . -type d -name "node_modules" -prune -exec rm -rf {} +
find . -type d -name "dist" -prune -exec rm -rf {} +
find . -type d -name ".next" -prune -exec rm -rf {} +
find . -type d -name "__pycache__" -prune -exec rm -rf {} +
find . -type d -name "bin" -prune -exec rm -rf {} +
find . -type d -name "obj" -prune -exec rm -rf {} +

echo "Cleanup complete."
