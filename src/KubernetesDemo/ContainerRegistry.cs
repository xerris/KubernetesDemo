using Amazon.CDK;
using Amazon.CDK.AWS.ECR;

namespace KubernetesDemo
{
    /// <summary>
    /// Standard ECR to be configured for fluxCD to pull from
    /// </summary>
    public class ContainerRegistry : Construct
    {
        public Repository Registry { get; set; }
        public ContainerRegistry(Construct scope, string id, DeploymentEnvironment env) : base(scope, id)
        {
            var repositoryName = $"demo-{env.ToString().ToLower()}";
            Registry = new Repository(this, repositoryName, new RepositoryProps
            {
                RepositoryName = repositoryName,
                RemovalPolicy = RemovalPolicy.DESTROY
            });
            //TODO: need a rule to kill all untagged images...need tags!!!
            // ecr.AddLifecycleRule(new LifecycleRule
            // {
            //     RulePriority = 1,
            //     MaxImageCount = 1,
            //     Description = "all images must be tagged, or they will be deleted",
            //     TagStatus = TagStatus.UNTAGGED
            // });

            //TODO: Rule so only valid tag names are used
            // eg. <service>-master-gitHash
            // hello-master-38fde3d
            // ecr.AddLifecycleRule(new LifecycleRule
            // {
            //     RulePriority = 1,
            //     Description = "keep it clean, only 10 images of a tag are allowed",
            //     MaxImageCount = 10,
            //     TagPrefixList = new [] {"prod-*", "master-*"},
            //     TagStatus = TagStatus.TAGGED
            // });
        }

    }
}