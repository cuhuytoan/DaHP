#!/usr/bin/env bash
set -euo pipefail

COMPOSE_FILE="${COMPOSE_FILE:-sqlserver/docker-compose.sqlserver.yml}"
SERVICE="${SERVICE:-mssqlserver}"
SA_PASSWORD="${MSSQL_SA_PASSWORD:-Toantom123!}"
TRIES="${TRIES:-60}"
SLEEP_SECS="${SLEEP_SECS:-2}"

echo "Waiting for SQL Server (${SERVICE}) to be ready..."
for i in $(seq 1 "${TRIES}"); do
  if docker compose -f "${COMPOSE_FILE}" exec -T "${SERVICE}" \
    /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "${SA_PASSWORD}" -Q "SELECT 1" -b -o /dev/null 2>/dev/null; then
    echo "SQL Server is ready."
    exit 0
  fi
  echo "  not ready yet (${i}/${TRIES})... sleeping ${SLEEP_SECS}s"
  sleep "${SLEEP_SECS}"
done

echo "ERROR: SQL Server did not become ready in time."
exit 1




