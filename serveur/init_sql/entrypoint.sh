#!/bin/bash

/opt/mssql/bin/sqlservr --accept-eula --pid $MSSQL_PID &

sleep 10

# Run the SQL script to initialize the database
echo "Initializing database..."
/opt/mssql-tools18/bin/sqlcmd -S localhost -U $MSSQL_USER -P $MSSQL_SA_PASSWORD -i /docker-entrypoint-initdb.d/init.sql -C
/opt/mssql-tools18/bin/sqlcmd -S localhost -U $MSSQL_USER -P $MSSQL_SA_PASSWORD -d $MSSQL_DATABASE -i /docker-entrypoint-initdb.d/init_donnees.sql -C

# Keep the container running
tail -f /dev/null