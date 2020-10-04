﻿using Amazon.CDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KubernetesDemo
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new KubernetesDemoStack(app, "KubernetesDemoStack");
            app.Synth();
        }
    }
}
