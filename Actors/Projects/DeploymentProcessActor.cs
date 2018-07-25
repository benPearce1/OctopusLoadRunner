using System;
using Akka.Actor;
using Octopus.Client;
using Octopus.Client.Editors.Async;
using Octopus.Client.Model;
using Octopus.Client.Model.DeploymentProcess;
using OctopusLoadRunner.Messages;

namespace OctopusLoadRunner.Actors
{
    public class DeploymentProcessActor : ReceiveActor
    {
        public DeploymentProcessActor()
        {
            Receive<CreateDeploymentProcess>(async msg =>
            {
                var endpoint = new OctopusServerEndpoint(msg.Url, msg.ApiKey);
                using (var client = await OctopusAsyncClient.Create(endpoint))
                {
                    DeploymentProcessEditor editor = new DeploymentProcessEditor(client.Repository.DeploymentProcesses);
                    await editor.Load(msg.DeploymentProcessId);
                    var addOrUpdateScriptAction = editor.AddOrUpdateStep("Script").AddOrUpdateScriptAction("Script",
                        ScriptAction.InlineScript(ScriptSyntax.PowerShell, "Write-Host \"hello\""), ScriptTarget.Server);
                    await editor.Save();
                    Console.WriteLine($"Created process for {msg.ProjectId}");
                }
            });
        }
    }
}
