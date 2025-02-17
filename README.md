# ELSA 3.0

![Elsa Workflows](./design/artwork/elsa-logo-art.png)

[![Elsa 3 Prerelease](https://github.com/elsa-workflows/elsa-core/actions/workflows/packages.yml/badge.svg)](https://github.com/elsa-workflows/elsa-core/actions/workflows/packages.yml)
[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Elsa)](https://www.nuget.org/packages/Elsa/)
[![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Felsa-workflows%2Felsa-3%2Fshield%2FElsa%2Flatest)](https://f.feedz.io/elsa-workflows/elsa-3/nuget/index.json)
[![npm](https://img.shields.io/npm/v/@elsa-workflows/elsa-workflows-studio)](https://www.npmjs.com/package/@elsa-workflows/elsa-workflows-studio)
[![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Felsa-workflows%2Felsa-3%2Fshield%2F%2540elsa-workflows%252Felsa-studio-wasm%2Flatest&label=npm%20preview)](https://f.feedz.io/elsa-workflows/elsa-3/packages/%40elsa-workflows%2Felsa-studio-wasm/latest/download)
[![Docker Image Version (latest semver)](https://img.shields.io/docker/v/elsaworkflows/elsa-v3?label=docker&logo=docker)](https://hub.docker.com/repository/docker/elsaworkflows/elsa-v3)
[![Discord](https://img.shields.io/discord/814605913783795763?label=discord&logo=discord)](https://discord.gg/hhChk5H472)
[![Stack Overflow questions](https://img.shields.io/badge/stackoverflow-elsa_workflows-orange.svg)]( http://stackoverflow.com/questions/tagged/elsa-workflows )
[![Subreddit subscribers](https://img.shields.io/reddit/subreddit-subscribers/elsaworkflows?style=social)](https://www.reddit.com/r/elsaworkflows/)

### [For Elsa 2 Click Here](https://github.com/elsa-workflows/elsa-core/tree/2.x)

## Introduction
Elsa is a powerful workflow library that enables workflow execution within any .NET application. Elsa allows you to define workflows in various ways, including:

- Writing C# code
- Using a visual designer
- Specifying workflows in JSON

![Elsa ships with a powerful visual designer](./design/screenshots/http-hello-world-workflow-designer.png)

### Try with Docker

To give the Elsa Studio + Elsa Server a quick spin, you can run the following command to start the Elsa Docker container:

```shell
docker pull elsaworkflows/elsa-v3:latest
docker run -t -i -e ASPNETCORE_ENVIRONMENT='Development' -e HTTP_PORTS=8080 -p 13000:8080 elsaworkflows/elsa-v3:latest
```

> This Docker image is based on a reference ASP.NET application that hosts both the workflow server and designer and is not intended for production use.

By default, you can access http://localhost:13000 and log in with:

```
  Username: admin
  Password: password
```

## Table of Contents

- [Documentation](#documentation)
- [Known Issues and Limitations](#known-issues-and-limitations)
- [Features](#features)
- [Roadmap](#roadmap)
- [Use Cases](#use-cases)

## Documentation

For comprehensive documentation and to get started with Elsa, please visit the [Elsa Documentation Website](https://v3.elsaworkflows.io/).

## Known Issues and Limitations

Elsa is continually evolving, and while it offers powerful capabilities, there are some known limitations and ongoing work:

- Documentation is still a work in progress.
- The designer is not yet fully embeddable in other applications; this feature is planned for a future release.
- C# and Python expressions are not yet fully tested.
- Bulk Dispatch Workflows is a new activity and not yet fully tested.
- Input/Output is not yet implemented in the Workflow Instance Viewer.
- Starting workflows from the designer is currently supported only for workflows that do not require input and do not start with a trigger; this is planned for a future release.
- The designer currently only supports Flowchart activities. Support for Sequence and StateMachine activities is planned for a future release.
- UI input validation is not yet implemented.

## Features

Elsa offers a wide range of features for building and executing workflows, including:

- Execution of workflows in any .NET application with support for .NET 6 and beyond.
- Support for both short-running and long-running workflows.
- A programming model loosely inspired by Windows Workflow Foundation.
- A web-based drag & drop designer with support for custom activities.
- Native support for activity composition, including activities like `Sequence`, `Flowchart`, and `ForEach`.
- Parallel execution of activities.
- Built-in activities for common scenarios, such as sending emails, making HTTP calls, scheduling tasks, sending and receiving messages, and more.
- Workflow versioning and migration via API.
- Easy integration with external applications via HTTP, message queues, and more.
- Actor model for increased workflow throughput.
- Dynamic expressions with support for C#, JavaScript, Python, and Liquid.
- Persistence agnostic, with support for Entity Framework Core, MongoDB, and Dapper out of the box.
- [Elsa Studio](https://github.com/elsa-workflows/elsa-studio): a modular Blazor dashboard app for managing and designing workflows.

## Roadmap

The following features are planned for future releases of Elsa:

- [ ] Multi-tenancy
- [ ] State Machine activity
- [ ] Designer support for Sequence activity & StateMachine activity
- [ ] BPMN 2.0 support
- [ ] DMN support
- [ ] Workflow migration to new versions via UI
- [ ] Capsules ("hot" deployable workflow packages containing activities and configuration)

## Use Cases

Elsa can be used in a variety of scenarios, including:

- Long-running workflows such as order fulfillment and product approval.
- Short-running workflows such as sending emails and generating PDFs.
- Scheduled workflows such as sending daily reports.
- Event-driven workflows such as sending welcome emails when a user signs up.

## Programmatic Workflows

Elsa allows you to define workflows in code using C#. The following example shows how to receive HTTP requests and send an email in response:

```csharp
public class SendEmailWorkflow : WorkflowBase
{
    protected override void Build(IWorkflowBuilder builder)
    {
        builder.Root = new Sequence
        {
            Activities =
            {
                new HttpEndpoint
                {
                    Path = new("/send-email"),
                    SupportedMethods = new(new[] { HttpMethods.Post }),
                    CanStartWorkflow = true
                },
                new SendEmail
                {
                    From = new("alic@acme.com"),
                    To = new(new[]{ "bob@acme.com" }),
                    Subject = new("Your workflow has been triggered!"),
                    Body = new("Hello!")
                }
            }
        };
    }
}
```

## Designed Workflows

Elsa allows you to define workflows using a visual designer. The following example shows how to receive HTTP requests and send an email in response:

![Elsa ships with a powerful visual designer](./design/screenshots/http-send-email-workflow-designer.png)