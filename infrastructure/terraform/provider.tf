################################
## Azure Provider and Backend ##
################################
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.62.0"
    }
  }

  backend "azurerm" {
      subscription_id = "349526ee-2859-455b-8a10-1ffc02973465" // sub for terraform backend config.
  }

  required_version = ">= 1.3.3"
}

variable "azure_subscription_id" {
  description = "Subscription ID in which terraform will act"
}

provider "azurerm" {
  features {
    resource_group {
      prevent_deletion_if_contains_resources = true
    }
  }
  
  subscription_id            = var.azure_subscription_id
  client_id                  = var.azure_spn_client_id
  client_secret              = var.azure_spn_client_secret
  tenant_id                  = var.azure_spn_tenant_id
  skip_provider_registration = "true"
}
