# How to create new environment

1. Create new keyvault in azure resource group with name rg-eshopontelegram-common. Taking into account, that azure limitations allow only 24 characters in key vault name keyvault should be called using naming convention 'kv-es-common-xx-env' where 'xx' are client initials and 'env' is environment name - dev, stage, prod etc.

2. After key vault is deployed - open Access configuration tab and set key vault permission model to Vault access policy.

3. Go to access policies and add Get, List to 'azure-devops-sp' service principal. Add policies for yourself as well to create new secrets.

4. Create all required secrete. Example what keys are required can be found in 'kv-es-common-dev' keyvault.

5. Create terraform variables file inside infrastructure/terraform/vars. For each client create new folder. Each client folder will containt his enviroments terraform variables.

6. Create Azure DevOps pipeline environemt using naming convention 'eshopontelegram-xx-env' where xx are client initials and env is environment name.

7. For running deployment apply change pipeline parameters. 'env' parameter should reflect terraform variables relative path. For example, if inside terraform vars folder you have xx folder with file name prod.tfvars, then use xx/prod for env parameter. Pipeline environment use based on created name in azure.

8. Run deploy. In case of error troubleshoot and fix them