using System;
using System.Globalization;
using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.EKS;

namespace KubernetesDemo
{
    public class SpotFleetWorkerGroup : AbstractNodeGroup
    {
        public SpotFleetWorkerGroup(Construct scope, string id, Cluster cluster) : base(scope, id)
        {
            //m5d.large
            const string instanceType = "m5d.large";
            var listPrice = new decimal(0.113);
            var discount = new decimal(0.70);


            //Get current price listed at https://www.ec2instances.info/?selected=t3.large

            var spotPrice = listPrice - listPrice * discount;
            var finalSpotPrice = spotPrice.ToString(CultureInfo.InvariantCulture);
      

            var spotFleet = cluster.AddAutoScalingGroupCapacity($"{id}-spot", new AutoScalingGroupCapacityOptions
            {
                SpotPrice = finalSpotPrice,
                InstanceType = new InstanceType(instanceType),
                MaxCapacity = 6,
                MinCapacity = 2,
            });
            //TODO: set user data commands here
            //spotFleet.AddUserData("install  falco");
            spotFleet.Role.AddToPrincipalPolicy(autoScalePolicy);
        }
    }
}