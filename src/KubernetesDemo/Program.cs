using System;
using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.EC2;

namespace KubernetesDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new App();

            CreateDemoStack(app, DeploymentEnvironment.Sandbox);
            CreateDemoStack(app, DeploymentEnvironment.Dev);
            CreateDemoStack(app, DeploymentEnvironment.Stage);
            CreateDemoStack(app, DeploymentEnvironment.Prod);

            app.Synth();
        }
        private static void CreateDemoStack(App app, DeploymentEnvironment env)
        {
            var stackName = $"{env.ToString().ToLower()}-demo";
            var kube = new DemoStack(app, stackName, env,
                new StackProps
                {
                    StackName = stackName,
                    Env = new Amazon.CDK.Environment
                    {
                        //TODO: get this from CircleCI context
                        Region = "us-west-2",
                        Account = "994532171183"
                    },
                    Tags = new Dictionary<string, string>
                    {
                        {"purpose", "to feed people"},
                        {"cost-code", "1"},
                        //TODO: only for dev!!!
                        {"k8s.io/cluster-autoscaler/enabled", "enabled"},
                        {"k8s.io/cluster-autoscaler/yoda", "enabled"}
                    }
                });

        }
    }
}
