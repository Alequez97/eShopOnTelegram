##### AZ RESOURCES DOCU ####
## https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/
data "azurerm_client_config" "ui" {}

resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = "northeurope"
}

resource "azurerm_mssql_server" "mssqlserver" {
  name                         = var.sql_server_name
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = azurerm_resource_group.rg.location
  version                      = "12.0"
  administrator_login          = "aleksandrs"
  administrator_login_password = "4-v3ry-53cr37-p455w0rd"
}

resource "azurerm_mssql_database" "mssqldatabase" {
  name           = var.sql_database_name
  server_id      = azurerm_mssql_server.mssqlserver.id
  collation      = "SQL_Latin1_General_CP1_CI_AS"
  ledger_enabled = false
  license_type   = "BasePrice"
  sku_name       = "Basic"

  tags = {
    environment = var.app_environment
  }
}