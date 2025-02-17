using System;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Extensions;
using Elsa.Testing.Shared;
using Elsa.Workflows.Contracts;
using Elsa.Workflows.Options;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Elsa.IntegrationTests.Activities;

public class ForkTests
{
    private readonly IWorkflowRunner _workflowRunner;
    private readonly CapturingTextWriter _capturingTextWriter = new();
    private readonly IWorkflowBuilderFactory _workflowBuilderFactory;
    private IServiceProvider _services;

    public ForkTests(ITestOutputHelper testOutputHelper)
    {
        _services = new TestApplicationBuilder(testOutputHelper).WithCapturingTextWriter(_capturingTextWriter).Build();
        _workflowBuilderFactory = _services.GetRequiredService<IWorkflowBuilderFactory>();
        _workflowRunner = _services.GetRequiredService<IWorkflowRunner>();
    }

    [Fact(DisplayName = "Each branch executes")]
    public async Task Test1()
    {
        await _services.PopulateRegistriesAsync();
        var workflow = await _workflowBuilderFactory.CreateBuilder().BuildWorkflowAsync<BasicForkWorkflow>();
        await _workflowRunner.RunAsync(workflow);
        var lines = _capturingTextWriter.Lines.ToList();
        Assert.Equal(new[] { "Branch 1", "Branch 2", "Branch 3" }, lines);
    }

    [Fact(DisplayName = "Wait AnyAsync causes workflow to continue")]
    public async Task Test2()
    {
        await _services.PopulateRegistriesAsync();
        var workflow = await _workflowBuilderFactory.CreateBuilder().BuildWorkflowAsync<JoinAnyForkWorkflow>();

        // First run.
        var result = await _workflowRunner.RunAsync(workflow);

        // Collect one of the bookmarks to resume the workflow.
        var bookmark = result.WorkflowState.Bookmarks.FirstOrDefault(x => x.ActivityId == "Event2");
        Assert.NotNull(bookmark);

        // Resume the workflow.
        var runOptions = new RunWorkflowOptions { BookmarkId = bookmark.Id };
        await _workflowRunner.RunAsync(workflow, result.WorkflowState, runOptions);

        // Verify output.
        var lines = _capturingTextWriter.Lines.ToList();
        Assert.Equal(new[] { "Start", "Branch 2", "End" }, lines);
    }
}