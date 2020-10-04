using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.ECR;
using Amazon.CDK.AWS.EKS;
using Amazon.CDK.AWS.IAM;

namespace KubernetesDemo
{
    public class KubernetesCluster : Construct
    {
        public Cluster Master { get; }
        public KubernetesCluster(Construct scope, string id, Vpc vpc, string clusterName = "yoda") : base(scope, id)
        {
            var clusterAdmin = SetupSecurity(scope, id);

            Master = new Cluster(this, $"{id}-eks-cluster", new ClusterProps
            {
                ClusterName = clusterName,
                Version = KubernetesVersion.V1_16,
                DefaultCapacity = 0,
                MastersRole = clusterAdmin,
                Vpc = vpc
            });
            new CfnOutput(scope, $"{id}-kube-cluster-name", new CfnOutputProps
            {
                Description = "name of cluster",
                Value = clusterName
            });
        }

        private static Role SetupSecurity(Construct scope, string id)
        {
            var clusterAdmin = new Role(scope, $"{id}-cluster-administrator", new RoleProps
            {
                RoleName = $"{id}-cluster-administrator",
                AssumedBy = new AccountRootPrincipal(),
                Description = "super admin"
            });

            new CfnOutput(scope, $"{id}-kube-role-arn", new CfnOutputProps
            {
                Description = "value of the cluster admin IAM role",
                Value = clusterAdmin.RoleArn
            });

            return clusterAdmin;
        }
    }
}