#!/bin/bash

sudo systemctl stop eShopOnTelegram.TelegramBot

git pull
rm -rf /srv/eShopOnTelegram.TelegramBot
mkdir /srv/eShopOnTelegram.TelegramBot
chown root /srv/eShopOnTelegram.TelegramBot
cd ..
dotnet publish -c Release -o /srv/eShopOnTelegram.TelegramBot

cd Systemd
sudo cp eShopOnTelegram.TelegramBot.service /etc/systemd/system/eShopOnTelegram.TelegramBot.service
sudo systemctl daemon-reload
echo "run sudo systemctl start eShopOnTelegram.TelegramBot to start service"