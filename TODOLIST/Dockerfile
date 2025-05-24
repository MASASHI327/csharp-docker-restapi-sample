
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG WAIT_FOR_HOST
ARG WAIT_FOR_PORT

ENV WAIT_FOR_HOST=${WAIT_FOR_HOST}
ENV WAIT_FOR_PORT=${WAIT_FOR_PORT}

WORKDIR /src
COPY . .
RUN dotnet restore "TODOLIST.csproj"
RUN dotnet publish "TODOLIST.csproj" -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .


ENTRYPOINT ["sh", "-c", "wait-for-it $WAIT_FOR_HOST:$WAIT_FOR_PORT -- dotnet TODOLIST.dll"]