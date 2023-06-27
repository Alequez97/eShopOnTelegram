##### AZ RESOURCES DOCU ####
## https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/
data "azurerm_client_config" "ui" {}

resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = "northeurope"

  tags = local.az_common_tags
}

resource "azurerm_mssql_server" "mssqlserver" {
  name                         = var.sql_server_name
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = azurerm_resource_group.rg.location
  version                      = "12.0"
  administrator_login          = "aleksandrs"
  administrator_login_password = "4-v3ry-53cr37-p455w0rd"

  tags = local.az_common_tags
}

resource "azurerm_mssql_database" "mssqldatabase" {
  name           = var.sql_database_name
  server_id      = azurerm_mssql_server.mssqlserver.id
  collation      = "SQL_Latin1_General_CP1_CI_AS"
  ledger_enabled = false
  license_type   = "BasePrice"
  sku_name       = "Basic"

  tags = local.az_common_tags
}

resource "azurerm_storage_account" "storageaccount" {
  name                     = var.storage_account_name
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "GRS"
  allow_nested_items_to_be_public   = true

  tags = local.az_common_tags
}

resource "azurerm_service_plan" "serviceplan" {
  name                = var.app_service_plan_name
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  os_type             = "Linux"
  sku_name            = "F1"

  tags                = local.az_common_tags
}

resource "azurerm_linux_web_app" "admin" {
  name                = var.admin_app_name
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_service_plan.serviceplan.location
  service_plan_id     = azurerm_service_plan.serviceplan.id
  https_only          = true

  site_config {
    always_on = false
    minimum_tls_version = 1.2

    application_stack {
      dotnet_version ="6.0"
    }
  }

  app_settings = {
    "Azure__StorageAccountConnectionString" = azurerm_storage_account.storageaccount.primary_connection_string
  }

  tags = local.az_common_tags
}