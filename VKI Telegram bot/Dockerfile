FROM mcr.microsoft.com/dotnet/runtime:6.0
RUN mkdir /app
WORKDIR /app
COPY . /app
ENV TELEGRAM_TOKEN 1295431345:AAGYesCdzZxET-n4B_PPhAeXVPCA-m01DZE
ENV UPDATER_WAIT 1800000
CMD ["dotnet", "VKI Telegram bot.dll"]