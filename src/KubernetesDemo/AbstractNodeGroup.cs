using Amazon.CDK;
using Amazon.CDK.AWS.IAM;

namespace KubernetesDemo
{
    abstract public class AbstractNodeGroup : Construct
    {
        protected static readonly PolicyStatement autoScalePolicy = new PolicyStatement(new PolicyStatementProps()
        {
            Effect = Effect.ALLOW,
            Actions = new string[]
            {
                "autoscaling:DescribeAutoScalingGroups",
                "autoscaling:DescribeAutoScalingInstances",
                "autoscaling:DescribeLaunchConfigurations",
                "autoscaling:DescribeTags",
                "autoscaling:SetDesiredCapacity",
                "autoscaling:TerminateInstanceInAutoScalingGroup"
            },
            Resources = new[]
            {
                "*"
            }
        });
        protected AbstractNodeGroup(Construct scope, string id) : base(scope, id)
        {
        }
    }
}