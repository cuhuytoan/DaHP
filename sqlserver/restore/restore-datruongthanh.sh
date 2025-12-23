#!/usr/bin/env bash
set -euo pipefail

COMPOSE_FILE="${COMPOSE_FILE:-sqlserver/docker-compose.sqlserver.yml}"
SERVICE="${SERVICE:-mssqlserver}"
SA_PASSWORD="${MSSQL_SA_PASSWORD:-Toantom123!}"

SOURCE_BAK_IN_CONTAINER="${BAK_PATH_IN_CONTAINER:-/backup/DaTruongThanh_full.bak}"
TARGET_DB="${TARGET_DB:-DaTruongThanh}"

TARGET_DATA_FILE="${TARGET_DATA_FILE:-/var/opt/mssql/data/DaTruongThanh.mdf}"
TARGET_LOG_FILE="${TARGET_LOG_FILE:-/var/opt/mssql/data/DaTruongThanh_log.ldf}"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

"${SCRIPT_DIR}/wait-for-sqlserver.sh"

echo "Detecting logical file names from .bak (RESTORE FILELISTONLY)..."
FILELIST_RAW="$(
  docker compose -f "${COMPOSE_FILE}" exec -T "${SERVICE}" \
    /opt/mssql-tools18/bin/sqlcmd \
      -S localhost \
      -U sa \
      -P "${SA_PASSWORD}" \
      -b \
      -h -1 \
      -W \
      -s "|" \
      -Q "SET NOCOUNT ON; RESTORE FILELISTONLY FROM DISK = N'${SOURCE_BAK_IN_CONTAINER}';"
)"

if [[ -z "${FILELIST_RAW}" ]]; then
  echo "ERROR: FILELISTONLY returned no output. Is the .bak mounted at ${SOURCE_BAK_IN_CONTAINER}?"
  exit 1
fi

# FILELISTONLY columns include: LogicalName|PhysicalName|Type|...
LOGICAL_DATA="$(
  printf '%s\n' "${FILELIST_RAW}" \
    | awk -F'|' '{
        # trim whitespace around fields
        for (i=1; i<=NF; i++) { gsub(/^[ \t\r\n]+|[ \t\r\n]+$/, "", $i) }
        if ($3 == "D") { print $1; exit }
      }'
)"
LOGICAL_LOG="$(
  printf '%s\n' "${FILELIST_RAW}" \
    | awk -F'|' '{
        for (i=1; i<=NF; i++) { gsub(/^[ \t\r\n]+|[ \t\r\n]+$/, "", $i) }
        if ($3 == "L") { print $1; exit }
      }'
)"

if [[ -z "${LOGICAL_DATA}" || -z "${LOGICAL_LOG}" ]]; then
  echo "ERROR: Could not detect logical data/log names from FILELISTONLY output."
  echo
  echo "---- RAW FILELISTONLY OUTPUT (pipe-separated) ----"
  printf '%s\n' "${FILELIST_RAW}"
  echo "-------------------------------------------------"
  exit 1
fi

echo "Logical data file: ${LOGICAL_DATA}"
echo "Logical log file : ${LOGICAL_LOG}"
echo
echo "Restoring database [${TARGET_DB}] from ${SOURCE_BAK_IN_CONTAINER} ..."

RESTORE_SQL=$(
  cat <<SQL
RESTORE DATABASE [${TARGET_DB}]
FROM DISK = N'${SOURCE_BAK_IN_CONTAINER}'
WITH
  MOVE N'${LOGICAL_DATA}' TO N'${TARGET_DATA_FILE}',
  MOVE N'${LOGICAL_LOG}'  TO N'${TARGET_LOG_FILE}',
  REPLACE,
  STATS = 5;
SQL
)

docker compose -f "${COMPOSE_FILE}" exec -T "${SERVICE}" \
  /opt/mssql-tools18/bin/sqlcmd \
    -S localhost \
    -U sa \
    -P "${SA_PASSWORD}" \
    -b \
    -Q "${RESTORE_SQL}"

echo
echo "Restore completed. Quick check: listing databases..."
docker compose -f "${COMPOSE_FILE}" exec -T "${SERVICE}" \
  /opt/mssql-tools18/bin/sqlcmd \
    -S localhost \
    -U sa \
    -P "${SA_PASSWORD}" \
    -b \
    -Q "SELECT name FROM sys.databases ORDER BY name;"




