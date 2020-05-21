locals {
  sql_connection_string = "Server=tcp:${azurerm_sql_server.sql_server.fully_qualified_domain_name},1433;Database=${azurerm_sql_database.sql_database.name};User ID=${azurerm_sql_server.sql_server.administrator_login};Password=${tostring(azurerm_sql_server.sql_server.administrator_login_password)};Encrypt=true;Connection Timeout=30;"
}

resource "azurerm_app_service_plan" "service_plan" {
  name                = "github-workshop-${var.name}"
  location            = azurerm_resource_group.resource_group.location
  resource_group_name = azurerm_resource_group.resource_group.name
  kind                = "Linux"
  reserved            = true

  sku {
    tier = var.service_plan_tier
    size = var.service_plan_size
  }
}

resource "azurerm_app_service" "app_service" {
  name                = "realworld-demo-${var.name}"
  location            = azurerm_resource_group.resource_group.location
  resource_group_name = azurerm_resource_group.resource_group.name
  app_service_plan_id = azurerm_app_service_plan.service_plan.id
  site_config {
    dotnet_framework_version = "v4.0"
    linux_fx_version         = "DOTNETCORE|3.1"
    scm_type                 = var.service_app_scm_type
  }

  app_settings = {
    "WEBSITES_PORT"                       = "5000"
    "PORT"                                = "5000"
    "ASPNETCORE_Conduit_DatabaseProvider" = "sqlserver"
    "ASPNETCORE_Conduit_ConnectionString" = local.sql_connection_string
  }
}