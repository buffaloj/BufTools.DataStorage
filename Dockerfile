

FROM mcr.microsoft.com/mssql/server
RUN mkdir -p /var/opt/mssql/seed

COPY create_test_db.sql /var/opt/mssql/seed

ENV MSSQL_SA_PASSWORD=ght%543#kjhkjh&
ENV ACCEPT_EULA=true
ENV PORT=1433

EXPOSE 1433

RUN /opt/mssql/bin/sqlservr & sleep 60; /opt/mssql-tools/bin/sqlcmd -U sa -P ght%543#kjhkjh& -i /var/opt/mssql/seed/create_test_db.sql

