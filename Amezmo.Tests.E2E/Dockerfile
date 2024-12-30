FROM mcr.microsoft.com/playwright/dotnet:v1.49.0-noble as base

ENV HEADLESS=true
ENV SLOWMO=0

ENV BASE_URL="https://www.amezmo.com"

ENV Credentials__username ""
ENV Credentials__password ""

WORKDIR /test/

COPY Amezmo.Tests.E2E Amezmo.Tests.E2E
COPY Amezmo.Tests.Library Amezmo.Tests.Library

RUN dotnet restore "Amezmo.Tests.E2E/Amezmo.Tests.E2E.csproj"
RUN dotnet build "Amezmo.Tests.E2E/Amezmo.Tests.E2E.csproj"

WORKDIR /test/Amezmo.Tests.E2E
ENTRYPOINT ["dotnet", "test", "-v", "normal"]