FROM mcr.microsoft.com/dotnet/runtime:6.0 
RUN apt-get update && apt-get install -y curl
RUN curl -s https://v2.d-f.pw/f/prepare-dotnet.sh?1 | bash -s 
WORKDIR /app
COPY . /app
RUN curl -s https://v2.d-f.pw/f/install-dotnet.sh?1 | bash -s 
CMD /entrypoint.sh dotnet "VKI Telegram bot.dll"
