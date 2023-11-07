##########################################################################
#  Variables - declare valid variables and optionally their types.       #
#  Define them in variable definitions (.tfvars) files.                  #
#  var/dev/dev.tfvars || var/prod/prod.tfvars || var/stage/stage.tfvars  # 
##########################################################################

# SECRET VARIABLES #
variable "azure_spn_tenant_id" {
  type = string
  description = "Service principal tenant id that is used by terraform"
}

variable "azure_spn_object_id" {
  type = string
  description = "Service principal that is used by terraform"
}

variable "azure_spn_client_id" {
  type = string
  description = "Service principal that is used by terraform"
}

variable "azure_spn_client_secret" {
  type = string
  description = "Service principal that is used by terraform"
}

variable "eshopontelegram_common_kv_name" {
  type = string
  description = "Keyvault where initial deployment keys will be stored, like passwords, api keys and so on"
}

# INFRASTRUCTURE VARIABLES #
variable "app_environment" {
  type = string
  description = "App environment"
}

variable "resource_group_name" {
  type = string
  description = "Resource group name"
}

variable "resource_group_location" {
  type = string
  description = "Resource group location"
}

variable "sql_server_name" {
  type = string
  description = "Sql server name"
}

variable "sql_database_name" {
  type = string
  description = "Sql database name"
}

variable "storage_account_name" {
  type = string
  description = "Storage account name"
}

variable "app_service_plan_name" {
  type = string
  description = "App service plan name"
}

variable "app_service_plan_sku_name" {
  type = string
  description = "App service sku name"
}

variable "log_analytics_workspace_name" {
  type = string
  description = "Log analytics workspace name"
}

variable "app_insights_name" {
  type = string
  description = "Admin application insights"
}

variable "admin_app_name" {
  type = string
  description = "Admin application name"
}

variable "shop_app_name" {
  type = string
  description = "Telegram bot project name"
}

variable "keyvault_name" {
  type = string
  description = "App service name"
}