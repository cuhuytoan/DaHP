#!/usr/bin/env bash
set -euo pipefail

COMPOSE_FILE="${COMPOSE_FILE:-sqlserver/docker-compose.sqlserver.yml}"
SERVICE="${SERVICE:-mssqlserver}"
BAK_PATH_IN_CONTAINER="${BAK_PATH_IN_CONTAINER:-/backup/DaTruongThanh_full.bak}"
SA_PASSWORD="${MSSQL_SA_PASSWORD:-Toantom123!}"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

"${SCRIPT_DIR}/wait-for-sqlserver.sh"

echo "Running RESTORE FILELISTONLY on ${BAK_PATH_IN_CONTAINER} ..."
docker compose -f "${COMPOSE_FILE}" exec -T "${SERVICE}" \
  /opt/mssql-tools18/bin/sqlcmd \
    -S localhost \
    -U sa \
    -P "${SA_PASSWORD}" \
    -b \
    -Q "RESTORE FILELISTONLY FROM DISK = N'${BAK_PATH_IN_CONTAINER}';"




