using System;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Testing.Shared;
using Elsa.Workflows.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Elsa.IntegrationTests.Scenarios.SetGetVariables;

public class Tests
{
    private readonly IWorkflowRunner _workflowRunner;
    private readonly CapturingTextWriter _capturingTextWriter = new();
    private readonly IServiceProvider _services;

    public Tests(ITestOutputHelper testOutputHelper)
    {
        _services = new TestApplicationBuilder(testOutputHelper).WithCapturingTextWriter(_capturingTextWriter).Build();
        _services.GetRequiredService<IWorkflowBuilderFactory>();
        _workflowRunner = _services.GetRequiredService<IWorkflowRunner>();
    }

    [Fact(DisplayName = "Workflow can set variable")]
    public async Task Test1()
    {
        await _services.PopulateRegistriesAsync();
        await _workflowRunner.RunAsync<SetGetVariableWorkflow>();
        var lines = _capturingTextWriter.Lines.ToList();
        Assert.Equal(new[] { "Line 5" }, lines);
    }

    [Fact(DisplayName = "Workflow can reference variables set in previous activities")]
    public async Task Test2()
    {
        await _services.PopulateRegistriesAsync();
        await _workflowRunner.RunAsync<SetGetVariablesWorkflow>();
        var lines = _capturingTextWriter.Lines.ToList();
        Assert.Equal(new[] { "Variable 2: The value of variable 1" }, lines);
    }

    [Fact(DisplayName = "Workflow can set and get named variables")]
    public async Task Test3()
    {
        await _services.PopulateRegistriesAsync();
        await _workflowRunner.RunAsync<SetGetNamedVariableWorkflow>();
        var lines = _capturingTextWriter.Lines.ToList();
        Assert.Equal(new[]
        {
            "Foo = Bar",
            "Foo = Baz"
        }, lines);
    }
}