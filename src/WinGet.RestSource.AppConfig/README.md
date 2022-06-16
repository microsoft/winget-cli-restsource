# WinGet.RestSource Feature Flags.

This project uses Azure App Configuration to enable features.
For more information see https://docs.microsoft.com/en-us/azure/azure-app-configuration/overview

## How to add a new feature flag
1. Add the new flag in the appcofig.json file of the WinGet.RestSource.Infrastructure project.
``` 
...
    "parameters": {
        ...
        "newFeature": {
            "type": "bool",
            "defaultValue": false,
            "metadata": {
                "description": "Example."
            }
        },
        ...
    },
    "variables": {
        ...
        "newFeatureValue": {
            "id": "NewFeature",
            "description": "Example value.",
            "enabled": "[parameters('newFeature')]",
            "conditions": {"client_filters":[]}
        }
        ...
    },
    "resources": [
        {
            ...
            "resources": [
                ...
                {
                    "name": "[concat('.appconfig.featureflag~2F', variables('newFeatureValue').id)]",
                    "type": "keyValues",
                    "apiVersion": "2020-07-01-preview",
                    "dependsOn": [
                        "[parameters('appConfigName')]"
                    ],
                    "properties": {
                        "value": "[string(variables('newFeatureValue'))]",
                        "contentType": "application/vnd.microsoft.appconfig.ff+json;charset=utf-8"
                    }
                },
                ...
            ]
        }
    ]
}
```
2. Add a new value in the FeatureFlag enum in FeatureFlag.cs of the WinGet.RestSource.FeatureFlags project. The name *MUST* match the id value of the new variable defined in appconfig.json
3. Add a default value in the private constructor of WinGetAppConfig. The value must be the bahaviour of the feature if Azure App Config is down.
4. Add the new parameter in all the appconfig.*.*.json files WinGet.RestSource.Infrastructure project. It should match per env.

## How does it work in WinGet.RestSource
We setup two Azure App Config per environment, the primary and secondary. WinGetAppConfig loads it will try to load the features from the secondary first, then it will try to load from the primary (which overrites the secondary). This allows us to have georedundancy as Azure App Config doesn't support it for now. If primary fails, then we already have the secondary loaded. If secondary also fails we have the default values. By default, we refresh the feature flags every 30 seconds.

## How to enable/disable features without deployment
You can modify the feature flag in the Azure portal by going into the Azure App Config resource and going to Feature Manager. Remember to modify the primary and secondary. Depending on the env you need to do a JIT request.
