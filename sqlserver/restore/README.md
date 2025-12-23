## SQL Server restore (DaTruongThanh_full.bak → SQL Server container)

This folder provides a **Docker-only** restore flow for the backup file `sqlserver/DaTruongThanh_full.bak`.

### Prerequisites

- Docker + `docker compose`

### 1) Start temporary SQL Server

From repo root:

```bash
# Optional: choose your own password (must satisfy SQL Server complexity rules)
export MSSQL_SA_PASSWORD='YourStr0ngP@ss!'

# Start SQL Server (port defaults to 11433)
docker compose -f sqlserver/docker-compose.sqlserver.yml up -d
```

### 2) Inspect logical file names (FILELISTONLY)

```bash
chmod +x sqlserver/restore/*.sh
./sqlserver/restore/filelistonly.sh
```

### 3) Restore database as `DaTruongThanh`

This script runs `RESTORE FILELISTONLY` internally, detects the first `D` (data) and `L` (log) logical files, and then runs `RESTORE DATABASE ... WITH MOVE ...`.

```bash
./sqlserver/restore/restore-datruongthanh.sh
```

### 4) Connection info

- **Host**: `localhost`
- **Port**: `11433` (or `MSSQL_PORT` if you changed it)
- **User**: `sa`
- **Password**: `$MSSQL_SA_PASSWORD`
- **Database**: `DaTruongThanh`

### 5) Clean up

```bash
docker compose -f sqlserver/docker-compose.sqlserver.yml down
# Optional: remove the restored SQL Server data volume
docker volume rm dahp_mssql_restore_data 2>/dev/null || true
```




