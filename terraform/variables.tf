##########################################################################
#  Variables - declare valid variables and optionally their types.       #
#  Define them in variable definitions (.tfvars) files.                  #
#  var/dev/dev.tfvars || var/prod/prod.tfvars || var/stage/stage.tfvars  # 
##########################################################################

# SECRET VARIABLES #
variable "app_sp_object_id" {
  type = string
  description = "Object id of service principal that is used to access keyvault keys from application"
}

variable "app_sp_client_id" {
  type = string
  description = "Object id of service principal that is used to access keyvault keys from application"
}

variable "app_sp_client_secret" {
  type = string
  description = "Object id of service principal that is used to access keyvault keys from application"
}

variable "admin_object_id" {
  type = string
  description = "Admin azure object id"
}

variable "sp_object_id" {
  type = string
  description = "Service principal azure object id"
}

variable "sql_admin_password" {
  type = string
  description = "Sql server admin password"
}

variable "telegram_bot_owner_telegram_id" {
  type = string
  description = "Sql server admin password"
}

variable "telegram_token" {
  type = string
  description = "Sql server admin password"
}

variable "payment_card_api_token" {
  type = string
  description = "Sql server admin password"
  default = ""
}

variable "payment_plicio_api_token" {
  type = string
  description = "Sql server admin password"
  default = ""
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

variable "telegram_webapp_name" {
  type = string
  description = "Telegram webapp name"
}

variable "container_registry_name" {
  type = string
  description = "Container registry name"
}

variable "keyvault_name" {
  type = string
  description = "App service name"
}