##### AZ RESOURCES DOCU ####
## https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/
data "azurerm_client_config" "ui" {}

resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = "northeurope"
}