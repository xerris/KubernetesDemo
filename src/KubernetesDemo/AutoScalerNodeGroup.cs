using System;
using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.EKS;
using Amazon.CDK.AWS.IAM;

namespace KubernetesDemo
{
    /// <summary>
    /// Abstraction to work with EKS Autoscaler (e.g. one node group per availability zone)
    /// Is autoscalled by the Kubernetes Autoscaler - <see cref="ClusterAutoscaler"/>
    /// </summary>
    public class AutoScalerNodeGroup : AbstractNodeGroup
    {
        public AutoScalerNodeGroup(Construct scope, string id, string nodeGroupName, Cluster cluster,
            string instanceType = "c5d.large") : base(scope, $"{id}-{nodeGroupName}")
        {
            for (var index = 0; index < cluster.Vpc.PrivateSubnets.Length; index++)
            {
                var subnet = cluster.Vpc.PrivateSubnets[index];
                CreateNodeGroup($"{id}-{nodeGroupName}", cluster, nodeGroupName, instanceType, subnet, (AZ)index, 1);
            }
        }
        private static void CreateNodeGroup(string id, Cluster cluster, string nodeGroupName, string instanceType,
            ISubnet subnet, AZ az, int actualMinSize = 1, int actualMaxSize = 5)
        {
            var nodegroup = cluster.AddNodegroupCapacity($"{id}-{az}", new NodegroupProps()
            {
                NodegroupName = $"{cluster.ClusterName}-{nodeGroupName}-{az}",
                InstanceType = new InstanceType(instanceType),
                MinSize = actualMinSize,
                MaxSize = actualMaxSize,
                Subnets = new SubnetSelection { Subnets = new[] { subnet } },
                Tags = new Dictionary<string, string>
                {
                    {"k8s.io/cluster-autoscaler/enabled", ""},
                    {"k8s.io/cluster-autoscaler/yoda", ""}
                }
            });
            nodegroup.Role.AddManagedPolicy(ManagedPolicy.FromAwsManagedPolicyName("CloudWatchAgentServerPolicy"));
            nodegroup.Role.AddToPrincipalPolicy(autoScalePolicy);
        }
        private static bool IsDev(Cluster cluster, DeploymentEnvironment environment)
        {
            return (environment == DeploymentEnvironment.Dev || environment == DeploymentEnvironment.Sandbox)
                   && cluster.Vpc.PrivateSubnets.Length >= 2;
        }

        private enum AZ
        {
            a, b, c, d, e, f, g, h, i, j, k
        }
    }
}