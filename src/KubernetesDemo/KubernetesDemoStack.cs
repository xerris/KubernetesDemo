using Amazon.CDK;
using Amazon.CDK.AWS.EC2;

namespace KubernetesDemo
{
    public class DemoStack : Amazon.CDK.Stack
    {
        public readonly Vpc Vpc;

        internal DemoStack(Construct scope, string id, DeploymentEnvironment env, IStackProps props = null) :
            base(scope, id, props)
        {
            //network
            Vpc = new Network(this, "{id}-supernetwork").Vpc;

            //Kubernetes 
            var ecr = new ContainerRegistry(this, $"{id}-container-registry", env);
            var k8 = new KubernetesCluster(this, $"{id}-cluster", Vpc, "democluster");

            //if (env == DeploymentEnvironment.Dev || env == DeploymentEnvironment.Sandbox)
            //{
                var sandboxCluster = new SpotFleetWorkerGroup(this, id, k8.Master);
             
            //}
            //else
            //{
            //    // WARNING: Changing Node Groups is destructive...
            //    // 
            //    // Process is to add a new node group to production then delete  the old cluster group
            //    // e.g. api-cluster-v1, api-cluster-v2, etc
            //    // Kubernetes Compute (e.g. EKS node groups)
            //    var apiCluster1 = new AutoScalerNodeGroup(this, id, "api-cluster-v1", k8.Master);
            //    apiCluster1.Node.AddDependency(k8);

            //    var productionDatabase = new Database(this, id, Vpc, new InstanceType("t3.medium"), Duration.Days(30));
            //}
        }
    }
}