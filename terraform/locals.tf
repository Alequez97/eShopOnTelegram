locals {
  az_common_tags = {
    Environment =  "${var.app_environment}"
    Team        = "eShopOnTelegram"
    Responsible = "aleksandrs@vaguscenko.lv"
  }
}