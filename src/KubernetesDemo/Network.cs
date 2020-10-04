#nullable enable
using Amazon.CDK;
using Amazon.CDK.AWS.EC2;

namespace KubernetesDemo
{
    // networks = [
    // "10.0.0.0/19",
    // "10.0.32.0/19",
    // "10.0.64.0/19",
    // "10.0.96.0/19",
    // "10.0.128.0/22",
    // "10.0.132.0/22",
    // "10.0.136.0/22",
    // "10.0.140.0/22",
    // ]

    public class Network : Construct
    {
        public Vpc Vpc { get; }

        public Network(Construct scope, string id) : base(scope, id)
        {
            ISubnetConfiguration[]? subnetConfiguration = {
                new SubnetConfiguration {
                    SubnetType = SubnetType.PUBLIC,
                    Name = "public",
                    CidrMask = 26
                },
                new SubnetConfiguration {
                    SubnetType = SubnetType.PRIVATE,
                    Name = "private",
                    CidrMask = 20,
                },
                //could leave room for more networks 
            };

            Vpc = new Vpc(scope, $"galaxy", new VpcProps
            {
                Cidr = "10.0.0.0/16",
                MaxAzs = 4,
                SubnetConfiguration = subnetConfiguration
            });
        }
    }
}