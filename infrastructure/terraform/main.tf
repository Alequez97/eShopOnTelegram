data "azurerm_key_vault" "kv_eshopontelegram_common" {
  name                = var.eshopontelegram_common_kv_name
  resource_group_name = "rg-eshopontelegram-common"
}

resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.resource_group_location

  tags = local.az_common_tags
}

data "azurerm_key_vault_secret" "common_kv_mssql_database_password" {
  name         = "sql-admin-password"
  key_vault_id = data.azurerm_key_vault.kv_eshopontelegram_common.id
}

resource "azurerm_mssql_server" "mssqlserver" {
  name                         = var.sql_server_name
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = azurerm_resource_group.rg.location
  version                      = "12.0"
  administrator_login          = "eshopontelegram"
  administrator_login_password = data.azurerm_key_vault_secret.common_kv_mssql_database_password.value

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
  retention_in_days   = 30
}

resource "azurerm_application_insights" "app_insights" {
  name                = var.app_insights_name
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  workspace_id        = azurerm_log_analytics_workspace.log_analytics_workspace.id
  application_type    = "web"
}

resource "azurerm_linux_web_app" "admin" {
  name                = var.admin_app_name
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_service_plan.serviceplan.location
  service_plan_id     = azurerm_service_plan.serviceplan.id
  https_only          = true

  site_config {
    always_on           = var.app_service_plan_sku_name == "F1" ? false : true
    app_command_line    = "dotnet eShopOnTelegram.Admin.dll"
    minimum_tls_version = 1.2

    application_stack {
      dotnet_version = "7.0"
    }
  }

  app_settings = {
    "Logging__LogLevel__Default"                                        = "Information"
    "Logging__ApplicationInsights"                                      = "Information"
    "AppSettings__AzureSettings__KeyVaultUri"                           = "https://${var.keyvault_name}.vault.azure.net"
    "AppSettings__AzureSettings__TenantId"                              = var.azure_spn_tenant_id
    "AppSettings__AzureSettings__ClientId"                              = var.azure_spn_client_id
    "AppSettings__AzureSettings__ClientSecret"                          = var.azure_spn_client_secret
    "AppSettings__AzureSettings__RuntimeConfigurationBlobContainerName" = azurerm_storage_container.runtime_configuration_blob_storage.name
    "AppSettings__AzureSettings__ProductImagesBlobContainerName"        = azurerm_storage_container.product_images_blob_storage.name
    "AppSettings__AzureSettings__ResourceGroupName"                     = var.resource_group_name
    "AppSettings__AzureSettings__ShopAppServiceName"                    = var.shop_app_name
    "AppSettings__JWTAuthSettings__Issuer"                              = var.admin_app_name
    "AppSettings__JWTAuthSettings__Audience"                            = var.admin_app_name
    "AppSettings__JWTAuthSettings__RefreshTokenLifetimeMinutes"         = 10080 // 1 week
    "AppSettings__JWTAuthSettings__JTokenLifetimeMinutes"               = 5
    "AppSettings__Language"                                             = var.language
    "AppSettings__ProductImagesHostName"                                = "https://${azurerm_storage_account.storageaccount.name}.blob.core.windows.net/${azurerm_storage_container.product_images_blob_storage.name}"
  }

  tags = local.az_common_tags
}

resource "azurerm_linux_web_app" "shop" {
  name                = var.shop_app_name
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_service_plan.serviceplan.location
  service_plan_id     = azurerm_service_plan.serviceplan.id
  https_only          = true

  site_config {
    always_on           = var.app_service_plan_sku_name == "F1" ? false : true
    app_command_line    = "dotnet eShopOnTelegram.Shop.dll"
    minimum_tls_version = 1.2

    application_stack {
      dotnet_version = "7.0"
    }
  }

  app_settings = {
    "Logging__LogLevel__Default"                                 = "Information"
    "Logging__ApplicationInsights"                               = "Information"
    "AppSettings__AzureSettings__KeyVaultUri"                    = "https://${var.keyvault_name}.vault.azure.net"
    "AppSettings__AzureSettings__TenantId"                       = var.azure_spn_tenant_id
    "AppSettings__AzureSettings__ClientId"                       = var.azure_spn_client_id
    "AppSettings__AzureSettings__ClientSecret"                   = var.azure_spn_client_secret
    "AppSettings__AzureSettings__ProductImagesBlobContainerName" = azurerm_storage_container.product_images_blob_storage.name
    "AppSettings__PaymentSettings__MainCurrency"                 = var.currency
    "AppSettings__TelegramBotSettings__WebAppUrl"                = "https://${var.shop_app_name}.azurewebsites.net"
    "AppSettings__TelegramBotSettings__ShopLayout"               = var.telegram_shop_layout
		"AppSettings__Language"                                      = var.language
    "AppSettings__AdminAppHostName"                              = "https://${azurerm_linux_web_app.admin.name}.azurewebsites.net"
    "AppSettings__ProductImagesHostName"                         = "https://${azurerm_storage_account.storageaccount.name}.blob.core.windows.net/${azurerm_storage_container.product_images_blob_storage.name}"
  }

  tags = local.az_common_tags
}

resource "azurerm_key_vault" "keyvault" {
  name                            = var.keyvault_name
  location                        = azurerm_resource_group.rg.location
  resource_group_name             = azurerm_resource_group.rg.name
  tenant_id                       = var.azure_spn_tenant_id
  soft_delete_retention_days      = "7"
  sku_name                        = "standard"

  depends_on   = [
    azurerm_linux_web_app.admin,
    azurerm_linux_web_app.shop
  ]

  # App identity access to keyvault
  # Same service principal uses terraform
  access_policy {
    tenant_id  =  var.azure_spn_tenant_id
    object_id  =  var.azure_spn_object_id

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

  # Aleksandrs Vaguscenko
  access_policy {
    tenant_id  =  var.azure_spn_tenant_id
    object_id  =  "aacc76fd-4210-4008-bb76-ba5869439d38"

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

data "azurerm_key_vault_secret" "common_kv_jwt_key" {
  name         = "jwt-key"
  key_vault_id = data.azurerm_key_vault.kv_eshopontelegram_common.id
}

resource "azurerm_key_vault_secret" "jwt_key" {
  name         = "AppSettings--JWTAuthSettings--Key"
  value        = data.azurerm_key_vault_secret.common_kv_jwt_key.value
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "sqlconnectionstring" {
  name         = "ConnectionStrings--Sql"
  value        = "Server=tcp:${azurerm_mssql_server.mssqlserver.name}.database.windows.net,1433;Initial Catalog=${azurerm_mssql_database.mssqldatabase.name};Persist Security Info=False;User ID=${azurerm_mssql_server.mssqlserver.administrator_login};Password=${azurerm_mssql_server.mssqlserver.administrator_login_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "storageaccountconnectionstring" {
  name         = "AppSettings--AzureSettings--StorageAccountConnectionString"
  value        = azurerm_storage_account.storageaccount.primary_connection_string
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "appinsightsconnectionstring" {
  name         = "AppSettings--AzureSettings--AppInsightsConnectionString"
  value        = "InstrumentationKey=${azurerm_application_insights.app_insights.instrumentation_key}"
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "pliciopaymentcryptocurrency" {
  count        = var.crypto_currency == null ? 0 : 1
  name         = "AppSettings--PaymentSettings--Plisio--CryptoCurrency"
  value        = var.crypto_currency
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

output "shop_app_name" {
  value = azurerm_linux_web_app.shop.name
}

output "key_vault_uri" {
  value = "https://${azurerm_key_vault.keyvault.name}.vault.azure.net"
}