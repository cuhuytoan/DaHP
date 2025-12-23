#!/bin/bash

# Script to start backend API with SSH tunnel to Postgres server
# Usage: ./scripts/start-backend-with-tunnel.sh

set -e

SSH_HOST="itadmin@chatbot2.ptha.io.vn"
SSH_PORT="28022"
SSH_PASS="Thanhcong@123"
LOCAL_PORT="15433"
REMOTE_PORT="15433"

echo "=== Setting up SSH tunnel for Postgres ==="

# Kill existing tunnel if any
pkill -f "ssh.*$LOCAL_PORT:$SSH_HOST" || true
sleep 1

# Create SSH tunnel
echo "Creating SSH tunnel: localhost:$LOCAL_PORT -> $SSH_HOST:$REMOTE_PORT"
sshpass -p "$SSH_PASS" ssh -p $SSH_PORT -L $LOCAL_PORT:localhost:$REMOTE_PORT -N -f $SSH_HOST

# Wait for tunnel to establish
sleep 2

# Test connection
echo "Testing database connection..."
if PGPASSWORD='vegax2025' psql -h localhost -p $LOCAL_PORT -U postgres -d DaTruongThanh -c "SELECT 1;" > /dev/null 2>&1; then
    echo "✓ Database connection successful!"
else
    echo "✗ Database connection failed!"
    exit 1
fi

echo ""
echo "=== Starting Backend API ==="
echo "API will be available at: http://localhost:5000"
echo "Swagger UI: http://localhost:5000/swagger"
echo ""
echo "Press Ctrl+C to stop API and tunnel"
echo ""

# Start backend API
cd "$(dirname "$0")/../backend/CMS.API"
dotnet run --urls "http://localhost:5000"

# Cleanup tunnel on exit
trap "pkill -f 'ssh.*$LOCAL_PORT:$SSH_HOST'; echo 'SSH tunnel closed'" EXIT



