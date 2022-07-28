# Azure Storage Todos
After cloning this repository. Open it in Visual Studio Code and when prompted say "No" to re-open in container at first. 

In order to configure your dev container to connect to our Azure App Configuration go to the .devcontainer folder. Make a copy of devcontainer.env.example and name it devcontainer.env. Copy the connection string from the link provided there to properly configure your APPCONFIG_CONNECTION_STRING environment variable.

Once you have a valid connection string do a CTRL+SHIFT+P to bring up the command pallet and select the option to re-open in container from there.

Once your container is built and Visual Studio Code is running from the container do a CTRL+SHIFT+` to bring up a command prompt from the container.

Execute the following commands (these commands will likely need to be done any time you rebuild the dev container):

```bash
dotnet restore
dotnet dev-certs https --trust
az login
```

After you have successfully logged in to the Azure CLI in your dev container you will be able to download the App Configuration and User Secrets for Development.