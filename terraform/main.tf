data "azurerm_client_config" "eshopontelegram" {}

resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.resource_group_location

  tags = local.az_common_tags
}

resource "azurerm_mssql_server" "mssqlserver" {
  name                         = var.sql_server_name
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = azurerm_resource_group.rg.location
  version                      = "12.0"
  administrator_login          = "eshopontelegram"
  administrator_login_password = var.sql_admin_password

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
  sku_name            = var.app_service_plan_sku_name

  tags                = local.az_common_tags
}

resource "azurerm_log_analytics_workspace" "log_analytics_workspace" {
  name                = var.log_analytics_workspace_name
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  sku                 = "Free"
  retention_in_days   = 30
}

resource "azurerm_application_insights" "app_insights" {
  name                = var.app_insights_name
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  application_type    = "web"
  workspace_id        = azurerm_log_analytics_workspace.log_analytics_workspace.id
  retention_in_days = 30
}

resource "azurerm_linux_web_app" "admin" {
  name                = var.admin_app_name
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_service_plan.serviceplan.location
  service_plan_id     = azurerm_service_plan.serviceplan.id
  https_only          = true

  site_config {
    always_on           = var.app_service_plan_sku_name == "F1" ? false : true
    minimum_tls_version = 1.2

    application_stack {
      dotnet_version = "7.0"
    }
  }

  app_settings = {
    "Logging__LogLevel__Default"                   = "Information"
    "Logging__ApplicationInsights"                 = "Information"
    "Azure__KeyVaultUri"                           = "https://${var.keyvault_name}.vault.azure.net"
    "Azure__TenantId"                              = data.azurerm_client_config.eshopontelegram.tenant_id
    "Azure__ClientId"                              = var.app_sp_client_id
    "Azure__ClientSecret"                          = var.app_sp_client_secret
    "Azure__RuntimeConfigurationBlobContainerName" = azurerm_storage_container.runtime_configuration_blob_storage.name
    "Azure__ProductImagesBlobContainerName"        = azurerm_storage_container.product_images_blob_storage.name
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
    always_on = var.app_service_plan_sku_name == "F1" ? false : true
    minimum_tls_version = 1.2

    application_stack {
      dotnet_version ="7.0"
    }
  }

  app_settings = {
    "Logging__LogLevel__Default"            = "Information"
    "Logging__ApplicationInsights"          = "Information"
    "Azure__KeyVaultUri"                    = "https://${var.keyvault_name}.vault.azure.net"
    "Azure__TenantId"                       = data.azurerm_client_config.eshopontelegram.tenant_id
    "Azure__ClientId"                       = var.app_sp_client_id
    "Azure__ClientSecret"                   = var.app_sp_client_secret
    "Azure__ProductImagesBlobContainerName" = azurerm_storage_container.product_images_blob_storage.name
  }

  tags = local.az_common_tags
}

resource "azurerm_container_registry" "containerregistry" {
  name                     = var.container_registry_name
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  sku                      = "Basic"
  admin_enabled            = true

  tags = local.az_common_tags
}

resource "azurerm_key_vault" "keyvault" {
  name                            = var.keyvault_name
  location                        = azurerm_resource_group.rg.location
  resource_group_name             = azurerm_resource_group.rg.name
  tenant_id                       = data.azurerm_client_config.eshopontelegram.tenant_id
  soft_delete_retention_days      = "7"
  sku_name                        = "standard"

  depends_on   = [
    azurerm_linux_web_app.admin,
    azurerm_linux_web_app.telegramwebapp
  ]

  # App identity access to keyvault
  access_policy {
    tenant_id  =  data.azurerm_client_config.eshopontelegram.tenant_id
    object_id  =  var.app_sp_object_id

    secret_permissions = [
      "Get",
      "List",
    ]
  }

  # Azure admin access to keyvault
  access_policy {
    tenant_id = data.azurerm_client_config.eshopontelegram.tenant_id
    object_id = var.admin_object_id

    secret_permissions = [
      "Get",
      "List",
      "Set",
      "Delete",
      "Purge",
      "Recover",
      "Restore"
    ]
  }

  # Github service principal access to keyvault
  access_policy {
    tenant_id = data.azurerm_client_config.eshopontelegram.tenant_id
    object_id = var.sp_object_id

    secret_permissions = [
      "Get",
      "List",
      "Set",
      "Delete",
      "Purge",
      "Recover",
      "Restore"
    ]
  }

  tags = local.az_common_tags
}

resource "azurerm_key_vault_secret" "sqlconnectionstring" {
  name         = "ConnectionStrings--Sql"
  value        = "Server=tcp:${azurerm_mssql_server.mssqlserver.name}.database.windows.net,1433;Initial Catalog=${azurerm_mssql_database.mssqldatabase.name};Persist Security Info=False;User ID=${azurerm_mssql_server.mssqlserver.administrator_login};Password=${azurerm_mssql_server.mssqlserver.administrator_login_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "storageaccountconnectionstring" {
  name         = "Azure--StorageAccountConnectionString"
  value        = azurerm_storage_account.storageaccount.primary_connection_string
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "appinsightsconnectionstring" {
  name         = "Azure--AppInsightsConnectionString"
  value        = "InstrumentationKey=${azurerm_application_insights.app_insights.instrumentation_key}"
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "productimageshostname" {
  name         = "ProductImagesHostName"
  value        = "https://${azurerm_storage_account.storageaccount.name}.blob.core.windows.net/${azurerm_storage_container.product_images_blob_storage.name}"
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "telegramownerid" {
  name         = "Telegram--BotOwnerTelegramId"
  value        = var.telegram_bot_owner_telegram_id
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "telegramtoken" {
  name         = "Telegram--Token"
  value        = var.telegram_token
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "telegramwebappurl" {
  name         = "Telegram--WebAppUrl"
  value        = "https://${azurerm_linux_web_app.telegramwebapp.name}.azurewebsites.net"
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "paymentcurrency" {
  name         = "Payment--MainCurrency"
  value        = "EUR"
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "cardpaymentenabled" {
  name         = "Payment--Card--Enabled"
  value        = var.payment_card_api_token == "" ? "false" : "true"
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "cardpaymentapitoken" {
  name         = "Payment--Card--ApiToken"
  value        = var.payment_card_api_token
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "pliciopaymentenabled" {
  name         = "Payment--Plisio--Enabled"
  value        = var.payment_plicio_api_token == "" ? "false" : "true"
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "pliciopaymentapitoken" {
  name         = "Payment--Plisio--ApiToken"
  value        = var.payment_plicio_api_token
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "pliciopaymentcryptocurrency" {
  name         = "Payment--Plisio--CryptoCurrency"
  value        = "BTC"
  key_vault_id = azurerm_key_vault.keyvault.id
}

output "resource_group_name" {
  value = azurerm_resource_group.rg.name
}

output "resource_group_location" {
  value = azurerm_resource_group.rg.location
}

output "admin_app_name" {
  value = azurerm_linux_web_app.admin.name
}

output "admin_app_hostname" {
  value = "https://${azurerm_linux_web_app.admin.name}.azurewebsites.net"
}

output "telegram_webapp_name" {
  value = azurerm_linux_web_app.telegramwebapp.name
}

output "acr_login_server" {
  value = azurerm_container_registry.containerregistry.login_server
}

output "acr_username" {
  value = azurerm_container_registry.containerregistry.admin_username
}

output "acr_password" {
  value = azurerm_container_registry.containerregistry.admin_password
  sensitive = true
}

output "key_vault_uri" {
  value = "https://${azurerm_key_vault.keyvault.name}.vault.azure.net"
}