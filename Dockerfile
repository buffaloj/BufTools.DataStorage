FROM mcr.microsoft.com/mssql/server
RUN mkdir -p /var/opt/mssql/seed

COPY create_test_db.sql /var/opt/mssql/seed

ENV MSSQL_SA_PASSWORD=change_this_password
ENV ACCEPT_EULA=true
ENV PORT=1433

RUN /opt/mssql/bin/sqlservr & sleep 60; /opt/mssql-tools/bin/sqlcmd -U sa -P change_this_password -i /var/opt/mssql/seed/create_test_db.sql