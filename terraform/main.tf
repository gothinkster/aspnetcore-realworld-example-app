provider "azurerm" {
  version = "2.10.0"
  features {}
}

resource "azurerm_resource_group" "resource_group" {
  name     = "github-workshop-${var.name}"
  location = var.location
}
