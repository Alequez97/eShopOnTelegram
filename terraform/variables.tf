##########################################################################
#  Variables - declare valid variables and optionally their types.       #
#  Define them in variable definitions (.tfvars) files.                  #
#  var/dev/dev.tfvars || var/prod/prod.tfvars || var/stage/stage.tfvars  # 
##########################################################################

# GENERAL VARIABLES #
variable "resource_group_name" {
  type = string
  description = "Resource group name"
}

variable "app_environment" {
  type = string
  description = "App environment"
}

variable "sql_server_name" {
  type = string
  description = "Sql server name"
}

variable "sql_database_name" {
  type = string
  description = "Sql database name"
}