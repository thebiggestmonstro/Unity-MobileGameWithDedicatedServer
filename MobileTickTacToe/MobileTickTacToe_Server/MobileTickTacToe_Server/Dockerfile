# 1. SDK 이미지를 9.0으로 변경
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

# 2. 프로젝트 파일 복사 (현재 구조 유지)
COPY ["MobileTickTacToe_Server/MobileTickTacToe_Server.csproj", "MobileTickTacToe_Server/"]

# 3. restore 실행
RUN dotnet restore "MobileTickTacToe_Server/MobileTickTacToe_Server.csproj"

# 4. 소스 코드 복사 및 빌드
COPY . .
WORKDIR "/app/MobileTickTacToe_Server"
RUN dotnet publish "MobileTickTacToe_Server.csproj" -c Release -o /app/out

# 5. 실행용 베이스 이미지도 9.0으로 변경
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
EXPOSE 9050/udp
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "MobileTickTacToe_Server.dll" ]