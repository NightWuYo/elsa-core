using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Contracts;
using Elsa.Workflows.Runtime.Activities;

namespace Elsa.IntegrationTests.Scenarios.WorkflowCancellation.Workflows;

public class SimpleSuspendedWorkflow : WorkflowBase
{
    protected override void Build(IWorkflowBuilder builder)
    {
        builder.Root = new Sequence
        {
            Activities =
            {
                new Start(),
                new Event("BlockingEvent"),
                new WriteLine("Workflow was not properly blocked")
            },
        };
    }
}