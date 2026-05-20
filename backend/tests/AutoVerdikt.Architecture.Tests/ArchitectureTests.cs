using System.Reflection;
using NetArchTest.Rules;
using Xunit;
using Shouldly;

namespace AutoVerdikt.Architecture.Tests;

public class ArchitectureTests
{
    private const string DomainNamespace = "AutoVerdikt.Domain";
    private const string ApplicationNamespace = "AutoVerdikt.Application";
    private const string InfrastructureNamespace = "AutoVerdikt.Infrastructure";
    private const string StoreNamespace = "AutoVerdikt.Store";
    private const string WebApiNamespace = "AutoVerdikt.WebApi";
    private const string AgentsNamespace = "AutoVerdikt.Agents";

    [Fact]
    public void Domain_Should_Not_HaveDependencyOn_AnyOtherProject()
    {
        var domainAssembly = Assembly.Load(DomainNamespace);

        var result = Types
            .InAssembly(domainAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                ApplicationNamespace, InfrastructureNamespace,
                StoreNamespace, WebApiNamespace, AgentsNamespace)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Application_Should_Not_HaveDependencyOn_OtherProjects()
    {
        var applicationAssembly = Assembly.Load(ApplicationNamespace);

        var result = Types
            .InAssembly(applicationAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(InfrastructureNamespace, StoreNamespace, WebApiNamespace, AgentsNamespace)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOn_Store_WebApi_Or_Agents()
    {
        var infrastructureAssembly = Assembly.Load(InfrastructureNamespace);

        var result = Types
            .InAssembly(infrastructureAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(StoreNamespace, WebApiNamespace, AgentsNamespace)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Store_Should_Not_HaveDependencyOn_Infrastructure_WebApi_Or_Agents()
    {
        var storeAssembly = Assembly.Load(StoreNamespace);

        var result = Types
            .InAssembly(storeAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(InfrastructureNamespace, WebApiNamespace, AgentsNamespace)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Agents_Should_Not_HaveDependencyOn_Infrastructure_Store_Or_WebApi()
    {
        var agentsAssembly = Assembly.Load(AgentsNamespace);

        var result = Types
            .InAssembly(agentsAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(InfrastructureNamespace, StoreNamespace, WebApiNamespace)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void WebApi_Should_Not_HaveDependencyOn_Agents()
    {
        // WebApi is the composition root and may wire up Store/Infrastructure for DI.
        // The only hard constraint is that it never depends on Agents.
        var webApiAssembly = Assembly.Load(WebApiNamespace);

        var result = Types
            .InAssembly(webApiAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(AgentsNamespace)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }
}
