# How to create Dynamic Load Orleans Grain

1. Create **.NET Standard 2.0** class library project for both Grain RPC interface & RPC implementation projects.
2. Add Correct Nuget to those projects:
   1. [GranDen.Orleans.Server.SharedInterface](https://www.nuget.org/packages/GranDen.Orleans.Server.SharedInterface) nuget to RPC implementation project.
   2. [Microsoft.Orleans.Core.Abstractions](https://www.nuget.org/packages/Microsoft.Orleans.Core.Abstractions) nuget to RPC interface project.
   3. [Microsoft.Orleans.CodeGenerator.MSBuild](https://www.nuget.org/packages/Microsoft.Orleans.CodeGenerator.MSBuild) nuget to both RPC interface & RPC implementation projects.
3. In RPC implementation project, add a class that inherit the `AbstractServiceConfigDelegate<MyGrai>`, the `MyGrain` is the C# Orleans Grain class that needs to be dynamic loaded; And this class has two porperties that is for:
   - **`Action<IApplicationPartManager> AppPartConfigurationAction`**:  
      This configure [Orleans Application Part](https://dotnet.github.io/orleans/Documentation/clusters_and_clients/configuration_guide/server_configuration.html#application-parts), there has already a basic implementation in parent `AbstractServiceConfigDelegate<T>` so it is usally doesn't have to override by yourself.
   - **`Action<HostBuilderContext, IServiceCollection> ServiceConfigurationAction`**:  
      This property controls the Default Dependecy Injection configuration that Orleans Grain class use, you can left a defualt empty  
      `public override Action<HostBuilderContext, IServiceCollection> ServiceConfigurationAction { get; }`  
      statment if you doesn't have any Dependecy Injection usage in Grain class.
4. Add an attribute `[assembly: KnownAssembly(typeof(IMyGrain))]` on top of the class that previously created, the `IMyGrain` is the Orleans Grain RPC interface that is declared in RCP interface project.  
   Follwing is an example code of the above 4 & 5 steps:  

   ```cs
   using Orleans.CodeGeneration;
   
   [assembly: KnownAssembly(typeof(MyReminder.ShareInterface.IMyReminder))]
   
   namespace MyReminder.Grain
   {
      // ReSharper disable once UnusedMember.Global
      public class MyReminderGrainServiceConfigure : AbstractServiceConfigDelegate<MyReminderGrain>
      {
        public override Action<HostBuilderContext, IServiceCollection> ServiceConfigurationAction =>
            (ctx, service) =>
            {
                service.AddTransient<IOutputMsg, OutputMsg>();
            };
      }
   }
   ```
   
5. Add `<CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>` inside the RPC implementation project file(***.csproj**)'s `<PropertyGroup>`.  
