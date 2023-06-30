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

resource "azurerm_mssql_firewall_rule" "firewall" {
  name             = "AllowAccessToAzureServices"
  server_id        = azurerm_mssql_server.mssqlserver.id
  start_ip_address = "0.0.0.0"
  end_ip_address   = "0.0.0.0"
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
  name                              = var.storage_account_name
  resource_group_name               = azurerm_resource_group.rg.name
  location                          = azurerm_resource_group.rg.location
  account_tier                      = "Standard"
  account_replication_type          = "GRS"
  allow_nested_items_to_be_public   = true

  tags = local.az_common_tags
}

resource "azurerm_storage_container" "runtime_configuration_blob_storage" {
  name                  = "runtime-configuration"
  storage_account_name  = azurerm_storage_account.storageaccount.name
  container_access_type = "private"
}

resource "azurerm_storage_container" "product_images_blob_storage" {
  name                  = "product-images"
  storage_account_name  = azurerm_storage_account.storageaccount.name
  container_access_type = "container"
}

resource "azurerm_service_plan" "serviceplan" {
  name                = var.app_service_plan_name
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  os_type             = "Linux"
  sku_name            = "F1"

  tags                = local.az_common_tags
}

resource "azurerm_application_insights" "app_insights" {
  name                = var.app_insights_name
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  application_type    = "web"
  retention_in_days = 30
}

resource "azurerm_linux_web_app" "admin" {
  name                = var.admin_app_name
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_service_plan.serviceplan.location
  service_plan_id     = azurerm_service_plan.serviceplan.id
  https_only          = true

  site_config {
    always_on           = false
    minimum_tls_version = 1.2

    application_stack {
      dotnet_version = "7.0"
    }
  }

  app_settings = {
    "Azure__AppInsightsInstrumentationKey"         = azurerm_application_insights.app_insights.instrumentation_key
    "Azure__StorageAccountConnectionString"        = azurerm_storage_account.storageaccount.primary_connection_string
    "Azure__RuntimeConfigurationBlobContainerName" = azurerm_storage_container.runtime_configuration_blob_storage.name
    "Azure__ProductImagesBlobContainerName"        = azurerm_storage_container.product_images_blob_storage.name
  }

  connection_string {
    name  = "Sql"
    type  = "SQLServer"
    value = "Server=tcp:${azurerm_mssql_server.mssqlserver.name}.database.windows.net,1433;Initial Catalog=${azurerm_mssql_database.mssqldatabase.name};Persist Security Info=False;User ID=${azurerm_mssql_server.mssqlserver.administrator_login};Password=${azurerm_mssql_server.mssqlserver.administrator_login_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }

  tags = local.az_common_tags
}

resource "azurerm_linux_web_app" "telegramwebapp" {
  name                = var.telegram_webapp_name
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_service_plan.serviceplan.location
  service_plan_id     = azurerm_service_plan.serviceplan.id
  https_only          = true

  site_config {
    always_on = false
    minimum_tls_version = 1.2

    application_stack {
      dotnet_version ="7.0"
    }
  }

  app_settings = {
    "Azure__AppInsightsInstrumentationKey"  = azurerm_application_insights.app_insights.instrumentation_key
    "Azure__StorageAccountConnectionString" = azurerm_storage_account.storageaccount.primary_connection_string
    "Azure__ProductImagesBlobContainerName" = azurerm_storage_container.product_images_blob_storage.name
    "ProductImagesHostName"                 = "https://${azurerm_storage_account.storageaccount.name}.blob.core.windows.net/${azurerm_storage_container.product_images_blob_storage.name}"
  }
  
  connection_string {
    name  = "Sql"
    type  = "SQLServer"
    value = "Server=tcp:${azurerm_mssql_server.mssqlserver.name}.database.windows.net,1433;Initial Catalog=${azurerm_mssql_database.mssqldatabase.name};Persist Security Info=False;User ID=${azurerm_mssql_server.mssqlserver.administrator_login};Password=${azurerm_mssql_server.mssqlserver.administrator_login_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }

  tags = local.az_common_tags
}

output "admin_app_name" {
  value = azurerm_linux_web_app.admin.name
}

output "telegram_webapp_name" {
  value = azurerm_linux_web_app.telegramwebapp.name
}